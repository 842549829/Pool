using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using Izual.Data;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.PolicyMatch;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class back_to_policy_view : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtStartTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
                this.txtEndTime.Text = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");


                SettingPolicy setting = CompanyService.GetPolicySetting(this.CurrentCompany.CompanyId);

                if (setting == null)
                {
                    RegisterScript("alert('还未有任何政策设置信息，不能访问本页面！请联系平台。');window.location.href='/Index.aspx';", true);
                    return;
                }
                if (setting.Airlines == "")
                {
                    RegisterScript("alert('还没有设置航空公司，请先设置航空公司！请联系平台。');window.location.href='/Index.aspx';", true);
                    return;
                }
                string[] str_ids = setting.Airlines.Split('/');
                var airline = from item in ChinaPay.B3B.Service.FoundationService.Airlines
                              where item.Valid && str_ids.Contains(item.Code.Value)
                              select new
                              {
                                  Code = item.Code,
                                  Name = item.Code + "-" + item.ShortName
                              };
                this.ddlAirline.DataSource = airline;
                this.ddlAirline.DataTextField = "Name";
                this.ddlAirline.DataValueField = "Code";
                this.ddlAirline.DataBind();
                this.txtDeparture.InitData(ChinaPay.B3B.Service.FoundationService.Airports.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));
            }
            this.pager.PageSize = grv_back.PageSize;
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
            QueryBackPolicy(1);
        }

        private void QueryBackPolicy(int pageindex)
        {
            var pagination = new Pagination()
            {
                PageSize = grv_back.PageSize,
                PageIndex = pageindex,
                GetRowCount = true
            };
            QueryPolicy(pagination);
            hidIds.Value = "";
            pager.CurrentPageIndex = pageindex;
        }
        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = grv_back.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            QueryPolicy(pagination);
        }
        void QueryPolicy(Pagination pagination)
        {
            try
            {
                var list_query = PolicyMatchServcie.GetRoundTripPolicies(this.CurrentCompany.CompanyId, GetCondition(pagination), item => item.OriginalPolicy.CreateTime, OrderMode.Descending);

                var list = from item in list_query
                           let item_round = item.OriginalPolicy as RoundTripPolicyInfo
                           select new
                           {
                               //政策编号
                               id = item_round.Id,
                               //航空公司
                               Airline = item_round.Airline,
                               //出发城市
                               Departure = item_round.Departure,
                               //到达城市
                               Arrival = item_round.Arrival,
                               //票证行程
                               TicketType = item_round.TicketType + "<br />" + item_round.VoyageType.GetDescription(),
                               //适用班期
                              // DepartureDatesFilter = item_round.DepartureDatesFilterType == DateMode.Date ? (item_round.DepartureDatesFilter) : (StringOperation.TransferToChinese(item_round.DepartureDatesFilter)),
                               //适用航班
                               Include = item_round.DepartureFlightsFilterType == LimitType.None ? "所有" : (item_round.DepartureFlightsFilterType == LimitType.Include ? (item_round.DepartureFlightsFilter) : "&nbsp;"),
                               //排除航班
                               Exclude = item_round.DepartureFlightsFilterType == LimitType.Exclude ? (item_round.DepartureFlightsFilter) : "&nbsp;",
                               //适用舱位
                               Berths = item_round.Berths,
                               //价格
                               Price = item_round.Price.TrimInvaidZero(),
                               PriceInfo = item_round.Price.TrimInvaidZero(),
                               //返佣信息
                               Commission = item_round.Owner == this.CurrentCompany.CompanyId ? (
                               BasePolicy.CanHaveSubordinate(CurrentCompany.CompanyId) ? "内部: " + (item_round.InternalCommission * 100).TrimInvaidZero()
                                    + "%<br />下级: " + (item_round.SubordinateCommission * 100).TrimInvaidZero()
                                    + "%<br />同行: " + (item_round.ProfessionCommission * 100).TrimInvaidZero() + "%" : "下级: " + (item_round.SubordinateCommission * 100).TrimInvaidZero()
                                    + "%<br />同行: " + (item_round.ProfessionCommission * 100).TrimInvaidZero() + "%") : "同行: " + (item.Commission * 100).TrimInvaidZero() + "%",
                               InternalCommission = (item_round.InternalCommission * 100).TrimInvaidZero(),
                               SubordinateCommission = (item_round.SubordinateCommission * 100).TrimInvaidZero(),
                               ProfessionCommission = (item_round.ProfessionCommission * 100).TrimInvaidZero(),
                               DepartureDates = item_round.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item_round.DepartureDateEnd.ToString("yyyy-MM-dd"),
                               //回程日期
                               ReturnDates = item_round.VoyageType == VoyageType.OneWay ? "" : (item_round.ReturnDateStart.Value.ToString("yyyy-MM-dd") + "<br />" + item_round.ReturnDateEnd.Value.ToString("yyyy-MM-dd")),
                               Commission_link = item_round.Owner == this.CurrentCompany.CompanyId ? "<a href=\"javascript:ModifyCommissionBack('" + item_round.Id + "','" + (item_round.InternalCommission * 100).TrimInvaidZero() + "','" + (item_round.SubordinateCommission * 100).TrimInvaidZero() + "','" + (item_round.ProfessionCommission * 100).TrimInvaidZero() + "','" + item_round.Price.TrimInvaidZero() + "','" + BasePolicy.CanHaveSubordinate(CurrentCompany.CompanyId) + "');\">修改返佣</a>" : "异地政策",
                               Policy_link = item_round.Owner == this.CurrentCompany.CompanyId ? "<a href='back_to_policy_edit.aspx?Id=" + item_round.Id + "&Type=Update&Check=view'>修改详细</a>" : "&nbsp;",
                               //操作人
                               Opearor = item_round.Owner == this.CurrentCompany.CompanyId ? item_round.Creator : "&nbsp;",
                           };
                this.grv_back.DataSource = list;
                this.grv_back.DataBind();
                if (list.Count() > 0)
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
            TicketType ticket;
            ticket = radB2B.Checked ? TicketType.B2B : TicketType.BSP;
            PolicyQueryParameter parameter = new PolicyQueryParameter
            {
                Airline = ddlAirline.SelectedValue,
                Departure = txtDeparture.Code,
                Arrival = txtArrival.Code,
                DepartureDateStart = (this.txtStartTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtStartTime.Text)),
                DepartureDateEnd = (this.txtEndTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtEndTime.Text)),
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize
                //TicketType = ra
            };
            if (!radall.Checked)
            {
                parameter.TicketType = ticket;
            }

            return parameter;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            PolicyManageService.UpdateRoundTripPolicyCommission(Guid.Parse(hidIds.Value), Decimal.Parse(this.txtPrice.Text == "" ? "0" : this.txtPrice.Text), Decimal.Parse(this.txtInternalCommission.Text == "" ? "0" : this.txtInternalCommission.Text) / 100, Decimal.Parse(this.txtSubordinateCommission.Text == "" ? "0" : this.txtSubordinateCommission.Text) / 100, Decimal.Parse(this.txtProfessionCommission.Text == "" ? "0" : this.txtProfessionCommission.Text) / 100, this.CurrentUser.UserName);
            QueryBackPolicy(grv_back.PageIndex + 1);
        }
    }
}