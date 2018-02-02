using System;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
    public class DistributionOEMUserIndividualUpdateInfo : CompanyIndividualUpdateInfo
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
