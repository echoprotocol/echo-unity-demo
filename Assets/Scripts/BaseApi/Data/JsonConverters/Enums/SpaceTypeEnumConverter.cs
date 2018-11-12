using System;
using Base.Config;


namespace Base.Data.Json
{
    public sealed class SpaceTypeEnumConverter : JsonCustomConverter<SpaceType, string>
    {
        private readonly static string BASE =/*                           */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.Base);
        private readonly static string ACCOUNT =/*                        */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.Account);
        private readonly static string ASSET =/*                          */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.Asset);
        private readonly static string FORCE_SETTLEMENT =/*               */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.ForceSettlement);
        private readonly static string COMMITTEE_MEMBER =/*               */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.CommitteeMember);
        private readonly static string WITNESS =/*                        */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.Witness);
        private readonly static string LIMIT_ORDER =/*                    */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.LimitOrder);
        private readonly static string CALL_ORDER =/*                     */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.CallOrder);
        private readonly static string CUSTOM =/*                         */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.Custom);
        private readonly static string PROPOSAL =/*                       */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.Proposal);
        private readonly static string OPERATION_HISTORY =/*              */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.OperationHistory);
        private readonly static string WITHDRAW_PERMISSION =/*            */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.WithdrawPermission);
        private readonly static string VESTING_BALANCE =/*                */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.VestingBalance);
        private readonly static string WORKER =/*                         */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.Worker);
        private readonly static string BALANCE =/*                        */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.Balance);
        private readonly static string CONTRACT =/*                       */string.Format("{0}.{1}", (int)ChainTypes.Space.ProtocolIds, (int)ChainTypes.ProtocolType.Contract);

        private readonly static string GLOBAL_PROPERTIES =/*              */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.GlobalProperties);
        private readonly static string DYNAMIC_GLOBAL_PROPERTIES =/*      */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.DynamicGlobalProperties);
        private readonly static string ASSET_DYNAMIC_DATA =/*             */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.AssetDynamicData);
        private readonly static string ASSET_BITASSET_DATA =/*            */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.AssetBitassetData);
        private readonly static string ACCOUNT_BALANCE =/*                */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.AccountBalance);
        private readonly static string ACCOUNT_STATISTICS =/*             */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.AccountStatistics);
        private readonly static string TRANSACTION =/*                    */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.Transaction);
        private readonly static string BLOCK_SUMMARY =/*                  */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.BlockSummary);
        private readonly static string ACCOUNT_TRANSACTION_HISTORY =/*    */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.AccountTransactionHistory);
        private readonly static string BLINDED_BALANCE =/*                */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.BlindedBalance);
        private readonly static string CHAIN_PROPERTY =/*                 */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.ChainProperty);
        private readonly static string WITNESS_SCHEDULE =/*               */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.WitnessSchedule);
        private readonly static string BUDGET_RECORD =/*                  */string.Format("{0}.{1}", (int)ChainTypes.Space.ImplementationIds, (int)ChainTypes.ImplementationType.BudgetRecord);


        protected override SpaceType Deserialize(string value, Type objectType) => ConvertFrom(value);

        protected override string Serialize(SpaceType value) => ConvertTo(value);

        public static string ConvertTo(SpaceType spaceType)
        {
            switch (spaceType)
            {
                case SpaceType.Base:/*                                  */return BASE;
                case SpaceType.Account:/*                               */return ACCOUNT;
                case SpaceType.Asset:/*                                 */return ASSET;
                case SpaceType.ForceSettlement:/*                       */return FORCE_SETTLEMENT;
                case SpaceType.CommitteeMember:/*                       */return COMMITTEE_MEMBER;
                case SpaceType.Witness:/*                               */return WITNESS;
                case SpaceType.LimitOrder:/*                            */return LIMIT_ORDER;
                case SpaceType.CallOrder:/*                             */return CALL_ORDER;
                case SpaceType.Custom:/*                                */return CUSTOM;
                case SpaceType.Proposal:/*                              */return PROPOSAL;
                case SpaceType.OperationHistory:/*                      */return OPERATION_HISTORY;
                case SpaceType.WithdrawPermission:/*                    */return WITHDRAW_PERMISSION;
                case SpaceType.VestingBalance:/*                        */return VESTING_BALANCE;
                case SpaceType.Worker:/*                                */return WORKER;
                case SpaceType.Balance:/*                               */return BALANCE;
                case SpaceType.Contract:/*                              */return CONTRACT;
                case SpaceType.GlobalProperties:/*                      */return GLOBAL_PROPERTIES;
                case SpaceType.DynamicGlobalProperties:/*               */return DYNAMIC_GLOBAL_PROPERTIES;
                case SpaceType.AssetDynamicData:/*                      */return ASSET_DYNAMIC_DATA;
                case SpaceType.AssetBitassetData:/*                     */return ASSET_BITASSET_DATA;
                case SpaceType.AccountBalance:/*                        */return ACCOUNT_BALANCE;
                case SpaceType.AccountStatistics:/*                     */return ACCOUNT_STATISTICS;
                case SpaceType.Transaction:/*                           */return TRANSACTION;
                case SpaceType.BlockSummary:/*                          */return BLOCK_SUMMARY;
                case SpaceType.AccountTransactionHistory:/*             */return ACCOUNT_TRANSACTION_HISTORY;
                case SpaceType.BlindedBalance:/*                        */return BLINDED_BALANCE;
                case SpaceType.ChainProperty:/*                         */return CHAIN_PROPERTY;
                case SpaceType.WitnessSchedule:/*                       */return WITNESS_SCHEDULE;
                case SpaceType.BudgetRecord:/*                          */return BUDGET_RECORD;
            }
            return string.Empty;
        }

        public static SpaceType ConvertFrom(string spaceType)
        {
            if (BASE.Equals(spaceType))/*                               */return SpaceType.Base;
            if (ACCOUNT.Equals(spaceType))/*                            */return SpaceType.Account;
            if (ASSET.Equals(spaceType))/*                              */return SpaceType.Asset;
            if (FORCE_SETTLEMENT.Equals(spaceType))/*                   */return SpaceType.ForceSettlement;
            if (COMMITTEE_MEMBER.Equals(spaceType))/*                   */return SpaceType.CommitteeMember;
            if (WITNESS.Equals(spaceType))/*                            */return SpaceType.Witness;
            if (LIMIT_ORDER.Equals(spaceType))/*                        */return SpaceType.LimitOrder;
            if (CALL_ORDER.Equals(spaceType))/*                         */return SpaceType.CallOrder;
            if (CUSTOM.Equals(spaceType))/*                             */return SpaceType.Custom;
            if (PROPOSAL.Equals(spaceType))/*                           */return SpaceType.Proposal;
            if (OPERATION_HISTORY.Equals(spaceType))/*                  */return SpaceType.OperationHistory;
            if (WITHDRAW_PERMISSION.Equals(spaceType))/*                */return SpaceType.WithdrawPermission;
            if (VESTING_BALANCE.Equals(spaceType))/*                    */return SpaceType.VestingBalance;
            if (WORKER.Equals(spaceType))/*                             */return SpaceType.Worker;
            if (BALANCE.Equals(spaceType))/*                            */return SpaceType.Balance;
            if (CONTRACT.Equals(spaceType))/*                           */return SpaceType.Contract;
            if (GLOBAL_PROPERTIES.Equals(spaceType))/*                  */return SpaceType.GlobalProperties;
            if (DYNAMIC_GLOBAL_PROPERTIES.Equals(spaceType))/*          */return SpaceType.DynamicGlobalProperties;
            if (ASSET_DYNAMIC_DATA.Equals(spaceType))/*                 */return SpaceType.AssetDynamicData;
            if (ASSET_BITASSET_DATA.Equals(spaceType))/*                */return SpaceType.AssetBitassetData;
            if (ACCOUNT_BALANCE.Equals(spaceType))/*                    */return SpaceType.AccountBalance;
            if (ACCOUNT_STATISTICS.Equals(spaceType))/*                 */return SpaceType.AccountStatistics;
            if (TRANSACTION.Equals(spaceType))/*                        */return SpaceType.Transaction;
            if (BLOCK_SUMMARY.Equals(spaceType))/*                      */return SpaceType.BlockSummary;
            if (ACCOUNT_TRANSACTION_HISTORY.Equals(spaceType))/*        */return SpaceType.AccountTransactionHistory;
            if (BLINDED_BALANCE.Equals(spaceType))/*                    */return SpaceType.BlindedBalance;
            if (CHAIN_PROPERTY.Equals(spaceType))/*                     */return SpaceType.ChainProperty;
            if (WITNESS_SCHEDULE.Equals(spaceType))/*                   */return SpaceType.WitnessSchedule;
            if (BUDGET_RECORD.Equals(spaceType))/*                      */return SpaceType.BudgetRecord;
            return SpaceType.Unknown;
        }
    }
}