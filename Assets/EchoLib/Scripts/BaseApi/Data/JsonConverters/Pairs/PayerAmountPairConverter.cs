using System;
using Base.Data.Pairs;
using Newtonsoft.Json.Linq;


namespace Base.Data.Json
{
    public sealed class PayerAmountPairConverter : PairConverter<PayerAmountPair>
    {
        protected override PayerAmountPair ConvertFrom(JToken key, JToken value)
        {
            return new PayerAmountPair(key.ToObject<SpaceTypeId>(), Convert.ToInt64(value));
        }

        protected override JArray ConvertTo(PayerAmountPair pair)
        {
            return new JArray(JToken.FromObject(pair.Payer), pair.Amount);
        }
    }
}