using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.FlightQuery;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.Core.Extension;
using FlightView = ChinaPay.B3B.DataTransferObject.FlightQuery.FlightView;
using PassengerView = ChinaPay.B3B.DataTransferObject.Order.PassengerView;

namespace ChinaPay.B3B.TransactionWeb.FlightReserveModule {
    public partial class ChoosePolicyWithImport : BasePage {
        /// <summary>
        /// 航班查询预订
        /// </summary>
        internal const string ReservateSource = "1";
        /// <summary>
        /// 编码导入
        /// </summary>
        internal const string ImportSource = "2";
        /// <summary>
        /// 编码方式升舱
        /// </summary>
        internal const string UpgradeByPNRCodeSource = "3";
        /// <summary>
        /// 查询航班方式升舱
        /// </summary>
        internal const string UpgradeByQueryFlightSource = "4";
        /// <summary>
        /// 换出票方
        /// </summary>
        internal const string ChangeProviderSource = "5";

        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("ticket.css");
            if(!IsPostBack) {
                lblPlatformName.Text = PlatformName;
                lblFax.Text = CurrenContract.Fax;
                lblServicePhone.Text = CurrenContract.ServicePhone;
                bindData(Request.QueryString["source"]);
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
                Response.Cache.SetNoStore();
            }
        }
        private void bindData(string source) {
            if(ChangeProviderSource == source) {
                decimal orderId;
                if(decimal.TryParse(Request.QueryString["orderId"], out orderId)) {
                    var order = Service.OrderQueryService.QueryOrder(orderId);
                    if(order == null) {
                        Response.Write("订单不存在");
                        Response.End();
                    } else {
                        string lockErrorMsg;
                        if(Lock(orderId, LockRole.Platform, "更换出票方", out lockErrorMsg)) {
                            Session["OriginalOrder"] = order;
                        } else {
                            Response.Write("锁定订单失败。原因:" + lockErrorMsg);
                            Response.End();
                        }
                    }
                } else {
                    Response.Write("参数错误");
                    Response.End();
                }
            }
            var passengers = GetPassengers(source);
            var flights = processFlights(source);
            var policy = GetPolicyView();
            bindFlights(flights,policy);
            bindPassenger(passengers, flights, GetPATPrice(source));
            setParametersInfo(source, flights);
            LoadIsTeam();
            var items = SystemDictionaryService.Query(SystemDictionaryType.StandbyAndApplyFor);
            hidSQtip.Value = items.ElementAt(1).Value;
            hidHBtip.Value = items.ElementAt(0).Value;
        }

        private PolicyView GetPolicyView()
        {
            return Session["FlightPolicy"] as PolicyView;
        }


        private void LoadIsTeam() {
            var orderView = Session["OrderView"] as OrderView;
            hidIsTeam.Value = (orderView == null || orderView.IsTeam) ? "1" : string.Empty;
        }

