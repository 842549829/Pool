using System;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.MaintenanceWeb
{
    public partial class Index : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblAccount.Text = CurrentUser.UserName;
                lblYear.Text = DateTime.Today.ToString("yyyy年MM月dd日");
                string[] week = new string[7] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                lblWeek.Text = week[(int)DateTime.Now.DayOfWeek];
                bindMenus();
            }
        }
        private void bindMenus()
        {
            var menusHTML = new StringBuilder();
            foreach (var menu in Permissions.GetMenus())
            {
                if (menu.Valid && menu.Display && menu.Children.Any(item => item.Valid && item.Display))
                {
                    menusHTML.Append("<p class='menu_head'><span><img src='../images/menu_icon.png' alt=''/></span><em><a href='#this'>");
                    menusHTML.AppendFormat("<img src='../images/empty.gif' height='11' width='12' alt='' /></a></em>{0}</p>", menu.Name);
                    menusHTML.Append("<div class='menu_body'>");
                    foreach (var subMenu in menu.Children)
                    {
                        if (subMenu.Valid && subMenu.Display)
                        {
                            menusHTML.AppendFormat("<a href='{0}' target='rightFrame'>{1}</a>", subMenu.Address, subMenu.Name);
                        }
                    }
                    menusHTML.Append("</div>");
                }
            }
            this.firstpane.InnerHtml = menusHTML.ToString();
        }
    }
}
