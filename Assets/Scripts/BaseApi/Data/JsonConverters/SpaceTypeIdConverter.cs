using System;
using Newtonsoft.Json.Linq;


namespace Base.Data.Json
{
    public sealed class SpaceTypeIdConverter : JsonCustomConverter<SpaceTypeId, JToken>
    {
        protected override SpaceTypeId Deserialize(JToken value, Type objectType) => ConvertFrom(value);

        protected override JToken Serialize(SpaceTypeId value) => ConvertTo(value);

        private static JToken ConvertTo(SpaceTypeId spaceTypeId)
        {
            return SpaceTypeId.EMPTY.Equals(spaceTypeId) ? JToken.FromObject(0) : JToken.FromObject(spaceTypeId.ToString());
        }

        private static SpaceTypeId ConvertFrom(JToken value)
        {
            return value.Type.Equals(JTokenType.String) ? SpaceTypeId.Create(value.ToString()) : SpaceTypeId.EMPTY;
        }
    }
}