using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order.External;

namespace ChinaPay.B3B.Service.ExternalPlatform.Processor {
    interface IOrderProcessor {
        /// <summary>
        /// 生成订单
        /// </summary>
        RequestResult<ExternalOrderView> Produce(decimal orderId, DataTransferObject.Order.OrderView orderView, DataTransferObject.Policy.ExternalPolicyView policy);
        /// <summary>
        /// 生成订单
        /// </summary>
        RequestResult<ExternalOrderView> Produce(decimal orderId, DataTransferObject.Order.OrderView orderView, string pnrContent, string patContent, DataTransferObject.Policy.ExternalPolicyView policy);
        /// <summary>
        /// 自动支付订单
        /// 代扣
        /// </summary>
        RequestResult<AutoPayResult> AutoPay(decimal orderId, string externalOrderId, PayInterface payInterface, decimal amount);
        /// <summary>
        /// 手动支付订单
        /// 获取支付地址
        /// </summary>
        RequestResult<string> ManualPay(decimal orderId, string externalOrderId, PayInterface payInterface, decimal amount);
        /// <summary>
        /// 取消订单
        /// </summary>
        RequestResult<bool> Cancel(decimal orderId, string externalOrderId, IEnumerable<string> passengers, string reason);
        /// <summary>
        /// 查询支付信息
        /// </summary>
        RequestResult<Payment> QueryPayment(decimal orderId, string externalOrderId);
        /// <summary>
        /// 查询票号信息
        /// </summary>
        RequestResult<TicketInfo> QueryTicketNo(decimal orderId, string externalOrderId);
    }
}