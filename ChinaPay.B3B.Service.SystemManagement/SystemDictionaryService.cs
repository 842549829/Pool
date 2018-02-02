using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.SystemManagement {
    /// <summary>
    /// 系统字典表服务类
    /// </summary>
    public static class SystemDictionaryService {
        /// <summary>
        /// 查询字典表子项
        /// </summary>
        public static IEnumerable<SystemDictionaryItem> Query(SystemDictionaryType type) {
            var systemDictionary = SystemDictionarys.Instance[type];
            if(systemDictionary != null) {
                return systemDictionary.Items;
            }
            return new List<SystemDictionaryItem>();
        }
        /// <summary>
        /// 修改字典表子项
        /// </summary>
        public static void UpdateItem(SystemDictionaryType type, SystemDictionaryItem item, string account) {
            if(null == item)
                throw new ArgumentNullException("item");
            var originalItem = SystemDictionarys.Instance[type][item.Id];
            if(null == originalItem)
                throw new ChinaPay.Core.CustomException("原子项不存在");
            var originalContent = originalItem.ToString();
            // 修改数据
            SystemDictionarys.Instance.UpdateItem(type, item);
            // 记录日志
            var logContent = string.Format("修改字典表 [{0}] 的子项。由 {1} 修改为 {2}", type.GetDescription(), originalContent, item.ToString());
            var log = new Service.Log.Domain.OperationLog(OperationModule.系统字典表, OperationType.Update, account, OperatorRole.Platform, item.Id.ToString(), logContent, DateTime.Now);
            Service.LogService.SaveOperationLog(log);
        }
        /// <summary>
        /// 添加字典表子项
        /// </summary>
        public static void AddItem(SystemDictionaryType type, SystemDictionaryItem item, string account) {
            if(null == item)
                throw new ArgumentNullException("item");
            // 添加数据
            SystemDictionarys.Instance.AddItem(type, item);
            // 记录日志
            var logContent = string.Format("添加字典表 [{0}] 的子项。内容：{1}", type.GetDescription(), item.ToString());
            var log = new Service.Log.Domain.OperationLog(OperationModule.系统字典表, OperationType.Insert, account, OperatorRole.Platform, item.Id.ToString(), logContent, DateTime.Now);
            Service.LogService.SaveOperationLog(log);
        }
        /// <summary>
        /// 删除字典表子项
        /// </summary>
        public static void DeleteItem(SystemDictionaryType type, Guid item, string account) {
            var originalItem = SystemDictionarys.Instance[type][item];
            if(null == originalItem)
                throw new ChinaPay.Core.CustomException("原子项不存在");
            var originalContent = originalItem.ToString();
            // 删除数据
            SystemDictionarys.Instance.DeleteItem(type, item);
            // 记录日志
            var logContent = string.Format("删除字典表 [{0}] 的子项。内容：{1}", type.GetDescription(), originalContent);
            var log = new Service.Log.Domain.OperationLog(OperationModule.系统字典表, OperationType.Delete, account, OperatorRole.Platform, item.ToString(), logContent, DateTime.Now);
            Service.LogService.SaveOperationLog(log);
        }
    }
}