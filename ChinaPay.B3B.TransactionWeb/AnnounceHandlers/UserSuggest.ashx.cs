using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Announce;
using ChinaPay.B3B.Service.SystemSetting;

namespace ChinaPay.B3B.TransactionWeb.AnnounceHandlers
{
    public class UserSuggest : IHttpHandler, IRequiresSessionState
    {
        private readonly Regex HtmlFilter = new Regex(@"<[\w\W]*>",RegexOptions.Compiled|RegexOptions.Multiline);

        public string AddSuggest(int category,string content,string method) {
            string userName = "匿名";
            string userAccount = string.Empty;
            if (HttpContext.Current.Session[BasePage.EmployeeSessionKey] != null)
            {
                var CurrentUser = HttpContext.Current.Session[BasePage.EmployeeSessionKey] as DataTransferObject.Organization.EmployeeDetailInfo;
                userName = CurrentUser.Name;
                userAccount = CurrentUser.UserName;
            }
            var suggest = new Suggest
            {
                SuggestCategory = (SuggestCategory)category,
                SuggestContent = HtmlFilter.Replace(content,string.Empty),
                ContractInformation =  HtmlFilter.Replace(method,string.Empty),
                CreateTime = DateTime.Now,
                Creator = userAccount,
                CreatorName = userName,
                Id = Guid.NewGuid(),
                Readed = false,
                Handled = false
            };
            SuggestService.AddSuggest(suggest);
            return "OK";
        }


        public void ProcessRequest(HttpContext context) {
            switch (context.Request.QueryString["action"])
            {
                case "AddSuggest":
                    var result = AddSuggest(int.Parse(context.Request.Form["category"]),
                        context.Request.Form["content"], context.Request.Form["method"]);
                    context.Response.Write(result);
                    break;
                default:
                    break;
            }
            
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}