using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
  public class DistribtionOEMUserCompanyDetailInfo : CompanyDetailInfo
    {
      /// <summary>
      /// 收益组Id
      /// </summary>
      public Guid? IncomeGroupId
      {
          get;
          set;
      }
      /// <summary>
      /// 收益组名称
      /// </summary>
      public string IncomeGroupName
      {
          get;
          set;
      }
    }
}
