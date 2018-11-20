using Base.ECC;
using Newtonsoft.Json;


namespace Base.Data.Witnesses
{
    // id "1.6.x"
    public sealed class WitnessObject : IdObject
    {
        [JsonProperty("witness_account")]
        public SpaceTypeId WitnessAccount { get; private set; }
        [JsonProperty("last_aslot")]
        public ulong LastAslot { get; private set; }
        [JsonProperty("signing_key")]
        public PublicKey SigningKey { get; private set; }
        [JsonProperty("next_secret_hash")]
        public string NextSecretHash { get; private set; }
        [JsonProperty("previous_secret")]
        public string PreviousSecret { get; private set; }
        [JsonProperty("pay_vb", NullValueHandling = NullValueHandling.Ignore)]
        public SpaceTypeId PayVestingBalance { get; private set; }
        [JsonProperty("vote_id")]
        public VoteId Vote { get; private set; }
        [JsonProperty("total_votes")]
        public ulong TotalVotes { get; private set; }
        [JsonProperty("url")]
        public string Url { get; private set; }
        [JsonProperty("total_missed")]
        public long TotalMissed { get; private set; }
        [JsonProperty("last_confirmed_block_num")]
        public uint LastConfirmedBlockNum { get; private set; }
    }
}