using System;
using System.Linq;
using ChinaPay.B3B.DataTransferObject;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.Organization.Repository;
using ChinaPay.Data;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Log.Domain;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Organization
{
    ///// <summary>
    ///// OEM服务层
    ///// </summary>
    public static class OEMService
    {
        static OEMService() { RefreshService.OEMSettingChanged += FlushOEM;
                              RefreshService.OEMStyleSetChanged += FlushOEMStyle;
                              RefreshService.OEMAdded += FlushNotOEMCache;
        }

        #region Contract
        //    /// <summary>
        //    /// 查询业务服务联系方式
        //    /// </summary>
        //    /// <param name="oemId">oemId</param>
        //    /// <returns>Contract</returns>
        //    public static Contract GetContract(Guid oemId){
        //        throw new NotImplementedException();
        //    }
        /// <summary>
        /// 保存业务服务联系方式
        /// </summary>
        /// <param name="oemInfo">oemInfo</param>
        /// <param name="operators">操作员</param>
        public static void SvaeContract(OEMInfo oemInfo, string operators)
        {
            var repository = Factory.CreateDistributionOEMRepository();
            repository.SvaeOEMContract(oemInfo);
            saveUpdateLog("OEM业务服务联系方式设置",
                string.Format("企业QQ:{0},儿童机票受理传真:{1},票务综合处理电话:{2},退票电话:{3},废票电话:{4},支付相关服务电话:{5},紧急业务受理电话:{6},投诉监督电话:{7},是否允许平台联系采购:{8},是否采用B3B客服电话:{9}",
                oemInfo.Contract.EnterpriseQQ,
                oemInfo.Contract.Fax,
                oemInfo.Contract.ServicePhone,
                oemInfo.Contract.RefundPhone,
                oemInfo.Contract.ScrapPhone,
                oemInfo.Contract.PayServicePhone,
                oemInfo.Contract.EmergencyPhone,
                oemInfo.Contract.ComplainPhone,
                oemInfo.Contract.AllowPlatformContractPurchaser ? "允许" : "不允许",
                oemInfo.Contract.UseB3BServicePhone ? "采用" : "不采用"),
                oemInfo.SiteName,
                OperatorRole.User,
                operators);
        }
        #endregion

        #region  查询

        static object locker = new object();
        static Cache<string, OEMInfo> OEMCache = new Cache<string, OEMInfo>()
        {
            Timeout = 30 * 60 * 1000
        };
        static Cache<string,Guid?> NotOEMCache = new Cache<string,Guid?>()
        {
            Timeout = 30*60*1000
        };

        /// <summary>
        /// 通过域名查找OEM信息
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static OEMInfo QueryOEM(string domain)
        {
            if (NotOEMCache.GetValue(domain) == Guid.Empty) return null;
            OEMInfo result = OEMCache.GetValue(domain);
            if (result == null)
            {
                lock (locker)
                {
                    result = OEMCache.GetValue(domain);
                    if (result != null) return result;
                    var repository = Factory.CreateCompanyRepository();
                    result = repository.QueryOEM(domain);
                    if (result != null) OEMCache.Add(domain, result);
                    else NotOEMCache.Add(domain, Guid.Empty);
                }
            }
            return result;
        }

        /// <summary>
        /// 通过公司ID查找OEM信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static OEMInfo QueryOEM(Guid companyId)
        {
            if(NotOEMCache.Values.Any(i=>i==companyId)) return null;
            OEMInfo result = OEMCache.Values.FirstOrDefault(o => o.CompanyId == companyId);
            if (result != null) return result;
            lock (locker)
            {
                result = OEMCache.Values.FirstOrDefault(o => o.CompanyId == companyId);
                if (result != null) return result;
                var repository = Factory.CreateCompanyRepository();
                result = repository.QueryOEM(companyId);
                if (result != null) OEMCache.Add(result.DomainName, result);
                else NotOEMCache.Add(companyId.ToString(), companyId);
            }
            return result;
        }

        /// <summary>
        /// 通过OEMID查找OEM信息
        /// </summary>
        /// <param name="oemId"></param>
        /// <returns></returns>
        public static OEMInfo QueryOEMById(Guid oemId)
        {
            OEMInfo result = OEMCache.Values.FirstOrDefault(o => o.Id == oemId);
            if (result != null) return result;
            lock (locker)
            {
                result = OEMCache.Values.FirstOrDefault(o => o.Id == oemId);
                if(result!=null) return result;
                var repository = Factory.CreateCompanyRepository();
                result = repository.QueryOEMById(oemId);
                if (result != null) OEMCache.Add(result.DomainName, result);
            }
            return result;
        }

        public static IEnumerable<KeyValuePair<Guid, bool>> QueryOEMContractSettings(IEnumerable<Guid> oemIds)
        {
            var repository = Factory.CreateCompanyRepository();
            return repository.QueryOEMContractSettings(oemIds);
        }

        #endregion


        static void FlushOEM(Guid oemId)
        {
            OEMInfo result = OEMCache.Values.FirstOrDefault(o => o.Id == oemId);
            if (result == null) return;
            lock (locker)
            {
                OEMCache.Remove(result.DomainName);
            }
        }
        static void FlushOEMStyle(Guid styleId) {
            var toBeFlushedOEM = OEMCache.Values.Where(o => o.OEMStyle.Id == styleId).ToList();
            lock (locker)
            {
                foreach (OEMInfo oemInfo in toBeFlushedOEM)
                {
                    OEMCache.Remove(oemInfo.DomainName);
                }
            }
        }
        static void FlushNotOEMCache() {
            NotOEMCache.Refresh(new List<KeyValuePair<string, Guid?>>());
        }

        #region 日志
        private static void saveUpdateLog(string itemName, string content, string key, OperatorRole role, string account)
        {
            saveLog(OperationType.Update, string.Format("{0} 已经修改为:{1}", itemName, content), role, key, account);
        }

        private static void saveLog(OperationType operationType, string content, OperatorRole role, string key, string account)
        {
            var log = new OperationLog(OperationModule.OEM信息设置, operationType, account, role, key, content);
            try
            {
                LogService.SaveOperationLog(log);
            }
            catch { }
        }
        #endregion
    }
}
