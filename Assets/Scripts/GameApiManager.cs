using System;
using System.Text;
using CustomTools.Extensions.Core;
using CustomTools.Extensions.Core.Array;
using RSG;
using Tools.Hash;
using Tools.HexBinDec;
using UnityEngine;


public class GameApiManager : MonoBehaviour
{
    public struct Point
    {
        public int x, y;
    }


    public enum GameState
    {
        Start = 1,
        InProgress = 2,
        End = 3
    }


    public enum MoveAnswer
    {
        None = 0,
        Miss = 1,
        Hurt = 2,
        Kill = 3,
        Defeat = 4
    }


    public enum GameResult
    {
        None = 0,
        Defeat = 2,
        Win = 3,
        Draw = 4
    }

    // "1.9.0" devnet
    [SerializeField] private uint contractId = 0;

    private BattleContainer currentButtle;


    private bool IsInitialized
    {
        get { return false; }
    }

    #region SearchGame

    //Запрос на поиск игры, если игры в ожидании нет, то добавляем его в ожидание, если есть, присоединяем.
    //Возвращает 0, если игра в ожидании, возвращаем ид битвы, если присоединен
    public IPromise<ulong> StartSearchGame()
    {
        if (IsInitialized)
        {
            return EchoApiManager.Instance.CallContract(contractId, GetBytecode("start_search")).Then(() => Promise<ulong>.Resolved(0));
        }
        return Promise<ulong>.Rejected(new Exception("Is't initialized"));
    }

    //Запрос на отмену игры, игра отменяется, если она в ожидании
    //Возвращает true, если отменена, иначе false
    public IPromise<bool> CancelSearchGame()
    {
        if (IsInitialized && !currentButtle.IsNull())
        {
            // cancel_search(uint64_t battle_id)
            return EchoApiManager.Instance.CallContract(contractId, GetBytecode("cancel_search", currentButtle.Id)).Then(() => Promise<bool>.Resolved(false));
        }
        return Promise<bool>.Rejected(new Exception("Is't initialized"));
    }

    //Запрос на проверку, присоединился ли второй человек
    //Возвращает ид битвы, если второй присоединился, иначе 0
    public IPromise<ulong> CallForBattle()
    {
        if (IsInitialized)
        {
            return EchoApiManager.Instance.QueryContract(contractId, GetBytecode("call_for_battle")).Then(result => Promise<ulong>.Resolved(0));
        }
        return Promise<ulong>.Rejected(new Exception("Is't initialized"));
    }

    //Отправка хэша стартовой позиции
    //Возвращает true, если хэш принят, иначе false(идет проверка на совпадение идов игроков в битве с battle_id)
    public IPromise<bool> StartGame(bool[] map)
    {
        if (IsInitialized && !currentButtle.IsNull())
        {
            // start_game(uint64_t battle_id, string hash_start_pos)
            var mapHash = string.Empty;
            return EchoApiManager.Instance.CallContract(contractId, GetBytecode("start_game", currentButtle.Id, mapHash)).Then(() => Promise<bool>.Resolved(false));
        }
        return Promise<bool>.Rejected(new Exception("Is't initialized"));
    }

    //Отправка хэша стартовой позиции и очередности текущего хода
    //Возвращает account_id - указывающий, кто сейчас ходит, иначе "" - соперник еще не прислал свой хеш
    public IPromise<string> CallForStart()
    {
        if (IsInitialized && !currentButtle.IsNull())
        {
            // call_for_start(uint64_t battle_id)
            return EchoApiManager.Instance.QueryContract(contractId, GetBytecode("call_for_start", currentButtle.Id)).Then(result => Promise<string>.Resolved(string.Empty));
        }
        return Promise<string>.Rejected(new Exception("Is't initialized"));
    }

    #endregion

    #region Game

    //Отправка хода
    //Возвращает true, если ответ принят, иначе false(идет проверка этот ли игрок сейчас ходит)
    public IPromise<bool> DoMove(byte x, byte y)
    {
        if (IsInitialized && !currentButtle.IsNull())
        {
            // move(uint64_t battle_id, uint8_t x, uint8_t y)
            return EchoApiManager.Instance.CallContract(contractId, GetBytecode("move", currentButtle.Id, x, y)).Then(() => Promise<bool>.Resolved(false));
        }
        return Promise<bool>.Rejected(new Exception("Is't initialized"));
    }

    //Запрос на то, прислал ли другой игрок свой ход
    //Возвращает пару[x, y], иначе[-1, -1]
    public IPromise<Point> GetMove()
    {
        if (IsInitialized && !currentButtle.IsNull())
        {
            // pair<uint8_t, uint8_t> call_for_move(uint64_t battle_id)
            return EchoApiManager.Instance.QueryContract(contractId, GetBytecode("call_for_move", currentButtle.Id)).Then(result => Promise<Point>.Resolved(new Point { x = -1, y = -1 }));
        }
        return Promise<Point>.Rejected(new Exception("Is't initialized"));
    }


