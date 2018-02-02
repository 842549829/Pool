using System;

namespace ChinaPay.B3B.DataTransferObject.Foundation
{
    /// <summary>
    /// 航空公司退规统计信息
    /// </summary>
    public class AirlineRulesView
    {
        readonly Func<string, string> getArilineName;
        public string AirlineCode { get; set; }
        public string AirlineName
        {
            get;
            set;
        }
        public int RulesCount { get; set; }

        /// <summary>
        /// 是否已添加详情
        /// </summary>
        public bool HasRules { get; set; }
    }
}