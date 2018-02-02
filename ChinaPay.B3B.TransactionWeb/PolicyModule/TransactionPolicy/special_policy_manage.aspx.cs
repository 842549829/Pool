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

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
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
                RegisterScript("alert('还未有任何政策设置信息，不能访问本页面！请联系平台。');window.location.href='/Index.aspx';", true);
                return;
            }
            if (setting.Airlines == "")
            {
                RegisterScript("alert('还没有设置航空公司，请先设置航空公司！请联系平台。');window.location.href='/Index.aspx';", true);
                return;
            }
            //特殊票类型
            var companyParameter = CompanyService.GetCompanyParameter(this.CurrentCompany.CompanyId);
            if (companyParameter.Singleness)
                this.ddlSpecialType.Items.Add(new ListItem("单程控位产品", ((int)SpecialProductType.Singleness).ToString()));
            if (companyParameter.Disperse)
                this.ddlSpecialType.Items.Add(new ListItem("散冲团产品", ((int)SpecialProductType.Disperse).ToString()));
            if (companyParameter.CostFree)
                this.ddlSpecialType.Items.Add(new ListItem("免票产品", ((int)SpecialProductType.CostFree).ToString()));
            if (companyParameter.Bloc)
                this.ddlSpecialType.Items.Add(new ListItem("集团票产品", ((int)SpecialProductType.Bloc).ToString()));
            if (companyParameter.Business)
                this.ddlSpecialType.Items.Add(new ListItem("商旅卡产品", ((int)SpecialProductType.Business).ToString()));
            if (companyParameter.OtherSpecial)
                this.ddlSpecialType.Items.Add(new ListItem("其他特殊产品", ((int)SpecialProductType.OtherSpecial).ToString()));
            if (companyParameter.LowToHigh)
                this.ddlSpecialType.Items.Add(new ListItem("低打高返特殊产品", ((int)SpecialProductType.LowToHigh).ToString()));
            this.ddlSpecialType.Items.Insert(0, new ListItem("-请选择-", ""));
            BindAriline(setting);
            BindCity(setting);
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
                               //政策编号
                               id = item.Id,
                               //航空公司
                               Airline = item.Airline,
                               //出发城市
                               Departure = item.Departure,
                               //到达城市
                               Arrival = item.Arrival,
                               //排除日期
                               DepartureDateFilter = item.DepartureDateFilter,
                               //适用班期
                               DepartureWeekFilter = StringOperation.TransferToChinese(item.DepartureWeekFilter),
                               //航班限制
                               Include = item.DepartureFlightsFilterType == LimitType.None ? "不限" : (item.DepartureFlightsFilterType == LimitType.Include ? ("适用：" + item.DepartureFlightsFilter) : "不适用：" + item.DepartureFlightsFilter),
                               //提前天数
                               BeforehandDays = item.BeforehandDays == -1 ? "" : item.BeforehandDays + "",
                               //特殊票类型
                               Type = item.Type.GetDescription(),
                               //发布价格
                               Price = item.PriceType == PriceType.Price ? item.Price.TrimInvaidZero() : (item.Price * 100).TrimInvaidZero(),
                               //价格/直减
                               PriceInfo = item.PriceType == PriceType.Price ? (item.Price == -1 ? "" : item.Price.TrimInvaidZero() + "元") : (item.PriceType == PriceType.Subtracting ? (item.Price * 100).TrimInvaidZero() + "%" : ""),
                               //航班日期
                               DepartureDates = item.DepartureDateStart.ToString("yyyy-MM-dd") + "<br />" + item.DepartureDateEnd.ToString("yyyy-MM-dd"),
                               item.PriceType,
                               item.IsInternal,
                               item.IsPeer,
                               InternalCommission = item.PriceType == PriceType.Price ? item.InternalCommission.TrimInvaidZero() : (item.InternalCommission * 100).TrimInvaidZero(),
                               SubordinateCommission = CurrentCompany.CompanyType == CompanyType.Provider ||( CurrentCompany.CompanyType == CompanyType.Supplier && OEM != null) ? item.PriceType == PriceType.Price ? item.SubordinateCommission.TrimInvaidZero() : (item.SubordinateCommission * 100).TrimInvaidZero() : "-1",
                               ProfessionCommission = item.PriceType == PriceType.Price ? item.ProfessionCommission.TrimInvaidZero() : (item.ProfessionCommission * 100).TrimInvaidZero(), 
                               Commission = ((item.InternalCommission < 0 && !item.IsInternal) ? "" : ("内部: " + (item.PriceType == PriceType.Price ? item.InternalCommission.TrimInvaidZero() + "元<br />" : (item.InternalCommission * 100).TrimInvaidZero() + "%<br />")))
                                              + (item.SubordinateCommission < 0 ? "" : CurrentCompany.CompanyType == CompanyType.Provider || (CurrentCompany.CompanyType == CompanyType.Supplier && OEM != null) ? ("下级: " + (item.PriceType == PriceType.Price ? item.SubordinateCommission.TrimInvaidZero() + "元<br />" : (item.SubordinateCommission * 100).TrimInvaidZero() + "%<br />")) : "")
                                              + ((item.ProfessionCommission < 0 && !item.IsPeer) ? "" : ("同行: " + (item.PriceType == PriceType.Price ? item.ProfessionCommission.TrimInvaidZero() + "元" : (item.ProfessionCommission * 100).TrimInvaidZero() + "%"))),
                               TypeValue = item.Type,

                               //操作人
                               Opearor = item.Creator,
                               Sudit = item.Audited ? "已审" : "未审",
                               SuditName = item.Audited ? "UnAudited" : "Audited",
                               SuditTip = item.Audited ? "取消审核" : "确认审核",
                               Hang = item.Suspended ? (item.SuspendByPlatform ? "平台挂起" : "公司挂起") : "未挂",
                               //是否是特价舱位
                               item.IsBargainBerths
                           };
                this.grv_specical.DataSource = list;
                this.grv_specical.DataBind();
                if (list.Any())
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    showempty.Visible = false;
                    grv_specical.HeaderRow.TableSection = TableRowSection.TableHeader;
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
            bool Audited;
            Audited = radAudit.Checked;
            PolicyQueryParameter parameter = new PolicyQueryParameter
            {
                Airline = ddlAirline.SelectedValue,
                Departure = txtDeparture.Code,
                Arrival = txtArrival.Code,
                DepartureDateStart = (this.txtStartTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtStartTime.Text)),
                DepartureDateEnd = (this.txtEndTime.Text == "" ? (Nullable<DateTime>)null : DateTime.Parse(txtEndTime.Text)),
                //PageIndex = pagination.PageIndex,
                //PageSize = pagination.PageSize,
                //Creator = ddlOperator.SelectedValue,
                Effective = ddlYouxiao.SelectedIndex == 0 ? (int?)null : ddlYouxiao.SelectedIndex,
                Owner = this.CurrentCompany.CompanyId,
                PubDateStart = DateTime.Parse(txtPubStartTime.Text),
                PubDateEnd = DateTime.Parse(txtPubEndTime.Text),
                Bunks = txtBunks.Text,
                OrderBy = 1,
                PolicyType = PolicyType.Special
                //TicketType = ra
            };
            if (!radAuditAll.Checked)
            {
                parameter.Audited = Audited;
            }
            parameter.SpecialProductType = string.IsNullOrWhiteSpace(ddlSpecialType.SelectedValue) ? (Nullable<SpecialProductType>)null : (SpecialProductType)int.Parse(ddlSpecialType.SelectedValue);
            //if (ddlOperator.SelectedIndex != 0)
            //{
            //    parameter.Operator = Guid.Parse(ddlOperator.SelectedValue);
            //}
            return parameter;
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            QuerySpecialPolicy(1);
            pager.CurrentPageIndex = 1;
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
            hidIsAll.Value = "";
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
                if (radCommission.Checked)
                {
                    priceType = PriceType.Commission;
                    price = -1;
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
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            Response.Redirect("./special_policy_publish.htm");
        }

        protected void grv_specical_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //确认审核
            if (e.CommandName == "Audited")
            {
                PolicyManageService.Audit(PolicyType.Special, this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                QuerySpecialPolicy(pager.CurrentPageIndex);
            }
            //取消审核
            if (e.CommandName == "UnAudited")
            {
                PolicyManageService.CancelAudit(PolicyType.Special, this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                QuerySpecialPolicy(pager.CurrentPageIndex);
            }
            //删除政策
            if (e.CommandName == "del")
            {
                try
                {
                    PolicyManageService.DeleteSpecialPolicy(this.CurrentUser.UserName, Guid.Parse(e.CommandArgument.ToString()));
                    QuerySpecialPolicy(pager.CurrentPageIndex);
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
                PolicyManageService.Audit(PolicyType.Special, this.CurrentUser.UserName, list_ids.ToArray());
                QuerySpecialPolicy(pager.CurrentPageIndex);
                ClaerHidValue();
            }
        }

        protected void btnUnAudited_Click(object sender, EventArgs e)
        {
            if (GetCheckBoxValue())
            {
                var list_ids = hidIsAll.Value == "0" ? hidIds.Value.Split(',').Select(Guid.Parse) : PolicyManageService.QueryPolicyIds(GetCondition()).Select(item => item.Key);
                PolicyManageService.CancelAudit(PolicyType.Special, this.CurrentUser.UserName, list_ids.ToArray());
                QuerySpecialPolicy(pager.CurrentPageIndex);
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
                    PolicyManageService.DeleteSpecialPolicy(this.CurrentUser.UserName, list_ids.ToArray());
                    QuerySpecialPolicy(pager.CurrentPageIndex);
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
        private void ClaerHidValue()
        {
            hidIsAll.Value = "";
            hidIds.Value = "";
        }

    }
}