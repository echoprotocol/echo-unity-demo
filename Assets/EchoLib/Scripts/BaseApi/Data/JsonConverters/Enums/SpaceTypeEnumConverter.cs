using System;
using Enum = Base.Data.SpaceType;
using Space = Base.Config.ChainTypes.Space;
using ProtocolType = Base.Config.ChainTypes.ProtocolType;
using ImplementationType = Base.Config.ChainTypes.ImplementationType;


namespace Base.Data.Json
{
    public sealed class SpaceTypeEnumConverter : JsonCustomConverter<Enum, string>
    {
        private readonly static string BASE =/*                            */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.Base);
        private readonly static string ACCOUNT =/*                         */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.Account);
        private readonly static string ASSET =/*                           */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.Asset);
        private readonly static string FORCE_SETTLEMENT =/*                */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.ForceSettlement);
        private readonly static string COMMITTEE_MEMBER =/*                */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.CommitteeMember);
        private readonly static string WITNESS =/*                         */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.Witness);
        private readonly static string LIMIT_ORDER =/*                     */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.LimitOrder);
        private readonly static string CALL_ORDER =/*                      */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.CallOrder);
        private readonly static string CUSTOM =/*                          */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.Custom);
        private readonly static string PROPOSAL =/*                        */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.Proposal);
        private readonly static string OPERATION_HISTORY =/*               */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.OperationHistory);
        private readonly static string WITHDRAW_PERMISSION =/*             */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.WithdrawPermission);
        private readonly static string VESTING_BALANCE =/*                 */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.VestingBalance);
        private readonly static string WORKER =/*                          */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.Worker);
        private readonly static string BALANCE =/*                         */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.Balance);
        private readonly static string CONTRACT =/*                        */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.Contract);
        private readonly static string RESULT_CONTRACT =/*                 */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.ResultContract);
        private readonly static string BLOCK_RESULT =/*                    */string.Format("{0}.{1}", (int)Space.ProtocolIds, (int)ProtocolType.BlockResult);

        private readonly static string GLOBAL_PROPERTIES =/*               */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.GlobalProperties);
        private readonly static string DYNAMIC_GLOBAL_PROPERTIES =/*       */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.DynamicGlobalProperties);
        private readonly static string ASSET_DYNAMIC_DATA =/*              */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.AssetDynamicData);
        private readonly static string ASSET_BITASSET_DATA =/*             */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.AssetBitassetData);
        private readonly static string ACCOUNT_BALANCE =/*                 */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.AccountBalance);
        private readonly static string ACCOUNT_STATISTICS =/*              */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.AccountStatistics);
        private readonly static string TRANSACTION =/*                     */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.Transaction);
        private readonly static string BLOCK_SUMMARY =/*                   */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.BlockSummary);
        private readonly static string ACCOUNT_TRANSACTION_HISTORY =/*     */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.AccountTransactionHistory);
        private readonly static string BLINDED_BALANCE =/*                 */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.BlindedBalance);
        private readonly static string CHAIN_PROPERTY =/*                  */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.ChainProperty);
        private readonly static string WITNESS_SCHEDULE =/*                */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.WitnessSchedule);
        private readonly static string BUDGET_RECORD =/*                   */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.BudgetRecord);
        private readonly static string SPECIAL_AUTHORITY =/*               */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.SpecialAuthority);
        private readonly static string BUYBACK =/*                         */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.Buyback);
        private readonly static string FBA_ACCUMULATOR =/*                 */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.FbaAccumulator);
        private readonly static string COLLATERAL_BID =/*                  */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.CollateralBid);
        private readonly static string CONTRACT_BALANCE =/*                */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.ContractBalance);
        private readonly static string CONTRACT_HISTORY =/*                */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.ContractHistory);
        private readonly static string CONTRACT_STATISTICS =/*             */string.Format("{0}.{1}", (int)Space.ImplementationIds, (int)ImplementationType.ContractStatistics);


        protected override Enum Deserialize(string value, Type objectType) => ConvertFrom(value);

        protected override string Serialize(Enum value) => ConvertTo(value);

        public static string ConvertTo(Enum value)
        {
            switch (value)
            {
                case Enum.Base:/*                                  */return BASE;
                case Enum.Account:/*                               */return ACCOUNT;
                case Enum.Asset:/*                                 */return ASSET;
                case Enum.ForceSettlement:/*                       */return FORCE_SETTLEMENT;
                case Enum.CommitteeMember:/*                       */return COMMITTEE_MEMBER;
                case Enum.Witness:/*                               */return WITNESS;
                case Enum.LimitOrder:/*                            */return LIMIT_ORDER;
                case Enum.CallOrder:/*                             */return CALL_ORDER;
                case Enum.Custom:/*                                */return CUSTOM;
                case Enum.Proposal:/*                              */return PROPOSAL;
                case Enum.OperationHistory:/*                      */return OPERATION_HISTORY;
                case Enum.WithdrawPermission:/*                    */return WITHDRAW_PERMISSION;
                case Enum.VestingBalance:/*                        */return VESTING_BALANCE;
                case Enum.Worker:/*                                */return WORKER;
                case Enum.Balance:/*                               */return BALANCE;
                case Enum.Contract:/*                              */return CONTRACT;
                case Enum.ResultContract:/*                        */return RESULT_CONTRACT;
                case Enum.BlockResult:/*                           */return BLOCK_RESULT;
                case Enum.GlobalProperties:/*                      */return GLOBAL_PROPERTIES;
                case Enum.DynamicGlobalProperties:/*               */return DYNAMIC_GLOBAL_PROPERTIES;
                case Enum.AssetDynamicData:/*                      */return ASSET_DYNAMIC_DATA;
                case Enum.AssetBitassetData:/*                     */return ASSET_BITASSET_DATA;
                case Enum.AccountBalance:/*                        */return ACCOUNT_BALANCE;
                case Enum.AccountStatistics:/*                     */return ACCOUNT_STATISTICS;
                case Enum.Transaction:/*                           */return TRANSACTION;
                case Enum.BlockSummary:/*                          */return BLOCK_SUMMARY;
                case Enum.AccountTransactionHistory:/*             */return ACCOUNT_TRANSACTION_HISTORY;
                case Enum.BlindedBalance:/*                        */return BLINDED_BALANCE;
                case Enum.ChainProperty:/*                         */return CHAIN_PROPERTY;
                case Enum.WitnessSchedule:/*                       */return WITNESS_SCHEDULE;
                case Enum.BudgetRecord:/*                          */return BUDGET_RECORD;
                case Enum.SpecialAuthority:/*                      */return SPECIAL_AUTHORITY;
                case Enum.Buyback:/*                               */return BUYBACK;
                case Enum.FbaAccumulator:/*                        */return FBA_ACCUMULATOR;
                case Enum.CollateralBid:/*                         */return COLLATERAL_BID;
                case Enum.ContractBalance:/*                       */return CONTRACT_BALANCE;
                case Enum.ContractHistory:/*                       */return CONTRACT_HISTORY;
                case Enum.ContractStatistics:/*                    */return CONTRACT_STATISTICS;
            }
            return string.Empty;
        }

        public static Enum ConvertFrom(string value)
        {
            if (BASE.Equals(value))/*                              */return Enum.Base;
            if (ACCOUNT.Equals(value))/*                           */return Enum.Account;
            if (ASSET.Equals(value))/*                             */return Enum.Asset;
            if (FORCE_SETTLEMENT.Equals(value))/*                  */return Enum.ForceSettlement;
            if (COMMITTEE_MEMBER.Equals(value))/*                  */return Enum.CommitteeMember;
            if (WITNESS.Equals(value))/*                           */return Enum.Witness;
            if (LIMIT_ORDER.Equals(value))/*                       */return Enum.LimitOrder;
            if (CALL_ORDER.Equals(value))/*                        */return Enum.CallOrder;
            if (CUSTOM.Equals(value))/*                            */return Enum.Custom;
            if (PROPOSAL.Equals(value))/*                          */return Enum.Proposal;
            if (OPERATION_HISTORY.Equals(value))/*                 */return Enum.OperationHistory;
            if (WITHDRAW_PERMISSION.Equals(value))/*               */return Enum.WithdrawPermission;
            if (VESTING_BALANCE.Equals(value))/*                   */return Enum.VestingBalance;
            if (WORKER.Equals(value))/*                            */return Enum.Worker;
            if (BALANCE.Equals(value))/*                           */return Enum.Balance;
            if (CONTRACT.Equals(value))/*                          */return Enum.Contract;
            if (RESULT_CONTRACT.Equals(value))/*                   */return Enum.ResultContract;
            if (BLOCK_RESULT.Equals(value))/*                      */return Enum.BlockResult;
            if (GLOBAL_PROPERTIES.Equals(value))/*                 */return Enum.GlobalProperties;
            if (DYNAMIC_GLOBAL_PROPERTIES.Equals(value))/*         */return Enum.DynamicGlobalProperties;
            if (ASSET_DYNAMIC_DATA.Equals(value))/*                */return Enum.AssetDynamicData;
            if (ASSET_BITASSET_DATA.Equals(value))/*               */return Enum.AssetBitassetData;
            if (ACCOUNT_BALANCE.Equals(value))/*                   */return Enum.AccountBalance;
            if (ACCOUNT_STATISTICS.Equals(value))/*                */return Enum.AccountStatistics;
            if (TRANSACTION.Equals(value))/*                       */return Enum.Transaction;
            if (BLOCK_SUMMARY.Equals(value))/*                     */return Enum.BlockSummary;
            if (ACCOUNT_TRANSACTION_HISTORY.Equals(value))/*       */return Enum.AccountTransactionHistory;
            if (BLINDED_BALANCE.Equals(value))/*                   */return Enum.BlindedBalance;
            if (CHAIN_PROPERTY.Equals(value))/*                    */return Enum.ChainProperty;
            if (WITNESS_SCHEDULE.Equals(value))/*                  */return Enum.WitnessSchedule;
            if (BUDGET_RECORD.Equals(value))/*                     */return Enum.BudgetRecord;
            if (SPECIAL_AUTHORITY.Equals(value))/*                 */return Enum.SpecialAuthority;
            if (BUYBACK.Equals(value))/*                           */return Enum.Buyback;
            if (FBA_ACCUMULATOR.Equals(value))/*                   */return Enum.FbaAccumulator;
            if (COLLATERAL_BID.Equals(value))/*                    */return Enum.CollateralBid;
            if (CONTRACT_BALANCE.Equals(value))/*                  */return Enum.ContractBalance;
            if (CONTRACT_HISTORY.Equals(value))/*                  */return Enum.ContractHistory;
            if (CONTRACT_STATISTICS.Equals(value))/*               */return Enum.ContractStatistics;
            return Enum.Unknown;
        }
    }
}