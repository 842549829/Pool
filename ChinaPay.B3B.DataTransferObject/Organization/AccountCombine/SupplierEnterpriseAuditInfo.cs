
using System;
namespace ChinaPay.B3B.DataTransferObject.Organization.AccountCombine
{
   public class SupplierEnterpriseAuditInfo : PurchaseEnterpriseInfo
    {
       /// <summary>
       /// 营业执照
       /// </summary>
       public byte[] BussinessLicense { get; set; }
       /// <summary>
       /// 航协许可证书
       /// </summary>
       public byte[] IATALicense { get; set; }

       /// <summary>
       /// 从业时间
       /// </summary>
       public int BussinessTime { get; set; }
       /// <summary>
       /// 使用有效开始日期
       /// </summary>
       public DateTime EffectBeginTime { get; set; }
       /// <summary>
       /// 使用有效结束日期
       /// </summary>
       public DateTime EffectEndTime { get; set; }
       /// <summary>
       /// 是否升级使用
       /// </summary>
       public bool IsUpgrade { get; set; }
       /// <summary>
       /// 操作员
       /// </summary>
       public string OperatorAccount { get; set; }
       /// <summary>
       /// 公司名称
       /// </summary>
       public string CompanyName { get; set; }
       /// <summary>
       /// 公司简称
       /// </summary>
       public string AbbreviateName { get; set; }
       /// <summary>
       /// 组织机构代码
       /// </summary>
       public string OrginationCode { get; set; }
       /// <summary>
       /// 公司电话
       /// </summary>
       public string OfficePhones { get; set; }
    }
}
