using System;
using Base.Data.Json;
using Base.Data.SpecialAuthorities;
using Base.ECC;
using CustomTools.Extensions.Core;
using Newtonsoft.Json;


namespace Base.Data.Accounts
{
    // id "1.2.x"
    public sealed class AccountObject : IdObject
    {
        [JsonProperty("membership_expiration_date"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime MembershipExpirationDate { get; private set; }
        [JsonProperty("registrar")]
        public SpaceTypeId Registrar { get; private set; }
        [JsonProperty("referrer")]
        public SpaceTypeId Referrer { get; private set; }
        [JsonProperty("lifetime_referrer")]
        public SpaceTypeId LifetimeReferrer { get; private set; }
        [JsonProperty("network_fee_percentage")]
        public ushort NetworkFeePercentage { get; private set; }
        [JsonProperty("lifetime_referrer_fee_percentage")]
        public ushort LifetimeReferrerFeePercentage { get; private set; }
        [JsonProperty("referrer_rewards_percentage")]
        public ushort ReferrerRewardsPercentage { get; private set; }
        [JsonProperty("name")]
        public string Name { get; private set; }
        [JsonProperty("owner")]
        public AuthorityData Owner { get; private set; }
        [JsonProperty("active")]
        public AuthorityData Active { get; private set; }
        [JsonProperty("ed_key")]
        public string EdKey { get; private set; }
        [JsonProperty("options")]
        public AccountOptionsData Options { get; private set; }
        [JsonProperty("statistics")]
        public SpaceTypeId Statistics { get; private set; }
        [JsonProperty("whitelisting_accounts")]
        public SpaceTypeId[] WhitelistingAccounts { get; private set; }
        [JsonProperty("whitelisted_accounts")]
        public SpaceTypeId[] WhitelistedAccounts { get; private set; }
        [JsonProperty("blacklisted_accounts")]
        public SpaceTypeId[] BlacklistedAccounts { get; private set; }
        [JsonProperty("blacklisting_accounts")]
        public SpaceTypeId[] BlacklistingAccounts { get; private set; }
        [JsonProperty("cashback_vb", NullValueHandling = NullValueHandling.Ignore)]
        public SpaceTypeId CashbackVestingBalance { get; private set; }
        [JsonProperty("owner_special_authority")]
        public SpecialAuthorityData OwnerSpecialAuthority { get; private set; }
        [JsonProperty("active_special_authority")]
        public SpecialAuthorityData ActiveSpecialAuthority { get; private set; }
        [JsonProperty("top_n_control_flags")]
        public byte TopNControlFlags { get; private set; }
        [JsonProperty("allowed_assets", NullValueHandling = NullValueHandling.Ignore)]
        public SpaceTypeId[] AllowedAssets { get; private set; }

        public bool IsEquelKey(AccountRole role, KeyPair key)
        {
            switch (role)
            {
                case AccountRole.Owner:
                    if (!Owner.IsNull() && !Owner.KeyAuths.IsNull())
                    {
                        foreach (var keyAuth in Owner.KeyAuths)
                        {
                            if (keyAuth.IsEquelKey(key))
                            {
                                CustomTools.Console.DebugLog(CustomTools.Console.LogGreenColor("Owner->", key.Public, "\n            Owner<-", keyAuth.Key));
                                return true;
                            }
                            CustomTools.Console.DebugLog(CustomTools.Console.LogRedColor("generated_key Owner->", key.Public, "\ngetted_key        Owner<-", keyAuth.Key));
                        }
                    }
                    return false;
                case AccountRole.Active:
                    if (!Active.IsNull() && !Active.KeyAuths.IsNull())
                    {
                        foreach (var keyAuth in Active.KeyAuths)
                        {
                            if (keyAuth.IsEquelKey(key))
                            {
                                CustomTools.Console.DebugLog(CustomTools.Console.LogGreenColor("Active->", key.Public, "\n            Active<-", keyAuth.Key));
                                return true;
                            }
                            CustomTools.Console.DebugLog(CustomTools.Console.LogRedColor("generated_key Active->", key.Public, "\ngetted_key        Active<-", keyAuth.Key));
                        }
                    }
                    return false;
                case AccountRole.Memo:
                    if (!Options.IsNull())
                    {
                        if (Options.IsEquelKey(key))
                        {
                            CustomTools.Console.DebugLog(CustomTools.Console.LogGreenColor("Memo->", key.Public, "\n            Memo<-", Options.MemoKey));
                            return true;
                        }
                        CustomTools.Console.DebugLog(CustomTools.Console.LogRedColor("generated_key Memo->", key.Public, "\ngetted_key        Memo<-", Options.MemoKey));
                    }
                    return false;
                default:
                    return false;
            }
        }
    }
}