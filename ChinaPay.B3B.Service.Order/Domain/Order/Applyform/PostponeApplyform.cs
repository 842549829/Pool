using System;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Data;
using System.Collections.Generic;
using ChinaPay.Core.Exception;
using ChinaPay.Core;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay;
using System.Linq;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform {
    /// <summary>
    /// 改期申请
    /// </summary>
    public class PostponeApplyform : BaseApplyform {
        LazyLoader<PostponePayBill> _payBillLoader;
        List<PostponeFlight> _flights = new List<PostponeFlight>();
        decimal? _postponeFee = null;

        internal PostponeApplyform(decimal orderId, decimal applyformId)
            : base(orderId, applyformId) {
            initLazyLoader();
        }
        internal PostponeApplyform(Order order, PostponeApplyformView postponeApplyformView)
            : base(order, postponeApplyformView) {
            initLazyLoader();
            setFlights(postponeApplyformView);
            this.Status = PostponeApplyformStatus.Applied;
            RequireRevisePrice = false;
        }
        void initLazyLoader() {
            _payBillLoader = new LazyLoader<PostponePayBill>(() => DistributionQueryService.QueryPostponePayBill(this.Id));
        }

        /// <summary>
        /// 改期手续费
        /// </summary>
        public decimal PostponeFee {
            get {
                if(!_postponeFee.HasValue) {
                    _postponeFee = this.Flights.Sum(item => item.PostponeFee) * this.Passengers.Count();
                }
                return _postponeFee.Value;
            }
            internal set {
                _postponeFee = value;
            }
        }
        /// <summary>
        /// 改期航班信息
        /// </summary>
        public IEnumerable<PostponeFlight> Flights {
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
        /// <summary>
        /// 详细状态
        /// </summary>
        public PostponeApplyformStatus Status {
            get;
            internal set;
        }
        /// <summary>
        /// 处理状态
        /// </summary>
        public override ApplyformProcessStatus ProcessStatus {
            get {
                switch(this.Status) {
                    case PostponeApplyformStatus.Applied:
                        return ApplyformProcessStatus.Applied;
                    case PostponeApplyformStatus.Cancelled:
                    case PostponeApplyformStatus.Denied:
                    case PostponeApplyformStatus.Postponed:
                        return ApplyformProcessStatus.Finished;
                    default:
                        return ApplyformProcessStatus.Processing;
                }
            }
        }
        /// <summary>
        /// 支付账单
        /// </summary>
        public PostponePayBill PayBill {
            get {
                return _payBillLoader.QueryData();
            }
            internal set {
                if(value == null) throw new ArgumentNullException("payBill");
                _payBillLoader.SetData(value);
            }
        }

        internal override IEnumerable<Guid> GetAppliedFlights() {
            return this.Flights.Select(item => item.OriginalFlight.Id).ToList();
        }

        internal void AddFlight(PostponeFlight flight) {
            if(flight == null) throw new ArgumentNullException("flight");
            if(flight.OriginalFlight == null) throw new ArgumentNullException("flight.OriginalFlight");
            if(_flights.Exists(item => item.OriginalFlight.Id == flight.OriginalFlight.Id || Flight.IsSameFlight(flight.OriginalFlight, item.OriginalFlight)))
                throw new RepeatedItemException("不能重复添加相同航段");
            _flights.Add(flight);
        }
        /// <summary>
        /// 同意改期
        /// </summary>
        internal void Agree(PostponeView postponeView, string operatorAccount, string @operator) {
            if(this.Status == PostponeApplyformStatus.Applied || this.Status == PostponeApplyformStatus.Paid) {
                if(postponeView == null || postponeView.Items == null || !postponeView.Items.Any())
                    throw new CustomException("改期信息不能为空");
                foreach(var flight in this.Flights) {
                    var postponeVoyageView = postponeView.Items.FirstOrDefault(item => flight.OriginalFlight.IsSameVoyage(item.AirportPair));
                    if(postponeVoyageView == null)
                        throw new NotFoundException("缺少航段【" + flight.OriginalFlight.Departure.Code + "-" + flight.OriginalFlight.Arrival.Code + "】的改期信息");
                    flight.NewFlight.FlightNo = postponeVoyageView.FlightNo;
                    flight.NewFlight.AirCraft = postponeVoyageView.AirCraft;
                    flight.NewFlight.TakeoffTime = postponeVoyageView.TakeoffTime;
                    flight.NewFlight.LandingTime = postponeVoyageView.LandingTime;

                    flight.OriginalFlight.Ticket.RemoveFlight(flight.OriginalFlight);
                    flight.OriginalFlight.Ticket.AddFlight(flight.NewFlight);
                }
                if(!PNRPair.IsNullOrEmpty(postponeView.NewPNR))
                    this.NewPNR = postponeView.NewPNR;
                this.Status = PostponeApplyformStatus.Postponed;
                this.Operator = @operator;
                this.OperatorAccount = operatorAccount;
                Processed();
                this.Order.Update(this);
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }
        /// <summary>
        /// 收取改期费
        /// </summary>
        internal void ChargePostponeFee(IEnumerable<PostponeFeeView> postponeFeeViews) {
            if(postponeFeeViews == null || !postponeFeeViews.Any()) throw new CustomException("改期费信息不能为空");
            if(this.Status == PostponeApplyformStatus.Applied) {
                foreach(var flight in this.Flights) {
                    var postponeFeeView = postponeFeeViews.FirstOrDefault(item => flight.OriginalFlight.IsSameVoyage(item.AirportPair));
                    if(postponeFeeView == null) throw new NotFoundException("缺少航段【" + flight.OriginalFlight.Departure.Code + "-" + flight.OriginalFlight.Arrival.Code + "】的改期费信息");
                    flight.PostponeFee = postponeFeeView.Fee;
                }
                this._postponeFee = null;
                this.Status = PostponeApplyformStatus.Agreed;
                Processed();
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }
        /// <summary>
        /// 是否可以支付
        /// </summary>
        internal bool IsPayable(out string message) {
            if(this.Status == PostponeApplyformStatus.Agreed) {
                var expressedTime = this.ProcessedTime.Value.AddMinutes(SystemManagement.SystemParamService.PostponePayableLimit);
                if(expressedTime <= DateTime.Now) {
                    message = "改期申请单已超出支付时限。请先取消该申请，然后重新申请";
                } else {
                    message = string.Empty;
                    return true;
                }
            } else {
                message = "仅待支付改期费的申请单才需要支付";
            }
            return false;
        }
        /// <summary>
        /// 取消申请
        /// </summary>
        internal void Cancel() {
            if(this.Status == PostponeApplyformStatus.Agreed) {
                this.Status = PostponeApplyformStatus.Cancelled;
                Processed();
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }
        /// <summary>
        /// 拒绝改期
        /// </summary>
        internal void Deny(string reason) {
            if(this.Status == PostponeApplyformStatus.Applied || this.Status == PostponeApplyformStatus.Paid) {
                if(string.IsNullOrWhiteSpace(reason)) throw new CustomException("必须提供拒绝改期的原因");
                this.Status = PostponeApplyformStatus.Denied;
                Processed(reason);
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }
        /// <summary>
        /// 支付成功
        /// </summary>
        internal void PaySuccess() {
            if(this.Status == PostponeApplyformStatus.Agreed) {
                this.Status = PostponeApplyformStatus.Paid;
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }

        public override string ToString() {
            return "改期";
        }

        void setFlights(PostponeApplyformView postponeApplyformView) {
            if(postponeApplyformView.Items != null) {
                var pnrInfo = this.Order.PNRInfos.First(item => item.IsSamePNR(postponeApplyformView.PNR));
                foreach(var item in postponeApplyformView.Items) {
                    if(item != null) {
                        var originalFlight = pnrInfo.Flights.FirstOrDefault(flight => flight.Id == item.Voyage);
                        if(originalFlight == null)
                            throw new NotFoundException("原编码中不存在航段信息。" + item.Voyage);
                        var postponeFlight = new PostponeFlight() {
                            OriginalFlight = originalFlight,
                            NewFlight = originalFlight.Copy()
                        };
                        postponeFlight.NewFlight.FlightNo = item.NewFlightNo;
                        postponeFlight.NewFlight.TakeoffTime = item.NewFlightDate;
                        this.AddFlight(postponeFlight);
                    }
                }
            }
            if(!this.Flights.Any())
                throw new CustomException("缺少航段信息");
        }
    }
}