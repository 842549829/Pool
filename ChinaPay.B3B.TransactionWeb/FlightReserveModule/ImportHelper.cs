using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Command.Domain;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.FlightQuery.Domain;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.B3B.Service.PriceCheck;
using ChinaPay.B3B.Service.Remind;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.Core;
using FlightView = ChinaPay.B3B.DataTransferObject.FlightQuery.FlightView;
using PassengerView = ChinaPay.B3B.DataTransferObject.Order.PassengerView;
using PriceView = ChinaPay.B3B.DataTransferObject.Command.PNR.PriceView;

namespace ChinaPay.B3B.TransactionWeb.FlightReserveModule
{
    public class ImportHelper
    {
        public static PnrImportResult ImportByPNRCode(string adultPNRCode, string childPNRCode, PassengerType passengerType, string patContent, HttpContext context)
        {
            PnrImportResult result;
            ExecuteResult<ReservedPnr> adultPNRContent = getPNRContent(adultPNRCode);
            if (adultPNRContent.Success && !adultPNRContent.Result.HasCanceled)
            {
                adultPNRContent.Result.PatRawData = patContent;
                List<PriceView> patPrices = Service.Command.Domain.Utility.Parser.GetPatPrices(patContent);
                decimal minFare = (patPrices == null || patPrices.Count == 0) ? 0 : patPrices.Min(p => p.Fare);
                PriceView patPrice = (patPrices == null || patPrices.Count == 0) ? null : patPrices.First(item => item.Fare == minFare);
                PriceView maxpatPrice = GetMaxPatPrice(patPrices);

                if (passengerType == PassengerType.Child)
                {
                    ExecuteResult<ReservedPnr> childrenPNRContent = getPNRContent(childPNRCode);
                    if (childrenPNRContent.Success)
                    {
                        result = importChildrenOrder(context, adultPNRContent.Result, childrenPNRContent.Result, OrderSource.CodeImport, patPrice,maxpatPrice);
                        result.PNRContent = adultPNRContent.Message;
                        return result;
                    }
                    else
                    {
                        throw new CustomException("提取儿童编码信息失败：" + childrenPNRContent.Message);
                    }
                }
                else
                {
                    result = importAdultOrder(context, adultPNRContent.Result, OrderSource.CodeImport, patPrice,maxpatPrice);
                    result.PNRContent = adultPNRContent.Message;
                    return result;
                }
            }
            else
            {
                throw new CustomException("提取成人编码信息失败：" + adultPNRContent.Message);
            }
        }

        private static PriceView GetMaxPatPrice(List<PriceView> patPrices) { 
            if(!patPrices.Any()) return null;
            var maxFare = patPrices.Max(P => P.Fare);
            return patPrices.First(p => p.Fare == maxFare);
        }

