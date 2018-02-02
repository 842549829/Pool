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
using Izual.Data;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class low_price_policy_view : BasePage
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
                    RegisterScript("alert('还未有任何政策设置信息，不能访问本页面！请联系平台。');window.location.href='/Index.aspx';", true);
                    return;
                }
                if (setting.Airlines == "")
                {
                    RegisterScript("alert('还没有设置航空公司，请先设置航空公司！请联系平台。');window.location.href='/Index.aspx';", true);
                    return;
                }
                BindAriline(setting);
                BindCity(setting);
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
                var paramer = GetCondition(pagination);
                var query_list = ChinaPay.B3B.Service.PolicyMatch.PolicyMatchServcie.GetBargainPolicies(this.CurrentCompany.CompanyId, paramer, item => item.Rebate, OrderMode.Descending);
                var list = from item in query_list
                           let item_bargin = item.OriginalPolicy as BargainPolicyInfo
                           select new
                           {
                               //政策编号
                               id = item_bargin.Id,
                               //航空公司
                               Airline = item_bargin.Airline,
                               //出发城市
                               Departure = paramer.Departure,
                               //到达城市
                               Arrival = paramer.Arrival,
                               //中转城市
                               Transit = paramer.Transit,
                               Ticket = item_bargin.TicketType + "<br />" + item_bargin.VoyageType.GetDescription(),
                               //排除日期
                               DepartureDateFilter = item_bargin.DepartureDateFilter,
                               //适用班期
                               DepartureWeekFilter = StringOperation.TransferToChinese(item_bargin.DepartureWeekFilter),
                               //航班限制
                               Include = item_bargin.DepartureFlightsFilterType == LimitType.None ? "不限" : (item_bargin.DepartureFlightsFilterType == LimitType.Include ? ("适用：" + item_bargin.DepartureFlightsFilter) : "不适用：" + item_bargin.DepartureFlightsFilter),
                               //发布价格
                               Price = item_bargin.PriceType == PriceType.Price ? item_bargin.Price.TrimInvaidZero() : (item_bargin.Price * 100).TrimInvaidZero(),
                               //发布价格
                               PriceInfo = item_bargin.PriceType == PriceType.Price ? (item_bargin.Price > -1 ? (item_bargin.Price.TrimInvaidZero() + "元") : "") : (item_bargin.PriceType == PriceType.Discount ? (item_bargin.Price * 100).TrimInvaidZero() + "折" : "按返佣"),
                               //发布折扣
                               DisCount = item_bargin.PriceType == PriceType.Discount ? (item_bargin.Price * 100).TrimInvaidZero() : "",
                               //价格类型
                               PriceTypes = item_bargin.PriceType,
                               //提前天数
                               BeforehandDays = "最少提前：" + (item_bargin.BeforehandDays > -1 ? item_bargin.BeforehandDays.ToString() + "天" : "") + "<br />最多提前：" + (item_bargin.MaxBeforehandDays > -1 ? item_bargin.MaxBeforehandDays.ToString() + "天" : ""),
                               Berths = item_bargin.Berths,
                               //返佣信息
                               Commission = item_bargin.Owner == this.CurrentCompany.CompanyId ? (
                               (item_bargin.IsInternal ? "内部: " + (item_bargin.InternalCommission * 100).TrimInvaidZero() + "%<br />" : "")
                                             + "下级: " + (item_bargin.SubordinateCommission * 100).TrimInvaidZero() + "%<br />"
                                             + "" + ((item_bargin.IsPeer) ? "同行: " + (item_bargin.ProfessionCommission * 100).TrimInvaidZero() + "%" : "")) : "同行: " + (item.Commission * 100).TrimInvaidZero() + "%",
                               InternalCommission = (item_bargin.InternalCommission * 100).TrimInvaidZero(),
                               SubordinateCommission = (item_bargin.SubordinateCommission * 100).TrimInvaidZero(),
                               ProfessionCommission = (item_bargin.ProfessionCommission * 100).TrimInvaidZero(),
                               //去程日期
                               DepartureDates = item_bargin.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item_bargin.DepartureDateEnd.ToString("yyyy-MM-dd"),
                               Commission_link = item_bargin.Owner == this.CurrentCompany.CompanyId ? "<a href=\"javascript:ModifyCommissionLow('" + item_bargin.Id + "','" + (item_bargin.InternalCommission * 100).TrimInvaidZero() + "','" + (item_bargin.SubordinateCommission * 100).TrimInvaidZero() + "','" + (item_bargin.ProfessionCommission * 100).TrimInvaidZero() + "','" + (item_bargin.Price).TrimInvaidZero() + "','" + (item_bargin.Price * 100).TrimInvaidZero() + "','" + item_bargin.PriceType + "','" + item_bargin.IsInternal + "','" + item_bargin.IsPeer + "','" + item_bargin.VoyageType + "');\">修改返佣</a> " : "同行政策",
                               Policy_link = item_bargin.Owner == this.CurrentCompany.CompanyId ? "<a href='low_price_policy_edit.aspx?Id=" + item_bargin.Id + "&Type=Update&Check=view'>修改详细</a>" : "",
                               //操作人
                               Opearor = item_bargin.Owner == this.CurrentCompany.CompanyId ? item_bargin.Creator : "&nbsp;",
                               Sudit = item_bargin.Audited == true ? "已审" : "未审",
                               Hang = item_bargin.Suspended == true ? "挂起" : "未挂"
                           };
                this.grv_bargain.DataSource = list;
                this.grv_bargain.DataBind();
                if (list.Any())
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = query_list.RowCount;
                    }
                    showempty.Visible = false;
                }
                else
                {
                    showempty.Visible = true;
                    this.pager.Visible = false;
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
                PageIndex = pagination.PageIndex,
                PageSize = pager.PageSize
                //TicketType = ra
            };
            if (!string.IsNullOrWhiteSpace(this.ddlTicketType.SelectedValue))
                parameter.TicketType = (TicketType)int.Parse(this.ddlTicketType.SelectedValue);
            if (!string.IsNullOrWhiteSpace(this.ddlVoyageType.SelectedValue))
                parameter.VoyageType = (VoyageType)int.Parse(this.ddlVoyageType.SelectedValue);
            return parameter;
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
            QueryBargainPolicy(1);
        }

        private void QueryBargainPolicy(int pageindex)
        {
            QueryPolicy(new Pagination
            {
                PageSize =pager.PageSize,
                PageIndex = pageindex,
                GetRowCount = true
            });
            hidIds.Value = "";
            pager.CurrentPageIndex = pageindex;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            PriceType type = PriceType.Price;
            decimal price = -1;
            if (radPrice.Checked)
            {
                type = PriceType.Price;
                if (!string.IsNullOrWhiteSpace(this.txtPrice.Text))
                    price = Decimal.Parse(this.txtPrice.Text.Trim());
            }
            if (radDiscount.Checked)
            {
                type = PriceType.Discount;
                if (!string.IsNullOrWhiteSpace(this.txtDisCount.Text))
                    price = decimal.Parse(this.txtDisCount.Text.Trim()) / 100;
            }
            if (radCommission.Checked)
            {
                type = PriceType.Commission;
                price = -1;
            }
            PolicyManageService.UpdateBargainPolicyCommission(Guid.Parse(hidIds.Value), price, type, Decimal.Parse(this.txtInternalCommission.Text.Trim() == "" ? "0" : this.txtInternalCommission.Text.Trim()) / 100, Decimal.Parse(this.txtSubordinateCommission.Text.Trim() == "" ? "0" : this.txtSubordinateCommission.Text.Trim()) / 100, Decimal.Parse(this.txtProfessionCommission.Text.Trim() == "" ? "0" : this.txtProfessionCommission.Text.Trim()) / 100, this.CurrentUser.UserName);
            QueryBargainPolicy(pager.CurrentPageIndex);
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