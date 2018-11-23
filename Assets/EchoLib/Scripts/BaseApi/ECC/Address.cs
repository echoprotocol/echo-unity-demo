using System;
using System.Security.Cryptography;
using Base.Config;
using Base.Data;
using Base.Data.Json;
using Buffers;
using CustomTools.Extensions.Core;
using CustomTools.Extensions.Core.Array;
using Newtonsoft.Json;
using SimpleBase;
using Tools.Assert;
using Tools.Hash;
using Tools.HexBinDec;


namespace Base.ECC
{
    // todo check
    [JsonConverter(typeof(AddressConverter))]
    public sealed class Address : ISerializeToBuffer, IEquatable<Address>, IComparable<Address>
    {
        private readonly byte[] addy = new byte[0];


        private Address() { }

        private Address(byte[] addy)
        {
            this.addy = addy;
        }

        public static Address FromBuffer(byte[] buffer)
        {
            var hash = SHA512.Create().HashAndDispose(buffer);
            var addy = RIPEMD160.Create().HashAndDispose(hash);
            return new Address(addy);
        }

        private byte[] ToArray() => addy;

        public override string ToString() => ToAddressString();

        public string ToAddressString(string addressPrefix = null)
        {
            if (addressPrefix.IsNull())
            {
                addressPrefix = ChainConfig.AddressPrefix;
            }
            var buffer = ToArray();
            var checksum = RIPEMD160.Create().HashAndDispose(buffer);
            buffer = buffer.Concat(checksum.Slice(0, 4));
            return addressPrefix + Base58.Encode(buffer);
        }

        public static Address FromAddressString(string address, string addressPrefix = null)
        {
            try
            {
                return FromStringOrThrow(address, addressPrefix);
            }
            catch
            {
                return null;
            }
        }

        private static Address FromStringOrThrow(string address, string addressPrefix = null)
        {
            if (addressPrefix.IsNull())
            {
                addressPrefix = ChainConfig.AddressPrefix;
            }
            var prefix = address.Substring(0, addressPrefix.Length);

            Assert.Equal(
                addressPrefix, prefix,
                string.Format("Expecting key to begin with {0}, instead got {1}", addressPrefix, prefix)
            );

            address = address.Substring(addressPrefix.Length);
            var addy = Base58.Decode(address);

            var checksum = addy.Slice(addy.Length - 4);
            addy = addy.Slice(0, addy.Length - 4);

            var newChecksum = RIPEMD160.Create().HashAndDispose(addy);
            newChecksum = newChecksum.Slice(0, 4);

            var isEqual = checksum.DeepEqual(newChecksum); // invalid checksum
            if (!isEqual)
            {
                throw new InvalidOperationException("Checksum did not match");
            }
            return new Address(addy);
        }

        // return Address - Compressed PTS format (by default)
        private static Address FromPublicKey(PublicKey publicKey, bool compressed = true, byte version = 56)
        {
            var hash = SHA256.Create().HashAndDispose(publicKey.ToBuffer(compressed));
            hash = RIPEMD160.Create().HashAndDispose(hash);
            var addr = new [] { (byte)(0xFF & version) }.Concat(hash);
            var check = SHA256.Create().HashAndDispose(addr);
            check = SHA256.Create().HashAndDispose(check);
            var buffer = addr.Concat(check.Slice(0, 4));
            return new Address(RIPEMD160.Create().HashAndDispose(buffer));
        }

        public static Address FromHex(string hexString) => FromBuffer(hexString.FromHex2Data());

        public string ToHex() => ToArray().ToHexString();

        public override int GetHashCode() => ToString().GetHashCode();

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!(obj is Address))
            {
                return false;
            }
            return Equals((Address)obj);
        }

        public bool Equals(Address other) => ToString().Equals(other.ToNullableString());

        public int CompareTo(Address other)
        {
            return string.Compare(ToAddressString(), other.ToAddressString(), StringComparison.Ordinal);
        }

        public static int Compare(Address a, Address b) => a.CompareTo(b);

        public ByteBuffer ToBuffer(ByteBuffer buffer = null)
        {
            return (buffer ?? new ByteBuffer(ByteBuffer.LITTLE_ENDING)).WriteBytes(ToArray(), false);
        }
    }
}