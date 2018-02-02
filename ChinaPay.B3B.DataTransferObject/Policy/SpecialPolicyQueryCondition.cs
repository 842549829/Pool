using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Policy
{
    /// <summary>
    /// 特殊政策查询条件 
    /// </summary>
    public class SpecialPolicyQueryCondition : PolicyQueryCondition
    {
        /// <summary>
        /// 产品方ID
        /// </summary>
        public Guid? ResourcerId { get; set; }
        /// <summary>
        /// 产品方审核
        /// </summary>
        public AuditStatus? ResourcerValue { get; set; }
        /// <summary>
        /// 平台审核
        /// </summary>
        public AuditStatus? PlatformValue { get; set; }
    }
   
}
