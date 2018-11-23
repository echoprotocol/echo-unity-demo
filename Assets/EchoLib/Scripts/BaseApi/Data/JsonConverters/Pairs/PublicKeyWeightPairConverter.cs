using System;
using Base.Data.Pairs;
using Base.ECC;
using Newtonsoft.Json.Linq;


namespace Base.Data.Json
{
    public sealed class PublicKeyWeightPairConverter : PairConverter<PublicKeyWeightPair>
    {
        protected override PublicKeyWeightPair ConvertFrom(JToken key, JToken value)
        {
            return new PublicKeyWeightPair(key.ToObject<PublicKey>(), Convert.ToUInt16(value));
        }

        protected override JArray ConvertTo(PublicKeyWeightPair pair)
        {
            return new JArray(JToken.FromObject(pair.PublicKey), pair.Weight);
        }
    }
}