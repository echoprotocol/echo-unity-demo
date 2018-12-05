using Base.Data.Json;
using Base.ECC;
using Newtonsoft.Json;


namespace Base.Data.Pairs
{
    [JsonConverter(typeof(PublicKeyWeightPairConverter))]
    public sealed class PublicKeyWeightPair : Pair<PublicKey, ushort>
    {
        public PublicKeyWeightPair(PublicKey publicKey, ushort weight) : base(publicKey, weight) { }
   
        public bool IsEquelKey(KeyPair key) => key.Equals(Key);
    }
}