using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb {
    public partial class Default : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                var homePage = CurrentCompany.CompanyType == CompanyType.Purchaser
                                   ? "PurchaseDefault.aspx"
                                   : "Index.aspx";
                Server.Transfer(homePage);
            }
        }
    }
}