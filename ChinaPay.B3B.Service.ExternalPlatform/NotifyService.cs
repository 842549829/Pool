using ChinaPay.B3B.DataTransferObject.Order.External;

namespace ChinaPay.B3B.Service.ExternalPlatform {
    public static class NotifyService {
        /// <summary>
        /// 易行通知
        /// </summary>
        public static ExternalPlatformNotifyView YeexingNotify(System.Web.HttpContext context) {
            var processor = Yeexing.NotifyProcessor.CreateProcessor(context);
            return processor.Execute();
        }
        public static ExternalPlatformNotifyView _517NaNotify(System.Web.HttpContext context) {
            var processor = _517Na.NotifyProcessor.CreateProcessor(context);
            return processor.Execute();
        }
        /// <summary>
        /// 51book通知
        /// </summary>
        public static ExternalPlatformNotifyView _51BookNotify(System.Web.HttpContext context) {
            var processor = _51book.NotifyProcessor.CreateProcessor(context);
            return processor.Execute();
        }
        /// <summary>
        /// 51book支付成功通知
        /// </summary>
        public static PaySuccessNotifyView _51BookPaySuccessNotify(System.Web.HttpContext context) {
            var processor = new _51book.PaySuccessNotify(context);
            var notifyView = processor.Execute();
            return notifyView == null ? null : notifyView as PaySuccessNotifyView;
        }
    }
}