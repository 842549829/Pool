using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Order {
    /// <summary>
    /// 资源信息
    /// </summary>
    public class ResourceView {
        /// <summary>
        /// 成人编码
        /// </summary>
        public PNRPair AudltPNR { get; set; }
        /// <summary>
        /// 儿童编码
        /// </summary>
        public PNRPair ChildPNR { get; set; }
    }
}
