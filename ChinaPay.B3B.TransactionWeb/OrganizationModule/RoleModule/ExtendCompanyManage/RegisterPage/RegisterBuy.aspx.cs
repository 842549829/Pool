using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.ExtendCompanyManage.RegisterPage
{
    public partial class RegisterBuy : BasePage
    {
        #region 采购用户开户
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                CompanyInfo companyInfo = this.GetCompanyInfo();
                this.AccountsType(companyInfo);
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
        /// 获取参数值
        /// </summary>
        private string GetTypes()
        {
            return Request.QueryString["type"] == null ? string.Empty : Request.QueryString["type"];
        }
        /// <summary>
        /// 开户
        /// </summary>
        private bool AccountsType(CompanyInfo companyInfo)
        {
            string type = this.GetTypes();
            return !string.IsNullOrEmpty(type) ? (type == "SpreadBuy" ? CompanyService.Spread(this.CurrentCompany.CompanyId, companyInfo)
                : (type == "Subordinate" ? (CompanyService.CreateSubordinate(this.CurrentCompany.CompanyId, companyInfo))
                : (CompanyService.AddPurchaser(this.CurrentCompany.CompanyId, companyInfo))))
                : false;
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
                City =address.CityCode,
                District = address.CountyCode,
                CompanyType = CompanyType.Purchaser,
                CompanyId = Guid.NewGuid()
            };
        }
        #endregion
    }
}