using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Organization;
using PoolPay.DataTransferObject;
using ChinaPay.IdentityCard;
using System.Text.RegularExpressions;
using ChinaPay.PoolPay.Service;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.RegisterPage
{
    public partial class PerfectPayInfo : BasePage
    {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack)
            {
                if (Session["Info"] == null) Response.Redirect("~/Default.aspx", false);
                this.EsistsAccount();
            }
        }
        private void EsistsAccount() {
            string account = Request.QueryString["AccountType"];
            if (!string.IsNullOrEmpty(account))
            {
                if (account != "Agent")
                    this.tbEnterprise.Visible = false;
                else 
                    this.tbEnterprise.Visible = true;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                IdCard.CheckIdentityCard(this.txtIDCard.Text);
                var user = this.IsAccountType() ?AccountBaseService.EnterpriseAccountOpening(this.GetEnterprisAccount()):
                    AccountBaseService.PersonAccountOpening(this.GetAccount() == null ? this.GetEnterprisAccount() : this.GetAccount());
                this.SucceedInfo();
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "开户");
            }
        }
        /// <summary>
        /// 成功信息
        /// </summary>
        private void SucceedInfo()
        {
            Response.Redirect("./RegisterPaySucceed.aspx?AccountNo=" + this.txtAccountNo.Text, false);
        }
        /// <summary>
        /// 失败信息
        /// </summary>
        private void FailInfo(string message)
        {
            this.lblMessage.InnerText = message;
        }
        /// <summary>
        /// 获取数据信息
        /// </summary>
        /// <returns></returns>
        private bool IsAccountType()
        {
            string account = Request.QueryString["AccountType"];
            return account!=null && account == "Agent" ? true : false;
        }
        /// <summary>
        /// 获取产品方信息
        /// </summary>
        private AccountDTO GetAccount()
        {
            SupplierCreatureInfo providerInfo = Session["Info"] as SupplierCreatureInfo;
            if (providerInfo == null) return null;
            return new AccountDTO
            {
                AccountNo = this.txtAccountNo.Text.Trim(),
                LoginPassword = providerInfo.UserPassword,
                PayPassword = IdCard.GetPayPassword(this.txtIDCard.Text.Trim()),
                AdministorName = providerInfo.Name,
                Email = providerInfo.Email,
                ContactPhone = providerInfo.ContactPhone,
                AdministorCertId = this.txtIDCard.Text.Trim(),
                OwnerState = AddressShow.GetProvince(providerInfo.Province),
                OwnerCity =AddressShow.GetCity(providerInfo.City),
                OwnerZone = AddressShow.GetCounty(providerInfo.District),
                OwnerStreet = providerInfo.Address,
                PostalCode = providerInfo.ZipCode
            };
        }
        /// <summary>
        /// 获取采购or出票 信息
        /// </summary>
        private EnterpriseAccountDTO GetEnterprisAccount()
        {
            CompanyInfo companyInfo = Session["Info"] as CompanyInfo;
            return new EnterpriseAccountDTO
            {
                AccountNo = this.txtAccountNo.Text.Trim(),
                LoginPassword = companyInfo.UserPassword,
                PayPassword = IdCard.GetPayPassword(this.txtIDCard.Text.Trim()),
                AdministorName = companyInfo.Contact,
                Email = companyInfo.ManagerEmail,
                ContactPhone = companyInfo.ContactPhone,
                AdministorCertId = this.txtIDCard.Text.Trim(),
                OwnerState = AddressShow.GetProvince(companyInfo.Province),
                OwnerCity = AddressShow.GetCity(companyInfo.City),
                OwnerZone = AddressShow.GetCounty(companyInfo.District),
                OwnerStreet = companyInfo.Address,
                PostalCode = companyInfo.ZipCode,
                OrganizationCode = this.txtOrganizationCode.Text.Trim(),
                CompanyName = companyInfo.CompanyName,
                LegalContactPhone = this.txtLegalPersonPhone.Text.Trim(),
                LegalPersonName = this.txtLegalPersonName.Text.Trim(),
                LegalPersonCertId = this.txtIDCard.Text.Trim()
            };
        }
    }
}