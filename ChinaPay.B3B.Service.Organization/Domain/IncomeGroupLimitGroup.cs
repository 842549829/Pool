using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    /// <summary>
    /// 收益设置信息
    /// </summary>
    public class IncomeGroupLimitGroup
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 所属公司Id
        /// </summary>
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 所属用户组Id
        /// </summary>
        public Guid? IncomeGroupId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 收益设置信息
        /// </summary>
        public IList<IncomeGroupLimit> Limitation { get; set; }

        public override string ToString()
        {
            return string.Format("编号 {0},公司编号 {1},所属用户组编号 {2},备注 {3},收益设置信息 {4} ",
                Id, CompanyId, IncomeGroupId, Remark, Limitation.Join(" ，",item => item.ToString()));
        }
    }
}
