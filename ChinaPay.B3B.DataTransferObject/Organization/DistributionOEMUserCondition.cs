using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class DistributionOEMUserCondition
    {
       /// <summary>
       /// 所属公司
       /// </summary>
       public Guid CompanyId
       {
           get;
           set;
       }
        // <summary>
        /// 注册开始时间
        /// </summary>
        public DateTime? RegisterBeginTime
        {
            get;
            set;
        }
        /// <summary>
        /// 注册结束时间
        /// </summary>
        public DateTime? RegisterEndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserNo
        {
            get;
            set;
        }
        /// <summary>
        /// 授权状态
        /// </summary>
        public bool? Enable
        {
            get;
            set;
        }
        /// <summary>
        /// 公司简称
        /// </summary>
        public string AbbreviateName
        {
            get;
            set;
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName
        {
            get;
            set;
        }
       /// <summary>
       /// 收益组Id
       /// </summary>
        public Guid? IncomeGroup
        {
            get;
            set;
        }
       /// <summary>
       /// 是否拥有包含自己的所有的用户
       /// </summary>
        public bool? IsOwnerAll
        {
            get;
            set;
        }
    }
}
