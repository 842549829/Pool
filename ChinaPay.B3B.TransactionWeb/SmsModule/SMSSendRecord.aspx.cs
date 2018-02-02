using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using ChinaPay.SMS.Service;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SmsModule
{
    public partial class SMSSendRecord : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                //InitSendData(new Pagination() { PageSize = 10, PageIndex = 1, GetRowCount = true });
                txtBuyStartTime.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtBuyEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtSendStartTime.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtSendEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            this.send_pager.CurrentPageChanged += send_pager_CurrentPageChanged;
            this.buy_pager.CurrentPageChanged += buy_pager_CurrentPageChanged;
        }

        void buy_pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = buy_pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            InitBuyData(pagination);
        }

        void send_pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = send_pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            InitSendData(pagination);
        }
        /// <summary>
        /// 查询发送记录
        /// </summary>
        /// <param name="pagin"></param>
        void InitSendData(Pagination pagination)
        {
            var query = SMSAccountService.QuerySendRecords(sendQueryCondition(), pagination);
            if (query.Any())
            {
                grv_sendRecord.DataSource = from item in query
                                            select new
                                            {
                                                item.CompanyId,
                                                Mobiles = item.Mobiles.Join("<br />"),
                                                item.Content,
                                                item.SendTime,
                                                Status = item.Status == SMS.DataTransferObject.SendStatus.Failed && item.Mobiles.Count == 1 ? "<a href='javascript:send(\"" + item.Id + "\");'>重新发送</a>" : item.Status.GetDescription(),
                                                item.OperatorAccount,
                                                FiadPhone = item.Status == SMS.DataTransferObject.SendStatus.PartFailed || item.Status == SMS.DataTransferObject.SendStatus.Failed ? item.FailedMobiles.Join("<br />") : ""
                                            };
                grv_sendRecord.DataBind();
                grv_buyRecord.Visible = false;
                showempty.Visible = false;
                buy_pager.Visible = false;
                send_pager.Visible = true;
                grv_sendRecord.Visible = true;
                send_pager.RowCount = pagination.RowCount;
            }
            else
            {
                grv_buyRecord.Visible = false;
                grv_sendRecord.Visible = false;
                showempty.Visible = true;
                buy_pager.Visible = false;
                send_pager.Visible = false;
            }
        }
        /// <summary>
        /// 查询购买记录
        /// </summary>
        /// <param name="pagin"></param>
        void InitBuyData(Pagination pagination)
        {
            var query = SMSOrderService.QueryOrders(orderQueryCondition(), pagination);
            if (query.Any())
            {
                grv_buyRecord.DataSource = from item in query
                                           select new
                                           {
                                               TotalAmount = item.IsExChange ? item.TotalAmount.TrimInvaidZero() + "积分" : item.TotalAmount.TrimInvaidZero() + "元",
                                               PayTime = item.Payment.PayTime,
                                               item.TotalCount,
                                               item.Status
                                           };
                grv_buyRecord.DataBind();
                showempty.Visible = false;
                send_pager.Visible = false;
                grv_sendRecord.Visible = false;
                buy_pager.Visible = true;
                grv_buyRecord.Visible = true;
                buy_pager.RowCount = pagination.RowCount;
            }
            else
            {
                grv_buyRecord.Visible = false;
                grv_sendRecord.Visible = false;
                showempty.Visible = true;
                buy_pager.Visible = false;
                send_pager.Visible = false;
            }
        }

        ChinaPay.SMS.DataTransferObject.OrderQueryCondition orderQueryCondition()
        {
            ChinaPay.SMS.DataTransferObject.OrderQueryCondition condiition = new SMS.DataTransferObject.OrderQueryCondition();
            condiition.Account = CurrentCompany.CompanyId;

            condiition.PurchaseDateRange = new Range<DateTime?>() { Lower = DateTime.Parse(txtBuyStartTime.Text), Upper = DateTime.Parse(txtBuyEndTime.Text).AddDays(1).AddMilliseconds(-1) };

            return condiition;
        }

        ChinaPay.SMS.DataTransferObject.SendRecordQueryCondition sendQueryCondition()
        {
            ChinaPay.SMS.DataTransferObject.SendRecordQueryCondition condiition = new SMS.DataTransferObject.SendRecordQueryCondition();
            //condiition.OperatorAccount = CurrentUser.UserName;
            condiition.Account = CurrentCompany.CompanyId;
            condiition.SendDateRange = new Range<DateTime?>() { Lower = DateTime.Parse(txtSendStartTime.Text), Upper = DateTime.Parse(txtSendEndTime.Text).AddDays(1).AddMilliseconds(-1) };
            if (!string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                condiition.Mobile = txtPhone.Text.Trim();
            }
            if (drpStatus.SelectedIndex == 1)
            {
                condiition.Status = SMS.DataTransferObject.SendStatus.Committed;
            }
            else if (drpStatus.SelectedIndex == 2)
            {
                condiition.Status = SMS.DataTransferObject.SendStatus.Failed;
            }
            else if (drpStatus.SelectedIndex == 3)
            {
                condiition.Status = SMS.DataTransferObject.SendStatus.Success;
            }
            else if (drpStatus.SelectedIndex == 4)
            {
                condiition.Status = SMS.DataTransferObject.SendStatus.PartFailed;
            }
            return condiition;
        }
        protected void btnSendQuery_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination()
            {
                PageSize = send_pager.PageSize,
                PageIndex = 1,
                GetRowCount = true
            };
            send_pager.CurrentPageIndex = 1;
            InitSendData(pagination);
        }

        protected void btnBuyQuery_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination()
            {
                PageSize = buy_pager.PageSize,
                PageIndex = 1,
                GetRowCount = true
            };
            InitBuyData(pagination);
            buy_pager.CurrentPageIndex = 1;
        }

        protected void btnReSend_Click(object sender, EventArgs e)
        {
            try
            {
                SMSSendService.ReSendMsg(Guid.Parse(hidSendId.Value));
                hidSendId.Value = "";
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "重新发送");
            }
            var pagination = new Pagination()
            {
                PageSize = 10,
                PageIndex = 1,
                GetRowCount = true
            };
            send_pager.CurrentPageIndex = 1;
            InitSendData(pagination);
        }
    }
}