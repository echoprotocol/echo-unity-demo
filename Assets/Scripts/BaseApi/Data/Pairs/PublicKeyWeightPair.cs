using Base.Data.Json;
using Base.ECC;
using Newtonsoft.Json;


namespace Base.Data.Pairs
{
    [JsonConverter(typeof(PublicKeyWeightPairConverter))]
    public class PublicKeyWeightPair
    {
        public PublicKey PublicKey { get; private set; }
        public ushort Weight { get; private set; }

        public PublicKeyWeightPair(PublicKey publicKey, ushort weight)
        {
            PublicKey = publicKey;
            Weight = weight;
        }

        public bool IsEquelKey(KeyPair key)
        {
            return key.Equals(PublicKey);
        }
    }
}