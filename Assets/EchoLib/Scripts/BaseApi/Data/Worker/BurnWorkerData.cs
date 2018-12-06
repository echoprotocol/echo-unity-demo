using Base.Config;
using Buffers;
using Newtonsoft.Json.Linq;
using Tools.Json;


namespace Base.Data.Workers
{
    public sealed class BurnWorkerData : WorkerData
    {
        private const string TOTAL_BURNED_FIELD_KEY = "total_burned";


        public long TotalBurned { get; set; }

        public override ChainTypes.Worker Type => ChainTypes.Worker.Burn;

        public override ByteBuffer ToBufferRaw(ByteBuffer buffer = null)
        {
            buffer = buffer ?? new ByteBuffer(ByteBuffer.LITTLE_ENDING);
            buffer.WriteInt64(TotalBurned);
            return buffer;
        }

        public override string Serialize()
        {
            return new JsonBuilder(new JsonDictionary {
                { TOTAL_BURNED_FIELD_KEY,    TotalBurned }
            }).Build();
        }

        public static BurnWorkerData Create(JObject value)
        {
            var token = value.Root;
            var instance = new BurnWorkerData();
            instance.TotalBurned = value.TryGetValue(TOTAL_BURNED_FIELD_KEY, out token) ? token.ToObject<long>() : 0;
            return instance;
        }
    }
}