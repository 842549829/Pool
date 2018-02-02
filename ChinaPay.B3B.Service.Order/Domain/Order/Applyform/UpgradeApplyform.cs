using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform {
    /// <summary>
    /// 升舱申请
    /// </summary>
    internal class UpgradeApplyform : BaseApplyform {
        List<FlightView> _flights = new List<FlightView>();

        internal UpgradeApplyform(UpgradeApplyformView upgradeApplyformView, Order order)
            : base(order, upgradeApplyformView) {
            this.NewPNR = upgradeApplyformView.NewPNR;
            this.Source = upgradeApplyformView.PNRSource;
            this.PATPrice = upgradeApplyformView.PATPrice;
            foreach(var item in upgradeApplyformView.Items) {
                _flights.Add(item.Flight);
            }
        }

        public OrderSource Source { get; private set; }
        /// <summary>
        /// 原航段信息
        /// </summary>
        public IEnumerable<FlightView> Flights {
            get {
                return _flights.AsReadOnly();
            }
        }
        public override IEnumerable<Flight> OriginalFlights {
            get { throw new InvalidOperationException(); }
        }
        /// <summary>
        /// 处理状态
        /// </summary>
        public override ApplyformProcessStatus ProcessStatus {
            get {
                return ApplyformProcessStatus.Finished;
            }
        }
        /// <summary>
        /// 新订单号
        /// </summary>
        public decimal NewOrderId { get; internal set; }
        /// <summary>
        /// PAT价格
        /// </summary>
        public DataTransferObject.Command.PNR.PriceView PATPrice { get; private set; }

        public override string ToString() {
            return "升舱";
        }

        internal override IEnumerable<Guid> GetAppliedFlights() {
            return new List<Guid>();
        }
    }
}