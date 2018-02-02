using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Policy
{
   /// <summary>
   /// 特价默认政策
   /// </summary>
   public class BargainDefaultPolicyInfo
    {
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
       ///// <summary>
       ///// 省份名称
       ///// </summary>
       // public string ProvinceName { get; set; }
       /// <summary>
       /// 省份代码
       /// </summary>
        public string ProvinceCode { get; set; }  
        /// <summary>
        /// 成人默认出票方
        /// </summary>
        public Guid AdultProviderId { get; set; }
        /// <summary>
        /// 成人默认出票方名称
        /// </summary>
        public string AdultProviderName { get; set; }
        /// <summary>
        /// 成人默认出票方简称
        /// </summary>
        public string AdultProviderAbbreviateName { get; set; }
        /// <summary>
        /// 成人默认佣金
        /// </summary>
        public decimal AdultCommission { get; set; }
         

    }
}
