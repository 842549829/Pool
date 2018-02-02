namespace ChinaPay.B3B.Interface.Processor {
    /// <summary>
    /// 支付订单
    /// 在网站上
    /// </summary>
    internal class PayOrder : RequestProcessor {
        protected override string ExecuteCore() {
            var orderId = Context.GetParameterValue("id");
            if(string.IsNullOrWhiteSpace(orderId)) InterfaceInvokeException.ThrowParameterMissException("id");

            return PayUtility.GetPayUrl(orderId, "1", Context.UserName);
        }
    }
}