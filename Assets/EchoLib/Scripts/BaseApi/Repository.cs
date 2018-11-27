using System;
using System.Collections.Generic;
using Base.Api.Database;
using Base.Data;
using Base.Data.Accounts;
using Base.Data.Assets;
using Base.Data.Block;
using Base.Data.Contract;
using Base.Data.Operations;
using Base.Data.Order;
using Base.Data.Properties;
using Base.Data.Transactions;
using Base.Data.Witnesses;
using CustomTools.Extensions.Core;
using CustomTools.Extensions.Core.Action;
using Newtonsoft.Json.Linq;
using Promises;
using IdObjectDictionary = System.Collections.Generic.Dictionary<Base.Data.SpaceTypeId, Base.Data.IdObject>;


namespace Base.Storage
{
    public static class Repository
    {
        public static event Action<IdObject> OnGetObject;
        public static event Action<string> OnGetString;

        private readonly static Dictionary<SpaceType, IdObjectDictionary> root = new Dictionary<SpaceType, IdObjectDictionary>();


        private static void GetObject(IdObject idObject) => OnGetObject.SafeInvoke(idObject);

        private static void GetString(string value) => OnGetString.SafeInvoke(value);

        private static void ChangeNotify(JToken[] list)
        {  
            var notifyObjectList = new List<IdObject>();
            var notifyStringList = new List<string>();
            foreach (var item in list)
            {
                if (item.Type.Equals(JTokenType.Object))
                {
                    var idObject = item.ToIdObject();
                    if (idObject.IsNull())
                    {
                        continue;
                    }
                    Add(idObject);
                    notifyObjectList.Add(idObject);
                    CustomTools.Console.DebugLog("Update object:", CustomTools.Console.LogGreenColor(idObject.SpaceType), idObject.Id, '\n', CustomTools.Console.LogWhiteColor(idObject));
                }
                else
                if (item.Type.Equals(JTokenType.String))
                {
                    notifyStringList.Add(item.ToString());
                    CustomTools.Console.DebugLog("Get string:", CustomTools.Console.LogCyanColor(item));
                }
                else
                {
                    CustomTools.Console.DebugWarning("Get unexpected json type:", CustomTools.Console.LogYellowColor(item.Type), CustomTools.Console.LogCyanColor(item));
                }
            }
            foreach (var newObject in notifyObjectList)
            {
                GetObject(newObject);
            }
            foreach (var newString in notifyStringList)
            {
                GetString(newString);
            }
        }

        private static void Add(IdObject idObject)
        {
            (root.ContainsKey(idObject.SpaceType) ? root[idObject.SpaceType] : (root[idObject.SpaceType] = new IdObjectDictionary()))[idObject.Id] = idObject;
        }

        private static IPromise AddInPromise(IdObject idObject)
        {
            Add(idObject);
            return Promise.Resolved();
        }

        private static IPromise Init(DatabaseApi api)
        {
            return Promise.All(
                api.GetDynamicGlobalProperties().Then(AddInPromise),
                api.GetGlobalProperties().Then(AddInPromise),
                api.GetAsset().Then(AddInPromise)
            );
        }

        public static IPromise SubscribeToNotice(DatabaseApi api)
        {
            return api.SubscribeNotice(ChangeNotify);//.Then(() => Init(api));
        }

        public static bool IsExist(SpaceTypeId spaceTypeId)
        {
            return root.ContainsKey(spaceTypeId.SpaceType) && root[spaceTypeId.SpaceType].ContainsKey(spaceTypeId);
        }

        public static IPromise<T> GetInPromise<T>(SpaceTypeId key, Func<IPromise<T>> getter = null) where T : IdObject
        {
            if (IsExist(key))
            {
                return Promise<T>.Resolved((T)root[key.SpaceType][key]);
            }
            if (getter.IsNull())
            {
                return Promise<T>.Resolved(null);
            }
            return getter.Invoke().Then(idObject =>
            {
                Add(idObject);
                return Promise<T>.Resolved(idObject);
            });
        }

        public static IdObject[] GetAll(SpaceType spaceType)
        {
            return root.ContainsKey(spaceType) ? new List<IdObject>(root[spaceType].Values).ToArray() : new IdObject[0];
        }
    }


