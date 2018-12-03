using Newtonsoft.Json;


namespace Base.Data
{
    // id "2.16.x"
    public sealed class FbaAccumulatorObject : IdObject
    {
        [JsonProperty("accumulated_fba_fees")]
        public long AccumulatedFbaFees { get; private set; }
        [JsonProperty("designated_asset", NullValueHandling = NullValueHandling.Ignore)]
        public SpaceTypeId DesignatedAsset { get; private set; }
    }
}