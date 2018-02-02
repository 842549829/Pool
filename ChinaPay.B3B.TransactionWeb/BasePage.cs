using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund.Normal;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.TransactionWeb.OrderModule.Provide;
using ChinaPay.B3B.TransactionWeb.UserControl;

namespace ChinaPay.B3B.TransactionWeb
{
    public class BasePage : UnAuthBasePage
    {
        internal const string EmployeeSessionKey = "CurrentEmployee";
        internal const string CompanySessionKey = "CurrentCompany";
        internal const string PermissionSessionKey = "CurrentPermission";
        internal const string SuperiorOEMSessiongKey = "SuperiorOEMSessiongKey";

        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        protected DataTransferObject.Organization.EmployeeDetailInfo CurrentUser
        {
            get
            {
                return BasePage.LogonUser;
            }
        }
        protected DataTransferObject.Organization.CompanyDetailInfo CurrentCompany
        {
            get
            {
                return BasePage.LogonCompany;
            }
        }

        #region  站点OEM信息

        /// <summary>
        /// 当期服务联系方式信息
        /// </summary>
        internal static OEMContract CurrenContract
        {
            get
            {
                return IsOEM
                    && !OEM.Contract.UseB3BServicePhone
                    && OEM.Valid
                        ? OEM.Contract : OEMContract.B3BDefault;
            }
        }
        #endregion

