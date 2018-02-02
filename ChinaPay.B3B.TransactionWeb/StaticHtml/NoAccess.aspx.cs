using System;
using System.Web.UI;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.TransactionWeb.StaticHtml
{
    public partial class NoAccess : UnAuthBasePage
    {
        protected bool IsPurchaser
        {
            get
            {
                var company = Session[BasePage.CompanySessionKey] as CompanyDetailInfo;
                if (company == null)
                {
                    return false;
                }
                return company.CompanyType == CompanyType.Purchaser;
            }
        }

        protected void Page_Load(object sender, EventArgs e) { }
    }
}