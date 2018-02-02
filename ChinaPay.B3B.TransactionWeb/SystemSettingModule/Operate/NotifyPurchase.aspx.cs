using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using ChinaPay.B3B.Service.FlightTransfer;
using ChinaPay.Core.Extension;
using ChinaPay.SMS.Service;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate
{
    public partial class NotifyPurchase : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                initData();
                queryNoticyPurchase(new Pagination
                {
                    PageIndex = 1,
                    GetRowCount = true,
                    PageSize = pager.PageSize
                });
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination
            {
                PageIndex = newPage,
                PageSize = pager.PageSize,
                GetRowCount = true
            };
            queryNoticyPurchase(pagination);
        }

        private void queryNoticyPurchase(Pagination pagination)
        {
            this.rptNotify.DataSource = from item in QSService.QueryTransferDetails(pagination, Guid.Parse(Request.QueryString["purchaserId"]))
                                        select new
                                        {
                                            PNR = item.PNR.ToString(),
                                            OrderId = item.OrderId,
                                            OriginalCarrierName = item.OriginalCarrierName,
                                            TransferType = item.TransferType.GetDescription(),
                                            OriginalFlightNo = item.OriginalFlightNo,
                                            FlightNo = item.FlightNo,
                                            OriginalTakeoffTime = item.OriginalTakeoffTime,
                                            TakeoffTime = item.TakeoffTime.HasValue ? item.TakeoffTime.Value.ToString("yyyy-MM-dd HH:mm") : string.Empty,
                                            OriginalArrivalTime = item.OriginalArrivalTime,
                                            ArrivalTime = item.ArrivalTime.HasValue?item.ArrivalTime.Value.ToString("yyyy-MM-dd HH:mm"):string.Empty,
                                            CarrierName = item.CarrierName,
                                            TransferId = item.TransferId
                                        };
            this.rptNotify.DataBind();
            if (pagination.RowCount > 0)
            {
                this.pager.Visible = true;
                if (pagination.GetRowCount)
                {
                    this.pager.RowCount = pagination.RowCount;
                }
            }
            else
            {
                this.pager.Visible = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                var ids = new List<Guid>();
                var transferIds = this.hfdTransferIds.Value.Split(',');
                foreach (var item in transferIds)
                {
                    ids.Add(Guid.Parse(item));
                }
                try
                {
                    if (this.radioMessage.Checked)
                    {
                        QSService.MessageInformPurchaser(Guid.Parse(Request.QueryString["purchaserId"]),ids, this.txtMessage.InnerText.Trim(), CurrentUser.UserName, CurrentUser.Name);
                        var contactPhone = new List<string>();
                        contactPhone.Add(Request.QueryString["contractPhone"]);
                        SMSSendService.SendCustomMessage(new SMS.Service.Domain.Account(Guid.Empty,CurrentUser.UserName),
                            contactPhone,
                                this.txtMessage.InnerText.Trim(),true);
                    }
                    if (this.radioHandler.Checked)
                    {
                        QSService.InformPurchaser(Guid.Parse(Request.QueryString["purchaserId"]), ids,(InformType)int.Parse(this.ddlNoticeWay.SelectedValue),
                            (InformResult)int.Parse(this.ddlNoticeResult.SelectedValue), this.txtNoticeRemark.Text.Trim(), CurrentUser.UserName, CurrentUser.Name);
                    }
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "success", "alert('通知成功');window.location.href='FlightChangeNotice.aspx'", true);
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "通知");
                }
            }
        }

        private bool validate()
        {
            if (this.radioMessage.Checked)
            {
                if (this.txtMessage.InnerText.Trim().Length == 0)
                {
                    ShowMessage("请输入短信内容");
                    return false;
                }
                if (this.txtMessage.InnerText.Trim().Length > 300)
                {
                    ShowMessage("短信字数不能超过300位");
                    return false;
                }
            }
            if (this.radioHandler.Checked)
            {
                if (string.IsNullOrWhiteSpace(this.ddlNoticeWay.SelectedValue))
                {
                    ShowMessage("请选择通知方式");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(this.ddlNoticeResult.SelectedValue))
                {
                    ShowMessage("请选择通知结果");
                    return false;
                }
                if (this.txtNoticeRemark.Text.Trim().Length > 1000)
                {
                    ShowMessage("通知备注不能超过1000位");
                    return false;
                }
            }
            return true;
        }

        private void initData()
        {
            var purchaserAccount = Request.QueryString["purchaserAccount"];
            if (!string.IsNullOrWhiteSpace(purchaserAccount))
            {
                this.lblPurchaseAccountNo.Text = this.lblPurchaseNo.Text = purchaserAccount;
            }
            var purchaserName = Request.QueryString["purchaserName"];
            if (!string.IsNullOrWhiteSpace(purchaserName))
            {
                this.lblPurchaseName.Text = purchaserName;
            }
            var contractPhone = Request.QueryString["contractPhone"];
            if (!string.IsNullOrWhiteSpace(contractPhone))
            {
                this.lblPurchaseContactPhone.Text = contractPhone;
            }
            var flightCount = Request.QueryString["flightCount"];
            if (!string.IsNullOrWhiteSpace(flightCount))
            {
                this.lblPurchaseFlightCount.Text = flightCount;
            }
            var orderCount = Request.QueryString["orderCount"];
            if (!string.IsNullOrWhiteSpace(orderCount))
            {
                this.lblPurchaseOrderCount.Text = orderCount;
            }
            var informWay = Enum.GetValues(typeof(InformType)) as InformType[];
            foreach (var item in informWay)
            {
                if (item != InformType.Message)
                    this.ddlNoticeWay.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlNoticeWay.Items.Insert(0, new ListItem("全部", ""));
            var informResult = Enum.GetValues(typeof(InformResult)) as InformResult[];
            foreach (var item in informResult)
            {
                this.ddlNoticeResult.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.ddlNoticeResult.Items.Insert(0, new ListItem("全部", ""));
        }
    }
}