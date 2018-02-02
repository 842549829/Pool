using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.TransactionWeb {
    public partial class ValidateCode : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            var service = new ChinaPay.Utility.VerifyCodeService();
            service.CreateImageOnPage(LogonUtility.GenerateValidateCode(), HttpContext.Current);
        }
    }
}