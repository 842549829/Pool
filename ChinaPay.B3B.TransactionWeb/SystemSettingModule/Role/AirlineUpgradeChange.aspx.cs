using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class AirlineUpgradeChange : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                string showType = Request.QueryString["ShowType"];
                switch (showType)
                {
                    case "Upgrade":
                        bindUpgrade();
                        break;
                    case "Check-In":
                        bindCheck_In();
                        break;
                    default:
                        bindUpgrade();
                        break;
                }
            }
        }
        private void bindCheck_In() 
        {
            string check_in = string.Empty;
            foreach (var item in FoundationService.Check_Ins)
            {
                check_in += string.Format("<tr><td>{0}</td><td>{1}</td><td><a target=\"_blank\" href='{2}'>前往值机</a></td></tr>",
                    item.AirlineName,
                    item.Remark,
                    item.OperatingHref);
            }
            tabContext.InnerHtml = "<tr><th>航空公司</th><th>备注</th><th>操作</th></tr>" + check_in;
        }
        private void bindUpgrade()
        {
            string upgradeContent = string.Empty;
            foreach (var item in FoundationService.NewRefundAndReschedulings)
            {
                if (!string.IsNullOrEmpty(item.Upgrade))
                {
                    upgradeContent += "<tr><td style='width:120px;'>"
                        + item.Airline.Name.Replace("中国", "").Replace("股份", "").Replace("有限", "").Replace("责任", "").Replace("公司", "")
                        + "&nbsp;&nbsp;&nbsp;"
                        + item.AirlineCode.Value
                        + "</td><td class='txt-l'>"
                        + item.Upgrade
                        + "</td></tr>";
                }
            }
            tabContext.InnerHtml = "<tr><th style='width:120px;'>航空公司</th><th>处理意见</th></tr>" + upgradeContent;
        }
    }
}