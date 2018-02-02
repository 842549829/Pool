using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Announce;
using ChinaPay.B3B.Service.Announce.Repository;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.Announce
{
  public static class AnnounceService
  {
      #region"查询"
      /// <summary>
      /// 查找平台Id
      /// </summary>
      /// <returns></returns>
      public static Guid QueryPlatForm()
      {
          var reposity = Factory.CreateAnnounceReposity();
          return reposity.QueryPlatForm();
      }
      /// <summary>
      /// 得到平台发布的所有紧急公告的Id
      /// </summary>
      /// <returns></returns>
      public static IEnumerable<Guid> QueryEmergencyIdsByPlatform()
      {
          var reposity = Factory.CreateAnnounceReposity();
          return reposity.QueryEmergencyIds();
      }

      public static IEnumerable<Guid> QueryEmergencyIdsByOem(Guid companyId,bool domainIsOem,bool companyIsOem)
      {
          var reposity = Factory.CreateAnnounceReposity();
          return reposity.QueryEmergencyIdsByOem(companyId,domainIsOem,companyIsOem);
      }
        /// <summary>
        /// 查询公告信息
        /// </summary>
        /// <param name="Id">公告信息Id</param>
        /// <returns></returns>
        public static AnnounceView Query(Guid id)
        {
            var repository = Factory.CreateAnnounceReposity();
            return repository.QueryAnnounce(id);
        }
        /// <summary>
        /// 公司查询公告信息列表(分页)
        /// </summary>
        /// <param name="company">公司Id</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static IEnumerable<AnnounceListView> Query(Guid company, AnnounceQueryCondition condition,Pagination pagination)
        {
            var repository = Factory.CreateAnnounceReposity();
            return repository.QueryAnnounceList(company, condition, pagination);
        }
        /// <summary>
        /// 公司查询公告信息列表
        /// </summary>
        /// <param name="company">公司Id</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static IEnumerable<AnnounceListView> Query(Guid company, AnnounceQueryCondition condition)
        {
            var repository = Factory.CreateAnnounceReposity();
            return repository.QueryAnnounceList(company, condition);
        }
        /// <summary>
        /// 用户查询公告的结果(分页)
        /// </summary>
        /// <param name="company">公司Id</param>
        /// <returns>公司信息列表</returns>
        public static IEnumerable<AnnounceListView> UserQuery(Guid company, bool domainIsOem, bool companyIsOem, Pagination pagination)
        {
            var repository = Factory.CreateAnnounceReposity();
            return repository.Query(company,domainIsOem,companyIsOem,pagination);
        }
      /// <summary>
      /// 查询审核状态
      /// </summary>
      /// <param name="id">公告Id</param>
      /// <returns></returns>
        public static AduiteStatus QueryAduiteStatus(Guid id)
        {
            var repository = Factory.CreateAnnounceReposity();
            return repository.QueryAduiteStatus(id);
        }
      #endregion
      #region"新增"
        /// <summary>
      /// 平台添加公告
      /// </summary>
      /// <param name="company">公司Id</param>
      /// <param name="role">发布的角色</param>
      /// <param name="view">公告信息</param>
      /// <param name="operatorAccount">操作员帐号</param>
      public static void InsertPlatform(Guid company, AnnounceView view, string operatorAccount)
      {
          var model = new Domain.Announce();
          model.Company = company;
          model.AduiteStatus = AduiteStatus.UnAudit;
          model.PublishRole = PublishRole.平台;
          model.PublishTime = view.PublishTime;
          model.PublishAccount = operatorAccount;
          model.Title = view.Title;
          model.Content = view.Content;
          model.AnnounceType = view.AnnounceType;
          model.AnnunceScope = view.AnnounceScope;
          var repository = Factory.CreateAnnounceReposity();
          repository.Insert(model);
          // 记录日志
          string content =string.Format("公告Id:{0},标题:{1},公告类型:{2},内容:{3},审核状态:{4},发布时间:{5},公司Id:{6}",
                                         model.Id,view.Title,view.AnnounceType,view.Content,model.AduiteStatus,view.PublishTime.Date.ToString("yyyy-MM-dd HH:mm:ss"),company);
          saveAddLog("公告",content, OperatorRole.Platform,company.ToString()+","+model.Id.ToString(),operatorAccount);
      }
      /// <summary>
      /// OEM版本新增公告
      /// </summary>
      /// <param name="company">公司Id</param>
      /// <param name="view">公告信息</param>
      /// <param name="operatorAccount">操作员帐号</param>
      public static void InsertOEM(Guid company, AnnounceView view, string operatorAccount)
      {
          var model = new Domain.Announce();
          model.Company = company;
          model.AduiteStatus = AduiteStatus.UnAudit;
          model.PublishRole =  PublishRole.用户;
          model.PublishTime = view.PublishTime;
          model.PublishAccount = operatorAccount;
          model.Title = view.Title;
          model.Content = view.Content;
          model.AnnounceType = view.AnnounceType;
          model.AnnunceScope = view.AnnounceScope;
          var repository = Factory.CreateAnnounceReposity();
          repository.Insert(model);
          // 记录日志
          string content = string.Format("公告Id:{0},标题:{1},公告类型:{2},内容:{3},发布时间:{4},公司Id:{5}", model.Id, view.Title, view.AnnounceType, view.Content, view.PublishTime.Date.ToString("yyyy-MM-dd HH:mm:ss"), company);
          saveAddLog("公告", content, OperatorRole.Provider, company.ToString() + "," + model.Id.ToString(), operatorAccount);
      }
        #endregion
      #region"修改"
      /// <summary>
      /// 修改公告信息
      /// </summary>
      /// <param name="Id">公告信息Id</param>
      /// <param name="company">公司Id</param>
      /// <param name="role">发布角色</param>
      /// <param name="status">审核状态</param>
      /// <param name="view">公告信息</param>
      /// <param name="operatorAccount">操作员帐号</param>
      public static void Update(Guid id, AnnounceView view, PublishRole role,string operatorAccount)
      {
          var announceModel = AnnounceService.Query(id);
          if (announceModel == null) throw new ArgumentException("公告信息不存在");
          var model = new Domain.Announce(id);
          model.AnnounceType = view.AnnounceType;
          model.Content = view.Content;
          model.Title = view.Title;
          model.PublishRole = role;
          model.AnnunceScope = view.AnnounceScope;
          var repository = Factory.CreateAnnounceReposity();
          repository.Update(model);
          // 记录日志
          OperatorRole operatorRole = getOperatorRole(role);
          string originalContent = string.Format("公告Id:{0},公告标题:{1},公告类型:{2},公告内容:{3}",id,announceModel.Title,announceModel.AnnounceType,announceModel.Content);
          string newContent = string.Format("公告Id:{0},公告标题:{1},公告类型:{2},公告内容:{3}",id,view.Title,view.AnnounceType,view.Content);
          saveUpdateLog("公告内容", originalContent, newContent, operatorRole, id.ToString(), operatorAccount);
      }
      /// <summary>
      /// 审核状态
      /// </summary>
      /// <param name="Id">公告信息Id</param>
      /// <param name="status">审核状态</param>
      /// <param name="operatorAccount">操作员帐号</param>
      public static void UpdateAduiteStatus(Guid id, AduiteStatus status,PublishRole role, string operatorAccount)
      {
          var announceModel = AnnounceService.Query(id);
          if (announceModel == null) throw new ArgumentException("公告信息不存在");
          var model = AnnounceService.QueryAduiteStatus(id);
          var repository = Factory.CreateAnnounceReposity();
          repository.UpdateStatus(id, status);
          // 记录日志
          var view = AnnounceService.Query(id);
          string originalContent = string.Format("公告Id:{0},标题:{1},公告类型:{2},内容:{3},审核状态:{4}",id,view.Title,view.AnnounceType,view.Content,model);
          string newContent = string.Format("公告Id:{0},标题:{1},公告类型:{2},内容:{3},审核状态:{1}", id, view.Title, view.AnnounceType, view.Content,status);
          OperatorRole operatorRole = getOperatorRole(role);
          saveUpdateLog("审核状态", originalContent, newContent, operatorRole, id.ToString(), operatorAccount);
      }

      /// <summary>
      /// 批量更改状态
      /// </summary>
      /// <param name="ids"></param>
      /// <param name="status"></param>
      /// <param name="role"></param>
      /// <param name="operatorAccount"></param>
      public static void UpdateAuditStatuses(IEnumerable<Guid> ids, AduiteStatus status, PublishRole role, string operatorAccount)
      {
          string content = "";
          foreach (var id in ids)
          {
              var model = AnnounceService.Query(id);
              if (model == null) throw new ArgumentException("公告信息不存在");
              var orginalStatus = QueryAduiteStatus(id);
              content += string.Format("公告Id:{0},标题:{1},内容:{2},公告类型:{3},发布时间:{4},审核状态:{5};", id, model.Title, model.Content, model.AnnounceType, model.PublishTime.Date.ToString("yyyy-MM-dd HH:mm:ss"), orginalStatus);
          }
          var repository = Factory.CreateAnnounceReposity();
          repository.UpdateStatuses(ids, status);
          // 记录日志
          string newContent ="";
          foreach (var id in ids)
          {
              var model = AnnounceService.Query(id);
              newContent += string.Format("公告Id:{0},标题:{1},内容:{2},公告类型:{3},发布时间:{4},审核状态:{5};", id, model.Title, model.Content, model.AnnounceType, model.PublishTime.Date.ToString("yyyy-MM-dd HH:mm:ss"), status);
          }
          OperatorRole operatorRole = getOperatorRole(role);
          saveUpdateLog("审核状态", content, newContent, operatorRole, ids.Join(",", item => item.ToString()), operatorAccount);
      }
      #endregion
      #region"删除"
      /// <summary>
      /// 删除公告信息
      /// </summary>
      /// <param name="Ids">公告信息Ids</param>
      /// <param name="operatorAccount">操作员帐号</param>
      public static void Delete(IEnumerable<Guid> ids, PublishRole role,string operatorAccount)
      {
          string content = "";
          foreach (var id in ids)
          {
              var model = AnnounceService.Query(id);
              content += string.Format("标题:{0},内容:{1},公告类型:{2},发布时间:{3};", model.Title, model.Content, model.AnnounceType, model.PublishTime.Date.ToString("yyyy-MM-dd HH:mm:ss"));
          }
          var repository = Factory.CreateAnnounceReposity();
          repository.Delete(ids);
          // 记录日志
          OperatorRole operatorRole = getOperatorRole(role);
          saveDeleteLog("公告",content,operatorRole,ids.Join(",",item=>item.ToString()),operatorAccount);
      }
      #endregion
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
          var log = new Log.Domain.OperationLog(OperationModule.公告管理, operationType, account, role, key, content);
          try
          {
              LogService.SaveOperationLog(log);
          }
          catch { }
      }
      static OperatorRole getOperatorRole(PublishRole role)
      {
          switch (role)
          {
              case PublishRole.平台:
                  return OperatorRole.Platform;
              case PublishRole.用户:
                  return OperatorRole.Provider;
              default:
                  throw new NotSupportedException(role.ToString());
          }
      }
      #endregion
    }
}
