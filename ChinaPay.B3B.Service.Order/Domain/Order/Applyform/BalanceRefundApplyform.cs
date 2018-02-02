using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.Core;
using ChinaPay.Data;
using ChinaPay.Sequence;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform
{
    /// <summary>
    /// 差额退款申请单
    /// </summary>
    public class BalanceRefundApplyform : BaseApplyform
    {
        private RefundOrScrapApplyform _applyform;
        private LazyLoader<RefundOrScrapApplyform> _applyformLoader;
        private LazyLoader<NormalRefundBill> _refundBillLoader;

        #region "Constructors"


        internal BalanceRefundApplyform(decimal orderId,decimal applyformId) : base(orderId,applyformId)
        {
            initLazyLoaders();
        }

        internal BalanceRefundApplyform(Order order, BalanceRefundApplyView applyView)
            : base(order, applyView)
        {
            initLazyLoaders();
            var applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyView.AssociateApplyformId);
            if (applyform == null) throw new ArgumentNullException("申请单信息不存在！");
            if (applyform.HasBalanceRefund) throw new CustomException("每个申请单只能申请一次差错退款！");
            if (applyform.Status != RefundApplyformStatus.Refunded) throw new CustomException("仅处理完成的申请单可以申请差额退款");
            AssociateApplyformId = applyform.Id;
            ApplyRemark = applyView.Reason;
            BalanceRefundStatus = BalanceRefundProcessStatus.AppliedForPlatform;
            AppliedTime = DateTime.Now;
            PurchaserName = applyform.PurchaserName;
            PurchaserId = applyform.PurchaserId;
            ProviderId = applyform.ProviderId;
            ProviderName = applyform.ProviderName;
            OriginalPNR = applyform.OriginalPNR;
            _applyform = applyform;
            IsInterior = applyform.IsInterior;

        }

        private void initLazyLoaders()
        {
            _applyformLoader = new LazyLoader<RefundOrScrapApplyform>(() => ApplyformQueryService.QueryRefundOrScrapApplyform(AssociateApplyformId));
            _refundBillLoader = new LazyLoader<NormalRefundBill>(() => DistributionQueryService.QueryNormalRefundBill(AssociateApplyformId));
        }

        #endregion

        /// <summary>
        /// 申请单号
        /// </summary>
        public override decimal AssociateApplyformId { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public BalanceRefundProcessStatus BalanceRefundStatus { get; set; }

        /// <summary>
        /// 处理人帐号
        /// </summary>
        //public string ProcessorAccount { get; set; }

        /// <summary>
        /// 处理人姓名
        /// </summary>
        //public string Processor { get; set; }

        /// <summary>
        /// 申请人帐号
        /// </summary>
        //public string ApplyAccount { get; set; }

        /// <summary>
        /// 申请人姓名
        /// </summary>
        //public string Applyer { get; set; }

        /// <summary>
        /// 采购公司名称
        /// </summary>
        //public string PurchaserName { get; set; }

        /// <summary>
        /// 原航班信息
        /// </summary>
        public override IEnumerable<Flight> OriginalFlights
        {
            get
            {
                return new List<Flight>();
            }
        }

        /// <summary>
        /// 采购Id
        /// </summary>
        //public Guid PurchaserId { get; set; }

        /// <summary>
        /// 出票方Id
        /// </summary>
        //public Guid ProviderId { get; set; }

        /// <summary>
        /// 出票方公司名称
        /// </summary>
        //public string ProviderName { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public override ApplyformProcessStatus ProcessStatus
        {
            get
            {
                switch (BalanceRefundStatus)
                {
                    case BalanceRefundProcessStatus.AppliedForPlatform:
                        return ApplyformProcessStatus.Applied;
                    case BalanceRefundProcessStatus.AppliedForProvider:
                        case BalanceRefundProcessStatus.AgreedByProviderBusiness:
                        case BalanceRefundProcessStatus.DeniedByProviderTreasurer:
                        case BalanceRefundProcessStatus.DeniedByProviderBusiness:
                        return ApplyformProcessStatus.Processing;
                    case BalanceRefundProcessStatus.Finished:
                    case BalanceRefundProcessStatus.DenyRefund:
                        return ApplyformProcessStatus.Finished;
                    default: throw new CustomException("状态错误");
                }
            }
        }

        /// <summary>
        /// 同意退款人帐号
        /// </summary>
        //public string AgreeRefunAccount { get; set; }

        /// <summary>
        /// 关联申请单信息
        /// </summary>
        public RefundOrScrapApplyform Applyform
        {
            get
            {
                if (_applyform == null)
                {
                    _applyform = _applyformLoader.QueryData();
                }
                return _applyform;
            }
            set { _applyform = value; }
        }

        /// <summary>
        /// 退款账单
        /// </summary>
        public NormalRefundBill RefundBill
        {
            get { return _refundBillLoader.QueryData(); }
            internal set
            {
                if (value == null) throw new ArgumentNullException("refundBill");
                _refundBillLoader.SetData(value);
            }
        }

        internal override IEnumerable<Guid> GetAppliedFlights() { return Applyform.Flights.Select(f => f.OriginalFlight.Id); }

        #region  状态处理
        /// <summary>
        /// 平台同意退款
        /// </summary>
        /// <param name="processerAccount"></param>
        /// <param name="processorName"></param>
        internal void PlatformAgreeRefund(string processerAccount, string processorName)
        {
            BalanceRefundStatus = BalanceRefundProcessStatus.AppliedForProvider;
            OperatorAccount = processerAccount;
            @Operator = processorName;
            ProcessedTime = DateTime.Now;
        }

        /// <summary>
        /// 平台拒绝差错退款
        /// </summary>
        /// <param name="processerAccount"></param>
        /// <param name="processorName"></param>
        /// <param name="reason"> </param>
        internal void PlatformNotAgreeRefund(string processerAccount, string processorName,string reason)
        {
            BalanceRefundStatus = BalanceRefundProcessStatus.DenyRefund;
            OperatorAccount = processerAccount;
            @Operator = processorName;
            ProcessedFailedReason = reason;
            ProcessedTime = DateTime.Now;
        }

        /// <summary>
        /// 出票方业务同意差额退款
        /// </summary>
        /// <param name="processerAccount"></param>
        /// <param name="processorName"></param>
        internal void ProviderBusinessAgreeRefund(string processerAccount, string processorName)
        {
            BalanceRefundStatus = BalanceRefundProcessStatus.AgreedByProviderBusiness;
            OperatorAccount = processerAccount;
            @Operator = processorName;
            ProcessedTime = DateTime.Now;
        }
        
        /// <summary>
        /// 出票方业务拒绝退
        /// </summary>
        /// <param name="processerAccount"></param>
        /// <param name="processorName"></param>
        internal void ProviderBusinessNotAgreeRefund(string processerAccount, string processorName,string reason)
        {
            BalanceRefundStatus = BalanceRefundProcessStatus.DeniedByProviderBusiness;
            OperatorAccount = processerAccount;
            @Operator = processorName;
            ProcessedTime = DateTime.Now;
            ProcessedFailedReason = reason;
        }

        /// <summary>
        /// 出票方财务同意退款
        /// </summary>
        /// <param name="processerAccount"></param>
        /// <param name="processorName"></param>
        internal void ProviderTreasurerAgreeRefund(string processerAccount, string processorName)
        {
            BalanceRefundStatus = BalanceRefundProcessStatus.Finished;
            OperatorAccount = processerAccount;
            @Operator = processorName;
            ProcessedTime = DateTime.Now;
        }

        /// <summary>
        /// 出票方财务拒绝退款
        /// </summary>
        /// <param name="processerAccount"></param>
        /// <param name="processorName"></param>
        /// <param name="reason"> </param>
        internal void ProviderTreasurerNotAgreeRefund(string processerAccount, string processorName,string reason)
        {
            BalanceRefundStatus = BalanceRefundProcessStatus.DeniedByProviderTreasurer;
            OperatorAccount = processerAccount;
            @Operator = processorName;
            ProcessedTime = DateTime.Now;
            ProcessedFailedReason = reason;
        }

        #endregion




        public override string ToString()
        {
            return "差额退款";
        }
    }
}