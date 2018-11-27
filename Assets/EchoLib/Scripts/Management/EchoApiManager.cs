using System;
using System.Collections.Generic;
using Base;
using Base.Api;
using Base.Api.Database;
using Base.Config;
using Base.Data;
using Base.Data.Operations;
using Base.Data.Transactions;
using Base.Requests;
using Base.Responses;
using Base.Storage;
using CustomTools.Extensions.Core;
using CustomTools.Extensions.Core.Action;
using CustomTools.Extensions.Core.Array;
using Newtonsoft.Json.Linq;
using Promises;
using Tools.HexBinDec;
using WebSocketSharp;


public sealed class EchoApiManager : CustomTools.Singleton.SingletonMonoBehaviour<EchoApiManager>, ISender
{
    public static event Action<string> OnConnectionOpened;
    public static event Action<string> OnConnectionClosed;

    public static event Action OnAllApiInitialized;
    public static event Action<DatabaseApi> OnDatabaseApiInitialized;
    public static event Action<NetworkBroadcastApi> OnNetworkBroadcastApiInitialized;
    public static event Action<HistoryApi> OnHistoryApiInitialized;
    public static event Action<CryptoApi> OnCryptoApiInitialized;

    private static string chainId = string.Empty;
    private static RequestIdentificator identificators;

    private readonly static List<Request> requestBuffer = new List<Request>();

    [UnityEngine.SerializeField]
    private bool sendByUpdate = false;

    private DatabaseApi database;
    private NetworkBroadcastApi networkBroadcast;
    private HistoryApi history;
    private CryptoApi crypto;
    private AuthorizationContainer authorizationContainer;


    public static string ChainId => chainId.OrEmpty();

    public static bool CanDoRequest => IsInstanceExist && ConnectionManager.IsConnected;

    public DatabaseApi Database => database ?? (database = DatabaseApi.Create(this));

    public NetworkBroadcastApi NetworkBroadcast => networkBroadcast ?? (networkBroadcast = NetworkBroadcastApi.Create(this));

    public HistoryApi History => history ?? (history = HistoryApi.Create(this));

    public CryptoApi Crypto => crypto ?? (crypto = CryptoApi.Create(this));

    public AuthorizationContainer Authorization => authorizationContainer ?? (authorizationContainer = new AuthorizationContainer());


    #region UnityCallbacks
    protected override void Awake()
    {
        identificators = new RequestIdentificator(0);
        ConnectionManager.OnConnectionChanged += InitRegularCallbacks;
        base.Awake();
    }

    private void Update()
    {
        UpConnection();
        if (sendByUpdate && CanSend)
        {
            ConnectionManager.DoAll(requestBuffer);
        }
    }

    private void UpConnection()
    {
        if (ConnectionManager.ReadyState.Equals(WebSocketState.Closed) || ConnectionManager.ReadyState.Equals(WebSocketState.Closing))
        {
            ConnectionManager.Instance.InitConnect();
        }
    }

    private bool CanSend => ConnectionManager.ReadyState.Equals(WebSocketState.Open);

    protected override void OnDestroy()
    {
        ConnectionManager.OnConnectionChanged -= InitRegularCallbacks;
        base.OnDestroy();
    }

    private void OnApplicationQuit() => requestBuffer.Clear();
    #endregion


    #region Initialization
    private void ConnectionOpened(Response response)
    {
        CustomTools.Console.DebugLog("EchoApiManager class", CustomTools.Console.LogMagentaColor("Regular Callback:"), "ConnectionOpened()");
        InitializeApi(LoginApi.Create(this));
        response.SendResultData<string>(url => OnConnectionOpened.SafeInvoke(url));
    }

    private void ConnectionClosed(Response response)
    {
        CustomTools.Console.DebugLog("EchoApiManager class", CustomTools.Console.LogMagentaColor("Regular Callback:"), "ConnectionClosed()");
        ResetApi();
        response.SendResultData<string>(reason => OnConnectionClosed.SafeInvoke(reason));
    }

    private void ResetApi()
    {
        database = null;
        networkBroadcast = null;
        history = null;
        crypto = null;
    }

    private void InitializeDone()
    {
        //Authorization.Registration("new_folder", "qwerqwerqwe").Then(result => {
        //    CustomTools.Console.Warning("Registration:", CustomTools.Console.LogCyanColor(result));
        //});

        Authorization.AuthorizationBy(
            "m-mikhno",
            "P5KaiZRa3Yrb2pbnMkvGxwTK2umzF9mhVY7eUVZkbecN7"
        ).Then(result =>
        {


            CustomTools.Console.Warning("IsAuthorized:", CustomTools.Console.LogCyanColor(result));
            var account = Authorization.Current.UserNameData.FullAccount.Account.Id;
            var contract = SpaceTypeId.Create("1.16.4003");
            //var bytecode = "12e905b0";   // method = selfAddress()
            //var bytecode = "8da5cb5b";   // method = owner()
            //var bytecode = "1f7b6d32";   // method = length()
            //var bytecode = "d504ea1d";   // method = getArray()
            var bytecode =               // method = setArray(uint32[])
                "eb5c23e5" +
                "20".FromHex2Data(32).ToHexString() +
                "02".FromHex2Data(32).ToHexString() +
                "01".FromHex2Data(32).ToHexString() +
                "02".FromHex2Data(32).ToHexString();

            CallContract(account.Id, contract.Id, bytecode, 0, 0, 10000000, 0, confirmation =>
            {
                CustomTools.Console.Warning(confirmation);
                Database.GetContractResult((confirmation.Transaction.OperationResults.First().Value as SpaceTypeId).Id).Then(contractResult =>
                {
                    CustomTools.Console.Warning(contractResult.Result.Output.ToHexString());
                });
            });

            //Database.CallContractNoChangingState(contract.Id, account.Id, 0, bytecode).Then(output =>
            //{
            //    CustomTools.Console.Warning(output);
            //});
        });

        OnAllApiInitialized.SafeInvoke();
    }

