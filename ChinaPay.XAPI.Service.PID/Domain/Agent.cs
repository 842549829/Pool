using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.XAPI.Service.Pid.Domain
{
    /// <summary>
    /// 代理商
    /// </summary>
    public class Agent
    {
        public Agent(string name, string description, int status)
        {
            this.Name = name;
            this.Description = description;
            this.Status = status;
        }

        public Agent(int id, string name, string description, int status)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Status = status;
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
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
