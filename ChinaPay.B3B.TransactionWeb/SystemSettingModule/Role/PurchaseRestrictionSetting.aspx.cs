using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using System.Text.RegularExpressions;
using ChinaPay.Core.Extension;
using System.Web.Script.Serialization;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class PurchaseRestrictionSetting : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                initData();
            }
        }


        private void initData()
        {
            var incomeGroup = IncomeGroupService.QueryIncomeGroup(this.CurrentCompany.CompanyId, null);
            this.ddlIncomeGroup.DataSource = incomeGroup;
            this.ddlIncomeGroup.DataTextField = "Name";
            this.ddlIncomeGroup.DataValueField = "Id";
            this.ddlIncomeGroup.DataBind();
            this.ddlIncomeGroup.Items.Insert(0, new ListItem("-请选择-", ""));


            string strAirlines = "";
            int i = 0;
            foreach (var item in FoundationService.Airlines)
            {
                i++;
                strAirlines += "<input type='checkbox' id='checkbox_" + i + "' value='" + item.Code.Value + "' /><label for='checkbox_" + i + "'>" + item.ShortName + "</label>&nbsp;&nbsp;";
                if (i % 15 == 0)
                {
                    strAirlines += "<br />";
                }
            }
            divAirlinelist.InnerHtml = strAirlines;


            txtDepartureAirports.InitData(true, Service.FoundationService.Airports.Where(item => item.Valid));

            string incomeGroupId = Request.QueryString["IncomeGroupId"];
            if (!string.IsNullOrWhiteSpace(incomeGroupId))
            {
                var incomeGroupView = IncomeGroupService.QueryIncomeGroupView(Guid.Parse(incomeGroupId));
                lblGroupNameTitle.Text = this.lblGroupName.Text = incomeGroupView.Name;
                this.lblUserCount.Text = incomeGroupView.UserCount.ToString();
                this.lblGroupDescription.Text = incomeGroupView.Description;
                this.hfdCurrentIncomeGroupId.Value = incomeGroupId;
                this.queryUsrList.HRef = "DistributionOEMUserList.aspx?IncomeGroupId=" + incomeGroupId;
                var global = PurchaseLimitationService.QueryPurchaseLimitationGroupInfo(Guid.Parse(incomeGroupId));
                if (global != null)
                {
                    string strLimitation = "";
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    strLimitation = jss.Serialize(global.Limitation);
                    strLimitation = "var limitation=" + strLimitation;
                    this.hfdLimitation.Value = strLimitation;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (valiate())
            {
                try
                {
                    var view = savaData();
                    string incomeGroupId = Request.QueryString["IncomeGroupId"];
                    if (!string.IsNullOrWhiteSpace(incomeGroupId))
                    {
                        var global = PurchaseLimitationService.QueryPurchaseLimitationGroupInfo(Guid.Parse(incomeGroupId));
                        if (global == null)
                        {
                            view.IncomeGroupId = Guid.Parse(incomeGroupId);
                            PurchaseLimitationService.InsertPurchaseLimitationGroup(view, this.CurrentUser.UserName);
                            RegisterScript("alert('添加成功');window.location.href='IncomeGroupList.aspx';", false);
                        }
                        else
                        {
                            view.IncomeGroupId = Guid.Parse(incomeGroupId);
                            PurchaseLimitationService.UpdatePurchaseRestrictionSetting(view, this.CurrentUser.UserName);
                            RegisterScript("alert('修改成功');window.location.href='IncomeGroupList.aspx';", false);
                        }
                    }
                }
                catch (Exception ex)
                {

                    ShowExceptionMessage(ex, "保存");
                }
            }
        }

        private ChinaPay.B3B.Service.Organization.Domain.PurchaseLimitationGroup savaData()
        {
            var global = new ChinaPay.B3B.Service.Organization.Domain.PurchaseLimitationGroup();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var keys = Request.Form.AllKeys;
            List<string> name = new List<string>();
            global.Limitation = new List<PurchaseLimitation>();
            foreach (var item in keys)
            {
                if (Regex.IsMatch(item, "^Data_\\d{1,}$"))
                {
                    name.Add(item);
                    var data = jss.Deserialize<PurchaseLimitation>(Request.Form[item]);
                    data.LimitationGroupId = global.Id;
                    global.Limitation.Add(data);
                }
            }
            global.Limitation = BindId(global.Limitation);
            return global;
        }

        private IList<PurchaseLimitation> BindId(IList<PurchaseLimitation> limitation)
        {
            foreach (var item in limitation)
            {
                item.Id = Guid.NewGuid();
                foreach (var it in item.Rebate)
                {
                    it.LimitationId = item.Id;
                }
            }
            return limitation;
        }

        private bool valiate()
        {
            if (this.rbnPurchaseOnlySelfNormalPolicy.Checked)
            {
                if (!Regex.IsMatch(this.txtDefaultRebateAdultNormalPolicy.Text, "^\\d{1,2}(.\\d{1})?$"))
                {
                    ShowMessage("下级默认返点（普通政策）格式错误,只支持保留一位小数");
                    return false;
                }
            }


            if (this.rbnPurchaseOnlySelfBargainPolicy.Checked)
            {
                if (!Regex.IsMatch(this.txtDefaultRebateAdultBargainPolicy.Text, "^\\d{1,2}(.\\d{1})?$"))
                {
                    ShowMessage("下级默认返点（特价政策）格式错误,只支持保留一位小数");
                    return false;
                }
            }
            return true;
        }

    }
}