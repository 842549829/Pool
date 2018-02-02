using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.XAPI.Service.Pid.Domain
{
    /// <summary>
    /// 业务操作
    /// </summary>
    public class Operation
    {
        public Operation(string name, string cofigurationType, RuleType ruleType, string description)
        {
            this.Name = name;
            this.ConfigurationType = cofigurationType;
            this.RuleType = ruleType;
            this.Description = description;
        }

        public Operation(int id, string name, string cofigurationType, RuleType ruleType, string description)
        {
            this.Id = id;
            this.Name = name;
            this.ConfigurationType = cofigurationType;
            this.RuleType = ruleType;
            this.Description = description;
        }
        
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 配置类型
        /// </summary>
        public string ConfigurationType { get; set; }
        /// <summary>
        /// 规则类型
        /// </summary>
        public RuleType RuleType { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }

    
}
