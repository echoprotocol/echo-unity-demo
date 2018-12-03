using Buffers;
using Base.Config;
using Base.Data.Json;
using Newtonsoft.Json;


namespace Base.Data.Workers
{
    [JsonConverter(typeof(WorkerDataPairConverter))]
    public abstract class WorkerData : SerializableObject, ISerializeToBuffer
    {
        public abstract ChainTypes.Worker Type { get; }
        public abstract ByteBuffer ToBufferRaw(ByteBuffer buffer = null);

        public ByteBuffer ToBuffer(ByteBuffer buffer = null)
        {
            buffer = buffer ?? new ByteBuffer(ByteBuffer.LITTLE_ENDING);
            buffer.WriteVarInt32((int)Type);
            return ToBufferRaw(buffer);
        }

        public WorkerData Clone()
        {
            return (WorkerData)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(this, new WorkerDataPairConverter()), GetType());
        }
    }
}