using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core;
using Izual.Data;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy
{
    public partial class policy_set_manage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                this.txtStartDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                this.txtEndDate.Text = DateTime.Today.AddMonths(1).ToString("yyyy-MM-dd");
                this.ddlAirlines.DataSource = from item in FoundationService.Airlines
                                              select new
                                              {
                                                  ShortName = item.Code + "-" + item.ShortName,
                                                  Code = item.Code
                                              };
                ddlAirlines.DataTextField = "ShortName";
                ddlAirlines.DataValueField = "Code";
                this.ddlAirlines.DataBind();
                ddlAirlines.Items.Insert(0, new ListItem("-请选择-", ""));
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            queryPolicySetting(pagination);
        }

        private void queryPolicySetting(Pagination pagination)
        {
            try
            {
                var queryList = PolicyManageService.GetPolicySettings(getCondition(),pagination);
                var list = from item in queryList
                           select new
                           {
                               Id = item.Id,
                               Airline = item.Airline,
                               //出发城市
                               Departure = item.Departure,
                               //到达城市
                               Arrival = item.Arrivals,
                               // VoyageType = item.VoyageType.GetDescription(),
                               Commission = GetCommisson(item.Periods),
                               Berths = item.Berths,
                               EffectiveTime = item.EffectiveTimeStart.ToShortDateString() + "<br />" + item.EffectiveTimeEnd.ToShortDateString(),
                               Creator = item.Creator,
                               LastModifyTime = item.LastModifyTime,
                               Remark = item.Remark
                           };
                this.dataSource.DataSource = list;
                this.dataSource.DataBind();
                if (pagination.RowCount > 0)
                {
                    this.pager.Visible = true;
                    emptyDataInfo.Visible = false;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    dataSource.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    this.pager.Visible = false;
                    emptyDataInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }
        private string GetCommisson(IEnumerable<PolicySettingPeriod> item)
        {
            item = item.OrderBy(i => i.PeriodStart);
            string str = "";
            foreach (PolicySettingPeriod i in item)
            {
                if (i.PeriodStart == 0)
                {
                    if (i.Rebate == 0)
                    {
                        str += double.Parse((i.PeriodStart * 100).ToString()).ToString() + "≤返点≤" + double.Parse((i.PeriodEnd * 100).ToString()).ToString() + "&nbsp;&nbsp;" + 0 + "%" + "<br />";
                    }
                    else
                    {
                        if (i.Rebate > 0)
                        {
                            str += double.Parse((i.PeriodStart * 100).ToString()).ToString() + "≤返点≤" + double.Parse((i.PeriodEnd * 100).ToString()).ToString() + ",扣点:" + double.Parse((i.Rebate * 100).ToString()).ToString() + "%" + "<br />";
                        }
                    }
                }
                else
                {
                    if (i.Rebate == 0)
                    {
                        str += double.Parse((i.PeriodStart * 100).ToString()).ToString() + "<返点≤" + double.Parse((i.PeriodEnd * 100).ToString()).ToString() + "&nbsp;&nbsp;" + 0 + "%" + "<br />";
                    }
                    else
                    {
                        if (i.Rebate > 0)
                        {
                            str += double.Parse((i.PeriodStart * 100).ToString()).ToString() + "<返点≤" + double.Parse((i.PeriodEnd * 100).ToString()).ToString() + ",扣点:" + double.Parse((i.Rebate * 100).ToString()).ToString() + "%" + "<br />";
                        }
                    }
                }
                if (i.Rebate < 0)
                {
                    str = "贴点:" + double.Parse((-i.Rebate * 100).ToString()).ToString() + "%" + "<br />";
                }
            }
            return str;
        }
        private PolicySettingQueryParameter getCondition()
        {
            PolicySettingQueryParameter parameter = new PolicySettingQueryParameter();
            if (!string.IsNullOrWhiteSpace(this.ddlAirlines.SelectedValue))
            {
                parameter.Airline = this.ddlAirlines.SelectedValue;
            }
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                parameter.EffectiveTimeStart = DateTime.Parse(this.txtStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                parameter.EffectiveTimeEnd = DateTime.Parse(this.txtEndDate.Text).AddDays(1).AddMilliseconds(-3);
            }
            if (!string.IsNullOrWhiteSpace(txtDeparture.Code))
            {
                parameter.Departure = txtDeparture.Code;
            }
            if (!string.IsNullOrWhiteSpace(txtArrival.Code))
            {
                parameter.Arrival = txtArrival.Code;
            }
            if (dropTieType.SelectedIndex != 0)
            {
                parameter.Rebate = dropTieType.SelectedIndex == 1;
            }
            else
            {
                parameter.Rebate = null;
            }
            //parameter.Rebate = decimal.Parse(dropTieType.SelectedValue);
            return parameter;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination()
                {
                    GetRowCount = true,
                    PageIndex = 1,
                    PageSize = pager.PageSize
                };
                queryPolicySetting(pagination);
            }
            else
            {
                this.pager.CurrentPageIndex = 1;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<Guid> ids = new List<Guid>();

            foreach (GridViewRow row in dataSource.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkBox");
                Label lbl = row.FindControl("lblId") as Label;
                if (chk.Checked)
                {
                    Guid id = Guid.Parse(lbl.Text);
                    ids.Add(id);
                }
            }
            if (ids.Count > 0)
            {
                PolicyManageService.DeletePolicySetting(this.CurrentUser.UserName,
                    ids.ToArray());
                btnQuery_Click(sender, e);
            }
        }

        protected void dataSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "del")
            {
                try
                {
                    PolicyManageService.DeletePolicySetting(this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                    btnQuery_Click(sender, e);
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "删除");
                }
            }
        }
    }
}