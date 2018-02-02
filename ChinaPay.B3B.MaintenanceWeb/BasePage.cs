using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ChinaPay.B3B.MaintenanceWeb.UserControl;

namespace ChinaPay.B3B.MaintenanceWeb {
    public class BasePage : Page {
        internal const string EmployeeSessionKey = "CurrentEmployee";
        internal const string CompanySessionKey = "CurrentCompany";
        internal const string PermissionSessionKey = "CurrentPermission";
        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        protected DataTransferObject.Organization.EmployeeDetailInfo CurrentUser {
            get {
                return BasePage.LogonUser;
            }
        }
        protected DataTransferObject.Organization.CompanyDetailInfo CurrentCompany {
            get {
                return BasePage.LogonCompany;
            }
        }
        protected static Service.Permission.Domain.PermissionCollection Permissions {
            get {
                if(HttpContext.Current.Session == null || HttpContext.Current.Session[PermissionSessionKey] == null)
                    return null;
                return HttpContext.Current.Session[PermissionSessionKey] as ChinaPay.B3B.Service.Permission.Domain.PermissionCollection;
            }
        }
        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        public static DataTransferObject.Organization.EmployeeDetailInfo LogonUser {
            get {
                if(HttpContext.Current.Session == null || HttpContext.Current.Session[EmployeeSessionKey] == null)
                    return null;
                return HttpContext.Current.Session[EmployeeSessionKey] as DataTransferObject.Organization.EmployeeDetailInfo;
            }
        }
        public static DataTransferObject.Organization.CompanyDetailInfo LogonCompany {
            get {
                if(HttpContext.Current.Session == null || HttpContext.Current.Session[CompanySessionKey] == null)
                    return null;
                return HttpContext.Current.Session[CompanySessionKey] as ChinaPay.B3B.DataTransferObject.Organization.CompanyDetailInfo;
            }
        }
        protected override void OnInit(EventArgs e) {
            if (System.Web.HttpContext.Current != null)
            {
                if (this.CurrentUser == null || !this.Context.User.Identity.IsAuthenticated)
                {
                    RegisterScript("window.top.location='" + System.Web.Security.FormsAuthentication.LoginUrl + "';", true);
                }
                if (!HasPermission(Request.Url.AbsolutePath))
                {
                    RegisterScript("window.top.location='" + ResolveUrl("~/StaticHtml/NoAccess.htm") + "';", true);
                }
                base.OnInit(e);
            }
        }
        protected override void OnError(EventArgs e) {
            var error = Server.GetLastError();
            Service.LogService.SaveExceptionLog(error, "系统错误");
            Server.ClearError();
            if(error is HttpRequestValidationException) {
                RegisterScript(this, "alert('请输入合法的字符串');window.location.href='" + Request.Url.PathAndQuery + "';", true);
            } else {
                Response.Redirect(ResolveUrl("~/StaticHtml/ErrorPage.htm"), true);
            }
            base.OnError(e);
        }
        /// <summary>
        /// 向当前页面发送提示信息
        /// </summary>
        /// <param name="message">信息内容</param>
        protected void ShowMessage(string message) {
            BasePage.ShowMessage(this, message);
        }
        /// <summary>
        /// 向当前页面发送错误信息
        /// </summary>
        protected void ShowExceptionMessage(Exception ex, string operation) {
            if(ex != null) {
                if(ex is System.Data.Common.DbException) {
                    Service.LogService.SaveExceptionLog(ex, "数据库错误");
                    ShowMessage(operation + "失败");
                } else if(ex is Core.CustomException) {
                    ShowMessage(ex.Message);
                } else {
                    ShowMessage(string.Format("{0}失败.{1}失败原因:{2}", operation, Environment.NewLine, ex.Message));
                }
            }
        }
        /// <summary>
        /// 向当前页面注册脚本
        /// </summary>
        /// <param name="script">脚本内容</param>
        protected void RegisterScript(string script) {
            this.RegisterScript(script, false);
        }
        /// <summary>
        /// 向当前页面注册脚本
        /// 该方法不能放在try{ ... }catch{ ... }语句块中
        /// </summary>
        /// <param name="script">脚本内容</param>
        /// <param name="stopResponse">是否注册完脚本后，停止输出</param>
        protected void RegisterScript(string script, bool stopResponse) {
            BasePage.RegisterScript(this, script, stopResponse);
        }
        /// <summary>
        /// 向页面发送提示信息
        /// </summary>
        /// <param name="page">目标页面</param>
        /// <param name="message">信息内容</param>
        public static void ShowMessage(Page page, string message) {
            RegisterScript(page, string.Format("alert('{0}');", FormatMessage(message)), false);
        }
        /// <summary>
        /// 向页面注册脚本
        /// </summary>
        /// <param name="page">目标页面</param>
        /// <param name="script">脚本内容</param>
        public static void RegisterScript(Page page, string script) {
            RegisterScript(page, script, false);
        }
        /// <summary>
        /// 向页面注册脚本
        /// 该方法不能放在try{ ... }catch{ ... }语句块中
        /// </summary>
        /// <param name="page">目标页面</param>
        /// <param name="script">脚本内容</param>
        /// <param name="stopResponse">是否注册完脚本后，停止输出</param>
        public static void RegisterScript(Page page, string script, bool stopResponse) {
            if(stopResponse) {
                page.Response.Write(string.Format("<script type='text/javascript'>{0}</script>", script));
                page.Response.End();
            } else {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), page.UniqueID, script, true);
            }
        }
        private static string FormatMessage(string message) {
            return message.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "\\r").Replace("\n", "\\n");
        }
        public static bool HasPermission(string target)
        {
            if(target == "/Default.aspx" || target == "/Index.aspx") {
                return true;
            } else {
                return Permissions.HasPermission(target);
            }
        }

        internal bool IsLoacCondition { get; set; }
        internal void LoadCondition(string pageName) {
            if (Request.QueryString["Search"] != "Back") return;
            this.IsLoacCondition = true;
            IList<ConditionItem> conditions = new List<ConditionItem>();
            HttpCookie conditionCookie = Request.Cookies[pageName];
            if (conditionCookie != null && conditionCookie.HasKeys) {
                foreach (string id in conditionCookie.Values.AllKeys)
                {
                    if (id != "undefined") conditions.Add(new ConditionItem(id,Server.UrlDecode(conditionCookie.Values[id])));
                }
            }
            foreach (ConditionItem item in conditions)
            {
                switch (item.ConditionType)
                {
                    case 1:
                        TextBox controlText = Page.FindControl(item.ControlId) as TextBox;
                        if (controlText == null) continue;
                        controlText.Text = item.ConditionValue;
                        break;
                    case 2:
                        DropDownList controlDrop = Page.FindControl(item.ControlId) as DropDownList;
                        if (controlDrop == null) continue;
                        ListItem selectedItem = controlDrop.Items.FindByText(item.ConditionValue);
                        if (selectedItem != null)
                        {
                            controlDrop.ClearSelection();
                            selectedItem.Selected = true;
                        }
                        break;
                    case 3:
                        Pager controlPager = Page.FindControl("pagerl") as Pager;
                        if (controlPager == null) continue;
                        controlPager.CurrentPageIndex = int.Parse(item.ConditionValue);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}