        private IEnumerable<FlightView> processFlights(string source) {
            if(ChangeProviderSource == source) {
                var flights = (from pnr in GetOriginalOrder(source).PNRInfos
                               from flight in pnr.Flights
                               select new FlightView {
                                   Serial = flight.Serial,
                                   AirlineCode = flight.Carrier.Code,
                                   AirlineName = flight.Carrier.Name,
                                   FlightNo = flight.FlightNo,
                                   Aircraft = flight.AirCraft,
                                   Departure = new AirportView {
                                       Code = flight.Departure.Code,
                                       Name = flight.Departure.Name,
                                       City = flight.Departure.City,
                                       Time = flight.TakeoffTime
                                   },
                                   Arrival = new AirportView() {
                                       Code = flight.Arrival.Code,
                                       Name = flight.Arrival.Name,
                                       City = flight.Arrival.City,
                                       Time = flight.LandingTime
                                   },
                                   YBPrice = flight.YBPrice,
                                   AirportFee = flight.AirportFee,
                                   BAF = flight.BAF,
                                   Fare = flight.Price.Fare,
                                   BunkCode = flight.Bunk.Code,
                                   BunkType = flight.Bunk is Service.Order.Domain.Bunk.SpecialBunk ? BunkType.Economic : flight.Bunk.Type,
                                   EI = flight.Bunk.EI,
                                   Discount = flight.Bunk.Discount,
                               }).ToList();
                System.Web.HttpContext.Current.Session["ReservedFlights"] = flights;
                return flights;
            } else {
                return System.Web.HttpContext.Current.Session["ReservedFlights"] as IEnumerable<FlightView>;
            }
        }
        internal static IEnumerable<FlightView> GetFlights(string source) {
            return System.Web.HttpContext.Current.Session["ReservedFlights"] as IEnumerable<FlightView>;
        }
        internal static IEnumerable<PassengerView> GetPassengers(string source) {
            if(ReservateSource == source || ImportSource == source) {
                return GetOrderView(source).Passengers;
            } else if(ChangeProviderSource == source) {
                return from pnr in GetOriginalOrder(source).PNRInfos
                       from passenger in pnr.Passengers
                       select new PassengerView {
                           Name = passenger.Name,
                           Credentials = passenger.Credentials,
                           CredentialsType = passenger.CredentialsType,
                           PassengerType = passenger.PassengerType,
                           Phone = passenger.Phone
                       };
            } else {
                return System.Web.HttpContext.Current.Session["Passengers"] as IEnumerable<PassengerView>;
            }
        }
        private void bindFlights(IEnumerable<FlightView> flights,PolicyView policy) {
            this.ucFlights.InitData(false, flights,policy);
            hidFlightCount.Value = flights.Count().ToString();
            var firstFlgiht = flights.First();
            if(firstFlgiht.BunkType != null && firstFlgiht.YBPrice == 0 && firstFlgiht.BunkType.Value == BunkType.Free) hidisFreeTicket.Value = "1";
            hidFlishtInfos.Value = string.Format("{0},{1},{2:yyyy-MM-dd}", firstFlgiht.Departure.Code, firstFlgiht.Arrival.Code, firstFlgiht.Departure.Time);
        }
        private void bindPassenger(IEnumerable<PassengerView> passengers, IEnumerable<FlightView> flights, DataTransferObject.Command.PNR.PriceView patPrice) {
            var passengersHTML = new StringBuilder();
            passengersHTML.Append("<table><tr><th>姓名</th><th>类型</th><th>联系方式</th><th>证件类型</th><th>证件号</th><th>票面价</th><th>民航基金/燃油税</th><th>总价</th></tr>");
            var totalFare = getTotalFare(flights, patPrice);
            var totalAirportFee = patPrice == null ? flights.Sum(f => f.AirportFee) : patPrice.AirportTax;
            var totalBAF = flights.Sum(f => f.BAF);
            foreach(var item in passengers) {
                passengersHTML.Append("<tr>");
                passengersHTML.AppendFormat("<td>{0}</td>", item.Name);
                passengersHTML.AppendFormat("<td>{0}</td>", item.PassengerType.GetDescription());
                passengersHTML.AppendFormat("<td>{0}</td>", item.Phone);
                passengersHTML.AppendFormat("<td>{0}</td>", item.CredentialsType.ToString());
                passengersHTML.AppendFormat("<td>{0}</td>", item.Credentials);
                passengersHTML.AppendFormat("<td class='Price'>{0}</td>", totalFare.TrimInvaidZero());
                passengersHTML.AppendFormat("<td>{0}/{1}</td>", totalAirportFee.TrimInvaidZero(), totalBAF.TrimInvaidZero());
                passengersHTML.AppendFormat("<td>{0}</td>", (totalFare + totalAirportFee + totalBAF).TrimInvaidZero());
                passengersHTML.Append("</tr>");
            }
            passengersHTML.Append("</table>");
            this.divPassengers.InnerHtml = passengersHTML.ToString();
            this.hidPassengerCount.Value = passengers.Count().ToString();
            this.hidPassengerType.Value = ((int)passengers.First().PassengerType).ToString();
        }

        private static decimal getTotalFare(IEnumerable<FlightView> flights, DataTransferObject.Command.PNR.PriceView patPrice) {
            if(patPrice != null) return patPrice.Fare;
            if(flights.First().BunkType == BunkType.Production) return flights.First().Fare;
            return flights.Sum(f => f.Fare);
        }

