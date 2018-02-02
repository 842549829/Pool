using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Data.DataMapping;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy
{
    public partial class default_policy_addormodify : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                InitData();
                string ariline = Request.QueryString["airline"];
                if (!string.IsNullOrWhiteSpace(ariline))
                {
                    this.lblAddOrUpdate.Text = "修改";
                    var defaultPolicy = PolicyManageService.GetNormalDefaultPolicy(ariline);
                    ddlAirlines.SelectedValue = defaultPolicy.Airline;
                    ddlAirlines.Enabled = false;
                    AdultAgentCompany.SetCompanyName(defaultPolicy.AdultProviderId);
                    ChildAgentCompany.SetCompanyName( defaultPolicy.ChildProviderId);
                    txtAdult.Text = double.Parse((defaultPolicy.AdultCommission * 100).ToString()).ToString();
                    txtChild.Text = double.Parse((defaultPolicy.ChildCommission * 100).ToString()).ToString();

                }
            }
        }

        private void InitData()
        {
            ddlAirlines.DataSource = from item in FoundationService.Airlines
                                     select new
                                     {
                                         ShortName = item.Code + "-" + item.ShortName,
                                         Code = item.Code
                                     };
            ddlAirlines.DataTextField = "ShortName";
            ddlAirlines.DataValueField = "Code";
            ddlAirlines.DataBind();
            ddlAirlines.Items.Insert(0, new ListItem("-请选择-", ""));
            //var companies = CompanyService.GetCompanies(CompanyType.Provider);
            //AdultAgentCompany.InitCompanies(companies);
            //ChildAgentCompany.InitCompanies(companies);

            AdultAgentCompany.SetCompanyType(CompanyType.Provider);
            ChildAgentCompany.SetCompanyType(CompanyType.Provider);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Valiate())
            {
                try
                {
                    DefaultPolicy policy = saveInfo();
                    PolicyManageService.SaveDefaultPolicy(policy,this.CurrentUser.UserName);
                    RegisterScript("alert('保存成功');window.location.href='default_policy_manage.aspx';", false);
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "保存");
                }
            }
        }

        private DefaultPolicy saveInfo()
        {
            return new DefaultPolicy()
            {
                Airline = this.ddlAirlines.SelectedValue,
                ChildProvider = this.ChildAgentCompany.CompanyId.Value,
                AdultProvider = this.AdultAgentCompany.CompanyId.Value,
                AdultCommission = decimal.Parse(this.txtAdult.Text) / 100,
                ChildCommission = decimal.Parse(this.txtChild.Text) / 100
            };
        }

        private bool Valiate()
        {
            var pattern = "^[0-9]{1,2}(.[0-9])?$";
            if (string.IsNullOrWhiteSpace(this.ddlAirlines.SelectedValue))
            {
                ShowMessage("请选择航空公司！");
                return false;
            }
            if (!this.AdultAgentCompany.CompanyId.HasValue)
            {
                ShowMessage("请选择成人默认出票方！");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.txtAdult.Text))
            {
                ShowMessage("请填写成人默认返佣！");
                return false;
            }
            else
            {
                if (!Regex.IsMatch(this.txtAdult.Text, pattern))
                {
                    ShowMessage("成人默认返佣格式错误！");
                    return false;
                }
            }
            if (!this.ChildAgentCompany.CompanyId.HasValue)
            {
                ShowMessage("请选择儿童默认出票方！");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.txtChild.Text))
            {
                ShowMessage("请填写儿童默认返佣！");
                return false;
            }
            else
            {
                if (!Regex.IsMatch(this.txtChild.Text, pattern))
                {
                    ShowMessage("儿童默认返佣格式错误！");
                    return false;
                }
            }
            return true;
        }
    }
}