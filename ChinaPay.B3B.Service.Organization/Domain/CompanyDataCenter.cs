using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    public class CompanyDataCenter
    {
        private static CompanyDataCenter _instance;
        private static object _locker = new object();
        private ChinaPay.Data.Cache<Dictionary<Guid, DataTransferObject.Organization.CompanyListInfo>> _validReceiveCompanyCache;
        public static CompanyDataCenter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new CompanyDataCenter();
                        }
                    }
                }
                return _instance;
            }
        }
        private CompanyDataCenter()
        {
            // 有效收款账号，过期时间为半个小时
            _validReceiveCompanyCache = new ChinaPay.Data.Cache<Dictionary<Guid, DataTransferObject.Organization.CompanyListInfo>>
            {
                Timeout = 1800
            };
        }/// <summary>
        /// 得到所有公司及管理员账号
        /// </summary>
        public Dictionary<Guid, DataTransferObject.Organization.CompanyListInfo> GetCompanyInfo()
        {
            var result = _validReceiveCompanyCache.Value;
            if (result == null)
            {
                result = CompanyService.QueryCompany().ToDictionary(item => item.CompanyId);
                _validReceiveCompanyCache.Value = result;
            }
            return result;
        }
    }
}