        public static PnrImportResult ImportByPNRContent(string pnrContent, string adultPNRCode, PassengerType passengerType, HttpContext context)
        {
            //// 做一些替换，注意，所有的序号开始前的数据全部被替换了；
            //pnrContent = Regex.Replace(pnrContent, "(\\n)+", "");
            //pnrContent = Regex.Replace(pnrContent, "(\\r)+", ";");

            //if (Parser.GetPassengerConsistsType(pnrContent) == PassengerConsistsType.Group)
            //{
            //    pnrContent = Regex.Replace(pnrContent, @"(?:(?:^.*;|^))(?=\s?0\.)", "");
            //    // 2012-10-29 同CodeLineRegex一起修改的；
            //    // pnrContent = Regex.Replace(pnrContent, @"^.*;(?=\s0\.)", "");
            //}
            //else
            //{
            //    // 2012-10-29 同CodeLineRegex一起修改的；
            //    pnrContent = Regex.Replace(pnrContent, @"(?:(?:^.*;|^))(?=\s?1\.)", "");
            //}

            var pnrContentModel = CommandService.GetReservedPnr(pnrContent);
            if (pnrContentModel == null || !pnrContentModel.Success)
            {
                throw new CustomException("编码内容解析失败,请确认编码内容是否正确");
            }
            else if (pnrContentModel.Result.HasCanceled)
            {
                throw new CustomException("编码已被取消，请确认！");
            }
            else
            {
                if (PNRPair.IsNullOrEmpty(pnrContentModel.Result.PnrPair)) throw new CustomException("内容中缺少编码");
                //if (string.IsNullOrWhiteSpace(pnrContentModel.Code.PNR)) throw new CustomException("缺少小编码");
                if (string.IsNullOrWhiteSpace(pnrContentModel.Result.PnrPair.PNR) && string.IsNullOrWhiteSpace(pnrContentModel.Result.PnrPair.BPNR)) throw new CustomException("编码信息不全");

                List<PriceView> patPrices = Service.Command.Domain.Utility.Parser.GetPatPrices(pnrContent);
                //if (patPrices == null || patPrices.Count == 0) throw new CustomException("缺少PAT内容");

                decimal minFare = (patPrices == null || patPrices.Count == 0) ? 0 : patPrices.Min(p => p.Fare);
                PriceView patPrice = (patPrices == null || patPrices.Count == 0) ? null : patPrices.First(item => item.Fare == minFare);
                PriceView maxPatPrice = GetMaxPatPrice(patPrices);

                if (passengerType == PassengerType.Child)
                {
                    ExecuteResult<ReservedPnr> adultPNRContent = getPNRContent(adultPNRCode);
                    if (adultPNRContent.Success)
                    {
                        return importChildrenOrder(context, adultPNRContent.Result, pnrContentModel.Result, OrderSource.ContentImport, patPrice, maxPatPrice);
                    }
                    else
                    {
                        throw new CustomException("提取成人编码信息失败：" + adultPNRContent.Message);
                    }
                }
                else
                {
                    return importAdultOrder(context, pnrContentModel.Result, OrderSource.ContentImport, patPrice, maxPatPrice);
                }
            }
        }

        /// <summary>
        /// 从PNR中解析航班信息
        /// </summary>
        /// <param name="adultPNRCode">PNR编码</param>
        /// <param name="context">页面请求上下文</param>
        public static Tuple<IEnumerable<FlightView>, IEnumerable<Passenger>> AnalysisPNR(string adultPNRCode, HttpContext context)
        {
            ExecuteResult<ReservedPnr> pnrContent = getPNRContent(adultPNRCode);
            if (pnrContent.Success)
            {
                CommandService.ValidatePNR(pnrContent.Result, PassengerType.Adult);
                IEnumerable<FlightView> flightViews = ReserveViewConstuctor.GetQueryFlightView(pnrContent.Result.Voyage.Segments, pnrContent.Result.Voyage.Type,
                    pnrContent.Result.Passengers.First().Type, pnrContent.Result.IsTeam,null);
                var result = new Tuple<IEnumerable<FlightView>,
                    IEnumerable<Passenger>>(flightViews, pnrContent.Result.Passengers);
                return result;
            }
            throw new CustomException("提取成人编码信息失败：" + pnrContent.Message);
        }

        private static ExecuteResult<ReservedPnr> getPNRContent(string pnrCode)
        {
            var pnrPair = new PNRPair(pnrCode, string.Empty);
            return CommandService.GetReservedPnr(pnrPair, BasePage.OwnerOEMId);
        }

