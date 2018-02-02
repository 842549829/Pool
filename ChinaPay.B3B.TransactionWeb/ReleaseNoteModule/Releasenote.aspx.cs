using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.ReleaseNote;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.ReleaseNoteModule
{
    public partial class Releasenote : UnAuthBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (BasePage.LogonCompany == null)
            {
                Response.Redirect("/Logon.aspx", true);
            }
            if (!IsPostBack)
            {
                divContext.InnerHtml = "<h2>" + BasePage.PlatformName + "更新日志</h2>";
                string str = "";
                var query = ReleaseNoteService.Query(new Pagination() { PageSize = 10 }, null, null, BasePage.LogonCompany.CompanyType,ChinaPay.B3B.Common.Enums.ReleaseNoteType.B3BVisible);
                foreach (var item in query)
                {
                    if (str == "")
                    {
                        str += "<div class='history-date-box clearfix'><h3><span class='recently'></span>" + item.UpdateTime.ToString("yyyy年MM月dd日") + "<br /></h3><div class='history-date-con'><h4 class='font-strong'>" + item.Title + "</h4><p>" + item.Context + "</p></div></div>";
                    }
                    else
                    {
                        str += "<div class='history-date-box clearfix'><h3>" + item.UpdateTime.ToString("yyyy年MM月dd日") + "<br /></h3><div class='history-date-con'><h4 class='font-strong'>" + item.Title + "</h4><p>" + item.Context + "</p></div></div>";
                    }
                }
                divContext.InnerHtml += str;
            }
        }
    }
}