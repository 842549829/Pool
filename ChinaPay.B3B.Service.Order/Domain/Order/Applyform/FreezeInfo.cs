using System;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform {
    public abstract class FreezeBaseInfo {
        protected FreezeBaseInfo()
            : this(Guid.NewGuid()) {
        }
        protected FreezeBaseInfo(Guid id) {
            this.Id = id;
        }
        public Guid Id { get; private set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal OrderId { get; internal set; }
        /// <summary>
        /// 申请单号
        /// </summary>
        public decimal ApplyformId { get; internal set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; internal set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; internal set; }
        /// <summary>
        /// 处理批次号
        /// </summary>
        public string No { get; internal set; }
        /// <summary>
        /// 提交请求时间
        /// </summary>
        public DateTime RequestTime { get; internal set; }
        /// <summary>
        /// 处理是否成功
        /// </summary>
        public bool Success { get; internal set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessedTime { get; internal set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; internal set; }
    }
    public class FreezeInfo : FreezeBaseInfo {
        internal FreezeInfo() {
        }
        internal FreezeInfo(Guid id)
            : base(id) {
        }

        /// <summary>
        /// 支付交易流水号
        /// </summary>
        public string TradeNo { get; internal set; }
    }
    public class UnfreezeInfo : FreezeBaseInfo {
        internal UnfreezeInfo() {
        }
        internal UnfreezeInfo(Guid id)
            : base(id) {
        }
        /// <summary>
        /// 冻结批次号
        /// </summary>
        public string FreezeNo { get; internal set; }
    }
}