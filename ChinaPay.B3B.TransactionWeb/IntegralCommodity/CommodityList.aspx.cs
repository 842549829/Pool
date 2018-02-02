using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Commodity;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.IntegralCommodity
{
    public partial class CommodityList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");

            if (!IsPostBack)
            { 
                Pagination pagination = new Pagination
                {
                    GetRowCount = true,
                    PageIndex = 1,
                    PageSize = pager.PageSize
                };
                pager.CurrentPageIndex = 1;
                Query(pagination);
            }
            this.pager.CurrentPageChanged += pager_CurrentPageChanged;
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

        private void Query(Pagination pagination)
        {
            try
            {
                var query_list = CommodityServer.GetCommodityList(false, pagination);
                var list = from item in query_list
                           select new
                           {
                               item.ID,
                               item.Num,
                               item.StockNumber,
                               item.SurplusNumber,
                               item.CommodityName,
                               item.NeedIntegral,
                               item.ExchangeNumber,
                               State = item.State ? "正常" : "禁用",
                               StateInfo = item.State ? "禁用" : "启用",
                               ShelvesInfo = "<a href='javascript:ShangJia(\"" + item.ID + "\",\"" + item.SurplusNumber + "\");'>上下架</a>",
                               StateCmd = item.State ? "disable" : "enable"
                           };
                this.grv_commodity.DataSource = list;
                this.grv_commodity.DataBind();
                if (query_list.Any())
                {
                    this.pager.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pager.RowCount = pagination.RowCount;
                    }
                    grv_commodity.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                else
                {
                    this.pager.Visible = false;
                    showempty.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            NewMethod(1);
        }

        protected void grv_commodity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "disable")
                {
                    CommodityServer.UpdateState(Guid.Parse(e.CommandArgument.ToString()), false, this.CurrentUser.UserName);
                }
                if (e.CommandName == "enable")
                {
                    CommodityServer.UpdateState(Guid.Parse(e.CommandArgument.ToString()), true, this.CurrentUser.UserName);
                }
                NewMethod(grv_commodity.PageIndex + 1);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "修改");
            }
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

        protected void btnOk_Click(object sender, EventArgs e)
        {
            string msg = "下架";
            try
            {
                if (selOption.SelectedIndex == 0)
                {
                    msg = "上架";
                    CommodityServer.UpdateShelvesNum(Guid.Parse(hidId.Value), int.Parse(txtNum.Text), this.CurrentUser.UserName);
                }
                else
                {
                    CommodityServer.UpdateShelvesNum(Guid.Parse(hidId.Value), 0 - int.Parse(txtNum.Text), this.CurrentUser.UserName);
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, msg);
            }
            selOption.SelectedIndex = 0;
            txtNum.Text = "";
            hidId.Value = "";
            hidNum.Value = "";
            NewMethod(grv_commodity.PageIndex + 1);
        }
    }
}