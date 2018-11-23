using Base.Data.Json;
using Newtonsoft.Json;


namespace Base.Data.Pairs
{
    [JsonConverter(typeof(AccountIdWeightPairConverter))]
    public class AccountIdWeightPair
    {
        public SpaceTypeId Account { get; private set; }
        public ushort Weight { get; private set; }

        public AccountIdWeightPair(SpaceTypeId account, ushort weight)
        {
            Account = account;
            Weight = weight;
        }
    }
}