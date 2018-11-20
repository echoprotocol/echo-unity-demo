using System;
using Base.ECC;


namespace Base.Data.Json
{
    public sealed class AddressConverter : JsonCustomConverter<Address, string>
    {
        protected override Address Deserialize(string value, Type objectType) => ConvertFrom(value);

        protected override string Serialize(Address value) => ConvertTo(value);

        private static string ConvertTo(Address value) => value.ToString();

        private static Address ConvertFrom(string value) => Address.FromAddressString(value);
    }
}