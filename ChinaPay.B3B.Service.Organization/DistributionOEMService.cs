using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.Core;
using System.Transactions;

namespace ChinaPay.B3B.Service.Organization
{
    public static class DistributionOEMService
    {
        /// <summary>
        /// 新增授权详情
        /// </summary>
        /// <param name="oem">oem信息</param>
        public static void RegisterDistributionOEM(Domain.DistributionOEM oem,bool isOem,string abbreviateName)
        {
            if (isOem)
                throw new InvalidOperationException("该账号已是OEM");
            if (CheckExsistDomainName(oem.DomainName))
                 throw new InvalidOperationException("系统中已存在指定的域名。");
            if (CheckInitiatorIsOem(oem.CompanyId))
                throw new InvalidOperationException("OEM下的用户不能申请");
            var orginationCompany = QueryOrginationCompany();
            if (orginationCompany.Contains(oem.CompanyId))
                throw new InvalidOperationException("内部机构的用户不能申请");
            using (var tran = new TransactionScope())
            {
                CompanyService.SetOem(oem.CompanyId);
                var repository = Factory.CreateDistributionOEMRepository();
                repository.Insert(oem, abbreviateName);
                tran.Complete();
            }
            saveAddLog("OEM授权信息", string.Format("OEMId:{0}公司简称:{1},OEM名称:{2},授权域名:{3},授权到期:{4},授权保证金:{5},配置来源:{6}", 
                oem.Id,abbreviateName, oem.SiteName,oem.DomainName,oem.EffectTime,oem.AuthCashDeposit,oem.UseB3BConfig?"使用平台配置":"使用单独配置"), OperatorRole.Platform,oem.Id.ToString(), oem.OperatorAccount);
        }
        /// <summary>
        /// 修改授权详情
        /// </summary>
        /// <param name="oem">oem信息</param>
        public static void UpdateDistributionOEM(Domain.DistributionOEM oem)
        {
            var repository = Factory.CreateDistributionOEMRepository();
            repository.Update(oem);
        }
        /// <summary>
        /// 修改oem站点设置信息
        /// </summary>
        /// <param name="oem"></param>
        public static void UpdateDistributionOEMSetting(Domain.OemSetting setting, Guid oemid, string operatorAccount)
        {
            var oem = OEMService.QueryOEMById(oemid);
            var repository = Factory.CreateDistributionOEMRepository();
            using (var tran = new TransactionScope())
            {
                if (oem.Setting.Id == Guid.Empty)
                {
                    setting.Id = Guid.NewGuid();
                    repository.InsertSetting(setting, oemid);
                    repository.InsertOEMLinks(setting.Id, setting.FooterLinks);
                    repository.InsertOEMLinks(setting.Id, setting.HeaderLinks);
                }
                else
                {
                    setting.Id = oem.Setting.Id;
                    repository.UpdateSetting(setting);
                    repository.InsertOEMLinks(setting.Id, setting.FooterLinks);
                    repository.InsertOEMLinks(setting.Id, setting.HeaderLinks);
                }
                tran.Complete();
            }
            saveUpdateLog("OEM站点设置信息", oem.Setting.ToString(), setting.ToString(), OperatorRole.User, oem.Setting.Id.ToString(), oem.OperatorAccount);
        }
        ///// <summary>
        ///// 添加oem站点设置信息
        ///// </summary>
        ///// <param name="oem"></param>
        //public static void InsertDistributionOEMSetting(Domain.OemSetting oem)
        //{
        //    var repository = Factory.CreateDistributionOEMRepository();
        //    repository.InsertSetting(oem);
        //    repository.InsertOEMLinks(oem.Id, oem.FooterLinks);
        //    repository.InsertOEMLinks(oem.Id, oem.HeaderLinks);
        //}
        /// <summary>
        /// 查询oem站点设置信息
        /// </summary>
        /// <param name="oem"></param>
        public static Domain.OemSetting QueryDistributionOEMSetting(Guid id)
        {
            var repository = Factory.CreateDistributionOEMRepository();
            return repository.QuerySetting(id);
        }
        public static void UpdateOemInfo(Domain.DistributionOEM oem,string operatorAccount)
        {
            var distibutionOEM = OEMService.QueryOEMById(oem.Id);
            if (distibutionOEM.DomainName != oem.DomainName && CheckExsistDomainName(oem.DomainName))
                throw new InvalidOperationException("系统中已存在指定的域名。");
            string orginalContent = string.Format("OEMId:{0},OEM名称:{1},授权域名:{2},授权到期:{3},授权保证金:{4},配置来源:{5}",
                distibutionOEM.Id, distibutionOEM.SiteName, distibutionOEM.DomainName, distibutionOEM.EffectTime, distibutionOEM.AuthCashDeposit, distibutionOEM.UseB3BConfig ? "使用平台配置" : "使用单独配置");
            string newContent = string.Format("OEMId:{0},OEM名称:{1},授权域名:{2},授权到期:{3},授权保证金:{4},配置来源:{5}",
                oem.Id, oem.SiteName, oem.DomainName, oem.EffectTime, oem.AuthCashDeposit, oem.UseB3BConfig ? "使用平台配置" : "使用单独配置");
            var repository = Factory.CreateDistributionOEMRepository();
            repository.UpdateOemInfo(oem);
            saveUpdateLog("OEM授权信息",orginalContent,newContent,OperatorRole.Platform,oem.Id.ToString(),operatorAccount);
        }
        /// <summary>
        /// 检查域名是否存在
        /// </summary>
        /// <param name="domainName">域名</param>
        public static bool CheckExsistDomainName(string domainName)
        {
            var respository = Factory.CreateDistributionOEMRepository();
            return respository.CheckExsistDomainName(domainName);
        }
        /// <summary>
        /// 检查上级是否是OEM
        /// </summary>
        /// <param name="userNo">用户名</param>
        /// <returns></returns>
        public static bool CheckInitiatorIsOem(Guid companyId)
        {
            var repository = Factory.CreateDistributionOEMRepository();
            return repository.CheckInitiatorIsOem(companyId);
        }
        /// <summary>
        /// 查询所有组织机构公司
        /// </summary>
        public static IEnumerable<Guid> QueryOrginationCompany()
        {
            var repository = Factory.CreateDistributionOEMRepository();
            return repository.QueryOrginationCompany();
        }
        /// <summary>
        /// 查询授权列表
        /// </summary>
        public static System.Data.DataTable QueryDistributionOEMList(DistributionOEMCondition condition,Pagination pagination)
        {
            var repository = Factory.CreateDistributionOEMRepository();
            return repository.QueryDistributionOEMList(condition,pagination);
        }
        /// <summary>
        /// 查询所有是OEM的公司
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DistributionOEMView> QueryDistributionOEM()
        {
            var repository = Factory.CreateDistributionOEMRepository();
            return repository.QueryDistributionOEM();
        }
        /// <summary>
        /// 查询某个OEM下的用户
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页信息</param>
        public static IEnumerable<DistributionOEMUserListView> QueryDistributionOEMUserList(DistributionOEMUserCondition condition,Pagination pagination)
        {
            var repository = Factory.CreateDistributionOEMRepository();
            return repository.QueryDistributionOEMUserList(condition,pagination);
        }
        /// <summary>
        /// 查询某个OEM下单个用户的详细信息
        /// </summary>
        /// <param name="companyId">公司Id</param>
        public static DistribtionOEMUserCompanyDetailInfo QueryDistributionOEMUserDetailInfo(Guid companyId)
        {
            var repository = Factory.CreateDistributionOEMRepository();
            return repository.QueryDistributionOEMUserDetail(companyId);
        }
        /// <summary>
        /// 选择风格
        /// </summary>
        /// <param name="oemId"></param>
        /// <param name="styleId"></param>
        public static void ChooiceStyle(Guid oemId, Guid styleId, string operatorAccount)
        {
            var repository = Factory.CreateDistributionOEMRepository();
            repository.ChooiceStyle(oemId, styleId);
            saveAddLog("OEM选择风格", oemId + "选择" + styleId, OperatorRole.User, oemId.ToString(), operatorAccount);
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
