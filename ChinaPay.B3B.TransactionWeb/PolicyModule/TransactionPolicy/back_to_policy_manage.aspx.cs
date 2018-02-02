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
using ChinaPay.B3B.TransactionWeb.PublicClass;
using Izual.Data;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Foundation.Domain;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
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
            var employees = Service.Organization.CompanyService.GetEmployees(CurrentCompany.CompanyId);
            this.ddlOperator.DataTextField = "UserName";
            this.ddlOperator.DataValueField = "UserName";
            this.ddlOperator.DataSource = employees;
            this.ddlOperator.DataBind();
            this.ddlOperator.Items.Insert(0, new ListItem { Value = "", Text = "-请选择-" });

            SettingPolicy setting = CompanyService.GetPolicySetting(this.CurrentCompany.CompanyId);

            if (setting == null)
            {
                RegisterScript("alert('还未有任何政策设置信息，不能访问本页面！请联系平台。');window.location.href='/Default.aspx';", true);
                return;
            }
            if (setting.Airlines == "")
            {
                RegisterScript("alert('还没有设置航空公司，请先设置航空公司！请联系平台。');window.location.href='/Default.aspx';", true);
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
            this.ddlAirline.Items.Insert(0, new ListItem { Value = "", Text = "-请选择-" });

            this.txtDeparture.InitData(ChinaPay.B3B.Service.FoundationService.Airports.Where(item => setting.Departure.Split('/').Contains(item.Code.Value)));
            //this.txtArrival.InitData(ChinaPay.B3B.Service.FoundationService.Airports.Where(item => setting.Airlines.Split('/').Contains(item.Code.Value)));
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
                var list_query = PolicyManageService.GetRoundTripPolicies(GetCondition(pagination), item => item.CreateTime, OrderMode.Descending);
                var list = from item in list_query
                           select new
                           {
                               //政策编号
                               id = item.Id,
                               //航空公司
                               Airline = item.Airline,
                               //出发城市
                               Departure = item.Departure,
                               //到达城市
                               Arrival = item.Arrival,
                               //票证行程
                               TicketType = item.TicketType + "<br />" + item.VoyageType.GetDescription(),
                               //适用班期
                             //  DepartureDatesFilter = item.DepartureDatesFilterType == DateMode.Date ? item.DepartureDatesFilter : StringOperation.TransferToChinese(item.DepartureDatesFilter),
                               //适用航班
                               Include = item.DepartureFlightsFilterType == LimitType.None ? "所有" : (item.DepartureFlightsFilterType == LimitType.Include ? item.DepartureFlightsFilter : "&nbsp;"),
                               //排除航班
                               Exclude = item.DepartureFlightsFilterType == LimitType.Exclude ? item.DepartureFlightsFilter : "&nbsp;",
                               //适用舱位
                               Berths = item.Berths,
                               //价格
                               Price = item.Price.TrimInvaidZero(),
                               //返佣信息
                               Commission = BasePolicy.CanHaveSubordinate(CurrentCompany.CompanyId) ?
                                         "内部: " + (item.InternalCommission * 100).TrimInvaidZero()
                                        + "%<br />下级: " + (item.SubordinateCommission * 100).TrimInvaidZero()
                                        + "%<br />同行: " + (item.ProfessionCommission * 100).TrimInvaidZero() + "%" :
                                            "下级: " + (item.SubordinateCommission * 100).TrimInvaidZero()
                                        + "%<br />同行: " + (item.ProfessionCommission * 100).TrimInvaidZero() + "%",
                               InternalCommission = (item.InternalCommission * 100).TrimInvaidZero(),
                               SubordinateCommission = (item.SubordinateCommission * 100).TrimInvaidZero(),
                               ProfessionCommission = (item.ProfessionCommission * 100).TrimInvaidZero(),
                               //去程日期
                               DepartureDates = item.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item.DepartureDateEnd.ToString("yyyy-MM-dd"),
                               //回程日期
                               ReturnDates = item.VoyageType == VoyageType.OneWay ? "" : (item.ReturnDateStart.Value.ToString("yyyy-MM-dd") + "<br />" + item.ReturnDateEnd.Value.ToString("yyyy-MM-dd")),
                               //供应方
                               Opearor = item.Creator,
                               Sudit = item.Audited ? "已审" : "未审",
                               SuditName = item.Audited ? "UnAudited" : "Audited",
                               SuditTip = item.Audited ? "取消审核" : "确认审核",
                               CanHaveSubordinate = BasePolicy.CanHaveSubordinate(CurrentCompany.CompanyId),
                               Hang = item.Suspended ? BasePolicy.GetHungInfo(item.Airline,this.CurrentCompany.CompanyId) : "未挂"
                           };
                this.grv_back.DataSource = list;
                this.grv_back.DataBind();
                if (list.Any())
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = list_query.RowCount;
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
            bool Auditeds = Audited.Checked;
            PolicyQueryParameter parameter = new PolicyQueryParameter
            {
                Airline = ddlAirline.SelectedValue,
                DepartureDateStart = (this.txtStartTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtStartTime.Text)),
                DepartureDateEnd = (this.txtEndTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtEndTime.Text)),
                Creator = ddlOperator.SelectedValue,
                Owner = this.CurrentCompany.CompanyId,
                Arrival = txtArrival.Code,
                Departure = txtDeparture.Code,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize
            };
            if (!radall.Checked)
            {
                parameter.TicketType = ticket;
            }
            if (!AuditedAll.Checked)
            {
                parameter.Audited = Auditeds;
            }
            return parameter;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            QueryBackPolicy(1);
        }

        private void QueryBackPolicy(int pageindex)
        {
            QueryPolicy(new Pagination
            {
                PageSize = grv_back.PageSize,
                PageIndex = pageindex,
                GetRowCount = true
            });
            pager.CurrentPageIndex = pageindex;
            hidIds.Value = "";
        }
        protected void btnAudited_Click(object sender, EventArgs e)
        {
            if (GetCheckBoxValue())
            {
                var list_ids = hidIds.Value.Split(',').Select(Guid.Parse);
                PolicyManageService.Audit(PolicyType.RoundTrip, this.CurrentUser.UserName, list_ids.ToArray());
                QueryBackPolicy(pager.CurrentPageIndex);
            }
        }

        protected void btnUnAudited_Click(object sender, EventArgs e)
        {
            if (GetCheckBoxValue())
            {
                var list_ids = hidIds.Value.Split(',').Select(Guid.Parse);
                PolicyManageService.CancelAudit(PolicyType.RoundTrip, this.CurrentUser.UserName, list_ids.ToArray());
                QueryBackPolicy(pager.CurrentPageIndex);
            }

        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (GetCheckBoxValue())
            {
                var list_ids = hidIds.Value.Split(',').Select(Guid.Parse).ToArray();
                PolicyManageService.DeleteRoundTripPolicy(this.CurrentUser.UserName, list_ids);
                QueryBackPolicy(pager.CurrentPageIndex);
            }
        }

        private bool GetCheckBoxValue()
        {
            if (hidIds.Value == "")
            {
                ShowMessage("没有选中任何行,执行被取消");
                return false;
            }
            return true;
        }

        protected void grv_back_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //确认审核
            if (e.CommandName == "Audited")
            {
                PolicyManageService.Audit(PolicyType.RoundTrip, this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                QueryBackPolicy(pager.CurrentPageIndex);
            }
            //取消审核
            if (e.CommandName == "UnAudited")
            {
                PolicyManageService.CancelAudit(PolicyType.RoundTrip, this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                QueryBackPolicy(pager.CurrentPageIndex);
            }
            //删除政策
            if (e.CommandName == "del")
            {
                PolicyManageService.DeleteRoundTripPolicy(this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                QueryBackPolicy(pager.CurrentPageIndex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            PolicyManageService.UpdateRoundTripPolicyCommission(Guid.Parse(hidIds.Value), Decimal.Parse(this.txtPrice.Text == "" ? "0" : this.txtPrice.Text), Decimal.Parse(this.txtInternalCommission.Text == "" ? "0" : this.txtInternalCommission.Text) / 100, Decimal.Parse(this.txtSubordinateCommission.Text == "" ? "0" : this.txtSubordinateCommission.Text) / 100, Decimal.Parse(this.txtProfessionCommission.Text == "" ? "0" : this.txtProfessionCommission.Text) / 100, this.CurrentUser.UserName);
            QueryBackPolicy(pager.CurrentPageIndex);
            try
            {
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改返佣信息");
            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            Response.Redirect("./back_to_policy_publish.htm");
        }
         
    }
}