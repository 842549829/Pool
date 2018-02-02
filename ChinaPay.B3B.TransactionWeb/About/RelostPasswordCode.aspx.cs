using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.AddressLocator;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.About
{
    public partial class RelostPasswordCode : UnAuthBasePage
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
                if (Session["phone"] != null)
                {
                    lblphone.InnerHtml = Session["phone"].ToString().LeftString(3) + "********";
                }
                else
                {
                    BasePage.RegisterScript(this, "window.location.href='LostPassword.aspx'");
                }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (txtReCode.Value.ToUpper() != Session["phoneValidateCode"].ToString().ToUpper())
            {
                BasePage.RegisterScript(this, "alert('验证码错误。');");
                return;
            }
            BasePage.RegisterScript(this, "window.location.href='UpdatelostPassword.aspx'"); 
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

        protected void btnPhoneCode_Click(object sender, EventArgs e)
        {
            try
            {
                if (SendSms())
                {
                    SendSMSTime();
                }
            }
            catch (Exception ex)
            {
                BasePage.ShowExceptionMessage(this, ex, "发送验证码");
            }
        }
        private bool SendSms()
        {
            IPAddress ip = IPAddressLocator.GetRequestIP(HttpContext.Current.Request);
            if (!AccountCombineService.ValidateIP(ip.ToString()))
            {
                BasePage.RegisterScript(this, "alert('同一个IP一天只有3次获取验证码的机会');");
                return false;
            }
            var verfiCode = new VerfiCode
            {
                CellPhone = Session["phone"].ToString(),
                Code = ChinaPay.Utility.VerifyCodeUtility.CreateVerifyCode(6),
                IP = ip.ToString(),
                Type = Common.Enums.VerfiCodeType.Register,
                AccountNo = Session["accountno"].ToString()
            };
            Session["phoneValidateCode"] = verfiCode.Code;
            Session["phone"] = verfiCode.CellPhone;
            Session["phoneTime"] = DateTime.Now;
            ChinaPay.SMS.Service.SMSSendService.SendB3bTrade(verfiCode.CellPhone, verfiCode.Code, 20,BasePage.CurrenContract.ServicePhone);
            AccountCombineService.SaveVerfiCode(verfiCode);
            return true;
        }

    }
}