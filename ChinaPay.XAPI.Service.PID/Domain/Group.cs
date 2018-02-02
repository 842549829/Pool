using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace ChinaPay.XAPI.Service.Pid.Domain
{
     public class Group
    {
         public Group(string name, string officeNo, string xapiName, string xapiPassword, IPEndPoint xapiAddress, bool isPublic, string description)
        {
            this.Name = name;
            this.OfficeNo = officeNo;
            this.XapiName = xapiName;
            this.XapiPassword = xapiPassword;
            this.XapiAddress = xapiAddress;
            this.IsPublic = isPublic;
            this.Description = description;
        }

        public Group(int id, string name, string officeNo, string xapiName, string xapiPassword, IPEndPoint xapiAddress,  bool isPublic, string description)
        {
            this.Id = id;
            this.Name = name;
            this.OfficeNo = officeNo;
            this.XapiName = xapiName;
            this.XapiPassword = xapiPassword;
            this.XapiAddress = xapiAddress;
            this.IsPublic = isPublic;
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
        /// Office号
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// XAPI连接用户名
        /// </summary>
        public string XapiName { get; set; }
        /// <summary>
        /// XAPI连接用户密码
        /// </summary>
        public string XapiPassword { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public IPEndPoint XapiAddress { get; set; }
        /// <summary>
        /// 是否公有配置组
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
