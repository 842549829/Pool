using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.SystemSetting.MarketingArea;
using ChinaPay.B3B.Service.SystemSetting.Domain;
using ChinaPay.B3B.Service.SystemSetting.Repository;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.SystemSetting {
    public class AreaService
    {
        #region"查询"
        /// <summary>
        /// 查询销售区域列表
        /// </summary>
        /// <param name="condition">区域查询条件</param>
        /// <returns>销售区域列表</returns>
        public static IEnumerable<AreaListView> Query(AreaQueryConditon condition) {
            var repository = Factory.CreateAreaRepository();
            return repository.Query(condition);
        }
        /// <summary>
        /// 查询销售区域列表(分页)
        /// </summary>
        /// <param name="condition">区域查询条件</param>
        /// <returns>销售区域列表</returns>
        public static IEnumerable<AreaListView> Query(AreaQueryConditon condition,Pagination pagination)
        {
            var repository = Factory.CreateAreaRepository();
            return repository.Query(condition,pagination);
        }
        /// <summary>
        /// 查询区域关联列表
        /// </summary>
        /// <param name="condition">区域关联条件</param>
        /// <returns>返回区域关联列表</returns>
        public static IEnumerable<AreaRelationListView> Query(AreaRelationQueryCondtion condition,Pagination pagination) {
            var repository = Factory.CreateAreaRepository();
            return repository.Query(condition,pagination);
        }
        /// <summary>
        /// 根据区域Id查询区域
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <returns>销售区域信息</returns>
        public static AreaView Query(Guid id)
        {
            var reposity = Factory.CreateAreaRepository();
            return reposity.Query(id);
        }
        /// <summary>
        /// 区域关联
        /// </summary>
        /// <param name="provinceCode">省份代码</param>
        /// <returns>区域关联信息</returns>
        public static AreaRelationView QueryRelation(string provinceCode)
        {
            var reposity = Factory.CreateAreaRepository();
            return reposity.QueryRelation(provinceCode);
        }
        /// <summary>
        /// 查询区域代码
        /// </summary>
        /// <param name="name">区域名</param>
        /// <returns></returns>
        public static Guid QueryAreaCode(string name)
        {
            var reposity = Factory.CreateAreaRepository();
            return reposity.QueryAreaCode(name);
        }
        #endregion
        #region"新增"
        /// <summary>
        /// 新增区域
        /// </summary>
        /// <param name="areaView">区域信息</param>
        /// <param name="operatorAccount">操作者帐号</param>
        public static void InsertArea(AreaView areaView, string operatorAccount) {
            if (null == areaView)
                throw new ArgumentNullException("areaView");
            var area = new SellArea();
            area.Name = areaView.Name;
            area.Remark = areaView.Remark;
            var repository = Factory.CreateAreaRepository();
            repository.InsertArea(area);
            saveAddLog("销售区域", string.Format("区域名称:{0},备注:{1}",areaView.Name,areaView.Remark),area.Id.ToString(),operatorAccount);
        }
        /// <summary>
        /// 新增区域关联
        /// </summary>
        /// <param name="areaName">区域名称</param>
        /// <param name="provinceCode">省份代码</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void InsertAreaRelation(string areaName, string provinceCode, string operatorAccount) {
            var repository = Factory.CreateAreaRepository();
            Guid area =AreaService.QueryAreaCode(areaName);
            repository.InsertAreaRelation(area, provinceCode);
            saveAddLog("区域关联",string.Format("区域Id:{0},省份代码:{1}",area,provinceCode),provinceCode.ToString(),operatorAccount);
        }
        #endregion
        #region"修改"
        /// <summary>
        /// 修改销售区域
        /// </summary>
        /// <param name="areaView">销售区域信息</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void UpdateArea(Guid id,AreaView areaView, string operatorAccount)
        {
            var area = new SellArea(id);
            area.Name = areaView.Name;
            area.Remark = areaView.Remark;
            var repository = Factory.CreateAreaRepository();
            repository.UpdateArea(area);
            // 记录日志
            var originalView = AreaService.Query(id);
            string originalContent = string.Format("区域名称:{0},区域备注:{1}",originalView.Name,originalView.Remark);
            string newContent=string.Format("区域名称:{0},区域备注:{1}",areaView.Name,areaView.Remark);
            saveUpdateLog("销售区域",originalContent,newContent,id.ToString(),operatorAccount);
        }
        /// <summary>
        /// 修改区域关联
        /// </summary>
        /// <param name="areaName">区域名称</param>
        /// <param name="provinceCode">省份代码</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void UpdateAreaRelation(string areaName, string provinceCode, string operatorAccount) {
            var areaId = AreaService.QueryAreaCode(areaName);
            var repository = Factory.CreateAreaRepository();
            repository.UpdateAreaRelation(areaId, provinceCode);
            // 记录日志
            var oldView =AreaService.QueryRelation(provinceCode);
            string originalContent = string.Format("省份名称:{0},区域名称:{1}",oldView.ProcinceName,oldView.AreaName);
            string newContent = string.Format("省份名称:{0},区域名称:{1}",oldView.ProcinceName,areaName);
            saveUpdateLog("区域关联",originalContent,newContent,provinceCode,operatorAccount);
        }
        #endregion
        #region"删除"
        /// <summary>
        /// 根据区域Id删除区域
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void DeleteArea(Guid id, string operatorAccount)
        {
            var repository = Factory.CreateAreaRepository();
            var areaView = AreaService.Query(id);
            repository.DeleteArea(id);
            // 记录日志
            saveDeleteLog("销售区域", string.Format("区域名称:{0},备注:{1}", areaView.Name, areaView.Remark), id.ToString(), operatorAccount);
        }
        /// <summary>
        /// 批量删除区域
        /// </summary>
        /// <param name="ids">区域Id集合</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void DeleteArea(IEnumerable<Guid> ids, string operatorAccount)
        {
            string content = "";
            foreach (var item in ids)
            {
                var areaView = AreaService.Query(item);
                content += string.Format("区域名称:{0},备注:{1}。", areaView.Name, areaView.Remark);
            }
            var repository = Factory.CreateAreaRepository();
            repository.DeleteArea(ids);
            // 记录日志
            saveDeleteLog("销售区域",content , ids.Join(",",item=>item.ToString()), operatorAccount);
        }
        /// <summary>
        /// 删除区域关联
        /// </summary>
        /// <param name="provinceCode">省份代码</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void DeleteAreaRelation(string provinceCode, string operatorAccount) {
            var repository = Factory.CreateAreaRepository();
            var areaRelationView = AreaService.QueryRelation(provinceCode);
            repository.DeleteAreaRelation(provinceCode);
            // 记录日志
           saveDeleteLog("区域关联", string.Format("区域名称:{0},省份代码:{1}", areaRelationView.AreaName, areaRelationView.Province),provinceCode,operatorAccount);
        }
        /// <summary>
        /// 批量删除区域关联
        /// </summary>
        /// <param name="provinceCode">省份代码</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void DeleteAreaRelation(IEnumerable<string> provinceCode, string operatorAccount) {
            var repository = Factory.CreateAreaRepository();
            repository.DeleteAreaRelation(provinceCode);
            // 记录日志
            string content = "";
            foreach (var item in provinceCode)
            {
                var areaRelationView = AreaService.QueryRelation(item);
                content += string.Format("区域名称:{0},省份代码:{1}",areaRelationView.AreaName,areaRelationView.ProcinceName);
            }
            saveDeleteLog("区域关联",content , provinceCode.ToString(), operatorAccount);
        }
        #endregion
        #region"日志"
        static void saveAddLog(string itemName, string content, string key, string account)
        {
            saveLog(OperationType.Insert,"添加"+itemName+"。"+ content, key, account);
        }
        static void saveUpdateLog(string itemName, string originalContent, string newContent, string key, string account)
        {
            saveLog(OperationType.Update, string.Format("修改{0}。由 {1} 修改为 {2}", itemName, originalContent, newContent), key, account);
        }
        static void saveDeleteLog(string itemName, string content, string key, string account)
        {
            saveLog(OperationType.Delete,"删除"+itemName+"。"+ content, key, account);
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
