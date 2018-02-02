using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.SMS.Service;
using ChinaPay.Core;
using ChinaPay.SMS.Service.Domain;
using ChinaPay.SMS.DataTransferObject;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SmsModule
{
    public partial class SMSBuyRecord : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                txtStartTime.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                ddlPackback.DataSource = from item in SMSProductService.Query()
                                         orderby item.Amount
                                         select new
                                         {
                                             value = item.Id,
                                             text = item.Amount
                                         };
                ddlPackback.DataValueField = "value";
                ddlPackback.DataTextField = "text";
                ddlPackback.DataBind();
                ddlPackback.Items.Insert(0, "所有");
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    ddlPackback.SelectedValue = Request.QueryString["id"];
                }
                InitDataGrid(new Pagination() { PageSize = pager.PageSize, PageIndex = 1, GetRowCount = true });
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
        }
        void InitDataGrid(Pagination pagination)
        {
            var query = SMSOrderService.QueryOrders(Condition(), pagination);

            if (query.Any())
            {
                grv_Record.DataSource = from item in query
                                        select new
                                            {
                                                Amount = item.IsExChange ? item.Product.Amount.TrimInvaidZero() + "分" : item.Product.Amount.ToString("C"),
                                                Count = item.Count,
                                                item.CompanyUserName,
                                                PurchaseAccount = item.PurchaseAccount == null ? "" : item.PurchaseAccount,
                                                PayTime = item.Payment == null ? "" : item.Payment.PayTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                                PayType = item.IsExChange ? "积分兑换" : item.Payment == null ? "" : item.Payment.IsPoolPay ? "国付通" : "非国付通"
                                            };
                grv_Record.DataBind();
                showempty.Visible = false;
                pager.Visible = true;
                grv_Record.Visible = true;
                pager.RowCount = pagination.RowCount;
            }
            else
            {
                grv_Record.Visible = false;
                showempty.Visible = true;
                pager.Visible = false;
            }
        }
        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            InitDataGrid(pagination);
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination()
            {
                PageSize = pager.PageSize,
                PageIndex = 1,
                GetRowCount = true
            };
            pager.CurrentPageIndex = 1;
            InitDataGrid(pagination);
        }
        OrderQueryCondition Condition()
        {
            return new OrderQueryCondition()
            {
                ProductId = ddlPackback.SelectedIndex == 0 ? (Guid?)null : Guid.Parse(ddlPackback.SelectedValue),
                AccountNo = txtAccountNo.Text,
                Company = txtCompany.Text,
                PurchaseDateRange = new Range<DateTime?>() { Lower = DateTime.Parse(txtStartTime.Text), Upper = DateTime.Parse(txtEndTime.Text) }
            };
        }
    }
}