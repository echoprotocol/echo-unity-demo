using Newtonsoft.Json;


namespace Base.Data
{
    // id "2.18.x"
    public sealed class ContractBalanceObject : IdObject
    {
        [JsonProperty("owner")]
        public SpaceTypeId Owner { get; private set; }
        [JsonProperty("asset_type")]
        public SpaceTypeId Asset { get; private set; }
        [JsonProperty("balance")]
        public long Balance { get; private set; }
    }
}