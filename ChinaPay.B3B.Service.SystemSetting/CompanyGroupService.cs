using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.SystemSetting.CompanyGroup;
using ChinaPay.B3B.Service.SystemSetting.Domain;
using System.Text;
using ChinaPay.B3B.Service.SystemSetting.Repository;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.SystemSetting {
    /// <summary>
    /// 政策组服务
    /// </summary>
    public static class CompanyGroupService
    {
        #region"查询"
        /// <summary>
        /// 查询公司组列表
        /// </summary>
        /// <param name="company">当前公司Id</param>
        /// <param name="condition">查询条件</param>
        public static IEnumerable<CompanyGroupListView> QueryCompanyGroups(Guid company, CompanyGroupQueryCondition condition)
        {
            var reposity = Factory.CreateCompanyGroupRepository();
            return reposity.QueryCompanyGroups(company, condition);
        }
        /// <summary>
        /// 查询公司组
        /// </summary>
        /// <param name="companyGroup">公司组Id</param>
        public static CompanyGroupView QueryCompanyGroup(Guid companyGroup)
        {
            var reposity = Factory.CreateCompanyGroupRepository();
            return reposity.QueryCompanyGroup(companyGroup);
        }
        /// <summary>
        /// 查询公司组成员
        /// </summary>
        /// <param name="companyGroup">公司组Id</param>
        /// <param name="condition">查询条件</param>
        public static IEnumerable<MemberListView> QueryMembers(Guid companyGroup, MemberQueryCondition condition)
        {
            var reposity = Factory.CreateCompanyGroupRepository();
            return reposity.QueryMembers(companyGroup, condition);
        }
        /// <summary>
        /// 查询公司组可添加的成员
        /// </summary>
        /// <param name="company">当前公司Id</param>
        /// <param name="condition">查询条件</param>
        public static IEnumerable<MemberListView> QueryCandidateMembers(Guid company, MemberQueryCondition condition)
        {
            var reposity = Factory.CreateCompanyGroupRepository();
            return reposity.QueryCandidateMembers(company, condition);
        }
        #endregion
        #region"新增"
        /// <summary>
        /// 添加公司组
        /// </summary>
        /// <param name="company">当前公司Id</param>
        /// <param name="companyGroupView">公司组信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void RegisterCompanyGroup(Guid company, CompanyGroupView companyGroupView, string operatorAccount) {
            var model = new ChinaPay.B3B.Service.SystemSetting.Domain.CompanyGroup();
            model.Name = companyGroupView.Name;
            model.PurchaseMyPolicyOnly = companyGroupView.PurchaseMyPolicyOnly;
            model.Description = companyGroupView.Description;
            model.Company = company;
            if(companyGroupView.Limitations != null){
                foreach(var item in companyGroupView.Limitations){
                    var limitationItem = new PurchaseLimitation();
                    limitationItem.Airlines = item.Airlines;
                    limitationItem.DefaultRebateForNonePolicy = item.DefaultRebateForNonePolicy;
                    limitationItem.Departures = item.Departures;
                    limitationItem.PurchaseMyPolicyOnlyForNonePolicy = item.PurchaseMyPolicyOnlyForNonePolicy;
                    model.AppendLimitaion(limitationItem);
                }
            }
            model.RegisterAccount = operatorAccount;
            model.RegisterTime = DateTime.Now;
            var repository = Factory.CreateCompanyGroupRepository();
            repository.RegisterCompanyGroup(model);
            string strContent = string.Format("公司组Id:{0},组别名称:{1},组别描述:{2},是否采购其他代理政策:{3},操作者账号:{4},创建时间:{5},公司Id:{6},受限航空公司集合:",
                              model.Id, model.Name, model.Description, model.PurchaseMyPolicyOnly, model.RegisterAccount, model.RegisterTime.Date.ToString("yyyy-MM-dd HH:mm:ss"), model.Company);
            if (companyGroupView.Limitations != null)
            {
                foreach (var item in companyGroupView.Limitations)
                {
                    strContent += string.Format("受限航空公司:{0},限制出港城市:{1},未发布政策时,是否只能采购自己的政策:{2},未发布政策时,设置默认返点:{3}；", item.Airlines,
                                item.Departures, item.PurchaseMyPolicyOnlyForNonePolicy, item.DefaultRebateForNonePolicy);
                }
            } 
            saveAddLog("公司组", strContent,"公司Id:"+company.ToString()+"公司组Id:"+model.Id.ToString(), operatorAccount);
        }
        /// <summary>
        /// 添加公司组成员
        /// </summary>
        /// <param name="companyGroup">公司组Id</param>
        /// <param name="members">成员公司Id集合</param>
        public static void RegisterMembers(Guid companyGroup, IEnumerable<Guid> members, string operatorAccount)
        {
            var reposity = Factory.CreateCompanyGroupRepository();
            reposity.RegisterMembers(companyGroup, members);
            string content = "";
            foreach (var item in members)
            {
                content += string.Format("公司组Id:{0},成员公司Id{1}",companyGroup,item);
            }
            saveAddLog("公司组成员",content,companyGroup.ToString()+":"+members.Join(",",item=>item.ToString()),operatorAccount);
        }
        #endregion
        #region"修改"
        /// <summary>
        /// 修改公司组
        /// </summary>
        /// <param name="company">当前公司Id</param>
        /// <param name="companyGroup">公司组Id</param>
        /// <param name="companyGroupView">公司组信息</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void UpdateCompanyGroup(Guid company, Guid companyGroup, CompanyGroupView companyGroupView, string operatorAccount) {
            var model = new CompanyGroup(companyGroup);
            model.Company = company;
            model.Description = companyGroupView.Description;
            model.Name = companyGroupView.Name;
            model.UpdateAccount = operatorAccount;
            model.UpdateTime = DateTime.Now;
            model.PurchaseMyPolicyOnly = companyGroupView.PurchaseMyPolicyOnly;
            if (companyGroupView.Limitations != null)
            {
                foreach (var item in companyGroupView.Limitations)
                {
                    var limitationItem = new PurchaseLimitation();
                    limitationItem.Airlines = item.Airlines;
                    limitationItem.DefaultRebateForNonePolicy = item.DefaultRebateForNonePolicy;
                    limitationItem.Departures = item.Departures;
                    limitationItem.PurchaseMyPolicyOnlyForNonePolicy = item.PurchaseMyPolicyOnlyForNonePolicy;
                    model.AppendLimitaion(limitationItem);
                }
            }
            var reposity = Factory.CreateCompanyGroupRepository();
            reposity.UpdateCompanyGroup(model);
            // 记录日志
            var view = QueryCompanyGroup(companyGroup);
            string  originalContent = string.Format("公司组Id:{0},组别名称:{1},组别描述:{2},是否采购其他代理政策:{3},公司Id:{4},限制信息集合:",
                                companyGroup, view.Name, view.Description, view.PurchaseMyPolicyOnly, company);
            if (view.Limitations != null)
            {
                foreach (var item in companyGroupView.Limitations)
                {
                    originalContent += string.Format("受限航空公司{0},限制出港城市{1},未发布政策时,是否只能采购自己的政策{2},未发布政策时,设置默认返点{3};", item.Airlines,
                                item.Departures, item.PurchaseMyPolicyOnlyForNonePolicy, item.DefaultRebateForNonePolicy);
                }
            } 
            string newContent = string.Format("公司组Id:{0},组别名称:{1},组别描述:{2},是否采购其他代理政策:{3},更新操作者账号:{4},更新时间:{5},公司Id:{6},限制信息集合：",
                                model.Id, model.Name, model.Description, model.PurchaseMyPolicyOnly, model.UpdateAccount, model.UpdateTime.Value.Date.ToString("yyyy-MM-dd HH:mm:ss"), model.Company);
            if (companyGroupView.Limitations != null)
            {
                foreach (var item in companyGroupView.Limitations)
                {
                    newContent +=string.Format("受限航空公司{0},限制出港城市{1},未发布政策时,是否只能采购自己的政策{2},未发布政策时,设置默认返点{3};", item.Airlines,
                                item.Departures, item.PurchaseMyPolicyOnlyForNonePolicy, item.DefaultRebateForNonePolicy);
                }
            }
            saveUpdateLog("公司组", originalContent, newContent,"公司Id:"+company.ToString()+"公司组Id:"+ companyGroup.ToString(), operatorAccount);
        }
        #endregion
        #region"删除"
        /// <summary>
        /// 删除公司组
        /// </summary>
        /// <param name="company">当前公司Id</param>
        /// <param name="companyGroup">公司组Id</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DeleteCompanyGroup(Guid company, Guid companyGroup, string operatorAccount) {
            var reposity = Factory.CreateCompanyGroupRepository();
            reposity.DeleteCompanyGroup(company, companyGroup);
            // 记录日志
            var companyGroupView = QueryCompanyGroup(companyGroup);
            string content = string.Format("公司组Id:{0},公司Id:{1},名称:{2},描述:{3},是否只能采购自己的政策:{4},限制信息集合:", companyGroup, company,
                                              companyGroupView.Name,companyGroupView.Description,companyGroupView.PurchaseMyPolicyOnly);
            if (companyGroupView.Limitations != null)
            {
                foreach (var item in companyGroupView.Limitations)
                {
                    content += string.Format("受限航空公司{0},限制出港城市{1},未发布政策时,是否只能采购自己的政策{2},未发布政策时,设置默认返点{3}", item.Airlines,
                                item.Departures, item.PurchaseMyPolicyOnlyForNonePolicy, item.DefaultRebateForNonePolicy);
                }
            }
            saveDeleteLog("公司组", content, companyGroup.ToString(), operatorAccount);
        }
        /// <summary>
        /// 删除公司组
        /// </summary>
        /// <param name="company">当前公司Id</param>
        /// <param name="companyGroups">公司组集合</param>
        /// <param name="operatorAccount">操作员账号</param>
        public static void DeleteCompanyGroups(Guid company, IEnumerable<Guid> companyGroups, string operatorAccount) {
            var reposity = Factory.CreateCompanyGroupRepository();
            reposity.DeleteCompanyGroups(company, companyGroups);
            // 记录日志
            string strContent = "";
            foreach (var item in companyGroups)
            {
                var view = CompanyGroupService.QueryCompanyGroup(item);
                strContent = string.Format("公司组Id:{0},公司Id:{1},名称:{2},描述:{3},是否只能采购自己的政策:{4},限制信息集合:", item, company,
                                              view.Name, view.Description, view.PurchaseMyPolicyOnly);
                if (view.Limitations != null)
                {
                    foreach (var item1 in view.Limitations)
                    {
                        strContent += string.Format("受限航空公司{0},限制出港城市{1},未发布政策时,是否只能采购自己的政策{2},未发布政策时,设置默认返点{3}", item1.Airlines,
                                    item1.Departures, item1.PurchaseMyPolicyOnlyForNonePolicy, item1.DefaultRebateForNonePolicy);
                    }
                }
            }
            saveDeleteLog("公司组",strContent,company.ToString()+":"+companyGroups.Join(",",item=>item.ToString()),operatorAccount);
        }
        /// <summary>
        /// 删除公司组成员
        /// </summary>
        /// <param name="companyGroup">公司组Id</param>
        /// <param name="members">成员公司Id集合</param>
        public static void DeleteMembers(Guid companyGroup, IEnumerable<Guid> members, string operatorAccount) {
            var reposity = Factory.CreateCompanyGroupRepository();
            reposity.DeleteMembers(companyGroup, members);
            // 记录日志
            string content = string.Format("公司组Id:{0},公司成员Id:{1}",companyGroup.ToString(),members.Join(",",item=>item.ToString()));
            saveDeleteLog("公司组成员",content ,companyGroup.ToString()+","+members.Join(",",item=>item.ToString()),operatorAccount);
        }
        #endregion
        #region "日志"
        static void saveAddLog(string itemName, string content, string key, string account)
        {
            saveLog(OperationType.Insert, "添加" + itemName + ":" + content, key, account);
        }
        static void saveUpdateLog(string itemName, string originalContent, string newContent, string key, string account)
        {
            saveLog(OperationType.Update, string.Format("修改{0}:由 {1} 修改为 {2}", itemName, originalContent, newContent), key, account);
        }
        static void saveDeleteLog(string itemName, string content, string key, string account)
        {
            saveLog(OperationType.Delete, "删除" + itemName + ":" + content, key, account);
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
