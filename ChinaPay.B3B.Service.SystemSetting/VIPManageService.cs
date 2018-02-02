using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.SystemSetting.VIP;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.SystemSetting.Repository;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.SystemSetting {
    public static class VIPManageService
    {
        #region"查询"
        /// <summary>
        /// 查询所有VIP帐号信息
        /// </summary>
        /// <returns>VIP帐号信息列表</returns>
        public static IEnumerable<VIPManageView> Query() {
            var repository = Factory.CreateVIPManagerRepository();
            return repository.Query();
        }
        /// <summary>
        /// 根据条件查询VIP帐号信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>VIP帐号信息列表</returns>
        public static IEnumerable<VIPManageView> Query(VIPManageQueryCondition condition) {
            var repository = Factory.CreateVIPManagerRepository();
            return repository.Query(condition);
        }
        /// <summary>
        /// 根据Id查询VIP帐号信息
        /// </summary>
        /// <param name="id">VIP帐号Id</param>
        /// <returns></returns>
        public static Domain.VIPManagement Query(Guid id)
        {
            var reposity = Factory.CreateVIPManagerRepository();
            return reposity.Query(id);
        }
        #endregion
        #region"新增"
        public static void Insert(Domain.VIPManagement managerment, string operatorAccount)
        {
            var repository = Factory.CreateVIPManagerRepository();
            repository.Insert(managerment);
            // 记录日志
            string content = string.Format("公司Id:{0},是否VIP:{1}",managerment.Company,managerment.IsVip);
            saveAddLog("VIP帐号管理", content, managerment.Id.ToString(), operatorAccount);
        }
        #endregion
        #region"修改"
        /// <summary>
        /// 修改是否是VIP帐号
        /// </summary>
        /// <param name="id">VIP帐号Id</param>
        /// <param name="enabled">ture/false</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void Update(Guid id, bool enabled, string operatorAccount) {
            var repository = Factory.CreateVIPManagerRepository();
            repository.Update(id, enabled);
            // 记录日志
            var vipManageView = VIPManageService.Query(id);
            string originalContent = string.Format("公司Id:{0},是否VIP:{1}",vipManageView.Company, vipManageView.IsVip);
            string newContent = string.Format("公司Id:{0},是否VIP:{1}", vipManageView.Company, enabled);
            saveUpdateLog("VIP帐号管理", originalContent, newContent, id.ToString(), operatorAccount);
        }
        /// <summary>
        /// 批量修改是否为VIP
        /// </summary>
        /// <param name="ids">VIP帐号管理中Id</param>
        /// <param name="enabled">true/false</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void Update(IEnumerable<Guid> ids, bool enabled, string operatorAccount) {
            var repository = Factory.CreateVIPManagerRepository();
            repository.Update(ids, enabled);
            // 记录日志
            string originalContent = "";
            foreach (var item in ids)
            {
                var originalView = VIPManageService.Query(item);
                originalContent += string.Format("公司Id:{0},是否VIP:{1}",originalView.Company,originalView.IsVip);
            }
            string newContent = "";
            foreach (var item in ids)
            {
                var vipManageView = VIPManageService.Query(item);
                newContent += string.Format("公司Id:{0},是否VIP:{1}", vipManageView.Company, enabled);
            }
            saveUpdateLog("VIP帐号管理", originalContent, newContent, ids.Join(",",item=>item.ToString()), operatorAccount);
        }
        #endregion
        #region"删除"
        public static void Delete(Guid id, string operatorAccount)
        {
            var repository = Factory.CreateVIPManagerRepository();
            repository.Delete(id);
            // 记录日志
            var management = VIPManageService.Query(id);
            saveDeleteLog("VIP帐号", string.Format("公司Id:{0},是否VIP:{1}",management.Company,management.IsVip), id.ToString(), operatorAccount);
        }
        public static void Delete(IEnumerable<Guid> ids, string operatorAccount)
        {
            var reposity = Factory.CreateVIPManagerRepository();
            reposity.Delete(ids);
            //记录日志
            string content = "";
            foreach (var item in ids)
            {
                var managerment = VIPManageService.Query(item);
                content += string.Format("公司Id:{0},是否VIP:{1}",managerment.Company,managerment.IsVip);
            }
            saveDeleteLog("批量VIP帐号", content, ids.Join(",", item => item.ToString()), operatorAccount);
        }
        #endregion
        #region"日志"
        static void saveAddLog(string itemName, string content, string key, string account)
        {
            saveLog(OperationType.Insert,string.Format("添加"+itemName+"。"+ content), key, account);
        }
        static void saveUpdateLog(string itemName, string originalContent, string newContent, string key, string account)
        {
            saveLog(OperationType.Update, string.Format("修改{0}。由 {1} 修改为 {2}", itemName, originalContent, newContent), key, account);
        }
        static void saveDeleteLog(string itemName, string content, string key, string account)
        {
            saveLog(OperationType.Delete,string.Format("删除"+itemName+"。"+ content), key, account);
        }
        static void saveLog(OperationType operationType,  string content, string key, string account) 
        {
            var log = new Log.Domain.OperationLog(OperationModule.系统设置, operationType, account, OperatorRole.Platform, key, content);
            try
            {
                LogService.SaveOperationLog(log);
            }
            catch { }
        }
        #endregion
    }
}
