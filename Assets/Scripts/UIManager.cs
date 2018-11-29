using Base.Data;
using CustomTools.Extensions.Core.Array;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject login;
    [SerializeField] private GameObject work;

    [SerializeField] private Button loginButton;
    [SerializeField] private InputField loginInputField;
    [SerializeField] private InputField passwordInputField;

    [SerializeField] private InputField contractAddressInputField;
    [SerializeField] private InputField bytecodeInputField;
    [SerializeField] private Button deployButton;

    [SerializeField] private InputField regexInputField;
    [SerializeField] private Button calculateButton;

    private void Awake()
    {
        EchoApiManager.OnDatabaseApiInitialized += OnDatabaseApiInitialized;

        work.SetActive(false);

        loginButton.interactable = false;
        loginButton.onClick.AddListener(Login);

        deployButton.onClick.AddListener(DeployContract);

        calculateButton.onClick.AddListener(Calculate);
    }

    private void OnDatabaseApiInitialized(Base.Api.Database.DatabaseApi api)
    {
        loginButton.interactable = true;
    }

    private void Login()
    {
        loginButton.interactable = false;

        EchoApiManager.Instance.Authorization.AuthorizationBy(loginInputField.text, passwordInputField.text).Then(res =>
        {
            loginButton.interactable = true;

            if (res)
            {
                InitializeWork();
            }
        });
    }

    private void InitializeWork()
    {
        login.SetActive(false);
        work.SetActive(true);

        contractAddressInputField.text = $"{PlayerPrefs.GetString("contract_address", "1.16.4318")}";
    }

    private void DeployContract()
    {
        deployButton.interactable = false;

        EchoApiManager.Instance.DeployContract(EchoApiManager.Instance.Authorization.Current.UserNameData.FullAccount.Account.Id.Id, bytecodeInputField.text, 0, 10000000, 0, res =>
        {
            deployButton.interactable = true;
            CustomTools.Console.Warning(res);

            EchoApiManager.Instance.Database.GetContractResult((res.Transaction.OperationResults.First().Value as SpaceTypeId).Id).Then(contractResult =>
            {
                CustomTools.Console.Warning(contractResult);
            });
        });
    }

    private void Calculate()
    {
        calculateButton.interactable = false;

        var accountId = EchoApiManager.Instance.Authorization.Current.UserNameData.FullAccount.Account.Id.Id;
        var contractId = uint.Parse(regexInputField.text.Split('.')[2]);

        EchoApiManager.Instance.CallContract(accountId, contractId, "9acd39fe", 0, 0, 10000000, 0, res =>
        {
            calculateButton.interactable = true;

            EchoApiManager.Instance.Database.GetContractResult((res.Transaction.OperationResults.First().Value as SpaceTypeId).Id).Then(contractResult =>
            {
                CustomTools.Console.Warning(contractResult);
            });
        });
    }
}
