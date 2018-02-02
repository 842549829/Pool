using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.Organization;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class IncomeGroupAdd : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                 string companyId = Request.QueryString["CompanyId"];
                 if (!string.IsNullOrWhiteSpace(companyId))
                 {
                     this.btnBack.Attributes.Add("onclick", "window.location.href='DistributionOEMUserUpdate.aspx?CompanyId="+companyId + "';");
                 }
            }
        }

        protected void btnSubmitIncomeGroup_Click(object sender, EventArgs e)
        {
            if (valiate())
            {
                try
                {
                    var incomeGroup = new IncomeGroup();
                    incomeGroup.Id = Guid.NewGuid();
                    incomeGroup.Company = this.CurrentCompany.CompanyId;
                    incomeGroup.CreateTime = DateTime.Now;
                    incomeGroup.Creator = this.CurrentUser.UserName;
                    incomeGroup.Name = this.txtIncomeGroupName.Text.Trim();
                    incomeGroup.Description = this.txtIncomeGroupDescription.Text.Trim();
                    IncomeGroupService.RegisterIncomeGroup(incomeGroup, this.CurrentUser.UserName);
                     string companyId = Request.QueryString["CompanyId"];
                     string incomeGroupId = Request.QueryString["IncomeGroupId"];
                     string strAccountType = Request.QueryString["AccountType"];
                     if (!string.IsNullOrWhiteSpace(companyId))
                     {
                         RegisterScript("alert('添加成功');window.location.href='DistributionOEMUserUpdate.aspx?CompanyId=" + companyId + "&IncomeGroupId=" + incomeGroupId + "&AccountType="+strAccountType + "'", false);
                     }
                     else
                     {
                         RegisterScript("alert('添加成功');window.location.href='IncomeGroupList.aspx';", false);
                     }
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex,"添加用户组");
                }
               

            }
        }

        private bool valiate()
        {
            if (this.txtIncomeGroupName.Text.Trim().Length == 0)
            {
                ShowMessage("请输入用户组名称");
                return false;
            }
            if (this.txtIncomeGroupName.Text.Trim().Length > 25)
            {
                ShowMessage("用户组名称位数不能超过25");
                return false;
            }

            if (this.txtIncomeGroupDescription.Text.Trim().Length > 200)
            {
                ShowMessage("用户组描述位数不能超过200");
                return false;
            }
            return true;
        }
    }
}