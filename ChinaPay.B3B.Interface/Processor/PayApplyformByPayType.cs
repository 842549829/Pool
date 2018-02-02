using System.Web;

namespace ChinaPay.B3B.Interface.Processor {
    internal class PayApplyformByPayType : RequestProcessor {
        protected override string ExecuteCore() {
            var applyformIdText = Context.GetParameterValue("id");
            var payInterfaceCode = Context.GetParameterValue("payType");
            if(string.IsNullOrWhiteSpace(applyformIdText)) InterfaceInvokeException.ThrowParameterMissException("id");
            if(string.IsNullOrWhiteSpace(payInterfaceCode)) InterfaceInvokeException.ThrowParameterMissException("payType");

            decimal applyformId;
            var url = string.Empty;
            if(decimal.TryParse(applyformIdText, out applyformId)) {
                string message;
                if(Service.ApplyformProcessService.Payable(applyformId, out message)) {
                    var payInterface = PayUtility.GetPayInterface(payInterfaceCode);
                    url = Service.Tradement.PaymentService.OnlinePayPostponeFee(applyformId, payInterface, Context.ClientIP, Employee.UserName);
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