using Newtonsoft.Json;


namespace Base.Data
{
    // id "1.17.x"
    public sealed class ResultContractObject : IdObject
    {
        [JsonProperty("contracts_id")]
        public SpaceTypeId[] Contracts { get; private set; }
    }
}