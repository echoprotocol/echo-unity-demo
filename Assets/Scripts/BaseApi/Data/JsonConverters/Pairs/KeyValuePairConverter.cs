using Base.Data.Pairs;
using CustomTools.Extensions.Core;
using Newtonsoft.Json.Linq;


namespace Base.Data.Json
{
    public sealed class KeyValuePairConverter : PairConverter<KeyValuePair>
    {
        protected override KeyValuePair ConvertFrom(JToken key, JToken value)
        {
            return new KeyValuePair(key.ToNullableString(), value.ToNullableString());
        }

        protected override JArray ConvertTo(KeyValuePair pair)
        {
            return new JArray(JToken.FromObject(pair.Key), pair.Value);
        }
    }
}