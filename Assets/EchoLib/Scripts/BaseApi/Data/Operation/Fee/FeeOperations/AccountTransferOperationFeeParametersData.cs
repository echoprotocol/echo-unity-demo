using Buffers;
using Base.Config;
using Newtonsoft.Json.Linq;
using Tools.Json;


namespace Base.Data.Operations.Fee
{
    public sealed class AccountTransferOperationFeeParametersData : FeeParametersData
    {
        private const string FEE_FIELD_KEY = "fee";


        public ulong Fee { get; set; }

        public override ChainTypes.FeeParameters Type => ChainTypes.FeeParameters.AccountTransferOperation;

        public override ByteBuffer ToBufferRaw(ByteBuffer buffer = null)
        {
            buffer = buffer ?? new ByteBuffer(ByteBuffer.LITTLE_ENDING);
            buffer.WriteUInt64(Fee);
            return buffer;
        }

        public override string Serialize()
        {
            return new JsonBuilder(new JsonDictionary {
                { FEE_FIELD_KEY,    Fee }
            }).Build();
        }

        public static AccountTransferOperationFeeParametersData Create(JObject value)
        {
            var token = value.Root;
            var instance = new AccountTransferOperationFeeParametersData();
            instance.Fee = value.TryGetValue(FEE_FIELD_KEY, out token) ? token.ToObject<ulong>() : 0;
            return instance;
        }
    }
}