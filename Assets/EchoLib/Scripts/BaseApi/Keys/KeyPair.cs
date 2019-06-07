using System;
using System.Text;
using Buffers;
using CustomTools.Extensions.Core;
using Tools.Assert;


namespace Base.Keys
{
    public class KeyPair : IEquatable<KeyPair>, IEquatable<IPublicKey>, IDisposable
    {
        private readonly IPrivateKey privateKey = null;


        private KeyPair() { }

        public KeyPair(string role, string userName, string password, IPrivateKeyFactory factory)
        {
            var buffer = new ByteBuffer(ByteBuffer.LITTLE_ENDING);
            var data = Encoding.UTF8.GetBytes(userName.Trim());
            buffer.WriteBytes(data, false);
            data.Clear();
            data = Encoding.UTF8.GetBytes(role.Trim());
            buffer.WriteBytes(data, false);
            data.Clear();
            data = Encoding.UTF8.GetBytes(password.Trim());
            data.Clear();
            var seed = buffer.ToArray();
            buffer.Dispose();
            privateKey = factory.FromSeed(seed);
            seed.Clear();
        }

        public KeyPair(IPrivateKey privateKey, string associatePublicKey = null)
        {
            this.privateKey = privateKey;
            if (!associatePublicKey.IsNull())
            {
                Assert.Equal(associatePublicKey, Public.ToString(), "Associate public key doesn't equal with generated public key");
            }
        }

        public void Dispose() => privateKey.Dispose();

        public bool Equals(KeyPair otherKeyPair) => Equals(otherKeyPair.Public);

        public bool Equals(IPublicKey publicKey) => Public.Equals(publicKey);

        public IPrivateKey Private => privateKey;

        public IPublicKey Public => privateKey.ToPublicKey();
    }
}