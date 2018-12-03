using Newtonsoft.Json;


namespace Base.Data.Accounts
{
    // id "2.10.x"
    public sealed class BlindedBalanceObject : IdObject
    {
        [JsonProperty("commitment")]
        public string Commitment { get; set; }
        [JsonProperty("asset_id")]
        public SpaceTypeId Asset { get; set; }
        [JsonProperty("owner")]
        public AuthorityData Owner { get; set; }
    }
}