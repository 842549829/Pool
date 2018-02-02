namespace ChinaPay.B3B.Service.Statistic {
    public class SpecialProductStatisticInfo {
        /// <summary>
        /// 订单数
        /// </summary>
        public int OrderCount { get; internal set; }
        /// <summary>
        /// 成功订单数
        /// </summary>
        public int SuccessOrderCount { get; internal set; }
        /// <summary>
        /// 票数
        /// </summary>
        public int TicketCount { get; internal set; }
        /// <summary>
        /// 成功票数
        /// </summary>
        public int SuccessTicketCount { get; internal set; }

        /// <summary>
        /// 订单成功率
        /// </summary>
        public decimal OrderSuccessRate {
            get {
                if(OrderCount == 0) return 0;
                var rate = (decimal)SuccessOrderCount / OrderCount;
                return ChinaPay.Utility.Calculator.Round(rate, -2);
            }
        }
        /// <summary>
        /// 客票成功率
        /// </summary>
        public decimal TicketSuccessRate {
            get {
                if(TicketCount == 0) return 0;
                var rate = (decimal)SuccessTicketCount / TicketCount;
                return ChinaPay.Utility.Calculator.Round(rate, -2);
            }
        }
    }
}