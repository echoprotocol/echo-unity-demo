﻿using System;
using Base.Data.Json;
using Base.Data.Transactions;
using Base.ECC;
using Newtonsoft.Json;


namespace Base.Data
{
    // id "1.10.x"
    public sealed class ProposalObject : IdObject
    {
        [JsonProperty("expiration_time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime ExpirationTime { get; private set; }
        [JsonProperty("review_period_time", NullValueHandling = NullValueHandling.Ignore), JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? ReviewPeriodTime { get; private set; }
        [JsonProperty("proposed_transaction")]
        public TransactionData ProposedTransaction { get; private set; }
        [JsonProperty("required_active_approvals")]
        public SpaceTypeId[] RequiredActiveApprovals { get; private set; }
        [JsonProperty("available_active_approvals")]
        public SpaceTypeId[] AvailableActiveApprovals { get; private set; }
        [JsonProperty("required_owner_approvals")]
        public SpaceTypeId[] RequiredOwnerApprovals { get; private set; }
        [JsonProperty("available_owner_approvals")]
        public SpaceTypeId[] AvailableOwnerApprovals { get; private set; }
        [JsonProperty("available_key_approvals")]
        public PublicKey[] AvailableKeyApprovals { get; private set; }
    }
}