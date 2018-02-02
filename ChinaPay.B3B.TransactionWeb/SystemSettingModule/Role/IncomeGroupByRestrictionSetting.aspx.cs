using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class IncomeGroupByRestrictionSetting : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            initData();
            if (!IsPostBack)
            {
                var setting = IncomeGroupService.QueryIncomeGroupDeductGlobalSetting(Guid.Parse(Request.QueryString["id"]));
                if (setting != null)
                {
                    hidSettingId.Value = setting.Id.ToString();
                    radQujian.Checked = setting.Type == PeriodType.Interval;
                    radTongyi.Checked = setting.Type == PeriodType.Unite;

                    txtPrice.Text = setting.Price.ToString();
                    txtRemark.Text = setting.Remark;
                    if (setting.Period.Any())
                    {
                        txtTongyi.Text = (setting.Period.FirstOrDefault().Period * 100).TrimInvaidZero();
                        string ranges = setting.Period.OrderBy(item => item.StartPeriod)
                            .Join(",", item => (item.StartPeriod * 100).TrimInvaidZero() + "|" + ((item.EndPeriod == 0 ? 1 : item.EndPeriod) * 100).TrimInvaidZero() + "|" + (item.Period * 100).TrimInvaidZero());
                        hidRanges.Value = ranges;
                        if (setting.Type == PeriodType.Interval)
                        {
                            qujian.Style.Add("display", "");
                            tongyi.Style.Add("display", "none");
                        }
                        else
                        {
                            qujian.Style.Add("display", "none");
                            tongyi.Style.Add("display", "");
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            IncomeGroupDeductGlobal set = new IncomeGroupDeductGlobal();

            set.CompanyId = CurrentCompany.CompanyId;
            set.IsGlobal = false;
            set.CompanyId = CurrentCompany.CompanyId;
            set.Price = int.Parse(txtPrice.Text == "" ? "0" : txtPrice.Text);
            set.Remark = txtRemark.Text;
            set.Type = radQujian.Checked ? PeriodType.Interval : PeriodType.Unite;
            set.Id = string.IsNullOrEmpty(hidSettingId.Value) ? Guid.NewGuid() : Guid.Parse(hidSettingId.Value);
            if (radQujian.Checked)
            {
                var rangeList = this.hidRanges.Value.Split(',');
                var ranges = new List<IncomeGroupPeriod>();
                foreach (var item in rangeList)
                {
                    string[] range = item.Split('|');
                    IncomeGroupPeriod period = new IncomeGroupPeriod();
                    if (!string.IsNullOrWhiteSpace(range[0]))
                    {
                        period.StartPeriod = decimal.Parse(range[0]) / 100;
                    }
                    if (!string.IsNullOrWhiteSpace(range[1]))
                    {
                        period.EndPeriod = decimal.Parse(range[1]) / 100;
                    }

                    if (!string.IsNullOrWhiteSpace(range[2]))
                    {
                        period.Period = decimal.Parse(range[2]) / 100;
                    }
                    period.DeductId = set.Id;
                    ranges.Add(period);
                }
                set.Period = ranges;
            }
            else
            {
                var ranges = new List<IncomeGroupPeriod>();
                IncomeGroupPeriod period = new IncomeGroupPeriod();
                period.StartPeriod = 0;
                period.EndPeriod = 0;
                period.Period = txtTongyi.Text == "" ? 0 : decimal.Parse(txtTongyi.Text) / 100;
                period.DeductId = set.Id;
                ranges.Add(period);
                set.Period = ranges;
            }
            set.IncomeGroupId = Guid.Parse(Request.QueryString["id"]);

            try
            {
                IncomeGroupService.InsertIncomeGroupDeductSetting(set, CurrentUser.UserName);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "设置收益信息");
                return;
            }
            ////刷新缓存
            //var oem = OEMService.QueryOEM(CurrentCompany.CompanyId);
            //if (oem != null)
            //    FlushRequester.TriggerOEMFlusher(oem.Id);
            RegisterScript("alert('设置收益信息成功！');window.location.href='IncomeGroupList.aspx';", true);

        }
        private void initData()
        {
            var incomeGroup = IncomeGroupService.QueryIncomeGroup(this.CurrentCompany.CompanyId, null);
            this.ddlIncomeGroup.DataSource = incomeGroup;
            this.ddlIncomeGroup.DataTextField = "Name";
            this.ddlIncomeGroup.DataValueField = "Id";
            this.ddlIncomeGroup.DataBind();
            this.ddlIncomeGroup.Items.Insert(0, new ListItem("-请选择-", ""));

            string incomeGroupId = Request.QueryString["id"];
            if (!string.IsNullOrWhiteSpace(incomeGroupId))
            {
                var global = IncomeGroupService.QueryIncomeGroupView(Guid.Parse(incomeGroupId));
                if (global != null)
                {
                    lblName.Text = lblGroupName.Text = global.Name;
                    this.lblUserCount.Text = global.UserCount.ToString();
                    this.lblGroupDescription.Text = global.Description;
                    this.hfdCurrentIncomeGroupId.Value = global.IncomeGroupId.ToString();
                    this.queryUsrList.HRef = "DistributionOEMUserList.aspx?IncomeGroupId=" + global.IncomeGroupId.ToString();
                }
            }
        }
    }
}