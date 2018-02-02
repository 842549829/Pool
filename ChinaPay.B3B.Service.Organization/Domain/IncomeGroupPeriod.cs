using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    /// <summary>
    /// 收益组扣点区间
    /// </summary>
    public class IncomeGroupPeriod
    {
        /// <summary>
        /// 扣点编号
        /// </summary>
        public Guid DeductId { get; set; }
        /// <summary>
        /// 开始值
        /// </summary>
        public decimal StartPeriod { get; set; }
        /// <summary>
        /// 结束值
        /// </summary>
        public decimal EndPeriod { get; set; }
        /// <summary>
        /// 扣点值
        /// </summary>
        public decimal Period { get; set; }
        public override string ToString()
        {
            return string.Format("扣点编号{0},开始值{1},结束值{2},扣点值{3}", DeductId, StartPeriod, EndPeriod, Period);
        }
    }
}
