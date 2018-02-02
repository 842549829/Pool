using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    public class IncomeGroupDeductSetting
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 收益组编号
        /// </summary>
        public Guid IncomeGroupId { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airlines { get; set; }
        /// <summary>
        /// 出港城市
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 扣点类型
        /// </summary>
        public PeriodType Type { get; set; }
        /// <summary>
        /// 扣点区域
        /// </summary>
        public List<IncomeGroupPeriod> Period { get; set; }
        /// <summary>
        /// 特殊票加价
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        public override string ToString()
        {
            var t = Period.Join(",", item => item.ToString());
            return string.Format("编号{0},收益组编号{1},航空公司{2},出港城市{3},扣点类型{4},扣点区域{5},特殊票加价{6},备注{7}",
                Id, IncomeGroupId, Airlines, Departure, Type.GetDescription(), t, Price.ToString(), Remark);
        }
    }
}
