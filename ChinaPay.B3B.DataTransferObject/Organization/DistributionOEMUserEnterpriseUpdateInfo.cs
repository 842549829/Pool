using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
    public class DistributionOEMUserEnterpriseUpdateInfo : CompanyEnterpriseUpdateInfo
    {
        /// <summary>
        /// 收益组Id
        /// </summary>
        public Guid? IncomeGroupId { get; set; }
        /// <summary>
        /// 原收益组Id
        /// </summary>
        public Guid? OrginalIncomeGroupId { get; set; }
    }
}
