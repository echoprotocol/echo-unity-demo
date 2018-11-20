using Base.Data.Pairs;
using CustomTools.Extensions.Core;
using Newtonsoft.Json.Linq;


namespace Base.Data.Json
{
    public sealed class UserNameAccountIdPairConverter : PairConverter<UserNameAccountIdPair>
    {
        protected override UserNameAccountIdPair ConvertFrom(JToken key, JToken value)
        {
            return new UserNameAccountIdPair(key.ToNullableString(), value.ToObject<SpaceTypeId>());
        }

        protected override JArray ConvertTo(UserNameAccountIdPair pair)
        {
            return new JArray(pair.UserName, JToken.FromObject(pair.Id));
        }
    }
}