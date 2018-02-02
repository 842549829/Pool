using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.Organization
{
   public static class CompanyUpgradeService
    {
        /// <summary>
       /// 判断是否能够申请升级
       /// 下级及内部机构不能申请升级
       /// </summary>
       /// <param name="companyId">单位Id</param>
        public static bool IsAllowUpgrade(Guid companyId) {
           var relation = QueryRelationship(companyId);
           return relation == null || (relation.Type != RelationshipType.Organization && relation.Type != RelationshipType.Distribution);
       }
        /// <summary>
       /// 查询需要审核的升级公司
       /// </summary>
       /// <returns></returns>
        public static IEnumerable<CompanyUpgrade> QueryCompanyUpgradeNeedAudit()
       {
           var repository = Factory.CreateCompanyUpgradeRepository();
           return repository.QueryNeedAuditCompanys();
       }
        /// <summary>
        /// 查询公司升级申请信息
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <returns></returns>
        public static CompanyUpgrade QueryCompanyUpgrade(Guid companyId)
        {
            var repository = Factory.CreateCompanyUpgradeRepository();
            return repository.Query(companyId);
        }
        /// <summary>
        /// 账户升级审核通过
        /// </summary>
        /// <param name="companyInfo"></param>
        /// <param name="operatorAccount"></param>
        public static void Enable(Guid companyId,string operatorAccount)
        {
            Audit(companyId, true, DateTime.Now,operatorAccount);
        }
        /// <summary>
       /// 账户升级审核拒绝
       /// </summary>
       /// <param name="companyInfo"></param>
       /// <param name="operatorAccount"></param>
        public static void Disable(Guid companyId,string companyAccount,string reason,string operatorAccount)
        {
            Audit(companyId,false,DateTime.Now,operatorAccount);
            saveElseLog("公司升级认证拒绝",  string.Format("拒绝公司账号为{0}的公司升级信息，拒绝原因:{1}", companyAccount,reason), companyAccount, operatorAccount);
        }
        /// <summary>
       /// 保存公司升级申请信息
       /// </summary>
       /// <param name="companyUpgrade"></param>
        public static void Save(CompanyUpgrade companyUpgrade,string operatorAccount)
        {
            if (!string.IsNullOrWhiteSpace(companyUpgrade.Name)&&CompanyService.ExistsCompanyName(companyUpgrade.Name))
                throw new InvalidOperationException("系统中已存在指定的 \"单位名称\"。");
            if (!string.IsNullOrWhiteSpace(companyUpgrade.AbbreviateName)&&CompanyService.ExistsAbbreviateName(companyUpgrade.AbbreviateName))
                throw new InvalidOperationException("系统中已存在指定的 \"单位简称\"。");
            var repository = Factory.CreateCompanyUpgradeRepository();
            repository.Save(companyUpgrade);
            saveAddLog("公司升级申请信息", string.Format("公司账号为{0}的账号申请为{1}({2}),申请时间为{3}", companyUpgrade.UserNo, companyUpgrade.Type.GetDescription(), companyUpgrade.AccountType.GetDescription(), companyUpgrade.ApplyTime), companyUpgrade.UserNo, operatorAccount);
        }
        /// <summary>
       /// 验证是否显示”账户升级“内容
       /// </summary>
       /// <param name="companyInfo"></param>
       /// <returns></returns>
        public static bool IsValid(CompanyDetailInfo companyInfo)
        {
            if (IsVerify(companyInfo) && IsVerify(companyInfo.CompanyId) && IsAllowUpgrade(companyInfo.CompanyId))
                return true;
            return false;
        }
        /// <summary>
       /// 账户升级类型的判断
       /// </summary>
       /// <param name="companyInfo"></param>
       /// <returns></returns>
        private static bool IsVerify(CompanyDetailInfo companyInfo)
        {
            if (companyInfo.CompanyType == Common.Enums.CompanyType.Purchaser)
                return true;
            if (companyInfo.CompanyType == Common.Enums.CompanyType.Supplier&&companyInfo.AuditTime.HasValue)
                return true;
            if (companyInfo.CompanyType == Common.Enums.CompanyType.Provider && companyInfo.AuditTime.HasValue && !companyInfo.Audited)
                return true;
            return false;
        }
        /// <summary>
       /// 一年一个账号只能由一次账户升级的机会
       /// </summary>
       /// <param name="companyId"></param>
       /// <returns></returns>
        private static bool IsVerify(Guid companyId)
        {
            var companyUpgrade = QueryCompanyUpgrade(companyId);
            if (companyUpgrade == null) {
                return true;
            } else  {
                if (companyUpgrade.ApplyTime.Year != DateTime.Now.Year)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 审核公司升级
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="audited"></param>
        /// <param name="auditTime"></param>
        private static void Audit(Guid companyId, bool audited, DateTime auditTime,string operatorAccount)
        {
            var repository = Factory.CreateCompanyUpgradeRepository();
            repository.Audit(companyId, audited, auditTime);
        }
        /// <summary>
        /// 查询关系
        /// </summary>
        /// <param name="companyId">公司 Id</param>
        /// <returns></returns>
        private static Data.DataMapping.Relationship QueryRelationship(Guid companyId)
        {
            var repository = Factory.CreateCompanyRepository();
            return repository.QueryRelationship(companyId);
        }

        #region 日志

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

        static void saveElseLog(string itemName, string content, string key, string account)
        {
            saveLog(OperationType.Else,  itemName + "：" + content, key, account);
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
    }
}
