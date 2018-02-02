using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization.AccountCombine
{
  public class PurchaseEnterpriseInfo
    {
        /// <summary>
        /// 传真
        /// </summary>
        public string Faxes { get; set; }
        /// <summary>
        /// 所在地省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 所在地城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 所在地区县
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string ManagerName { get; set; }
        /// <summary>
        /// 负责人手机
        /// </summary>
        public string ManagerCellphone { get; set; }
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContact { get; set; }
        /// <summary>
        /// 紧急电话
        /// </summary>
        public string EmergencyCall { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
      /// <summary>
        /// MSN
      /// </summary>
        public string MSN { get; set; }
        /// <summary>
        /// 联系人QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
      /// 邮编
      /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        ///联系人姓名
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 公司电话
        /// </summary>
        public string CompanyPhone { get; set; }
    }
}
