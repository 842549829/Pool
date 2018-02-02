using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.Command.PNR;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.TransactionWeb.OrderModule.UserControls;
using ChinaPay.Core.Extension;
using PNRInfo = ChinaPay.B3B.Service.Order.Domain.PNRInfo;
using Passenger = ChinaPay.B3B.Service.Command.Domain.PNR.Passenger;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class Supply : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                string id = Request.QueryString["id"];
                decimal orderId;
                if (decimal.TryParse(id, out orderId))
                {
                    Order order = OrderQueryService.QueryOrder(orderId);
                    if (order != null)
                    {
                        if (order.Status == OrderStatus.Applied || order.Status == OrderStatus.PaidForSupply)
                        {
                            string lockErrorMsg;
                            if (Lock(orderId, LockRole.Supplier, "处理座位", out lockErrorMsg))
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
                            showErrorMessage("仅待确认座位或待提供座位的订单可做座位处理");
                        }
                        return;
                    }
                }
                showErrorMessage("订单不存在");
            }
        }

        private void bindOrder(Order order)
        {
            bindOrderHeader(order);
            bindPNRInfos(order);
            bindPolicyRemark(order);
            bindHKCode(order);
            divPNRCode.Visible = true;
        }

        private void bindOrderHeader(Order order)
        {
            divHeader.Visible = true;
            lblOrderId.Text = order.Id.ToString();
            lblStatus.Text = StatusService.GetOrderStatus(order.Status, OrderRole.Supplier);
            lblProducedTime.Text = order.Purchaser.ProducedTime.ToString("yyyy-MM-dd HH:mm:ss");
            var product = order.IsThirdRelation ? order.Supplier.Product : order.Provider.Product as SpeicalProductInfo;
            lblSpecialPolicyType.Text = product.SpeicalProductType.GetDescription();
            if (product.SpeicalProductType == SpecialProductType.CostFree)
            {
                hidIsFree.Value = "1";
            }
            hidIsThirdRelation.Value=order.IsThirdRelation?"1":string.Empty;
        }

        private void bindPNRInfos(Order order)
        {
            pnrGroups.Visible = true;
            pnrGroups.InitData(order, order.PNRInfos.First(), Mode.Supply);
            var firstFlight = order.PNRInfos.First().Flights.First();
            JsParameter.Value = string.Format("JsParameter={{Carrier:'{0}',flightNO:'{0}{1}',flightDate:'{2}'}}", firstFlight.Carrier.Code, firstFlight.FlightNo, firstFlight.TakeoffTime.ToString("yyyy-MM-dd"));
        }

        private void bindPolicyRemark(Order order)
        {
            divPolicyRemark.Visible = true;
            divPolicyRemarkContent.InnerHtml = string.Format("{0} <span class='systemEndFix'>{1}</span>", order.Supplier == null ? order.Provider.Product.Remark : order.Supplier.Product.Remark, SystemParamService.PolicyRemark);
            //liOfficeNo.Text = order.Supplier.Product.ProductType.
            PolicyId = order.IsThirdRelation ? order.Supplier.Product.Id : order.Provider.Product.Id;
        }

        private void bindHKCode(Order order)
        {
            divHKCode.Visible = true;
            PNRInfo pnr = order.PNRInfos.First();
            int passengerCount = pnr.Passengers.Count();
            string hkCodes = CommandService.GetBookingTicketsString(
                                new ReservationInfo
                                {
                                    AgentPhoneNumber = order.Contact.Mobile,
                                    Passengers = (from p in pnr.Passengers
                                                  select new ReservationPassengerInfo
                                                  {
                                                      Name = p.Name,
                                                      Type = p.PassengerType,
                                                      CertificateNumber = p.Credentials,
                                                      CertificateType = p.CredentialsType,
                                                      MobilephoneNumber = p.Phone,
                                                      Birthday = p.Birthday
                                                  }
                                                 ).ToList(),
                                    Segements = (from p in pnr.Flights
                                                 select new ReservationSegmentInfo
                                                 {
                                                     Carrier = p.Carrier.Code,
                                                     InternalNumber = p.FlightNo,
                                                     ClassOfService = p.Bunk.Code,
                                                     Date = p.TakeoffTime,
                                                     DepartureAirportCode = p.Departure.Code,
                                                     ArrivalAirportCode = p.Arrival.Code,
                                                 }).ToList()

                                }, BasePage.OwnerOEMId
                //new ReservationInformation
                //    {
                //        Airline = pnr.Flights.First().Carrier.Code,
                //        PassengerType = pnr.Passengers.First().PassengerType,
                //        Passengers = pnr.Passengers.Select(ToCommandType).ToList(),
                //        Segments = pnr.Flights.Select(f => ToCommandType(f, passengerCount)).ToList(),
                //        PassengerPhone = order.Contact.Mobile,
                //    }
                    );

            var workingSetting = CompanyService.GetWorkingSetting(CurrentCompany.CompanyId);
            txtHKCode.Text = workingSetting != null ? hkCodes.Replace("OfficeNo", workingSetting.DefaultOfficeNumber) : hkCodes;
        }

        private void setButtons(Order order)
        {
            if (order.Status == OrderStatus.Applied && order.IsThirdRelation)
            {
                btnReviseReleasedFare.Visible = true;
            }
            btnDeny.Visible = true;
            btnSupply.Visible = true;
            // 处理类型  0：确认座位 1：提供座位
            hidProcessType.Value = order.Status == OrderStatus.Applied ? "0" : "1";
        }

        private void setBackButton()
        {
            // 返回
            string returnUrl = Request.QueryString["returnUrl"];
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = (Request.UrlReferrer ?? Request.Url).PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
            this.returnUrl.Value = returnUrl;   
        }

        private void showErrorMessage(string message)
        {
            divError.Visible = true;
            divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }

        protected void btnReleaseLockAndBack_Click(object sender, EventArgs e)
        {
            decimal orderId;
            if (decimal.TryParse(Request.QueryString["id"], out orderId))
            {
                ReleaseLock(orderId);
                string returnUrl = Request.QueryString["returnUrl"];
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    returnUrl = (Request.UrlReferrer ?? Request.Url).PathAndQuery;
                }
                if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
                RegisterScript(this, "location.href='" + returnUrl + "'", true);
            }
        }


        #region  类型转换

        private Passenger ToCommandType(Service.Order.Domain.Passenger passenger)
        {
            return new Passenger
                {
                    Name = passenger.Name,
                    CertificateType = passenger.CredentialsType,
                    CertificateNumber = passenger.Credentials,
                    Type = passenger.PassengerType,
                    Mobilephone = passenger.Phone,
                    TicketNumbers = passenger.Tickets.Select(ticket => ticket.No).ToList(),
                };
        }

        private Segment ToCommandType(Flight flight, int seatCount)
        {
            return new Segment(flight.Carrier.Code, flight.FlightNo,
                flight.Bunk.Code,
                flight.TakeoffTime,
                new AirportPair(flight.Departure.Code, flight.Arrival.Code), seatCount);
        }

        #endregion

        protected Guid PolicyId
        {
            get
            {
                if (ViewState["PolicyId"] != null)
                {
                    return (Guid)ViewState["PolicyId"];
                }
                return ChinaPay.B3B.TransactionWeb.OrderHandlers.Order.DefaultGuid;
            }
            set { ViewState["PolicyId"] = value; }
        }

    }
}