using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.Distribution.Domain.Tradement {
    /// <summary>
    /// 支付信息
    /// </summary>
    public class Payment : Domain.Tradement.Tradement {
        internal Payment()
            : this(System.Guid.NewGuid()) {
        }
        internal Payment(System.Guid id)
            : base(id) {
        }

        internal void PaySuccess(string payAccount, string tradeNo, PayInterface payInterface, PayAccountType payAccountType, string channelTradeNo) {
            PayAccount = payAccount;
            TradeNo = tradeNo;
            IsPoolpay = payInterface == DataTransferObject.Common.PayInterface.Virtual;
            PayInterface = payInterface;
            PayAccountType = payAccountType;
            ChannelTradeNo = channelTradeNo;
        }
        internal Refundment MakeRefundment(decimal refundAmount, decimal refundedTradeFee, string refundTradeNo) {
            var refundment = new Refundment {
                Payment = this,
                PayAccount = this.PayAccount,
                PayInterface = this.PayInterface,
                PayAccountType = this.PayAccountType,
                PayeeAccount = this.PayeeAccount,
                IsPoolpay = this.IsPoolpay,
                TradeRate = this.TradeRate,
                Amount = refundAmount,
                TradeNo = refundTradeNo,
                ChannelTradeNo = refundTradeNo
            };
            // 防止退还交易手续费 超过 收取的交易手续费
            if(refundment.TradeFee + refundedTradeFee > TradeFee) {
                refundment.TradeFee = TradeFee - refundedTradeFee;
            }
            return refundment;
        }

        public override string ToString() {
            return "支付";
        }
    }
}