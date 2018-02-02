using System;

namespace ChinaPay.B3B.DataTransferObject.Organization.AccountCombine
{
   public class SupplierIndividualAuditInfo :PurchaseIndividualInfo
    {
       /// <summary>
       /// 身份证
       /// </summary>
       public byte[] CertLicense { get; set; }
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
    }
}
