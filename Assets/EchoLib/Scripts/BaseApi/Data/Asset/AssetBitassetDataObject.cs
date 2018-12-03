using System;
using Base.Data.Json;
using Newtonsoft.Json;


namespace Base.Data.Assets
{
    // id "2.4.x"
    public sealed class AssetBitassetDataObject : IdObject
    {
        [JsonProperty("options")]
        public BitassetOptionsData Options { get; private set; }
        [JsonProperty("feeds")]
        public object[] Feeds { get; private set; }                        // flat_map<account_id_type, pair<time_point_sec, price_feed>>
        [JsonProperty("current_feed")]
        public PriceFeedData CurrentFeed { get; private set; }
        [JsonProperty("current_feed_publication_time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime CurrentFeedPublicationTime { get; private set; }
        [JsonProperty("is_prediction_market")]
        public bool IsPredictionMarket { get; private set; }
        [JsonProperty("force_settled_volume")]
        public long ForceSettledVolume { get; private set; }
        [JsonProperty("settlement_price")]
        public PriceData SettlementPrice { get; private set; }
        [JsonProperty("settlement_fund")]
        public long SettlementFund { get; private set; }
    }
}