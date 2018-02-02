using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    public partial class special_policy_view : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                this.txtStartTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
                //this.txtEndTime.Text = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");

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
                var list_query = ChinaPay.B3B.Service.PolicyMatch.PolicyMatchServcie.GetSpecialPolicies(this.CurrentCompany.CompanyId, paramer, item => item.Rebate, OrderMode.Descending);
                var list = from item in list_query
                           let item_special = item.OriginalPolicy as SpecialPolicyInfo
                           select new
                           {
                               //政策编号
                               id = item_special.Id,
                               //航空公司
                               Airline = item_special.Airline,
                               //出发城市
                               Departure = paramer.Departure,
                               //到达城市
                               Arrival = paramer.Arrival,
                               //排除日期
                               DepartureDateFilter = item_special.DepartureDateFilter,
                               //适用班期
                               DepartureWeekFilter = StringOperation.TransferToChinese(item_special.DepartureWeekFilter),
                               //航班限制
                               Include = item_special.DepartureFlightsFilterType == LimitType.None ? "不限" : (item_special.DepartureFlightsFilterType == LimitType.Include ? ("适用：" + item_special.DepartureFlightsFilter) : "不适用：" + item_special.DepartureFlightsFilter),
                               //发布价格
                               Price = item_special.Price == -1 ? "" : item_special.Price.TrimInvaidZero(),
                               //提前天数
                               BeforehandDays = item_special.BeforehandDays == -1 ? "" : item_special.BeforehandDays + "",
                               //去程日期
                               DepartureDates = item_special.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item_special.DepartureDateEnd.ToString("yyyy-MM-dd"),
                               Commission_link = item_special.Owner == this.CurrentCompany.CompanyId ? "<a href=\"javascript:ModifyCommission('" + item_special.Id + "','" + item_special.Price.TrimInvaidZero() + "','" + item_special.PriceType + "','" + item_special.Type + "','" + item_special.IsInternal + "','" + item_special.IsPeer + "','" + item_special.InternalCommission.TrimInvaidZero() + "','" + (CurrentCompany.CompanyType == CompanyType.Provider || (CurrentCompany.CompanyType == CompanyType.Supplier && OEM != null) ? item_special.SubordinateCommission.TrimInvaidZero() : "-1") + "','" + item_special.ProfessionCommission.TrimInvaidZero() + "','" + item_special.IsBargainBerths + "');\">修改返佣</a>" : "同行政策",
                               Policy_link = item_special.Owner == this.CurrentCompany.CompanyId ? "<a href='special_policy_edit.aspx?Id=" + item_special.Id + "&Type=Update&Check=view'>修改详细</a>" : "&nbsp",
                               //操作人
                               Opearor = item_special.Owner == this.CurrentCompany.CompanyId ? item_special.Creator : "&nbsp",
                               Sudit = item_special.Audited == true ? "已审" : "未审",
                               Hang = item_special.Suspended == true ? "挂起" : "未挂"
                           };
                this.grv_specical.DataSource = list;
                this.grv_specical.DataBind();
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
            PolicyQueryParameter parameter = new PolicyQueryParameter
            {
                Airline = ddlAirline.SelectedValue,
                Departure = txtDeparture.Code,
                Arrival = txtArrival.Code,
                DepartureDateStart = (this.txtStartTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtStartTime.Text)),
                //DepartureDateEnd = (this.txtEndTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtEndTime.Text)),
                PageIndex = pagination.PageIndex,
                PageSize = pager.PageSize
                //TicketType = ra
            };
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
            hidIds.Value = "";
            pager.CurrentPageIndex = pageindex;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (valiate())
            {
                PriceType priceType = PriceType.Price;
                decimal subordinateCommission = 0;
                decimal professionCommission = 0;
                decimal internalCommission = 0;
                decimal price = 0;
                if (radPrice.Checked)
                {
                    priceType = PriceType.Price;
                    price = decimal.Parse(this.txtPrice.Text);
                    if (!string.IsNullOrWhiteSpace(this.txtSubordinateCommission.Text))
                        subordinateCommission = decimal.Parse(this.txtSubordinateCommission.Text);
                    if (!string.IsNullOrWhiteSpace(this.txtProfessionCommission.Text))
                        professionCommission = decimal.Parse(this.txtProfessionCommission.Text);
                    if (!string.IsNullOrWhiteSpace(this.txtInternalCommission.Text))
                        internalCommission = decimal.Parse(this.txtInternalCommission.Text);
                }
                if (radLapse.Checked)
                {
                    priceType = PriceType.Subtracting;
                    price = decimal.Parse(this.txtLapse.Text) / 100;
                    if (!string.IsNullOrWhiteSpace(this.txtSubordinateCommission.Text))
                        subordinateCommission = decimal.Parse(this.txtSubordinateCommission.Text) / 100;
                    if (!string.IsNullOrWhiteSpace(this.txtProfessionCommission.Text))
                        professionCommission = decimal.Parse(this.txtProfessionCommission.Text) / 100;
                    if (!string.IsNullOrWhiteSpace(this.txtInternalCommission.Text))
                        internalCommission = decimal.Parse(this.txtInternalCommission.Text) / 100;
                }

                PolicyManageService.UpdateSpecialPolicyPrice(Guid.Parse(hidIds.Value), priceType, price, subordinateCommission, internalCommission, professionCommission, this.CurrentUser.UserName);
                QuerySpecialPolicy(pager.CurrentPageIndex);
            }
            QuerySpecialPolicy(pager.CurrentPageIndex);
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

        private bool valiate()
        {
            string commissionPattern = "^[0-9]{1,2}(.[0-9])?$";
            string pricePattern = "^[0-9]{1,10}$";
            if (radPrice.Checked)
            {
                if (txtPrice.Text != "-1" && !Regex.IsMatch(this.txtPrice.Text, pricePattern))
                {
                    ShowMessage("价格格式错误！");
                    return false;
                }
                if (this.hfdSpecialType.Value == "Bloc" || this.hfdSpecialType.Value == "Business")
                {
                    if (!Regex.IsMatch(this.txtSubordinateCommission.Text, pricePattern))
                    {
                        ShowMessage("下级佣金格式错误！");
                        return false;
                    }
                    if (this.hfdIsInternal.Value == "True")
                    {
                        if (!Regex.IsMatch(this.txtInternalCommission.Text, pricePattern))
                        {
                            ShowMessage("内部佣金格式错误！");
                            return false;
                        }
                    }
                    if (this.hfdIsPeer.Value == "True")
                    {
                        if (!Regex.IsMatch(this.txtProfessionCommission.Text, pricePattern))
                        {
                            ShowMessage("同行佣金格式错误！");
                            return false;
                        }
                    }
                }
            }
            if (radLapse.Checked)
            {
                if (!Regex.IsMatch(this.txtLapse.Text, commissionPattern))
                {
                    ShowMessage("直减格式错误！");
                    return false;
                }
                if (this.hfdSpecialType.Value == "Bloc" || this.hfdSpecialType.Value == "Business")
                {
                    if (!Regex.IsMatch(this.txtSubordinateCommission.Text, commissionPattern))
                    {
                        ShowMessage("下级佣金格式错误！");
                        return false;
                    }
                    if (this.hfdIsInternal.Value == "True")
                    {
                        if (!Regex.IsMatch(this.txtInternalCommission.Text, commissionPattern))
                        {
                            ShowMessage("内部佣金格式错误！");
                            return false;
                        }
                    }
                    if (this.hfdIsPeer.Value == "True")
                    {
                        if (!Regex.IsMatch(this.txtProfessionCommission.Text, commissionPattern))
                        {
                            ShowMessage("同行佣金格式错误！");
                            return false;
                        }
                    }
                }
            }
            return true;
        }

    }
}