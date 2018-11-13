using System;
using Base.ECC;


namespace Base.Data.Json
{
    public sealed class PublicKeyConverter : JsonCustomConverter<PublicKey, string>
    {
        protected override PublicKey Deserialize(string value, Type objectType) => PublicKey.FromPublicKeyString(value);

        protected override string Serialize(PublicKey value) => value.ToString();
    }
}