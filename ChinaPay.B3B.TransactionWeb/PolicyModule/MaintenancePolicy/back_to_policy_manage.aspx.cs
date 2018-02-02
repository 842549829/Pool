using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using Izual.Data;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy
{
    public partial class back_to_policy_manage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtStartTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
                this.txtEndTime.Text = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
                initData();
            }
            this.pager.PageSize = grv_back.PageSize;
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }

        void initData()
        {
            AgentCompany.InitCompanies(CompanyService.GetCompanies(p => p.CompanyType == CompanyType.Provider));
            var airline = from item in Service.FoundationService.Airlines
                          select new
                          {
                              text = item.Code.Value + "-" + item.ShortName,
                              value = item.Code.Value
                          };
            this.ddlAirline.DataSource = airline;
            this.ddlAirline.DataTextField = "text";
            this.ddlAirline.DataValueField = "value";
            this.ddlAirline.DataBind();
            this.ddlAirline.Items.Insert(0, new ListItem { Value = "", Text = "-请选择-" });
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
                var query_list = PolicyManageService.GetRoundTripPolicies(GetCondition(pagination), item => item.CreateTime, OrderMode.Descending);
                var list = from item in query_list
                           select new
                           {
                               //政策编号
                               id = item.Id,
                               //航空公司
                               Airline = item.Airline,
                               //出发城市
                               Departure = item.Departure ,
                               //到达城市
                               Arrival = item.Arrival ,
                               //票证行程
                               TicketType = item.TicketType + "<br />" + item.VoyageType.GetDescription(),
                               //适用班期
                              // DepartureDatesFilter = item.DepartureDatesFilterType == DateMode.Date ? item.DepartureDatesFilter : StringOperation.TransferToChinese(item.DepartureDatesFilter),
                               //适用航班
                               Include = item.DepartureFlightsFilterType == LimitType.None ? "所有" : (item.DepartureFlightsFilterType == LimitType.Include ? item.DepartureFlightsFilter : "&nbsp;"),
                               //排除航班
                               Exclude = item.DepartureFlightsFilterType == LimitType.Exclude ? item.DepartureFlightsFilter : "&nbsp;",
                               ////排除航线
                               //ExceptAirways = item.ExceptAirways.Length > 3 ? item.ExceptAirways.ToString().Substring(0, 3) + "...<div class='DepartureTip'>" + StringOperation.InsertFormat(item.ExceptAirways.ToString(), 32, "<br />") + "</div>" : item.ExceptAirways,
                               //适用舱位
                               Berths = item.Berths,
                               //返佣信息
                               Commission = "内部:" + (item.InternalCommission * 100).TrimInvaidZero()
                                    + "%<br />下级:" + (item.SubordinateCommission * 100).TrimInvaidZero()
                                    + "%<br />同行:" + (item.ProfessionCommission * 100).TrimInvaidZero() + "%",
                               //去程日期
                               DepartureDates = item.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item.DepartureDateEnd.ToString("yyyy-MM-dd"),
                               //回程日期
                               ReturnDates = item.VoyageType == VoyageType.OneWay ? "" : (item.ReturnDateStart.Value.ToString("yyyy-MM-dd") + "<br />" + item.ReturnDateEnd.Value.ToString("yyyy-MM-dd")),
                               //供应方
                               Opearor = GetCompanyName(item.Owner),
                               Sudit = item.Audited == true ? "已审" : "未审",
                               Lock = item.Freezed == true ? "锁定" : "未锁定",
                               LockTip = item.Freezed == true ? "<a href='javascript:unlockpolicy(\"" + item.Id + "\")'>解锁</a>" : "<a href='javascript:lockpolicy(\"" + item.Id + "\")'>锁定</a>",
                               Hang = item.Suspended ? ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy.BasePolicy.GetHungInfo(item.Airline, item.Owner) : "未挂"
                           };
                this.grv_back.DataSource = list;
                this.grv_back.DataBind();
                if (list.Any())
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = query_list.RowCount;
                    }
                    showempty.Visible = false;
                    grv_back.HeaderRow.TableSection = TableRowSection.TableHeader;
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
            bool @lock = radLock.Checked;
            bool Auditeds = Audited.Checked;
            PolicyQueryParameter parameter = new PolicyQueryParameter
            {
                Airline = ddlAirline.SelectedValue,
                Departure = txtDeparture.Code,
                Arrival = txtArrival.Code,
                Owner = AgentCompany.CompanyId,
                DepartureDateStart = (this.txtStartTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtStartTime.Text)),
                DepartureDateEnd = (this.txtEndTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtEndTime.Text)),
                InternalCommissionLower = (this.txtInternalCommissionStart.Text == "" ? (Nullable<Decimal>)null : Decimal.Parse(txtInternalCommissionStart.Text) / 100),
                InternalCommissionUpper = (this.txtInternalCommissionEnd.Text == "" ? (Nullable<Decimal>)null : Decimal.Parse(txtInternalCommissionEnd.Text) / 100),
                SubordinateCommissionLower = (this.txtSubordinateCommissionStart.Text == "" ? (Nullable<Decimal>)null : Decimal.Parse(txtSubordinateCommissionStart.Text) / 100),
                SubordinateCommissionUpper = (this.txtSubordinateCommissionEnd.Text == "" ? (Nullable<Decimal>)null : Decimal.Parse(txtSubordinateCommissionEnd.Text) / 100),
                ProfessionCommissionLower = (this.txtProfessionCommissionStart.Text == "" ? (Nullable<Decimal>)null : Decimal.Parse(txtProfessionCommissionStart.Text) / 100),
                ProfessionCommissionUpper = (this.txtProfessionCommissionEnd.Text == "" ? (Nullable<Decimal>)null : Decimal.Parse(txtProfessionCommissionEnd.Text) / 100),
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize
                //TicketType = ra
            };
            if (!AuditedAll.Checked)
            {
                parameter.Audited = Auditeds;
            }
            if (!radLockAll.Checked)
            {
                parameter.Freezed = @lock;
            }
            if (!radall.Checked)
            {
                parameter.TicketType = ticket;
            }
            parameter.Owner = AgentCompany.CompanyId;
            return parameter;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            QueryBackPolicy(1);
            pager.CurrentPageIndex = 1;
        }

        private void QueryBackPolicy(int pageindex)
        {
            QueryPolicy(new Pagination
            {
                PageSize = grv_back.PageSize,
                PageIndex = pageindex,
                GetRowCount = true
            });
        }

        private bool CheckBoxValue()
        {
            if (hidIds.Value == "")
            {
                ShowMessage("没有选中任何行,执行被取消");
                return false;
            }
            return true;
        }
        protected void btnSavelock_Click(object sender, EventArgs e)
        {
            if (CheckBoxValue())
            {
                var list_ids = hidIds.Value.Split(',').Select(item => Guid.Parse(item));
                PolicyManageService.LockPolicy(PolicyType.RoundTrip, this.CurrentUser.UserName, this.txtlockReason.Text, list_ids.ToArray());
                QueryBackPolicy(grv_back.PageIndex + 1);
            }
        }

        protected void btnSaveunlock_Click(object sender, EventArgs e)
        {
            if (CheckBoxValue())
            {
                var list_ids = hidIds.Value.Split(',').Select(item => Guid.Parse(item));
                PolicyManageService.UnLockPolicy(PolicyType.RoundTrip, this.CurrentUser.UserName, this.txtunlockReason.Text, list_ids.ToArray());
                QueryBackPolicy(grv_back.PageIndex + 1);
            }
        }

    }
}