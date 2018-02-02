using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public partial class special_policy_info : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                InitlblData();
                setBackButton();
            }
        }

        private void InitlblData()
        {
            SpecialPolicy Special = PolicyManageService.GetSpecialPolicy(Guid.Parse(Request.QueryString["id"].ToString()));
            if (Special != null)
            {
                lowTr.Visible = false;
                lblAirline.Text = Special.Airline;
                lblVoyage.Text = Special.VoyageType.GetDescription();
                lblOfficeNo.Text = Special.OfficeCode;
                lblSpecialType.Text = Special.Type.GetDescription();
                lblDeparture.Text = Special.Departure;
                lblArrival.Text = Special.Arrival;
                lblDepartureDate.Text = (Special.DepartureDateStart == null ? "" : Special.DepartureDateStart.ToString("yyyy-MM-dd")) + "至" + (Special.DepartureDateEnd == null ? "" : Special.DepartureDateEnd.ToString("yyyy-MM-dd"));
                lblOutWithFilght.Text = Special.DepartureFlightsFilterType == LimitType.None ? "所有" : (Special.DepartureFlightsFilterType == LimitType.Include ? "适用：" + Special.DepartureFlightsFilter : "不适用：" + Special.DepartureFlightsFilter);
                lblExceptDay.Text = Special.DepartureDateFilter;
                lblCreateTime.Text = Special.ProvideDate.ToString("yyyy-MM-dd");
                lblCustomCode.Text = Special.CustomCode;
                lblRetreat.Text = "作废规定：" + Special.InvalidRegulation + "<br />"
                                + "改签规定：" + Special.ChangeRegulation + "<br />"
                                + "签转规定：" + Special.EndorseRegulation + "<br />"
                                + "退票规定：" + Special.RefundRegulation + "<br />";

                lblDays.Text = Special.BeforehandDays > -1 ? Special.BeforehandDays + "天" : "";
                lblChang.Text = Special.ConfirmResource ? "需要" : "不需要";
                lblDrawerCondition.Text = Special.DrawerCondition;
                lblRemark.Text = Special.Remark;
                lblPrintBeforeTwoHours.Text = Special.PrintBeforeTwoHours ? "可以" : "不可以";
                if (Special.PriceType == PriceType.Price)
                {
                    lblPrice.Text = Special.Price == -1 ? "" : "￥" + Special.Price.TrimInvaidZero();
                    if (Special.IsInternal)
                    {
                        lblInternal.Text = "￥" + Special.InternalCommission.TrimInvaidZero();
                    }
                    else
                    {
                        internalTitle.Visible = false;
                        internalValue.Visible = false;
                    }
                    lblSubOrdinate.Text = Special.SubordinateCommission.TrimInvaidZero() == "-1" ? "" : "￥" + Special.SubordinateCommission.TrimInvaidZero();
                    if (Special.IsPeer)
                    {
                        lblProfession.Text = "￥" + Special.ProfessionCommission.TrimInvaidZero();
                    }
                    else
                    {
                        professionTitle.Visible = false;
                        professionValue.Visible = false;
                    }
                }
                else
                {
                    lblPrice.Text = Special.Price == -0.01M ? "" : "直减 " + (Special.Price * 100).TrimInvaidZero() + "%";
                    if (Special.IsInternal)
                    {
                        lblInternal.Text = (Special.InternalCommission * 100).TrimInvaidZero() + "%";
                    }
                    else
                    {
                        internalTitle.Visible = false;
                        internalValue.Visible = false;
                    }
                    lblSubOrdinate.Text = Special.SubordinateCommission == -1 ? "" : (Special.SubordinateCommission * 100).TrimInvaidZero() + "%";
                    if (Special.IsPeer)
                    {
                        lblProfession.Text = (Special.ProfessionCommission * 100).TrimInvaidZero() + "%";
                    }
                    else
                    {
                        professionTitle.Visible = false;
                        professionValue.Visible = false;
                    }
                }
                if (Special.Type == SpecialProductType.CostFree)
                {
                    lblPrice.Text = Special.Price == -1 ? "" : "￥" + Special.Price.TrimInvaidZero();
                    freeTicket.Visible = true;
                    productNumberTitle.Visible = false;
                    productNumberValue.Visible = false;
                    lblExceptAirlines.Text = Special.ExceptAirways;
                    if (Special.SynBlackScreen)
                    {
                        lblIsSynsy.Text = "同步：舱位 " + Special.Berths;
                    }
                    else
                    {
                        lblIsSynsy.Text = "不同步：可提供产品数量 " + Special.ResourceAmount;
                    }
                }
                if (Special.Type == SpecialProductType.Bloc || Special.Type == SpecialProductType.Business)
                {
                    productNumberTitle.Visible = false;
                    productNumberValue.Visible = false;

                    bussiness.Visible = true;
                    if (Special.Type == SpecialProductType.Bloc)
                    {
                        this.lblTicketType.Text = Special.TicketType.GetDescription();

                    }
                    else
                    {
                        this.ticketTypeTitle.Visible = false;
                        this.ticketTypeValue.Visible = false;
                    }
                    lowTr.Visible = true;
                    lblLowtype.Text = Special.LowNoType.GetDescription();
                    lblLowPrice.Text = Special.LowNoType == LowNoType.LowInterval ? (Special.LowNoType == LowNoType.LowInterval ? "票面价区间：" + Special.LowNoMinPrice.TrimInvaidZero() + "元(包含)至" + Special.LowNoMaxPrice.TrimInvaidZero() + "元(包含)" : "") : "无";
                }
                if (Special.Type == SpecialProductType.LowToHigh)
                {
                    productNumberTitle.Visible = false;
                    productNumberValue.Visible = false;

                    bussiness.Visible = true;

                    this.ticketTypeTitle.Visible = false;
                    this.ticketTypeValue.Visible = false;
                    lowTr.Visible = false;

                    lblPrice.Text = "";
                    if (Special.IsInternal)
                    {
                        lblInternal.Text = "返佣：" + (Special.InternalCommission * 100).TrimInvaidZero() + "%";
                    }
                    else
                    {
                        internalTitle.Visible = false;
                        internalValue.Visible = false;
                    }
                    lblSubOrdinate.Text = "返佣：" + (Special.SubordinateCommission == -1 ? "" : (Special.SubordinateCommission * 100).TrimInvaidZero() + "%");
                    if (Special.IsPeer)
                    {
                        lblProfession.Text = "返佣：" + (Special.ProfessionCommission * 100).TrimInvaidZero() + "%";
                    }
                    else
                    {
                        professionTitle.Visible = false;
                        professionValue.Visible = false;
                    }
                }
                else
                {
                    lblPrice.Text = Special.Price == -1 ? "" : "￥" + Special.Price.TrimInvaidZero();
                }

                lblExceptAirlines.Text = Special.ExceptAirways;

                lblNum.Text = Special.ResourceAmount == -1 ? "" : Special.ResourceAmount + "张";
                lblBunks.Text = Special.Berths;
            }
            else
            {
                ShowMessage("该政策已不存在");
            }
        }


        private void setBackButton()
        {
            // 返回
            var returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = (Request.UrlReferrer ?? Request.Url).PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
    }
}