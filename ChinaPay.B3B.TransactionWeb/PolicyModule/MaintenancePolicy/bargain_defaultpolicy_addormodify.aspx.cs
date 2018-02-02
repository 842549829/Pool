using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Policy;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy
{
    public partial class bargain_defaultpolicy_addormodify : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                InitData();
                string ariline = Request.QueryString["airline"];
                string provinceCode = Request.QueryString["ProvinceCode"];
                if (!string.IsNullOrWhiteSpace(ariline) && !string.IsNullOrWhiteSpace(provinceCode))
                {
                    this.lblAddOrUpdate.Text = "修改";
                    var defaultPolicy = PolicyManageService.GetBargainDefaultPolicy(ariline, provinceCode);
                    ddlAirlines.SelectedValue = defaultPolicy.Airline;
                    ddlAirlines.Enabled = false;
                    ddlProvince.Enabled = false;
                    this.ddlProvince.SelectedValue = defaultPolicy.ProvinceCode; 
                    AdultAgentCompany.SetCompanyName(defaultPolicy.AdultProviderId);
                    AdultAgentCompany.SetCompanyType(CompanyType.Provider);
                    txtAdult.Text = double.Parse((defaultPolicy.AdultCommission * 100).ToString()).ToString();
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
            //AdultAgentCompany.InitCompanies(CompanyService.GetCompanies(CompanyType.Provider));
            ddlProvince.DataSource = FoundationService.Provinces;
            ddlProvince.DataTextField = "Name";
            ddlProvince.DataValueField = "Code";
            ddlProvince.DataBind();
            ddlProvince.Items.Insert(0, new ListItem("-请选择-", ""));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Valiate())
            {
                try
                {
                    BargainDefaultPolicy policy = saveInfo();
                    PolicyManageService.SaveBargainDefaultPolicy(policy,this.CurrentUser.UserName);
                    RegisterScript("alert('保存成功');window.location.href='bargain_defaultpolicy_manage.aspx';", false);
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "保存");
                }
            }
        }

        private BargainDefaultPolicy saveInfo()
        {
            return new BargainDefaultPolicy()
            {
                Airline = this.ddlAirlines.SelectedValue,
                Province = this.ddlProvince.SelectedValue,
                AdultProvider = this.AdultAgentCompany.CompanyId.Value,
                AdultCommission = decimal.Parse(this.txtAdult.Text) / 100,
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
            if (string.IsNullOrWhiteSpace(this.ddlProvince.SelectedValue))
            {
                ShowMessage("请选择省份！");
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

            return true;
        }

        protected void btnSaveAndContinue_Click(object sender, EventArgs e)
        {
            if (Valiate())
            {
                try
                {
                    BargainDefaultPolicy policy = saveInfo();
                    PolicyManageService.SaveBargainDefaultPolicy(policy, this.CurrentUser.UserName);
                    RegisterScript("alert('保存成功');window.location.href='bargain_defaultpolicy_addormodify.aspx';", false);
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "保存");
                }
            }
        }
    }
}