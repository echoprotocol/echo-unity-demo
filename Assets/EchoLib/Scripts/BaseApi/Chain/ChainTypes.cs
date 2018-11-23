namespace Base.Config
{
    public static class ChainTypes
    {
        public enum Space : byte
        {
            RelativeProtocolIds/* */= 0,
            ProtocolIds/*         */= 1,
            ImplementationIds/*   */= 2
        }


        public enum ProtocolType : byte
        {
            Null/*                 */= 0,
            Base/*                 */= 1,
            Account/*              */= 2,
            Asset/*                */= 3,
            ForceSettlement/*      */= 4,
            CommitteeMember/*      */= 5,
            Witness/*              */= 6,
            LimitOrder/*           */= 7,
            CallOrder/*            */= 8,
            Custom/*               */= 9,
            Proposal/*             */= 10,
            OperationHistory/*     */= 11,
            WithdrawPermission/*   */= 12,
            VestingBalance/*       */= 13,
            Worker/*               */= 14,
            Balance/*              */= 15,
            Contract/*             */= 16,
            ResultExecute/*        */= 17,
            BlockResult/*          */= 18
        }


        public enum ImplementationType : byte
        {
            GlobalProperties/*                        */= 0,
            DynamicGlobalProperties/*                 */= 1,
            IndexMeta/*                               */= 2,
            AssetDynamicData/*                        */= 3,
            AssetBitassetData/*                       */= 4,
            AccountBalance/*                          */= 5,
            AccountStatistics/*                       */= 6,
            Transaction/*                             */= 7,
            BlockSummary/*                            */= 8,
            AccountTransactionHistory/*               */= 9,
            BlindedBalance/*                          */= 10,
            ChainProperty/*                           */= 11,
            WitnessSchedule/*                         */= 12,
            BudgetRecord/*                            */= 13,
            _todo_object_2_14/*                       */= 14,
            _todo_object_2_15/*                       */= 15,
            _todo_object_2_16/*                       */= 16,
            _todo_object_2_17/*                       */= 17,
            _todo_object_2_18/*                       */= 18,
            ContractTransactionHistory/*              */= 19,
            ContractStatistics/*                      */= 20
        }


        public enum VoteType : byte
        {
            Committee/*   */= 0,
            Witness/*     */= 1,
            Worker/*      */= 2
        }


        public enum Operation : int
        {
            Transfer/*                                */= 0,
            LimitOrderCreate/*                        */= 1,
            LimitOrderCancel/*                        */= 2,
            CallOrderUpdate/*                         */= 3,
            FillOrder/*                               */= 4,
            AccountCreate/*                           */= 5,
            AccountUpdate/*                           */= 6,
            AccountWhitelist/*                        */= 7,
            AccountUpgrade/*                          */= 8,
            AccountTransfer/*                         */= 9,
            AssetCreate/*                             */= 10,
            AssetUpdate/*                             */= 11,
            AssetUpdateBitasset/*                     */= 12,
            AssetUpdateFeedProducers/*                */= 13,
            AssetIssue/*                              */= 14,
            AssetReserve/*                            */= 15,
            AssetFundFeePool/*                        */= 16,
            AssetSettle/*                             */= 17,
            AssetGlobalSettle/*                       */= 18,
            AssetPublishFeed/*                        */= 19,
            WitnessCreate/*                           */= 20,
            WitnessUpdate/*                           */= 21,
            ProposalCreate/*                          */= 22,
            ProposalUpdate/*                          */= 23,
            ProposalDelete/*                          */= 24,
            WithdrawPermissionCreate/*                */= 25,
            WithdrawPermissionUpdate/*                */= 26,
            WithdrawPermissionClaim/*                 */= 27,
            WithdrawPermissionDelete/*                */= 28,
            CommitteeMemberCreate/*                   */= 29,
            CommitteeMemberUpdate/*                   */= 30,
            CommitteeMemberUpdateGlobalParameters/*   */= 31,
            VestingBalanceCreate/*                    */= 32,
            VestingBalanceWithdraw/*                  */= 33,
            WorkerCreate/*                            */= 34,
            Custom/*                                  */= 35,
            Assert/*                                  */= 36,
            BalanceClaim/*                            */= 37,
            OverrideTransfer/*                        */= 38,
            TransferToBlind/*                         */= 39,
            BlindTransfer/*                           */= 40,
            TransferFromBlind/*                       */= 41,
            AssetSettleCancel/*                       */= 42,
            AssetClaimFees/*                          */= 43,
            FbaDistribute/*                           */= 44,
            BidCollateral/*                           */= 45,
            ExecuteBid/*                              */= 46,
            Contract/*                                */= 47,
            ContractTransfer/*                        */= 48
        }


        public enum FeeParameters : int
        {
            TransferOperation/*                               */= 0,
            LimitOrderCreateOperation/*                       */= 1,
            LimitOrderCancelOperation/*                       */= 2,
            CallOrderUpdateOperation/*                        */= 3,
            FillOrderOperation/*                              */= 4,
            AccountCreateOperation/*                          */= 5,
            AccountUpdateOperation/*                          */= 6,
            AccountWhitelistOperation/*                       */= 7,
            AccountUpgradeOperation/*                         */= 8,
            AccountTransferOperation/*                        */= 9,
            AssetCreateOperation/*                            */= 10,
            AssetUpdateOperation/*                            */= 11,
            AssetUpdateBitassetOperation/*                    */= 12,
            AssetUpdateFeedProducersOperation/*               */= 13,
            AssetIssueOperation/*                             */= 14,
            AssetReserveOperation/*                           */= 15,
            AssetFundFeePoolOperation/*                       */= 16,
            AssetSettleOperation/*                            */= 17,
            AssetGlobalSettleOperation/*                      */= 18,
            AssetPublishFeedOperation/*                       */= 19,
            WitnessCreateOperation/*                          */= 20,
            WitnessUpdateOperation/*                          */= 21,
            ProposalCreateOperation/*                         */= 22,
            ProposalUpdateOperation/*                         */= 23,
            ProposalDeleteOperation/*                         */= 24,
            WithdrawPermissionCreateOperation/*               */= 25,
            WithdrawPermissionUpdateOperation/*               */= 26,
            WithdrawPermissionClaimOperation/*                */= 27,
            WithdrawPermissionDeleteOperation/*               */= 28,
            CommitteeMemberCreateOperation/*                  */= 29,
            CommitteeMemberUpdateOperation/*                  */= 30,
            CommitteeMemberUpdateGlobalParametersOperation/*  */= 31,
            VestingBalanceCreateOperation/*                   */= 32,
            VestingBalanceWithdrawOperation/*                 */= 33,
            WorkerCreateOperation/*                           */= 34,
            CustomOperation/*                                 */= 35,
            AssertOperation/*                                 */= 36,
            BalanceClaimOperation/*                           */= 37,
            OverrideTransferOperation/*                       */= 38,
            TransferToBlindOperation/*                        */= 39,
            BlindTransferOperation/*                          */= 40,
            TransferFromBlindOperation/*                      */= 41,
            AssetSettleCancelOperation/*                      */= 42,
            AssetClaimFeesOperation/*                         */= 43,

            // 44
            // 45
            // 46
            // 47
            // 48
        }


        public enum OperationResult : int
        {
            Void/*            */= 0,
            SpaceTypeId/*     */= 1,
            Asset/*           */= 2
        }


        public enum VestingPolicyInitializer : int
        {
            Linear/*          */= 0,
            Cdd/*             */= 1
        }


        public enum WorkerInitializer : int
        {
            Refund/*          */= 0,
            VestingBalance/*  */= 1,
            Burn/*            */= 2
        }


        public enum Predicate : int
        {
            AccountName/*     */= 0,
            AssetSymbol/*     */= 1,
            BlockId/*         */= 2
        }


        public enum TransactionException : int
        {
            None/*                                */= 0,
            Unknown/*                             */= 1,
            BadRLP/*                              */= 2,
            InvalidFormat/*                       */= 3,
            OutOfGasIntrinsic/*                   */= 4,  // Too little gas to pay for the base transaction cost.
            InvalidSignature/*                    */= 5,
            InvalidNonce/*                        */= 6,
            NotEnoughCash/*                       */= 7,
            OutOfGasBase/*                        */= 8,  // Too little gas to pay for the base transaction cost.
            BlockGasLimitReached/*                */= 9,
            BadInstruction/*                      */= 10,
            BadJumpDestination/*                  */= 11,
            OutOfGas/*                            */= 12, // Ran out of gas executing code of the transaction.
            OutOfStack/*                          */= 13, // Ran out of stack executing code of the transaction.
            StackUnderflow/*                      */= 14,
            RevertInstruction/*                   */= 15,
            InvalidZeroSignatureFormat/*          */= 16,
            AddressAlreadyUsed/*                  */= 17
        }

        public enum CodeDeposit
        {
            None/*            */= 0,
            Failed/*          */= 1,
            Success/*         */= 3,
        }
    }
}