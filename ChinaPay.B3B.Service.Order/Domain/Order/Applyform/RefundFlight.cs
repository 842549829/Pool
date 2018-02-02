namespace ChinaPay.B3B.Service.Order.Domain.Applyform {
    public class RefundFlight {
        /// <summary>
        /// 原航班信息
        /// </summary>
        public Flight OriginalFlight {
            get;
            internal set;
        }
        /// <summary>
        /// 退还服务费
        /// </summary>
        public decimal? RefundServiceCharge {
            get;
            internal set;
        }
        /// <summary>
        /// 退/废票手续费率
        /// </summary>
        public decimal RefundRate {
            get;
            internal set;
        }

        /// <summary>
        /// 差错金额
        /// </summary>
        public decimal? BanlanceFare { get; set; }
    }
}