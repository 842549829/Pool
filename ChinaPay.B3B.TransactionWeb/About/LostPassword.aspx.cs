using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.AddressLocator;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;

namespace ChinaPay.B3B.TransactionWeb.About
{
    public partial class LostPassword :UnAuthBasePage
    {
        private const int m_item = 120;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("core.css");
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            SendSMSTime();
            if (!IsPostBack)
            {
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
                Response.Cache.SetNoStore();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            EmployeeDetailInfo info = EmployeeService.QueryEmployee(txtAccountNo.Value);
            if (info == null)
            {
                txtAccountNoTip.InnerHtml = "账号无效，不存在该账户";
                txtReAccountNoTip.InnerHtml = "";
                txtCodeTip.InnerHtml = "";
                return;
            }
            if (Session["lostCode"] == null) {
                txtAccountNoTip.InnerHtml = "";
                txtReAccountNoTip.InnerHtml = "";
                txtCodeTip.InnerHtml = "验证码已过期";
                return;
            }
            if (txtCode.Value.ToUpper() != Session["lostCode"].ToString().ToUpper())
            {
                txtAccountNoTip.InnerHtml = "";
                txtReAccountNoTip.InnerHtml = "";
                txtCodeTip.InnerHtml = "验证码不正确";
                BasePage.RegisterScript(this, "window.onload = function(){loadValidateCode(); };");
                return;
            }

            IPAddress ip = IPAddressLocator.GetRequestIP(HttpContext.Current.Request);
            if (!AccountCombineService.ValidateIP(ip.ToString()))
            {
                txtAccountNoTip.InnerHtml = "";
                txtReAccountNoTip.InnerHtml = "";
                txtCodeTip.InnerHtml = "同一个IP一天只有3次获取验证码的机会";
                return;
            }
            var verfiCode = new VerfiCode()
            {
                CellPhone = info.Cellphone,
                Code = ChinaPay.Utility.VerifyCodeUtility.CreateVerifyCode(6),
                IP = ip.ToString(),
                Type = Common.Enums.VerfiCodeType.Register,
                AccountNo = txtAccountNo.Value
            };
            Session["phoneValidateCode"] = verfiCode.Code;
            Session["phone"] = verfiCode.CellPhone;
            Session["accountno"] = verfiCode.AccountNo;
            Session["phoneTime"] = DateTime.Now;
            ChinaPay.SMS.Service.SMSSendService.SendB3bTrade(verfiCode.CellPhone, verfiCode.Code, 20,BasePage.CurrenContract.ServicePhone);
            AccountCombineService.SaveVerfiCode(verfiCode);
            SendSMSTime();
            BasePage.RegisterScript(this, "window.location.href='RelostPasswordCode.aspx'"); 
        }
        private void SendSMSTime()
        {
            if (Session["phoneTime"] != null)
            {
                var phoneTime = (DateTime)Session["phoneTime"];
                int timeSeconds = (int)(DateTime.Now - phoneTime).TotalSeconds;
                if (timeSeconds <= m_item)
                {
                    BasePage.RegisterJavaScript(this, "window.onload = function(){CountDown(" + (m_item - timeSeconds) + "); };");
                }
            }
        }

    }
}