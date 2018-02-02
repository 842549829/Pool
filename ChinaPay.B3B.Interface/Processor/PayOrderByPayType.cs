using System.Web;

namespace ChinaPay.B3B.Interface.Processor {
    internal class PayOrderByPayType : RequestProcessor {
        protected override string ExecuteCore() {
            var orderIdText = Context.GetParameterValue("id");
            var payInterfaceCode = Context.GetParameterValue("payType");
            if(string.IsNullOrWhiteSpace(orderIdText)) InterfaceInvokeException.ThrowParameterMissException("id");
            if(string.IsNullOrWhiteSpace(payInterfaceCode)) InterfaceInvokeException.ThrowParameterMissException("payType");

            decimal orderId;
            var url = string.Empty;
            if(decimal.TryParse(orderIdText, out orderId)) {
                string message;
                if(Service.OrderProcessService.Payable(orderId, out message)) {
                    var payInterface = PayUtility.GetPayInterface(payInterfaceCode);
                    url = Service.Tradement.PaymentService.OnlinePayOrder(orderId, payInterface, Context.ClientIP, Employee.UserName);
                } else {
                    InterfaceInvokeException.ThrowException("9", message);
                }
            } else {
                InterfaceInvokeException.ThrowParameterErrorException("id");
            }
            return "<payUrl>" + HttpUtility.UrlEncode(url) + "</payUrl>";
        }
    }
}