using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class SubordinateCompanyListInfo
    {
       public Guid CompanyId { get; set; }
       /// <summary>
       /// 公司名称
       /// </summary>
       public string CompanyName { get; set; }
        /// <summary>
        /// 下级单位简称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 下级单位账号
        /// </summary>
        public string UserNo { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountBaseType? AccountType { get; set; }
        /// <summary>
        /// 下级单位联系人姓名
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 下级单位状态
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 关系类型
        /// </summary>
        public RelationshipType? RelationshipType { get; set; }
       /// <summary>
       /// 公司组
       /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string City { get; set; }
       /// <summary>
       /// 联系电话
       /// </summary>
        public string ContactPhone { get; set; }
        public bool Audtied { get; set; }
        public DateTime AuditTime { get; set; }
       /// <summary>
       /// 注册时间
       /// </summary>
        public DateTime RegisterTime { get; set; }
    }
}
