namespace ChinaPay.B3B.Interface.Processor {
    /// <summary>
    /// 支付申请单
    /// 在网站上
    /// </summary>
    internal class PayApplyform : RequestProcessor {
        protected override string ExecuteCore() {
            var applyformId = Context.GetParameterValue("id");
            if(string.IsNullOrWhiteSpace(applyformId)) InterfaceInvokeException.ThrowParameterMissException("id");

            return PayUtility.GetPayUrl(applyformId, "2", Context.UserName);
        }
    }
}