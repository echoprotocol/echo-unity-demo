using System;
using Base.Data.Json;
using Base.Data.Pairs;
using Base.Data.Transactions;
using Newtonsoft.Json;


namespace Base.Data.Block
{
    public class BlockHeaderData : SerializableObject
    {
        [JsonProperty("previous")]
        public string Previous { get; private set; }
        [JsonProperty("state_root_hash")]
        public string StateRootHash { get; private set; }
        [JsonProperty("result_root_hash")]
        public string ResultRootHash { get; private set; }
        [JsonProperty("timestamp"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; private set; }
        [JsonProperty("witness")]
        public SpaceTypeId Witness { get; private set; }
        [JsonProperty("account")]
        public SpaceTypeId Account { get; private set; }
        [JsonProperty("transaction_merkle_root")]
        public string TransactionMerkleRoot { get; private set; }
        [JsonProperty("extensions")]
        public object[] Extensions { get; private set; }
    }


    public class SignedBlockHeaderData : BlockHeaderData
    {
        [JsonProperty("witness_signature")]
        public string WitnessSignature { get; private set; }
        [JsonProperty("ed_signature")]
        public string EdSignature { get; private set; }
        [JsonProperty("verifications")]
        public AccountIdSignaturePair[] Verifications { get; private set; }
    }


    public sealed class SignedBlockData : SignedBlockHeaderData
    {
        [JsonProperty("transactions")]
        public ProcessedTransactionData[] Transactions { get; private set; }
    }
}