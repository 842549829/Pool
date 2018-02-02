using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChinaPay.B3B.Interface.Processor
{
    class ManualPay : RequestProcessor
    {
        protected override string ExecuteCore()
        {
            var idText = Context.GetParameterValue("id");
            var payInterfaceCode = Context.GetParameterValue("payType");
            var businessType = Context.GetParameterValue("businessType");
            if (string.IsNullOrWhiteSpace(idText)) InterfaceInvokeException.ThrowParameterMissException("id");
            if (string.IsNullOrWhiteSpace(payInterfaceCode)) InterfaceInvokeException.ThrowParameterMissException("payType");
            if (string.IsNullOrWhiteSpace(businessType)) InterfaceInvokeException.ThrowParameterMissException("businessType");
            decimal id;
            var url = string.Empty;
            if (decimal.TryParse(idText, out id))
            {
                string message;
                if (businessType == "0")
                {
                    if (Service.OrderProcessService.Payable(id, out message))
                    {
                        var payInterface = PayUtility.GetPayInterface(payInterfaceCode);
                        url = Service.Tradement.PaymentService.OnlinePayOrder(id, payInterface, Context.ClientIP, Employee.UserName);
                    }
                    else
                    {
                        InterfaceInvokeException.ThrowException("9", message);
                    }
                }
                else if (businessType == "1")
                {
                    if (Service.ApplyformProcessService.Payable(id, out message))
                    {
                        var payInterface = PayUtility.GetPayInterface(payInterfaceCode);
                        url = Service.Tradement.PaymentService.OnlinePayPostponeFee(id, payInterface, Context.ClientIP, Employee.UserName);
                    }
                    else
                    {
                        InterfaceInvokeException.ThrowException("9", message);
                    }
                }
            }
            else
            {
                InterfaceInvokeException.ThrowParameterErrorException("id");
            }
            return "<payUrl>" + HttpUtility.UrlEncode(url) + "</payUrl>";
        }
    }
}