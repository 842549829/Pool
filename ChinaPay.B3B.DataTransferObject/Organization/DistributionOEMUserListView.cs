using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
   public class DistributionOEMUserListView
    {
       public DateTime RegisterTime { get; set; }
       public string Login { get; set; }
       public string AbbreviateName { get; set; }
       public string IncomeGroupName { get; set; }
       public CompanyType Type { get; set; }
       public AccountBaseType AccountType { get; set; }
       public string ContactName { get; set; }
       public bool Enabled { get; set; }
       public Guid CompanyId { get; set; }
       public Guid? IncomeGroupId { get; set; }

    }
}
