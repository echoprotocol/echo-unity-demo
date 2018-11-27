﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Base.Data;
using Base.Data.Accounts;
using Base.Data.Pairs;
using Base.Data.Transactions;
using Base.ECC;
using Base.Storage;
using CustomTools.Extensions.Core;
using CustomTools.Extensions.Core.Action;
using CustomTools.Extensions.Core.Array;
using Newtonsoft.Json.Linq;
using Promises;
using Tools.Json;


public sealed class AuthorizationContainer
{
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
            if (idObject.Id.Equals(UserNameData.FullAccount.Account.Id))
            {
                UserNameData.FullAccount.Account = (AccountObject)idObject;
            }
            else
            if (idObject.Id.Equals(UserNameData.FullAccount.Statistics.Id))
            {
                UserNameData.FullAccount.Statistics = (AccountStatisticsObject)idObject;
            }
            else
            if (idObject.Id.SpaceType.Equals(SpaceType.AccountBalance))
            {
                for (var i = 0; i < UserNameData.FullAccount.Balances.OrEmpty().Length; i++)
                {
                    if (idObject.Id.Equals(UserNameData.FullAccount.Balances[i].Id))
                    {
                        UserNameData.FullAccount.Balances[i] = (AccountBalanceObject)idObject;
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

    public IPromise<bool> Registration(string userName, string password)
    {
        return EchoApiManager.Instance.Database.GetFullAccount(userName.Trim(), true).Then(result =>
        {
            if (result.IsNull())
            {
                if (!NodeManager.IsInstanceExist || NodeManager.Instance.RegistrationUrl.IsNullOrEmpty())
                {
                    return Promise<bool>.Rejected(new InvalidOperationException("Registration url incorrect."));
                }
                return new Promise<bool>((resolve, reject) =>
                {
                    new Task(() =>
                    {
                        try
                        {
                            var keys = Keys.FromSeed(userName, password, false);
                            var request = (HttpWebRequest)WebRequest.Create(NodeManager.Instance.RegistrationUrl);
                            request.ContentType = "application/json";
                            request.Method = "POST";
                            using (var writer = new StreamWriter(request.GetRequestStream()))
                            {
                                writer.Write(new JsonBuilder(new JsonDictionary {
                                    { "name",          userName },
                                    { "owner_key",     keys[AccountRole.Owner].ToString() },
                                    { "active_key",    keys[AccountRole.Active].ToString() },
                                    { "memo_key",      keys[AccountRole.Memo].ToString() }
                                }).Build());
                                writer.Flush();
                                writer.Close();
                            }
                            var jsonResponse = string.Empty;
                            using (var reader = new StreamReader((request.GetResponse() as HttpWebResponse).GetResponseStream()))
                            {
                                jsonResponse = reader.ReadToEnd();
                                reader.Close();
                            }
                            if (jsonResponse.IsNullOrEmpty())
                            {
                                throw new InvalidOperationException();
                            }
                            var confirmation = JToken.Parse(jsonResponse).First.ToObject<TransactionConfirmation>();
                            var account = confirmation.Transaction.OperationResults.First().Value as SpaceTypeId;
                            (account.SpaceType.Equals(SpaceType.Account) ? AuthorizationBy(account.Id, password) : Promise<bool>.Resolved(false)).Then(resolve).Catch(reject);
                        }
                        catch (Exception ex)
                        {
                            reject(ex);
                        }
                    }).Start();
                });
            }
            return AuthorizationBy(result, password);
        });
    }

    private IPromise<bool> AuthorizationBy(uint id, string password)
    {
        return EchoApiManager.Instance.Database.GetFullAccount(SpaceTypeId.ToString(SpaceType.Account, id), true).Then(result =>
        {
            return AuthorizationBy(result, password);
        });
    }

    private IPromise<bool> AuthorizationBy(UserNameFullAccountDataPair dataPair, string password)
    {
        return new Promise<bool>((resolve, reject) =>
        {
            new Task(() =>
            {
                try
                {
                    var validKeys = Keys.FromSeed(dataPair.UserName, password, false).CheckAuthorization(dataPair.FullAccount.Account);
                    if (!validKeys.IsNull())
                    {
                        if (!Current.IsNull())
                        {
                            Repository.OnGetObject -= Current.UpdateAccountData;
                        }
                        Current = new AuthorizationData(validKeys, dataPair);
                        Repository.OnGetObject += Current.UpdateAccountData;
                        resolve(true);
                    }
                    else
                    {
                        resolve(false);
                    }
                }
                catch (Exception ex)
                {
                    reject(ex);
                }
            }).Start();
        });
    }

    public IPromise<bool> AuthorizationBy(string userName, string password)
    {
        return EchoApiManager.Instance.Database.GetFullAccount(userName.Trim(), true).Then(result =>
        {
            return AuthorizationBy(result, password);
        });
    }

    public void AuthorizationBy(string userName, string password, Action<bool> onSuccess, Action<Exception> onFailed)
    {
        AuthorizationBy(userName, password).Then(onSuccess).Catch(onFailed);
    }

    public void ResetAuthorization()
    {
        if (!Current.IsNull())
        {
            Repository.OnGetObject -= Current.UpdateAccountData;
        }
        Current = null;

    }

    public IPromise ProcessTransaction(TransactionBuilder builder, SpaceTypeId asset = null, Action<TransactionConfirmation> resultCallback = null)
    {
        if (!IsAuthorized)
        {
            return Promise.Rejected(new InvalidOperationException("Isn't Authorized!"));
        }
        var existPublicKeys = Current.Keys.PublicKeys;
        return new Promise((resolve, reject) => TransactionBuilder.SetRequiredFees(builder, asset).Then(b => b.GetPotentialSignatures().Then(signatures =>
        {
            var availableKeys = new List<PublicKey>();
            foreach (var existPublicKey in existPublicKeys)
            {
                if (!availableKeys.Contains(existPublicKey) && Array.IndexOf(signatures.PublicKeys, existPublicKey) != -1)
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