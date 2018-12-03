using Base.Config;
using Buffers;
using Newtonsoft.Json.Linq;
using Tools.Json;


namespace Base.Data.Workers
{
    public sealed class VestingBalanceWorkerData : WorkerData
    {
        private const string BALANCE_FIELD_KEY = "balance";


        public SpaceTypeId Balance { get; set; }

        public override ChainTypes.Worker Type => ChainTypes.Worker.VestingBalance;

        public override ByteBuffer ToBufferRaw(ByteBuffer buffer = null)
        {
            buffer = buffer ?? new ByteBuffer(ByteBuffer.LITTLE_ENDING);
            Balance.ToBuffer(buffer);
            return buffer;
        }

        public override string Serialize()
        {
            return new JsonBuilder(new JsonDictionary {
                { BALANCE_FIELD_KEY,    Balance }
            }).Build();
        }

        public static VestingBalanceWorkerData Create(JObject value)
        {
            var token = value.Root;
            var instance = new VestingBalanceWorkerData();
            instance.Balance = value.TryGetValue(BALANCE_FIELD_KEY, out token) ? token.ToObject<SpaceTypeId>() : SpaceTypeId.EMPTY;
            return instance;
        }
    }
}