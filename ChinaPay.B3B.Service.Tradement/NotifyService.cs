using System;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core.Extension;
using ChinaPay.SMS.Service;

namespace ChinaPay.B3B.Service.Tradement {
    /// <summary>
    /// 通知服务
    /// </summary>
    public static class NotifyService {
        /// <summary>
        /// 支付成功
        /// </summary>
        /// <param name="id">业务Id</param>
        /// <param name="remark">备注</param>
        /// <param name="payTradeNo">支付交易流水号</param>
        /// <param name="payTime">支付时间</param>
        /// <param name="payInterface">支付接口</param>
        /// <param name="payAccountType">支付账号类型</param>
        /// <param name="payAccount">付款账号</param>
        /// <returns>返回是否处理成功</returns>
        public static bool PaySuccess(decimal id, string remark, string payTradeNo, string channelTradeNo, DateTime payTime, string payInterface, string payAccountType, string payAccount)
        {
            var paramArray = remark.Split('|');
            if(paramArray.Length >= 3) {
                var paymentType = paramArray[0];
                //var payAccount = paramArray[1];
                var operatorAccount = paramArray[2];
                if(Tradement.PaymentService.TicketOrderPayType == paymentType) {
                    return OrderPaySuccess(id, payAccount, payTradeNo, channelTradeNo, payTime, ParsePayInterface(payInterface), ParsePayAccountType(payAccountType), operatorAccount);
                } else if(Tradement.PaymentService.PostponeApplyformPayType == paymentType) {
                    return PostponeApplyformPaySuccess(id, payAccount, payTradeNo, channelTradeNo, payTime, ParsePayInterface(payInterface), ParsePayAccountType(payAccountType), operatorAccount);
                } else if(Tradement.PaymentService.SMSOrderPayType == paymentType) {
                    return SMSApplyformPaySuccess(id, payAccount, payTradeNo, payTime, ParsePayInterface(payInterface), ParsePayAccountType(payAccountType), operatorAccount);
                }
            }
            return false;
        }
        static bool OrderPaySuccess(decimal orderId, string payAccount, string payTradeNo, string channelTradeNo, DateTime payTime, PayInterface payInterface, PayAccountType payAccountType, string operatorAccount)
        {
            var result = false;
            var request = string.Format("订单:{0} 支付账号:{1} 流水号:{2} 支付时间:{3} 支付接口:{4} 支付账号类型:{5} 操作员账号:{6} 通道流水号:{7} ",
                orderId, payAccount, payTradeNo, payTime, payInterface.GetDescription(), payAccountType.GetDescription(), operatorAccount,channelTradeNo);
            var response = string.Empty;
            try {
                OrderProcessService.PaySuccess(orderId, payAccount, payTradeNo, channelTradeNo, payTime, payInterface, payAccountType, operatorAccount);
                result = true;
                response = "处理成功";
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "订单支付通知 " + request);
                response = "处理失败 " + ex.Message;
            }
            var tradementLog = new Log.Domain.TradementLog {
                OrderId = orderId,
                Request = request,
                Response = response,
                Time = DateTime.Now,
                Remark = "支付成功通知",
            };
            LogService.SaveTradementLog(tradementLog);
            return result;
        }
        static bool PostponeApplyformPaySuccess(decimal applyformId, string payAccount, string payTradeNo, string channelTradeNo, DateTime payTime, PayInterface payInterface, PayAccountType payAccountType, string account)
        {
            var result = false;
            decimal? orderId = null;
            var request = string.Format("申请单:{0} 支付账号:{1} 流水号:{2} 支付时间:{3} 支付接口:{4} 支付账号类型:{5} 操作员账号:{6}  通道流水号:{7} ",
                applyformId, payAccount, payTradeNo, payTime, payInterface.GetDescription(), payAccountType.GetDescription(), account, channelTradeNo);
            var response = string.Empty;
            try {
                orderId = ApplyformProcessService.PostponeFeePaySuccess(applyformId, payAccount, payTradeNo, channelTradeNo, payTime, payInterface, payAccountType, account);
                result = true;
                response = "处理成功";
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "申请单支付通知 " + request);
                response = "处理失败 " + ex.Message;
            }
            if(orderId.HasValue) {
                var tradementLog = new Log.Domain.TradementLog {
                    OrderId = orderId.Value,
                    ApplyformId = applyformId,
                    Request = request,
                    Response = response,
                    Time = DateTime.Now,
                    Remark = "支付成功通知"
                };
                LogService.SaveTradementLog(tradementLog);
            }
            return result;
        }
        static bool SMSApplyformPaySuccess(decimal applyformId, string payAccount, string payTradeNo, DateTime payTime, PayInterface payInterface, PayAccountType payAccountType, string account) {
            var result = false;
            decimal? orderId = null;
            var request = string.Format("订单:{0} 支付账号:{1} 流水号:{2} 支付时间:{3} 支付接口:{4} 支付账号类型:{5} 操作员账号:{6}",
                applyformId, payAccount, payTradeNo, payTime, payInterface.GetDescription(), payAccountType.GetDescription(), account);
            var response = string.Empty;
            try {
                orderId = applyformId;
                SMSOrderService.PaySuccess(applyformId, payAccount, payTradeNo, payInterface == PayInterface.Virtual, payTime, account);
                result = true;
                response = "支付成功";
            } catch(Exception ex) {
                LogService.SaveExceptionLog(ex, "短信购买支付通知 " + request);
                response = "处理失败 " + ex.Message;
            }
            if(orderId.HasValue) {
                var tradementLog = new Log.Domain.TradementLog {
                    OrderId = orderId.Value,
                    ApplyformId = applyformId,
                    Request = request,
                    Response = response,
                    Time = DateTime.Now,
                    Remark = "支付成功通知"
                };
                LogService.SaveTradementLog(tradementLog);
            }
            return result;
        }
        public static PayInterface ParsePayInterface(string code)
        {
            if (code == "Alipay")
                return PayInterface.Alipay;
            if (code == "ChinaPnr")
                return PayInterface.ChinaPnr;
            if (code == "Tenpay")
                return PayInterface.Tenpay;
            if (code == "99Bill")
                return PayInterface._99Bill;
            return PayInterface.Virtual;
        }
        public static PayAccountType ParsePayAccountType(string code) {
            if(code == "1")
                return PayAccountType.Credit;
            return PayAccountType.Cash;
        }
    }
}