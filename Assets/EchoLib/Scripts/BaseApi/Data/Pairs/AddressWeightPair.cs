using Base.Data.Json;
using Newtonsoft.Json;


namespace Base.Data.Pairs
{
    [JsonConverter(typeof(AddressWeightPairConverter))]
    public class AddressWeightPair
    {
        public string Address { get; private set; }
        public ushort Weight { get; private set; }

        public AddressWeightPair(string address, ushort weight)
        {
            Address = address;
            Weight = weight;
        }
    }
}