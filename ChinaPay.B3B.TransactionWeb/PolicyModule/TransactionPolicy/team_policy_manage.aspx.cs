using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class team_policy_manage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                this.txtPubStartTime.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                this.txtPubEndTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
                initData();
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        void initData()
        {
            //var employees = EmployeeService.QueryEmployees(CurrentCompany.CompanyId);
            //this.ddlOperator.DataTextField = "UserName";
            //this.ddlOperator.DataValueField = "UserName";
            //this.ddlOperator.DataSource = employees;
            //this.ddlOperator.DataBind();
            //this.ddlOperator.Items.Insert(0, new ListItem { Value = "", Text = "-请选择-" });

            SettingPolicy setting = CompanyService.GetPolicySetting(this.CurrentCompany.CompanyId);
            if (setting == null)
            {
                RegisterScript("alert('还未有任何政策设置信息，不能访问本页面！请联系平台。');window.location.href='/TicketDefault.aspx';", true);
                return;
            }
            if (setting.Airlines == "")
            {
                RegisterScript("alert('还没有设置航空公司，请先设置航空公司！请联系平台。');window.location.href='/TicketDefault.aspx';", true);
                return;
            }
            //航空公司
            BindAriline(setting);
            //出发地/到达地
            BindCity(setting);
            this.ddlOffice.DataSource = CompanyService.QueryOfficeNumbers(this.CurrentCompany.CompanyId).Select(o => o.Number);
            this.ddlOffice.DataBind();
            this.ddlOffice.Items.Insert(0, new ListItem { Value = "", Text = "-请选择-" });
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
                var querylist = PolicyManageService.QueryTeamPolicies(GetCondition(), pagination);
                var list = from item in querylist
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
                               //票证行程
                               TicketType = item.TicketType + "<br />" + item.VoyageType.GetDescription(),
                               //排除日期
                               DepartureDateFilter = item.DepartureDateFilter,
                               //适用班期
                               DepartureWeekFilter = StringOperation.TransferToChinese(item.DepartureWeekFilter),
                               //航班限制
                               Include = item.DepartureFlightsFilterType == LimitType.None ? "不限" : (item.DepartureFlightsFilterType == LimitType.Include ? ("适用：" + item.DepartureFlightsFilter) : "不适用：" + item.DepartureFlightsFilter),
                               //排除航线
                               ExceptAirways = item.ExceptAirways,
                               //适用舱位
                               Berths = item.Berths,
                               //返佣信息
                               Commission = (item.IsInternal ? "内部: " + (item.InternalCommission * 100).TrimInvaidZero() + "%<br />" : "")
                                             + "下级: " + (item.SubordinateCommission * 100).TrimInvaidZero() + "%<br />"
                                             + "" + ((item.IsPeer) ? "同行: " + (item.ProfessionCommission * 100).TrimInvaidZero() + "%" : ""),
                               InternalCommission = (item.InternalCommission * 100).TrimInvaidZero(),
                               SubordinateCommission = (item.SubordinateCommission * 100).TrimInvaidZero(),
                               ProfessionCommission = (item.ProfessionCommission * 100).TrimInvaidZero(),
                               // VipCommission = (item.VipCommission * 100).TrimInvaidZero(),
                               //去程日期
                               DepartureDates = item.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item.DepartureDateEnd.ToString("yyyy-MM-dd"),
                               //操作人
                               Opearor = item.Creator,
                               Sudit = item.Audited ? "已审" : "未审",
                               SuditName = item.Audited ? "UnAudited" : "Audited",
                               SuditTip = item.Audited ? "取消审核" : "确认审核",
                               //同行
                               item.IsPeer,
                               //内部
                               item.IsInternal,
                               Hang = item.Suspended ? (item.SuspendByPlatform ? "平台挂起" : "公司挂起") : "未挂"
                           };
                this.grv_normal.DataSource = list;
                this.grv_normal.DataBind();
                if (list.Any())
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
            TicketType ticket;
            bool Audited;
            ticket = radB2B.Checked ? TicketType.B2B : TicketType.BSP;
            Audited = radAudit.Checked;
            PolicyQueryParameter parameter = new PolicyQueryParameter
            {
                Airline = ddlAirline.SelectedValue,
                OfficeCode = ddlOffice.SelectedValue,
                Departure = txtDeparture.Code,
                Transit = txtTransit.Code,
                Arrival = txtArrival.Code,
                DepartureDateStart = (this.txtStartTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtStartTime.Text)),
                DepartureDateEnd = (this.txtEndTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtEndTime.Text)),
                //PageIndex = pagination.PageIndex,
                //PageSize = pagination.PageSize,
                //Creator = ddlOperator.SelectedValue,
                Effective = ddlYouxiao.SelectedIndex == 0 ? (int?)null : ddlYouxiao.SelectedIndex,
                VoyageType = ddlVoyage.SelectedValue == "" ? (Nullable<VoyageType>)null : (VoyageType)byte.Parse(ddlVoyage.SelectedValue),
                Owner = this.CurrentCompany.CompanyId,
                PubDateStart = DateTime.Parse(txtPubStartTime.Text),
                PubDateEnd = DateTime.Parse(txtPubEndTime.Text),
                Bunks = txtBunks.Text,
                OrderBy = 1,
                PolicyType = PolicyType.Team
            };
            if (!radall.Checked)
            {
                parameter.TicketType = ticket;
            }
            if (!radAuditAll.Checked)
            {
                parameter.Audited = Audited;
            }
            return parameter;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            QueryTeamPolicy(1);
        }

        private void QueryTeamPolicy(int pageindex)
        {
            QueryPolicy(new Pagination
            {
                PageSize = pager.PageSize,
                PageIndex = pageindex,
                GetRowCount = true
            });
            pager.CurrentPageIndex = pageindex;
            hidIds.Value = ""; hidIsAll.Value = "";
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("./team_policy_publish.htm");
        }

        protected void grv_normal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //确认审核
            if (e.CommandName == "Audited")
            {
                PolicyManageService.Audit(PolicyType.Team, this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                QueryTeamPolicy(pager.CurrentPageIndex);
            }
            //取消审核
            if (e.CommandName == "UnAudited")
            {
                PolicyManageService.CancelAudit(PolicyType.Team, this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                QueryTeamPolicy(pager.CurrentPageIndex);
            }
            //删除政策
            if (e.CommandName == "del")
            {
                try
                {
                    PolicyManageService.DeleteTeamPolicy(this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                    QueryTeamPolicy(pager.CurrentPageIndex);
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "删除政策");
                }
            }
        }

        protected void btnAudited_Click(object sender, EventArgs e)
        {
            if (GetCheckBoxValue())
            {
                var list_ids = hidIsAll.Value == "0" ? hidIds.Value.Split(',').Select(Guid.Parse) : PolicyManageService.QueryPolicyIds(GetCondition()).Select(item => item.Key);
                try
                {
                    PolicyManageService.Audit(PolicyType.Team, this.CurrentUser.UserName, list_ids.ToArray());
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "审核");
                }
                QueryTeamPolicy(pager.CurrentPageIndex);
                ClaerHidValue();
            }
        }

        protected void btnUnAudited_Click(object sender, EventArgs e)
        {
            if (GetCheckBoxValue())
            {
                var list_ids = hidIsAll.Value == "0" ? hidIds.Value.Split(',').Select(Guid.Parse) : PolicyManageService.QueryPolicyIds(GetCondition()).Select(item => item.Key);
                try
                {
                    PolicyManageService.CancelAudit(PolicyType.Team, this.CurrentUser.UserName, list_ids.ToArray());
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "取消审核");
                }
                QueryTeamPolicy(pager.CurrentPageIndex);
                ClaerHidValue();
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (GetCheckBoxValue())
            {
                var p = PolicyManageService.QueryPolicyIds(GetCondition());
                if (hidIsAll.Value == "1")
                {
                    int count = p.Count(item => item.Value == true);
                    if (count != 0)
                    {
                        ShowMessage("删除政策失败，其中存在 " + count + " 条 已审核。全部取消审核才能删除！");
                        return;
                    }
                }
                var list_ids = hidIsAll.Value == "0" ? hidIds.Value.Split(',').Select(Guid.Parse) : p.Select(item => item.Key);
                try
                {
                    PolicyManageService.DeleteTeamPolicy(this.CurrentUser.UserName, list_ids.ToArray());
                    QueryTeamPolicy(pager.CurrentPageIndex);
                    ClaerHidValue();
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "删除政策");
                }
            }
        }

        private bool GetCheckBoxValue()
        {
            if (hidIds.Value == "" || hidIsAll.Value == "")
            {
                ShowMessage("没有选中任何行,执行被取消");
                return false;
            }
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            PolicyManageService.UpdateTeamPolicyCommission(Guid.Parse(hidIds.Value), Decimal.Parse(string.IsNullOrWhiteSpace(this.txtInternalCommission.Text) ? "0" : this.txtInternalCommission.Text) / 100, Decimal.Parse(string.IsNullOrWhiteSpace(this.txtSubordinateCommission.Text) ? "0" : this.txtSubordinateCommission.Text) / 100, Decimal.Parse(string.IsNullOrWhiteSpace(this.txtProfessionCommission.Text) ? "0" : this.txtProfessionCommission.Text) / 100, this.CurrentUser.UserName);
            QueryTeamPolicy(pager.CurrentPageIndex);
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
            this.ddlAirline.Items.Insert(0, new ListItem("全部", ""));
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
        private void ClaerHidValue()
        {
            hidIsAll.Value = "";
            hidIds.Value = "";
        }
    }
}