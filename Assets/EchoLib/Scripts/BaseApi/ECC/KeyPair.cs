using System;
using CustomTools.Extensions.Core;
using Tools.Assert;


namespace Base.ECC
{
    public class KeyPair : IEquatable<KeyPair>, IEquatable<PublicKey>
    {
        private readonly PrivateKey privateKey = null;


        private KeyPair() { }

        public KeyPair(string role, string userName, string password)
        {
            privateKey = PrivateKey.FromSeed(userName.Trim() + role.Trim() + password.Trim()); // args order very important!
        }

        public KeyPair(PrivateKey privateKey, string associatePublicKey = null)
        {
            this.privateKey = privateKey;
            if (!associatePublicKey.IsNull())
            {
                Assert.Equal(associatePublicKey, Public.ToString(), "Associate public key doesn't equal with generated public key");
            }
        }

        public bool Equals(KeyPair otherKeyPair) => Equals(otherKeyPair.Public);

        public bool Equals(PublicKey publicKey) => Public.Equals(publicKey);

        public PrivateKey Private => privateKey;

        public PublicKey Public => privateKey.ToPublicKey();
    }
}