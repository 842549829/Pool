using System;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Command.PNR;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Order.Domain {
    abstract class ApplyChecker {
        protected ApplyChecker(Order order, ApplyformView applyformView) {
            this.Order = order;
            this.Applyform = applyformView;
        }

        public Order Order {
            get;
            private set;
        }
        public ApplyformView Applyform {
            get;
            private set;
        }

        public void Execute() {
            if (this is BalanceRefundApplyChecker) return;
            if(this.Order.Status != OrderStatus.Finished) throw new ChinaPay.Core.Exception.StatusException("未出票完成的订单不能申请退改签");
            // 检查该产品是否可以申请退改签
            CheckProductCanApply();
            // 检查申请历史记录
            checkApplyformHistory();
            CheckCore();
            // 检查申请受理方是否下班
            CheckAccepterIsWorking();
        }

        protected virtual void CheckProductCanApply() { }
        protected abstract void CheckAccepterIsWorking();
        protected virtual void CheckCore(Passenger passenger, Guid voyage, Flight originalFlight) { }
        protected abstract void CheckCore();
        protected void CheckInWorkingTimeOfProvider() {
            var workingHours = Service.Organization.CompanyService.GetWorkinghours(this.Order.Provider.CompanyId);
            Izual.Time startTime, endTime;
            if (this.Order.IsB3BOrder)
            {
                if (DateTime.Today.IsWeekend())
                {
                    startTime = workingHours.RestdayWorkStart;
                    endTime = workingHours.RestdayWorkEnd;
                }
                else
                {
                    startTime = workingHours.WorkdayWorkStart;
                    endTime = workingHours.WorkdayWorkEnd;
                }
            }
            else
            {
                var externalPolicy = OrderProcessService.LoadExternalPolicy(Order.Id);
                if (DateTime.Today.IsWeekend())
                {
                    startTime = externalPolicy.RestWorkTimeStart;
                    endTime = externalPolicy.RestWorkTimeEnd;
                }
                else
                {
                    startTime = externalPolicy.WorkTimeStart;
                    endTime = externalPolicy.WorkTimeEnd;
                }
            }
            if(Izual.Time.Now < startTime || Izual.Time.Now > endTime) {
                throw new CustomException("供应商已下班");
            }
        }
        protected void CheckInRefundingTimeOfProvider() {
            var workingHours = Service.Organization.CompanyService.GetWorkinghours(this.Order.Provider.CompanyId);
            Izual.Time startTime, endTime;
            if (this.Order.IsB3BOrder)
            {
                if (DateTime.Today.IsWeekend())
                {
                    startTime = workingHours.RestdayRefundStart;
                    endTime = workingHours.RestdayRefundEnd;
                }
                else
                {
                    startTime = workingHours.WorkdayRefundStart;
                    endTime = workingHours.WorkdayRefundEnd;
                }
            }
            else
            {
                var  externalPolicy = OrderProcessService.LoadExternalPolicy(Order.Id);
                if (DateTime.Today.IsWeekend())
                {
                    startTime = externalPolicy.RestRefundTimeStart;
                    endTime = externalPolicy.RestRefundTimeEnd;
                }
                else
                {
                    startTime = externalPolicy.WorkRefundTimeStart;
                    endTime = externalPolicy.WorkRefundTimeEnd;
                }
            }
            if(Izual.Time.Now < startTime || Izual.Time.Now > endTime) {
                throw new CustomException("供应商已下班");
            }
        }

        private void checkApplyformHistory() {
            foreach(var voyage in this.Applyform.Voyages) {
                var originalFlight = GetOriginalFlight(voyage);
                if (originalFlight == null) throw new CustomException("未找到航段信息，请先确认该行段是否已被取消或改期！"); //throw new ChinaPay.Core.Exception.NotFoundException("原航段[" + voyage + "]不存在");
                foreach(var passenger in this.Applyform.Passengers) {
                    var originalPassenger = GetOriginalPassenger(passenger);
                    if(originalPassenger == null) throw new ChinaPay.Core.Exception.NotFoundException("原乘机人[" + passenger.ToString() + "]不存在");

                    // 只要有未完成的申请，就不能再申请
                    var unfinishedApplyform = getUnfinishedApplyforms(passenger, voyage);
                    if(unfinishedApplyform != null) {
                        throw new ChinaPay.Core.CustomException(string.Format("乘机人【{0}】 航段【{1}】已经进行过相应申请。申请单号:{2},请不要重复提交,可以根据申请单号查看申请处理状态。",
                            originalPassenger.Name, GetFlightName(originalFlight), unfinishedApplyform.Id));
                    }

                    // 如果已经完成的申请是退废票申请，则不能再申请
                    var finishedApplyforms = getFinishedApplyforms(passenger, voyage);
                    if(finishedApplyforms.Any(item => item is Domain.Applyform.RefundOrScrapApplyform
                        && ((Domain.Applyform.RefundOrScrapApplyform)item).Status == RefundApplyformStatus.Refunded)) {
                        throw new ChinaPay.Core.CustomException(string.Format("乘机人[{0}] 航段[{1}]已退票",
                            originalPassenger.Name, GetFlightName(originalFlight)));
                    }
                }
            }
        }
        private Domain.Applyform.BaseApplyform getUnfinishedApplyforms(Guid passenger, Guid flight) {
            return this.Order.Applyforms.FirstOrDefault(item => item.Contains(passenger, flight) && item.ProcessStatus != ApplyformProcessStatus.Finished);
        }
        private IEnumerable<Domain.Applyform.BaseApplyform> getFinishedApplyforms(Guid passenger, Guid flight) {
            return this.Order.Applyforms.Where(item => item.Contains(passenger, flight) && item.ProcessStatus == ApplyformProcessStatus.Finished).ToList();
        }
        protected Passenger GetOriginalPassenger(Guid passenger) {
            return (from pnr in this.Order.PNRInfos
                    from op in pnr.Passengers
                    where op.Id == passenger
                    select op).FirstOrDefault();
        }
        protected Flight GetOriginalFlight(Guid flight) {
            return (from pnr in this.Order.PNRInfos
                    from of in pnr.Flights
                    where of.Id == flight
                    select of).FirstOrDefault();
        }
        protected string GetFlightName(Flight flight) {
            return flight.Departure.Name + "-" + flight.Arrival.Name;
        }
        public static ApplyChecker GetChecker(Order order, ApplyformView applyformView) {
            if(applyformView is RefundApplyformView) {
                return new RefundApplyChecker(order, applyformView as RefundApplyformView);
            } else if(applyformView is ScrapApplyformView) {
                return new ScrapApplyChecker(order, applyformView as ScrapApplyformView);
            } else if(applyformView is PostponeApplyformView) {
                return new PostponeApplyChecker(order, applyformView as PostponeApplyformView);
            } else if(applyformView is UpgradeApplyformView) {
                return new UpgradeApplyChecker(order, applyformView as UpgradeApplyformView);
            }
            else if (applyformView is BalanceRefundApplyView)
            {
                return new BalanceRefundApplyChecker(order, applyformView as BalanceRefundApplyView);
            }
            throw new NotSupportedException();
        }
    }

    internal class BalanceRefundApplyChecker : ApplyChecker
    {
        public BalanceRefundApplyChecker(Order order, BalanceRefundApplyView balanceRefundApplyView):base(order,balanceRefundApplyView) { 
        }
        protected override void CheckAccepterIsWorking() {  }

        protected override void CheckCore() { }

    }

    class RefundApplyChecker : ApplyChecker {
        public RefundApplyChecker(Order order, RefundApplyformView applyformView)
            : base(order, applyformView) {
        }

        protected override void CheckAccepterIsWorking() {
        }
        protected override void CheckCore() { }
    }
    class ScrapApplyChecker : ApplyChecker {
        public ScrapApplyChecker(Order order, ScrapApplyformView applyformView)
            : base(order, applyformView) {
        }

        protected override void CheckProductCanApply() {
            if(this.Order.ETDZTime.Value.Date != DateTime.Today)
                throw new ChinaPay.Core.CustomException("只能出票当天申请废票");
        }
        protected override void CheckAccepterIsWorking() {
            CheckInRefundingTimeOfProvider();
        }
        protected override void CheckCore() {
            foreach(var voyage in this.Applyform.Voyages) {
                var originalFlight = GetOriginalFlight(voyage);
                if((originalFlight.TakeoffTime - DateTime.Now).TotalHours < 2) {
                    throw new ChinaPay.Core.CustomException("航班起飞前2小时内不允许废票");
                }
            }

            // 不能把票号分开废
            foreach(var voyage in Applyform.Voyages) {
                var originalFlight = GetOriginalFlight(voyage);
                if(originalFlight.Ticket.Flights.Count() > 1) {
                    var otherFlight = originalFlight.Ticket.Flights.First(f => f.Id != voyage);
                    if(!Applyform.Voyages.Contains(otherFlight.Id))
                        throw new CustomException(string.Format("航段[{0}]与[{1}]属于同一张票，不能单独作废", GetFlightName(originalFlight), GetFlightName(otherFlight)));
                }
            }
        }
    }
    class PostponeApplyChecker : ApplyChecker {
        public PostponeApplyChecker(Order order, PostponeApplyformView applyformView)
            : base(order, applyformView) {
        }

        protected override void CheckProductCanApply() {
            if(this.Order.IsSpecial) throw new CustomException("特殊票不允许改期");
        }
        protected override void CheckAccepterIsWorking() {
        }
        protected override void CheckCore() {
            var postponeApplyform = this.Applyform as PostponeApplyformView;
            foreach(var voyage in this.Applyform.Voyages) {
                var postponeApplyFlight = getPostponeApplyformFlight(postponeApplyform, voyage);
                if(postponeApplyFlight.NewFlightDate.Date < DateTime.Now.Date)
                    throw new CustomException("不能改期到今天以前的航班");
            }
        }

        private PostponeApplyformView.Item getPostponeApplyformFlight(PostponeApplyformView postponeApplyformView, Guid voyage) {
            return postponeApplyformView.Items.First(item => item.Voyage == voyage);
        }
    }
    class UpgradeApplyChecker : ApplyChecker {
        public UpgradeApplyChecker(Order order, UpgradeApplyformView applyformView)
            : base(order, applyformView) {
        }

        protected override void CheckProductCanApply() {
            if(this.Order.IsSpecial) throw new CustomException("特殊票不允许升舱");
            if(this.Order.IsChildrenOrder) throw new CustomException("不支持儿童票升舱");
            if(this.Order.IsTeam) throw new CustomException("团队票不能升舱");
            switch(this.Order.TripType) {
                case DataTransferObject.Command.PNR.ItineraryType.OneWay:
                case DataTransferObject.Command.PNR.ItineraryType.Roundtrip:
                    break;
                default:
                    throw new CustomException("仅支持单程和往返的升舱");
            }
        }
        protected override void CheckAccepterIsWorking() {
            CheckInWorkingTimeOfProvider();
        }
        protected override void CheckCore() {
            var upgradeApplyform = this.Applyform as UpgradeApplyformView;
            foreach(var voyage in this.Applyform.Voyages) {
                var originalFlight = GetOriginalFlight(voyage);

                var upgradeApplyFlight = getUpgradeApplyformItem(upgradeApplyform, voyage);
                if(upgradeApplyFlight.Flight.TakeoffTime.Date < DateTime.Now.Date)
                    throw new CustomException("不能升舱到今天以前的航班");
                if(upgradeApplyFlight.Flight.TakeoffTime < DateTime.Now)
                    throw new CustomException("新航班不能为已失效的航班");
                if(originalFlight.Carrier.Code != upgradeApplyFlight.Flight.Airline)
                    throw new CustomException("必须是同一航空公司航班才能升舱");
                var newBunkDiscount = getGeneralBunkDiscount(upgradeApplyFlight.Flight);
                if(newBunkDiscount < 1)
                    throw new CustomException("高舱位必须是100折及以上折扣舱位");
                if(originalFlight.Bunk is Bunk.GeneralBunk) {
                    var originalBunkDiscount = originalFlight.Bunk.Discount;
                    if(originalBunkDiscount >= newBunkDiscount)
                        throw new CustomException("新舱位必须比原舱位的折扣高");
                }
            }
        }

        private UpgradeApplyformView.Item getUpgradeApplyformItem(UpgradeApplyformView upgradeApplyformView, Guid voyage) {
            return upgradeApplyformView.Items.First(item => item.Voyage == voyage);
        }
        private decimal getGeneralBunkDiscount(FlightView flightView) {
            var bunks = FoundationService.QueryBunk(flightView.Airline, flightView.Departure, flightView.Arrival, flightView.TakeoffTime, flightView.Bunk);
            var generalBunk = bunks.FirstOrDefault(item => item is Foundation.Domain.GeneralBunk);
            if(generalBunk != null) {
                return (generalBunk as Foundation.Domain.GeneralBunk).GetDiscount(flightView.Bunk);
            }
            throw new CustomException("无明折明扣相关数据");
        }
    }
}