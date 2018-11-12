using System;
using Base.Config;
using Base.Data.Operations;
using Newtonsoft.Json.Linq;
using Tools;


namespace Base.Data.Json
{
    public sealed class OperationDataPairConverter : JsonCustomConverter<OperationData, JArray>
    {
        protected override OperationData Deserialize(JArray value, Type objectType)
        {
            if (value.IsNullOrEmpty() || value.Count != 2)
            {
                return null;
            }
            var type = (ChainTypes.Operation)Convert.ToInt32(value.First);
            switch (type)
            {
                case ChainTypes.Operation.Transfer:
                    return TransferOperationData.Create(value.Last as JObject);
                case ChainTypes.Operation.AccountCreate:
                    return AccountCreateOperationData.Create(value.Last as JObject);
                case ChainTypes.Operation.ProposalCreate:
                    return ProposalCreateOperationData.Create(value.Last as JObject);
                default:
                    CustomTools.Console.Error("Unexpected operation type:", type);
                    return null;
            }
        }

        protected override JArray Serialize(OperationData value)
        {
            if (value == null)
            {
                return new JArray();
            }
            return new JArray((int)value.Type, JObject.Parse(value.ToString()));
        }
    }
}