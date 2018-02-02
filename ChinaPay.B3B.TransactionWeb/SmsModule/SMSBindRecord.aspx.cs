using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.SMS.Service;
using ChinaPay.SMS.DataTransferObject;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SmsModule
{
    public partial class SMSBindRecord : UnAuthBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                txtStartTime.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
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
            InitData(pagination);
        }
        protected void btnBindQuery_Click(object sender, EventArgs e)
        {
            Pagination pagination = new Pagination() { GetRowCount = true, PageIndex = 1, PageSize = 10 };
            InitData(pagination);
        }
        void InitData(Pagination pagination)
        {
            var query = SMSCompanySmsParamService.QuerySmsParams(QueryCondtion(), pagination);
            if (query.Any())
            {
                grv_Record.DataSource = from item in query
                                        select new
                                        {
                                            CompanyId = item.CompanyId,
                                            AccountType = item.AccountType.GetDescription(),
                                            AccountNo = item.AccountNo,
                                            CompanyNo = item.CompanyNo + "(" + item.CompanyName + ")",
                                            CompanyType = item.CompanyType.GetDescription(),
                                            OperationTime = item.OperationTime
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
        ParamQueryCondtion QueryCondtion()
        {
            return new ParamQueryCondtion()
            {
                CompanyNo = string.IsNullOrEmpty(txtB3B.Text) ? "" : txtB3B.Text,
                AccountNo = string.IsNullOrEmpty(txtPoolpay.Text) ? "" : txtPoolpay.Text,
                PurchaseDateRange = new Range<DateTime?>() { Lower = DateTime.Parse(txtStartTime.Text), Upper = DateTime.Parse(txtEndTime.Text) }
            };
        }
    }
}