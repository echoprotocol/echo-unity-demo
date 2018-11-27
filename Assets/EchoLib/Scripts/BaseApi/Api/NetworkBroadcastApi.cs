using System;
using Base.Data.Transactions;
using Base.Requests;
using CustomTools.Extensions.Core;
using Newtonsoft.Json.Linq;
using Promises;


namespace Base.Api.Database
{
    public sealed class NetworkBroadcastApi : ApiId
    {
        private NetworkBroadcastApi(ISender sender) : base(null, sender) { }

        public static NetworkBroadcastApi Create(ISender sender) => new NetworkBroadcastApi(sender);

        public IPromise<NetworkBroadcastApi> Init()
        {
            return new Promise<int>((resolve, reject) =>
            {
#if DEBUG
                var debug = false;
#else
                var debug = false;
#endif
                var methodName = "network_broadcast";
                var parameters = new Parameters { LoginApi.ID, methodName, new object[0] };
                DoRequest(GenerateNewId(), parameters, resolve, reject, methodName, debug);
            }).Then(apiId => (NetworkBroadcastApi)Init(apiId));
        }

        public IPromise BroadcastTransactionWithCallback(SignedTransactionData transaction, Action<JToken[]> transactionResultCallback = null)
        {
            if (IsInitialized)
            {
                return new Promise((resolve, reject) =>
                {
#if DEBUG
                    var debug = true;
#else
                    var debug = false;
#endif
                    var requestId = GenerateNewId();
                    var methodName = "broadcast_transaction_with_callback";
                    var title = methodName + " " + requestId;
                    var parameters = new Parameters { Id.Value, methodName, new object[] { requestId, transaction } };
                    DoRequestVoid(requestId, parameters, () =>
                    {
                        if (!transactionResultCallback.IsNull())
                        {
                            ConnectionManager.Subscribe("broadcast by " + requestId, requestId, transactionResultCallback, debug, true);
                        }
                        resolve();
                    }, reject, title, debug);
                });
            }
            return Init().Then(api => api.BroadcastTransactionWithCallback(transaction, transactionResultCallback));
        }
    }
}