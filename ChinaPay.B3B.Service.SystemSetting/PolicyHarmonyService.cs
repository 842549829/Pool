using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.SystemSetting.PolicyHarmony;
using ChinaPay.B3B.Service.SystemSetting.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.SystemSetting.Repository;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.SystemSetting
{
    public static class PolicyHarmonyService
    {
        #region"查询"
        public static IEnumerable<PolicyHarmonyView> Query(PolicyHarmonyQueryCondition condition)
        {
            var repository = Factory.CreatePolicyHarmonyRepository();
            return repository.Query(condition);
        }
        public static PolicyHarmonyView Query(Guid id)
        {
            var reposity = Factory.CreatePolicyHarmonyRepository();
            return reposity.Query(id);
        }
        #endregion
        #region"新增"
        public static void Insert(PolicyHarmony harmony, string operatorAccount)
        {
            var repository = Factory.CreatePolicyHarmonyRepository();
            repository.Insert(harmony);
            // 记录日志
            string content = string.Format("政策协调Id:{0},航空公司:{1},始发地:{2},目的地:{3},政策类型:{4},受限城市:{5},帐号类型:{6],返佣类型:{7},有效时间:{8}-{9},政策协调值:{10},备注:{11}",
                                           harmony.Id,harmony.Airlines.Join(","),harmony.Departure.Join(","),harmony.Arrival.Join(","),harmony.PolicyType,harmony.CityLimit.Join(","),
                                           harmony.IsVIP,harmony.DeductionType,harmony.EffectiveDateRange.Lower.Date,harmony.EffectiveDateRange.Upper.Date,harmony.HarmonyValue,harmony.Remark);
            saveAddLog("政策协调", content, harmony.Id.ToString(), operatorAccount);
        }
        #endregion
        #region"修改"
        public static void Update(PolicyHarmony harmony, string operatorAccount)
        {
            var repository = Factory.CreatePolicyHarmonyRepository();
            repository.Update(harmony);
            // 记录日志
            var oldHarmony = PolicyHarmonyService.Query(harmony.Id);
            string orginialContent = string.Format("政策协调Id:{0},航空公司:{1},始发地:{2},目的地:{3},政策类型:{4},受限城市:{5},帐号类型:{6],返佣类型:{7},有效时间:{8}-{9},政策协调值:{10},备注:{11}",
                                           oldHarmony.Id, oldHarmony.Airlines.Join(","), oldHarmony.Departure.Join(","), oldHarmony.Arrival.Join(","), oldHarmony.PolicyType, oldHarmony.CityLimit.Join(","),
                                           oldHarmony.IsVIP, oldHarmony.DeductionType, oldHarmony.EffectiveDate.Lower.Date.ToString("yyyy-MM-dd"), oldHarmony.EffectiveDate.Upper.Date.ToString("yyyy-MM-dd"), oldHarmony.HarmonyValue, oldHarmony.Remark);
            string newContent = string.Format("政策协调Id:{0},航空公司:{1},始发地:{2},目的地:{3},政策类型:{4},受限城市:{5},帐号类型:{6],返佣类型{7},有效时间:{8}-{9},政策协调值:{9},备注:{10}",
                                            harmony.Id, harmony.Airlines.Join(","), harmony.Departure.Join(","), harmony.Arrival.Join(","), harmony.PolicyType, harmony.CityLimit.Join(","),
                                            harmony.IsVIP, harmony.DeductionType, harmony.EffectiveDateRange.Lower.Date.ToString("yyyy-MM-dd"), harmony.EffectiveDateRange.Upper.Date.ToString("yyyy-MM-dd"), harmony.HarmonyValue, harmony.Remark);
            saveUpdateLog("政策协调",orginialContent,newContent,harmony.Id.ToString(),operatorAccount);
        }
        #endregion
        #region"删除"
        public static void Delete(Guid id, string operatorAccount)
        {
            var repository = Factory.CreatePolicyHarmonyRepository();
            repository.Delete(id);
            // 记录日志
            var view = PolicyHarmonyService.Query(id);
            string content = string.Format("政策协调Id:{0},航空公司:{1},始发地:{2},目的地:{3},政策类型:{4},受限城市:{5},帐号类型:{6],返佣类型:{7},有效时间:{8}-{9},政策协调值:{10},备注:{11}",
                                           view.Id, view.Airlines.Join(","), view.Departure.Join(","), view.Arrival.Join(","), view.PolicyType, view.CityLimit.Join(","),
                                           view.IsVIP, view.DeductionType, view.EffectiveDate.Lower.Date.ToString("yyyy-MM-dd"),view.EffectiveDate.Upper.Date.ToString("yyyy-MM-dd"), view.HarmonyValue, view.Remark);
            saveDeleteLog("政策协调", content, id.ToString(), operatorAccount);
        }
        public static void Delete(IEnumerable<Guid> ids, string operatorAccount)
        {
            var repository = Factory.CreatePolicyHarmonyRepository();
            repository.Delete(ids);
            // 记录日志
            string content = "";
            foreach (var item in ids)
            {
                var view = PolicyHarmonyService.Query(item);
                content += string.Format("政策协调Id:{0},航空公司:{1},始发地:{2},目的地:{3},政策类型:{4},受限城市:{5},帐号类型:{6],返佣类型:{7},有效时间:{8}-{9},政策协调值:{10},备注:{11}",
                                               view.Id, view.Airlines.Join(","), view.Departure.Join(","), view.Arrival.Join(","), view.PolicyType, view.CityLimit.Join(","),
                                               view.IsVIP, view.DeductionType, view.EffectiveDate.Lower.Date.ToString("yyyy-MM-dd"),view.EffectiveDate.Upper.Date.ToString("yyyy-MM-dd"), view.HarmonyValue, view.Remark);
            }
            saveDeleteLog("政策协调", content, ids.Join(",", item2 => item2.ToString()), operatorAccount);
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
