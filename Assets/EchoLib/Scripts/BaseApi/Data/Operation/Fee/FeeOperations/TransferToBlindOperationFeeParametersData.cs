using Buffers;
using Base.Config;
using Newtonsoft.Json.Linq;
using Tools.Json;


namespace Base.Data.Operations.Fee
{
    public sealed class TransferToBlindOperationFeeParametersData : FeeParametersData
    {
        private const string FEE_FIELD_KEY = "fee";
        private const string PRICE_PER_OUTPUT_FIELD_KEY = "price_per_output";


        public ulong Fee { get; set; }
        public uint PricePerOutput { get; set; }

        public override ChainTypes.FeeParameters Type => ChainTypes.FeeParameters.TransferToBlindOperation;

        public override ByteBuffer ToBufferRaw(ByteBuffer buffer = null)
        {
            buffer = buffer ?? new ByteBuffer(ByteBuffer.LITTLE_ENDING);
            buffer.WriteUInt64(Fee);
            buffer.WriteUInt32(PricePerOutput);
            return buffer;
        }

        public override string Serialize()
        {
            return new JsonBuilder(new JsonDictionary {
                { FEE_FIELD_KEY,                 Fee },
                { PRICE_PER_OUTPUT_FIELD_KEY,    PricePerOutput }
            }).Build();
        }

        public static TransferToBlindOperationFeeParametersData Create(JObject value)
        {
            var token = value.Root;
            var instance = new TransferToBlindOperationFeeParametersData();
            instance.Fee = value.TryGetValue(FEE_FIELD_KEY, out token) ? token.ToObject<ulong>() : 0;
            instance.PricePerOutput = value.TryGetValue(PRICE_PER_OUTPUT_FIELD_KEY, out token) ? token.ToObject<uint>() : uint.MinValue;
            return instance;
        }
    }
}