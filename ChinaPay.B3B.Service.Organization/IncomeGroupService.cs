using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Core;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Organization.Domain;
using System.Transactions;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.Service.Organization
{
    public static class IncomeGroupService
    {
        public static IEnumerable<IncomeGroupListView> QueryIncomeGroup(Guid companyId, Pagination pagination)
        {
            var repository = Factory.CreateIncomeGroupRepository();
            return repository.Query(companyId, pagination);
        }

        public static void RegisterIncomeGroup(Domain.IncomeGroup incomeGroup, string operatorAccount)
        {
            var respository = Factory.CreateIncomeGroupRepository();
            respository.Insert(incomeGroup);
            saveAddLog("收益组信息", string.Format("公司Id{0},收益组名称:{1},描述:{2},创建时间:{3}", incomeGroup.Company, incomeGroup.Name, incomeGroup.Description,
               incomeGroup.CreateTime), OperatorRole.User, "", operatorAccount);
        }

        public static void DeleteIncomeGroup(Guid groupId, string operatorAccount)
        {
            var repository = Factory.CreateIncomeGroupRepository();
            repository.Delete(groupId);
        }

        public static void DeleteIncomeGroupList(IEnumerable<Guid> groupIds, string operatorAccount)
        {
            var repository = Factory.CreateIncomeGroupRepository();
            repository.Delete(groupIds);
        }

        public static void UpdateIncomeGroup(Domain.IncomeGroup incomeGroup, string operatorAccount)
        {
            var orginalIncomeGroup = QueryIncomeGroup(incomeGroup.Id);
            string orginalContent = string.Format("公司Id{0},收益组名称:{1},描述:{2}", orginalIncomeGroup.Company, orginalIncomeGroup.Name, orginalIncomeGroup.Description);
            string newContent = string.Format("公司Id{0},收益组名称:{1},描述:{2}", incomeGroup.Company, incomeGroup.Name, incomeGroup.Description);
            var repository = Factory.CreateIncomeGroupRepository();
            repository.Update(incomeGroup);
            saveUpdateLog("收益组信息", orginalContent, newContent, OperatorRole.User, incomeGroup.Id.ToString(), operatorAccount);
        }

        public static Domain.IncomeGroup QueryIncomeGroup(Guid groupId)
        {
            var repository = Factory.CreateIncomeGroupRepository();
            return repository.Query(groupId);
        }

        public static IncomeGroupView QueryIncomeGroupView(Guid groupId)
        {
            var repository = Factory.CreateIncomeGroupRepository();
            return repository.QueryIncomeGroup(groupId);
        }

        public static IEnumerable<IncomeGroupView> QueryIncomeGroup(IEnumerable<Guid> groupIds)
        {
            var repository = Factory.CreateIncomeGroupRepository();
            return repository.QueryIncomeGroup(groupIds);
        }

        public static void UpdateIncomeGroupRelation(Guid? orginalIncomeGroupId, Guid? newIncomeGroupId, Guid companyId)
        {
            var repository = Factory.CreateIncomeGroupRepository();
            repository.UpdateIncomeGroupRelation(orginalIncomeGroupId, newIncomeGroupId, companyId);
        }

        public static void UpdateIncomeGroupRelation(Guid newIncomeGroupId, IEnumerable<Guid> companyId)
        {
            var repository = Factory.CreateIncomeGroupRepository();
            repository.UpdateIncomeGroupRelation(newIncomeGroupId, companyId);
        }

        public static void InsertIncomeGroupDeductSetting(IncomeGroupDeductGlobal setting, string operatorAccount)
        {
            var repository = Factory.CreateIncomeGroupDeductGlobalRepository();
            repository.Insert(setting);
            saveAddLog("收益设置", setting.ToString(), OperatorRole.User, setting.Id.ToString(), operatorAccount);
        }
        public static void UpdateIncomeGroupDeductSetting(IncomeGroupDeductGlobal setting, string operatorAccount)
        {
            var repository = Factory.CreateIncomeGroupDeductGlobalRepository();
            repository.Update(setting);
            saveAddLog("收益设置", setting.ToString(), OperatorRole.User, setting.Id.ToString(), operatorAccount);
        }

        public static IncomeGroupDeductGlobal QueryIncomeGroupDeductGlobalSetting(Guid id)
        {
            var repository = Factory.CreateIncomeGroupDeductGlobalRepository();
            return repository.Query(id);
        }
        public static IncomeGroupDeductGlobal QueryIncomeGroupDeductGlobalByCompanyId(Guid companyId)
        {
            var repository = Factory.CreateIncomeGroupDeductGlobalRepository();
            return repository.QueryByCompanyId(companyId);
        }
        public static void UpdateIsGlobal(Guid companyId, bool isGbobal)
        {
            var repository = Factory.CreateIncomeGroupDeductGlobalRepository();
            repository.UpdateIsGlobal(companyId, isGbobal);
        }

        /// <summary>
        /// 查询收益信息
        /// </summary>
        /// <param name="owner">上级公司编号</param>
        /// <param name="purchaserId">当前采购账号</param>
        /// <returns>可能为空，即没有设置</returns>
        public static IncomeGroupDeductGlobal QueryIncomeGroupDeductGlobalSetting(Guid owner, Guid purchaserId)
        {
            var repository = Factory.CreateIncomeGroupDeductGlobalRepository();
            var income = repository.QueryByCompanyId(owner);
            if (income != null && income.IsGlobal.Value)
            {
                return income;
            }
            return repository.GetIncomeGroupDeductGlobalByPurchaser(owner, purchaserId);
        }

        #region 日志
        static void saveAddLog(string itemName, string content, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Insert, "添加" + itemName + "。" + content, role, key, account);
        }
        static void saveUpdateLog(string itemName, string originalContent, string newContent, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Update, string.Format("修改{0}。由 {1} 修改为 {2}", itemName, originalContent, newContent), role, key, account);
        }
        static void saveDeleteLog(string itemName, string content, OperatorRole role, string key, string account)
        {
            saveLog(OperationType.Delete, "删除" + itemName + "。" + content, role, key, account);
        }
        static void saveLog(OperationType operationType, string content, OperatorRole role, string key, string account)
        {
            var log = new Log.Domain.OperationLog(OperationModule.其他, operationType, account, role, key, content);
            try
            {
                LogService.SaveOperationLog(log);
            }
            catch { }
        }
        #endregion
    }
}
