using Newtonsoft.Json;


namespace Base.Data.Accounts
{
    public sealed class FullAccountData : SerializableObject
    {
        [JsonProperty("account")]
        public AccountObject Account { get; set; }
        [JsonProperty("statistics")]
        public AccountStatisticsObject Statistics { get; set; }
        [JsonProperty("registrar_name")]
        public string RegistrarName { get; set; }
        [JsonProperty("referrer_name")]
        public string ReferrerName { get; set; }
        [JsonProperty("lifetime_referrer_name")]
        public string LifetimeReferrerName { get; set; }
        [JsonProperty("votes")]
        public object[] Votes { get; set; }                         // todo
        [JsonProperty("cashback_balance", NullValueHandling = NullValueHandling.Ignore)]
        public object CashbackBalance { get; set; }                 // todo
        [JsonProperty("balances")]
        public AccountBalanceObject[] Balances { get; set; }
        [JsonProperty("vesting_balances")]
        public object[] VestingBalances { get; set; }               // todo
        [JsonProperty("limit_orders")]
        public object[] LimitOrders { get; set; }                   // todo
        [JsonProperty("call_orders")]
        public object[] CallOrders { get; set; }                    // todo
        [JsonProperty("proposals")]
        public object[] Proposals { get; set; }                     // todo
        [JsonProperty("pending_dividend_payments")]
        public object[] PendingDividendPayments { get; set; }       // todo
    }
}