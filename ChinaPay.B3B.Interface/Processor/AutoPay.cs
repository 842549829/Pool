using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.Interface.Processor
{
    /// <summary>
    /// 代扣
    /// </summary>
    internal class AutoPay : RequestProcessor
    {
        protected override string ExecuteCore()
        {
            var orderId = Context.GetParameterValue("id");
            var payType = Context.GetParameterValue("payType");
            var orderType = Context.GetParameterValue("businessType");
            if (string.IsNullOrWhiteSpace(orderId)) InterfaceInvokeException.ThrowParameterMissException("id");
            if (string.IsNullOrWhiteSpace(payType) || (payType != "0" && payType != "1")) InterfaceInvokeException.ThrowParameterMissException("payType");
            if (string.IsNullOrWhiteSpace(orderType) || (orderType != "0" && orderType != "1")) InterfaceInvokeException.ThrowParameterMissException("businessType");
            decimal oid;
            string msg = "";
            if (decimal.TryParse(orderId, out oid))
            {
                decimal amount = 0M;
                if (orderType == "0")
                {
                    OrderProcessService.Payable(oid, out msg);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        InterfaceInvokeException.ThrowCustomMsgException(msg);
                    }
                    var orderInfo = OrderQueryService.QueryOrder(oid);
                    if (orderInfo.Purchaser.CompanyId != Company.CompanyId) InterfaceInvokeException.ThrowCustomMsgException("暂无此订单");
                    amount = orderInfo.Purchaser.Amount;
                }
                else if (orderType == "1")
                {
                    ApplyformProcessService.Payable(oid, out msg);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        InterfaceInvokeException.ThrowCustomMsgException(msg);
                    }
                    var orderInfo = ApplyformQueryService.QueryPostponeApplyform(oid);
                    if (orderInfo.Purchaser.CompanyId != Company.CompanyId) InterfaceInvokeException.ThrowCustomMsgException("暂无此订单");
                    amount = orderInfo.PayBill.Applier.Amount;
                }
                if (AutoPayService.QueryAuto(oid) != null) InterfaceInvokeException.ThrowCustomMsgException("存在重复的代扣记录");
                var auto = AccountService.GetWithholding((WithholdingAccountType)byte.Parse(payType), Employee.Owner);
                if (auto == null || auto.Status == WithholdingProtocolStatus.Submitted) InterfaceInvokeException.ThrowCustomMsgException("该账户还没有进行代扣设置，请先登录平台进行设置！");
                if (payType == "0" && amount > auto.Amount) InterfaceInvokeException.ThrowCustomMsgException("订单[ " + orderId + " ] 需要支付的金额超过的代扣金额上限！");
                AutoPayService.InsertAutoPay(new ChinaPay.B3B.Service.Order.Domain.AutoPay.AutoPay() { PayAccountNo = auto.AccountNo, PayType = (WithholdingAccountType)byte.Parse(payType), OrderId = oid, OrderType = (OrderType)byte.Parse(orderType), ProcessState = false, Success = false, Time = DateTime.Now });
            }
            else
            {
                InterfaceInvokeException.ThrowParameterMissException("orderId");
            }
            return "";
        }
    }
}