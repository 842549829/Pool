using System;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class UpdateOutWorkInfo : BasePage
    {
        private readonly decimal defaultRate = 0M;
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                string companyId = Request.QueryString["CompanyId"];
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    Guid id = Guid.Parse(companyId);
                    hidId.Value = companyId;
                    this.BindAccount(id);
                    this.BindCompanyParameter(id);
                }
            }
        }
        /// <summary>
        /// 查询账号
        /// </summary>
        private void BindAccount(Guid id)
        {
            //BindArea(id);
            BindPayment(id);
            BindReceiving(id);
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
        /// <summary>
        /// 绑定公司参数
        /// </summary>
        private void BindCompanyParameter(Guid id) 
        {
            CompanySettingsInfo parameter = CompanyService.GetCompanySettingsInfo(id);
            if (parameter != null && parameter.Parameter != null)
            {
                this.chkAllowBSPTicket.Checked = parameter.Parameter.AutoPrintBSP;
                this.txtLockTicket.Text = parameter.Parameter.RefundCountLimit.ToString();
                this.chkAllowB2BTicket.Checked = parameter.Parameter.AutoPrintB2B;
                this.txtVoluntaryRefundsLimit.Text = parameter.Parameter.RefundTimeLimit.ToString();
                //this.chkPNR.Checked = parameter.Parameter.CancelPnrBySelf;
                this.txtAllRefundsLimit.Text = parameter.Parameter.FullRefundTimeLimit.ToString();
                this.chkVIP.Checked = parameter.Parameter.CanReleaseVip;
                this.chkAllowBrotherPurchase.Checked = parameter.Parameter.AllowBrotherPurchase;
                this.txtPeerTradingRate.Text =(parameter.Parameter.ProfessionRate * 1000M).TrimInvaidZero();
                this.txtLowerRates.Text = (parameter.Parameter.SubordinateRate * 1000M).TrimInvaidZero();
                this.chkRefundFinancialAudit.Checked = parameter.WorkingSetting != null ? parameter.WorkingSetting.RefundNeedAudit : false;
               // this.txtSpecialRates.Text = (parameter.Parameter.SpecialRate * 1000M).TrimInvaidZero();
                this.chkInternalOrganization.Checked = parameter.Parameter.CanHaveSubordinate;
                this.chkAutomaticAuditPolicy.Checked = parameter.Parameter.AutoPlatformAudit;
                this.txtBeginTime.Text = parameter.Parameter.ValidityStart.HasValue ? parameter.Parameter.ValidityStart.Value.ToString("yyyy-MM-dd") : string.Empty;
                this.txtEndTime.Text = parameter.Parameter.ValidityEnd.HasValue ? parameter.Parameter.ValidityEnd.Value.ToString("yyyy-MM-dd") : string.Empty;
                if (!parameter.Parameter.Creditworthiness.HasValue)
                {
                    this.dropCreditworthiness.SelectedValue = "5";
                }
                else
                {
                    this.dropCreditworthiness.SelectedValue = parameter.Parameter.Creditworthiness.Value.TrimInvaidZero();
                }
                BindSpecialProduct(parameter.Parameter);
            }
            BindChildern(parameter);
        }
        private void BindSpecialProduct(CompanyParameter paramter)
        {
            chkSingleness.Checked = paramter.Singleness && SpecialProductService.Query(SpecialProductType.Singleness).Enabled;
            txtSingleness.Text = (paramter.SinglenessRate * 1000M).TrimInvaidZero();
            chkDisperse.Checked = paramter.Disperse && SpecialProductService.Query(SpecialProductType.Disperse).Enabled;
            txtDisperse.Text = (paramter.DisperseRate * 1000M).TrimInvaidZero();
            chkCostFree.Checked = paramter.CostFree && SpecialProductService.Query(SpecialProductType.CostFree).Enabled;
            txtCostFree.Text = (paramter.CostFreeRate * 1000M).TrimInvaidZero();
            chkBloc.Checked = paramter.Bloc && SpecialProductService.Query(SpecialProductType.Bloc).Enabled;
            txtBloc.Text = (paramter.BlocRate * 1000M).TrimInvaidZero();
            chkBusiness.Checked = paramter.Business && SpecialProductService.Query(SpecialProductType.Business).Enabled;
            txtBusiness.Text = (paramter.BusinessRate * 1000M).TrimInvaidZero();
            chkOtherSpecial.Checked = paramter.OtherSpecial && SpecialProductService.Query(SpecialProductType.OtherSpecial).Enabled;
            txtOtherSpecialRate.Text = (paramter.OtherSpecialRate * 1000M).TrimInvaidZero();
            chkLowToHigh.Checked = paramter.LowToHigh && SpecialProductService.Query(SpecialProductType.LowToHigh).Enabled;
            txtLowToHighRate.Text = (paramter.LowToHighRate * 1000M).TrimInvaidZero();

            if (!SpecialProductService.Query(SpecialProductType.Singleness).Enabled) { chkSingleness.Enabled = false; }
            if (!SpecialProductService.Query(SpecialProductType.Disperse).Enabled) { chkDisperse.Enabled = false; }
            if (!SpecialProductService.Query(SpecialProductType.CostFree).Enabled) { chkCostFree.Enabled = false; }
            if (!SpecialProductService.Query(SpecialProductType.Bloc).Enabled) { chkBloc.Enabled = false; }
            if (!SpecialProductService.Query(SpecialProductType.Business).Enabled) { chkBusiness.Enabled = false; }
            if (!SpecialProductService.Query(SpecialProductType.OtherSpecial).Enabled) { chkOtherSpecial.Enabled = false; }
            if (!SpecialProductService.Query(SpecialProductType.LowToHigh).Enabled) { chkLowToHigh.Enabled = false; }
        }
        private void BindChildern(CompanySettingsInfo parameter)
        {
            if (parameter != null && parameter.WorkingSetting != null )
            {
                this.lblDefaultOffice.Text = parameter.WorkingSetting.DefaultOfficeNumber;
                if (parameter.WorkingSetting.RebateForChild.HasValue)
                {
                    this.chkChildren.Checked = true;
                    this.lblChildren.Text = (parameter.WorkingSetting.RebateForChild.Value * 100M).TrimInvaidZero();
                    if (!string.IsNullOrEmpty(parameter.WorkingSetting.AirlineForChild))
                    {
                        foreach (var item in parameter.WorkingSetting.AirlineForChild.Split('/'))
                        {
                            ListItem listItem = new ListItem(item.ToString());
                            listItem.Selected = true;
                            this.chklChildren.Items.Add(listItem);
                        }
                    }
                }
                else {
                   lblChildrens.Visible = false;
                }

                if (!string.IsNullOrWhiteSpace(parameter.WorkingSetting.AirlineForDefault)&&parameter.WorkingSetting.RebateForDefault.HasValue)
                {
                    this.chkDefault.Checked = true;
                    this.lblRebateForDefault.Text = (parameter.WorkingSetting.RebateForDefault.Value * 100M).TrimInvaidZero();
                    if (!string.IsNullOrEmpty(parameter.WorkingSetting.AirlineForDefault))
                    {
                        foreach (var item in parameter.WorkingSetting.AirlineForDefault.Split('/'))
                        {
                            ListItem listItem = new ListItem(item.ToString());
                            listItem.Selected = true;
                            this.chkAirlineForDefault.Items.Add(listItem);
                        }
                    }
                }
                else
                {
                    lblDefaultRebate.Visible = false;
                }

            }
            else
            {
                lblChildrens.Visible = false;
                lblDefaultRebate.Visible = false;
            }
        }
        /// <summary>
        /// 获取参数信息
        /// </summary>
        private CompanyParameter GetCompanyParameter(Guid id)
        {
            var parameter = new CompanyParameter();
            parameter.Company =id;
            parameter.AutoPrintBSP = this.chkAllowBSPTicket.Checked;
            parameter.AutoPrintB2B = this.chkAllowB2BTicket.Checked;
            parameter.AutoPlatformAudit = this.chkAutomaticAuditPolicy.Checked;
            //parameter.CancelPnrBySelf = this.chkRefundFinancialAudit.Checked,
            parameter.CanHaveSubordinate = this.chkInternalOrganization.Checked;
            parameter.CanReleaseVip = this.chkVIP.Checked;
            parameter.AllowBrotherPurchase = chkAllowBrotherPurchase.Checked;
            parameter.FullRefundTimeLimit = int.Parse(this.txtAllRefundsLimit.Text.Trim());
            parameter.ProfessionRate = decimal.Parse(this.txtPeerTradingRate.Text.Trim()) / 1000M;
           // parameter.SpecialRate = decimal.Parse(this.txtSpecialRates.Text.Trim()) / 1000M;
            parameter.SubordinateRate = decimal.Parse(this.txtLowerRates.Text.Trim()) / 1000M;
            parameter.RefundCountLimit = int.Parse(this.txtLockTicket.Text.Trim());
            parameter.RefundTimeLimit = int.Parse(this.txtVoluntaryRefundsLimit.Text);
            parameter.ValidityStart = DateTime.Parse(this.txtBeginTime.Text);
            parameter.ValidityEnd = DateTime.Parse(this.txtEndTime.Text);
            parameter.Creditworthiness = decimal.Parse(dropCreditworthiness.SelectedValue);
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
            parameter.CostFree = chkCostFree.Checked && chkCostFree.Enabled;
            if (parameter.CostFree)
            {
                parameter.CostFreeRate = decimal.Parse(txtCostFree.Text.Trim()) / 1000M;
            }
            else
            {
                parameter.CostFreeRate = defaultRate;
            }
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
            parameter.OtherSpecial = chkOtherSpecial.Checked && chkOtherSpecial.Enabled;
            if (parameter.OtherSpecial)
            {
                parameter.OtherSpecialRate = decimal.Parse(txtOtherSpecialRate.Text.Trim()) / 1000M;
            }
            else 
            {
                parameter.OtherSpecialRate = defaultRate;
            }
            parameter.LowToHigh = chkLowToHigh.Checked && chkLowToHigh.Enabled;
            if (parameter.LowToHigh)
            {
                parameter.LowToHighRate = decimal.Parse(txtLowToHighRate.Text.Trim()) / 1000M;
            }
            else 
            {
                parameter.LowToHighRate = defaultRate;
            }
            #endregion
            return parameter;
        }
        /// <summary>
        /// 保存信息
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var comapnyParameter = this.GetCompanyParameter(Guid.Parse(Request.QueryString["CompanyId"]));
                CompanyService.SetCompanyParameter(comapnyParameter,this.CurrentUser.UserName);
                Response.Redirect("./CompanyList.aspx?Search=Back", false);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "保存");
            }
        }
    }
}