using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Data;
using Base.Data.Accounts;
using Base.Data.Balances;
using Base.Data.Pairs;
using Base.Data.Transactions;
using Base.Keys;
using Base.Storage;
using CustomTools.Extensions.Core;
using CustomTools.Extensions.Core.Action;
using CustomTools.Extensions.Core.Array;
using Promises;
using Tools.Json;


public sealed class AuthorizationContainer
{
    public enum AuthorizationResult
    {
        Ok,             // authorized
        Failed,         // username password pair is't correct
        UserNotFound,   // user nor found
        Error           // some error
    }


    public sealed class AuthorizationData
    {
        public UserNameFullAccountDataPair UserNameData { get; private set; }

        public AuthorizationData(UserNameFullAccountDataPair userNameData)
        {
            UserNameData = userNameData;
        }

        public void UpdateAccountData(IdObject idObject)
        {
            if (idObject.Id.Equals(UserNameData.Value.Account.Id))
            {
                UserNameData.Value.Account = (AccountObject)idObject;
            }
            else
            if (idObject.Id.Equals(UserNameData.Value.Statistics.Id))
            {
                UserNameData.Value.Statistics = (AccountStatisticsObject)idObject;
            }
            else
            if (idObject.Id.SpaceType.Equals(SpaceType.AccountBalance))
            {
                for (var i = 0; i < UserNameData.Value.Balances.OrEmpty().Length; i++)
                {
                    if (idObject.Id.Equals(UserNameData.Value.Balances[i].Id))
                    {
                        UserNameData.Value.Balances[i] = (AccountBalanceObject)idObject;
                        break;
                    }
                }
            }
        }

        public async Task<bool> CheckAuthorizationAsync(string password)
        {
            var keys = Keys.FromSeed(UserNameData.Key, password);
            var result = await keys.CheckAuthorizationAsync(UserNameData.Value.Account);
            keys.Dispose();
            return result != null;
        }

        public bool CheckAuthorizationSync(string password)
        {
            var keys = Keys.FromSeed(UserNameData.Key, password);
            var result = keys.CheckAuthorizationSync(UserNameData.Value.Account);
            keys.Dispose();
            return result != null;
        }
    }


    public static event Action<AuthorizationData> OnAuthorizationChanged;

    private AuthorizationData authorization;


    public AuthorizationData Current
    {
        get { return authorization; }
        private set
        {
            if (authorization != value)
            {
                authorization = value;
                OnAuthorizationChanged.SafeInvoke(authorization);
            }
        }
    }

    private IPromise<AuthorizationResult> AuthorizationBy(UserNameFullAccountDataPair dataPair, string password)
    {
        return new Promise<AuthorizationResult>(async (resolve, reject) =>
        {
            try
            {
                var authData = new AuthorizationData(dataPair);
                if (await authData.CheckAuthorizationAsync(password))
                {
                    if (!Current.IsNull())
                    {
                        Repository.OnGetObject -= Current.UpdateAccountData;
                    }
                    Current = authData;
                    Repository.OnGetObject += Current.UpdateAccountData;
                    resolve(AuthorizationResult.Ok);
                }
                else
                {
                    resolve(AuthorizationResult.Failed);
                }
            }
            catch
            {
                resolve(AuthorizationResult.Error);
            }
        });
    }

    private IPromise<AuthorizationResult> AuthorizationBy(uint id, string password)
    {
        return EchoApiManager.Instance.Database.GetFullAccount(SpaceTypeId.ToString(SpaceType.Account, id), true).Then(result =>
        {
            if (result == null)
            {
                return Promise<AuthorizationResult>.Resolved(AuthorizationResult.UserNotFound);
            }
            return AuthorizationBy(result, password);
        });
    }

    public IPromise<AuthorizationResult> AuthorizationBy(string userName, string password)
    {
        return EchoApiManager.Instance.Database.GetFullAccount(userName.Trim(), true).Then(result =>
        {
            if (result == null)
            {
                return Promise<AuthorizationResult>.Resolved(AuthorizationResult.UserNotFound);
            }
            return AuthorizationBy(result, password);
        });
    }

    public void ResetAuthorization()
    {
        if (!Current.IsNull())
        {
            Repository.OnGetObject -= Current.UpdateAccountData;
        }
        Current = null;
    }

    public IPromise ProcessTransaction(TransactionBuilder builder, string password, SpaceTypeId asset = null, Action<TransactionConfirmationData> resultCallback = null)
    {
        if (!IsAuthorized)
        {
            return Promise.Rejected(new InvalidOperationException("Isn't Authorized!"));
        }
        return new Promise(async (resolve, reject) =>
        {
            var keys = Keys.FromSeed(Current.UserNameData.Key, password);

            void Resolve()
            {
                keys.Dispose();
                resolve();
            };

            void Reject(Exception ex)
            {
                keys.Dispose();
                reject(ex);
            };

            try
            {
                var validKeys = await keys.CheckAuthorizationAsync(Current.UserNameData.Value.Account);
                if (!validKeys.IsNull())
                {
                    var existPublicKeys = validKeys.PublicKeys;
                    TransactionBuilder.SetRequiredFees(builder, asset).Then(b => b.GetPotentialSignatures().Then(potentialPublicKeys =>
                    {
                        var availableKeys = new List<IPublicKey>();
                        foreach (var existPublicKey in existPublicKeys)
                        {
                            if (!availableKeys.Contains(existPublicKey) && Array.IndexOf(potentialPublicKeys, existPublicKey) != -1)
                            {
                                availableKeys.Add(existPublicKey);
                            }
                        }
                        if (availableKeys.IsNullOrEmpty())
                        {
                            Reject(new InvalidOperationException("Available key doesn't find!"));
                        }
                        return b.GetRequiredSignatures(availableKeys.ToArray()).Then(requiredPublicKeys =>
                        {
                            if (requiredPublicKeys.IsNullOrEmpty())
                            {
                                Reject(new InvalidOperationException("Required key doesn't find!"));
                            }
                            if (!IsAuthorized)
                            {
                                Reject(new InvalidOperationException("Isn't Authorized!"));
                            }
                            var selectedPublicKey = requiredPublicKeys.First(); // select key
                            b.AddSigner(new KeyPair(validKeys[selectedPublicKey])).Broadcast(resultCallback).Then(Resolve).Catch(Reject);
                        }).Catch(Reject);
                    }).Catch(Reject)).Catch(Reject);
                }
                else
                {
                    Reject(new InvalidOperationException("Isn't Authorized!"));
                }
            }
            catch (Exception ex)
            {
                Reject(ex);
            }
        });
    }

    public bool IsAuthorized => !Current.IsNull();

    public UserNameFullAccountDataPair UserData => IsAuthorized ? Current.UserNameData : null;
}