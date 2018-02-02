namespace ChinaPay.B3B.Service.Policy.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
  public class CompanyLimitPolicy
    {
      public CompanyLimitPolicy(Guid companyId)
      {
          this.Company = companyId;
      }
      public Guid Company
      {
          get;
          private set;
      }
      /// <summary>
      /// 儿童返佣
      /// </summary>
      public ReturnDeduction Child
      {
          get;
          set;
      }
    }

  public class ReturnDeduction
  {
      /// <summary>
      /// 返点
      /// </summary>
      public decimal Rebate
      {
          get;
          set;
      }
      /// <summary>
      /// 可出票航空公司
      /// </summary>
      public IEnumerable<string> Airlines
      {
          get;
          set;
      }
  }
}
