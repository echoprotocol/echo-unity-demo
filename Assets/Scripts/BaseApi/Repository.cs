using System;
using System.Collections.Generic;
using Base.Api.Database;
using Base.Data;
using Base.Data.Accounts;
using Base.Data.Assets;
using Base.Data.Block;
using Base.Data.Operations;
using Base.Data.Properties;
using Base.Data.Transactions;
using Base.Data.Witnesses;
using CustomTools.Extensions.Core.Action;
using Newtonsoft.Json.Linq;
using Promises;
using Tools;
using IdObjectDictionary = System.Collections.Generic.Dictionary<Base.Data.SpaceTypeId, Base.Data.IdObject>;


namespace Base
{
    public static class Repository
    {
        public static event Action<IdObject> OnObjectUpdate;

        private readonly static Dictionary<SpaceType, IdObjectDictionary> root = new Dictionary<SpaceType, IdObjectDictionary>();


        private static void ObjectUpdate(IdObject idObject)
        {
            OnObjectUpdate.SafeInvoke(idObject);
        }

        private static void ChangeNotify(JToken[] list)
        {
            var notifyList = new List<IdObject>();
            foreach (var item in list)
            {
                if (!item.Type.Equals(JTokenType.Object))
                {
                    CustomTools.Console.Warning("Get unexpected type:", CustomTools.Console.SetCyanColor(item.ToString()));
                    continue;
                }
                var idObject = item.ToObject<IdObject>();
                if (idObject.Id.IsNullOrEmpty())
                {
                    CustomTools.Console.Warning("Get unexpected object:", item.ToString());
                    continue;
                }
                switch (idObject.SpaceType)
                {
                    case SpaceType.Account:
                        idObject = item.ToObject<AccountObject>();
                        break;
                    case SpaceType.Asset:
                        idObject = item.ToObject<AssetObject>();
                        break;
                    case SpaceType.Witness:
                        idObject = item.ToObject<WitnessObject>();
                        break;
                    case SpaceType.OperationHistory:
                        idObject = item.ToObject<OperationHistoryObject>();
                        break;
                    case SpaceType.DynamicGlobalProperties:
                        idObject = item.ToObject<DynamicGlobalPropertiesObject>();
                        break;
                    case SpaceType.GlobalProperties:
                        idObject = item.ToObject<GlobalPropertiesObject>();
                        break;
                    case SpaceType.AssetDynamicData:
                        idObject = item.ToObject<AssetDynamicDataObject>();
                        break;
                    case SpaceType.AccountBalance:
                        idObject = item.ToObject<AccountBalanceObject>();
                        break;
                    case SpaceType.AccountStatistics:
                        idObject = item.ToObject<AccountStatisticsObject>();
                        break;
                    case SpaceType.Transaction:
                        idObject = item.ToObject<TransactionObject>();
                        break;
                    case SpaceType.BlockSummary:
                        idObject = item.ToObject<BlockSummaryObject>();
                        break;
                    case SpaceType.AccountTransactionHistory:
                        idObject = item.ToObject<AccountTransactionHistoryObject>();
                        break;
                    case SpaceType.WitnessSchedule:
                    case SpaceType.VestingBalance:
                    case SpaceType.BudgetRecord:
                    case SpaceType.CommitteeMember:
                        // skip objects
                        continue;
                    default:
                        CustomTools.Console.Warning("Get unexpected object type:", CustomTools.Console.SetCyanColor(idObject.SpaceType), '\n', item.ToString());
                        continue;
                }
                Add(idObject);
                notifyList.Add(idObject);
                CustomTools.Console.Log("Update object:", CustomTools.Console.SetGreenColor(idObject.SpaceType), idObject.Id, '\n', CustomTools.Console.SetWhiteColor(idObject));
            }
            foreach (var notify in notifyList)
            {
                ObjectUpdate(notify);
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
            return api.Subscribe(ChangeNotify).Then(() => Init(api));
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
}