        /// <summary>
        /// 当前登录用户
        /// </summary>
        internal static DataTransferObject.Organization.EmployeeDetailInfo LogonUser
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[EmployeeSessionKey] == null)
                    return null;
                return HttpContext.Current.Session[EmployeeSessionKey] as DataTransferObject.Organization.EmployeeDetailInfo;
            }
        }
        internal static DataTransferObject.Organization.CompanyDetailInfo LogonCompany
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[CompanySessionKey] == null)
                    return null;
                return HttpContext.Current.Session[CompanySessionKey] as DataTransferObject.Organization.CompanyDetailInfo;
            }
        }

        #region  上级OEM信息
        /// <summary>
        /// 上级OEM信息，有可能为NULL
        /// </summary>
        internal static OEMInfo SuperiorOEM
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[SuperiorOEMSessiongKey] == null)
                    return null;
                return HttpContext.Current.Session[SuperiorOEMSessiongKey] as OEMInfo;
            }
        }

        public static Guid OwnerOEMId
        {
            get
            {
                if (SuperiorOEM == null) return Guid.Empty;
                return SuperiorOEM.Id;
            }
        }
        #endregion

        internal static Service.Permission.Domain.PermissionCollection Permissions
        {
            get
            {
                if (HttpContext.Current.Session == null || HttpContext.Current.Session[PermissionSessionKey] == null)
                    return null;
                return HttpContext.Current.Session[PermissionSessionKey] as ChinaPay.B3B.Service.Permission.Domain.PermissionCollection;
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (this.CurrentUser == null || !this.Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect(System.Web.Security.FormsAuthentication.LoginUrl, true);
            }
            if (!BasePage.HasPermission(Request.Url.AbsolutePath))
            {
                Service.LogService.SaveTextLog("无权限访问地址:" + Request.Url);
                Response.Redirect(ResolveUrl("~/StaticHtml/NoAccess.aspx"), true);
            }
            AccessStatistic(Request.Url.AbsolutePath);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void OnError(EventArgs e)
        {
            var error = Server.GetLastError();
            Service.LogService.SaveExceptionLog(error, "系统错误 路径:" + Request.Url.PathAndQuery);
            Server.ClearError();
            if (error is HttpRequestValidationException)
            {
                RegisterScript(this, "alert('请输入合法的字符串');window.location.href='" + Request.Url.PathAndQuery + "';", true);
            }
            else
            {
                Response.Redirect(ResolveUrl("~/StaticHtml/ErrorPage.aspx"), true);
            }
            base.OnError(e);
        }
        /// <summary>
        /// 判断是否有访问权限
        /// </summary>
        /// <param name="target">目标地址</param>
        public static bool HasPermission(string target)
        {
            target = Regex.Replace(target, "/[%28|\\(].*?[%29|\\)]/", "/").ToUpper();
            if (target == "/DEFAULT.ASPX" || target == "/PURCHASEDEFAULT.ASPX" || target == "/INDEX.ASPX" || target == "/TICKETDEFAULT.ASPX")
            {
                return true;
            }
            else
            {
                return Permissions.HasPermission(target);
            }
        }

        /// <summary>
        /// 向当前页面发送提示信息
        /// </summary>
        /// <param name="message">信息内容</param>
        protected void ShowMessage(string message)
        {
            BasePage.ShowMessage(this, message);
        }
        /// <summary>
        /// 向当前页面发送错误信息
        /// </summary>
        protected void ShowExceptionMessage(Exception ex, string operation)
        {
            ShowExceptionMessage(this, ex, operation);
        }
        /// <summary>
        /// 向当前页面发送错误信息
        /// </summary>
        public static void ShowExceptionMessage(Page page, Exception ex, string operation)
        {
            if (ex != null)
            {
                if (ex is System.Data.Common.DbException)
                {
                    Service.LogService.SaveExceptionLog(ex);
                    ShowMessage(page, operation + "失败");
                }
                else if (ex is Core.CustomException)
                {
                    ShowMessage(page, ex.Message);
                }
                else
                {
                    ShowMessage(page, string.Format("{0}失败.{1}失败原因:{2}", operation, Environment.NewLine, ex.Message));
                }
            }
        }
        /// <summary>
        /// 向当前页面注册脚本
        /// </summary>
        /// <param name="script">脚本内容</param>
        protected void RegisterScript(string script)
        {
            this.RegisterScript(script, false);
        }
        /// <summary>
        /// 向当前页面注册脚本
        /// 该方法不能放在try{ ... }catch{ ... }语句块中
        /// </summary>
        /// <param name="script">脚本内容</param>
        /// <param name="stopResponse">是否注册完脚本后，停止输出</param>
        protected void RegisterScript(string script, bool stopResponse)
        {
            BasePage.RegisterScript(this, script, stopResponse);
        }
        /// <summary>
        /// 向页面发送提示信息
        /// </summary>
        /// <param name="page">目标页面</param>
        /// <param name="message">信息内容</param>
        public static void ShowMessage(Page page, string message)
        {
            RegisterScript(page, string.Format("alert('{0}');", FormatMessage(message)), false);
        }
        /// <summary>
        /// 向页面注册脚本
        /// </summary>
        /// <param name="page">目标页面</param>
        /// <param name="script">脚本内容</param>
        public static void RegisterScript(Page page, string script)
        {
            RegisterScript(page, script, false);
        }
        /// <summary>
        /// 向页面注册脚本
        /// 该方法不能放在try{ ... }catch{ ... }语句块中
        /// </summary>
        /// <param name="page">目标页面</param>
        /// <param name="script">脚本内容</param>
        /// <param name="stopResponse">是否注册完脚本后，停止输出</param>
        public static void RegisterScript(Page page, string script, bool stopResponse)
        {
            if (stopResponse)
            {
                page.Response.Write(string.Format("<script type='text/javascript'>{0}</script>", script));
                page.Response.End();
            }
            else
            {
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), page.UniqueID, script, true);
            }
        }

        public static void RegisterJavaScript(Page page, string script)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), page.UniqueID, script, true);
        }
        public static string FormatMessage(string message)
        {
            return message.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\r", "\\r").Replace("\n", "\\n");
        }
        public static bool Lock(decimal key, Service.Locker.LockRole lockRole, string remark, out string errorMsg)
        {
            var lockInfo = new Service.Locker.LockInfo(key.ToString())
            {
                LockRole = lockRole,
                Company = LogonCompany.CompanyId,
                CompanyName = LogonCompany.AbbreviateName,
                Account = LogonUser.UserName,
                Name = LogonUser.Name,
                Remark = remark
            };
            return Service.LockService.Lock(lockInfo, out errorMsg);
        }
        public static void ReleaseLock(decimal key)
        {
            Service.LockService.UnLock(key.ToString(), LogonUser.UserName);
        }

        /// <summary>
        /// 禁用页面缓存，防止某些操作在浏览器返回后“失效”
        /// </summary>
        protected void SetPageNoCache()
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.AppendHeader("Pragma", "No-Cache");
        }

        /// <summary>
        /// 添加Repeater没有数据时的现实内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddEmptyTemplate(object sender, EventArgs e)
        {
            var repeater = sender as Repeater;
            if (repeater == null || repeater.DataSource == null || repeater.Items.Count > 0) return;
            repeater.SetRenderMethodDelegate(delegate(HtmlTextWriter output, Control container)
            {
                output.Write("<div class=\"box\">没有符合条件的查询结果</div>"); output.EndRender();
            });

        }

        internal static NormalRefundRoleBill SearchBill(NormalRefundBill refundBill)
        {
            if (LogonCompany.CompanyType == CompanyType.Platform) return refundBill.Platform.Deduction;
            if (refundBill.Provider != null && refundBill.Provider.Owner.Id == LogonUser.Id) return refundBill.Provider;
            if (refundBill.Supplier != null && refundBill.Supplier.Owner.Id == LogonUser.Id) return refundBill.Supplier;
            if (refundBill.Purchaser != null && refundBill.Purchaser.Owner.Id == LogonUser.Id) return refundBill.Purchaser;
            return refundBill.Purchaser;
        }

        internal static OrderRole GetOrderRole(Order order)
        {
            if (LogonCompany.CompanyType == CompanyType.Platform) return OrderRole.Platform;
            if (order.Purchaser != null && order.Purchaser.CompanyId == LogonCompany.CompanyId) return OrderRole.Purchaser;
            if (order.Provider != null && order.Provider.CompanyId == LogonCompany.CompanyId) return OrderRole.Provider;
            if (order.Supplier != null && order.Supplier.CompanyId == LogonCompany.CompanyId) return OrderRole.Supplier;
            return OrderRole.OEMOwner;
        }

        internal bool IsLoacCondition
        {
            get;
            set;
        }

        internal void LoadCondition(string pageName)
        {
            if (Request.QueryString["Search"] != "Back") return;
            IsLoacCondition = true;
            List<ConditionItem> AllConditions = new List<ConditionItem>();
            HttpCookie Condition = Request.Cookies[pageName];
            if (Condition != null && Condition.HasKeys)
            {
                foreach (string id in Condition.Values.AllKeys)
                {
                    if (id != "undefined")
                        AllConditions.Add(new ConditionItem(id, Server.UrlDecode(Condition.Values[id])));
                }
            }
            foreach (ConditionItem item in AllConditions)
            {
                if (item.ConditionType == 1)
                {
                    var control = Page.FindControl(item.ControlId) as TextBox;
                    if (control == null)
                        continue;
                    control.Text = item.ConditionValue;
                }
                if (item.ConditionType == 2)
                {
                    var control = Page.FindControl(item.ControlId) as DropDownList;
                    if (control == null)
                    {
                        if (item.ControlId.IndexOf("_") != -1)
                        {
                            var newControl = Page.FindControl(item.ControlId.Substring(0, item.ControlId.IndexOf("_"))) as ChinaPay.B3B.TransactionWeb.UserControl.Airport;
                            if (newControl != null)
                            {
                                if (item.ConditionValue.IndexOf("-") != -1)
                                    newControl.Code = item.ConditionValue.Substring(0, item.ConditionValue.IndexOf("-"));
                            }
                        }
                        continue;
                    }
                    var selectedItem = control.Items.FindByText(item.ConditionValue);
                    if (selectedItem != null)
                    {
                        control.ClearSelection();
                        selectedItem.Selected = true;
                    }
                }
                if (item.ConditionType == 3)
                {
                    var control = Page.FindControl("pager") as Pager;
                    if (control != null)
                    {
                        control.CurrentPageIndex = int.Parse(item.ConditionValue);
                    }
                }
                if (item.ConditionType == 4)
                {
                    var control = Page.FindControl(item.ControlId) as HiddenField;
                    if (control != null)
                    {
                        control.Value = item.ConditionValue;
                    }
                }
            }
        }

        private void AccessStatistic(string pageName)
        {
            //if(pageName == "/Index.aspx") return;
            //var accessSessionKey = "PageAccess";
            //if(Session[accessSessionKey] == null) {
            //    Session[accessSessionKey] = new Dictionary<string, DateTime> { { pageName, DateTime.Now } };
            //} else {
            //    var accesses = Session[accessSessionKey] as Dictionary<string, DateTime>;
            //    DateTime preAccessTime;
            //    if(accesses.TryGetValue(pageName, out preAccessTime)) {
            //        var currentTime = DateTime.Now;
            //        if((currentTime - preAccessTime).TotalSeconds < 0.2) {
            //            Service.LogService.SaveTextLog(string.Format("访问频率控制:页面:{0} 上次访问时间:{1:yy-MM-dd HH:mm:ss fff} 当前时间:{2:yy-MM-dd HH:mm:ss fff}", pageName, preAccessTime, currentTime));
            //            RegisterScript(this, "alert('动作太快了');window.location.href='" + Request.Url.PathAndQuery + "';", true);
            //        } else {
            //            accesses[pageName] = DateTime.Now;
            //        }
            //    } else {
            //        accesses.Add(pageName, DateTime.Now);
            //    }
            //}
        }

    }

    public class UnAuthBasePage : Page
    {
        private readonly List<string> m_CssFiles;
        private readonly List<string> m_JsFiles;

        public UnAuthBasePage()
        {
            m_CssFiles = new List<string>();
            m_JsFiles = new List<string>();
            RegisterOEMSkins("core.css");
        }


        internal const string DefaultStyleFile = "/Styles/skins/default/";
        protected List<string> renderstyles = new List<string>();
        #region  站点信息
        /// <summary>
        /// 是否是OEM
        /// </summary>
        internal static bool IsOEM
        {
            get
            {
                return OEM != null;
            }
        }
        /// <summary>
        /// 域名
        /// </summary>
        internal static string DomainName
        {
            get
            {
                return HttpContext.Current.Request.Url.Host;
                //return "admin77q";
            }
        }
        /// <summary>
        /// 当前登录OEM信息，需要验证是是否为NULL
        /// </summary>
        internal static OEMInfo OEM
        {
            get
            {
                return OEMService.QueryOEM(DomainName);
            }
        }
        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            m_CssFiles.ForEach(f => AddStyleSheet(this, f));
            LoadStyle(this);
            LoadSietMate(this);
            this.Title = PlatformName;
        }




        #region  站点信息注册
        /// <summary>
        /// 平台名称
        /// </summary>
        internal static string PlatformName
        {
            get
            {
                return IsOEM ? OEM.SiteName : ChinaPay.B3B.Service.SystemManagement.SystemParamService.DefaultPlatformName;
            }
        }

        private void LoadSietMate(Page page)
        {
            page.Header.Controls.Add(new HtmlMeta
            {
                Content = "text/html; charset=utf-8",
                HttpEquiv = "Content-Type"
            });
            if (OEM != null)
            {
                AddMeta(page, "keywords", OEM.Setting.SiteKeyWord);
                AddMeta(page, "description", OEM.Setting.SiteDescription);
            }
            else
            {
                AddMeta(page, "description", "B3B机票交易平台是国内领先的机票行业同行购票平台，政策好、返点高、政策齐全，支持单程/往返/联程/缺口程的团队及散客票、特价票、特殊票交易，是中国机票销售平台的领导者与创新旗舰平台；是中国首家任意行程编码完美支持平台！欢迎加入B3B与我们一起协同创富！详询400-739-0838。");
                AddMeta(page, "keywords", "国内机票,B2B机票,机票平台,机票同行,特殊票,免票,散冲团,集团票,商旅卡,b3b,b3b机票,b3b机票交易,电子客票,黄牛机票,打折机票,特价机票,免费机票,卖机票,机票分销,机票高返");
            }
        }

        public static void LoadStyle(Page page)
        {
            if (IsOEM && OEM.OEMStyle != null)
                foreach (string style in OEM.OEMStyle.StylePath)
                {
                    AddStyleSheet(page, style);
                }
        }


        protected void RegisterOEMSkins(string fileName)
        {
            if (OEM != null && OEM.OEMStyle != null)
                m_CssFiles.Add(OEM.OEMStyle.TemplatePath + fileName);
            else m_CssFiles.Add(DefaultStyleFile + fileName);
        }


        #endregion

        #region  内容注册
        /// <summary>
        /// 更换样式文件路径
        /// </summary>
        /// <param name="page"></param>
        /// <param name="cssPath"></param>
        internal static void AddStyleSheet(Page page, string cssPath)
        {
            var link = new HtmlLink
            {
                Href = cssPath
            };
            link.Attributes["rel"] = "stylesheet";
            link.Attributes["type"] = "text/css";
            page.Header.Controls.Add(link);
        }

        /// <summary>
        /// 更换样式文件路径
        /// </summary>
        /// <param name="page"></param>
        /// <param name="name"> </param>
        /// <param name="content"></param>
        internal static void AddMeta(Page page, string name, string content)
        {
            var mate = new HtmlMeta
            {
                Name = name,
                Content = content
            };
            page.Header.Controls.Add(mate);
        }

        #endregion
    }
}