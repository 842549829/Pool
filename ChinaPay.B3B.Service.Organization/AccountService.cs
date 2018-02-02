using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.SMS.Service;
using ChinaPay.PoolPay.Service;

namespace ChinaPay.B3B.Service.Organization
{
    /// <summary>
    /// 单位收/付款账号服务
    /// </summary>
    public static class AccountService
    {
        public static IEnumerable<Account> Query(Guid companyId)
        {
            var repository = Factory.CreateAccountRepository();
            return repository.Query(companyId);
        }
        public static IEnumerable<Account> Query(string accountNo)
        {
            var repository = Factory.CreateAccountRepository();
            return repository.Query(accountNo);
        }
        public static Account Query(Guid companyId, AccountType type)
        {
            var repository = Factory.CreateAccountRepository();
            return repository.Query(companyId, type);
        }
        public static Dictionary<Guid, Account> GetAllValidAccount(AccountType type)
        {
            var repository = Factory.CreateAccountRepository();
            return repository.GetAllValidAccount(type).ToDictionary(item => item.Company);
        }
        public static void Update(Guid companyId, Account account, string operatorAccount)
        {
            var repository = Factory.CreateAccountRepository();
            repository.Save(companyId, account);

            if (account.Type == AccountType.Receiving)
            {
                //将此账号设置为VIP账户
                AccountBaseService.SetVipAccount(account.No);
            }

            var companysrv = SMSCompanySmsParamService.Query(account.Type, companyId);
            if (companysrv == null)
            {
                var company = CompanyService.GetCompanyInfo(companyId);
                var emp = EmployeeService.QueryCompanyAdmin(companyId);
                //绑定收款账号
                SMSCompanySmsParamService.SaveAccount(new ChinaPay.SMS.Service.Domain.CompanySmsParam() { AccountNo = account.No, CompanyId = company.Id, CompanyName = company.AbbreviateName, CompanyType = company.Type, CompanyNo = emp.Login, AccountType = AccountType.Receiving });
            }
            else
            {
                companysrv.AccountNo = account.No;
                //修改收付款账号绑定
                SMSCompanySmsParamService.UpdateAccount(companysrv);
            }

            // 记录日志
            saveAddLog("收/付款账号", string.Format("账号:{0},账号类型:{1},是否有效:{2},账号绑定时间:{3}", account.No, account.Type.GetDescription(), account.Valid == true ? "是" : "否", account.Time.ToString()), account.No, operatorAccount);
        }
        public static void Enable(Guid companyId, AccountType type, string operatorAccount)
        {
            Account orginAccount = Query(companyId, type);
            UpdateStatus(companyId, type, true);
            if (type == AccountType.Receiving)
            {
                //将此账号设置为VIP账户
                AccountBaseService.SetVipAccount(orginAccount.No);
            }

            // 记录日志
            saveUpdateLog("收/付款账号", string.Format("公司Id:{0},账号:{1},账号类型:{2},账号状态:{3}", companyId, orginAccount.No, orginAccount.Type.GetDescription(), orginAccount.Valid == true ? "是" : "否"),
                string.Format("公司Id:{0},账号:{1},账号类型:{2},账号状态:{3}", companyId, orginAccount.No, orginAccount.Type.GetDescription(), "是"), orginAccount.No, operatorAccount);
        }
        public static void Disable(Guid companyId, AccountType type, string operatorAccount)
        {
            Account orginAccount = Query(companyId, type);
            UpdateStatus(companyId, type, false);

            if (type == AccountType.Receiving)
            {
                //取消此VIP收款账号
                AccountBaseService.CancelVipAccount(orginAccount.No);
            }
            // 记录日志
            saveUpdateLog("收/付款账号", string.Format("公司Id:{0},账号:{1},账号类型:{2},账号状态:{3}", companyId, orginAccount.No, orginAccount.Type.GetDescription(), orginAccount.Valid == true ? "是" : "否"),
                string.Format("公司Id:{0},账号:{1},账号类型:{2},账号状态:{3}", companyId, orginAccount.No, orginAccount.Type.GetDescription(), "否"), orginAccount.No, operatorAccount);
        }
        public static void SetWithholdingInfo(WithholdingView withholdingView) {
            var view = GetWithholding(withholdingView.AccountType,withholdingView.Company);
            if (view != null && view.Status == WithholdingProtocolStatus.Success)
            {
                throw new ChinaPay.Core.Exception.KeyRepeatedException("该账户已签约");
            }
            var repository = Factory.CreateAccountRepository();
            repository.SetWithholdingInfo(withholdingView);
        }
        public static void CancelWithholdingInfo(WithholdingView withholdingView)
        {
            var repository = Factory.CreateAccountRepository();
            repository.SetWithholdingInfo(withholdingView);
        }
        public static WithholdingView GetWithholding(WithholdingAccountType accountType, Guid company)
        {
            var repository = Factory.CreateAccountRepository();
            return repository.GetWithholding( accountType,  company);
        }
        private static void UpdateStatus(Guid companyId, AccountType type, bool enabled)
        {
            var repository = Factory.CreateAccountRepository();
            repository.UpdateStatus(companyId, type, enabled);
        }

        #region"日志"

        static void saveAddLog(string itemName, string content, string key, string account)
        {
            saveLog(OperationType.Insert, "添加" + itemName + "。" + content, key, account);
        }

        static void saveUpdateLog(string itemName, string originalContent, string newContent, string key, string account)
        {
            saveLog(OperationType.Update, string.Format("修改{0}。由 {1} 修改为 {2}", itemName, originalContent, newContent), key, account);
        }

        static void saveDeleteLog(string itemName, string content, string key, string account)
        {
            saveLog(OperationType.Delete, "删除" + itemName + "。" + content, key, account);
        }

        static void saveLog(OperationType operationType, string content, string key, string account)
        {
            var log = new Log.Domain.OperationLog(OperationModule.其他, operationType, account, OperatorRole.User, key, content);
            try
            {
                LogService.SaveOperationLog(log);
            }
            catch { }
        }
        #endregion

        public static IEnumerable<AccountDetailInfo> GetAccount(PaymentsAccountQueryCondition condition, Pagination pagination)
        {
            var repository = Factory.CreateAccountRepository();
            return repository.QueryAccount(pagination, condition);
        }
    }
}
