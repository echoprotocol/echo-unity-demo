using System;
using Base.Config;
using Buffers;
using Newtonsoft.Json.Linq;
using Tools.Json;


namespace Base.Data.Operations.Fee
{
    public sealed class WitnessUpdateOperationFeeParametersData : FeeParametersData
    {
        private const string FEE_FIELD_KEY = "fee";


        public long Fee { get; set; }

        public override ChainTypes.FeeParameters Type => ChainTypes.FeeParameters.WitnessUpdateOperation;

        public override ByteBuffer ToBufferRaw(ByteBuffer buffer = null)
        {
            buffer = buffer ?? new ByteBuffer(ByteBuffer.LITTLE_ENDING);
            buffer.WriteInt64(Fee);
            return buffer;
        }

        public override string Serialize()
        {
            return new JsonBuilder(new JsonDictionary {
                { FEE_FIELD_KEY,    Fee }
            }).Build();
        }

        public static WitnessUpdateOperationFeeParametersData Create(JObject value)
        {
            var token = value.Root;
            var instance = new WitnessUpdateOperationFeeParametersData();
            instance.Fee = Convert.ToInt64(value.TryGetValue(FEE_FIELD_KEY, out token) ? token.ToObject<object>() : 0);
            return instance;
        }
    }
}