        private static PnrImportResult importAdultOrder(HttpContext context, ReservedPnr adultPNRContent, OrderSource orderSource, PriceView patPrice, PriceView maxpatPrice)
        {
            CheckFligtTime(adultPNRContent);
            IEnumerable<FlightView> reservedFlights = ReserveViewConstuctor.GetQueryFlightView(adultPNRContent.Voyage.Segments, adultPNRContent.Voyage.Type, PassengerType.Adult,
                adultPNRContent.IsTeam,patPrice);
            bool isFreeTicket = adultPNRContent.Voyage.Segments.Any() && reservedFlights.First().BunkType.Value == BunkType.Free;
            //如果遇到证件号不全体提编码
            if (adultPNRContent.Passengers.Any(p => string.IsNullOrEmpty(p.CertificateNumber)) && !PNRPair.IsNullOrEmpty(adultPNRContent.PnrPair))
            {
                var pnr = adultPNRContent.PnrPair.PNR;
                ExecuteResult<ReservedPnr> rtResult = CommandService.GetReservedPnr(adultPNRContent.PnrPair, BasePage.OwnerOEMId);
                if (rtResult.Success && !rtResult.Result.HasCanceled)
                {
                    string patContent = adultPNRContent.PatRawData;
                    adultPNRContent = rtResult.Result;
                    adultPNRContent.PnrPair.PNR = pnr;
                    adultPNRContent.PatRawData = patContent;
                }
            }
            CommandService.ValidatePNR(adultPNRContent, PassengerType.Adult);
            if (!isFreeTicket && patPrice == null) return new PnrImportResult(false, "需要输入PAT内容", true);
            if (isFreeTicket)
            {
                Segment segment = adultPNRContent.Voyage.Segments.First();
                Flight flight = FlightQueryService.QueryFlight(segment.AirportPair.Departure, segment.AirportPair.Arrival,
                    segment.Date.AddHours(segment.DepartureTime.Hour).AddMinutes(segment.DepartureTime.Minute), segment.AirlineCode, segment.InternalNo, BasePage.OwnerOEMId);
                if (flight != null)
                {
                    patPrice = new PriceView
                        {
                            Total = flight.BAF.Adult + flight.AirportFee,
                            Fare = 0m,
                            AirportTax = flight.AirportFee,
                            BunkerAdjustmentFactor = flight.BAF.Adult
                        };
                }
            }
            if (patPrice == null) throw new CustomException("缺少PAT内容");
            //if (adultPNRContent.Price != null) throw new CustomException("成人编码中不能包含票价组项");
            saveImportInfo(context, adultPNRContent, null, orderSource, patPrice, maxpatPrice, PassengerType.Adult);
            return new PnrImportResult(true);
        }

        private static void CheckFligtTime(ReservedPnr reservedPnr) {
            if (reservedPnr.Voyage.Segments.Any())
            {
                var firstFlightDate = reservedPnr.Voyage.Segments.Min(f => f.Date.AddHours(f.DepartureTime.Hour).AddMinutes(f.DepartureTime.Minute));
                var minutesBeforeTakeOff = (firstFlightDate - DateTime.Now).TotalMinutes;
                if (minutesBeforeTakeOff <= SystemParamService.FlightDisableTime) throw new CustomException("由于时间受限,飞机起飞前" + SystemParamService.FlightDisableTime+"分钟,不允许再购买此航班");
            }
        }

        /// <summary>
        /// 检查舱位是否是免票舱位
        /// </summary>
        /// <param name="bunkCode"></param>
        /// <param name="airLineCode">航空公司代码 </param>
        /// <returns></returns>
        internal static bool CheckIfFreeTicket(string bunkCode, string airLineCode) { return FoundationService.Bunks.Any(p => p.Type == BunkType.Free && p.Code.Value == bunkCode.ToUpper() && p.AirlineCode.Value == airLineCode.ToUpper()); }

        private static PnrImportResult importChildrenOrder(HttpContext context, ReservedPnr adultPNRContent, ReservedPnr childrenPNRContent, OrderSource orderSource, PriceView patPrice, PriceView maxpatPrice)
        {
            CheckFligtTime(adultPNRContent);
            if (patPrice == null) return new PnrImportResult(false, "需要输入PAT内容", true);
            CommandService.ValidatePNR(childrenPNRContent, PassengerType.Child);
            //if (childrenPNRContent.Price != null) throw new CustomException("儿童编码中不能包含票价组项");
            //CheckPNRWithETDZ(adultPNRContent, Common.Enums.PassengerType.Adult);
            //checkVoyages(adultPNRContent.Segments.Values, childrenPNRContent.Segments.Values);
            //if(adultPNRContent.Passengers.Count < childrenPNRContent.Passengers.Count) {
            //    throw new CustomException("儿童数不能超过成人数");
            //}
            patPrice.AirportTax = 0m; //儿童票机建是0
            patPrice.Total = patPrice.AirportTax + patPrice.BunkerAdjustmentFactor + patPrice.Fare;
            saveImportInfo(context, childrenPNRContent, adultPNRContent.PnrPair, orderSource, patPrice, maxpatPrice, PassengerType.Child);
            return new PnrImportResult(true);
        }

