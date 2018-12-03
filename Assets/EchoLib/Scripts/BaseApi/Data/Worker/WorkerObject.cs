using System;
using Base.Data.Json;
using Newtonsoft.Json;


namespace Base.Data.Workers
{
    // id "1.14.x"
    public sealed class WorkerObject : IdObject
    {
        [JsonProperty("worker_account")]
        public SpaceTypeId WorkerAccount { get; private set; }
        [JsonProperty("work_begin_date"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime WorkBeginDate { get; private set; }
        [JsonProperty("work_end_date"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime WorkEndDate { get; private set; }
        [JsonProperty("daily_pay")]
        public long DailyPay { get; private set; }
        [JsonProperty("worker")]
        public WorkerData Worker { get; private set; }
        [JsonProperty("name")]
        public string Name { get; private set; }
        [JsonProperty("url")]
        public string Url { get; private set; }
        [JsonProperty("vote_for")]
        public VoteId VoteFor { get; private set; }
        [JsonProperty("vote_against")]
        public VoteId VoteAgainst { get; private set; }
        [JsonProperty("total_votes_for")]
        public ulong TotalVotesFor { get; private set; }
        [JsonProperty("total_votes_against")]
        public ulong TotalVotesAgainst { get; private set; }
    }
}