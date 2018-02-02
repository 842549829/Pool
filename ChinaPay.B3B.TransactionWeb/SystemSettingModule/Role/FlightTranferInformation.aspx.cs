using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using ChinaPay.B3B.Service.FlightTransfer;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.FlightTransfer;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class FlightTranferInformation : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                lblPlatformName.Text = lblPlatformName1.Text = lblPlatformName2.Text = PlatformName;
                initData();   
            }
            this.Pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(Pager_CurrentPageChanged);
        }

        void Pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination
            {
                GetRowCount = true,
                PageIndex = newPage,
                PageSize = Pager.PageSize,
            };
            queryFlightInformationByPurchase(pagination);
        }


        private void queryFlightInformationByPurchase(Pagination pagination)
        {
            var data = from item in QSService.QueryTransferInformationByPurchase(getCondition(),pagination)
                       select new
                       {
                           OriginalCarrierName = item.OriginalCarrierName,
                           CarrierName = item.CarrierName,
                           TransferType = item.TransferType.GetDescription(),
                           OriginalFlightNo = item.OriginalFlightNo,
                           FlightNo = item.FlightNo,
                           OriginalTakeoffTime = item.OriginalTakeoffTime,
                           TakeoffTime = item.TakeoffTime.HasValue?item.TakeoffTime.ToString():string.Empty,
                           OriginalArrivalTime = item.OriginalArrivalTime,
                           ArrivalTime = item.ArrivalTime.HasValue?item.ArrivalTime.ToString():string.Empty
                       };
            this.rptInformation.DataSource = data;
            this.rptInformation.DataBind();
            if (data.Count() > 0)
            {
                this.Pager.Visible = true;
                this.rptInformation.Visible = true;
                this.emptyDataInfo.Visible = false;
                if (pagination.GetRowCount)
                {
                    Pager.RowCount = pagination.RowCount;
                }
            }
            else
            {
                this.Pager.Visible = false;
                this.rptInformation.Visible = false;
                this.emptyDataInfo.Visible = true;
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            if (this.Pager.CurrentPageIndex == 1)
            {
                var pagination = new Pagination
                {
                    GetRowCount = true,
                    PageIndex = 1,
                    PageSize = Pager.PageSize,
                };
                queryFlightInformationByPurchase(pagination);
            }
            else
            {
                this.Pager.CurrentPageIndex = 1;
            }
        }

        private FlightTransferCondition getCondition()
        {
            var condition = new FlightTransferCondition();
            if (!string.IsNullOrWhiteSpace(this.ddlAirline.SelectedValue))
            {
                condition.Carrier = this.ddlAirline.SelectedValue;
            }
            if (!string.IsNullOrWhiteSpace(this.txtFlightNo.Text))
            {
                condition.OriginalFlightNo = this.txtFlightNo.Text;
            }
            if (!string.IsNullOrWhiteSpace(this.ddlTranferType.SelectedValue))
            {
                condition.TransferType = (TransferType)int.Parse(this.ddlTranferType.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.txtTakeOffLowerTime.Text))
            {
                condition.OriginalTakeOffLowerTime = DateTime.Parse(this.txtTakeOffLowerTime.Text);
            }
            if (!string.IsNullOrWhiteSpace(this.txtTakeOffUpperTime.Text))
            {
                condition.OriginalTakeOffUpperTime = DateTime.Parse(this.txtTakeOffUpperTime.Text).AddDays(1).AddMilliseconds(-3);
            }
            return condition;
        }

        private void initData()
        {
            this.ddlAirline.DataSource = from item in FoundationService.Airlines
                                         select new
                                         {
                                             Text = item.Code+"-"+item.ShortName,
                                             Value = item.Code
                                         };
            this.ddlAirline.DataTextField = "Text";
            this.ddlAirline.DataValueField = "Value";
            this.ddlAirline.DataBind();
            this.ddlAirline.Items.Insert(0, new ListItem("全部",""));
            this.txtTakeOffLowerTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.txtTakeOffUpperTime.Text = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");
            var tranferType = Enum.GetValues(typeof(TransferType)) as TransferType[];
            foreach (var item in tranferType)
            {
                this.ddlTranferType.Items.Add(new ListItem(item.GetDescription(),((byte)item).ToString()));
            }
            this.ddlTranferType.Items.Insert(0, new ListItem("全部",""));
        }
    }
}