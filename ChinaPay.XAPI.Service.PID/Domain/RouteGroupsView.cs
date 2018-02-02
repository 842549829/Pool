using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace ChinaPay.XAPI.Service.Pid.Domain
{
    public class RouteGroupsView
    {
        public RouteGroupsView(int agentId, int operationId, string xapiName, string xapiPassword, string officeNo, IPEndPoint xapiAddress, RuleType ruleType, bool isPublic, int count)
        {
            this.AgentId = agentId;
            this.OperationId = operationId;
            this.XapiName = xapiName;
            this.XapiPassword = xapiPassword;
            this.XapiAddress = xapiAddress;
            this.OfficeNo = officeNo;
            this.RuleType = ruleType;
            this.IsPublic = IsPublic;
            this.Count = count;
        }
        /// <summary>
        /// 代理编号
        /// </summary>
        public int AgentId { get; set; }
        /// <summary>
        /// 操作编号
        /// </summary>
        public int OperationId { get; set; }
        /// <summary>
        /// XAPI的用户名
        /// </summary>
        public string XapiName { get; set; }
        /// <summary>
        /// XAPI的密码
        /// </summary>
        public string XapiPassword { get; set; }
        /// <summary>
        /// office号
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// XAPI的服务器地址
        /// </summary>
        public IPEndPoint XapiAddress { get; set; }
        /// <summary>
        /// 规则类型
        /// </summary>
        public RuleType RuleType { get; set; }
        /// <summary>
        /// 组配置是公有还是私有
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        /// 配置组中资源总数
        /// </summary>
        public int Count { get; set; }
    }
}
