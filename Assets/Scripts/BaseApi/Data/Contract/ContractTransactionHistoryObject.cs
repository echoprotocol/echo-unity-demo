﻿using Newtonsoft.Json;


namespace Base.Data.Contract
{
    // id "2.19.x"
    public sealed class ContractTransactionHistoryObject : IdObject //todo check
    {
        [JsonProperty("contract")]
        public SpaceTypeId Contract { get; private set; }
        [JsonProperty("operation_id")]
        public SpaceTypeId Operation { get; private set; }
        [JsonProperty("sequence")]
        public uint Sequence { get; private set; }
        [JsonProperty("next")]
        public SpaceTypeId Next { get; private set; }
    }
}