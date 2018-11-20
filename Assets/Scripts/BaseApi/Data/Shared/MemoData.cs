using System;
using Base.Data.Json;
using Base.ECC;
using Buffers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tools.Json;


namespace Base.Data
{
    [JsonConverter(typeof(MemoDataConverter))]
    public sealed class MemoData : ISerializeToBuffer
    {
        private const string FROM_FIELD_KEY = "from";
        private const string TO_FIELD_KEY = "to";
        private const string NONCE_FIELD_KEY = "nonce";
        private const string MESSAGE_FIELD_KEY = "message";


        public PublicKey From { get; set; }
        public PublicKey To { get; set; }
        public ulong Nonce { get; set; }
        public byte[] Message { get; set; }

        public ByteBuffer ToBuffer(ByteBuffer buffer = null)
        {
            buffer = buffer ?? new ByteBuffer(ByteBuffer.LITTLE_ENDING);
            From.ToBuffer(buffer);
            To.ToBuffer(buffer);
            buffer.WriteUInt64(Nonce);
            buffer.WriteBytes(Message);
            return buffer;
        }

        public string Serialize()
        {
            return new JsonBuilder(new JsonDictionary {
                { FROM_FIELD_KEY,       From },
                { TO_FIELD_KEY,         To },
                { NONCE_FIELD_KEY,      Nonce.ToString() },
                { MESSAGE_FIELD_KEY,    Message }
            }).Build();
        }

        public override string ToString() => Serialize();

        public static MemoData Create(JObject value)
        {
            var token = value.Root;
            var instance = new MemoData();
            instance.From = value.TryGetValue(FROM_FIELD_KEY, out token) ? token.ToObject<PublicKey>() : null;
            instance.To = value.TryGetValue(TO_FIELD_KEY, out token) ? token.ToObject<PublicKey>() : null;
            instance.Nonce = Convert.ToUInt64(value.TryGetValue(NONCE_FIELD_KEY, out token) ? token.ToObject<object>() : 0);
            instance.Message = value.TryGetValue(MESSAGE_FIELD_KEY, out token) ? token.ToObject<byte[]>(new ByteArrayConverter().GetSerializer()) : new byte[0];
            return instance;
        }
    }
}