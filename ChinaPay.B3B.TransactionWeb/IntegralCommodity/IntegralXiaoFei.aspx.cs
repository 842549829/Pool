using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Integral;
using ChinaPay.Core;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.IntegralCommodity
{
    public partial class IntegralXiaoFei : BasePage
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
                var query_list = IntegralServer.GetIntegralConsumptionList(time, CurrentUser.Owner, (IntegralWay)int.Parse(XiaoFei.Value), ExchangeState.All,null, "",null, pagination);
                var list = from item in query_list
                           select new
                           {
                               item.ExchangeTiem,
                               Way = item.Way.GetDescription(),
                               item.ConsumptionIntegral,
                               Remark = item.Exchange == ExchangeState.Refuse ? "拒绝兑换商品，拒绝原因：<br />" + item.Reason + "<br /><a href='javascript:show(\"" + item.CommodityName + "\",\"" + item.CommodityCount + "\",\"" + item.ConsumptionIntegral + "\",\"" + item.DeliveryAddress + "\",\"" + item.ExpressCompany + "\",\"" + item.ExpressDelivery + "\",\"" + item.Exchange + "\");'>" + item.CommodityName + " | " + item.CommodityCount + "件</a>" : item.Way == IntegralWay.Exchange || item.Way == IntegralWay.ExchangeSms ? "<a href='javascript:show(\"" + item.CommodityName + "\",\"" + item.CommodityCount + "\",\"" + item.ConsumptionIntegral + "\",\"" + item.DeliveryAddress + "\",\"" + item.ExpressCompany + "\",\"" + item.ExpressDelivery + "\",\"" + item.Exchange + "\");'>" + item.CommodityName + " | " + item.CommodityCount + "件</a>" : (item.Way == IntegralWay.TuiPiao ? "<a href='/OrderModule/Purchase/RefundApplyformDetail.aspx?id=" + item.Remark + "'>" + item.Remark + "</a>" : item.Way.GetDescription())
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