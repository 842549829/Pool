using System;
using System.Collections.Generic;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.SystemSetting.Domain;
using ChinaPay.B3B.DataTransferObject.SystemSetting.VIPHarmony;
using ChinaPay.B3B.Service.SystemSetting.Repository;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.SystemSetting {
    public static class VIPHarmonyService
    {
        #region"查询"
        public static IEnumerable<VIPHarmonyListView> Query() {
            var repository = Factory.CreateVIPHarmonyRepository();
            return repository.Query();
        }
        public static VIPHarmonyView Query(Guid id)
        {
            var repositoy = Factory.CreateVIPHarmonyRepository();
            return repositoy.Query(id);
        }
        #endregion
        #region"新增"
        public static void Insert(VIPHarmony harmony, string operatorAccount) {
            var repository = Factory.CreateVIPHarmonyRepository();
            repository.Insert(harmony);
            // 记录日志
            string content = string.Format("区域Id:{0},受限航空公司:{1},受限出港城市:{1},添加帐号:{2},添加时间:{3},备注:{4}",
                                            harmony.Id,harmony.AirlineLimit.Join(","),harmony.CityLimit.Join(","),harmony.Account,harmony.AddTime,harmony.Remark);
            saveAddLog("VIP协调", content, harmony.Id.ToString(), operatorAccount);
        }
        #endregion
        #region"修改"
        public static void Update(VIPHarmony harmony, string operatorAccount) {
            var repository = Factory.CreateVIPHarmonyRepository();
            repository.Update(harmony);
            // 记录日志
            var orginialView = VIPHarmonyService.Query(harmony.Id);
            string orginialContent = string.Format("区域Id:{0},受限航空公司:{1},受限出港城市:{1},备注:{2}",
                                            orginialView.Id, orginialView.AirlineLimit.Join(","), orginialView.CityLimit.Join(","), orginialView.Remark);
            string newContent = string.Format("区域Id:{0},受限航空公司:{1},受限出港城市:{1},备注:{2}",
                                            harmony.Id, harmony.AirlineLimit.Join(","), harmony.CityLimit.Join(","), harmony.Remark);
            saveUpdateLog("VIP协调", orginialContent, newContent, harmony.Id.ToString(), operatorAccount);
        }
        #endregion
        #region"删除"
        public static void Delete(Guid id, string operatorAccount) {
            var repository = Factory.CreateVIPHarmonyRepository();
            repository.Delete(id);
            // 记录日志
            var management = VIPHarmonyService.Query(id);
            string content = string.Format("区域Id:{0},受限航空公司:{1},受限出港城市:{1},备注:{2}",
                                            management.Id, management.AirlineLimit.Join(","), management.CityLimit.Join(","), management.Remark);
            saveDeleteLog("VIP协调", content, id.ToString(), operatorAccount);
        }
        public static void Delete(IEnumerable<Guid> ids, string operatorAccount) {
            var repository = Factory.CreateVIPHarmonyRepository();
            repository.Delete(ids);
            // 记录日志
            string content = "";
            foreach (var item in ids)
            {
                var management = VIPHarmonyService.Query(item);
                content += string.Format("区域Id:{0},受限航空公司:{1},受限出港城市:{1},备注:{2}",
                                                management.Id, management.AirlineLimit.Join(","), management.CityLimit.Join(","), management.Remark);
            }
            saveDeleteLog("VIP协调", content, ids.Join(",",item=>item.ToString()), operatorAccount);
        }
        #endregion
        #region "日志"
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
