using System;
using System.Web.UI;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Purchase {
    public partial class Pay : Page {
        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                decimal businessId;
                var bankInfo = Request.QueryString["bank"];
                var userName = Request.QueryString["userName"];
                if(decimal.TryParse(Request.QueryString["id"], out businessId) || string.IsNullOrWhiteSpace(bankInfo)) {
                    try {
                        string url;
                        bankInfo = System.Web.HttpUtility.UrlDecode(bankInfo);
                        userName = System.Web.HttpUtility.UrlDecode(userName);
                        var clientIP = ChinaPay.AddressLocator.IPAddressLocator.GetRequestIP(Request).ToString();
                        if(Request.QueryString["type"] == "2") {
                            url = Service.Tradement.PaymentService.OnlinePayPostponeFee(businessId, bankInfo, clientIP, userName);
                        } else  
                        if (Request.QueryString["type"] == "3")
                        {
                            var company = CompanyService.GetCompanyDetail(userName);
                            url = Service.Tradement.PaymentService.OnlinePaySMSOrder(businessId, bankInfo, clientIP, userName,company.CompanyId);
                        }
                        else
                        {
                            url = Service.Tradement.PaymentService.OnlinePayOrder(businessId, bankInfo, clientIP, userName);
                        }
                        Response.Redirect(url, false);
                    } catch(Exception ex) {
                        Response.Write(ex.Message);
                    }
                } else {
                    Response.Write("参数错误");
                }
            }
        }
    }
}