        private void setParametersInfo(string source, IEnumerable<FlightView> flights) {
            // 来源
            this.hidSource.Value = source;
            // 政策类型
            Common.Enums.PolicyType policyType;
            // 通过航班查询时，使用前面选择的政策类型
            if(ReservateSource == source || UpgradeByQueryFlightSource == source) {
                policyType = GetPolicyView(source).Type;
            } else if(ImportSource == source) {
                // 编码导入时
                // 如果团队编码，则为团队政策
                if(GetOrderView(source).IsTeam)
                    policyType = PolicyType.Team;
                else if (GetOrderView(source).TripType ==ItineraryType.Notch)
                {
                    policyType = PolicyType.Notch;
                }else 
                {
                    // 根据舱位的类型来决定政策类型
                    if(flights.Any(f => f.BunkType == BunkType.Promotion || f.BunkType == BunkType.Production || f.BunkType == BunkType.Transfer)) {
                        policyType = PolicyType.Bargain;
                    } else {
                        switch(flights.First().BunkType) {
                            case BunkType.Economic:
                            case BunkType.FirstOrBusiness:
                                policyType = PolicyType.Normal ;
                                break;
                            case BunkType.Promotion:
                            case BunkType.Production:
                            case BunkType.Transfer:
                                policyType = PolicyType.Bargain;
                                break;
                            default:
                                policyType = PolicyType.Special;
                                break;
                        }
                    }
                }
            } else {
                // 通过编码方式升舱 或 换出票方时，只能使用普通政策
                policyType = PolicyType.Normal;
            }
            this.hidDefaultPolicyType.Value = ((int)policyType).ToString();
            // 如果是升舱 或 换出票方，需要原订单号和原出票方
            if(UpgradeByPNRCodeSource == source || UpgradeByQueryFlightSource == source) {
                this.hidOriginalOrderId.Value = Request.QueryString["orderId"];
                this.hidOriginalPolicyOwner.Value = Request.QueryString["provider"];
            } else if(ChangeProviderSource == source) {
                this.hidOriginalOrderId.Value = Request.QueryString["orderId"];
                this.hidOriginalPolicyOwner.Value = GetOriginalOrder(source).Provider.CompanyId.ToString();
            }
            // 如果是编码导入 或 通过编码方式升舱 或换出票方 的时候，需要编码信息
            if(UpgradeByPNRCodeSource == source) {
                this.hidPNRCode.Value = GetApplyformView(source).PNR.PNR;
            } else if(ImportSource == source) {
                this.hidPNRCode.Value = GetOrderView(source).PNR.PNR;
            } else if(ChangeProviderSource == source) {
                this.hidPNRCode.Value = GetOriginalOrder(source).ReservationPNR.PNR;
            }
        }
        internal static DataTransferObject.Command.PNR.PriceView GetPATPrice(string source) {
            if(UpgradeByQueryFlightSource == source || UpgradeByPNRCodeSource == source) {
                return GetApplyformView(source).PATPrice;
            } else if(ChangeProviderSource == source) {
                return null;
            } else {
                return GetOrderView(source).PATPrice;
            }
        }
        internal static Service.Order.Domain.Order GetOriginalOrder(string source) {
            if(ChangeProviderSource == source) {
                return System.Web.HttpContext.Current.Session["OriginalOrder"] as Service.Order.Domain.Order;
            }
            return null;
        }
        internal static OrderView GetOrderView(string source) {
            if(UpgradeByQueryFlightSource == source || UpgradeByPNRCodeSource == source) {
                return null;
            } else {
                return System.Web.HttpContext.Current.Session["OrderView"] as OrderView;
            }
        }
        internal static UpgradeApplyformView GetApplyformView(string source) {
            if(UpgradeByQueryFlightSource == source || UpgradeByPNRCodeSource == source) {
                return System.Web.HttpContext.Current.Session["ApplyformView"] as UpgradeApplyformView;
            } else {
                return null;
            }
        }
        internal static PolicyView GetPolicyView(string source) {
            if(ReservateSource == source || UpgradeByQueryFlightSource == source) {
                return System.Web.HttpContext.Current.Session["FlightPolicy"] as PolicyView;
            } else {
                return null;
            }
        }
        internal static VoyageType GetVoyageType(string source) {
            if(ReservateSource == source) {
                // 查询预订
                return GetVoyageType(GetOrderView(source).TripType);
            } else if(UpgradeByQueryFlightSource == source || UpgradeByPNRCodeSource == source) {
                // 升舱
                var aplyformView = GetApplyformView(source);
                return GetVoyageType(aplyformView.Voyages.Count() == 2 ? ItineraryType.Roundtrip : ItineraryType.OneWay);
            } else if(ChangeProviderSource == source) {
                // 换出票方
                return GetVoyageType(GetOriginalOrder(source).TripType);
            } else if(ImportSource == source) {
                // 编码导入
                return GetVoyageType(GetOrderView(source).TripType);
            }
            throw new NotImplementedException();
        }
        private static VoyageType GetVoyageType(ItineraryType type) {
            switch(type) {
                case ItineraryType.OneWay:
                    return VoyageType.OneWay;
                case ItineraryType.Roundtrip:
                    return VoyageType.RoundTrip;
                case ItineraryType.Conjunction:
                    return VoyageType.TransitWay;
                case ItineraryType.Notch:
                    return VoyageType.Notch;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}