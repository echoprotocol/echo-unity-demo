using Newtonsoft.Json;


namespace Base.Data.Transactions
{
    public sealed class TransactionConfirmation : SerializableObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("block_num")]
        public uint BlockNumber { get; set; }
        [JsonProperty("trx_num")]
        public uint TransactionNumber { get; set; }
        [JsonProperty("trx")]
        public ProcessedTransactionData Transaction { get; set; }
    }
}