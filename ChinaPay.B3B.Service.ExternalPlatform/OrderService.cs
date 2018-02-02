using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order.External;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.ExternalPlatform {
    public static class OrderService {
        /// <summary>
        /// 生成订单
        /// </summary>
        public static RequestResult<ExternalOrderView> Produce(decimal orderId, DataTransferObject.Order.OrderView orderView, DataTransferObject.Policy.ExternalPolicyView policy) {
            if(orderView == null) throw new ArgumentNullException("orderView");
            if(policy == null) throw new ArgumentNullException("policy");

            var processor = createOrderProcessor(policy.Platform);
            return processor.Produce(orderId, orderView, policy);
        }
        /// <summary>
        /// 生成订单
        /// </summary>
        public static RequestResult<ExternalOrderView> Produce(decimal orderId, DataTransferObject.Order.OrderView orderView, string pnrContent, string patContent, DataTransferObject.Policy.ExternalPolicyView policy) {
            if(orderView == null) throw new ArgumentNullException("orderView");
            if(string.IsNullOrWhiteSpace(pnrContent)) throw new ArgumentNullException("pnrContent");
            if(string.IsNullOrWhiteSpace(patContent)) throw new ArgumentNullException("patContent");
            if(policy == null) throw new ArgumentNullException("policy");

            pnrContent = pnrContent.RemovePrintedContent().RemoveETermSpecialContentOnWeb();
            var processor = createOrderProcessor(policy.Platform);
            return processor.Produce(orderId, orderView, pnrContent, patContent, policy);
        }
        /// <summary>
        /// 自动支付
        /// </summary>
        public static RequestResult<AutoPayResult> Pay(Common.Enums.PlatformType platformType, decimal orderId, string externalOrderId, decimal amount) {
            var platform = Processor.PlatformBase.GetPlatform(platformType);
            return Pay(platformType, orderId, externalOrderId, amount, platform.Setting.PayInterface);
        }
        /// <summary>
        /// 自动支付
        /// </summary>
        public static RequestResult<AutoPayResult> Pay(Common.Enums.PlatformType platformType, decimal orderId, string externalOrderId, decimal amount, PayInterface payInterface) {
            return Pay(platformType, orderId, externalOrderId, amount, new[] { payInterface });
        }
        /// <summary>
        /// 自动支付
        /// </summary>
        private static RequestResult<AutoPayResult> Pay(Common.Enums.PlatformType platformType, decimal orderId, string externalOrderId, decimal amount, IEnumerable<PayInterface> payInterfaces) {
            if(string.IsNullOrWhiteSpace(externalOrderId)) throw new ArgumentNullException("externalOrderId");
            LogService.SaveTextLog("自动支付方式：" + payInterfaces.Join(",", p => p.ToString()));
            RequestResult<AutoPayResult> result = null;
            var processor = createOrderProcessor(platformType);
            foreach(var payInterface in payInterfaces) {
                result = processor.AutoPay(orderId, externalOrderId, payInterface, amount);
                if(result.Success && result.Result.Success) {
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取手动支付地址
        /// </summary>
        public static RequestResult<string> GetPayUrl(Common.Enums.PlatformType platformType, decimal orderId, string externalOrderId, PayInterface payInterface, decimal amount) {
            if(string.IsNullOrWhiteSpace(externalOrderId)) throw new ArgumentNullException("externalOrderId");

            var platform = Processor.PlatformBase.GetPlatform(platformType);
            if(platform.SuportManualPay()) {
                var processor = platform.GetOrderProcessor();
                return processor.ManualPay(orderId, externalOrderId, payInterface, amount);
            } else {
                return new RequestResult<string> {
                    Success = false,
                    ErrMessage = "不支持手动支付，请到" + platform.PlatformInfo.GetDescription() + "平台进行支付"
                };
            }
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        public static RequestResult<bool> Cancel(Common.Enums.PlatformType platformType, decimal orderId, string externalOrderId, IEnumerable<string> passengers, string reason) {
            if(string.IsNullOrWhiteSpace(externalOrderId)) throw new ArgumentNullException("externalOrderId");
            if(string.IsNullOrWhiteSpace(reason)) throw new ArgumentNullException("reason");

            var processor = createOrderProcessor(platformType);
            return processor.Cancel(orderId, externalOrderId, passengers, reason);
        }
        /// <summary>
        /// 查询支付信息
        /// </summary>
        public static RequestResult<Payment> QueryPayment(Common.Enums.PlatformType platformType, decimal orderId, string externalOrderId) {
            var processor = createOrderProcessor(platformType);
            return processor.QueryPayment(orderId, externalOrderId);
        }
        /// <summary>
        /// 查询票号信息
        /// </summary>
        public static RequestResult<TicketInfo> QueryTicketNo(Common.Enums.PlatformType platformType, decimal orderId, string externalOrderId) {
            var processor = createOrderProcessor(platformType);
            return processor.QueryTicketNo(orderId, externalOrderId);
        }

        private static Processor.IOrderProcessor createOrderProcessor(Common.Enums.PlatformType platformType) {
            var platform = Processor.PlatformBase.GetPlatform(platformType);
            return platform.GetOrderProcessor();
        }
    }
}