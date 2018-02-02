using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill {
    public abstract class BillBase {
        protected BillBase(decimal id) {
            Id = id;
        }

        public decimal Id { get; private set; }
        /// <summary>
        /// 角色账单
        /// </summary>
        public abstract IEnumerable<RoleBill> RoleBills { get; }
        /// <summary>
        /// 交易信息
        /// </summary>
        public abstract Tradement.Tradement TradementBase { get; }
        /// <summary>
        /// 平台基础利润信息
        /// </summary>
        public abstract PlatformBasicProfit PlatformBasicProfit { get; }
        /// <summary>
        /// 交易成功时间
        /// </summary>
        public abstract DateTime? TradeTime { get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; internal set; }
    }
    public class PlatformBasicProfit {
        /// <summary>
        /// 平台收取用户的交易手续费
        /// </summary>
        public decimal TradeFee { get; internal set; }
        /// <summary>
        /// 溢价
        /// </summary>
        public decimal Premium { get; internal set; }
        public string Account { get; internal set; }
        public bool Success { get; internal set; }
    }
}