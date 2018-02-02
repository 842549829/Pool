using System;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using ChinaPay.Core;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class BasePrice : BasePage
    {
        readonly int m_pageSize = 10;

        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropList();
                this.Pagerl.Visible = false;
                LoadCondition("BasePrice");
                this.btnSelect_Click(this, e);
            }
            this.Pagerl.PageSize = m_pageSize;
            this.Pagerl.CurrentPageChanged += Pager1_CurrentPageChanged;
        }
        private void DropList()
        {
            foreach (var item in FoundationService.Airlines)
                this.ddlAirline.Items.Add(new ListItem((item.Code.Value + "-" + item.ShortName), item.Code.Value));
            this.ddlAirline.Items.Insert(0, new ListItem("-所有-", ""));

            foreach (var item in FoundationService.Airports)
            {
                string name = item.Code.Value + "-" + item.ShortName;
                this.drpDepartAirport.Items.Add(new ListItem(name, item.Code.Value));
                this.drpArrivedAirport.Items.Add(new ListItem(name, item.Code.Value));
            }
            this.drpArrivedAirport.Items.Insert(0, new ListItem("-所有-", ""));
            this.drpDepartAirport.Items.Insert(0, new ListItem("-所有-", ""));
        }
        #endregion

        #region 查询
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination()
            {
                PageSize = m_pageSize,
                PageIndex = IsLoacCondition ? Pagerl.CurrentPageIndex : Pagerl.CurrentPageIndex = 1,
                GetRowCount = true
            };
            bindDatas(pagination);
        }
        void Pager1_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = m_pageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            bindDatas(pagination);
        }
        void refresh()
        {
            var pagination = new Pagination()
            {
                PageSize = m_pageSize,
                PageIndex = this.Pagerl.CurrentPageIndex,
                GetRowCount = true
            };
            bindDatas(pagination);
        }
        void bindDatas(Pagination pagination)
        {
            try
            {
                var totalDatas = from item in FoundationService.QueryBasicPriceView(this.ddlAirline.SelectedValue.Trim(),
                                                                       this.drpDepartAirport.SelectedValue.Trim(),
                                                                       this.drpArrivedAirport.SelectedValue.Trim(), pagination)
                                 select new
            {
                AirlineCode = item.Airline,
                AirlineShortName =string.IsNullOrWhiteSpace(item.Airline)?"": FoundationService.QueryAirline(item.Airline).ShortName,
                DepartureShortName = FoundationService.QueryAirport(item.Departure) == null ? "" : FoundationService.QueryAirport(item.Departure).ShortName,
                ArrivalShortName = FoundationService.QueryAirport(item.Arrival)== null?"" :FoundationService.QueryAirport(item.Arrival).ShortName,
                FlightDate = item.FlightDate,
                ETDZDate = item.ETDZDate,
                Price = item.Price,
                Mileage = item.Mileage,
                Id = item.Id
            };
                this.gvBasePrice.DataSource = totalDatas;
                this.gvBasePrice.DataBind();
                if (totalDatas.Count() > 0)
                {
                    this.Pagerl.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.Pagerl.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    this.Pagerl.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }
        #endregion

        #region 删除
        protected void gvBasePrice_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            LinkButton linkButton = this.gvBasePrice.Rows[e.RowIndex].FindControl("linkDel") as LinkButton;
            if (linkButton != null)
            {
                try
                {
                    FoundationService.DeleteBasicPrice(new Guid(linkButton.CommandArgument), CurrentUser.UserName);
                    RegisterScript("alert('删除成功！'); window.location.href='BasePrice.aspx';");
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "删除");
                    return;
                }
                refresh();
            }
        }
        #endregion

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            var systemDicionary = ChinaPay.B3B.Service.SystemManagement.SystemDictionaryService.Query(Service.SystemManagement.Domain.SystemDictionaryType.SystemRefreshCacheAddress);
            var key = Utility.MD5EncryptorService.MD5FilterZero(System.Configuration.ConfigurationManager.AppSettings["SignKey"], "utf-8");
            foreach (var item in systemDicionary)
            {
                ChinaPay.Utility.HttpRequestUtility.GetHttpResult(item.Value + "?Action=Flush&Target=PRICE&Key=" + key, 3000);
            }
            ShowMessage("刷新成功");
        }
    }
}