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
    public partial class SMMSendRecords : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                //InitSendData(new Pagination() { PageSize = 10, PageIndex = 1, GetRowCount = true }); 
                txtSendStartTime.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtSendEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            this.send_pager.CurrentPageChanged += send_pager_CurrentPageChanged;
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
                                                Status = item.Status.GetDescription(),
                                                item.OperatorAccount,
                                                FiadPhone = item.Status == SMS.DataTransferObject.SendStatus.PartFailed || item.Status == SMS.DataTransferObject.SendStatus.Failed ? item.FailedMobiles.Join("<br />") : ""
                                            };
                grv_sendRecord.DataBind();
                showempty.Visible = false;
                send_pager.Visible = true;
                grv_sendRecord.Visible = true;
                send_pager.RowCount = pagination.RowCount;
            }
            else
            {
                grv_sendRecord.Visible = false;
                showempty.Visible = true;
                send_pager.Visible = false;
            }
        }

        ChinaPay.SMS.DataTransferObject.SendRecordQueryCondition sendQueryCondition()
        {
            ChinaPay.SMS.DataTransferObject.SendRecordQueryCondition condiition = new SMS.DataTransferObject.SendRecordQueryCondition();
            condiition.SendDateRange = new Range<DateTime?>() { Lower = DateTime.Parse(txtSendStartTime.Text), Upper = DateTime.Parse(txtSendEndTime.Text).AddDays(1).AddMilliseconds(-1) };
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
            if (!string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                condiition.Mobile = txtPhone.Text.Trim();
            }
            if (!string.IsNullOrEmpty(txtAccountNo.Text.Trim()))
            {
                condiition.OperatorAccount = txtAccountNo.Text.Trim();
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
    }
}