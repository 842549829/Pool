using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Organization
{
    public static class OEMStyleService
    {
        /// <summary>
        /// 添加风格
        /// </summary>
        /// <param name="style"></param>
        /// <param name="operatorAccount"></param>
        public static void InsertOEMStyle(OEMStyle style, string operatorAccount)
        {
            style.Id = Guid.NewGuid();
            var repository = Factory.CreateOEMStyleRepository();
            repository.Insert(style);
        }
        /// <summary>
        /// 删除风格
        /// </summary>
        /// <param name="styleId"></param>
        /// <param name="operatorAccount"></param>
        public static void DeleteOEMStyle(Guid styleId, string operatorAccount)
        {
            var repository = Factory.CreateOEMStyleRepository();
            repository.Delete(styleId);
        }
        /// <summary>
        /// 修改风格
        /// </summary>
        /// <param name="style"></param>
        /// <param name="operatorAccount"></param>
        public static void UpdateOEMStyle(OEMStyle style, string operatorAccount)
        {
            var repository = Factory.CreateOEMStyleRepository();
            repository.Update(style);
        }
        /// <summary>
        /// 启用禁用
        /// </summary>
        /// <param name="styleId"></param>
        /// <param name="enable"></param>
        /// <param name="operatorAccount"></param>
        public static void UpdateEnable(Guid styleId, bool enable, string operatorAccount)
        {
            var repository = Factory.CreateOEMStyleRepository();
            repository.UpdateEnable(styleId, enable);
        }
        /// <summary>
        /// 查询一条
        /// </summary>
        /// <param name="styleId"></param>
        /// <returns></returns>
        public static OEMStyle QueryOEMStyle(Guid styleId)
        {
            var repository = Factory.CreateOEMStyleRepository();
            return repository.Query(styleId);
        }
        public static IEnumerable<OEMStyle> QueryOEMStyles()
        {
            var repository = Factory.CreateOEMStyleRepository();
            return repository.Query();
        }
        public static IEnumerable<OEMStyle> QueryOEMVailStyles()
        {
            var repository = Factory.CreateOEMStyleRepository();
            return repository.Query().OrderBy(item => item.Sort).Where(item => item.Enable);
        }
        #region "日志"
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