    // todo
    //Отправка ответа
    //Возвращает true, если ответ принят, иначе false(идет проверка этот ли игрок сейчас отвечает)
    public IPromise<byte> DoAnswer(MoveAnswer answer)
    {
        if (IsInitialized && !currentButtle.IsNull())
        {
            // uint8_t answer(uint64_t battle_id, uint8_t answer)
            return EchoApiManager.Instance.CallContract(contractId, GetBytecode("answer", currentButtle.Id, (byte)answer)).Then(() => Promise<byte>.Resolved(0));
        }
        return Promise<byte>.Rejected(new Exception("Is't initialized"));
    }

    //Запрос на то, прислал ли другой игрок ответ
    //Возвращает ответ, если прислал, иначе 0
    public IPromise<MoveAnswer> GetAnswer()
    {
        if (IsInitialized && !currentButtle.IsNull())
        {
            // uint8_t call_for_answer(uint64_t battle_id)
            return EchoApiManager.Instance.QueryContract(contractId, GetBytecode("call_for_answer", currentButtle.Id)).Then(result => Promise<MoveAnswer>.Resolved(MoveAnswer.None));
        }
        return Promise<MoveAnswer>.Rejected(new Exception("Is't initialized"));
    }

    #endregion

    #region EndGame

    //Отправка стартовой позиции в открытом виде
    //Возвращает true, если стартовая позиция принята, иначе false
    public IPromise<bool> EndGame(bool[] map)
    {
        if (IsInitialized && !currentButtle.IsNull())
        {
            // end_game(uint64_t battle_id, string start_pos)
            var mapString = string.Empty;
            return EchoApiManager.Instance.CallContract(contractId, GetBytecode("end_game", currentButtle.Id, mapString)).Then(() => Promise<bool>.Resolved(false));
        }
        return Promise<bool>.Rejected(new Exception("Is't initialized"));
    }

    //Запрос на то, отправил ли другой игрок стартовую позицию
    //Возврашает true, если отправил, иначе false
    public IPromise<bool> CallForEnd()
    {
        if (IsInitialized && !currentButtle.IsNull())
        {
            return EchoApiManager.Instance.QueryContract(contractId, GetBytecode("call_for_end", currentButtle.Id)).Then(result => Promise<bool>.Resolved(false));
        }
        return Promise<bool>.Rejected(new Exception("Is't initialized"));
    }

    //Проверка валидности всех ходов, ответов, стартовой позиции
    //Возвращает win = 3, если победа, defeat = 2, если проиграл, draw = 4, если ничья, иначе 0
    public IPromise<GameResult> CheckValidity()
    {
        if (IsInitialized && !currentButtle.IsNull())
        {
            // uint8_t check_validity(uint64_t battle_id)
            return EchoApiManager.Instance.QueryContract(contractId, GetBytecode("check_validity", currentButtle.Id)).Then(result => Promise<GameResult>.Resolved(GameResult.None));
        }
        return Promise<GameResult>.Rejected(new Exception("Is't initialized"));
    }

    #endregion


    public IPromise<bool> CheckLeave(GameState state)
    {
        // cash state
        if (IsInitialized && !currentButtle.IsNull())
        {
            // check_leave(uint64_t battle_id, uint8_t stage)
            return EchoApiManager.Instance.QueryContract(contractId, GetBytecode("check_leave", currentButtle.Id, (byte)state)).Then(result => Promise<bool>.Resolved(false));
        }
        return Promise<bool>.Rejected(new Exception("Is't initialized"));
    }

    private string GetBytecode(string funcName, params object[] args)//, param: [AbiTypeValueInputModel])
    {
        var argsData = "";// try argumentCoder.getArguments(valueTypes: args).hex;
        var builder = new StringBuilder(funcName);
        builder.Append('(');
        for (var i = 0; i < args.Length; i++)
        {
            ((i > 0) ? builder.Append(',') : builder).Append("");//  args[i].Type.Description);
        }
        builder.Append(')');
        var data = builder.Append(argsData).ToString();
        builder.Clear();
        return KECCAK256.Create().ComputeHash(Encoding.UTF8.GetBytes(data)).Slice(0, 4).ToHexString();
    }

    private void Awake()
    {
        EchoApiManager.OnAllApiInitialized += Test;
    }

    private void Test()
    {
        //EchoApiManager.Instance.Authorization.AuthorizationBy("test123", new WifContainer("5K8NSy4K9yoKY2gBPsZnnnjuwkVyq6vbgVgmPwPWJzt9Lfk4YvW")).Then(result =>
        //{
        //    if (result == AuthorizationContainer.AuthorizationResult.Ok)
        //    {
        //        EchoApiManager.Instance.CallContract(contractId, GetBytecode("start_search"));
        //    }
        //});
    }



    private class PassContainer : Base.Keys.IPass
    {
        private string password;


        public PassContainer(string password)
        {
            this.password = password;
        }

        public void Dispose() => password = null;

        public byte[] Get() => Encoding.UTF8.GetBytes(password.Trim());
    }

    private class WifContainer : Base.Keys.IWif
    {
        private string wif;


        public WifContainer(string wif)
        {
            this.wif = wif;
        }

        public void Dispose() => wif = null;

        public string Get() => wif.Trim();
    }
}