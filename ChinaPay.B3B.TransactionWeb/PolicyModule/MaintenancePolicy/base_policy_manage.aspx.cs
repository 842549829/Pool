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
    public partial class base_policy_manage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                this.txtPubStartTime.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                this.txtPubEndTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
                InitData();
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        void InitData()
        {
            AgentCompany.SetCompanyType(CompanyType.Provider);
            //AgentCompany.InitCompanies(CompanyService.GetCompanies(CompanyType.Provider));
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
                var listQuery = PolicyManageService.QueryNormalPolicies(GetCondition(), pagination);
                var list = from item in listQuery
                           select new
                           {
                               //政策编号
                               id = item.Id,
                               //航空公司
                               item.Airline,
                               //出发城市
                               item.Departure,
                               //到达城市
                               item.Arrival,
                               //中转城市
                               item.Transit,
                               //票证行程
                               TicketType = item.TicketType + "<br />" + item.VoyageType.GetDescription(),
                               //排除日期
                               DepartureDateFilter = item.DepartureDateFilter,
                               //适用班期
                               DepartureWeekFilter = StringOperation.TransferToChinese(item.DepartureWeekFilter),
                               //航班限制
                               Include = item.DepartureFlightsFilterType == LimitType.None ? "不限" : (item.DepartureFlightsFilterType == LimitType.Include ? ("适用：" + item.DepartureFlightsFilter) : "不适用：" + item.DepartureFlightsFilter),
                               //排除航线
                               item.ExceptAirways,
                               //适用舱位
                               item.Berths,
                               //返佣信息
                               Commission = (item.IsInternal ? "内部: " + (item.InternalCommission * 100).TrimInvaidZero() + "%<br />" : "")
                                             + "下级: " + (item.SubordinateCommission * 100).TrimInvaidZero() + "%<br />"
                                             + "" + ((item.IsPeer) ? "同行: " + (item.ProfessionCommission * 100).TrimInvaidZero() + "%" : ""),
                               //去程日期
                               DepartureDates = item.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item.DepartureDateEnd.ToString("yyyy-MM-dd"),
                               //操作人
                               Opearor = base_policy_manage.GetCompanyName(item.Owner)
                               + (item.DiscountPoint ? "<br />已扣点" : "")
                               + (item.MountPoint ? "<br />已贴点" : ""),
                               Sudit = item.Audited ? "已审" : "未审",
                               Lock = item.Freezed ? "锁定" : "未锁",
                               LockTip = item.Freezed ? "<a href='javascript:unlockpolicy(\"" + item.Id + "\")'>解锁</a>" : "<a href='javascript:lockpolicy(\"" + item.Id + "\")'>锁定</a>",
                               Hang = item.Suspended ? (item.SuspendByPlatform ? "平台挂起" : "公司挂起") : "未挂"
                           };
                this.grv_normal.DataSource = list;
                this.grv_normal.DataBind();
                if (listQuery.Any())
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    showempty.Visible = false;
                    grv_normal.HeaderRow.TableSection = TableRowSection.TableHeader;
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
            TicketType ticket = radB2B.Checked ? TicketType.B2B : TicketType.BSP;
            bool auditeds = Audited.Checked;
            var parameter = new PolicyQueryParameter
            {
                Airline = ddlAirline.SelectedValue,
                Departure = txtDeparture.Code,
                Arrival = txtArrival.Code,
                Transit = txtTransit.Code,
                //InternalCommissionLower = (this.txtInternalCommissionStart.Text == "" ? (decimal?)null : Decimal.Parse(txtInternalCommissionStart.Text) / 100),
                //InternalCommissionUpper = (this.txtInternalCommissionEnd.Text == "" ? (decimal?)null : Decimal.Parse(txtInternalCommissionEnd.Text) / 100),
                SubordinateCommissionLower = (this.txtSubordinateCommissionStart.Text == "" ? (decimal?)null : Decimal.Parse(txtSubordinateCommissionStart.Text) / 100),
                SubordinateCommissionUpper = (this.txtSubordinateCommissionEnd.Text == "" ? (decimal?)null : Decimal.Parse(txtSubordinateCommissionEnd.Text) / 100),
                ProfessionCommissionLower = (this.txtProfessionCommissionStart.Text == "" ? (decimal?)null : Decimal.Parse(txtProfessionCommissionStart.Text) / 100),
                ProfessionCommissionUpper = (this.txtProfessionCommissionEnd.Text == "" ? (decimal?)null : Decimal.Parse(txtProfessionCommissionEnd.Text) / 100),
                DepartureDateStart = (this.txtStartTime.Text == "" ? (DateTime?)null : DateTime.Parse(txtStartTime.Text)),
                DepartureDateEnd = (this.txtEndTime.Text == "" ? (DateTime?)null : DateTime.Parse(txtEndTime.Text)),
                VoyageType = ddlVoyage.SelectedValue == "" ? (VoyageType?)null : (VoyageType)byte.Parse(ddlVoyage.SelectedValue),
                Effective = radYouxiaoAll.Checked ? (int?)null : radYouxiao.Checked ? 1 : 2,
                Suspended = ddlGua.SelectedIndex == 0 ? (int?)null : int.Parse(ddlGua.SelectedValue),
                //PageIndex = pagination.PageIndex,
                //PageSize = pagination.PageSize,
                PubDateStart = DateTime.Parse(txtPubStartTime.Text),
                PubDateEnd = DateTime.Parse(txtPubEndTime.Text),
                Bunks = "",
                OrderBy = 2
                // Suspended = radSuspendedAll.Checked ? (bool?)null : radSuspended.Checked
                //TicketType = ra
            };
            if (!radall.Checked)
            {
                parameter.TicketType = ticket;
            }

            parameter.Owner = AgentCompany.CompanyId;
            if (!radLockAll.Checked)
            {
                parameter.Freezed = @lock;
            }
            if (!AuditedAll.Checked)
            {
                parameter.Audited = auditeds;
            }
            return parameter;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            QueryNormalPolicy(1);
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
        private void QueryNormalPolicy(int pageindex)
        {
            QueryPolicy(new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = pageindex,
                GetRowCount = true
            }); ClaerHidValue();
        }



        protected void btnSavelock_Click(object sender, EventArgs e)
        {
            if (CheckBoxValue())
            {
                var p = GetCondition();
                p.PolicyType = PolicyType.Normal;
                var listIds = hidIsAll.Value == "0" ? hidIds.Value.Split(',').Select(Guid.Parse) : PolicyManageService.QueryPolicyIds(p).Select(item => item.Key); 
                hidIsAll.Value = "";
                hidIds.Value = "";
                try
                {
                    PolicyManageService.LockPolicy(PolicyType.Normal, this.CurrentUser.UserName, this.txtlockReason.Text, CurrentCompany.CompanyType == CompanyType.Platform ? OperatorRole.Platform : OperatorRole.User, listIds.ToArray());
                    QueryNormalPolicy(grv_normal.PageIndex + 1); ClaerHidValue();
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "锁定政策");
                    QueryNormalPolicy(grv_normal.PageIndex + 1);
                    return;
                }
            }
        }

        protected void btnSaveunlock_Click(object sender, EventArgs e)
        {
            if (CheckBoxValue())
            {
                var p = GetCondition();
                p.PolicyType = PolicyType.Normal;
                var listIds = hidIsAll.Value == "0" ? hidIds.Value.Split(',').Select(Guid.Parse) : PolicyManageService.QueryPolicyIds(p).Select(item => item.Key);
                hidIsAll.Value = "";
                hidIds.Value = "";
                try
                {
                    PolicyManageService.UnLockPolicy(PolicyType.Normal, this.CurrentUser.UserName, this.txtunlockReason.Text, CurrentCompany.CompanyType == CompanyType.Platform ? OperatorRole.Platform : OperatorRole.User, listIds.ToArray());
                    QueryNormalPolicy(grv_normal.PageIndex + 1);
                    ClaerHidValue();
                }
                catch (Exception ex)
                {
                    QueryNormalPolicy(grv_normal.PageIndex + 1);
                    ShowExceptionMessage(ex, "解锁政策");
                    QueryNormalPolicy(grv_normal.PageIndex + 1);
                    return;
                }
            }
        }
        public static string GetCompanyName(Guid id)
        {
            var detailinfo = CompanyService.GetCompanyInfo(id);
            if (detailinfo == null)
            {
                return "";
            }
            else
            {
                return detailinfo.AbbreviateName;
            }
        }
        private void ClaerHidValue()
        {
            hidIsAll.Value = "";
            hidIds.Value = "";
        }

    }
}