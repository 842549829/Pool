using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.SystemManagement;
using Izual;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Purchase
{
    public partial class Apply : BasePage
    {
        //private Guid ProviderId {
        //    get {
        //        if (ViewState["ProviderId"]==null)
        //        {
        //            return Guid.Empty;
        //        }
        //        return (Guid)ViewState["ProviderId"];
        //    }
        //    set {
        //        ViewState["ProviderId"] = value;
        //    }
        //}

        private string _workTimeSet {
            get
            {
                if(ViewState["WorkTime"]==null) return null;
                return ViewState["WorkTime"].ToString();
            }
            set
            {
                ViewState["WorkTime"] = value;
            }
        }

        protected string GetWorkTimeSet(Order order) {
                if (_workTimeSet==null)
                {
                    var isRestDay = DateTime.Now.IsWeekend();
                    KeyValuePair<Time, Time> scrapHours;
                    KeyValuePair<Time, Time> refundHours;
                    if (order.IsB3BOrder)
                    {
                        var workSetting = CompanyService.GetWorkinghours(order.Provider.CompanyId);
                        if (workSetting == null)
                        {
                            return string.Empty;
                        }
                        scrapHours = new KeyValuePair<Time, Time>(
                            isRestDay ? workSetting.RestdayRefundStart : workSetting.WorkdayRefundStart,
                            isRestDay ? workSetting.RestdayRefundEnd : workSetting.WorkdayRefundEnd);
                        refundHours = new KeyValuePair<Time, Time>(
                            isRestDay ? workSetting.RestdayWorkStart : workSetting.WorkdayWorkStart,
                            isRestDay ? workSetting.RestdayWorkEnd : workSetting.WorkdayWorkEnd);
                    }
                    else
                    {
                        var policy = Service.OrderProcessService.LoadExternalPolicy(order.Id);
                        scrapHours = new KeyValuePair<Time, Time>(
                            isRestDay ? policy.RestWorkTimeStart : policy.WorkTimeStart,
                            isRestDay ? policy.RestWorkTimeEnd : policy.WorkTimeEnd);
                        refundHours = new KeyValuePair<Time, Time>(
                            isRestDay ? policy.RestRefundTimeStart : policy.WorkRefundTimeStart,
                            isRestDay ? policy.RestRefundTimeEnd : policy.WorkRefundTimeEnd);
                    }
                    Time now = new Time(DateTime.Now.Hour, DateTime.Now.Minute, 0);

                    _workTimeSet = string.Format("var param = {{scrapEnable: {0},RefundEnabled:{1},scrapTime:'{2:00}:{3:00}-{4:00}:{5:00}',RefundTime: '{6:00}:{7:00}-{8:00}:{9:00}'}};",
                        scrapHours.Key < now && now < scrapHours.Value ? "true" : "false",
                        refundHours.Key < now && now < refundHours.Value ? "true" : "false",
                        scrapHours.Key.Hour, scrapHours.Key.Minute,
                        scrapHours.Value.Hour, scrapHours.Value.Minute,
                        refundHours.Key.Hour, refundHours.Key.Minute,
                        refundHours.Value.Hour, refundHours.Value.Minute
                        );
                }
                return _workTimeSet;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                hfdServicePhone.Value = CurrenContract.ServicePhone;
                lblServicePhone.Text = CurrenContract.ServicePhone;
                setBackButton();
                var id = Request.QueryString["id"];
                decimal orderId;
                if (decimal.TryParse(id, out orderId))
                {
                    var order = Service.OrderQueryService.QueryOrder(orderId);
                    if (order == null)
                    {
                        showErrorMessage("订单不存在");
                    }
                    else
                    {
                        hidWorkTimeSet.Value = GetWorkTimeSet(order);
                        if (order.Status == DataTransferObject.Order.OrderStatus.Finished)
                        {
                            string lockErrorMsg;
                            if (BasePage.Lock(orderId, Service.Locker.LockRole.Purchaser, "申请退改签", out lockErrorMsg))
                            {
                                bindOrder(order);
                                setButtons(order);
                            }
                            else
                            {
                                showErrorMessage("锁定订单失败。原因:" + lockErrorMsg);
                            }
                        }
                        else
                        {
                            showErrorMessage("仅已出票的订单才能申请退改签");
                        }
                    }
                }
                else
                {
                    showErrorMessage("参数错误");
                }
            }
        }
        private void bindOrder(Service.Order.Domain.Order order)
        {
            bindOrderHeader(order);
            bindPNRGroups(order);
        }
        private void bindOrderHeader(Service.Order.Domain.Order order)
        {
            //ProviderId = order.Provider.CompanyId;
            this.lblOrderId.Text = order.Id.ToString();
            this.hdProvider.Value = order.Provider.CompanyId.ToString();
            this.hidApplyType.Value = order.IsSpecial.ToString().ToUpper();
            IsImport = order.Source  != OrderSource.PlatformOrder;
            this.lblProviderRate.InnerText = SystemParamService.ProviderRate.ToString();
            this.lblResourcerRate.InnerText = (SystemParamService.ResourcerRate + SystemParamService.ProviderRate).ToString();
            this.lblStatus.Text = Service.Order.StatusService.GetOrderStatus(order.Status, DataTransferObject.Order.OrderRole.Purchaser);
            this.lblAmount.Text = order.Purchaser.Amount.ToString("F2");
            var product = order.IsThirdRelation ? order.Supplier.Product : order.Provider.Product;
            if (product is SpeicalProductInfo)
            {
                var specialProductInfo = product as SpeicalProductInfo;
                this.lblProductType.Text = order.Product.ProductType.GetDescription() + "（" + specialProductInfo.SpeicalProductType.GetDescription() + "）";
            }
            else
            {
                this.lblProductType.Text = order.Product.ProductType.GetDescription();
            }
            if (order.Provider != null && order.Provider.Product is Service.Order.Domain.CommonProductInfo)
            {
                this.lblTicketType.Text = (order.Provider.Product as Service.Order.Domain.CommonProductInfo).TicketType.ToString();
            }
            else
            {
                this.lblTicketType.Text = "-";
            }
            this.lblOriginalOrderId.Text = order.AssociateOrderId.HasValue ? order.AssociateOrderId.Value.ToString() : "-";
            this.lblProducedTime.Text = order.Purchaser.ProducedTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.lblPayTime.Text = order.Purchaser.PayTime.HasValue ? order.Purchaser.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";
            this.lblETDZTime.Text = order.ETDZTime.HasValue ? order.ETDZTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";
            var workingHours = Service.Organization.CompanyService.GetWorkinghours(order.Provider.CompanyId);
            //if (DateTime.Today.IsWeekend())
            //{
            //    this.lblWorkingTime.Text = workingHours.RestdayWorkStart.ToString("HH:mm") + " - " + workingHours.RestdayWorkEnd.ToString("HH:mm");
            //    this.lblScrapTime.Text = workingHours.RestdayRefundStart.ToString("HH:mm") + " - " + workingHours.RestdayRefundEnd.ToString("HH:mm");
            //}
            //else
            //{
            //    this.lblWorkingTime.Text = workingHours.WorkdayWorkStart.ToString("HH:mm") + " - " + workingHours.WorkdayWorkEnd.ToString("HH:mm");
            //    this.lblScrapTime.Text = workingHours.WorkdayRefundStart.ToString("HH:mm") + " - " + workingHours.WorkdayRefundEnd.ToString("HH:mm");
            //}
            var isRestDay = DateTime.Now.IsWeekend();
            KeyValuePair<Time, Time> scrapHours;
            KeyValuePair<Time, Time> refundHours;
            if (order.IsB3BOrder)
            {
                var workSetting = CompanyService.GetWorkinghours(order.Provider.CompanyId);
                if (workSetting == null)
                {
                    this.lblWorkingTime.Text = this.lblScrapTime.Text = string.Empty;
                    return;
                }
                scrapHours = new KeyValuePair<Time, Time>(
                    isRestDay ? workSetting.RestdayRefundStart : workSetting.WorkdayRefundStart,
                    isRestDay ? workSetting.RestdayRefundEnd : workSetting.WorkdayRefundEnd);
                refundHours = new KeyValuePair<Time, Time>(
                    isRestDay ? workSetting.RestdayWorkStart : workSetting.WorkdayWorkStart,
                    isRestDay ? workSetting.RestdayWorkEnd : workSetting.WorkdayWorkEnd);
            }
            else
            {
                var policy = Service.OrderProcessService.LoadExternalPolicy(order.Id);
                scrapHours = new KeyValuePair<Time, Time>(
                    isRestDay ? policy.RestWorkTimeStart : policy.WorkTimeStart,
                    isRestDay ? policy.RestWorkTimeEnd : policy.WorkTimeEnd);
                refundHours = new KeyValuePair<Time, Time>(
                    isRestDay ? policy.RestRefundTimeStart : policy.WorkRefundTimeStart,
                    isRestDay ? policy.RestRefundTimeEnd : policy.WorkRefundTimeEnd);
            }
            this.lblWorkingTime.Text = refundHours.Key.ToString("HH:mm") + " - " + refundHours.Value.ToString("HH:mm");
            this.lblScrapTime.Text = scrapHours.Key.ToString("HH:mm") + " - " + scrapHours.Value.ToString("HH:mm");

        }

        protected bool IsImport { get; set; }

        private void bindPNRGroups(Service.Order.Domain.Order order)
        {
            foreach (var item in order.PNRInfos)
            {
                if (item.Passengers.Any() && item.Flights.Any())
                {
                    var pnrInfo = LoadControl(ResolveUrl("~/OrderModule/UserControls/PNRInfo.ascx")) as OrderModule.UserControls.PNRInfo;
                    pnrInfo.InitData(order, item, UserControls.Mode.Apply);
                    this.divPNRGroups.Controls.Add(pnrInfo);
                }
                if (order.PNRInfos.First().Flights.Any())
                {
                    var firstFlight = order.PNRInfos.First().Flights.First();
                    JsParameter.Value = string.Format("JsParameter={{Carrier:'{0}',flightNO:'{0}{1}',flightDate:'{2}'}}", firstFlight.Carrier.Code, firstFlight.FlightNo, firstFlight.TakeoffTime.ToString("yyyy-MM-dd"));
                }
            }
        }
        private void setButtons(Service.Order.Domain.Order order)
        {
            // 解锁并返回
            this.btnReleaseLockAndBack.Visible = order.Status == DataTransferObject.Order.OrderStatus.Finished;
        }
        private void setBackButton()
        {
            // 返回
            var returnUrl = getReturnUrl();
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private string getReturnUrl()
        {
            var returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "ApplyList.aspx";
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            return returnUrl;
        }
        private void showErrorMessage(string message)
        {
            this.divError.Visible = true;
            this.divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }

        protected void btnReleaseLockAndBack_Click(object sender, EventArgs e)
        {
            Service.LockService.UnLock(Request.QueryString["id"], CurrentUser.UserName);
            Response.Redirect(getReturnUrl(), true);
        }

        protected void SearchAirLine(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];
            decimal orderId;
            if (decimal.TryParse(id, out orderId))
            {
                var order = Service.OrderQueryService.QueryOrder(orderId);
                var passengers = hidApplyPassengers.Value.Split(',');
                var voyages = hidApplyFlights.Value.Split(',');
                var pnr = hidApplyPNR.Value.Replace("(小)", string.Empty).Replace("(大)", string.Empty).Split('|');
                if (passengers.Any() && voyages.Any())
                {
                    var currentPNRPair = order.PNRInfos.FirstOrDefault(p => p.Code.PNR == pnr[0] && p.Code.BPNR == pnr[1]);
                    if (currentPNRPair == null)
                    {
                        ShowMessage("编码信息错误！");
                        return;
                    }
                    var selectedPasserengerIds = passengers.Select(Guid.Parse);
                    var selectedVoyageIds = voyages.Select(Guid.Parse);
                    var selectedPasserengers = from sp in selectedPasserengerIds
                                               from op in currentPNRPair.Passengers
                                               where sp == op.Id
                                               select new DataTransferObject.Order.PassengerView()
                                               {
                                                   Name = op.Name,
                                                   Credentials = op.Credentials,
                                                   CredentialsType = op.CredentialsType,
                                                   PassengerType = op.PassengerType,
                                                   Phone = op.Phone
                                               };
                    var isGoBackFlight = !string.IsNullOrEmpty(Request.Form["date1"]);
                    var applyForm = new UpgradeApplyformView()
                    {
                        Passengers = selectedPasserengerIds,
                        PNR = new PNRPair(pnr[0], pnr[1]),
                        PNRSource = OrderSource.PlatformOrder,

                    };
                    var selectedVoyages = selectedVoyageIds.Join(currentPNRPair.Flights, p => p, p => p.Id, (p, q) => q).ToList();
                    selectedVoyages.ForEach(p => applyForm.AddItem(new UpgradeApplyformView.Item()
                    {
                        Voyage = p.Id,
                        Flight = new FlightView()
                        {
                            SerialNo = p.Serial,
                            Departure = p.Departure.Code,
                            Arrival = p.Arrival.Code,
                            TakeoffTime = p.TakeoffTime,
                            LandingTime = p.LandingTime,
                            Airline = p.Carrier.Code,
                            YBPrice = p.YBPrice,
                            FlightNo = p.FlightNo,
                            AirCraft = p.AirCraft,
                            Bunk = p.Bunk.Code,
                            Type = p.Bunk.Type
                        }
                    }));
                    var flight = selectedVoyages.OrderBy(p => p.TakeoffTime).First();
                    Session["UpgradeApplyformView"] = applyForm;
                    Session["UpgradePassengers"] = selectedPasserengers.ToList();
                    //Session["ReservedFlights"] = selectedVoyages;
                    //isGoBackFlight = 
                    var dealUrl = string.Format("{7}?airline={0}&departure={1}&arrival={2}&goDate={3}{4}&source=4&orderId={5}&provider={6}", flight.Carrier.Code, flight.Departure.Code, flight.Arrival.Code, Request.Form["date0"], isGoBackFlight ? string.Format("&backDate={0}", Request.Form["date1"]) : string.Empty, orderId, order.Provider.CompanyId, isGoBackFlight ? "/FlightReserveModule/FlightQueryGoResult.aspx" : "/FlightReserveModule/FlightQueryResult.aspx");
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "redirect", "window.parent.location.href='" + dealUrl + "'", true);
                }
                else
                {
                    ShowMessage("没有选择需要升舱的航段或乘客！");
                }
            }
        }
    }
}