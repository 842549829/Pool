using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.SystemSetting.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.SystemSetting.OnLineCustomer;
using ChinaPay.B3B.Service.SystemSetting.Repository;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.SystemSetting {
    /// <summary>
    /// 在线客服设置
    /// </summary>
    public static class OnLineCustomerService {
        #region"查询"
        /// <summary>
        /// 查找平台Id
        /// </summary>
        /// <returns></returns>
        public static Guid QueryPlatForm() {
            var reposity = Factory.CreateOnLineCustomerRepository();
            return reposity.QueryPlatFormCompany();
        }
        /// <summary>
        /// 根据Id查询在线客服信息
        /// </summary>
        /// <param name="id">在线客服Id</param>
        /// <returns></returns>
        public static OnLineCustomerView Query(Guid company) {
            var reposity = Factory.CreateOnLineCustomerRepository();
            return reposity.Query(company);
        }
        /// <summary>
        /// 查询平台在线客服设置
        /// </summary>
        public static OnLineCustomer Query(Guid company, PublishRoles roles)
        {
            //Guid company = QueryPlatForm();
            var reposity = Factory.CreateOnLineCustomerRepository();
            return reposity.Query(company, roles);
        }
        /// <summary>
        /// 查询OEM在线客服设置
        /// </summary>
        /// <param name="company">公司Id</param>
        public static OnLineCustomer QueryOEM(Guid company) {
            var reposity = Factory.CreateOnLineCustomerRepository();
            return reposity.Query(company, PublishRoles.OEM);
        }
        /// <summary>
        /// 查询分组信息
        /// </summary>
        /// <param name="divideGroup">分组Id</param>
        /// <returns>分组信息</returns>
        public static DivideGroupView QueryDivideGroup(Guid divideGroup) {
            var reposity = Factory.CreateOnLineCustomerRepository();
            return reposity.QueryDivideGroup(divideGroup);
        }
        /// <summary>
        /// 查询分组信息列表
        /// </summary>
        /// <param name="online">在线服务Id</param>
        /// <returns></returns>
        public static IEnumerable<DivideGroupView> QueryDivideGroups(Guid online) {
            var reposity = Factory.CreateOnLineCustomerRepository();
            return reposity.QueryDivideGroups(online);
        }
        /// <summary>
        /// 查询成员管理信息
        /// </summary>
        /// <param name="member">成员管理Id</param>
        /// <returns>返回单个成员管理信息</returns>
        public static MemberManage QueryMember(Guid member) {
            var reposity = Factory.CreateOnLineCustomerRepository();
            return reposity.QueryMember(member);
        }
        /// <summary>
        /// 查询成员管理信息列表
        /// </summary>
        /// <param name="divideGroup">分组信息Id</param>
        /// <returns>返回成员管理信息列表</returns>
        public static IEnumerable<MemberManage> QueryMembers(Guid divideGroup) {
            var reposity = Factory.CreateOnLineCustomerRepository();
            return reposity.QueryMembers(divideGroup);
        }
        #endregion
        #region"保存"
        /// <summary>
        /// 保存平台发布的在线服务
        /// </summary>
        /// <param name="view">在线服务信息</param>
        /// <param name="operatorAccount">操作员帐号</param>
        /// <param name="publishRoles">发布角色</param>
        public static void SavePlatForm(Guid company, OnLineCustomerView view, string operatorAccount, PublishRoles publishRoles)
        {
            var reposity = Factory.CreateOnLineCustomerRepository();
            reposity.SaveOnLine(company, publishRoles, view);
            // 记录日志
            string content = string.Format("标题:{0},内容:{1},公司Id{2},发布角色{3}", view.Title, view.Content, company, OperatorRole.Platform.GetDescription());
            saveAddLog("在线客服", content, OperatorRole.Platform, company.ToString(), operatorAccount);
        }
        /// <summary>
        /// 保存OEM版本发布的在线服务
        /// </summary>
        /// <param name="company">公司Id</param>
        /// <param name="role">平台的角色</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void SaveOEM(Guid company, OnLineCustomerView view, string operatorAccount) {
            var reposity = Factory.CreateOnLineCustomerRepository();
            reposity.SaveOnLine(company, PublishRoles.OEM, view);
            // 记录日志
            string content = string.Format("标题:{0},内容:{1},公司Id{2},发布角色{3}", view.Title, view.Content, company, OperatorRole.Provider.GetDescription());
            saveAddLog("在线客服", content, OperatorRole.Provider, company.ToString(), operatorAccount);
        }
        #endregion
        #region"新增"
        /// <summary>
        /// 插入分组信息
        /// </summary>
        /// <param name="online">在线服务Id</param>
        /// <param name="view">分组信息</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void InsertDivideGroup(Guid online, DivideGroupView view, PublishRoles role, string operatorAccount) {
            var reposity = Factory.CreateOnLineCustomerRepository();
            reposity.InsertDivideGroup(online, view);
            // 记录日志
            OperatorRole operatorRole = getOperatorRole(role);
            string content = string.Format("在线客服Id{0},分组名称{1},分组描述{2},分组排序{3}", online, view.Name, view.Description, view.SortLevel);
            saveAddLog("分组信息", content, operatorRole, view.Id.ToString(), operatorAccount);
        }
        /// <summary>
        /// 插入成员管理
        /// </summary>
        /// <param name="divideGroup">分组信息Id</param>
        /// <param name="view">成员管理信息</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void InsertMember(Guid divideGroup, MemberView view, PublishRoles role, string operatorAccount) {
            var model = new MemberManage(view.Id);
            model.Remark = view.Remark;
            model.QQ = view.QQ;
            model.SortLevel = view.SortLevel;
            var reposity = Factory.CreateOnLineCustomerRepository();
            reposity.InsertMember(divideGroup, model);
            // 记录日志
            OperatorRole operatorRole = getOperatorRole(role);
            string content = string.Format("分组Id:{0},成员说明:{1},成员QQ:{2}", divideGroup, view.Remark, view.QQ.Join(","));
            saveAddLog("成员信息", content, operatorRole, view.Id.ToString(), operatorAccount);
        }
        #endregion
        #region"修改"
        /// <summary>
        /// 更新分组信息
        /// </summary>
        /// <param name="divideGroup">分组信息Id</param>
        /// <param name="view">分组信息</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void UpdateDivideGroup(DivideGroupView view, PublishRoles role, string operatorAccount) {
            var oldView = OnLineCustomerService.QueryDivideGroup(view.Id);
            var reposity = Factory.CreateOnLineCustomerRepository();
            reposity.UpdateDivideGroup(view);
            // 记录日志
            OperatorRole operatorRole = getOperatorRole(role);
            string originalContent = string.Format("分组名称:{0},分组描述:{1},分组排序:{2}", oldView.Name, oldView.Description, oldView.SortLevel);
            string newContent = string.Format("分组名称:{0},分组描述:{1},分组排序:{2}", view.Name, view.Description, view.SortLevel);
            saveUpdateLog("分组信息", originalContent, newContent, operatorRole, view.Id.ToString(), operatorAccount);
        }
        /// <summary>
        /// 更新成员管理信息
        /// </summary>
        /// <param name="member">成员管理Id</param>
        /// <param name="view">成员管理Id</param>
        /// <param name="operatorAccount"></param>
        public static void UpdateMember(MemberView view, PublishRoles role, string operatorAccount) {
            var member = OnLineCustomerService.QueryMember(view.Id);
            var model = new MemberManage(view.Id);
            model.QQ = view.QQ;
            model.Remark = view.Remark;
            model.SortLevel = view.SortLevel;
            var reposity = Factory.CreateOnLineCustomerRepository();
            reposity.UpdateMember(model);
            // 记录日志
            OperatorRole operatorRole = getOperatorRole(role);
            string originalContent = string.Format("成员说明:{0},成员QQ:{1}", member.Remark, member.QQ.Join(","));
            string newContent = string.Format("成员说明:{0},成员QQ:{1}", view.Remark, view.QQ.Join(","));
            saveUpdateLog("成员信息", originalContent, newContent, operatorRole, view.Id.ToString(), operatorAccount);
        }
        #endregion
        #region"删除"
        /// <summary>
        /// 删除分组信息
        /// </summary>
        /// <param name="divideGrop">分组Id</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void DeleteDivideGroup(Guid divideGrop, PublishRoles role, string operatorAccount) {
            var view = OnLineCustomerService.QueryDivideGroup(divideGrop);
            var reposity = Factory.CreateOnLineCustomerRepository();
            reposity.DeleteDivideGroup(divideGrop);
            // 记录日志
            OperatorRole operatorRole = getOperatorRole(role);
            string content = string.Format("分组名称:{0},分组描述:{1},分组排序:{2}", view.Name, view.Description, view.SortLevel);
            saveDeleteLog("分组信息", content, operatorRole, divideGrop.ToString(), operatorAccount);
        }
        /// <summary>
        /// 删除成员管理信息
        /// </summary>
        /// <param name="member">成员管理Id</param>
        /// <param name="operatorAccount">操作员帐号</param>
        public static void DeleteMember(Guid member, PublishRoles role, string operatorAccount) {
            var view = OnLineCustomerService.QueryMember(member);
            var reposity = Factory.CreateOnLineCustomerRepository();
            reposity.DeleteMember(member);
            // 记录日志
            OperatorRole operatorRole = getOperatorRole(role);
            string content = string.Format("成员说明:{0},成员QQ:{1}", view.Remark, view.QQ.Join(","));
            saveDeleteLog("成员管理", content, operatorRole, member.ToString(), operatorAccount);
        }
        #endregion
        #region "日志"
        static void saveAddLog(string itemName, string content, OperatorRole role, string key, string account) {
            saveLog(OperationType.Insert, "添加" + itemName + "。" + content, role, key, account);
        }
        static void saveUpdateLog(string itemName, string originalContent, string newContent, OperatorRole role, string key, string account) {
            saveLog(OperationType.Update, string.Format("修改{0}。由 {1} 修改为 {2}", itemName, originalContent, newContent), role, key, account);
        }
        static void saveDeleteLog(string itemName, string content, OperatorRole role, string key, string account) {
            saveLog(OperationType.Delete, "删除" + itemName + "。" + content, role, key, account);
        }
        static void saveLog(OperationType operationType, string content, OperatorRole role, string key, string account) {
            var log = new Log.Domain.OperationLog(OperationModule.系统设置, operationType, account, role, key, content);
            try {
                LogService.SaveOperationLog(log);
            } catch { }
        }
        static OperatorRole getOperatorRole(PublishRoles role) {
            switch(role) {
                case PublishRoles.OEM:
                    return OperatorRole.Provider;
                case PublishRoles.平台:
                    return OperatorRole.Platform;
                default:
                    throw new NotSupportedException(role.ToString());
            }
        }
        #endregion
    }
}