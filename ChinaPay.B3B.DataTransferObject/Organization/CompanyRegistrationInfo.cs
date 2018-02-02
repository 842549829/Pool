namespace ChinaPay.B3B.DataTransferObject.Organization {
    using B3B.Common.Enums;

    public class CompanyRegistrationInfo : CompanyInfo {
        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessType BusinessType { get; set; }
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

        /// <summary>
        /// 航协经营批准号
        /// </summary>
        public string Licence { get; set; }
        /// <summary>
        /// IATA 号
        /// </summary>
        public string IATA { get; set; }
        /// <summary>
        /// Office 号
        /// </summary>
        public string OfficeNumbers { get; set; }
        /// <summary>
        /// 担保金
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 代理资质
        /// </summary>
        public QualificationType QualificationType { get; set; }
    }
}
