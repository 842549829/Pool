using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;
using oem = ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule
{
    public partial class OEMContract : BasePage
    {
        private Guid GetGuid()
        {
            string company = Request.QueryString["CompanyId"];
            return !string.IsNullOrEmpty(company) && (company.Length == 36 || company.Length == 32) ? Guid.Parse(company) : CurrentCompany.CompanyId;
        }

        private bool IsPlateform
        {
            get
            {
                return GetGuid() != CurrentCompany.CompanyId;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                Guid companyId = GetGuid();
                btnCancel.Visible = IsPlateform;
                bingOEMContract(companyId);
            }
        }

        private void bingOEMContract(Guid companyId)
        {
            var oemInfo = OEMService.QueryOEM(companyId);
            if (oemInfo != null && oemInfo.Contract != null)
            {
                bingOEMContract(oemInfo.Contract);
            }
        }

        private void bingOEMContract(oem.OEMContract contract)
        {
            txtEnterpriseQQ.Text = contract.EnterpriseQQ;
            txtFax.Text = contract.Fax;
            txtServicePhone.Text = contract.ServicePhone;
            txtRefundPhone.Text = contract.RefundPhone;
            txtScrapPhone.Text = contract.ScrapPhone;
            txtPayServicePhone.Text = contract.PayServicePhone;
            txtEmergencyPhone.Text = contract.EmergencyPhone;
            txtComplainPhone.Text = contract.ComplainPhone;
            rdoAllowUseB3BServicePhone.Checked = contract.UseB3BServicePhone;
            rdoAllowNotUseB3BServicePhone.Checked = !contract.UseB3BServicePhone;
            rdoAllowPlatformContractPurchaser.Checked = contract.AllowPlatformContractPurchaser;
            rdoNotAllowPlatformContractPurchaser.Checked = !contract.AllowPlatformContractPurchaser;
        }

        private oem.OEMInfo createOEMContract()
        {
            Guid companyId = GetGuid();
            var oemInfo = OEMService.QueryOEM(companyId);
            oemInfo.Contract.EnterpriseQQ = txtEnterpriseQQ.Text.Trim();
            oemInfo.Contract.Fax = txtFax.Text.Trim();
            oemInfo.Contract.ServicePhone = txtServicePhone.Text.Trim();
            oemInfo.Contract.RefundPhone = txtRefundPhone.Text.Trim();
            oemInfo.Contract.ScrapPhone = txtScrapPhone.Text.Trim();
            oemInfo.Contract.PayServicePhone = txtPayServicePhone.Text.Trim();
            oemInfo.Contract.EmergencyPhone = txtEmergencyPhone.Text.Trim();
            oemInfo.Contract.ComplainPhone = txtComplainPhone.Text.Trim();
            oemInfo.Contract.UseB3BServicePhone = rdoAllowUseB3BServicePhone.Checked;
            oemInfo.Contract.AllowPlatformContractPurchaser = rdoAllowPlatformContractPurchaser.Checked;
            return oemInfo;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var contract = createOEMContract();
                OEMService.SvaeContract(contract, CurrentUser.UserName);
                FlushRequester.TriggerOEMFlusher(contract.Id);
                if (IsPlateform)
                {
                    Response.Redirect("/OrganizationModule/TerraceModule/DistributionOemAuthorizationList.aspx?Search=Back", true);
                }
                else
                {
                    ShowMessage("保存成功");
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "保存");
            }
        }
    }
}