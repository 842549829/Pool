using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy
{
    public partial class low_price_policy_manage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                InitData();
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void InitData()
        {
            this.txtPubStartTime.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
            this.txtPubEndTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
            //AgentCompany.InitCompanies(CompanyService.GetCompanies(CompanyType.Provider));
            AgentCompany.SetCompanyType(CompanyType.Provider);
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
                var query_list = PolicyManageService.QueryBargainPolicies(GetCondition(), pagination);
                var list = from item in query_list
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
                               //中转城市
                               Transit = item.Transit,
                               //排除日期
                               DepartureDateFilter = item.DepartureDateFilter,
                               //适用班期
                               DepartureWeekFilter = StringOperation.TransferToChinese(item.DepartureWeekFilter),
                               //航班限制
                               Include = item.DepartureFlightsFilterType == LimitType.None ? "不限" : (item.DepartureFlightsFilterType == LimitType.Include ? ("适用：" + item.DepartureFlightsFilter) : "不适用：" + item.DepartureFlightsFilter),
                               //适用舱位
                               Berths = item.Berths,
                               //提前天数
                               BeforehandDays = item.BeforehandDays > -1 ? item.BeforehandDays.ToString() : "",
                               //发布价格
                               PriceInfo = item.PriceType == PriceType.Price ? (item.Price > -1 ? (item.Price.TrimInvaidZero() + "元") : "") : (item.PriceType == PriceType.Discount ? (item.Price * 100).TrimInvaidZero() + "折" : "按返佣"),
                               //返佣信息
                               Commission = (item.IsInternal ? "内部: " + (item.InternalCommission * 100).TrimInvaidZero() + "%<br />" : "")
                                             + "下级: " + (item.SubordinateCommission * 100).TrimInvaidZero() + "%<br />"
                                             + "" + ((item.IsPeer) ? "同行: " + (item.ProfessionCommission * 100).TrimInvaidZero() + "%" : ""),
                               //行程类型
                               TicketType = item.TicketType + "<br />" + item.VoyageType.GetDescription(),
                               //航班日期
                               DepartureDates = item.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item.DepartureDateEnd.ToString("yyyy-MM-dd"),
                               //提供者
                               Opearor = base_policy_manage.GetCompanyName(item.Owner),
                               Sudit = item.Audited == true ? "已审" : "未审",
                               Hang = item.Suspended ? (item.SuspendByPlatform ? "平台挂起" : "公司挂起") : "未挂",
                               LockTip = item.Freezed == true ? "<a href='javascript:unlockpolicy(\"" + item.Id + "\")'>解锁</a>" : "<a href='javascript:lockpolicy(\"" + item.Id + "\")'>锁定</a>",
                               Lock = item.Freezed == true ? "锁定" : "未锁定"
                           };
                this.grv_bargain.DataSource = list;
                this.grv_bargain.DataBind();
                if (list.Any())
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    showempty.Visible = false;
                    grv_bargain.HeaderRow.TableSection = TableRowSection.TableHeader;
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

        private PolicyQueryParameter GetCondition()
        {
            bool @lock = radLock.Checked;
            bool Auditeds = radAudit.Checked;
            PolicyQueryParameter parameter = new PolicyQueryParameter
            {
                Airline = ddlAirline.SelectedValue,
                Departure = txtDeparture.Code,
                Arrival = txtArrival.Code,
                Transit = txtTransit.Code,
                DepartureDateStart = (this.txtStartDate.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtStartDate.Text)),
                DepartureDateEnd = (this.txtEndDate.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtEndDate.Text)),
                //InternalCommissionLower = (this.txtInternalCommissionStart.Text == "" ? (Nullable<Decimal>)null : Decimal.Parse(txtInternalCommissionStart.Text) / 100),
                //InternalCommissionUpper = (this.txtInternalCommissionEnd.Text == "" ? (Nullable<Decimal>)null : Decimal.Parse(txtInternalCommissionEnd.Text) / 100),
                SubordinateCommissionLower = (this.txtSubordinateCommissionStart.Text == "" ? (Nullable<Decimal>)null : Decimal.Parse(txtSubordinateCommissionStart.Text) / 100),
                SubordinateCommissionUpper = (this.txtSubordinateCommissionEnd.Text == "" ? (Nullable<Decimal>)null : Decimal.Parse(txtSubordinateCommissionEnd.Text) / 100),
                ProfessionCommissionLower = (this.txtProfessionCommissionStart.Text == "" ? (Nullable<Decimal>)null : Decimal.Parse(txtProfessionCommissionStart.Text) / 100),
                ProfessionCommissionUpper = (this.txtProfessionCommissionEnd.Text == "" ? (Nullable<Decimal>)null : Decimal.Parse(txtProfessionCommissionEnd.Text) / 100),
                Effective = radYouxiaoAll.Checked ? (int?)null : radYouxiao.Checked ? 1 : 2,
                Suspended = ddlGua.SelectedIndex == 0 ? (int?)null : int.Parse(ddlGua.SelectedValue),
                //PageIndex = pagination.PageIndex,
                //PageSize = pagination.PageSize,
                PubDateStart = DateTime.Parse(txtPubStartTime.Text),
                PubDateEnd = DateTime.Parse(txtPubEndTime.Text),
                Bunks = "",
                OrderBy = 2
                //Suspended = radSuspendedAll.Checked ? (bool?)null : radSuspended.Checked
                //TicketType = ra
            };
            parameter.Owner = AgentCompany.CompanyId;
            if (rbnBspTicket.Checked)
            {
                parameter.TicketType = TicketType.BSP;
            }
            if (rbnB2bTicket.Checked)
            {
                parameter.TicketType = TicketType.B2B;
            }
            if (!radAuditAll.Checked)
            {
                parameter.Audited = Auditeds;
            }
            if (!radLockAll.Checked)
            {
                parameter.Freezed = @lock;
            }
            parameter.VoyageType = string.IsNullOrWhiteSpace(ddlVoyage.SelectedValue) ? (Nullable<VoyageType>)null : (VoyageType)byte.Parse(ddlVoyage.SelectedValue);
            return parameter;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            QueryPolicy(new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = 1,
                GetRowCount = true
            });
            pager.CurrentPageIndex = 1;
        }


        private bool CheckBoxValue()
        {
            if (hidIds.Value == "" || hidIsAll.Value == "")
            {
                ShowMessage("没有选中任何行,执行被取消");
                return false;
            }
            return true;
        }
        private void QueryLowPricePolicy(int pageindex)
        {
            QueryPolicy(new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = pageindex,
                GetRowCount = true
            });
            ClaerHidValue();
        }
        protected void btnSavelock_Click(object sender, EventArgs e)
        {
            if (CheckBoxValue())
            {
                var p = GetCondition();
                p.PolicyType = PolicyType.Bargain;
                var listIds = hidIsAll.Value == "0" ? hidIds.Value.Split(',').Select(Guid.Parse) : PolicyManageService.QueryPolicyIds(p).Select(item => item.Key);
                hidIsAll.Value = "";
                hidIds.Value = "";
                try
                {
                    PolicyManageService.LockPolicy(PolicyType.Bargain, this.CurrentUser.UserName, this.txtlockReason.Text, CurrentCompany.CompanyType == CompanyType.Platform ? OperatorRole.Platform : OperatorRole.User, listIds.ToArray());
                    QueryLowPricePolicy(grv_bargain.PageIndex + 1);
                    ClaerHidValue();
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "锁定政策");
                    QueryLowPricePolicy(grv_bargain.PageIndex + 1);
                    return;
                }
            }
            //if (CheckBoxValue())
            //{
            //    var list_ids = hidIds.Value.Split(',').Select(item => Guid.Parse(item));
            //    PolicyManageService.LockPolicy(PolicyType.Bargain, this.CurrentUser.UserName, this.txtlockReason.Text, CurrentCompany.CompanyType == CompanyType.Platform ? OperatorRole.Platform : OperatorRole.User, list_ids.ToArray());
            //    QueryLowPricePolicy(grv_bargain.PageIndex + 1);
            //}
        }

        protected void btnSaveunlock_Click(object sender, EventArgs e)
        {
            if (CheckBoxValue())
            {
                var p = GetCondition();
                p.PolicyType = PolicyType.Bargain;
                var listIds = hidIsAll.Value == "0" ? hidIds.Value.Split(',').Select(Guid.Parse) : PolicyManageService.QueryPolicyIds(p).Select(item => item.Key);
                hidIsAll.Value = "";
                hidIds.Value = "";
                try
                {
                    PolicyManageService.UnLockPolicy(PolicyType.Bargain, this.CurrentUser.UserName, this.txtlockReason.Text, CurrentCompany.CompanyType == CompanyType.Platform ? OperatorRole.Platform : OperatorRole.User, listIds.ToArray());
                    QueryLowPricePolicy(grv_bargain.PageIndex + 1);
                    ClaerHidValue();
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "解锁政策");
                    QueryLowPricePolicy(grv_bargain.PageIndex + 1);
                    return;
                }
            }
            //if (CheckBoxValue())
            //{
            //    var list_ids = hidIds.Value.Split(',').Select(item => Guid.Parse(item));
            //    PolicyManageService.UnLockPolicy(PolicyType.Bargain, this.CurrentUser.UserName, this.txtunlockReason.Text, CurrentCompany.CompanyType == CompanyType.Platform ? OperatorRole.Platform : OperatorRole.User, list_ids.ToArray());
            //    QueryLowPricePolicy(grv_bargain.PageIndex + 1);
            //}
        }
        private void ClaerHidValue()
        {
            hidIsAll.Value = "";
            hidIds.Value = "";
        }

    }
}