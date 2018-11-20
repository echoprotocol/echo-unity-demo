using Base.Data.Accounts;
using Base.Data.Pairs;
using CustomTools.Extensions.Core;
using Newtonsoft.Json.Linq;


namespace Base.Data.Json
{
    public sealed class UserNameFullAccountDataPairConverter : PairConverter<UserNameFullAccountDataPair>
    {
        protected override UserNameFullAccountDataPair ConvertFrom(JToken key, JToken value)
        {
            return new UserNameFullAccountDataPair(key.ToNullableString(), value.ToObject<FullAccountData>());
        }

        protected override JArray ConvertTo(UserNameFullAccountDataPair pair)
        {
            return new JArray(pair.UserName, JToken.FromObject(pair.FullAccount));
        }
    }
}