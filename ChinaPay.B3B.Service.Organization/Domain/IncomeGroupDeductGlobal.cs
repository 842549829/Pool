using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    public class IncomeGroupDeductGlobal
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 收益组编号
        /// </summary>
        public Guid? IncomeGroupId { get; set; }
        /// <summary>
        /// 公司编号
        /// </summary>
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public PeriodType Type { get; set; }
        /// <summary>
        /// 扣点区域
        /// </summary>
        public List<IncomeGroupPeriod> Period { get; set; }
        /// <summary>
        /// 是否是全局收益设置
        /// </summary>
        public bool? IsGlobal { get; set; }
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
            return string.Format("编号{0},收益组编号{1},扣点类型{2},扣点区域{3},特殊票加价{4},备注{5},是否是全局收益设置{6},公司编号{7}",
                Id, IncomeGroupId.HasValue ? Guid.Empty : IncomeGroupId, Type.GetDescription(), t, Price.ToString(), Remark, IsGlobal.HasValue ? (IsGlobal.Value ? "是" : "否") : "", CompanyId);
        }
    }
}
