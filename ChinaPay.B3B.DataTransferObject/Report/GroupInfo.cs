namespace ChinaPay.B3B.DataTransferObject.Report {
    /// <summary>
    /// 分组信息
    /// </summary>
    public class GroupInfo {
        /// <summary>
        /// 乘运人
        /// </summary>
        public bool Carrier { get; set; }
        /// <summary>
        /// 航线
        /// </summary>
        public bool Voyage { get; set; }
        /// <summary>
        /// 航班号
        /// </summary>
        public bool FlightNo { get; set; }
        /// <summary>
        /// 舱位
        /// </summary>
        public bool Bunk { get; set; }
        /// <summary>
        /// 出票方
        /// </summary>
        public bool Provider { get; set; }
        /// <summary>
        /// 销售关系
        /// </summary>
        public bool Relation { get; set; }
    }
}