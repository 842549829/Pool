using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using Izual.Data;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class base_policy_view : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                this.txtStartTime.Text = DateTime.Today.ToString("yyyy-MM-dd");

                SettingPolicy setting = CompanyService.GetPolicySetting(this.CurrentCompany.CompanyId);
                if (setting == null)
                {
                    ClientScript.RegisterClientScriptBlock(typeof(Page), "", "alert('还未有任何政策设置信息，不能访问本页面！请联系平台。');window.location.href='/Index.aspx';", true);
                    return;
                }
                if (setting.Airlines == "")
                {
                    ClientScript.RegisterClientScriptBlock(typeof(Page), "", "alert('还没有设置航空公司，请先设置航空公司！请联系平台。');window.location.href='/Index.aspx';", true);
                    return;
                }
                //航空公司
                BindAriline(setting);
                //出发地/到达地
                BindCity(setting);

                bool vis = CompanyService.GetCompanyParameter(this.CurrentCompany.CompanyId).CanReleaseVip;
            }
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (txtDeparture.Code == "")
            {
                ShowMessage("执行被取消,请先选择一个有效的出发城市！");
                return;
            }
            if (txtArrival.Code == "")
            {
                ShowMessage("执行被取消,请先选择一个有效的到达城市！");
                return;
            }
            QueryNormalPolicyView(1);
        }

        private void QueryNormalPolicyView(int pageindex)
        {
            QueryPolicy(new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = pageindex,
                GetRowCount = true
            });
            hidIds.Value = "";
            pager.CurrentPageIndex = pageindex;
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            QueryPolicy(pagination);
        }

        void QueryPolicy(Pagination pagination)
        {
            try
            {
                var paramer = GetCondition(pagination);
                var list_query = ChinaPay.B3B.Service.PolicyMatch.PolicyMatchServcie.GetNormalPolicies(this.CurrentCompany.CompanyId, paramer, item => item.Commission, OrderMode.Descending);
                var list = from item in list_query
                           let item_normal = item.OriginalPolicy as NormalPolicyInfo
                           select new
                           {
                               //政策编号
                               id = item_normal.Id,
                               //航空公司
                               Airline = item_normal.Airline,
                               //出发城市
                               Departure = paramer.Departure,
                               //到达城市
                               Arrival = paramer.Arrival,
                               //中转城市
                               Transit = paramer.Transit,
                               //票证行程
                               TicketType = item_normal.TicketType + "<br />" + item_normal.VoyageType.GetDescription(),
                               //排除日期
                               DepartureDateFilter = item_normal.DepartureDateFilter,
                               //适用班期
                               DepartureWeekFilter = StringOperation.TransferToChinese(item_normal.DepartureWeekFilter),
                               //航班限制
                               Include = item_normal.DepartureFlightsFilterType == LimitType.None ? "不限" : (item_normal.DepartureFlightsFilterType == LimitType.Include ? ("适用：" + item_normal.DepartureFlightsFilter) : "不适用：" + item_normal.DepartureFlightsFilter),
                               //排除航线
                               ExceptAirways = item_normal.ExceptAirways,
                               //适用舱位
                               Berths = item_normal.Berths,
                               //返佣信息
                               Commission = item_normal.Owner == this.CurrentCompany.CompanyId ?
                               (item_normal.IsInternal ? "内部: " + (item_normal.InternalCommission * 100).TrimInvaidZero() + "%" : "")
                                             + "<br />下级: " + (item_normal.SubordinateCommission * 100).TrimInvaidZero() + "%<br />"
                                             + "" + ((item_normal.IsPeer) ? "同行: " + (item_normal.ProfessionCommission * 100).TrimInvaidZero() + "%" : "") : "同行: " + (item.Commission * 100).TrimInvaidZero() + "%",
                               InternalCommission = (item_normal.InternalCommission * 100).TrimInvaidZero(),
                               SubordinateCommission = (item_normal.SubordinateCommission * 100).TrimInvaidZero(),
                               ProfessionCommission = (item_normal.ProfessionCommission * 100).TrimInvaidZero(),
                               VipCommission = (item_normal.VipCommission * 100).TrimInvaidZero(),
                               //去程日期
                               DepartureDates = item_normal.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item_normal.DepartureDateEnd.ToString("yyyy-MM-dd"),
                               //操作人
                               Opearor = item_normal.Owner == this.CurrentCompany.CompanyId ? item_normal.Creator : "",
                               Commission_link = item_normal.Owner == this.CurrentCompany.CompanyId ? "<a href=\"javascript:ModifyCommissionBase('" + item_normal.Id + "','" + (item_normal.InternalCommission * 100).TrimInvaidZero() + "','" + (item_normal.SubordinateCommission * 100).TrimInvaidZero() + "','" + (item_normal.ProfessionCommission * 100).TrimInvaidZero() + "','" + item_normal.IsInternal + "','" + item_normal.IsPeer + "');\">修改返佣</a>" : "同行政策",
                               Policy_link = item_normal.Owner == this.CurrentCompany.CompanyId ? "<a href='base_policy_edit.aspx?Id=" + item_normal.Id + "&Type=Update&Check=view'>修改详细</a>" : ""
                           };
                this.grv_normalLook.DataSource = list;
                this.grv_normalLook.DataBind();
                if (list.Any())
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = list_query.RowCount;
                    }
                    showempty.Visible = false;
                }
                else
                {
                    this.pager.Visible = false;
                    showempty.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private PolicyQueryParameter GetCondition(Pagination pagination)
        {   
            PolicyQueryParameter parameter = new PolicyQueryParameter
            {
                Airline = ddlAirline.SelectedValue,
                Departure = txtDeparture.Code,
                Arrival = txtArrival.Code,
                DepartureDateStart = (this.txtStartTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtStartTime.Text)),
                // DepartureDateEnd = (this.txtEndTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtEndTime.Text)),
                // VoyageType = radOne.Checked ? VoyageType.OneWay : (radRound.Checked ? VoyageType.RoundTrip : (radRoundTip.Checked?VoyageType.OneWayOrRound:VoyageType.TransitWay)),
                PageIndex = pagination.PageIndex,
                PageSize =pager.PageSize
            };
            if (!string.IsNullOrWhiteSpace(this.ddlTicketType.SelectedValue))
                parameter.TicketType = (TicketType)int.Parse(this.ddlTicketType.SelectedValue);
            if (!string.IsNullOrWhiteSpace(this.ddlVoyageType.SelectedValue))
                parameter.VoyageType = (VoyageType)int.Parse(this.ddlVoyageType.SelectedValue);
            return parameter;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            PolicyManageService.UpdateNormalPolicyCommission(Guid.Parse(hidIds.Value), Decimal.Parse(this.txtInternalCommission.Text == "" ? "0" : this.txtInternalCommission.Text) / 100, Decimal.Parse(this.txtSubordinateCommission.Text == "" ? "0" : this.txtSubordinateCommission.Text) / 100, Decimal.Parse(this.txtProfessionCommission.Text == "" ? "0" : this.txtProfessionCommission.Text) / 100, this.CurrentUser.UserName);
            QueryNormalPolicyView(grv_normalLook.PageIndex + 1);
        }

        private void BindAriline(SettingPolicy settingPolicy)
        {
            IEnumerable<string> airlines = settingPolicy.Airlines.Split('/');
            var allAirlines = FoundationService.Airlines;
            foreach (Airline item in allAirlines)
            {
                if (item.Valid && airlines.Contains(item.Code.Value))
                {
                    ListItem listItem = new ListItem(item.Code.Value + "-" + item.ShortName, item.Code.Value);
                    this.ddlAirline.Items.Add(listItem);
                }
            }
        }

        private void BindCity(SettingPolicy settingPolicy)
        {
            var result = new List<Airport>();
            var airports = GetAirport(settingPolicy);
            var allAirports = FoundationService.Airports;
            foreach (Airport item in allAirports)
            {
                if (item.Valid && airports.Contains(item.Code.Value))
                {
                    result.Add(item);
                }
            }
            this.txtDeparture.InitData(result);
        }

        private List<string> GetAirport(SettingPolicy settingPolicy)
        {
            List<string> list = new List<string>();
            if (settingPolicy != null)
            {
                string[] airports = settingPolicy.Departure.Split('/');
                for (int i = 0; i < airports.Length; i++)
                {
                    list.Add(airports[i]);
                }
            }
            return list;
        }
    }
}