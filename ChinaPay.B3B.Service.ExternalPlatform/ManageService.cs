using System.Collections.Generic;
using ChinaPay.B3B.Service.ExternalPlatform.Repository;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.ExternalPlatform {
    public static class ManageService {
        /// <summary>
        /// 查询外平台的设置信息
        /// </summary>
        public static Setting QuerySetting(Common.Enums.PlatformType platform) {
            var repository = Factory.CreateSettingReposity();
            return repository.QuerySetting(platform);
        }
        /// <summary>
        /// 查询所有外平台的设置信息
        /// </summary>
        public static IEnumerable<Setting> QuerySettings() {
            var repository = Factory.CreateSettingReposity();
            return repository.QuerySettings();
        }
        /// <summary>
        /// 插入外平台的设置信息
        /// </summary>
        /// <param name="setting"></param>
        public static void InsertSetting(Setting setting,string account)
        {
            var repository = Factory.CreateSettingReposity();
             repository.InsertSetting(setting);
             saveAddLog("外平台接口设置",string.Format("平台:{0},扣点:{1},政策差:{2},出票方:{3},状态:{4},自动支付方式:{5}",setting.Platform.GetDescription(),
                 setting.Deduct.ToString(), setting.RebateBalance.ToString(), setting.ProviderAccount, setting.Enabled ? "启用" : "禁用", setting.PayInterface.Join("|", item => item.GetDescription())),
                 setting.Platform.GetDescription(),account);
        }
        /// <summary>
        /// 修改外平台的设置信息
        /// </summary>
        public static void UpdateSetting(Setting setting,string account) {
            var orginalSetting = QuerySetting(setting.Platform);
            var repository = Factory.CreateSettingReposity();
            repository.UpdateSetting(setting);
            saveUpdateLog("外接口政策设置",
                           string.Format("平台:{0},扣点:{1},政策差:{2},出票方:{3},状态:{4},自动支付方式:{5}", orginalSetting.Platform.GetDescription(),
                 orginalSetting.Deduct.ToString(), orginalSetting.RebateBalance.ToString(), orginalSetting.ProviderAccount, orginalSetting.Enabled ? "启用" : "禁用", orginalSetting.PayInterface.Join("|", item => item.GetDescription())),
                            string.Format("平台:{0},扣点:{1},政策差:{2},出票方:{3},状态:{4},自动支付方式:{5}", setting.Platform.GetDescription(),
                 setting.Deduct.ToString(), setting.RebateBalance.ToString(), setting.ProviderAccount, setting.Enabled ? "启用" : "禁用", setting.PayInterface.Join("|", item => item.GetDescription())),
                            setting.Platform.GetDescription(),
                            account);
        }
        /// <summary>
        /// 修改外平台的设置状态
        /// </summary>
        /// <param name="platform">外平台</param>
        /// <param name="enabled">状态</param>
        public static void UpdateStatus(Common.Enums.PlatformType platform, bool enabled,string account) {
            var reposity = Factory.CreateSettingReposity();
            reposity.UpdateStatus(platform, enabled);
            saveUpdateLog("外接口政策设置",
                string.Format("平台:{0},状态",!enabled?"启用":"禁用"),
                string.Format("平台:{0},状态", enabled ? "启用" : "禁用"),
                         platform.GetDescription(),
                         account);
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