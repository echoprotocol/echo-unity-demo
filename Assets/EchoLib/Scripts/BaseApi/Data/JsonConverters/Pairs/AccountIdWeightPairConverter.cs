using System;
using Base.Data.Pairs;
using Newtonsoft.Json.Linq;


namespace Base.Data.Json
{
    public sealed class AccountIdWeightPairConverter : PairConverter<AccountIdWeightPair>
    {
        protected override AccountIdWeightPair ConvertFrom(JToken key, JToken value)
        {
            return new AccountIdWeightPair(key.ToObject<SpaceTypeId>(), Convert.ToUInt16(value));
        }

        protected override JArray ConvertTo(AccountIdWeightPair pair)
        {
            return new JArray(JToken.FromObject(pair.Account), pair.Weight);
        }
    }
}