using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Policy;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy
{
    public partial class policy_set_addormodify : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                InitDataBind();
                string policySettingId = Request.QueryString["Id"];
                if (!string.IsNullOrWhiteSpace(policySettingId))
                {
                    hidAddOrUpdate.Value = "0";
                    PolicySettingInfo policySettingInfo = PolicyManageService.GetPolicySetting(Guid.Parse(policySettingId));
                    this.ddlAirLine.SelectedValue = policySettingInfo.Airline;
                    this.txtDeparture.Code = policySettingInfo.Departure;
                    this.AirLines.AirportsCode = policySettingInfo.Arrivals.Split('/');
                    this.txtStartTime.Text = policySettingInfo.EffectiveTimeStart.ToString("yyyy-MM-dd");
                    this.txtEndTime.Text = policySettingInfo.EffectiveTimeEnd.ToString("yyyy-MM-dd");
                    this.txtRemark.InnerText = policySettingInfo.Remark;
                    radEnable.Checked = policySettingInfo.Enable;
                    radDisable.Checked = !policySettingInfo.Enable;
                    //取满足航空公司条件的舱位
                    BindBunks(policySettingInfo);
                    //获取区域值
                    BindRanges(policySettingInfo);
                    sel.Visible = false;
                    //txtTiedianStart.Text = policySettingInfo.MountStart.HasValue ? policySettingInfo.MountStart.Value.ToString("hh:mm") : "";
                    //txtTiedianEnd.Text = policySettingInfo.MountEnd.HasValue ? policySettingInfo.MountEnd.Value.ToString("hh:mm") : "";
                }
            }
        }

        private void BindBunks(PolicySettingInfo policySettingInfo)
        {
            this.hidBunks.Value = policySettingInfo.Berths;
        }

        private void BindRanges(PolicySettingInfo policySettingInfo)
        {
            if (policySettingInfo.Periods.FirstOrDefault().Rebate > 0)
            {
                navTip.InnerText = " 扣点";
                string ranges = policySettingInfo.Periods.OrderBy(item => item.PeriodStart)
                    .Join(",", item => (item.PeriodStart * 100).TrimInvaidZero() + "|" + (item.PeriodEnd * 100).TrimInvaidZero() + "|" + ((item.Rebate > 0 ? item.Rebate : (-item.Rebate)) * 100).TrimInvaidZero());
                hidRanges.Value = ranges;
                hidKoudianOrTiedian.Value = "1";
                maxtiedian.Visible = false;
                tiedian.Visible = false;
                //tiedianTime.Visible = false;
                //this.radKoudian.Checked = true;
            }
            else
            {
                txtTiedian.Text = policySettingInfo.Periods.Join("", item => (-item.Rebate * 100).TrimInvaidZero());
                txtMaxTiedian.Text = policySettingInfo.Periods.Join("", item => (-item.MaxRebate * 100).TrimInvaidZero());
                navTip.InnerText = " 贴点";
                koudian.Visible = false;
                hidKoudianOrTiedian.Value = "2";
                //this.radTiedian.Checked = false;
            }
        }

        private void InitDataBind()
        {
            this.txtStartTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
            this.txtEndTime.Text = DateTime.Today.AddMonths(1).ToString("yyyy-MM-dd");
            this.ddlAirLine.DataSource = from item in FoundationService.Airlines
                                         select new
                                         {
                                             Text = item.Code.Value + "-" + item.ShortName,
                                             Value = item.Code.Value
                                         };
            this.ddlAirLine.DataTextField = "Text";
            this.ddlAirLine.DataValueField = "Value";
            this.ddlAirLine.DataBind();
            this.ddlAirLine.Items.Insert(0, new ListItem("-请选择-", ""));
            AirLines.InitData(true, Service.FoundationService.Airports);
            //txtTiedianStart.Text = "08:00";
            //txtTiedianEnd.Text = "18:00";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Valiate())
            {
                if (hidAddOrUpdate.Value == "")
                {
                    try
                    {
                        PolicySettingInfo setting = SaveInfo();
                        PolicyManageService.AddPolicySetting(setting, this.CurrentUser.UserName);
                        RegisterScript("alert('添加成功');window.location.href='policy_set_manage.aspx';", true);
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex, "添加");
                    }
                }
                else
                {
                    try
                    {
                        PolicySettingInfo setting = SaveInfo();
                        setting.Creator = this.CurrentUser.Name;
                        string policySettingId = Request.QueryString["Id"];
                        if (!string.IsNullOrWhiteSpace(policySettingId))
                        {
                            setting.Id = Guid.Parse(policySettingId);
                            PolicyManageService.UpdatePolicySetting(setting, this.CurrentUser.UserName);
                            RegisterScript("alert('修改成功');window.location.href='policy_set_manage.aspx';", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex, "修改");
                    }
                }
            }
        }

        private PolicySettingInfo SaveInfo()
        {
            PolicySettingInfo setting = new PolicySettingInfo();
            if (!string.IsNullOrWhiteSpace(ddlAirLine.SelectedValue))
            {
                setting.Airline = ddlAirLine.SelectedValue;
            }
            setting.Departure = txtDeparture.Code;
            setting.Arrivals = AirLines.AirportsCode.Join("/");
            if (!string.IsNullOrWhiteSpace(txtStartTime.Text))
            {
                setting.EffectiveTimeStart = DateTime.Parse(this.txtStartTime.Text);
            }
            if (!string.IsNullOrWhiteSpace(txtEndTime.Text))
            {
                setting.EffectiveTimeEnd = DateTime.Parse(this.txtEndTime.Text).AddDays(1).AddMilliseconds(-3);
            }
            setting.Berths = this.hidBunks.Value;

            //扣点
            if (hidKoudianOrTiedian.Value.Trim() == "1")
            {
                var rangeList = this.hidRanges.Value.Split(',');
                var ranges = new List<PolicySettingPeriod>();
                foreach (var item in rangeList)
                {
                    string[] range = item.Split('|');
                    PolicySettingPeriod period = new PolicySettingPeriod();
                    if (!string.IsNullOrWhiteSpace(range[0]))
                    {
                        period.PeriodStart = decimal.Parse(range[0]) / 100;
                    }
                    if (!string.IsNullOrWhiteSpace(range[1]))
                    {
                        period.PeriodEnd = decimal.Parse(range[1]) / 100;
                    }

                    if (!string.IsNullOrWhiteSpace(range[2]))
                    {
                        period.Rebate = decimal.Parse(range[2]) / 100;
                    }
                    period.MaxRebate = 0;
                    ranges.Add(period);
                }
                setting.Periods = ranges;
            }
            else
            {
                //贴点
                setting.Periods = new List<PolicySettingPeriod>() { new PolicySettingPeriod() { PeriodStart = 0, PeriodEnd = 0, Rebate = -(decimal.Parse(txtTiedian.Text) / 100), MaxRebate = -(decimal.Parse(txtMaxTiedian.Text) / 100) } };
                //setting.MountStart = DateTime.Parse(txtTiedianStart.Text);
                //setting.MountEnd = DateTime.Parse(txtTiedianEnd.Text); 
            }
            setting.Enable = radEnable.Checked;
            setting.RebateType = hidKoudianOrTiedian.Value.Trim() == "1";
            setting.Remark = this.txtRemark.InnerText;
            return setting;
        }

        private bool Valiate()
        {
            if (this.ddlAirLine.SelectedValue == "")
            {
                ShowMessage("请选择航空公司！");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.txtDeparture.Code))
            {
                ShowMessage("请选择出发地！");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.AirLines.AirportsCode.ToList()[0]))
            {
                ShowMessage("请选择目的地！");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.hidBunks.Value))
            {
                ShowMessage("请选择适用舱位！");
                return false;
            }
            if (this.txtRemark.Value.Trim().Length > 100)
            {
                ShowMessage("备注信息位数不能超过100位！");
                return false;
            }
            return true;
        }
    }
}