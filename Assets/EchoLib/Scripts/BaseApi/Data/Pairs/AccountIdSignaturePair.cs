using Base.Data.Json;
using Newtonsoft.Json;


namespace Base.Data.Pairs
{
    [JsonConverter(typeof(AccountIdSignaturePairConverter))]
    public sealed class AccountIdSignaturePair : Pair<SpaceTypeId, string>
    {
        public AccountIdSignaturePair(SpaceTypeId account, string signature) : base(account, signature) { }
    }
}