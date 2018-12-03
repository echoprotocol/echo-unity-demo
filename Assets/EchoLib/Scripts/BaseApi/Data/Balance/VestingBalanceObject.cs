﻿using Base.Data.VestingPolicies;
using Newtonsoft.Json;


namespace Base.Data
{
    // id "1.13.x"
    public sealed class VestingBalanceObject : IdObject
    {
        [JsonProperty("owner")]
        public SpaceTypeId Owner { get; private set; }
        [JsonProperty("balance")]
        public AssetData Balance { get; private set; }
        [JsonProperty("policy")]
        public VestingPolicyData Policy { get; private set; }
    }
}