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
    public partial class DistributionOEMkoudian : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");

            if (!IsPostBack)
            {
                chkAirlist.DataSource = from item in FoundationService.Airlines select new { Text = item.ShortName, Value = item.Code.Value };
                chkAirlist.DataTextField = "Text";
                chkAirlist.DataValueField = "Value";
                chkAirlist.DataBind();
                //txtDepartureAirports.InitData(); 
                var setting = IncomeGroupService.QueryIncomeGroupDeductSetting(Guid.Parse(Request.QueryString["id"]));
                if (setting != null)
                {
                   hidSettingId.Value = setting.Id.ToString();
                   foreach (var item in setting.Airlines.Split('/'))
                    {
                        foreach (ListItem it in chkAirlist.Items)
                        {
                            if (item == it.Value)
                            {
                                it.Selected = true;
                                break;
                            }
                        }
                    }
                    txtDepartureAirports.InitData(true, setting.Departure.Split('/').ToList());
                    radQujian.Checked = setting.Type == PeriodType.Interval;
                    radTongyi.Checked = setting.Type == PeriodType.Unite;
                    txtPrice.Text = setting.Price.ToString();
                    txtRemark.Text = setting.Remark;
                    if (setting.Period.Any())
                    {
                        txtTongyi.Text = (setting.Period.FirstOrDefault().Period * 100).TrimInvaidZero();
                        string ranges = setting.Period.OrderBy(item => item.StartPeriod)
                            .Join(",", item => (item.StartPeriod * 100).TrimInvaidZero() + "|" + (item.EndPeriod * 100).TrimInvaidZero() + "|" + (item.Period * 100).TrimInvaidZero());
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
            IncomeGroupDeductSetting set = new IncomeGroupDeductSetting();
            set.IncomeGroupId = Guid.Parse(Request.QueryString["id"]);
            set.Price = int.Parse(txtPrice.Text == "" ? "0" : txtPrice.Text);
            set.Remark = txtRemark.Text;
            set.Type = radQujian.Checked ? PeriodType.Interval : PeriodType.Unite;
            string str = "";
            foreach (ListItem item in chkAirlist.Items)
            {
                if (item.Selected)
                {
                    if (str == "")
                    {
                        str += item.Value;
                    }
                    else
                    {
                        str += "/" + item.Value;
                    }
                }
            }
            set.Airlines = str;
            set.Departure = txtDepartureAirports.AirportsCode.ToList().Join("/");
            set.Id = string.IsNullOrEmpty(hidSettingId.Value)?Guid.NewGuid():Guid.Parse(hidSettingId.Value);
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
                period.EndPeriod = 1;
                period.Period = txtTongyi.Text == "" ? 0 : decimal.Parse(txtTongyi.Text) / 100;
                period.DeductId = set.Id;
                ranges.Add(period);
                set.Period = ranges;
            }
            var oem = OEMService.QueryOEM(CurrentCompany.CompanyId);
            try
            {
                IncomeGroupService.InsertIncomeGroupDeductSetting(set, CurrentUser.UserName);
                //刷新缓存
                FlushRequester.TriggerOEMFlusher(oem.Id);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "设置扣点信息");
                return;
            }
            RegisterScript("alert('设置扣点信息成功！');window.location.href='IncomeGroupList.aspx';", true);

        }
    }
}