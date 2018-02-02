using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.FlightTransfer;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;
using ChinaPay.Core;
using System.Web.UI;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class FlightChangeNotice : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                initData();
                queryFlightChange(1);
            }
            //航班变动
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
            //查看通知记录
            this.recordPager.CurrentPageChanged += recordPager_CurrentPageChanged;
        }

        void recordPager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination
            {
                PageIndex = newPage,
                PageSize = recordPager.PageSize,
                GetRowCount = true
            };
            queryNoticeRecords(pagination);
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            queryFlightChange(newPage);
        }

        private void queryFlightChange(int newPage)
        {
            var pagination = new Pagination
            {
                GetRowCount = true,
                PageSize = pager.PageSize,
                PageIndex = newPage
            };
            queryFlightChange(pagination);
        }

        private void queryFlightChange(Pagination pagination)
        {
            this.dataList.DataSource = QSService.QueryTransferInformation(pagination);
            this.dataList.DataBind();
            if (pagination.RowCount > 0)
            {
                this.pager.Visible = true;
                this.dataList.Visible = true;
                this.emptyDataInfo.Visible = false;
                if (pagination.GetRowCount)
                {
                    this.pager.RowCount = pagination.RowCount;
                }
            }
            else
            {
                this.dataList.Visible = false;
                this.emptyDataInfo.Visible = true;
                this.pager.Visible = false;
            }
        }

        private void queryNoticeRecords(Pagination pagination)
        {
            this.noticeRecords.DataSource = from item in QSService.QueryInformRecords(getRecordConditon(), pagination)
                                            select new
                                            {
                                                PurchaserAccount = item.PurchaserAccount,
                                                CarrierName = item.CarrierName,
                                                DepartureName = item.DepartureName,
                                                ArrivalName = item.ArrivalName,
                                                FlightNO = item.FlightNO,
                                                TransferType = item.TransferType.GetDescription(),
                                                InformTime = item.InformTime.HasValue ? item.InformTime.Value.ToString("yyyy-MM-dd HH:mm") : string.Empty,
                                                InformMethod = item.InformType.HasValue ? item.InformType.GetDescription() : string.Empty,
                                                InformResult = item.InformResult.HasValue ? item.InformResult.GetDescription() : string.Empty,
                                                InfromerName = item.InfromerName,
                                                ArrivalCityName = item.ArrivalCityName,
                                                DepartureCityName = item.DepartureCityName
                                            };
            this.noticeRecords.DataBind();
            if (pagination.RowCount > 0)
            {
                this.recordPager.Visible = true;
                if (pagination.GetRowCount)
                {
                    this.recordPager.RowCount = pagination.RowCount;
                }
            }
            else
            {
                this.recordPager.Visible = false;
            }
        }

        private DataTransferObject.FlightTransfer.InfomrRecordSearchConditoin getRecordConditon()
        {
            var condition = new DataTransferObject.FlightTransfer.InfomrRecordSearchConditoin();
            if (!string.IsNullOrWhiteSpace(this.ddlAirlines.SelectedValue))
            {
                condition.Carrier = this.ddlAirlines.SelectedValue;
            }
            if (!string.IsNullOrWhiteSpace(this.txtFlightNo.Text))
            {
                condition.FlightNo = this.txtFlightNo.Text;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlChangeType.SelectedValue))
            {
                condition.TransferType = (TransferType)int.Parse(this.ddlChangeType.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.txtDepartureCity.Code))
            {
                condition.Departure = this.txtDepartureCity.Code;
            }
            if (!string.IsNullOrWhiteSpace(this.txtArrivalCity.Code))
            {
                condition.Arrival = this.txtArrivalCity.Code;
            }
            if (this.txtPurchase.CompanyId.HasValue)
            {
                condition.PurchaserId = this.txtPurchase.CompanyId;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlNoticeWay.SelectedValue))
            {
                condition.InformType = (InformType)int.Parse(this.ddlNoticeWay.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.ddlNoticeResult.SelectedValue))
            {
                condition.InformResult = (InformResult)int.Parse(this.ddlNoticeResult.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.txtNoticeLowerTime.Text))
            {
                condition.InformTimeFrom = DateTime.Parse(this.txtNoticeLowerTime.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtNoticeUpperTime.Text))
            {
                condition.InformTimeTo = DateTime.Parse(this.txtNoticeUpperTime.Text).AddDays(1).AddMilliseconds(-3);
            }
            return condition;
        }

        private void initData()
        {
            //航空公司
            this.ddlAirlines.DataSource = from item in FoundationService.Airlines
                                          select new
                                          {
                                              Name = item.Code + "-" + item.ShortName,
                                              Code = item.Code
                                          };
            this.ddlAirlines.DataTextField = "Name";
            this.ddlAirlines.DataValueField = "Code";
            this.ddlAirlines.DataBind();
            this.ddlAirlines.Items.Insert(0, new ListItem("全部", ""));
            //变更类型
            var transferType = Enum.GetValues(typeof(TransferType)) as TransferType[];
            foreach (var item in transferType)
            {
                this.ddlChangeType.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlChangeType.Items.Insert(0, new ListItem("全部", ""));
            //通知类型
            var informMethod = Enum.GetValues(typeof(InformType)) as InformType[];
            foreach (var item in informMethod)
            {
                this.ddlNoticeWay.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlNoticeWay.Items.Insert(0, new ListItem("全部", ""));
            //通知结果
            var informResult = Enum.GetValues(typeof(InformResult)) as InformResult[];
            foreach (var item in informResult)
            {
                this.ddlNoticeResult.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlNoticeResult.Items.Insert(0, new ListItem("全部", ""));
            //采购商
            //var companies = CompanyService.GetCompanies(CompanyType.Provider | CompanyType.Purchaser | CompanyType.Supplier, true);
            this.txtPurchase.SetCompanyType(CompanyType.Provider | CompanyType.Purchaser | CompanyType.Supplier, true);

            var flight = QSService.QueryFlightTransferStatInfo();
            if (flight != null)
            {
                this.lblAirlineCount.Text = flight.CarrierCount.ToString();
                this.lblFlightCount.Text = flight.FlightCount.ToString();
                this.lblPuchaseCount.Text = flight.PurchaserCount.ToString();
                this.lblFlightChangeTime.Text = flight.LastQSTime.ToString();
                lblToBeInformCount.Text = flight.ToBeInformCount.ToString();
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            this.hfdType.Value = "records";
            if (valiate())
            {
                if (this.pager.CurrentPageIndex == 1)
                {
                    var pagination = new Pagination
                    {
                        GetRowCount = true,
                        PageSize = recordPager.PageSize,
                        PageIndex = 1
                    };
                    queryNoticeRecords(pagination);

                }
                else
                {
                    this.pager.CurrentPageIndex = 1;
                }
            }
        }

        private bool valiate()
        {
            if (this.txtFlightNo.Text.Trim().Length > 0 && !Regex.IsMatch(this.txtFlightNo.Text.Trim(), "^[a-zA-Z0-9]{4}$"))
            {
                ShowMessage("航班号格式错误");
                return false;
            }
            return true;
        }
    }
}