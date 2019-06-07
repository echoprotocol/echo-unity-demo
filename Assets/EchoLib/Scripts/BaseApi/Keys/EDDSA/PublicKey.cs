using System;
using System.Security.Cryptography;
using Base.Config;
using Base.Data.Json;
using BigI;
using Buffers;
using CustomTools.Extensions.Core;
using CustomTools.Extensions.Core.Array;
using ED25519REF10;
using Newtonsoft.Json;
using SimpleBase;
using Tools.Assert;
using Tools.Hash;
using Tools.HexBinDec;


namespace Base.Keys.EDDSA
{
    [JsonConverter(typeof(EDPublicKeyConverter))]
    public sealed class PublicKey : IPublicKey
    {
        private readonly BigInteger d = null;


        private PublicKey() { }

        private PublicKey(BigInteger d)
        {
            this.d = d;
        }

        public void Dispose() => d.Dispose();

        private static PublicKey FromBuffer(byte[] buffer)
        {
            if (buffer.ToHexString().Equals(new string('0', 33 * 2)))
            {
                return new PublicKey(null);
            }
            return new PublicKey(BigInteger.FromBuffer(buffer));
        }

        public byte[] ToBuffer()
        {
            if (d.IsNull())
            {
                return new byte[33].Fill((byte)0x00);
            }
            return d.ToBuffer(32);
        }

        public static PublicKey DerivePublicKey(PrivateKey k)
        {
            var buffer = k.ToBuffer();
            var key = ED25519.DerivePublicKey(buffer);
            buffer.Clear();
            var result = FromBuffer(key);
            key.Clear();
            return result;
        }

        public override string ToString() => ToPublicKeyString();

        public string ToPublicKeyString(string addressPrefix = null)
        {
            if (addressPrefix.IsNull())
            {
                addressPrefix = ChainConfig.AddressPrefix;
            }
            var buffer = ToBuffer();
            var hash = RIPEMD160.Create().HashAndDispose(buffer);
            var checksum = hash.Slice(0, 4);
            hash.Clear();
            var key = buffer.Concat(checksum);
            buffer.Clear();
            checksum.Clear();
            var result = addressPrefix + Base58.Encode(key);
            key.Clear();
            return result;
        }

        public static IPublicKey FromPublicKeyString(string publicKey, string addressPrefix = null)
        {
            try
            {
                if (addressPrefix.IsNull())
                {
                    addressPrefix = ChainConfig.AddressPrefix;
                }
                var prefix = publicKey.Substring(0, addressPrefix.Length);
                Assert.Equal(
                    addressPrefix, prefix,
                    string.Format("Expecting key to begin with {0}, instead got {1}", addressPrefix, prefix)
                );
                publicKey = publicKey.Substring(addressPrefix.Length);
                var key = Base58.Decode(publicKey);
                var checksum = key.Slice(key.Length - 4);
                var buffer = key.Slice(0, key.Length - 4);
                key.Clear();
                var hash = RIPEMD160.Create().HashAndDispose(buffer);
                var newChecksum = hash.Slice(0, 4);
                hash.Clear();
                if (!checksum.DeepEqual(newChecksum))
                {
                    throw new InvalidOperationException("Checksum did not match");
                }
                checksum.Clear();
                newChecksum.Clear();
                var result = FromBuffer(buffer);
                buffer.Clear();
                return result;
            }
            catch
            {
                return null;
            }
        }

        public string ToAddressString(string addressPrefix = null)
        {
            if (addressPrefix.IsNull())
            {
                addressPrefix = ChainConfig.AddressPrefix;
            }
            var buffer = ToBuffer();
            var hash = SHA512.Create().HashAndDispose(buffer);
            buffer.Clear();
            var firstChecksum = RIPEMD160.Create().HashAndDispose(hash);
            hash.Clear();
            var secondChecksum = RIPEMD160.Create().HashAndDispose(firstChecksum);
            var checksum = secondChecksum.Slice(0, 4);
            secondChecksum.Clear();
            buffer = firstChecksum.Concat(checksum);
            firstChecksum.Clear();
            checksum.Clear();
            var result = addressPrefix + Base58.Encode(buffer);
            buffer.Clear();
            return result;
        }

        public string ToPtsAddy()
        {
            var buffer = ToBuffer();
            var firstHash = SHA256.Create().HashAndDispose(buffer);
            buffer.Clear();
            var secondHash = RIPEMD160.Create().HashAndDispose(firstHash);
            firstHash.Clear();
            var hash = new byte[] { 0x38 }.Concat(secondHash); // version 56(decimal)
            secondHash.Clear();
            var firstChecksum = SHA256.Create().HashAndDispose(hash);
            var secondChecksum = SHA256.Create().HashAndDispose(firstChecksum);
            firstChecksum.Clear();
            var checksum = secondChecksum.Slice(0, 4);
            secondChecksum.Clear();
            buffer = hash.Concat(checksum);
            checksum.Clear();
            hash.Clear();
            var result = Base58.Encode(buffer);
            buffer.Clear();
            return result;
        }

        public static IPublicKey FromHex(string hexString)
        {
            var data = hexString.FromHex2Data();
            var result = FromBuffer(data);
            data.Clear();
            return result;
        }

        public string ToHex()
        {
            var key = ToBuffer();
            var result = key.ToHexString();
            key.Clear();
            return result;
        }

        public override int GetHashCode() => ToString().GetHashCode();

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!(obj is IPublicKey))
            {
                return false;
            }
            return Equals((IPublicKey)obj);
        }

        public bool Equals(IPublicKey other) => ToString().Equals(other.ToNullableString());

        public int CompareTo(IPublicKey other)
        {
            return string.Compare(ToAddressString(), other.ToAddressString(), StringComparison.Ordinal);
        }

        public static int Compare(IPublicKey a, IPublicKey b) => a.CompareTo(b);

        public ByteBuffer ToBuffer(ByteBuffer buffer = null)
        {
            var key = ToBuffer();
            var result = (buffer ?? new ByteBuffer(ByteBuffer.LITTLE_ENDING)).WriteBytes(key, false);
            key.Clear();
            return result;
        }
    }
}