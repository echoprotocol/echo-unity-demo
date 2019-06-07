using System;
using System.Collections.Generic;
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
        Ok,
        Failed,
        UserNotFound,
        Error
    }


    public sealed class AuthorizationData
    {
        public Keys Keys { get; private set; }
        public UserNameFullAccountDataPair UserNameData { get; private set; }

        public AuthorizationData(Keys keys, UserNameFullAccountDataPair userNameData)
        {
            Keys = keys;
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
                var validKeys = await Keys.FromSeed(dataPair.Key, password).CheckAuthorizationAsync(dataPair.Value.Account);
                if (!validKeys.IsNull())
                {
                    if (!Current.IsNull())
                    {
                        Repository.OnGetObject -= Current.UpdateAccountData;
                    }
                    Current = new AuthorizationData(validKeys, dataPair);
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

    public IPromise ProcessTransaction(TransactionBuilder builder, SpaceTypeId asset = null, Action<TransactionConfirmationData> resultCallback = null)
    {
        if (!IsAuthorized)
        {
            return Promise.Rejected(new InvalidOperationException("Isn't Authorized!"));
        }
        var existPublicKeys = Current.Keys.PublicKeys;
        return new Promise((resolve, reject) => TransactionBuilder.SetRequiredFees(builder, asset).Then(b => b.GetPotentialSignatures().Then(potentialPublicKeys =>
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
                reject(new InvalidOperationException("Available key doesn't find!"));
            }
            return b.GetRequiredSignatures(availableKeys.ToArray()).Then(requiredPublicKeys =>
            {
                if (requiredPublicKeys.IsNullOrEmpty())
                {
                    reject(new InvalidOperationException("Required key doesn't find!"));
                }
                if (!IsAuthorized)
                {
                    reject(new InvalidOperationException("Isn't Authorized!"));
                }
                var selectedPublicKey = requiredPublicKeys.First(); // select key
                b.AddSigner(new KeyPair(Current.Keys[selectedPublicKey])).Broadcast(resultCallback).Then(resolve).Catch(reject);
            }).Catch(reject);
        }).Catch(reject)).Catch(reject));
    }

    public bool IsAuthorized => !Current.IsNull();

    public UserNameFullAccountDataPair UserData => IsAuthorized ? Current.UserNameData : null;
}