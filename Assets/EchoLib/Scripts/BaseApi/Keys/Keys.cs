using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Data;
using Base.Data.Accounts;


namespace Base.Keys
{
    public class Keys : IDisposable
    {
        private const string ACTIVE_KEY = "active";
        private const string ECHORAND_KEY = "echorand";

        private readonly Dictionary<AuthorityClassification, KeyPair> keys = new Dictionary<AuthorityClassification, KeyPair>();


        private Keys() { }

        private Keys(Dictionary<AuthorityClassification, KeyPair> keys)
        {
            this.keys = keys;
        }

        public static Keys FromSeed(string userName, string password)
        {
            var keys = new Dictionary<AuthorityClassification, KeyPair>();
            var roles = new[] { AuthorityClassification.Active, AuthorityClassification.Echorand };
            foreach (var role in roles)
            {
                keys[role] = new KeyPair(GetRole(role), userName, password, EDDSA.KeyFactory.Create());
            }
            return new Keys(keys);
        }

        public IPrivateKey this[IPublicKey publicKey]
        {
            get
            {
                foreach (var keyPair in keys.Values)
                {
                    if (keyPair.Equals(publicKey))
                    {
                        return keyPair.Private;
                    }
                }
                return null;
            }
        }

        public IPublicKey this[AuthorityClassification role] => keys.ContainsKey(role) ? keys[role].Public : null;

        public int Count => keys.Count;

        public IPublicKey[] PublicKeys
        {
            get
            {
                var result = new List<IPublicKey>();
                foreach (var keyPair in keys.Values)
                {
                    result.Add(keyPair.Public);
                }
                return result.ToArray();
            }
        }

        private Keys CheckAuthorization(AccountObject account)
        {
            if (account == null)
            {
                return null;
            }
            var result = new Dictionary<AuthorityClassification, KeyPair>();
            foreach (var pair in keys)
            {
                if (account.IsEquelKey(pair.Key, pair.Value))
                {
                    result[pair.Key] = pair.Value;
                }
            }
            return (result.Count > 0) ? new Keys(result) : null;
        }

        public async Task<Keys> CheckAuthorizationAsync(AccountObject account)
        {
            return await Task.Run(() => CheckAuthorization(account));
        }

        public void Dispose()
        {
            foreach (var pair in keys)
            {
                pair.Value.Dispose();
            }
            keys.Clear();
        }

        private static string GetRole(AuthorityClassification role)
        {
            if (role.Equals(AuthorityClassification.Active))
            {
                return ACTIVE_KEY;
            }
            if (role.Equals(AuthorityClassification.Echorand))
            {
                return ECHORAND_KEY;
            }
            return string.Empty;
        }
    }
}