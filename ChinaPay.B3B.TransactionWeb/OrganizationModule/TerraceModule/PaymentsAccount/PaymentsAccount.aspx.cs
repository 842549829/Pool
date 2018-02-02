using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.PaymentsAccount
{
    public partial class PaymentsAccount :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                this.setBackButton();
                string id = Request.QueryString["CompanyId"];
                if (!string.IsNullOrWhiteSpace(id))
                { 
                    this.BindCompany(CompanyService.GetCompanyDetail(Guid.Parse(id)));
                    this.hidId.Value = id;
                }
            }
        }
        private void BindCompany(CompanyDetailInfo info)
        {
            this.lblAccountNo.Text = info.UserName;
            this.lblCompanyType.Text = info.CompanyType.GetDescription();
            this.lblUserName.Text = info.CompanyName;
            this.lblPetName.Text = info.AbbreviateName;
            this.lblLocation.Text = ChinaPay.B3B.TransactionWeb.PublicClass.AddressShow.GetAddressText(info.Area,info.Province,info.City,info.District);
            this.lblAddress.Text = info.Address;
            this.lblPostCode.Text = info.ZipCode;
            this.lblFaxes.Text = info.Faxes;
            this.lblLinkman.Text = info.Contact;
            this.lblLinkmanPhone.Text = info.ContactPhone;
            this.lblEmail.Text = info.ManagerEmail;
            this.lblMSN.Text = info.ManagerMsn;
            this.lblQQ.Text = info.ManagerQQ;
            this.tbArgen.Visible = false;
            if (info.AccountType == AccountBaseType.Individual)
            {
                this.lblFixedPhone.Text = info.OfficePhones;
            }
            else
            {
                this.fixedPhoneTitle.Visible = false;
                this.fixedPhoneValue.Visible = false;
            }
            this.BindCompanyType(info);
            this.BindAccount(info);
        }

        private void BindAccount(CompanyDetailInfo info)
        {
            this.BindPayment();
            this.trReceiving.Visible = false;
            switch (info.CompanyType)
            {
                case CompanyType.Provider:
                    this.trReceiving.Visible = true;
                    this.BindReceiving();
                    break;
                case CompanyType.Supplier:
                    this.trReceiving.Visible = true;
                    this.BindReceiving();
                    break;
                case CompanyType.Purchaser:
                    if (info.IsOem) {
                        this.trReceiving.Visible = true;
                        BindReceiving();
                    }
                    break;
            }
        }
        private void BindCompanyType(CompanyDetailInfo info)
        {
            if (info.CompanyType != CompanyType.Supplier)
            {
                this.tbArgen.Visible = true;
                this.lblPrincipal.Text = info.ManagerName;
                this.lblPrincipalPhone.Text = info.ManagerCellphone;
                this.lblUrgencyLinkman.Text = info.EmergencyContact;
                this.lblUrgencyLinkmanPhone.Text = info.EmergencyCall;
            }
            else {
                this.tdNickName.InnerText = "昵称";
                this.tdUserName.InnerText ="用户名";
                this.lblEmail.Text = info.ContactEmail;
                this.lblMSN.Text = info.ContactMSN;
                this.lblQQ.Text = info.ContactQQ;
            }
        }
        /// <summary>
        /// 绑定付款账号
        /// </summary>
        private void BindPayment()
        {
            var payment = this.GetAccount(AccountType.Payment);
            if (payment != null && !string.IsNullOrWhiteSpace(payment.No))
            {
                this.lblpaymentAccount.Text = payment.No;
                if (payment.Valid)
                {
                    this.lblpayment.Text = "有效";
                }
                else
                {
                    this.lblpayment.Text = "无效";
                }
            }
            else {
                this.trPayment.Visible = false;
            }
        }
        /// <summary>
        /// 绑定收款账号
        /// </summary>
        private void BindReceiving()
        {
            var receiving = this.GetAccount(AccountType.Receiving);
            if (receiving != null && !string.IsNullOrWhiteSpace(receiving.No))
            {
                this.lblReceivingAccount.Text = receiving.No;
                if (receiving.Valid)
                {
                    this.lblReceiving.Text = "有效";
                    this.btnReceiving.Value = "无效";
                }
                else
                {
                    this.lblReceiving.Text = "无效";
                    this.btnReceiving.Value = "有效";
                }
            }
            else {
                this.trReceiving.Visible = false;
            }
        }
        private ChinaPay.B3B.Service.Organization.Domain.Account GetAccount(AccountType accountType)
        {
            return AccountService.Query(Guid.Parse(Request.QueryString["CompanyId"]), accountType);
        }
        private void setBackButton()
        {
            this.btnGoBack.Attributes.Add("onclick", "window.location.href='" + (Request.UrlReferrer ?? Request.Url).PathAndQuery + "';return false;");
        }
    }
}