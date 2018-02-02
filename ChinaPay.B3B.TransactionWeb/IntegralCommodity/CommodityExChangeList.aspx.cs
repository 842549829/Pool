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
    public partial class CommodityExChangeList : BasePage
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
                var query_list = IntegralServer.GetIntegralConsumptionList(time, null, IntegralWay.Exchange, (ExchangeState)int.Parse(XiaoFei.Value),CurrentCompany.CompanyType == CompanyType.Platform ?  Common.Enums.OEMCommodityState.Success : (OEMCommodityState?)null, "1",null, pagination);
                var list = from item in query_list
                           select new
                           {
                               ID = item.Id,
                               item.ExchangeTiem,
                               item.CommodityName,
                               item.CompanyShortName,
                               item.AccountNo,
                               item.AccountName,
                               Count = item.CommodityCount,
                               Integral = item.ConsumptionIntegral,
                               Phone = item.AccountPhone,
                               Exchange = item.Exchange.GetDescription(),
                               item.ConsumptionIntegral,
                               OEMName = item.OEMName,
                               Remark = item.Exchange == ExchangeState.Processing ? "<a href='CommodityExChange.aspx?id=" + item.Id + "&type=chuli'>兑换处理</a>" : "<a href='CommodityExChange.aspx?id=" + item.Id + "&type=look'>查看详细</a>"
                           };
                grv_xiaofei.DataSource = list;
                grv_xiaofei.DataBind();
                if (query_list.Any())
                {
                    this.pager.Visible = true;
                    showempty.Visible = false;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    grv_xiaofei.HeaderRow.TableSection = TableRowSection.TableHeader;
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