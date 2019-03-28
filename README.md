EchoUnityLib - Echo API implementation for Unity

*  using websocket-sharp
*  using Newtonsoft.Json
*  using Promise


1.   Connection to node
First you need to establish a connection to the node. To do this, use two scripts: ConnectionManager and NodeManager.
- ConnectionManager provides the ability to establish a connection to a node using the websocket-sharp lib.
- NodeManager operates with node addresses and initializes connection with them via ConnectionManager.


2.  Api
Access to the Echo API is provided by the EchoApiManager class.

3. Api Initialization
To initialize, you need to move the prefabs to the scene: EchoApiManager.prefab and ConnectionManager.prefab. EchoApiManager automatically up and maintains the connection to the node through the ConnectionManager.

After successful initialization, it becomes possible to access the following Api levels:
- Database - receiving objects and data
- NetworkBroadcast - execution of transactions
- History - getting account history
Api methods can be found in their respective classes.

Also, after the connection is established, EchoApiManager automatically subscribes to the notice distribution channel. For the aggregation and granulation of the current state of objects, the Repository class is used. it contains the latest object states received via the notice channel.

4. Authorization
EchoApiManager.Authorization contains current authorization information. You must call EchoApiManager.Authorization.AuthorizationBy (login, password) with a login and password.

`
>  
EchoApiManager.Instance.Authorization.AuthorizationBy(loginInputField.text, passwordInputField.text).Then(result =>
{
    if (result)
    {
        // Authorization successful
    }
    else
    {
        // Authorization failed
    }
});
`
>  

2.3) Contract
To work with contracts, the EchoApiManager.CallContract and EchoApiManager.DeployContract methods are used. As bytecode, the contrast code is transmitted when deploy, or the name of the method when call. The result of the execution can be obtained using EchoApiManager.Database.GetContractResult (resultId).

var bytecode = "7490d445"; // method getSize() at contract
EchoApiManager.Instance.CallContract(contractId, accountId, bytecode, 0, 0, 10000000, 0, res =>
{
    EchoApiManager.Instance.Database.GetContractResult((res.Transaction.OperationResults.First().Value as SpaceTypeId).Id).Then(contractResult =>
    {
        var data = contractResult.Result.Output;
        // Parse data to value...
    });
});


EchoApiManager.Instance.DeployContract(EchoApiManager.Instance.Authorization.Current.UserNameData.Value.Account.Id.Id, bytecodeInputField.text, 0, 10000000, 0, res =>
{
    EchoApiManager.Instance.Database.GetContractResult((res.Transaction.OperationResults.First().Value as SpaceTypeId).Id).Then(contractResult =>
    {
        // Getted contract deploy result
    });
});

The whole library is built on the mechanism of deferred call (promise). Before using Api it is recommended to study.

The EchoLib/Example/Scenes directory contains the scene Calculator.unity with an example of the work of a contract of a primitive calculator contract. The EchoLib/Example/Contract directory contains the source code of the contract and its bytecode.