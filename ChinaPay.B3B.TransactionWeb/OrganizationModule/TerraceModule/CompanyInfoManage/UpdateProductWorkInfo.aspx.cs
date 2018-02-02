using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class UpdateProductWorkInfo : BasePage
    {
        private readonly decimal defaultRate = 0M;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                if (!IsPostBack)
                {
                    string companyId = Request.QueryString["CompanyId"];
                    if (!string.IsNullOrWhiteSpace(companyId))
                    {
                        Guid id = Guid.Parse(companyId);
                        hidId.Value = companyId;
                        this.BindReceiving(id);
                        this.BindPayment(id);
                        this.BindParameter(id);
                    }
                }
            }
        }
        private void BindReceiving(Guid id)
        {
            var receiving = AccountService.Query(id, Common.Enums.AccountType.Receiving);
            if (receiving != null)
            {
                this.txtReceiving.Text = receiving.No;
                if (receiving.Valid)
                {
                    this.lblReceiving.InnerText = "有效";
                    this.btnReceiving.Visible = false;
                }
                else
                {
                    this.lblReceiving.InnerText = "无效";
                }
            }
        }
        private void BindPayment(Guid id)
        {
            var payment = AccountService.Query(id, Common.Enums.AccountType.Payment);
            if (payment != null)
            {
                this.txtPayment.Text = payment.No;
                if (payment.Valid)
                {
                    this.lblPayment.InnerText = "有效";
                    this.btnPayment.Visible = false;
                }
                else
                {
                    this.lblPayment.InnerText = "无效";
                }
            }
        }
        private void BindParameter(Guid id)
        {
            var parameter = CompanyService.GetCompanySettingsInfo(id);
            if (parameter != null && parameter.Parameter != null) {
                this.txtBeginTime.Text = parameter.Parameter.ValidityStart.HasValue ? parameter.Parameter.ValidityStart.Value.ToString("yyyy-MM-dd") : string.Empty;
                this.txtEndTime.Text = parameter.Parameter.ValidityEnd.HasValue ? parameter.Parameter.ValidityEnd.Value.ToString("yyyy-MM-dd") : string.Empty;
                //this.txtSpecialTicketCostRate.Text = (parameter.Parameter.SpecialRate*1000M).TrimInvaidZero();
                this.chkAutomaticAuditPolicy.Checked = parameter.Parameter.AutoPlatformAudit;
                if (!parameter.Parameter.Creditworthiness.HasValue)
                {
                    this.dropCreditworthiness.SelectedValue = "5";
                }
                else {
                    this.dropCreditworthiness.SelectedValue = parameter.Parameter.Creditworthiness.Value.TrimInvaidZero();    
                }
                BindSpecialProduct(parameter.Parameter);
            }
        }
        private void BindSpecialProduct(CompanyParameter paramter) {
            chkSingleness.Checked = paramter.Singleness && SpecialProductService.Query(SpecialProductType.Singleness).Enabled;
            txtSingleness.Text = (paramter.SinglenessRate * 1000M).TrimInvaidZero();
            chkDisperse.Checked = paramter.Disperse && SpecialProductService.Query(SpecialProductType.Disperse).Enabled;
            txtDisperse.Text = (paramter.DisperseRate * 1000M).TrimInvaidZero();
            //chkCostFree.Checked = paramter.CostFree && SpecialProductService.Query(SpecialProductType.CostFree).Enabled;
            //txtCostFree.Text = (paramter.CostFreeRate * 1000M).TrimInvaidZero();
            chkBloc.Checked = paramter.Bloc && SpecialProductService.Query(SpecialProductType.Bloc).Enabled;
            txtBloc.Text = (paramter.BlocRate * 1000M).TrimInvaidZero();
            chkBusiness.Checked = paramter.Business && SpecialProductService.Query(SpecialProductType.Business).Enabled;
            txtBusiness.Text = (paramter.BusinessRate * 1000M).TrimInvaidZero();

            if (!SpecialProductService.Query(SpecialProductType.Singleness).Enabled) { chkSingleness.Enabled = false; }
            if (!SpecialProductService.Query(SpecialProductType.Disperse).Enabled) { chkDisperse.Enabled = false; }
            //if (!SpecialProductService.Query(SpecialProductType.CostFree).Enabled) { chkCostFree.Enabled = false; }
            if (!SpecialProductService.Query(SpecialProductType.Bloc).Enabled) { chkBloc.Enabled = false; }
            if (!SpecialProductService.Query(SpecialProductType.Business).Enabled) { chkBusiness.Enabled = false; }
        }
        private CompanyParameter GetCompanyParameter(Guid id)
        {
            var parameter = new CompanyParameter();
            parameter.Company = id;
            parameter.ValidityStart = DateTime.Parse(this.txtBeginTime.Text);
            parameter.ValidityEnd = DateTime.Parse(this.txtEndTime.Text);
           // parameter.SpecialRate =decimal.Parse(this.txtSpecialTicketCostRate.Text.Trim())/1000M;
            parameter.AutoPlatformAudit = this.chkAutomaticAuditPolicy.Checked;
            parameter.Creditworthiness =decimal.Parse(dropCreditworthiness.SelectedValue);
            #region 特殊产品参数
            parameter.Singleness = chkSingleness.Checked && chkSingleness.Enabled;
            if (parameter.Singleness)
            {
                parameter.SinglenessRate = decimal.Parse(txtSingleness.Text.Trim()) / 1000M;
            }
            else
            {
                parameter.SinglenessRate = defaultRate;
            }
            parameter.Disperse = chkDisperse.Checked && chkDisperse.Enabled;
            if (parameter.Disperse)
            {
                parameter.DisperseRate = decimal.Parse(txtDisperse.Text.Trim()) / 1000M;
            }
            else
            {
                parameter.DisperseRate = defaultRate;
            }
            parameter.CostFree = false;
            parameter.CostFreeRate = defaultRate;
            //parameter.CostFree = chkCostFree.Checked && chkCostFree.Enabled;
            //if (parameter.CostFree)
            //{
            //    parameter.CostFreeRate = decimal.Parse(txtCostFree.Text.Trim()) / 1000M;
            //}
            //else
            //{
            //    parameter.CostFreeRate = defaultRate;
            //}
            parameter.Bloc = chkBloc.Checked && chkBloc.Enabled;
            if (parameter.Bloc)
            {
                parameter.BlocRate = decimal.Parse(txtBloc.Text.Trim()) / 1000M;
            }
            else
            {
                parameter.BlocRate = defaultRate;
            }
            parameter.Business = chkBusiness.Checked && chkBusiness.Enabled;
            if (parameter.Business)
            {
                parameter.BusinessRate = decimal.Parse(txtBusiness.Text.Trim()) / 1000M;
            }
            else
            {
                parameter.BusinessRate = defaultRate;
            } 
            #endregion
            return parameter;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyService.SetCompanyParameter(this.GetCompanyParameter(Guid.Parse(Request.QueryString["CompanyId"])),this.CurrentUser.UserName);
                Response.Redirect("./CompanyList.aspx?Search=Back", false);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex,"保存");
            }
        }
    }
}