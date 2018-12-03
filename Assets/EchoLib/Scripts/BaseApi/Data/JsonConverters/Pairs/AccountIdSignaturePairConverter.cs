using Base.Data.Pairs;
using Newtonsoft.Json.Linq;


namespace Base.Data.Json
{
    public sealed class AccountIdSignaturePairConverter : PairConverter<AccountIdSignaturePair>
    {
        protected override AccountIdSignaturePair ConvertFrom(JToken key, JToken value)
        {
            return new AccountIdSignaturePair(key.ToObject<SpaceTypeId>(), value.ToString());
        }

        protected override JArray ConvertTo(AccountIdSignaturePair pair)
        {
            return new JArray(JToken.FromObject(pair.Account), pair.Signature);
        }
    }
}