    public static class Extensions
    {
        public static IdObject ToIdObject(this JToken source)
        {
            var sample = source.ToObject<IdObject>();
            if (sample.Id.IsNullOrEmpty())
            {
                CustomTools.Console.DebugWarning("Get unexpected object:", source.ToString());
                return null;
            }
            switch (sample.SpaceType)
            {
                case SpaceType.Base:/*                          */return source.ToObject<BaseObject>();
                case SpaceType.Account:/*                       */return source.ToObject<AccountObject>();
                case SpaceType.Asset:/*                         */return source.ToObject<AssetObject>();
                case SpaceType.ForceSettlement:/*               */return source.ToObject<ForceSettlementObject>();
                case SpaceType.CommitteeMember:/*               */return source.ToObject<CommitteeMemberObject>();
                case SpaceType.Witness:/*                       */return source.ToObject<WitnessObject>();
                case SpaceType.LimitOrder:/*                    */return source.ToObject<LimitOrderObject>();
                case SpaceType.CallOrder:/*                     */return source.ToObject<CallOrderObject>();
                case SpaceType.Custom:/*                        */return source.ToObject<CustomObject>();
                case SpaceType.Proposal:/*                      */return source.ToObject<ProposalObject>();
                case SpaceType.OperationHistory:/*              */return source.ToObject<OperationHistoryObject>();
                case SpaceType.WithdrawPermission:/*            */return source.ToObject<WithdrawPermissionObject>();
                case SpaceType.VestingBalance:/*                */return source.ToObject<VestingBalanceObject>();
                case SpaceType.Worker:/*                        */return source.ToObject<WorkerObject>();
                case SpaceType.Balance:/*                       */return source.ToObject<BalanceObject>();
                case SpaceType.Contract:/*                      */return source.ToObject<ContractObject>();
                case SpaceType.ResultExecute:/*                 */return source.ToObject<ResultExecuteObject>();
                case SpaceType.BlockResult:/*                   */return source.ToObject<BlockResultObject>();
                case SpaceType.GlobalProperties:/*              */return source.ToObject<GlobalPropertiesObject>();
                case SpaceType.DynamicGlobalProperties:/*       */return source.ToObject<DynamicGlobalPropertiesObject>();
                case SpaceType.AssetDynamicData:/*              */return source.ToObject<AssetDynamicDataObject>();
                case SpaceType.AssetBitassetData:/*             */return source.ToObject<AssetBitassetDataObject>();
                case SpaceType.AccountBalance:/*                */return source.ToObject<AccountBalanceObject>();
                case SpaceType.AccountStatistics:/*             */return source.ToObject<AccountStatisticsObject>();
                case SpaceType.Transaction:/*                   */return source.ToObject<TransactionObject>();
                case SpaceType.BlockSummary:/*                  */return source.ToObject<BlockSummaryObject>();
                case SpaceType.AccountTransactionHistory:/*     */return source.ToObject<AccountTransactionHistoryObject>();
                case SpaceType.BlindedBalance:/*                */return source.ToObject<BlindedBalanceObject>();
                case SpaceType.ChainProperty:/*                 */return source.ToObject<ChainPropertyObject>();
                case SpaceType.WitnessSchedule:/*               */return source.ToObject<WitnessScheduleObject>();
                case SpaceType.BudgetRecord:/*                  */return source.ToObject<BudgetRecordObject>();
                case SpaceType._todo_object_2_14:/*             */return source.ToObject<Object_2_14>();
                case SpaceType._todo_object_2_15:/*             */return source.ToObject<Object_2_15>();
                case SpaceType._todo_object_2_16:/*             */return source.ToObject<Object_2_16>();
                case SpaceType._todo_object_2_17:/*             */return source.ToObject<Object_2_17>();
                case SpaceType._todo_object_2_18:/*             */return source.ToObject<Object_2_18>();
                case SpaceType.ContractTransactionHistory:/*    */return source.ToObject<ContractTransactionHistoryObject>();
                case SpaceType.ContractStatistics:/*            */return source.ToObject<ContractStatisticsObject>();
                default:
                    CustomTools.Console.DebugWarning("Get unexpected SpaceType:", CustomTools.Console.LogCyanColor(sample.SpaceType), sample.Id, '\n', source);
                    return null;
            }
        }
    }
}