using System;
using Newtonsoft.Json.Linq;


namespace Base.Data.Json
{
    public sealed class VoteIdConverter : JsonCustomConverter<VoteId, JToken>
    {
        protected override VoteId Deserialize(JToken value, Type objectType) => ConvertFrom(value);

        protected override JToken Serialize(VoteId value) => ConvertTo(value);

        private static JToken ConvertTo(VoteId voteId)
        {
            return VoteId.EMPTY.Equals(voteId) ? JToken.FromObject(0) : JToken.FromObject(voteId.ToString());
        }

        private static VoteId ConvertFrom(JToken value)
        {
            return value.Type.Equals(JTokenType.String) ? VoteId.Create(value.ToString()) : VoteId.EMPTY;
        }
    }
}