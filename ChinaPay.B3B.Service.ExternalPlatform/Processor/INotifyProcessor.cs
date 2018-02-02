using ChinaPay.B3B.DataTransferObject.Order.External;

namespace ChinaPay.B3B.Service.ExternalPlatform.Processor {
    interface INotifyProcessor {
        /// <summary>
        /// 通知
        /// </summary>
        ExternalPlatformNotifyView Execute();
    }
}