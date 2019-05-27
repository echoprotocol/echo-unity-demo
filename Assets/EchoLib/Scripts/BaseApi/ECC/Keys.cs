using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Data;
using Base.Data.Accounts;
using ED25519REF10;
using Tools.HexBinDec;

namespace Base.ECC
{
    public class Keys : IDisposable
    {
        private const string OWNER_KEY = "owner";
        private const string ACTIVE_KEY = "active";
        private const string MEMO_KEY = "memo";

        private readonly Dictionary<AccountRole, KeyPair> keys = new Dictionary<AccountRole, KeyPair>();


        private Keys() { }

        private Keys(Dictionary<AccountRole, KeyPair> keys)
        {
            this.keys = keys;
        }

        public static Keys FromSeed(string userName, string password, bool activeRewriteMemo = true)
        {
            var keys = new Dictionary<AccountRole, KeyPair>();
            var roles = new[] { AccountRole.Owner, AccountRole.Active, AccountRole.Memo };
            foreach (var role in roles)
            {
                if (role.Equals(AccountRole.Memo) && activeRewriteMemo)
                {
                    keys[role] = new KeyPair(GetRole(AccountRole.Active), userName, password);
                }
                else
                {
                    keys[role] = new KeyPair(GetRole(role), userName, password);
                }
            }
            return new Keys(keys);
        }

        public PrivateKey this[PublicKey publicKey]
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

        public PublicKey this[AccountRole role] => keys.ContainsKey(role) ? keys[role].Public : null;

        public int Count => keys.Count;

        public PublicKey[] PublicKeys
        {
            get
            {
                var result = new List<PublicKey>();
                foreach (var keyPair in keys.Values)
                {
                    result.Add(keyPair.Public);
                }
                return result.ToArray();
            }
        }

        public string EchoRandKey(AccountRole role)
        {
            var key = keys.ContainsKey(role) ? keys[role].Private : null;
            if (key == null)
            {
                return string.Empty;
            }
            return ED25519.DerivePublicKey(key.ToBuffer()).ToHexString();
        }

        private Keys CheckAuthorization(AccountObject account)
        {
            if (account == null)
            {
                return null;
            }
            var result = new Dictionary<AccountRole, KeyPair>();
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

        public void Dispose() => keys.Clear();

        private static string GetRole(AccountRole role)
        {
            if (role.Equals(AccountRole.Owner))
            {
                return OWNER_KEY;
            }
            if (role.Equals(AccountRole.Active))
            {
                return ACTIVE_KEY;
            }
            if (role.Equals(AccountRole.Memo))
            {
                return MEMO_KEY;
            }
            return string.Empty;
        }
    }
}