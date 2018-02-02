using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Organization.Domain;

namespace ChinaPay.B3B.TransactionWeb.UserControlHandler
{
    /// <summary>
    /// CompanyQuery 的摘要说明
    /// </summary>
    public class CompanyQuery : BaseHandler
    {
        public object QueryCompanyInfo(string companyName, byte companyType, byte showDisable)
        {
            var com = CompanyDataCenter.Instance.GetCompanyInfo();

            return (from item in com
                    where (item.Value.UserNo.StartsWith(companyName, StringComparison.CurrentCultureIgnoreCase) || item.Value.AbbreviateName.StartsWith(companyName))
                   && (companyType == 0 || ((item.Value.CompanyType & (CompanyType)companyType) == item.Value.CompanyType))
                   && (showDisable == 0 || (item.Value.Enabled))
                   orderby item.Value.UserNo
                   select new
                   {
                       text = item.Value.UserNo + "-" + item.Value.AbbreviateName,
                       value = item.Value.CompanyId
                   }).Take(10);
        }
    }
}