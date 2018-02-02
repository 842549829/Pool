namespace ChinaPay.B3B.DataTransferObject.Organization {
    using B3B.Common.Enums;

    public class SupplierRegistrationInfo : SupplierCreatureInfo {
        /// <summary>
        /// 拥有客户类型
        /// </summary>
        public HasClientType HasClientType { get; set; }
        /// <summary>
        /// 获知方式
        /// </summary>
        public HowToKnow HowToKnow { get; set; }
        /// <summary>
        /// 推荐者工号
        /// </summary>
        public string Recommender { get; set; }
    }
}
