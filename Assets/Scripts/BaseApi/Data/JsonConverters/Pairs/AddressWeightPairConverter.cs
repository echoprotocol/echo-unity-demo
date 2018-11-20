using System;
using Base.Data.Pairs;
using Base.ECC;
using Newtonsoft.Json.Linq;


namespace Base.Data.Json
{
    public sealed class AddressWeightPairConverter : PairConverter<AddressWeightPair>
    {
        protected override AddressWeightPair ConvertFrom(JToken key, JToken value)
        {
            return new AddressWeightPair(key.ToObject<Address>(), Convert.ToUInt16(value));
        }

        protected override JArray ConvertTo(AddressWeightPair pair)
        {
            return new JArray(JToken.FromObject(pair.Address), pair.Weight);
        }
    }
}