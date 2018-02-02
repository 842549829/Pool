using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.AddressLocator;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Command.Domain;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.B3B.Service.PriceCheck;
using ChinaPay.B3B.Service.Remind;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using FlightView = ChinaPay.B3B.DataTransferObject.FlightQuery.FlightView;
using PassengerView = ChinaPay.B3B.DataTransferObject.Order.PassengerView;
using PriceView = ChinaPay.B3B.DataTransferObject.Command.PNR.PriceView;

namespace ChinaPay.B3B.TransactionWeb.FlightReserveModule
{
    internal class PNRHelper
    {
        public static PNRPair ReserveSeat(IEnumerable<FlightView> flights, IEnumerable<PassengerView> passengers)
        {
            PassengerType passengerType = passengers.First().PassengerType;
            var reservationInfo = new ReservationInfo
                {
                    AgentPhoneNumber = SystemParamService.ContactInPNR,
                    Segements = flights.Select(f => new ReservationSegmentInfo
                        {
                            Carrier = f.AirlineCode,
                            InternalNumber = f.FlightNo,
                            ClassOfService = f.BunkCode,
                            Date = f.Departure.Time,
                            DepartureAirportCode = f.Departure.Code,
                            ArrivalAirportCode = f.Arrival.Code,
                        }).ToList(),
                    Passengers = passengers.Select(p => new ReservationPassengerInfo
                        {
                            Name = p.Name,
                            Type = p.PassengerType,
                            CertificateNumber = p.Credentials,
                            CertificateType = p.CredentialsType,
                            MobilephoneNumber = p.Phone,
                            Birthday = p.BirthDay
                        }).ToList(),
                };
            ExecuteResult<ReservedPnr> execResult = CommandService.ReserveTickets(reservationInfo, BasePage.OwnerOEMId);
            if (execResult.Success)
            {
                PNRPair pnrCode = execResult.Result.PnrPair;
                var opLog = new OperationLog(OperationModule.其他, OperationType.Else, BasePage.LogonUser.UserName,
                    OperatorRole.Purchaser, "订座记录", string.Format("PNR:{0}\n组织结构名称：{1}\n管理员帐号：{2}\n操作人：{3}\nIP:{4}\n乘机人：{5}\n航班信息：{6}",
                        pnrCode.ToListString("|"), BasePage.LogonCompany.CompanyName, BasePage.LogonCompany.UserName, BasePage.LogonUser.UserName,
                        IPAddressLocator.GetRequestIP(HttpContext.Current.Request),
                        passengers.Select(p => p.Name + "_" + p.CredentialsType.ToString() + "_" + p.Credentials + "_" + p.Phone).Join("|"),
                        flights.Select(p => p.AirlineCode + p.FlightNo + "_" + p.BunkCode + "_" + p.Departure.Time).Join("|")));
                LogService.SaveOperationLog(opLog);
                CommandService.ValidatePNR(execResult.Result, passengerType);

                return execResult.Result.PnrPair;
            }
            else
            {
                throw new CustomException("订座失败");
            }
        }

        internal static bool RequirePat(IEnumerable<FlightView> flights, PolicyType policyType)
        {
            // 普通政策，单程根据时间段来决定是否需要P价格，其他的都要
            // 如果是默认政策，舱位是普通舱位时，判断条件跟普通政策一样。
            FlightView firstFlight = flights.First();
            if (firstFlight.BunkType.HasValue)
            {
                BunkType bunkType = firstFlight.BunkType.Value;
                if (policyType == PolicyType.Normal || policyType == PolicyType.NormalDefault
                    || (policyType == PolicyType.OwnerDefault && (bunkType == BunkType.Economic || bunkType == BunkType.FirstOrBusiness)))
                {
                    switch (flights.Count())
                    {
                        case 2:
                            return true;
                        case 1:
                            DateTime today = DateTime.Today;
                            return SystemParamService.PATTimeZones.Any(item => item.Lower.Date <= today && today <= item.Upper.Date);
                    }
                }
            }
            return false;
        }

        internal static bool RequireFD(IEnumerable<FlightView> flights)
        {
            // 普通政策，单程根据时间段来决定是否需要核价格
            // 如果是默认政策，舱位是普通舱位时，判断条件跟普通政策一样。
            FlightView firstFlight = flights.First();
            if (firstFlight.BunkType.HasValue)
            {
                BunkType bunkType = firstFlight.BunkType.Value;
                if ((bunkType == BunkType.Economic || bunkType == BunkType.FirstOrBusiness))
                {
                    switch (flights.Count())
                    {
                        case 2:
                            return false;
                        case 1:
                            DateTime now = DateTime.Now;
                            return SystemParamService.FDTimeZones.Any(item => item.Lower.Date <= now && now <= item.Upper.Date);
                        default :
                            return false;
                    }
                }
            }
            return false;
        }

        internal static PriceView Pat(PNRPair pnr, IEnumerable<FlightView> flights, PassengerType passengerType)
        {
            ExecuteResult<IEnumerable<PriceView>> execResult = CommandService.QueryPriceByPNR(pnr, passengerType, BasePage.OwnerOEMId);
            if (execResult.Success)
            {
                var currentFare = flights.Sum(f=>f.Fare);
                decimal maxFare = execResult.Result.Any(item => item.Fare == currentFare) 
                                      ? currentFare : execResult.Result.Max(item => item.Fare);
                PriceView minPriceView = execResult.Result.FirstOrDefault(item => item.Fare == maxFare);
                // 检查是否与基础数据中的价格相同，不同则记录日志
                if (flights.Count() == 1)
                {
                    FlightView flight = flights.First();
                    if (flight.BunkType.HasValue && (flight.BunkType.Value == BunkType.Economic || flight.BunkType.Value == BunkType.FirstOrBusiness) && flight.Fare != maxFare &&
                        passengerType == PassengerType.Adult)
                    {
                        var fare = new FareErrorLog
                            {
                                Carrier = flight.AirlineCode,
                                Departure = flight.Departure.Code,
                                Arrival = flight.Arrival.Code,
                                FlightDate = flight.Departure.Time.Date,
                                Bunk = flight.BunkCode,
                                Fare = maxFare,
                                IsTreatment = true
                            };
                        if (RequireFD(flights))
                        {
                            PriceCheckService.CheckFd(flight.AirlineCode, flight.Departure.Code, flight.Arrival.Code,
                                fare.FlightDate);
                        }
                        else
                        {
                            fare.IsTreatment = false;
                            B3BEmailSender.SendFareError(fare, flight.Fare);
                        }
                        LogService.SaveFareErrorLog(fare);
                    }
                }
                return minPriceView;
            }
            return null;
        }
    }
}