using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Organization;
using PoolPay.DataTransferObject;
using AccountType = ChinaPay.B3B.Common.Enums.AccountType;

namespace ChinaPay.B3B.TransactionWeb.OrganizationHandlers
{
    /// <summary>
    /// 账号审核
    /// </summary>
    public class Account : BaseHandler
    {
        /// <summary>
        /// 账号的有效
        /// </summary>
        public bool IsEnblue(string id,byte bytType)
        {
            try
            { AccountService.Enable(System.Guid.Parse(id),(AccountType)bytType,this.CurrentUser.UserName); }
            catch { return false; }
            return true;
        }
        public bool IsDisable(string id,byte bytType)
        {
            try
            { AccountService.Disable(System.Guid.Parse(id), (AccountType)bytType,this.CurrentUser.UserName); }
            catch { return false; }
            return true;
        }
        /// <summary>
        /// 修改账号
        /// </summary>
        public bool UpdateAccount(string id,string account,byte bytType) {
            try
            {
                 AccountService.Update(System.Guid.Parse(id),this.GetAccout(account, bytType), this.CurrentUser.UserName);
            }
            catch { return false; }
            return true;
        }
        private ChinaPay.B3B.Service.Organization.Domain.Account GetAccout(string account,byte byteType)
        {
            return new ChinaPay.B3B.Service.Organization.Domain.Account(account, byteType == 1 ? AccountType.Area : (byteType == 2 ? AccountType.Payment : AccountType.Receiving),true);
        }
        /// <summary>
        /// 更换收款账号
        /// </summary>
        public void ReplacementAccountNo(string originalAccountNo, string newAccountNo, string payPassword)
        {
            try
            {
                if (!ChinaPay.PoolPay.Service.AccountBaseService.GetMebershipUser(newAccountNo))
                    throw new ChinaPay.Core.CustomException("新收款账号不存在");
                if (AccountService.Query(newAccountNo).Any())
                    throw new ChinaPay.Core.Exception.InvalidValueException(string.Format("新收款账号{0}已被绑定", newAccountNo));
                var membershipUser = ChinaPay.PoolPay.Service.AccountBaseService.GetMembershipUser(newAccountNo);
                if (membershipUser == null) throw new ChinaPay.Core.CustomException("account");
                if (membershipUser.PayPassword != ChinaPay.Utility.MD5EncryptorService.MD5FilterZero(payPassword, "utf-8"))
                    throw new ChinaPay.Core.Exception.InvalidValueException("支付密码错误");
                var account = AccountService.Query(originalAccountNo).FirstOrDefault(linq=>linq.No == originalAccountNo);
                if (account == null) throw new ChinaPay.Core.CustomException("account");
                var newAccount = new Service.Organization.Domain.Account(newAccountNo, account.Type,true);
                AccountService.Update(account.Company, newAccount, CurrentUser.UserName);
            }
            catch (System.Data.Common.DbException)
            {
                throw new ChinaPay.Core.CustomException("更换失败");
            }
        }
        /// <summary>
        /// 注册个人账户的收款账号
        /// </summary>
        /// <param name="info">个人账户信息</param>
        public void AddPersonAccount(AccountDTO info)
        {
            try
            {
                AccountCombineService.AddReciveAccount(this.CurrentCompany.CompanyId, info,CurrentUser.UserName);
            }
            catch (System.Data.Common.DbException)
            {
                throw new ChinaPay.Core.CustomException("注册失败");
            }
        }
        /// <summary>
        /// 注册企业账户的收款账号
        /// </summary>
        /// <param name="info">企业账户信息</param>
        public void AddCompanyAccount(EnterpriseAccountDTO info)
        {
            try
            {
                AccountCombineService.AddReciveAccount(this.CurrentCompany.CompanyId, info,CurrentUser.UserName);
            }
            catch (System.Data.Common.DbException)
            {
                throw new ChinaPay.Core.CustomException("注册失败");
            }
        }
    }
}