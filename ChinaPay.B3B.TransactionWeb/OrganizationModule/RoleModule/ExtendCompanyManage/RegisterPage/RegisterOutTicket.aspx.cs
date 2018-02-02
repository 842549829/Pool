using System;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.RegisterPage
{
    public partial class RegisterOutTicket :BasePage
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyInfo companyInfo = this.GetCompanyInfo();
                CompanyService.Spread(CurrentCompany.CompanyId, companyInfo);
                this.SucceedInfo(companyInfo);
            }
            catch (InvalidOperationException ex)
            {
                ShowMessage(ex.Message);
            }
            catch (Exception ex)
            {
                this.FailInfo(ex.Message);
            }
        }
        /// <summary>
        /// 成功信息
        /// </summary>
        private void SucceedInfo(CompanyInfo companyInfo)
        {
            Session["Info"] = companyInfo;
            Response.Redirect("./RegisterSucceed.aspx", false);
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
        private CompanyInfo GetCompanyInfo()
        {
            AddressInfo address = AddressInfo.GetAddress(this.hidAddress.Value);
            if (address == null) throw new ArgumentNullException("请选择所在地");
            return new CompanyInfo
            {
                CompanyId = Guid.NewGuid(),
                UserName = this.txtAccountNo.Text.Trim(),
                UserPassword = this.txtPassWord.Text.Trim(),
                ConfirmPassword = this.txtConfirmPassWord.Text.Trim(),
                CompanyName = this.txtCompanyName.Text.Trim(),
                OfficePhones = this.txtCompanyPhone.Text.Trim(),
                AbbreviateName = this.txtCompanyShortName.Text.Trim(),
                Address = this.txtAddress.Text.Trim(),
                ManagerName = this.txtPrincipal.Text.Trim(),
                ManagerCellphone = this.txtPrincipalPhone.Text.Trim(),
                Contact = this.txtLinkman.Text.Trim(),
                ContactPhone = this.txtLinkManPhone.Text.Trim(),
                EmergencyContact = this.txtUrgencyLinkMan.Text.Trim(),
                EmergencyCall = this.txtUrgencyLinkManPhone.Text.Trim(),
                ZipCode = this.txtPostCode.Text.Trim(),
                ManagerEmail = this.txtEmail.Text.Trim(),
                ManagerMsn = this.txtMSN.Text.Trim(),
                ManagerQQ = this.txtQQ.Text.Trim(),
                Faxes = this.txtFaxes.Text.Trim(),
                Area = address.AreaCode,
                Province = address.ProvinceCode,
                City = address.CityCode,
                District =address.CountyCode,
                CompanyType = CompanyType.Provider,
                PeriodStartOfUse = DateTime.Today.Date,
                PeriodEndOfUse =  DateTime.Today.AddYears(ChinaPay.B3B.Service.SystemManagement.SystemParamService.DefaultUseLimit).Date
            };
        }
    }
}