using System;
using Base.Config;
using Buffers;
using Newtonsoft.Json.Linq;
using Tools.Json;


namespace Base.Data.Operations
{
    public sealed class ContractOperationData : OperationData
    {
        private const string FEE_FIELD_KEY = "fee";
        private const string REGISTRAR_FIELD_KEY = "registrar";
        private const string RECEIVER_FIELD_KEY = "receiver";
        private const string ASSET_ID_FIELD_KEY = "asset_id";
        private const string VALUE_FIELD_KEY = "value";
        private const string GAS_PRICE_FIELD_KEY = "gasPrice";
        private const string GAS_FIELD_KEY = "gas";
        private const string CODE_FIELD_KEY = "code";


        public override AssetData Fee { get; set; }
        public SpaceTypeId Registrar { get; set; }
        public SpaceTypeId Receiver { get; set; }
        public SpaceTypeId Asset { get; set; }
        public ulong Value { get; set; }
        public ulong GasPrice { get; set; }
        public ulong Gas { get; set; }
        public string Code { get; set; }

        public override ChainTypes.Operation Type => ChainTypes.Operation.Contract;

        public override ByteBuffer ToBufferRaw(ByteBuffer buffer = null)
        {
            buffer = buffer ?? new ByteBuffer(ByteBuffer.LITTLE_ENDING);
            Fee.ToBuffer(buffer);
            Registrar.ToBuffer(buffer);
            buffer.WriteOptionalClass(Receiver, (b, value) => value.ToBuffer(b));
            Asset.ToBuffer(buffer);
            buffer.WriteUInt64(Value);
            buffer.WriteUInt64(GasPrice);
            buffer.WriteUInt64(Gas);
            buffer.WriteString(Code);
            return buffer;
        }

        public override string Serialize()
        {
            var builder = new JsonBuilder();
            builder.WriteKeyValuePair(FEE_FIELD_KEY, Fee);
            builder.WriteKeyValuePair(REGISTRAR_FIELD_KEY, Registrar);
            builder.WriteOptionalClassKeyValuePair(RECEIVER_FIELD_KEY, Receiver);
            builder.WriteKeyValuePair(ASSET_ID_FIELD_KEY, Asset);
            builder.WriteKeyValuePair(VALUE_FIELD_KEY, Value);
            builder.WriteKeyValuePair(GAS_PRICE_FIELD_KEY, GasPrice);
            builder.WriteKeyValuePair(GAS_FIELD_KEY, Gas);
            builder.WriteKeyValuePair(CODE_FIELD_KEY, Code);
            return builder.Build();
        }

        public static ContractOperationData Create(JObject value)
        {
            var token = value.Root;
            var instance = new ContractOperationData();
            instance.Fee = value.TryGetValue(FEE_FIELD_KEY, out token) ? token.ToObject<AssetData>() : AssetData.EMPTY;
            instance.Registrar = value.TryGetValue(REGISTRAR_FIELD_KEY, out token) ? token.ToObject<SpaceTypeId>() : SpaceTypeId.EMPTY;
            instance.Receiver = value.TryGetValue(RECEIVER_FIELD_KEY, out token) ? token.ToObject<SpaceTypeId>() : null;  // optional
            instance.Asset = value.TryGetValue(ASSET_ID_FIELD_KEY, out token) ? token.ToObject<SpaceTypeId>() : SpaceTypeId.EMPTY;
            instance.Value = Convert.ToUInt64(value.TryGetValue(VALUE_FIELD_KEY, out token) ? token.ToObject<object>() : 0);
            instance.GasPrice = Convert.ToUInt64(value.TryGetValue(GAS_PRICE_FIELD_KEY, out token) ? token.ToObject<object>() : 0);
            instance.Gas = Convert.ToUInt64(value.TryGetValue(GAS_FIELD_KEY, out token) ? token.ToObject<object>() : 0);
            instance.Code = value.TryGetValue(CODE_FIELD_KEY, out token) ? token.ToObject<string>() : string.Empty;
            return instance;
        }
    }
}