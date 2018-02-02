using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.Distribution.Domain.Tradement {
    /// <summary>
    /// 交易信息
    /// </summary>
    public abstract class Tradement {
        decimal? _tradeRate = null, _tradeFee = null;

        protected Tradement(System.Guid id) {
            Id = id;
        }

        public System.Guid Id { get; private set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal Amount { get; internal set; }
        /// <summary>
        /// 手续费率
        /// </summary>
        public decimal TradeRate {
            get {
                if(!_tradeRate.HasValue) {
                    _tradeRate = SystemManagement.SystemParamService.TradeRate;
                }
                return _tradeRate.Value;
            }
            internal set {
                _tradeRate = value;
            }
        }
        /// <summary>
        /// 手续费
        /// </summary>
        public decimal TradeFee {
            get {
                if(!_tradeFee.HasValue) {
                    _tradeFee = Utility.Calculator.Round(this.Amount * this.TradeRate, -2);
                }
                return _tradeFee.Value;
            }
            internal set {
                _tradeFee = value;
            }
        }
        /// <summary>
        /// 支付账号
        /// </summary>
        public string PayAccount { get; internal set; }
        /// <summary>
        /// 支付接口
        /// </summary>
        public PayInterface? PayInterface { get; internal set; }
        /// <summary>
        /// 支付账号类型
        /// </summary>
        public PayAccountType? PayAccountType { get; internal set; }
        /// <summary>
        /// 收款账号
        /// </summary>
        public string PayeeAccount { get; internal set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string TradeNo { get; internal set; }
        /// <summary>
        /// 通道交易流水号
        /// </summary>
        public string ChannelTradeNo { get; internal set; }
        /// <summary>
        /// 是否国付通账号支付
        /// </summary>
        public bool IsPoolpay { get; internal set; }
    }
}