        private static void saveImportInfo(HttpContext context, ReservedPnr pnrContent, PNRPair associatePNR, OrderSource orderSource, PriceView patPrice, PriceView maxpatPrice, PassengerType passengerType)
        {
            bool fdSuccess = true;
            IEnumerable<FlightView> reservedFlights = ReserveViewConstuctor.GetQueryFlightView(pnrContent.Voyage.Segments, pnrContent.Voyage.Type,
                pnrContent.Passengers.First().Type, pnrContent.IsTeam,patPrice);
            if (maxpatPrice != null && maxpatPrice.Fare != 0 && pnrContent.Voyage.Type == ItineraryType.OneWay
                && passengerType == PassengerType.Adult)
            {
                var flight = reservedFlights.First();
                if (flight.Fare != maxpatPrice.Fare && flight.Fare != 0 && (flight.BunkType == BunkType.Economic || flight.BunkType == BunkType.FirstOrBusiness))
                {
                    FareErrorLog fare = new FareErrorLog
                    {
                        Carrier = flight.AirlineCode,
                        Departure = flight.Departure.Code,
                        Arrival = flight.Arrival.Code,
                        FlightDate = flight.Departure.Time.Date,
                        Bunk = flight.BunkCode,
                        Fare = patPrice.Fare,
                    };
                    if (PNRHelper.RequireFD(reservedFlights))
                    {
                        try
                        {
                            flight.Fare = patPrice.Fare = PriceCheckService.CheckFd(flight.AirlineCode, flight.Departure.Code, flight.Arrival.Code,
                             flight.BunkCode, flight.Departure.Time);
                            fare.IsTreatment = true;
                        }
                        catch (Exception ex)
                        {
                            LogService.SaveExceptionLog(ex, string.Format("{0}{1}{2}{3}({4})", flight.AirlineCode, flight.Departure.Code, flight.Arrival.Code, flight.BunkCode, flight.Departure.Time));
                            fare.IsTreatment = false;
                            fdSuccess = false;
                        }
                    }
                    else
                    {
                        fdSuccess = false;
                        B3BEmailSender.SendFareError(fare, flight.Fare);
                    }
                    LogService.SaveFareErrorLog(fare);
                }
                
            }


            OrderView orderView = GetOrderView(orderSource, pnrContent, associatePNR, reservedFlights, passengerType);
            orderView.FdSuccess = fdSuccess;
            orderView.PatContent = pnrContent.PatRawData;
            orderView.PnrContent = pnrContent.PnrRawData;
            orderView.UseBPNR = !pnrContent.UsedCrsCode;
            orderView.PATPrice = patPrice;
            context.Session["ReservedFlights"] = reservedFlights;
            context.Session["OrderView"] = orderView;
        }

        private static void checkVoyages(IEnumerable<Segment> adult, IEnumerable<Segment> child)
        {
            if (adult.Count() != child.Count()) throw new CustomException("成人编码与儿童编码的航班不一致");
            foreach (Segment adultVoyage in adult)
            {
                Segment childVoyage = child.FirstOrDefault(item => item.AirportPair.Equals(adultVoyage.AirportPair));
                if (!Segment.IsSaveVoyage(adultVoyage, childVoyage)) throw new CustomException("成人编码与儿童编码的航班不一致");
            }
        }

        private static OrderView GetOrderView(OrderSource orderSource, ReservedPnr pnrContent, PNRPair associatePNR, IEnumerable<FlightView> flightViews,
            PassengerType passengerType)
        {
            return new OrderView
                {
                    FdSuccess = true,
                    Source = orderSource,
                    PNR = pnrContent.PnrPair,
                    Passengers = pnrContent.Passengers.OrderBy(p => p.Name).Select(p => new PassengerView
                        {
                            Name = p.Name,
                            Credentials = p.CertificateNumber,
                            CredentialsType = CredentialsType.身份证,
                            PassengerType = passengerType,
                            Phone = p.Mobilephone
                        }),
                    Flights = flightViews.Select(ReserveViewConstuctor.GetOrderFlightView).ToList(),
                    Contact = new Contact
                        {
                            Name = BasePage.LogonCompany.Contact,
                            Mobile = BasePage.LogonCompany.ContactPhone,
                            Email = BasePage.LogonCompany.ContactEmail
                        },
                    AssociatePNR = associatePNR,
                    IsTeam = pnrContent.IsTeam,
                    TripType = pnrContent.Voyage.Type
                };
        }
    }

    public class PnrImportResult
    {
        public PnrImportResult(bool success, string msg = "", bool needpat = false, string PNRContent = "")
        {
            Success = success;
            Message = msg;
            NeedPAT = needpat;
            this.PNRContent = PNRContent;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public bool NeedPAT { get; set; }
        public string PNRContent { get; set; }
    }
}