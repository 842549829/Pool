using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization.AccountCombine
{
   public class CompanyIndividualUpdateInfo
    {
       public Guid CompanyId { get; set; }
       /// <summary>
       /// 姓名
       /// </summary>
       public string Name { get; set; }
       /// <summary>
       /// 身份证号
       /// </summary>
       public string CertNo { get; set; }
       /// <summary>
       ///联系人姓名
       /// </summary>
       public string ContactName { get; set; }
       /// <summary>
       /// 联系人电话
       /// </summary>
       public string ContactPhone { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 所在地
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Faxes { get; set; }
        /// <summary>
        ///Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 联系人QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string OperatorAccount { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string OfficePhone { get; set; }
    }
}
