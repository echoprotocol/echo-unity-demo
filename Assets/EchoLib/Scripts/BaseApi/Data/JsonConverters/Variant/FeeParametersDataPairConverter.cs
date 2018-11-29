using System;
using Base.Config;
using Base.Data.Operations.Fee;
using CustomTools.Extensions.Core;
using Newtonsoft.Json.Linq;
using Tools.Json;


namespace Base.Data.Json
{
    public sealed class FeeParametersDataPairConverter : JsonCustomConverter<FeeParametersData, JArray>
    {
        protected override FeeParametersData Deserialize(JArray value, Type objectType)
        {
            if (value.IsNullOrEmpty() || value.Count != 2)
            {
                return null;
            }
            var type = (ChainTypes.FeeParameters)Convert.ToInt32(value.First);
            switch (type)
            {
                case ChainTypes.FeeParameters.TransferOperation:
                    return TransferOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.LimitOrderCreateOperation:
                    return LimitOrderCreateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.LimitOrderCancelOperation:
                    return LimitOrderCancelOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.CallOrderUpdateOperation:
                    return CallOrderUpdateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.FillOrderOperation:
                    return FillOrderOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AccountCreateOperation:
                    return AccountCreateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AccountUpdateOperation:
                    return AccountUpdateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AccountWhitelistOperation:
                    return AccountWhitelistOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AccountUpgradeOperation:
                    return AccountUpgradeOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AccountTransferOperation:
                    return AccountTransferOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssetCreateOperation:
                    return AssetCreateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssetUpdateOperation:
                    return AssetUpdateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssetUpdateBitassetOperation:
                    return AssetUpdateBitassetOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssetUpdateFeedProducersOperation:
                    return AssetUpdateFeedProducersOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssetIssueOperation:
                    return AssetIssueOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssetReserveOperation:
                    return AssetReserveOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssetFundFeePoolOperation:
                    return AssetFundFeePoolOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssetSettleOperation:
                    return AssetSettleOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssetGlobalSettleOperation:
                    return AssetGlobalSettleOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssetPublishFeedOperation:
                    return AssetPublishFeedOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.WitnessCreateOperation:
                    return WitnessCreateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.WitnessUpdateOperation:
                    return WitnessUpdateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.ProposalCreateOperation:
                    return ProposalCreateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.ProposalUpdateOperation:
                    return ProposalUpdateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.ProposalDeleteOperation:
                    return ProposalDeleteOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.WithdrawPermissionCreateOperation:
                    return WithdrawPermissionCreateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.WithdrawPermissionUpdateOperation:
                    return WithdrawPermissionUpdateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.WithdrawPermissionClaimOperation:
                    return WithdrawPermissionClaimOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.WithdrawPermissionDeleteOperation:
                    return WithdrawPermissionDeleteOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.CommitteeMemberCreateOperation:
                    return CommitteeMemberCreateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.CommitteeMemberUpdateOperation:
                    return CommitteeMemberUpdateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.CommitteeMemberUpdateGlobalParametersOperation:
                    return CommitteeMemberUpdateGlobalParametersOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.VestingBalanceCreateOperation:
                    return VestingBalanceCreateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.VestingBalanceWithdrawOperation:
                    return VestingBalanceWithdrawOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.WorkerCreateOperation:
                    return WorkerCreateOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.CustomOperation:
                    return CustomOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssertOperation:
                    return AssertOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.BalanceClaimOperation:
                    return BalanceClaimOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.OverrideTransferOperation:
                    return OverrideTransferOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.TransferToBlindOperation:
                    return TransferToBlindOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.BlindTransferOperation:
                    return BlindTransferOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.TransferFromBlindOperation:
                    return TransferFromBlindOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssetSettleCancelOperation:
                    return AssetSettleCancelOperationFeeParametersData.Create(value.Last as JObject);
                case ChainTypes.FeeParameters.AssetClaimFeesOperation:
                    return AssetClaimFeesOperationFeeParametersData.Create(value.Last as JObject);
                default:
                    CustomTools.Console.DebugError("Unexpected fee parameters type:", type);
                    return null;
            }
        }

        protected override JArray Serialize(FeeParametersData value)
        {
            return value.IsNull() ? new JArray() : new JArray((int)value.Type, JObject.Parse(value.ToString()));
        }
    }
}