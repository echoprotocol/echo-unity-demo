using System;
using Base.Config;
using Base.Data.Workers;
using CustomTools.Extensions.Core;
using Newtonsoft.Json.Linq;
using Tools.Json;


namespace Base.Data.Json
{
    public sealed class WorkerDataPairConverter : JsonCustomConverter<WorkerData, JArray>
    {
        protected override WorkerData Deserialize(JArray value, Type objectType)
        {
            if (value.IsNullOrEmpty() || value.Count != 2)
            {
                return null;
            }
            var type = (ChainTypes.Worker)Convert.ToInt32(value.First);
            switch (type)
            {
                case ChainTypes.Worker.Refund:
                    return RefundWorkerData.Create(value.Last as JObject);
                case ChainTypes.Worker.VestingBalance:
                    return VestingBalanceWorkerData.Create(value.Last as JObject);
                case ChainTypes.Worker.Burn:
                    return BurnWorkerData.Create(value.Last as JObject);
                default:
                    CustomTools.Console.DebugError("Unexpected worker type:", type);
                    return null;
            }
        }

        protected override JArray Serialize(WorkerData value)
        {
            return value.IsNull() ? new JArray() : new JArray((int)value.Type, JToken.FromObject(value.ToString()));
        }
    }
}