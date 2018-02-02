using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.Core;
using ChinaPay.Core.Exception;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform {
    /// <summary>
    /// 退/废票申请
    /// </summary>
    public abstract class RefundOrScrapApplyform : BaseApplyform {
        LazyLoader<NormalRefundBill> _refundBillLoader;
        List<RefundFlight> _flights = new List<RefundFlight>();

        protected RefundOrScrapApplyform(decimal orderId, decimal applyformId)
            : base(orderId, applyformId) {
            initLazyLoader();
        }
        protected RefundOrScrapApplyform(Order order, RefundOrScrapApplyformView refundOrScrapApplyformView, Guid oemId)
            : base(order, refundOrScrapApplyformView) {
            if(string.IsNullOrWhiteSpace(refundOrScrapApplyformView.Reason)) throw new CustomException("必须输入申请退/废票的原因");
            initLazyLoader();
            setFlights(refundOrScrapApplyformView);
            ProviderId = order.Provider.CompanyId;
            RequireRevisePrice = !order.RevisedPrice && order.TripType != DataTransferObject.Command.PNR.ItineraryType.OneWay;
            if (order.IsSpecial && !refundOrScrapApplyformView.DelegageCancelPNR)
            {
                Status = RefundApplyformStatus.AppliedForProvider;
            } else {
                if (refundOrScrapApplyformView.NeedPlatfromCancelPNR||requireCancelReservation() && !cancelReservation(oemId) || refundOrScrapApplyformView.DelegageCancelPNR)
                {
                    Status = RefundApplyformStatus.AppliedForCancelReservation;
                } else {
                    Status = RefundApplyformStatus.AppliedForProvider;
                }
            }
        }
        void initLazyLoader() {
            _refundBillLoader = new LazyLoader<NormalRefundBill>(() => DistributionQueryService.QueryNormalRefundBill(this.Id));
        }

        /// <summary>
        /// 详细状态
        /// </summary>
        public RefundApplyformStatus Status {
            get;
            internal set;
        }

        /// <summary>
        /// 是否有过差额退款记录
        /// </summary>
        public bool HasBalanceRefund
        {
            get;
            set;
        }

        /// <summary>
        /// 是否特殊票
        /// </summary>
        public bool IsSpecial {
            get {
                return this.ProductType == ProductType.Special;
            }
        }
        /// <summary>
        /// 处理状态
        /// </summary>
        public override ApplyformProcessStatus ProcessStatus {
            get {
                switch(this.Status) {
                    case RefundApplyformStatus.AppliedForPlatform:
                    case RefundApplyformStatus.AppliedForProvider:
                        return ApplyformProcessStatus.Applied;
                    case RefundApplyformStatus.Denied:
                    case RefundApplyformStatus.Refunded:
                        return ApplyformProcessStatus.Finished;
                    default:
                        return ApplyformProcessStatus.Processing;
                }
            }
        }
        /// <summary>
        /// 退款账单
        /// </summary>
        public NormalRefundBill RefundBill {
            get {
                return _refundBillLoader.QueryData();
            }
            internal set {
                if(value == null) throw new ArgumentNullException("refundBill");
                _refundBillLoader.SetData(value);
            }
        }
        /// <summary>
        /// 退/废票航段信息
        /// </summary>
        public IEnumerable<RefundFlight> Flights {
            get {
                return _flights.AsReadOnly();
            }
        }
        /// <summary>
        /// 原航段信息
        /// </summary>
        public override IEnumerable<Flight> OriginalFlights {
            get { return this.Flights.Select(item => item.OriginalFlight); }
        }

        internal override IEnumerable<Guid> GetAppliedFlights() {
            return this.Flights.Select(item => item.OriginalFlight.Id);
        }
        internal void AddFlight(RefundFlight flight) {
            if(flight == null) throw new ArgumentNullException("flight");
            if(flight.OriginalFlight == null) throw new ArgumentNullException("flight.OriginalFlight");
            if(_flights.Exists(item => item.OriginalFlight.Id == flight.OriginalFlight.Id || Flight.IsSameFlight(flight.OriginalFlight, item.OriginalFlight)))
                throw new RepeatedItemException("不能重复添加相同航段");
            _flights.Add(flight);
        }
        /// <summary> 
        /// 出票方同意退/废票
        /// 不会退款，仅提交财务审核
        /// </summary>
        internal IEnumerable<Service.Distribution.Domain.Bill.Refund.RefundFlight> AgreeByProvider(RefundProcessView processView, string operatorAccount, string operatorName) {
            checkProcessByBusiness();
            if(RequireSeparatePNR && PNRPair.IsNullOrEmpty(processView.NewPNR)) throw new CustomException("未提供分离后的新编码");
            NewPNR = processView.NewPNR;
            var refundFlights = AgreeByProviderExecuteCore(processView);
            Status = RefundApplyformStatus.AgreedByProviderBusiness;
            Operator = operatorName;
            OperatorAccount = operatorAccount;
            Processed();
            return refundFlights;
        }
        /// <summary>
        /// 出票方拒绝退/废票
        /// </summary>
        internal void DenyRefundByProvider(string reason) {
            checkProcessByBusiness();
            if(string.IsNullOrWhiteSpace(reason)) throw new CustomException("必须提供拒绝退/废票的原因");
            this.Status = RefundApplyformStatus.DeniedByProviderBusiness;
            Processed(reason);
        }
        /// <summary>
        /// 重新退/废票
        /// </summary>
        internal void ReProcess() {
            if(this.Status == RefundApplyformStatus.DeniedByProviderBusiness) {
                this.Status = RefundApplyformStatus.AppliedForProvider;
                Processed();
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }
        internal void ReservationCanceled() {
            if(this.Status == RefundApplyformStatus.AppliedForCancelReservation) {
                var refundapplyform = this as RefundApplyform;
                if (refundapplyform!=null&&refundapplyform.RefundType==RefundType.SpecialReason&&refundapplyform.Order.IsSpecial)
                {
                    this.Status = RefundApplyformStatus.AppliedForPlatform;
                    Processed();
                }
                else
                {
                    this.Status = RefundApplyformStatus.AppliedForProvider;
                    Processed();
                }
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }
        /// <summary>
        /// 平台拒绝退/废票
        /// </summary>
        internal void Deny(string reason) {
            if(this.Status == RefundApplyformStatus.DeniedByProviderBusiness) {
                if(string.IsNullOrWhiteSpace(reason)) throw new CustomException("必须提供拒绝退/废票的原因");
                this.Status = RefundApplyformStatus.Denied;
                Processed(reason);
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }
        /// <summary>
        /// 出票方财务同意退款
        /// </summary>
        internal void AgreeReturnMoneyByProviderTreasurer() {
            checkProcessByTreasurer();
            this.Status = RefundApplyformStatus.Refunded;
            Processed("同意退票并退款");
            this.Order.Update(this);
        }
        /// <summary>
        /// 出票方财务拒绝退款
        /// </summary>
        internal void DenyReturnMoneyByProviderTreasurer(string reason) {
            checkProcessByTreasurer();
            if(string.IsNullOrWhiteSpace(reason)) throw new CustomException("必须提供拒绝退款的原因");
            this.Status = RefundApplyformStatus.DeniedByProviderTreasurer;
            Processed(reason);
        }
        private void checkProcessByBusiness() {
            switch(this.Status) {
                case RefundApplyformStatus.AppliedForProvider:
                case RefundApplyformStatus.DeniedByProviderTreasurer:
                    break;
                default:
                    throw new StatusException(this.Id.ToString());
            }
        }
        void checkProcessByTreasurer() {
            if(this.Status != RefundApplyformStatus.AgreedByProviderBusiness) throw new StatusException(this.Id.ToString());
        }
        void setFlights(RefundOrScrapApplyformView refundOrScrapApplyformView) {
            if(refundOrScrapApplyformView.Items != null) {
                var pnrInfo = this.Order.PNRInfos.First(item => item.IsSamePNR(refundOrScrapApplyformView.PNR));
                foreach(var voyage in refundOrScrapApplyformView.Items) {
                    if(voyage != null) {
                        var originalFlight = pnrInfo.Flights.FirstOrDefault(item => item.Id == voyage);
                        if(originalFlight == null) throw new NotFoundException("原编码中不存在航段信息。");
                        var refundFlight = new RefundFlight() {
                            OriginalFlight = originalFlight
                        };
                        this.AddFlight(refundFlight);
                    }
                }
            }
            if(!this.Flights.Any()) throw new CustomException("缺少航段信息");
        }
        bool requireCancelReservation() {
            return (!(this.Order.Product is SpeicalProductInfo) || ((this.Order.Product as SpeicalProductInfo).SpeicalProductType == SpecialProductType.CostFree && !this.Order.IsCustomerResource))
                && this.Order.Source == OrderSource.PlatformOrder
                && PNRPair.Equals(this.Order.ReservationPNR, this.OriginalPNR);
        }
        bool cancelReservation(Guid oemId) {
            var pnrInfo = this.Order.GetPNRInfo(this.OriginalPNR);
            return pnrInfo.CancelReservation(GetAppliedPassengers(), GetAppliedFlights(),oemId);
        }

        protected abstract IEnumerable<Service.Distribution.Domain.Bill.Refund.RefundFlight> AgreeByProviderExecuteCore(RefundProcessView processView);

        internal static RefundOrScrapApplyform NewRefundOrScrapApplyform(Order order, RefundOrScrapApplyformView refundOrScrapApplyformView, Guid oemId)
        {
            if(refundOrScrapApplyformView is ScrapApplyformView) {
                return new ScrapApplyform(order, refundOrScrapApplyformView as ScrapApplyformView,oemId);
            } else {
                return new RefundApplyform(order, refundOrScrapApplyformView as RefundApplyformView,oemId);
            }
        }

        /// <summary>
        /// 申请差额退款
        /// </summary>
        public void ApplyBalanceRefundApplyform() {
            HasBalanceRefund = true;
        }
    }
}