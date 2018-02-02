using System;
using ChinaPay.B3B.DataTransferObject.Organization.AccountCombine;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationHandlers
{
    /// <summary>
    /// AddInfo 的摘要说明
    /// </summary>
    public class AddInfo : BaseHandler
    {
        /// <summary>
        /// 补填企业用户信息
        /// </summary>
        public bool AddPurchaseEnterpriseInfo(PurchaseEnterpriseInfo info)
        {
            try
            {
                AccountCombineService.AddPurchaseInfo(this.CurrentCompany.CompanyId, info,this.CurrentUser.UserName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 补填个人用户信息
        /// </summary>
        public bool AddPurchaseIndividualInfo(PurchaseIndividualInfo info) {
            try
            {
                AccountCombineService.AddPurchaseInfo(this.CurrentCompany.CompanyId,info,this.CurrentUser.UserName);
                return true;
            }
            catch (Exception)
            {
                return false;   
            }
        }
        public bool CheckInfo(string type) {
            var info = CompanyService.GetCompanyDetail(CurrentCompany.CompanyId);
            if (info == null)
            {
                return false;
            }
            if (info.AccountType == Common.Enums.AccountBaseType.Individual)
            {
                return !string.IsNullOrEmpty(info.ContactEmail) && !string.IsNullOrEmpty(info.ZipCode)
                    && !string.IsNullOrEmpty(info.Province) && !string.IsNullOrEmpty(info.City) &&
                    !string.IsNullOrEmpty(info.District) && !string.IsNullOrEmpty(info.Address);
            }
            else {
                return !string.IsNullOrEmpty(info.ContactEmail) && !string.IsNullOrEmpty(info.ZipCode)
                    && !string.IsNullOrEmpty(info.Province) && !string.IsNullOrEmpty(info.City) &&
                    !string.IsNullOrEmpty(info.District) && !string.IsNullOrEmpty(info.Address) &&
                    !string.IsNullOrEmpty(info.ManagerName) && !string.IsNullOrEmpty(info.ManagerCellphone)
                    && !string.IsNullOrEmpty(info.EmergencyContact) && !string.IsNullOrEmpty(info.EmergencyCall);
            }
        }
    }
} 