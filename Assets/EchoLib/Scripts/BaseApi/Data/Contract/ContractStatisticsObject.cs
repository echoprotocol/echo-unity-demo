using Newtonsoft.Json;


namespace Base.Data.Contract
{
    // id "2.20.x"
    public sealed class ContractStatisticsObject : IdObject
    {
        [JsonProperty("owner")]
        public SpaceTypeId Owner { get; private set; }
        [JsonProperty("most_recent_op")]
        public SpaceTypeId MostRecentOp { get; private set; }
        [JsonProperty("total_ops")]
        public uint TotalOps { get; private set; }
    }
}