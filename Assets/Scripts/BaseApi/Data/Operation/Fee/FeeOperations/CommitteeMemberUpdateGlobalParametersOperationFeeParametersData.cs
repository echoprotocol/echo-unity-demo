using System;
using Base.Config;
using Buffers;
using Newtonsoft.Json.Linq;
using Tools.Json;


namespace Base.Data.Operations.Fee
{
    public sealed class CommitteeMemberUpdateGlobalParametersOperationFeeParametersData : FeeParametersData
    {
        private const string FEE_FIELD_KEY = "fee";


        public ulong Fee { get; set; }

        public override ChainTypes.FeeParameters Type => ChainTypes.FeeParameters.CommitteeMemberUpdateGlobalParametersOperation;

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

        public static CommitteeMemberUpdateGlobalParametersOperationFeeParametersData Create(JObject value)
        {
            var token = value.Root;
            var instance = new CommitteeMemberUpdateGlobalParametersOperationFeeParametersData();
            instance.Fee = Convert.ToUInt64(value.TryGetValue(FEE_FIELD_KEY, out token) ? token.ToObject<object>() : 0);
            return instance;
        }
    }
}