using System;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class UpdateBuyWorkInfo :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                string companyId = Request.QueryString["CompanyId"];
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    BindAccountNo(Guid.Parse(companyId));
                }
            }
        }
        private void BindAccountNo(Guid companyId)
        {
            hidId.Value = companyId.ToString();
            BindPayment(companyId);
            var oemInfo = ChinaPay.B3B.Service.Organization.OEMService.QueryOEM(companyId);
            if (oemInfo != null)
            {
                BindReceiving(companyId);
            }
            else {
                hfdIsHiddenReceiving.Value = "true";
            }
        }

        private void BindReceiving(Guid companyId)
        {
            var receiving = AccountService.Query(companyId, Common.Enums.AccountType.Receiving);
            if (receiving != null)
            {
                txtReceiving.Text = receiving.No;
                lblReceiving.InnerText = receiving.Valid ? "有效" : "无效";
                btnReceiving.Visible = !receiving.Valid;
            }
        }

        private void BindPayment(Guid companyId)
        {
            var payment = AccountService.Query(companyId, Common.Enums.AccountType.Payment);
            if (payment != null)
            {
                txtPayment.Text = payment.No;
                lblPayment.InnerText = payment.Valid ? "有效" : "无效";
                btnPayment.Visible = !payment.Valid;
            }
        }
    }
}