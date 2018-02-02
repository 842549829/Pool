using System;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule
{
    public partial class DistributionOemAuthorizationAddOrUpdate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            setBackButton();
            if (!IsPostBack)
            {
                string oemId = Request.QueryString["OemId"];
                if (!string.IsNullOrWhiteSpace(oemId))
                {
                    initData(oemId);
                }
            }
        }

        private void initData(string oemId)
        {
            lblOperator.Text = "修改";
            insert.Visible = false;
            update.Visible = true;
            OEMInfo distributionOEM = OEMService.QueryOEMById(Guid.Parse(oemId));
            if (distributionOEM != null)
            {
                txtOemName.Text = distributionOEM.SiteName;
                txtAuthorizationDomain.Text = distributionOEM.DomainName;
                txtAuthorizationDeadline.Text = distributionOEM.EffectTime.HasValue ? distributionOEM.EffectTime.Value.ToString("yyyy-MM-dd") : string.Empty;
                txtAuthorizationDeposit.Text = distributionOEM.AuthCashDeposit.TrimInvaidZero();
                rdnPlatform.Checked = distributionOEM.UseB3BConfig;
                rdnOwner.Checked = !distributionOEM.UseB3BConfig;
                txtLoginUrl.Text = distributionOEM.LoginUrl;
                CompanyDetailInfo companyDetailInfo = CompanyService.GetCompanyDetail(distributionOEM.CompanyId);
                if (companyDetailInfo != null)
                    lblB3bAccountNo.Text = companyDetailInfo.UserName;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (valiate())
                {
                    DistributionOEM distributionOEM = save();
                    if (lblOperator.Text == "修改")
                    {
                        DistributionOEMService.UpdateOemInfo(distributionOEM, CurrentUser.UserName);
                        FlushRequester.TriggerOEMFlusher(distributionOEM.Id);
                    }
                    else
                    {
                        CompanyDetailInfo companyDetailInfo = CompanyService.GetCompanyDetail(txtB3bAccountNo.Text.Trim());
                        if (companyDetailInfo == null)
                        {
                            ShowMessage("该B3B账号不存在");
                        }
                        else if(companyDetailInfo.CompanyType==CompanyType.Platform)
                        {
                            ShowMessage("平台帐号不能开通OEM");
                        }
                        else
                        {
                            distributionOEM.CompanyId = companyDetailInfo.CompanyId;

                            DistributionOEMService.RegisterDistributionOEM(distributionOEM, companyDetailInfo.IsOem, companyDetailInfo.AbbreviateName);
                        }
                    }
                    FlushRequester.TriggerOEMAdder();
                    RegisterScript("alert('添加/修改成功');window.location.href='DistributionOemAuthorizationList.aspx'", false);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "添加/修改");
            }
        }

        private DistributionOEM save()
        {
            var distributionOem = new DistributionOEM();
            string oemId = Request.QueryString["OemId"];
            if (string.IsNullOrWhiteSpace(oemId))
            {
                distributionOem.Id = Guid.NewGuid();
                distributionOem.OperatorAccount = CurrentUser.UserName;
                distributionOem.RegisterTime = DateTime.Now;
            }
            else
            {
                distributionOem.Id = Guid.Parse(oemId);
            }
            distributionOem.AuthCashDeposit = decimal.Parse(txtAuthorizationDeposit.Text);
            distributionOem.DomainName = txtAuthorizationDomain.Text;
            distributionOem.EffectTime = DateTime.Parse(txtAuthorizationDeadline.Text);
            distributionOem.SiteName = txtOemName.Text;
            distributionOem.UseB3BConfig = rdnPlatform.Checked;
            distributionOem.LoginUrl = txtLoginUrl.Text.Trim();
            return distributionOem;
        }

        private bool valiate()
        {
            if (lblOperator.Text != "修改")
            {
                if (txtB3bAccountNo.Text.Trim().Length == 0)
                {
                    ShowMessage("请输入B3B账号");
                    return false;
                }
            }
            if (txtOemName.Text.Trim().Length == 0)
            {
                ShowMessage("请输入OEM名称");
                return false;
            }
            if (txtOemName.Text.Trim().Length > 50)
            {
                ShowMessage("OEM名称位数不能超过50位");
                return false;
            }
            if (txtAuthorizationDomain.Text.Trim().Length == 0)
            {
                ShowMessage("请输入授权域名");
                return false;
            }
            if (txtAuthorizationDomain.Text.Trim().Length > 40)
            {
                ShowMessage("授权域名位数不能超过40位");
                return false;
            }
            if (txtAuthorizationDeposit.Text.Trim().Length == 0)
            {
                ShowMessage("请输入授权保证金");
                return false;
            }
            if (!Regex.IsMatch(txtAuthorizationDeposit.Text.Trim(), "^[1-9][0-9]{0,7}(.[0-9]{1,2})?$"))
            {
                ShowMessage("授权保证金格式错误");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 返回
        /// </summary>
        private void setBackButton()
        {
            string returnUrl = Request.QueryString["returnUrl"] ?? Request.UrlReferrer.AbsoluteUri;
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "CompanyList.aspx";
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            btnGoBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';");
        }
    }
}