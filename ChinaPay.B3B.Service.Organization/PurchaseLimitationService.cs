using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.Organization
{
    public static class PurchaseLimitationService
    {
        /// <summary>
        /// 新增全局或分组采买限制
        /// </summary>
        /// <param name="setting">采买限制</param>
        /// <param name="operatorAccount">操作员</param>
        public static void InsertPurchaseLimitationGroup(PurchaseLimitationGroup setting, string operatorAccount)
        {
            var repository = Factory.CreatePurchaseRestrictionSettingRepository();
            repository.InsertPurchaseRestrictionSetting(setting);
            string sql = string.Format("限制组Id:{0},是否是全局:{1},所属公司Id:{2},所属公司组Id:{3}",
                setting.Id.ToString(), setting.IsGlobal ? "是" : "否", setting.CompanyId.HasValue ? setting.CompanyId.ToString() : "", setting.IncomeGroupId.HasValue ? setting.IncomeGroupId.ToString() : "");
            int i = 1;
            foreach (var item in setting.Limitation)
            {
                var normal = item.Rebate.Where(q => q.Type == PurchaseLimitationRateType.Normal).FirstOrDefault();
                var bargain = item.Rebate.Where(q => q.Type == PurchaseLimitationRateType.Bargain).FirstOrDefault();
                sql += string.Format("限制信息" + i + "：限制信息Id:{0},航空公司:{1},出发城市:{2}", item.Id.ToString(), item.Airlines, item.Departures);
                sql += string.Format("仅允许采购我发布的普通政策:{0},成人默认返点(普通政策):{1},仅允许采购我发布的特价政策:{2},成人默认返点(特价政策):{3}",
                          normal.AllowOnlySelf ? "是" : "可以采取平台上其他普通政策",
                          normal.Rebate.HasValue ? normal.Rebate.Value.ToString() : "",
                          bargain.AllowOnlySelf ? "是" : "可以采取平台上其他特价政策",
                          bargain.Rebate.HasValue ? bargain.Rebate.ToString() : "");
                i++;
            }
            saveAddLog("用户组采购限制设置",
                 sql,
                  OperatorRole.User,
                  setting.Id.ToString(),
                  operatorAccount);
        }
        /// <summary>
        /// 修改分组采买限制
        /// </summary>
        /// <param name="setting">采买限制</param>
        /// <param name="operatorAccount">操作员</param>
        public static void UpdatePurchaseRestrictionSetting(PurchaseLimitationGroup setting, string operatorAccount)
        {
            var orginalSetting = QueryPurchaseLimitationGroupInfo(setting.IncomeGroupId.Value);
            var repository = Factory.CreatePurchaseRestrictionSettingRepository();
            repository.UpdatePurchaseRestrictionSetting(setting);
            string orginalContent = "";
            if (orginalSetting != null)
            {
                orginalContent = string.Format("限制组Id:{0},是否是全局:{1},所属公司Id:{2},所属公司组Id:{3}",
                 orginalSetting.Id.ToString(), orginalSetting.IsGlobal ? "是" : "否", orginalSetting.CompanyId.HasValue ? orginalSetting.CompanyId.ToString() : "", orginalSetting.IncomeGroupId.HasValue ? orginalSetting.IncomeGroupId.ToString() : "");
                int i = 1;
                foreach (var item in orginalSetting.Limitation)
                {
                    var normal = item.Rebate.Where(q => q.Type == PurchaseLimitationRateType.Normal).FirstOrDefault();
                    var bargain = item.Rebate.Where(q => q.Type == PurchaseLimitationRateType.Bargain).FirstOrDefault();
                    orginalContent += string.Format("限制信息" + i + "：限制信息Id:{0},航空公司:{1},出发城市:{2}", item.Id.ToString(), item.Airlines, item.Departures);
                    orginalContent += string.Format("仅允许采购我发布的普通政策:{0},成人默认返点(普通政策):{1},仅允许采购我发布的特价政策:{2},成人默认返点(特价政策):{3}",
                              normal.AllowOnlySelf ? "是" : "可以采取平台上其他普通政策",
                              normal.Rebate.HasValue ? normal.Rebate.Value.ToString() : "",
                              bargain.AllowOnlySelf ? "是" : "可以采取平台上其他特价政策",
                              bargain.Rebate.HasValue ? bargain.Rebate.ToString() : "");
                    i++;
                }
            }
            string newContent = "";
            if (setting != null)
            {
                newContent = string.Format("限制组Id:{0},是否是全局:{1},所属公司Id:{2},所属公司组Id:{3}",
                setting.Id.ToString(), setting.IsGlobal ? "是" : "否", setting.CompanyId.HasValue ? setting.CompanyId.ToString() : "", setting.IncomeGroupId.HasValue ? setting.IncomeGroupId.ToString() : "");
                int i = 1;
                foreach (var item in setting.Limitation)
                {
                    var normal = item.Rebate.Where(q => q.Type == PurchaseLimitationRateType.Normal).FirstOrDefault();
                    var bargain = item.Rebate.Where(q => q.Type == PurchaseLimitationRateType.Bargain).FirstOrDefault();
                    newContent += string.Format("限制信息" + i + "：限制信息Id:{0},航空公司:{1},出发城市:{2}", item.Id.ToString(), item.Airlines, item.Departures);
                    newContent += string.Format("仅允许采购我发布的普通政策:{0},成人默认返点(普通政策):{1},仅允许采购我发布的特价政策:{2},成人默认返点(特价政策):{3}",
                              normal.AllowOnlySelf ? "是" : "可以采取平台上其他普通政策",
                              normal.Rebate.HasValue ? normal.Rebate.Value.ToString() : "",
                              bargain.AllowOnlySelf ? "是" : "可以采取平台上其他特价政策",
                              bargain.Rebate.HasValue ? bargain.Rebate.ToString() : "");
                    i++;
                }
            }
            saveUpdateLog("分组采购设置", orginalContent, newContent, OperatorRole.User, setting.Id.ToString(), operatorAccount);
            
        }
        /// <summary>
        /// 修改全局采买限制
        /// </summary>
        /// <param name="setting">全局采买限制</param>
        /// <param name="operatorAccount">操作员</param>
        public static void UpdatePurchaseRestrictionSettingGlobal(PurchaseLimitationGroup setting, PurchaseLimitationType orginalType,string operatorAccount)
        {
            var orginalSetting = QueryPurchaseLimitationGroup(setting.CompanyId.Value);
            var repository = Factory.CreatePurchaseRestrictionSettingRepository();
            repository.UpdatePurchaseRestrictionSettingGlobal(setting);
            string orginalContent = "";
            if (orginalType != PurchaseLimitationType.Global)
            {
                orginalContent = orginalType.GetDescription();
            }
            if (orginalSetting != null)
            {
                orginalContent = string.Format("限制组Id:{0},是否是全局:{1},所属公司Id:{2},所属公司组Id:{3}",
                 orginalSetting.Id.ToString(), orginalSetting.IsGlobal ? "是" : "否", orginalSetting.CompanyId.HasValue ? orginalSetting.CompanyId.ToString() : "", orginalSetting.IncomeGroupId.HasValue ? orginalSetting.IncomeGroupId.ToString() : "");
                int i = 1;
                foreach (var item in orginalSetting.Limitation)
                {
                    var normal = item.Rebate.Where(q => q.Type == PurchaseLimitationRateType.Normal).FirstOrDefault();
                    var bargain = item.Rebate.Where(q => q.Type == PurchaseLimitationRateType.Bargain).FirstOrDefault();
                    orginalContent += string.Format("限制信息" + i + "：限制信息Id:{0},航空公司:{1},出发城市:{2}", item.Id.ToString(), item.Airlines, item.Departures);
                    orginalContent += string.Format("仅允许采购我发布的普通政策:{0},成人默认返点(普通政策):{1},仅允许采购我发布的特价政策:{2},成人默认返点(特价政策):{3}",
                              normal.AllowOnlySelf ? "是" : "可以采取平台上其他普通政策",
                              normal.Rebate.HasValue ? normal.Rebate.Value.ToString() : "",
                              bargain.AllowOnlySelf ? "是" : "可以采取平台上其他特价政策",
                              bargain.Rebate.HasValue ? bargain.Rebate.ToString() : "");
                    i++;
                }
            }
            string newContent = "";
            if (setting != null)
            {
                newContent = string.Format("限制组Id:{0},是否是全局:{1},所属公司Id:{2},所属公司组Id:{3}",
                setting.Id.ToString(), setting.IsGlobal ? "是" : "否", setting.CompanyId.HasValue ? setting.CompanyId.ToString() : "", setting.IncomeGroupId.HasValue ? setting.IncomeGroupId.ToString() : "");
                int i = 1;
                foreach (var item in setting.Limitation)
                {
                    var normal = item.Rebate.Where(q => q.Type == PurchaseLimitationRateType.Normal).FirstOrDefault();
                    var bargain = item.Rebate.Where(q => q.Type == PurchaseLimitationRateType.Bargain).FirstOrDefault();
                    newContent += string.Format("限制信息" + i + "：限制信息Id:{0},航空公司:{1},出发城市:{2}", item.Id.ToString(), item.Airlines, item.Departures);
                    newContent += string.Format("仅允许采购我发布的普通政策:{0},成人默认返点(普通政策):{1},仅允许采购我发布的特价政策:{2},成人默认返点(特价政策):{3}",
                              normal.AllowOnlySelf ? "是" : "可以采取平台上其他普通政策",
                              normal.Rebate.HasValue ? normal.Rebate.Value.ToString() : "",
                              bargain.AllowOnlySelf ? "是" : "可以采取平台上其他特价政策",
                              bargain.Rebate.HasValue ? bargain.Rebate.ToString() : "");
                    i++;
                }
            }
                  saveUpdateLog("全局采购设置", orginalContent,newContent, OperatorRole.User,setting.Id.ToString(),operatorAccount);
            
        }
        /// <summary>
        /// 将全局设置成无效或分组
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="operatorAccount"></param>
        public static void UpdatePurchaseLimitationGroup(Guid companyId,PurchaseLimitationType orginalType, PurchaseLimitationType type, string operatorAccount)
        {
            var repository = Factory.CreatePurchaseRestrictionSettingRepository();
            repository.UpdatePurchaseLimitationGroup(companyId);
            saveLog(OperationType.Update,string.Format("全局设置采买限制:将采买限制类型由{0}修改为{1}",orginalType.GetDescription(),type.GetDescription()),OperatorRole.User,companyId.ToString(),operatorAccount);
        }
        /// <summary>
        /// 查询分组采买限制
        /// </summary>
        /// <param name="groupId">分组Id</param>
        public static PurchaseLimitationGroup QueryPurchaseLimitationGroupInfo(Guid groupId)
        {
            var repository = Factory.CreatePurchaseRestrictionSettingRepository();
            return repository.QueryPurchaseRestrictionSettingView(groupId);
        }
        /// <summary>
        /// 查询全局采买限制
        /// </summary>
        /// <param name="companyId">公司Id</param>
        public static PurchaseLimitationGroup QueryPurchaseLimitationGroup(Guid companyId)
        {
            var repository = Factory.CreatePurchaseRestrictionSettingRepository();
            return repository.QueryPurchaseRestrictionSetting(companyId);
        }

        public static PurchaseLimitationGroup QueryPurchaseLimitation(Guid superId, Guid purchseId)
        {
            var repository = Factory.CreatePurchaseRestrictionSettingRepository();
            return repository.QueryPurchaseRestrictionSettingList(superId, purchseId);
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
            var log = new Log.Domain.OperationLog(OperationModule.单位, operationType, account, role, key, content);
            try
            {
                LogService.SaveOperationLog(log);
            }
            catch { }
        }
        #endregion
    }
}
