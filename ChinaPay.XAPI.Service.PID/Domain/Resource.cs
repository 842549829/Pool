using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace ChinaPay.XAPI.Service.Pid.Domain
{
    /// <summary>
    /// PID资源
    /// </summary>
    public class Resource
    {
        public Resource(string name, string password, string officeNo, CertificationType certificationType,
            IPAddress certificationAddress, IPEndPoint serverAddress, string serverLoginString,
            string configurationType, int status, int agentId)
        {
            this.Name = name;
            this.Password = password;
            this.OfficeNo = officeNo;
            this.CertificationType = certificationType;
            this.CertificationAddress = certificationAddress;
            this.ServerAddress = serverAddress;
            this.ServerLoginString = serverLoginString;
            this.ConfigurationType = configurationType;
            this.Status = status;
            this.AgentId = agentId;
        }

        public Resource(int id, string name, string password,string officeNo, CertificationType certificationType,
            IPAddress certificationAddress, IPEndPoint serverAddress, string serverLoginString,
            string configurationType, int status, int agentId)
        {
            this.Id = id;
            this.Name = name;
            this.Password = password;
            this.OfficeNo = officeNo;
            this.CertificationType = certificationType;
            this.CertificationAddress = certificationAddress;
            this.ServerAddress = serverAddress;
            this.ServerLoginString = serverLoginString;
            this.ConfigurationType = configurationType;
            this.Status = status;
            this.AgentId = agentId;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Office号
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// 认证类型
        /// </summary>
        public CertificationType CertificationType { get; set; }
        /// <summary>
        /// 认证地址
        /// </summary>
        public IPAddress CertificationAddress { get; set; }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public IPEndPoint ServerAddress { get; set; }
        /// <summary>
        /// 服务器登录字串
        /// </summary>
        public string ServerLoginString { get; set; }
        /// <summary>
        /// 配置类型
        /// </summary>
        public string ConfigurationType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 代理人编号
        /// </summary>
        public int AgentId { get; set; }
    }
}
