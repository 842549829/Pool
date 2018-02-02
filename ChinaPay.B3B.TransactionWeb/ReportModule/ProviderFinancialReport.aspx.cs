using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Service.Report;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class ProviderFinancialReport : BasePage
    {
        public string totalAmount = "￥0.00";
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            this.dataList.Visible = false;
            if (!IsPostBack)
            {
                initData();
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                GetRowCount = true,
                PageIndex = newPage
            };
            queryProviderFinancial(pagination);
        }

        private void queryProviderFinancial(Pagination pagination)
        {
            try
            {
                decimal totalTradeAmount;
                var list = ReportService.QueryProvideFinancial(pagination, getCondition(), out totalTradeAmount);
                var tradeAmount = list.Compute("Sum(TradeAmount)", "");
                if (tradeAmount != DBNull.Value)
                {
                    this.totalAmount = "￥" + tradeAmount.ToString();
                }
                this.dataList.DataSource = list;
                this.dataList.DataBind();
                if (list.Rows.Count > 0)
                {
                    counts.Visible = true;
                    this.pager.Visible = true;
                    this.dataList.Visible = true;
                    this.emptyDataInfo.Visible = false;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    lblTradeAmount.Text = "￥" + totalTradeAmount + "元";
                }
                else
                {
                    counts.Visible = false;
                    this.pager.Visible = false;
                    this.emptyDataInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private DataTransferObject.Report.ProvideTicketView getCondition()
        {
            var view = new DataTransferObject.Report.ProvideTicketView();
            view.CompanyId = this.CurrentCompany.CompanyId;
            if (!string.IsNullOrWhiteSpace(this.txtStartDate.Text))
            {
                view.FinishBeginDate = DateTime.Parse(this.txtStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtEndDate.Text))
            {
                view.FinishEndDate = DateTime.Parse(this.txtEndDate.Text).AddDays(1).AddMilliseconds(-3);
            }
            if (!string.IsNullOrWhiteSpace(this.txtPayStartDate.Text))
            {
                view.PayBeginDate = DateTime.Parse(this.txtPayStartDate.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtPayEndDate.Text))
            {
                view.PayEndDate = DateTime.Parse(this.txtPayEndDate.Text).AddDays(1).AddMilliseconds(-3);
            }
            if (!string.IsNullOrWhiteSpace(this.txtTicketNo.Text))
            {
                view.TicketNo = this.txtTicketNo.Text;
            }
            if (!string.IsNullOrWhiteSpace(this.txtPassenger.Text))
            {
                view.Passenger = this.txtPassenger.Text;
            }
            if (!string.IsNullOrWhiteSpace(this.txtDeparture.Code))
            {
                view.Departure = this.txtDeparture.Code;
            }
            if (!string.IsNullOrWhiteSpace(this.txtArrivals.Code))
            {
                view.Arrival = this.txtArrivals.Code;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlTicketStatus.SelectedValue))
            {
                view.TicketState = (TicketState)int.Parse(this.ddlTicketStatus.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.ddlPolicyType.SelectedValue))
            {
                view.PolicyType = Convert.ToByte(this.ddlPolicyType.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.ddlAirlines.SelectedValue))
            {
                view.Airline = this.ddlAirlines.SelectedValue;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlTiketType.SelectedValue))
            {
                view.TicketType = (TicketType)int.Parse(this.ddlTiketType.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.txtOrderId.Text))
            {
                view.OrderId = decimal.Parse(this.txtOrderId.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.ddlOffice.SelectedValue))
            {
                view.OfficeNo = this.ddlOffice.SelectedValue;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlRelationType.SelectedValue))
            {
                view.RelationType = (RelationType)int.Parse(this.ddlRelationType.SelectedValue);
                if (view.RelationType == RelationType.Interior && !string.IsNullOrWhiteSpace(this.SubordinateCompany.SelectedValue))
                {
                    view.Purchase = Guid.Parse(this.SubordinateCompany.SelectedValue);
                }
                if (view.RelationType == RelationType.Junion && !string.IsNullOrWhiteSpace(this.LowerCompany.SelectedValue))
                {
                    view.Purchase = Guid.Parse(this.LowerCompany.SelectedValue);
                }
            }
            if (!string.IsNullOrWhiteSpace(this.ddlEmployee.SelectedValue))
            {
                view.ProcessorAccount = this.ddlEmployee.SelectedValue;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlSpecialType.SelectedValue))
                view.SpecialProductType = (SpecialProductType)int.Parse(this.ddlSpecialType.SelectedValue);
            return view;
        }

        private void initData()
        {
            seniorCondition.Style.Add(HtmlTextWriterStyle.Display, "none");
            if (DateTime.Now.CompareTo(DateTime.Parse("18:00")) > 0)
            {
                this.txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                this.txtStartDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                this.txtEndDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
            this.hfdProviderCompanyId.Value = this.CurrentCompany.CompanyId.ToString();
            BindAriline(this.CurrentCompany.CompanyId);
            BindCity(this.CurrentCompany.CompanyId);
            BindEmployee(this.CurrentCompany.CompanyId);
            //机票状态
            var ticketStatus = Enum.GetValues(typeof(TicketState)) as TicketState[];
            foreach (var item in ticketStatus)
            {
                if(item!= TicketState.Change)
                this.ddlTicketStatus.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlTicketStatus.Items.Insert(0, new ListItem("全部", ""));
            //机票类型
            var ticketType = Enum.GetValues(typeof(TicketType)) as TicketType[];
            foreach (var item in ticketType)
            {
                this.ddlTiketType.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlTiketType.Items.Insert(0, new ListItem("全部", ""));
            //OFFICE号
            this.ddlOffice.DataSource = CompanyService.QueryOfficeNumbers(this.CurrentCompany.CompanyId).Select(o => o.Number);
            this.ddlOffice.DataBind();
            this.ddlOffice.Items.Insert(0, new ListItem("全部", ""));
            //销售关系
            var relationType = Enum.GetValues(typeof(RelationType)) as RelationType[];
            foreach (var item in relationType)
            {
                this.ddlRelationType.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlRelationType.Items.Insert(0, new ListItem("全部", ""));

            var companyParameter = CompanyService.GetCompanyParameter(this.CurrentCompany.CompanyId);
            if (!companyParameter.CanHaveSubordinate)
            {
                this.ddlRelationType.Items.Remove(new ListItem(RelationType.Interior.GetDescription(), ((int)RelationType.Interior).ToString()));
            }
            if (!companyParameter.AllowBrotherPurchase)
                this.ddlRelationType.Items.Remove(new ListItem(RelationType.Brother.GetDescription(), ((int)RelationType.Brother).ToString()));
            var companies = CompanyService.GetAllSubordinates(new DataTransferObject.Organization.SubordinateQueryParameter
            {
                RelationshipType = RelationshipType.Distribution,
                Superior = CurrentCompany.CompanyId
            }, new Pagination
            {
                PageIndex = 1,
                PageSize = int.MaxValue
            });
            if (companies != null)
            {
                this.LowerCompany.DataSource = from item in companies
                                               select new
                                               {
                                                   Text = item.UserNo + "-" + item.AbbreviateName,
                                                   Value = item.CompanyId
                                               };
                this.LowerCompany.DataTextField = "Text";
                this.LowerCompany.DataValueField = "Value";
                this.LowerCompany.DataBind();
            }
            this.LowerCompany.Items.Insert(0, new ListItem("", ""));
            var interiorCompanies = CompanyService.GetAllSubordinates(new DataTransferObject.Organization.SubordinateQueryParameter
            {
                Superior = CurrentCompany.CompanyId,
                RelationshipType = RelationshipType.Organization
            }, new Pagination
            {
                PageIndex = 1,
                PageSize = int.MaxValue
            });
            if (interiorCompanies != null)
            {
                this.SubordinateCompany.DataSource = from item in interiorCompanies
                                                     select new
                                                     {
                                                         Text = item.UserNo + "-" + item.AbbreviateName,
                                                         Value = item.CompanyId
                                                     };
                this.SubordinateCompany.DataTextField = "Text";
                this.SubordinateCompany.DataValueField = "Value";
                this.SubordinateCompany.DataBind();
            }
            this.SubordinateCompany.Items.Insert(0, new ListItem("", ""));
            //特殊票类型
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
                this.ddlSpecialType.Items.Add(new ListItem(SpecialProductType.OtherSpecial.GetDescription(), ((int)SpecialProductType.OtherSpecial).ToString()));
            if (companyParameter.LowToHigh)
                this.ddlSpecialType.Items.Add(new ListItem(SpecialProductType.LowToHigh.GetDescription(), ((int)SpecialProductType.LowToHigh).ToString()));

            this.ddlSpecialType.Items.Insert(0, new ListItem("全部", ""));
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.hfdSeniorCondition.Value == "show")
            {
                this.seniorCondition.Style.Add(HtmlTextWriterStyle.Display, "");
                this.btnSeniorCondition.Value = "简化条件";
            }
            else
            {
                this.seniorCondition.Style.Add(HtmlTextWriterStyle.Display, "none");
                this.btnSeniorCondition.Value = "更多条件";
            }
            if (valiate())
            {
                if (this.pager.CurrentPageIndex == 1)
                {
                    var pagination = new Pagination()
                    {
                        PageIndex = 1,
                        GetRowCount = true,
                        PageSize = pager.PageSize
                    };
                    queryProviderFinancial(pagination);
                }
                else
                {
                    this.pager.CurrentPageIndex = 1;
                }
            }
        }

        private bool valiate()
        {
            if (this.txtOrderId.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtOrderId.Text.Trim(), @"^\d{1,13}$"))
            {
                ShowMessage("订单号格式错误！");
                return false;
            }
            if (this.txtPassenger.Text.Trim().Length > 25)
            {
                ShowMessage("乘机人位数不能超过25位！");
                return false;
            }
            if (this.txtTicketNo.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtTicketNo.Text.Trim(), @"^\d{10}$"))
            {
                ShowMessage("票号格式错误！");
                return false;
            }
            return true;
        }

        private void BindAriline(Guid company)
        {
            var airlines = PolicySetService.QueryAirlines(company);
            var allAirlines = FoundationService.Airlines;
            foreach (Airline item in allAirlines)
            {
                if (item.Valid && airlines.Contains(item.Code.Value))
                {
                    ListItem listItem = new ListItem(item.Code.Value + "-" + item.ShortName, item.Code.Value);
                    this.ddlAirlines.Items.Add(listItem);
                }
            }
            this.ddlAirlines.Items.Insert(0, new ListItem("全部", ""));
        }

        private void BindCity(Guid company)
        {
            var result = new List<Airport>();
            var airports = GetAirport(company);
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

        private List<string> GetAirport(Guid id)
        {
            List<string> list = new List<string>();
            var policy = CompanyService.GetPolicySetting(id);
            if (policy != null)
            {
                string[] airports = policy.Departure.Split('/');
                for (int i = 0; i < airports.Length; i++)
                {
                    list.Add(airports[i]);
                }
            }
            return list;
        }

        private void BindEmployee(Guid company)
        {
            this.ddlEmployee.DataSource = from item in EmployeeService.QueryEmployees(company)
                                          select new
                                          {
                                              Name = item.UserName + "-" + item.Name,
                                              Value = item.UserName
                                          };
            this.ddlEmployee.DataTextField = "Name";
            this.ddlEmployee.DataValueField = "Value";
            this.ddlEmployee.DataBind();
            this.ddlEmployee.Items.Insert(0, new ListItem("全部", ""));
        }
    }
}