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
    public partial class special_policy_manage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                this.txtPubStartTime.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                this.txtPubEndTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
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
                var specialType = Enum.GetValues(typeof(SpecialProductType)) as SpecialProductType[];
                foreach (var item in specialType)
                {
                    ddlSpecialType.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
                }
                ddlSpecialType.Items.Insert(0, new ListItem("-请选择-", ""));
                //AgentCompany.InitCompanies(CompanyService.GetCompanies(CompanyType.Provider | CompanyType.Supplier));
                AgentCompany.SetCompanyType(CompanyType.Provider | CompanyType.Supplier);
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
            QueryPolicy(pagination);
        }

        void QueryPolicy(Pagination pagination)
        {
            try
            {
                var list_query = PolicyManageService.QuerySpecialPolicies(GetCondition(), pagination);
                var list = from item in list_query
                           select new
                           {
                               //航空公司 	出发城市 	到达城市 	适用班期 	适用航班 	排除航班 	 	提前时间 	发布价格 	航班日期 	供应方 	产品审核 	平台审核 	是否锁定 	是否挂起 	
                               //政策编号
                               id = item.Id,
                               //航空公司
                               Airline = item.Airline,
                               //出发城市
                               Departure = item.Departure,
                               //到达城市
                               Arrival = item.Arrival,
                               //特殊票类型
                               SpecialType = item.Type.GetDescription(),
                               //排除日期
                               DepartureDateFilter = item.DepartureDateFilter,
                               //适用班期
                               DepartureWeekFilter = StringOperation.TransferToChinese(item.DepartureWeekFilter),
                               //航班限制
                               Include = item.DepartureFlightsFilterType == LimitType.None ? "不限" : (item.DepartureFlightsFilterType == LimitType.Include ? ("适用：" + item.DepartureFlightsFilter) : "不适用：" + item.DepartureFlightsFilter),
                               //提前时间
                               BeforehandDays = item.BeforehandDays > -1 ? item.BeforehandDays + "天" : "",
                               //发布价格
                               Price = item.Price,
                               //价格/直减
                               PriceInfo = item.PriceType == PriceType.Price ? (item.Price == -1 ? "" : item.Price.TrimInvaidZero() + "元") : (item.PriceType == PriceType.Subtracting ? (item.Price * 100).TrimInvaidZero() + "%" : ""),
                               //航班日期
                               DepartureDates = item.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item.DepartureDateEnd.ToString("yyyy-MM-dd"),

                               Commission = ((item.InternalCommission < 0 && !item.IsInternal) ? "" : ("内部: " + (item.PriceType == PriceType.Price ? item.InternalCommission.TrimInvaidZero() + "元<br />" : (item.InternalCommission * 100).TrimInvaidZero() + "%<br />")))
                                              + (item.SubordinateCommission < 0 ? "" : ("下级: " + (item.PriceType == PriceType.Price ? item.SubordinateCommission.TrimInvaidZero() + "元<br />" : (item.SubordinateCommission * 100).TrimInvaidZero() + "%<br />"))  )
                                              + ((item.ProfessionCommission < 0 && !item.IsPeer) ? "" : ("同行: " + (item.PriceType == PriceType.Price ? item.ProfessionCommission.TrimInvaidZero() + "元" : (item.ProfessionCommission * 100).TrimInvaidZero() + "%"))),
                               //供应方
                               Opearor = base_policy_manage.GetCompanyName(item.Owner),
                               //产品审核
                               Sudit = item.Audited == true ? "已审" : "未审",
                               //平台审核
                               State = item.PlatformAudited == true ? "已审" : "未审",
                               //是否锁定
                               Lock = item.Freezed == true ? "锁定" : "未锁定",

                               LockTip = item.Freezed == true ? "<a href='javascript:unlockpolicy(\"" + item.Id + "\")'>解锁</a>" : "<a href='javascript:lockpolicy(\"" + item.Id + "\")'>锁定</a>",
                               SuditName = item.PlatformAudited == true ? "UnAudited" : "Audited",
                               SuditTip = item.PlatformAudited == true ? "取消审核" : "确认审核",
                               //挂起
                               Hang = item.Suspended ? (item.SuspendByPlatform ? "平台挂起" : "公司挂起") : "未挂"
                           };
                this.grv_special.DataSource = list;
                this.grv_special.DataBind();
                if (list.Any())
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    showempty.Visible = false;
                    grv_special.HeaderRow.TableSection = TableRowSection.TableHeader;
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
            PolicyQueryParameter parameter = new PolicyQueryParameter
            {
                Airline = ddlAirline.SelectedValue,
                Departure = txtDeparture.Code,
                Arrival = txtArrival.Code,
                DepartureDateStart = (this.txtStartTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtStartTime.Text)),
                DepartureDateEnd = (this.txtEndTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtEndTime.Text)),
                Effective = radYouxiaoAll.Checked ? (int?)null : radYouxiao.Checked ? 1 : 2,
                Suspended = ddlGua.SelectedIndex == 0 ? (int?)null : int.Parse(ddlGua.SelectedValue),
                Owner = AgentCompany.CompanyId,
                //PageIndex = pagination.PageIndex,
                //PageSize = pagination.PageSize,
                PubDateStart = DateTime.Parse(txtPubStartTime.Text),
                PubDateEnd = DateTime.Parse(txtPubEndTime.Text),
                Bunks = "",
                OrderBy = 2
                //Suspended = radSuspendedAll.Checked ? (bool?)null : radSuspended.Checked
                //TicketType = ra
            };
            if (!radLockAll.Checked)
            {
                parameter.Freezed = @lock;
            }
            if (!radAutoAll.Checked)
            {
                parameter.Audited = radAutoed.Checked;
            }
            if (!radSuoyou.Checked)
            {
                parameter.PlatformAudited = radYijing.Checked;
            }
            parameter.SpecialProductType = string.IsNullOrWhiteSpace(ddlSpecialType.SelectedValue) ? (Nullable<SpecialProductType>)null : (SpecialProductType)int.Parse(ddlSpecialType.SelectedValue);
            return parameter;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            QuerySpecialPolicy(1);
        }

        private void QuerySpecialPolicy(int pageindex)
        {
            QueryPolicy(new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = pageindex,
                GetRowCount = true
            });
            pager.CurrentPageIndex = 1;
            ClaerHidValue();
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
        protected void btnSavelock_Click(object sender, EventArgs e)
        {
            if (CheckBoxValue())
            {
                var p = GetCondition();
                p.PolicyType = PolicyType.Special;
                var listIds = hidIsAll.Value == "0" ? hidIds.Value.Split(',').Select(Guid.Parse) : PolicyManageService.QueryPolicyIds(p).Select(item => item.Key);
                hidIsAll.Value = "";
                hidIds.Value = "";
                try
                {
                    PolicyManageService.LockPolicy(PolicyType.Special, this.CurrentUser.UserName, this.txtlockReason.Text, CurrentCompany.CompanyType == CompanyType.Platform ? OperatorRole.Platform : OperatorRole.User, listIds.ToArray());
                    QuerySpecialPolicy(grv_special.PageIndex + 1);
                    ClaerHidValue();
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "锁定政策");
                    QuerySpecialPolicy(grv_special.PageIndex + 1);
                    return;
                }
            } 
        }

        protected void btnSaveunlock_Click(object sender, EventArgs e)
        {
            if (CheckBoxValue())
            {
                var p = GetCondition();
                p.PolicyType = PolicyType.Special;
                var listIds = hidIsAll.Value == "0" ? hidIds.Value.Split(',').Select(Guid.Parse) : PolicyManageService.QueryPolicyIds(p).Select(item => item.Key);
                hidIsAll.Value = "";
                hidIds.Value = "";
                try
                {
                    PolicyManageService.UnLockPolicy(PolicyType.Special, this.CurrentUser.UserName, this.txtlockReason.Text, CurrentCompany.CompanyType == CompanyType.Platform ? OperatorRole.Platform : OperatorRole.User, listIds.ToArray());
                    QuerySpecialPolicy(grv_special.PageIndex + 1);
                    ClaerHidValue();
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "解锁政策");
                    QuerySpecialPolicy(grv_special.PageIndex + 1);
                    return;
                }
            }  
        }
        protected void grv_specical_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //确认审核
            if (e.CommandName == "Audited")
            {
                PolicyManageService.AuditSpecialPolicy(this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                QuerySpecialPolicy(grv_special.PageIndex + 1);
            }
            //取消审核
            if (e.CommandName == "UnAudited")
            {
                PolicyManageService.CancelAuditSpecialPolicy(this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                QuerySpecialPolicy(grv_special.PageIndex + 1);
            }
        }
        private void ClaerHidValue()
        {
            hidIsAll.Value = "";
            hidIds.Value = "";
        }
    }
}