using Base.Data.Json;
using Newtonsoft.Json;


namespace Base.Data.Pairs
{
    [JsonConverter(typeof(AccountIdSignaturePairConverter))]
    public class AccountIdSignaturePair
    {
        public SpaceTypeId Account { get; private set; }
        public string Signature { get; private set; }

        public AccountIdSignaturePair(SpaceTypeId account, string signature)
        {
            Account = account;
            Signature = signature;
        }
    }
}