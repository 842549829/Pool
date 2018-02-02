using System;
using System.Web;

namespace ChinaPay.B3B.MaintenanceWeb {
    public partial class ValidateCode : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            var service = new ChinaPay.Utility.VerifyCodeService();
            service.CreateImageOnPage(LogonUtility.GenerateValidateCode(), HttpContext.Current);
        }
    }
}