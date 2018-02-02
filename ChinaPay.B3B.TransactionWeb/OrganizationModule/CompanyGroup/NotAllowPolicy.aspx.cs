using System;
using System.Text;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.CompanyGroup
{
    public partial class NotAllowPolicy : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack && Request.QueryString["Id"] != null && Request.QueryString["Id"].Length == 36)
            {
                setBackUrl();
                this.tbCompanyGroup.InnerHtml = this.GetCompanyGroup(Guid.Parse(Request.QueryString["Id"]));
            }
        }
        private string GetCompanyGroup(Guid id) {
            StringBuilder builder = new StringBuilder(100);
            CompanyGroupDetailInfo info =   CompanyService.GetCompanyGroupDetailInfo(id);
            if (info == null) return string.Empty;
            foreach (CompanyGroupLimitation item in info.Limitations)
            {
                //string defaultRebate = item.DefaultRebate == 0M ? "允许采购其他政策" : string.Format("默认返点{0}%", (item.DefaultRebate * 100M).TrimInvaidZero());
                builder.AppendFormat("<tr><td class='postion_airline'>{0}</td><td class='postion_departure'>{1}</td><td>{2}</td></tr>",
                    item.Airlines,
                    item.Departures
                  //  defaultRebate
                    );
            }                                
            return builder.ToString();
        }
        private void setBackUrl()
        {
            var returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Request.UrlReferrer.PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
    }
}