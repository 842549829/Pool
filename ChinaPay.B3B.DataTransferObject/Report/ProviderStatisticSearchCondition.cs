using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.DataTransferObject.Report
{
    public class ProviderStatisticSearchCondition
    {
        /// <summary>
        /// 报表开始时间
        /// </summary>
        public DateTime? ReportStartDate
        {
            get;
            set;
        }
        /// <summary>
        /// 报表结束时间
        /// </summary>
        public DateTime? ReportEndDate
        {
            get;
            set;
        }
        /// <summary>
        /// 出票方Id
        /// </summary>
        public Guid? Provider
        {
            get;
            set;
        }
        /// <summary>
        /// 是否有交易
        /// </summary>
        public bool? IsHasTrade
        {
            get;
            set;
        }
        /// <summary>
        /// 承运人
        /// </summary>
        public string Carrier
        {
            get;
            set;
        }
        /// <summary>
        /// 出港地
        /// </summary>
        public string Departure
        {
            get;
            set;
        }

        /// <summary>
        /// 到达地代码
        /// </summary>
        public string Arrival { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public ProductType? ProductType { get; set; }

        /// <summary>
        /// 特殊产品类型
        /// </summary>
        public SpecialProductType? SpecialProductType { get; set; }

        /// <summary>
        /// 销售关系
        /// </summary>
        public RelationType? SaleRelation { get; set; }
    }
}