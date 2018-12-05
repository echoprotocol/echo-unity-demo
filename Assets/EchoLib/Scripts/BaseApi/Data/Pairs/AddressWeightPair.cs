using Base.Data.Json;
using Newtonsoft.Json;


namespace Base.Data.Pairs
{
    [JsonConverter(typeof(AddressWeightPairConverter))]
    public sealed class AddressWeightPair : Pair<string, ushort>
    {
        public AddressWeightPair(string address, ushort weight) : base(address, weight) { }
    }
}