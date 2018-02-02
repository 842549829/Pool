using System;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.Core;
using ChinaPay.Gateway.Tradement.Pay;
using ChinaPay.SMS.Service;
using AccountType = ChinaPay.B3B.Common.Enums.AccountType;

namespace ChinaPay.B3B.Service.Tradement {
    /// <summary>
    /// 支付服务
    /// </summary>
    public static class PaymentService {
        internal const string TicketOrderPayType = "Order";
        internal const string PostponeApplyformPayType = "Postpone";
        internal const string SMSOrderPayType = "SMS";

        /// <summary>
        /// 支付订单
        /// 在线方式支付
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="bankInfo">银行信息</param>
        /// <param name="clientIP">客户端IP</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static string OnlinePayOrder(decimal orderId, string bankInfo, string clientIP, string operatorAccount) {
            var order = OrderQueryService.QueryOrder(orderId);
            var bankInfoArray = bankInfo.Split('|');
            var channelId = int.Parse(bankInfoArray[0]);
            var bankCode = bankInfoArray[1];

            var payAccountNo = getPayAccountNo(order.Purchaser.CompanyId);
            var payOrderRequest = new PayOrderRequestProcess(orderId, order.Purchaser.Amount,
                payAccountNo, order.Bill.PayBill.Tradement.PayeeAccount, "支付机票款",
                order.ReservationPNR == null ? string.Empty : (order.ReservationPNR.PNR ?? order.ReservationPNR.BPNR),
                TicketOrderPayType + "|" + payAccountNo + "|" + operatorAccount,channelId.ToString(),bankCode);
            payOrderRequest.Execute();
            return payOrderRequest.PayUrl;
        }

        /// <summary>
        /// 支付改期手续费
        /// 在线方式支付
        /// </summary>
        /// <param name="postponeApplyformId">改期申请单号</param>
        /// <param name="bankInfo">银行信息</param>
        /// <param name="clientIP">客户端IP</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static string OnlinePayPostponeFee(decimal postponeApplyformId, string bankInfo, string clientIP, string operatorAccount) {
            var applyform = ApplyformQueryService.QueryPostponeApplyform(postponeApplyformId);
            var payAccount = getPayAccountNo(applyform.PurchaserId);
            var bankInfoArray = bankInfo.Split('|');
            var channelId = int.Parse(bankInfoArray[0]);
            var bankCode = bankInfoArray[1];

            var postPoneRequest = new PayPostPoneRequestProcess(applyform.OrderId, postponeApplyformId,
                Math.Abs(applyform.PayBill.Applier.Amount), payAccount,
                applyform.PayBill.Tradement.PayeeAccount, "支付改期费",
                applyform.OriginalPNR == null ? string.Empty : (applyform.OriginalPNR.PNR ?? applyform.OriginalPNR.BPNR),
                PostponeApplyformPayType + "|" + payAccount + "|" + operatorAccount + "|",
                channelId.ToString(), bankCode);
            postPoneRequest.Execute();
            return postPoneRequest.PayUrl;
        }

        /// <summary>
        /// 支付购买的短信
        /// 在线方式支付
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="bankInfo">银行信息</param>
        /// <param name="clientIP">客户端IP</param>
        /// <param name="operatorAccount">操作员账号</param>
        /// <param name="companyId"> </param>
        public static string OnlinePaySMSOrder(decimal orderId, string bankInfo, string clientIP, string operatorAccount, Guid companyId)
        {
            var order = SMSOrderService.QueryOrder(orderId);
            var bankInfoArray = bankInfo.Split('|');
            var channelId = bankInfoArray[0];
            var bankCode = bankInfoArray[1];
            var payAccount = getPayAccountNo(companyId);

            var smsPayRequest = new PaySMSRequestProcess(orderId, order.TotalAmount, payAccount, SystemManagement.SystemParamService.QueryParam(SystemManagement.Domain.SystemParamType.SMSIncomeAccount).Value, "支付短信票款", string.Empty,
                SMSOrderPayType + "|" + payAccount + "|" + operatorAccount, channelId, bankCode);
            smsPayRequest.Execute();
            return smsPayRequest.PayUrl;
        }

        private static string getPayAccountNo(Guid purchaser) {
            Account payAccount = AccountService.Query(purchaser, AccountType.Payment);
            if(payAccount == null) {
                throw new CustomException("缺少付款账号，不能支付");
            } else if(!payAccount.Valid) {
                throw new CustomException("付款账号无效，不能支付");
            }
            return payAccount.No;
        }
    }
}