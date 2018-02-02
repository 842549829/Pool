using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    /// <summary>
    /// 收益设置详细信息
    /// </summary>
    public class IncomeGroupLimit
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airlines { get; set; }
        /// <summary>
        /// 收益设置编号
        /// </summary>
        public Guid IncomeId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public PeriodType Type { get; set; }
        /// <summary>
        /// 特殊票加价
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 扣点区域
        /// </summary>
        public List<IncomeGroupPeriod> Period { get; set; }
        /// <summary>
        /// 是否是本公司政策
        /// </summary>
        public bool IsOwnerPolicy { get; set; }

        public override string ToString()
        {
            var t = Period.Join(",", item => item.ToString());
            return string.Format("编号{0},收益设置编号{1},扣点类型{2},扣点区域{3},特殊票加价{4},航空公司{5},是否是本公司政策{6} ",
                Id, IncomeId, Type.GetDescription(), t, Price.ToString(), Airlines, IsOwnerPolicy ? "是" : "否");
        }

        public override bool Equals(object obj)
        {
            var limit = (IncomeGroupLimit)obj;
            return limit.Id == Id && limit.IncomeId == IncomeId && limit.Type == Type && limit.Price == Price;
        }
    }
}
