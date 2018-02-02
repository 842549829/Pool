using System;
using System.Web.UI;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.Account
{
    public partial class SetPayOrPutAccount : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RegisterOEMSkins("form.css");

                AccountType(this.CurrentCompany.CompanyType);
                this.hidId.Value = this.CurrentCompany.CompanyId.ToString();
            }
        }
        private void AccountType(CompanyType type) {
            this.BindPayAccount();
            switch (type)
            {
                case CompanyType.Provider: /*出票*/
                    BindReceiving();
                    break;
                case CompanyType.Purchaser:  /*采购*/
                    DispalyAccount();
                    break;
                case CompanyType.Supplier:   /*产品*/
                    BindReceiving();
                    break;
                default:
                    BindReceiving();
                    break;
            }
        }
        private void DispalyAccount()
        {
            this.tbPut.Visible = false;
        }
        private void BindReceiving() {
            var receiving = AccountService.Query(this.CurrentCompany.CompanyId, Common.Enums.AccountType.Receiving);
            this.txtPutAccount.Style.Add(HtmlTextWriterStyle.Display, "none");
            if (receiving != null)
            {
                this.lblPutAccount.Text = receiving.No;
                
                if (receiving.Valid)
                {
                    this.lblPutStatus.Text = "有效";
                    this.btnPut.Visible = false;
                }
                else
                {
                    this.lblPutStatus.Text = "无效";
                }
            }
        }
        private void BindPayAccount() {
            var payment = AccountService.Query(this.CurrentCompany.CompanyId, Common.Enums.AccountType.Payment);
            this.txtPayAccount.Style.Add(HtmlTextWriterStyle.Display, "none");
            if (payment != null)
            {
                this.lblPayAccount.Text = payment.No;
                if (payment.Valid)
                {
                    this.lblPaySatus.Text = "有效";
                    this.btnPay.Visible = false;
                }
                else
                {
                    this.lblPaySatus.Text = "无效";
                }
            }
        }
    }
}