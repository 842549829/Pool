using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Core;

namespace ChinaPay.B3B.DataTransferObject.Policy
{
    /// <summary>
    /// 政策查询条件
    /// </summary>
    public class BasePolicyQueryCondition : PolicyQueryCondition
    {
        /// <summary>
        /// 内部返佣
        /// </summary>
        public Range<float?> Interior { get; set; }
        /// <summary>
        /// 下级返佣
        /// </summary>
        public Range<float?> Junior { get; set; }
        /// <summary>
        /// 同行返佣
        /// </summary>
        public Range<float?> Brother { get; set; }

    }
}
