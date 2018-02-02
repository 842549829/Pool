using System.Web;

namespace ChinaPay.B3B.MaintenanceWeb
{
    public class BaseHandler : ChinaPay.Infrastructure.WebEx.AjaxHandler.WebAjaxHandler {
        protected sealed override void Pre_Invoke(HttpContext context) {
            if(this.CurrentUser == null) {
                throw new RequireLogonException();
            }
            if (!BasePage.HasPermission(context.Request.UrlReferrer.AbsolutePath))
            {
                throw new UnauthorizedException();
            }
            base.Pre_Invoke(context);
        }
        protected DataTransferObject.Organization.EmployeeDetailInfo CurrentUser {
            get { return BasePage.LogonUser; }
        }
        protected DataTransferObject.Organization.CompanyDetailInfo CurrentCompany {
            get { return BasePage.LogonCompany; }
        }
    }
}