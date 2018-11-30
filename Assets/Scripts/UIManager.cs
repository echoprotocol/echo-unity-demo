using Base.Data;
using CustomTools.Extensions.Core.Array;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        calculateButton.interactable = false;

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

        //contractAddressInputField.text = $"{PlayerPrefs.GetString("contract_address", "1.16.7186")}";
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

    public void CheckInput(string input)
    {
        calculateButton.interactable = false;

        MatchCollection matches = Regex.Matches(regexInputField.text, @"([0-9]+|[-+*/])");

        string res = string.Empty;

        if (matches.Count > 0 && Regex.IsMatch(matches[0].Value, @"[0-9]+"))
        {
            res += matches[0].Value;
        }

        if(matches.Count > 1 && Regex.IsMatch(matches[1].Value, @"[-+*/]"))
        {
            res += " " + matches[1].Value;
        }

        if (matches.Count > 2 && Regex.IsMatch(matches[2].Value, @"[0-9]+"))
        {
            res += " " + matches[2].Value;
            calculateButton.interactable = true;
        }

        regexInputField.text = res;
        regexInputField.caretPosition = res.Length;
    }

    private void Calculate()
    {
        calculateButton.interactable = false;

        var accountId = EchoApiManager.Instance.Authorization.Current.UserNameData.FullAccount.Account.Id.Id;
        var contractId = uint.Parse(contractAddressInputField.text.Split('.')[2]);
        var values = regexInputField.text.Split(' ');

        string bytecode = "1eba86cc";
        Parser.SerializeInts(ref bytecode, RegexToInts(regexInputField.text));

        Debug.Log(bytecode);

        EchoApiManager.Instance.CallContract(accountId, contractId, bytecode, 0, 0, 10000000, 0, res =>
        {
            calculateButton.interactable = true;
            CustomTools.Console.Warning(res);

            EchoApiManager.Instance.Database.GetContractResult((res.Transaction.OperationResults.First().Value as SpaceTypeId).Id).Then(contractResult =>
            {
                CustomTools.Console.Warning(contractResult);
            });
        });
    }

    private int[] RegexToInts(string regex)
    {
        int[] res = new int[3];
        string[] values = regex.Split(' ');

        res[0] = int.Parse(values[0]);
        res[1] = int.Parse(values[2]);

        switch (values[1])
        {
            case "+":
                res[2] = 1;
                break;

            case "-":
                res[2] = 2;
                break;

            case "*":
                res[2] = 3;
                break;

            case "/":
                res[2] = 4;
                break;
        }

        return res;
    }
}
