using Newtonsoft.Json;


namespace Base.Data.Witnesses
{
    // id "2.12.x"
    public sealed class WitnessScheduleObject : IdObject
    {
        [JsonProperty("current_shuffled_witnesses")]
        public SpaceTypeId[] CurrentShuffledWitnesses { get; private set; }
    }
}