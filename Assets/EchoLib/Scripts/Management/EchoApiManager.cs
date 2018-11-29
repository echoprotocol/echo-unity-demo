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
using UnityEngine;
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
        CustomTools.Console.DebugLog("EchoApiManager class", CustomTools.Console.SetMagentaColor("Regular Callback:"), "ConnectionOpened()");
        InitializeApi(LoginApi.Create(this));
        response.SendResultData<string>(url => OnConnectionOpened.SafeInvoke(url));
    }

    private void ConnectionClosed(Response response)
    {
        CustomTools.Console.DebugLog("EchoApiManager class", CustomTools.Console.SetMagentaColor("Regular Callback:"), "ConnectionClosed()");
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
        //Authorization.AuthorizationBy(
        //    "m-mikhno",
        //    "P5KaiZRa3Yrb2pbnMkvGxwTK2umzF9mhVY7eUVZkbecN7"
        //).Then(result =>
        //{
        //    CustomTools.Console.Warning("IsAuthorized:", CustomTools.Console.SetCyanColor(result));
        //    var account = Authorization.Current.UserNameData.FullAccount.Account.Id;
        //    var contract = SpaceTypeId.Create("1.16.4318");

        //    //var bytecode = "608060405234801561001057600080fd5b50610641806100206000396000f3fe608060405260043610610062576000357c0100000000000000000000000000000000000000000000000000000000900463ffffffff1680635fe36f0a14610067578063a5f3c23b14610092578063adefc37b146100cf578063b2e9949d1461010c575b600080fd5b34801561007357600080fd5b5061007c61014b565b6040516100899190610524565b60405180910390f35b34801561009e57600080fd5b506100b960048036036100b49190810190610404565b610239565b6040516100c69190610546565b60405180910390f35b3480156100db57600080fd5b506100f660048036036100f19190810190610404565b610274565b6040516101039190610546565b60405180910390f35b34801561011857600080fd5b50610133600480360361012e9190810190610440565b6102af565b60405161014293929190610561565b60405180910390f35b60606000805480602002602001604051908101604052809291908181526020016000905b8282101561023057838290600052602060002090600302016060604051908101604052908160008201548152602001600182015481526020016002820160009054906101000a90047f0100000000000000000000000000000000000000000000000000000000000000027effffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff19167effffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916815250508152602001906001019061016f565b50505050905090565b6000610269838360017f010000000000000000000000000000000000000000000000000000000000000002610314565b818301905092915050565b60006102a4838360027f010000000000000000000000000000000000000000000000000000000000000002610314565b818303905092915050565b6000818154811015156102be57fe5b90600052602060002090600302016000915090508060000154908060010154908060020160009054906101000a90047f010000000000000000000000000000000000000000000000000000000000000002905083565b6000606060405190810160405280858152602001848152602001837effffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff191681525090806001815401808255809150509060018203906000526020600020906003020160009091929091909150600082015181600001556020820151816001015560408201518160020160006101000a81548160ff02191690837f010000000000000000000000000000000000000000000000000000000000000090040217905550505050505050565b60006103e882356105f3565b905092915050565b60006103fc82356105fd565b905092915050565b6000806040838503121561041757600080fd5b6000610425858286016103dc565b9250506020610436858286016103dc565b9150509250929050565b60006020828403121561045257600080fd5b6000610460848285016103f0565b91505092915050565b6000610474826105a5565b80845260208401935061048683610598565b60005b828110156104b85761049c8683516104e2565b6104a5826105b0565b9150606086019550600181019050610489565b50849250505092915050565b6104cd816105bd565b82525050565b6104dc816105e9565b82525050565b6060820160008201516104f860008501826104d3565b50602082015161050b60208501826104d3565b50604082015161051e60408501826104c4565b50505050565b6000602082019050818103600083015261053e8184610469565b905092915050565b600060208201905061055b60008301846104d3565b92915050565b600060608201905061057660008301866104d3565b61058360208301856104d3565b61059060408301846104c4565b949350505050565b6000602082019050919050565b600081519050919050565b6000602082019050919050565b60007fff0000000000000000000000000000000000000000000000000000000000000082169050919050565b6000819050919050565b6000819050919050565b600081905091905056fea265627a7a72305820130c0d431b44e65246f27fb5a5aef9df5b7940a06e27a0c3140891b39acea4c96c6578706572696d656e74616cf50037";
        //    //var bytecode = "a5f3c23b" + "10".FromHex2Data(32).ToHexString() + "03".FromHex2Data(32).ToHexString();
        //    var bytecode = "5fe36f0a";

        //    //var bytecode = "12e905b0";   // method = selfAddress()
        //    //var bytecode = "8da5cb5b";   // method = owner()
        //    //var bytecode = "1f7b6d32";   // method = length()
        //    //var bytecode = "d504ea1d";   // method = getArray()
        //    //var bytecode =               // method = setArray(uint32[])
        //    //    "eb5c23e5" +
        //    //    "20".FromHex2Data(32).ToHexString() +
        //    //    "02".FromHex2Data(32).ToHexString() +
        //    //    "01".FromHex2Data(32).ToHexString() +
        //    //    "02".FromHex2Data(32).ToHexString();

        //    //DeployContract(account.Id, bytecode, 0, 10000000, 0, confirmation =>
        //    //{
        //    //    CustomTools.Console.Warning(confirmation);
        //    //    Database.GetContractResult((confirmation.Transaction.OperationResults.First().Value as SpaceTypeId).Id).Then(contractResult =>
        //    //    {
        //    //        CustomTools.Console.Warning(contractResult);
        //    //    });
        //    //});

        //    CallContract(account.Id, contract.Id, bytecode, 0, 0, 10000000, 0, confirmation =>
        //    {
        //        CustomTools.Console.Warning(confirmation);
        //        Database.GetContractResult((confirmation.Transaction.OperationResults.First().Value as SpaceTypeId).Id).Then(contractResult =>
        //        {
        //            CustomTools.Console.Warning(contractResult);
        //        });
        //    });
        //});



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
                CustomTools.Console.DebugLog("RequestManager class", CustomTools.Console.SetRedColor("Login Failed!"), "Login()");
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