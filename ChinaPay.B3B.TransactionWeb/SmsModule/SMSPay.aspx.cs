using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Gateway.Tradement;
using ChinaPay.PoolPay.Service;
using System.Text;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using PoolPay.DataTransferObject;
using ChinaPay.SMS.Service;

namespace ChinaPay.B3B.TransactionWeb.SmsModule
{
    public partial class SMSPay : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                bindPayTypes();
                bindPayAounmt();
                //hidPayHost.Value = Service.SystemManagement.SystemParamService.PayHost;
                hidUserName.Value = CurrentUser.UserName;
            }
        }
        private void bindPayTypes()
        {
            var payTypesHTML = new StringBuilder();
            var payTypeDetailsHTML = new StringBuilder();

            string payAccount = GetCurrentCompanyPayAccount();
            PayPoolBindAccount.Value = payAccount;

            if (!string.IsNullOrWhiteSpace(payAccount))
            {
#if(DEBUG)
                FillSourceDTO onlinePayTypes = AccountFillService.GetPaySources();

                // 第三方支付
                payTypesHTML.Append("<li>    <a href='#payType_Online' class='payType_Online'>账户支付</a></li>");
                payTypeDetailsHTML.Append("<div id='payType_Online'><ul class=\"form paysList\">");

                //国付通支付通道
                payTypeDetailsHTML.AppendFormat(
                    "<li title='{1}' style=\"polition:relative\">   <span class=\"icon1 poolpay\">{1}</span>  <input type='hidden' value='{2}' /> <span class=\"payRec\">推荐</span></li>",
                    10000, "国付通", "10000|2" + "|" + "国付通");


                foreach (FillSourceDTO.FillBankDTO item in onlinePayTypes.Channels)
                {
                    payTypeDetailsHTML.AppendFormat("<li title='{1}'>   <span class=\"icon1 Pay{0}\">{1}</span>  <input type='hidden' value='{2}' /> </li>", item.ChannelId,
                        item.BankName, item.Bank + "|" + item.BankName);
                }
                payTypeDetailsHTML.Append("</ul></div>");

                // 网银
                payTypesHTML.Append("<li>    <a href='#payType_Bank' class='payType_Bank'>网上银行</a></li>");
                payTypeDetailsHTML.Append("<div id='payType_Bank' class='tab-item'><ul class='form paysList'>");
                foreach (FillSourceDTO.FillBankDTO item in onlinePayTypes.Banks)
                {
                    if (item.BankType == "0")
                        payTypeDetailsHTML.AppendFormat("<li title='{1}'> <span class=\"icon {0}\">{1}</span> <input type='hidden' value='{2}' />", item.BankCode, item.BankName,
                            item.Bank + "|" + item.BankName);
                    if (item.BankType == "1")
                        payTypeDetailsHTML.AppendFormat(
                            "<li title='{1}'> <span class=\"icon {0}\">{1}</span><span class=\"icon_cop2\">企业</span> <input type='hidden' value='{2}' />", item.BankCode,
                            item.BankName, item.Bank + "|" + item.BankName);
                }
                payTypeDetailsHTML.Append("</ul></div>");
#else
                PayChannelQueryProcessor channelsRequest = new PayChannelQueryProcessor();

                if (channelsRequest.Execute())
                {
                    // 第三方支付
                    payTypesHTML.Append("<li>    <a href='#payType_Online' class='payType_Online'>账户支付</a></li>");
                    payTypeDetailsHTML.Append("<div id='payType_Online'><ul class=\"form paysList\">");

                    int index = 1;//在第一个支付方式上添加推荐标记
                    foreach (PayChannel item in channelsRequest.Channels)
                    {
                        payTypeDetailsHTML.AppendFormat("<li title='{1}'>   <span class=\"icon1 Pay{0}\">{1}</span>  <input type='hidden' value='{2}' /> {3}</li>", item.Code,
                            item.Name, item.Code + "||" + item.Name, index == 1 ? "<span class=\"payRec\">推荐</span>" : "");
                        index++;
                    }
                    payTypeDetailsHTML.Append("</ul></div>");

                    // 网银
                    payTypesHTML.Append("<li>    <a href='#payType_Bank' class='payType_Bank'>网上银行</a></li>");
                    payTypeDetailsHTML.Append("<div id='payType_Bank' class='tab-item'><ul class='form paysList'>");
                    foreach (Bank item in channelsRequest.Banks)
                    {
                        if (item.BankType == "0")
                            payTypeDetailsHTML.AppendFormat("<li title='{1}'> <span class=\"icon {0}\">{1}</span> <input type='hidden' value='{2}' />", item.Code, item.Name,
                                item.Channel + "|" + item.BankChannel + "|" + item.Name);
                        if (item.BankType == "1")
                            payTypeDetailsHTML.AppendFormat(
                                "<li title='{1}'> <span class=\"icon {0}\">{1}</span><span class=\"icon_cop2\">企业</span> <input type='hidden' value='{2}' />", item.Code,
                                item.Name, item.Channel + "|" + item.BankChannel + "|" + item.Name);
                    }
                    payTypeDetailsHTML.Append("</ul></div>");
                }
#endif
            }
            divPayTypes.InnerHtml = payTypesHTML.ToString();
            divPayTypeDetails.InnerHtml = payTypeDetailsHTML.ToString();
        }

        private string GetCurrentCompanyPayAccount()
        {
            Account payAccountModel = AccountService.Query(CurrentCompany.CompanyId, Common.Enums.AccountType.Payment);
            string payAccount = payAccountModel != null && payAccountModel.Valid ? payAccountModel.No : string.Empty;
            return payAccount;
        }

        private void bindPayAounmt()
        {
            var order = SMSOrderService.QueryOrder(decimal.Parse(Request.QueryString["orderid"]));
            lblPayAmount.Text = order.TotalAmount.ToString("C");
            hidBusinessId.Value = order.Id.ToString();
            hidBusinessType.Value = "3";
        }
    }
}