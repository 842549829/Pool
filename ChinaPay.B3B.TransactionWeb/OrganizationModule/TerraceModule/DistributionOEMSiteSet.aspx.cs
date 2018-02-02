using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule
{
    public partial class DistributionOEMSiteSet : BasePage
    {
        private static string FileWeb = System.Configuration.ConfigurationManager.AppSettings["FileWeb"];
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                var oem = OEMService.QueryOEM(Guid.Parse(Request.QueryString["id"]));
                if (oem != null)
                {
                    OldImgUrlDiv.Visible = true;
                    txtName.Text = oem.SiteName;
                    txtDomain.Text = oem.DomainName;
                    txtEmail.Text = oem.ManageEmail;
                    txtICP.Text = oem.ICPRecord;
                    OldImgUrl.Text = FileWeb + oem.LogoPath;
                    txtEmbedCode.Text = oem.EmbedCode;
                    radEnabled.Checked = oem.Enabled;
                    radDisEnabled.Checked = !oem.Enabled;
                    radAllowSelfRegex.Checked = oem.AllowSelfRegex;
                    radDisSelfRegex.Checked = !oem.AllowSelfRegex;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (fileImg.PostedFile.ContentLength > 51200)
            {
                ShowMessage("上传图片过大，请选择小图片。");
                return;
            }
            string path = "";
            try
            {
                path = fileImg.HasFile ? Service.FileService.Upload(fileImg, "OEMLogo", "(jpg)|(gif)|(png)|(jpeg)", 51200) : OldImgUrl.Text.Replace(FileWeb, "");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "上传图片");
                return;
            }
            DistributionOEM oem = new DistributionOEM()
            {
                SiteName = txtName.Text,
                DomainName = txtDomain.Text,
                ManageEmail = txtEmail.Text,
                ICPRecord = txtICP.Text,
                LogoPath = path,
                EmbedCode = txtEmbedCode.Text,
                Enabled = radEnabled.Checked,
                AllowSelfRegex = radAllowSelfRegex.Checked
            };
            var oemold = OEMService.QueryOEM(Guid.Parse(Request.QueryString["id"]));
            oem.AirlineConfig = oemold.AirlineConfig;
            oem.AuthCashDeposit = oemold.AuthCashDeposit;
            oem.CompanyId = oemold.CompanyId;
            oem.Contract = oemold.Contract;
            oem.EffectTime = oemold.EffectTime;
            oem.RegisterTime = oemold.RegisterTime;
            oem.Setting = oemold.Setting;
            oem.UseB3BConfig = oemold.UseB3BConfig;
            oem.OperatorAccount = oemold.OperatorAccount;
            oem.Id = oemold.Id;
            oem.LoginUrl = oemold.LoginUrl;
            try
            {
                DistributionOEMService.UpdateDistributionOEM(oem);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改站点信息");
                return;
            }
            //刷新缓存
            FlushRequester.TriggerOEMFlusher(oem.Id);
            RegisterScript("alert('修改站点信息成功！');window.location.href='DistributionOemAuthorizationList.aspx';", true);
        }
    }
}