    private void InitializeApi(LoginApi api)
    {
        api.Login(string.Empty, string.Empty).Then(loginResult =>
        {
            if (loginResult)
            {
                Promise.All(
                    Database.Init().Then(DatabaseApiInitialized),
                    NetworkBroadcast.Init().Then(NetworkBroadcastApiInitialized),
                    History.Init().Then(HistoryApiInitialized),
                    Crypto.Init().Then(CryptoApiInitialized)
                ).Then((Action)InitializeDone);
            }
            else
            {
                CustomTools.Console.DebugLog("RequestManager class", CustomTools.Console.LogRedColor("Login Failed!"), "Login()");
            }
        });
    }

    private IPromise DatabaseApiInitialized(DatabaseApi api)
    {
        return api.GetChainId().Then((Action<string>)SetChainId).Then(result => Repository.SubscribeToNotice(api).Then(() =>
        {
            OnDatabaseApiInitialized.SafeInvoke(api);
            return Promise.Resolved();
        }));
    }

    private IPromise NetworkBroadcastApiInitialized(NetworkBroadcastApi api)
    {
        return new Promise((resolved, rejected) =>
        {
            OnNetworkBroadcastApiInitialized.SafeInvoke(api);
            resolved();
        });
    }

    private IPromise HistoryApiInitialized(HistoryApi api)
    {
        return new Promise((resolved, rejected) =>
        {
            OnHistoryApiInitialized.SafeInvoke(api);
            resolved();
        });
    }

    private IPromise CryptoApiInitialized(CryptoApi api)
    {
        return new Promise((resolved, rejected) =>
        {
            OnCryptoApiInitialized.SafeInvoke(api);
            resolved();
        });
    }
    #endregion


    #region Request/Response
    public void Send(Request request)
    {
        requestBuffer.Add(request);
        if (!sendByUpdate && CanSend)
        {
            ConnectionManager.DoAll(requestBuffer);
        }
    }

    public RequestIdentificator Identificators => identificators;

    private void InitRegularCallbacks(Connection connection)
    {
        connection.AddRegular(identificators.OpenId, ConnectionOpened);
        connection.AddRegular(identificators.CloseId, ConnectionClosed);
    }
    #endregion


    private static void SetChainId(string newChainId)
    {
        newChainId = newChainId.OrEmpty();
        chainId = newChainId;
        ChainConfig.SetChainId(newChainId);
    }

    public IPromise CallContract(uint accountId, uint contractId, string bytecode, uint feeAssetId = 0, ulong amount = 0, ulong gas = 4700000, ulong gasPrice = 0, Action<TransactionConfirmation> resultCallback = null)
    {
        if (!Authorization.IsAuthorized)
        {
            return Promise.Rejected(new InvalidOperationException("Isn't Authorized!"));
        }
        var operation = new ContractOperationData
        {
            Registrar = SpaceTypeId.CreateOne(SpaceType.Account, accountId),
            Receiver = SpaceTypeId.CreateOne(SpaceType.Contract, contractId),
            Code = bytecode.OrEmpty(),
            Asset = SpaceTypeId.CreateOne(SpaceType.Asset, feeAssetId),
            Value = amount,
            GasPrice = gasPrice,
            Gas = gas
        };
        return Authorization.ProcessTransaction(new TransactionBuilder().AddOperation(operation), operation.Asset, resultCallback);
    }

    public IPromise DeployContract(uint accountId, string bytecode, uint feeAssetId = 0, ulong gas = 4700000, ulong gasPrice = 0, Action<TransactionConfirmation> resultCallback = null)
    {
        if (!Authorization.IsAuthorized)
        {
            return Promise.Rejected(new InvalidOperationException("Isn't Authorized!"));
        }
        var operation = new ContractOperationData
        {
            Registrar = SpaceTypeId.CreateOne(SpaceType.Account, accountId),
            Receiver = null,
            Code = bytecode.OrEmpty(),
            Asset = SpaceTypeId.CreateOne(SpaceType.Asset, feeAssetId),
            Value = 0,
            GasPrice = gasPrice,
            Gas = gas
        };
        return Authorization.ProcessTransaction(new TransactionBuilder().AddOperation(operation), operation.Asset, resultCallback);
    }
}