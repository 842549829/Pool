using System.Linq;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.PoolPay.Service;

namespace ChinaPay.B3B.TransactionWeb.OrganizationHandlers
{
    /// <summary>
    /// 检查公司开户的基本信息
    /// </summary>
    public class CheckUpComPanyNews : ChinaPay.Infrastructure.WebEx.AjaxHandler.WebAjaxHandler
    {
        /// <summary>
        /// 检查用户名存在否
        /// </summary>
        /// <param name="userNo"></param>
        /// <returns></returns>
        public bool CheckUpUserNo(string userNo) 
        {
            return EmployeeService.ExistsUserName(userNo);
        }
        /// <summary>
        /// 检查公司名称存在否
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public bool CheckUpCompanyName(string companyName) 
        {
            return CompanyService.ExistsCompanyName(companyName);
        }
        /// <summary>
        /// 检查公司简称存在否
        /// </summary>
        /// <param name="companyForShort"></param>
        /// <returns></returns>
        public bool CkeckUpCompanyForShort(string companyForShort) 
        {
            return CompanyService.ExistsAbbreviateName(companyForShort);
        }
        /// <summary>
        /// 检查昵称存在否
        /// </summary>
        /// <param name="petName"></param>
        /// <returns></returns>
        public bool CheckUpPetName(string petName) 
        {
            return this.CkeckUpCompanyForShort(petName);
        }
        /// <summary>
        /// 检查国付通账号存在不
        /// </summary>
        public bool CheckPayAcccountNo(string accountNo) {
            return AccountBaseService.GetMebershipUser(accountNo);
        }
        /// <summary>
        /// 同时检查国付通账号和B3B账号
        /// </summary>
        public bool CheckAccountNo(string accountNo) 
        {
            return CheckPayAcccountNo(accountNo) || CheckUpUserNo(accountNo);
        }
        public string CheckPayAccountNOusable(string accountNo) {
             if(!AccountBaseService.GetMebershipUser(accountNo)){
                 return "该账号不存在";
             } 
             if (AccountService.Query(accountNo).Any()) 
             {
                 return "该账号已经被绑定";
             }
             return string.Empty;
        }
    }
}