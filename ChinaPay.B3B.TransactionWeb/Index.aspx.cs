using System;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                drawMenu();
                var redirectUrl = Request.QueryString["redirectUrl"];
                if (Request.QueryString.AllKeys.Count() > 1)
                {
                    int parameterIndex = Request.Url.OriginalString.IndexOf('=');
                    redirectUrl = Request.Url.OriginalString.Substring(parameterIndex + 1);
                }
                this.hidFrameTarget.Value = string.IsNullOrWhiteSpace(redirectUrl) ? (CurrentCompany.CompanyType == CompanyType.Purchaser) ? "/OrderModule/Purchase/OrderList.aspx?Search=Back" : "TicketDefault.aspx" : redirectUrl;

            }
        }
        private void drawMenu()
        {
            var menuHTML = new StringBuilder();
            menuHTML.Append("<ul class='menu-client'>");
            foreach (var menu in BasePage.Permissions.GetMenus())
            {
                if (menu.Valid && menu.Display && menu.Children.Any(item => item.Valid && item.Display))
                {
                    menuHTML.AppendFormat("<li><strong>{0}</strong>", menu.Name);
                    menuHTML.Append("<ul class='sub-menu' style='display:none;'>");
                    foreach (var subMenu in menu.Children)
                    {
                        if (subMenu.Valid && subMenu.Display)
                        {
                            menuHTML.AppendFormat("<li><a href='{0}' target='rightFrame'>{1}</a></li>", subMenu.Address, subMenu.Name);
                        }
                    }
                    menuHTML.Append("</ul></li>");
                }
            }
            menuHTML.Append("</ul>");
            this.divMenu.InnerHtml = menuHTML.ToString();
        }

        protected string SkingPath
        {
            get {
                if (OEM != null && OEM.OEMStyle != null)
                {
                    return OEM.OEMStyle.TemplatePath;
                }
                else
                {
                    return DefaultStyleFile;
                }
            }
            
        }
    }
}