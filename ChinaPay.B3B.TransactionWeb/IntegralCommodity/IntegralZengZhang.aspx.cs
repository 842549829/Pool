using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Integral;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.IntegralCommodity
{
    public partial class IntegralZengZhang : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");

            if (!IsPostBack)
            {
                txtStartTime.Text = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                txtEndTime.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }
            this.pager.CurrentPageChanged += new UserControl.CurrentPageChangedEventHandler(pager_CurrentPageChanged);
        }
        void pager_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            Pagination pagination = new Pagination
            {
                GetRowCount = true,
                PageIndex = newPage,
                PageSize = pager.PageSize
            };
            pager.CurrentPageIndex = newPage;
            Query(pagination);
        }
        void Query(Pagination pagination)
        {
            try
            {
                Range<DateTime> time = new Range<DateTime> { Lower = DateTime.Parse(txtStartTime.Text), Upper = DateTime.Parse(txtEndTime.Text) };
                var query_list = IntegralServer.GetIntegralList(time, CurrentUser.Owner, (IntegralWay)int.Parse(XiaoFei.Value), pagination);
                var list = from item in query_list
                           select new
                           {
                               ExchangeTiem = item.AccessTime,
                               Way = item.IntegralWay.GetDescription(),
                               ConsumptionIntegral = item.Integral,
                               Remark = item.IntegralWay == IntegralWay.Buy ? "<a href = '/OrderModule/Purchase/OrderDetail.aspx?id=" + item.Remark + "'>" + item.Remark + "</a>" : item.IntegralWay == IntegralWay.RefuseExchange ? item.Remark : item.IntegralWay.GetDescription()
                           };
                grv_zengzhang.DataSource = list;
                grv_zengzhang.DataBind();
                if (query_list.Any())
                {
                    this.pager.Visible = true;
                    showempty.Visible = false;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    grv_zengzhang.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    pager.Visible = false;
                    showempty.Visible = true;
                }
            }
            catch (Exception ex)
            {
                pager.Visible = false;
                showempty.Visible = true;
                ShowExceptionMessage(ex, "查询");
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            NewMethod(1);
        }
        private void NewMethod(int index)
        {
            Pagination pagination = new Pagination
            {
                GetRowCount = true,
                PageIndex = 1,
                PageSize = pager.PageSize
            };
            this.pager.CurrentPageIndex = index;
            Query(pagination);
        }
    }
}