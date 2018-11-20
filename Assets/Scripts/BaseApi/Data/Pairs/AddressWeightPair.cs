using Base.Data.Json;
using Base.ECC;
using Newtonsoft.Json;


namespace Base.Data.Pairs
{
    [JsonConverter(typeof(AddressWeightPairConverter))]
    public class AddressWeightPair
    {
        public Address Address { get; private set; }
        public ushort Weight { get; private set; }

        public AddressWeightPair(Address address, ushort weight)
        {
            Address = address;
            Weight = weight;
        }
    }
}