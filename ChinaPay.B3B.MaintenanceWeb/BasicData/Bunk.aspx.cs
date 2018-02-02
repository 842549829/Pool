using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class Bunk : BasePage
    {
        readonly int m_pageSize = 10;

        #region 数据加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropList();
                this.pagerl.Visible = false;
                LoadCondition("Bunk");
                this.btnSelect_Click(this, e);
            }
            this.pagerl.PageSize = m_pageSize;
            this.pagerl.CurrentPageChanged += Pager1_CurrentPageChanged;
        }
        /// <summary>
        /// 列表绑定
        /// </summary>
        private void DropList()
        {
            foreach (var item in FoundationService.Airlines)
            {
                this.ddlAirline.Items.Add(new ListItem(item.Code.Value + "-" + item.ShortName, item.Code.Value));
            }
            this.ddlAirline.Items.Insert(0, new ListItem("-所有-", ""));
            foreach (var item in FoundationService.Airports)
            {
                string name = item.Code.Value + "-" + item.ShortName;
                this.ddlDeparture.Items.Add(new ListItem(name, item.Code.Value));
                this.ddlArrival.Items.Add(new ListItem(name, item.Code.Value));
            }
            this.ddlDeparture.Items.Insert(0, new ListItem("-所有-", ""));
            this.ddlArrival.Items.Insert(0, new ListItem("-所有-", ""));

            dropBunk.DataSource = (Enum.GetValues(typeof(BunkType)) as BunkType[]).Select(item => new KeyValuePair<int, string>((int)item, item.GetDescription()));
            dropBunk.DataTextField = "Value";
            dropBunk.DataValueField = "Key";
            dropBunk.DataBind();
            dropBunk.Items.Insert(0, new ListItem("-所有-", ""));

            dropVoyageType.DataSource = (Enum.GetValues(typeof(VoyageTypeValue)) as VoyageTypeValue[]).Select(item => new KeyValuePair<int, string>((int)item, item.GetDescription()));
            dropVoyageType.DataTextField = "Value";
            dropVoyageType.DataValueField = "Key";
            dropVoyageType.DataBind();
            dropVoyageType.Items.Insert(0, new ListItem("-所有-", ""));
        }
        //航空公司状态
        public string GetState(string enabled)
        {
            return Convert.ToBoolean(enabled) == true ? "启用" : "禁用";
        }
        //操作状态
        public string ButtonState(string enabled)
        {
            return Convert.ToBoolean(enabled) == true ? "禁用" : "启用";
        }
        /// <summary>
        /// 类型转化
        /// </summary>
        /// <param name="bunk">Bunk类型</param>
        /// <returns>NewBunks</returns>
        private ListBunkView GetListBunk(Service.Foundation.Domain.Bunk bunk)
        {
            var result = new ListBunkView();
            result.Id = bunk.Id.ToString();
            result.FlightBeginDate = bunk.FlightBeginDate.ToString("yyyy-MM-dd");
            if (bunk.FlightEndDate != null) result.FlightEndDate = bunk.FlightEndDate.Value.ToString("yyyy-MM-dd");
            result.ETDZDate = bunk.ETDZDate.ToString("yyyy-MM-dd");
            result.BunkType = bunk.Type.GetDescription();
            result.BunkCode = bunk.Code.Value;
            var voyageType = (Enum.GetValues(typeof(VoyageTypeValue)) as VoyageTypeValue[]).Select(item => new KeyValuePair<int, string>((int)item, item.GetDescription()));
            result.VoyageType = string.Join("<Br/>",
                voyageType.Where(q => (VoyageTypeValue)q.Key == (bunk.VoyageType & (VoyageTypeValue)q.Key)).Select(item => item.Value));
            var extendBunks = string.Empty;
            if (bunk is GeneralBunk)
            {
                extendBunks = (bunk as GeneralBunk).Extended.Join(",", item => item.Code.Value);
            }
            else if (bunk is PromotionBunk)
            {
                extendBunks = (bunk as PromotionBunk).Extended.Join(",");
            }
            if (extendBunks.Length > 0)
            {
                result.BunkCode += "," + extendBunks;
            }
            result.Valid = bunk.Valid;
            result.AirlineCode = bunk.AirlineCode.Value;
            if (bunk is GeneralBunk)
            {
                var generalBunk = bunk as GeneralBunk;
                result.DepartAriport = generalBunk.DepartureCode.IsNullOrEmpty() ? "所有" : generalBunk.DepartureCode.Value;
                result.ArriveAriport = generalBunk.ArrivalCode.IsNullOrEmpty() ? "所有" : generalBunk.ArrivalCode.Value;
            }
            else
            {
                result.DepartAriport = "所有";
                result.ArriveAriport = "所有";
            }
            return result;
        }
        /// <summary>
        /// 舱位
        /// </summary>
        public class ListBunkView
        {
            /// <summary>
            /// GuId
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// 航班开始是日期
            /// </summary>
            public string FlightBeginDate { get; set; }
            /// <summary>
            /// 航班截止是日期
            /// </summary>
            public string FlightEndDate { get; set; }
            /// <summary>
            /// 出票时间
            /// </summary>
            public string ETDZDate { get; set; }
            /// <summary>
            /// 航空公司代码
            /// </summary>
            public string AirlineCode { get; set; }
            /// <summary>
            /// 舱位代码
            /// </summary>
            public string BunkCode { get; set; }
            /// <summary>
            /// 航班类型
            /// </summary>
            public string BunkType { get; set; }
            /// <summary>
            /// 状态
            /// </summary>
            public bool Valid { get; set; }
            /// <summary>
            /// 出发机场
            /// </summary>
            public string DepartAriport { get; set; }
            /// <summary>
            /// 到达机场
            /// </summary>
            public string ArriveAriport { get; set; }
            /// <summary>
            /// 适用行程
            /// </summary>
            public string VoyageType { get; set; }
        }
        #endregion

        #region 查询
        protected void btnSelect_Click(object sender, EventArgs e)
        {
            var pagination = new Pagination()
            {
                PageSize = m_pageSize,
                PageIndex = IsLoacCondition ? pagerl.CurrentPageIndex : pagerl.CurrentPageIndex = 1,
                GetRowCount = true
            };
            bindBunks(pagination);
        }
        void Pager1_CurrentPageChanged(UserControl.Pager sender, int newPage)
        {
            var pagination = new Pagination()
            {
                PageSize = m_pageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            bindBunks(pagination);
        }
        void refresh()
        {
            var pagination = new Pagination()
            {
                PageSize = m_pageSize,
                PageIndex = this.pagerl.CurrentPageIndex,
                GetRowCount = true
            };
            bindBunks(pagination);
        }
        void bindBunks(Pagination pagination)
        {
            try
            {
                var condition = GetCondition();
                var totalDatas = FoundationService.QueryBunkListView(condition, pagination).Select(GetListBunk);   
                this.gvDiscount.DataSource = totalDatas;
                this.gvDiscount.DataBind();
                if (totalDatas.Count() > 0)
                {
                    this.pagerl.Visible = true;
                    if (pagination.GetRowCount)
                    {
                        this.pagerl.RowCount = pagination.RowCount;
                    }
                }
                else
                {
                    this.pagerl.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "查询");
            }
        }

        bool existsInExtendedBunk(Service.Foundation.Domain.Bunk bunk, string bunkCode)
        {
            if (bunk is GeneralBunk)
            {
                var generalBunk = bunk as GeneralBunk;
                return generalBunk.Extended.Any(item => item.Code.Value == bunkCode);
            }
            else if (bunk is PromotionBunk)
            {
                var promotionBunk = bunk as PromotionBunk;
                return promotionBunk.Extended.Any(item => item == bunkCode);
            }
            else
            {
                return false;
            }
        }

        BunkQueryCondition GetCondition()
        {
            var condition = new BunkQueryCondition();
            condition.Departure = this.ddlDeparture.SelectedValue.Trim();
            condition.Arrival = this.ddlArrival.SelectedValue.Trim();
            condition.BunkCode = this.txtCwCode.Text.Trim().ToUpper();
            if (!string.IsNullOrEmpty(this.dropBunk.SelectedValue))
            {
                condition.BunkType = (BunkType)int.Parse(this.dropBunk.SelectedValue);
            }
            if (!string.IsNullOrEmpty(this.dropVoyageType.SelectedValue))
            {
                condition.VoyageType = (VoyageTypeValue)int.Parse(this.dropVoyageType.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.txtStartTime.Text))
            {
                condition.FlightBeginDate = DateTime.Parse(this.txtStartTime.Text.Trim());
            }
            if (!string.IsNullOrWhiteSpace(this.txtStopTime.Text))
            {
                condition.FlightEndDate = DateTime.Parse(this.txtStopTime.Text.Trim());
            }
            if (!string.IsNullOrWhiteSpace(this.ddlStatus.SelectedValue))
            {
                condition.Status = Convert.ToBoolean(this.ddlStatus.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.ddlAirline.SelectedValue))
            {
                condition.Airline = this.ddlAirline.SelectedValue;
            }
            return condition;
        }
        #endregion

        #region 状态控制
        protected void gvDiscount_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "opdate")
            {
                string code = e.CommandArgument.ToString();
                ChinaPay.B3B.Service.Foundation.Domain.Bunk bunk = FoundationService.QueryBunkNew(new Guid(code));
                if (bunk == null) return;
                BunkView bunkView = null;
                switch (bunk.Type)
                {
                    case BunkType.Economic:
                        EconomicBunk economicBunk = bunk as EconomicBunk;
                        var economicBunkView = new EconomicBunkView()
                        {
                            Arrival = economicBunk.ArrivalCode.Value,
                            Departure = economicBunk.DepartureCode.Value,
                            Discount = economicBunk.Discount
                        };
                        foreach (var item in economicBunk.Extended)
                            economicBunkView.AddExtended(new ExtendedWithDiscountBunkView { Code = item.Code.Value, Discount = item.Discount });
                        bunkView = economicBunkView;
                        break;
                    case BunkType.FirstOrBusiness:
                        FirstBusinessBunk firstBusinessBunk = bunk as FirstBusinessBunk;
                        var firstBusinessBunkView = new FirstBusinessBunkView()
                        {
                            Arrival = firstBusinessBunk.ArrivalCode.Value,
                            Departure = firstBusinessBunk.DepartureCode.Value,
                            Description = firstBusinessBunk.Description,
                            Discount = firstBusinessBunk.Discount
                        };
                        foreach (var item in firstBusinessBunk.Extended)
                            firstBusinessBunkView.AddExtended(new ExtendedWithDiscountBunkView { Code = item.Code.Value, Discount = item.Discount });
                        bunkView = firstBusinessBunkView;
                        break;
                    case BunkType.Promotion:
                        PromotionBunk promotionBunk = bunk as PromotionBunk;
                        var promotionBunkView = new PromotionBunkView() { Description = promotionBunk.Description };
                        foreach (var item in promotionBunk.Extended)
                            promotionBunkView.AddExtended(item);
                        bunkView = promotionBunkView;
                        break;
                    case BunkType.Production:
                        bunkView = new ProductionBunkView();
                        break;
                    case BunkType.Transfer:
                        bunkView = new TransferBunkView();
                        break;
                    case BunkType.Free:
                        FreeBunk freeBunk = bunk as FreeBunk;
                        bunkView = new FreeBunkView() { Description = freeBunk.Description };
                        break;
                    case BunkType.Team:
                        bunkView = new TeamBunkView();
                        break;
                    default:
                        throw new Exception("舱位不存在");
                }
                bunkView.Code = bunk.Code.Value;
                bunkView.RefundRegulation = bunk.RefundRegulation;
                bunkView.ChangeRegulation = bunk.ChangeRegulation;
                bunkView.EndorseRegulation = bunk.EndorseRegulation;
                bunkView.VoyageType = bunk.VoyageType;
                bunkView.PassengerType = bunk.PassengerType;
                bunkView.TravelType = bunk.TravelType;
                bunkView.Remarks = bunk.Remarks;
                bunkView.FlightBeginDate = bunk.FlightBeginDate;
                bunkView.FlightEndDate = bunk.FlightEndDate;
                bunkView.ETDZDate = bunk.ETDZDate;
                bunkView.Airline = bunk.AirlineCode.Value;
                bunkView.Valid = !bunk.Valid;
                try
                {
                    FoundationService.UpdateBunk(new Guid(code), bunkView, CurrentUser.UserName);
                    if (bunk.Valid)
                        RegisterScript("alert('禁用成功！'); window.location.href='Bunk.aspx?Search=Back';");
                    else
                        RegisterScript("alert('启用成功！'); window.location.href='Bunk.aspx?Search=Back';");
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, bunk.Valid ? "禁用" : "启用");
                    return;
                }
                refresh();
            }
        }
        #endregion

        #region 删除
        protected void gvDiscount_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            LinkButton linkButton = this.gvDiscount.Rows[e.RowIndex].FindControl("linkDel") as LinkButton;
            if (linkButton != null)
            {
                try
                {
                    FoundationService.DeleteBunk(new Guid(linkButton.CommandArgument), CurrentUser.UserName);
                    RegisterScript("alert('删除成功！');");
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "删除");
                    return;
                }
                var pagination = new Pagination()
                {
                    PageSize = m_pageSize,
                    PageIndex = 1,
                    GetRowCount = true
                };
                bindBunks(pagination);
            }
        }
        #endregion

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            var systemDicionary = Service.SystemManagement.SystemDictionaryService.Query(Service.SystemManagement.Domain.SystemDictionaryType.SystemRefreshCacheAddress);
            var key = Utility.MD5EncryptorService.MD5FilterZero(System.Configuration.ConfigurationManager.AppSettings["SignKey"], "utf-8");
            foreach (var item in systemDicionary)
            {
                ChinaPay.Utility.HttpRequestUtility.GetHttpResult(item.Value + "?Action=Flush&Target=BUNK&Key=" + key, 3000);
            }
            ShowMessage("刷新成功");
        }
    }
}
