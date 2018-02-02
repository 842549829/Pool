using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.XAPI.Service.Pid.Domain
{
    public class ResourceServiceHistory
    {
        public ResourceServiceHistory(int threadId, DateTime serviceTime, string sendMessage, string receiveMessage, int agentId, int operationId, int groupId)
        {
            this.ThreadId = threadId;
            this.GenerateTime = serviceTime;
            this.SendMessage = sendMessage;
            this.ReceiveMessage = receiveMessage;
            this.AgentId = agentId;
            this.OperationId = operationId;
            this.GroupId = groupId; 
        }

        /// <summary>
        /// PID
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime GenerateTime { get; set; }

        /// <summary>
        /// 发送消息
        /// </summary>
        public string SendMessage { get; set; }
        
        /// <summary>
        /// 接收消息
        /// </summary>
        public string ReceiveMessage { get; set; }

        /// <summary>
        /// 使用者编号
        /// </summary>
        public int AgentId { get; set; }

        /// <summary>
        /// 操作编号
        /// </summary>
        public int OperationId { get; set; }

        public int GroupId { get; set; }
    }
}
