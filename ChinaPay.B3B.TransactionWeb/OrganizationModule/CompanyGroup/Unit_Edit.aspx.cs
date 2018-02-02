using System;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CompanyGroup
{
    public partial class Unit_Edit : BasePage
    {
        protected bool IsAdd;

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            string groupId = Request.QueryString["Id"];
            var workSetting = CompanyService.GetWorkingSetting(this.CurrentCompany.CompanyId);
            if (workSetting == null)
            {
                RegisterScript("alert('还未设置公司工作信息，不能访问本页面！请设置公司工作信息。');window.location.href='/Index.aspx';", true);
                return;
            }
            if (string.IsNullOrWhiteSpace(workSetting.AirlineForDefault))
            {
                RegisterScript("alert('还未设置公司工作信息的默认返佣，不能访问本页面！请设置公司工作信息的默认返佣。');window.location.href='/Index.aspx';", true);
                return;
            }
            hidCompanyGroupId.Value = CurrentCompany.CompanyId.ToString();
            int str = CurrentCompany.CompanyId.ToString().Length;
            IsAdd = groupId == null;
            if (!IsPostBack && groupId != null)
            {
                hidId.Value = groupId;
            }
            SetPageNoCache();
        }


    }
}