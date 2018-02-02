﻿using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Service;
using FlightView = ChinaPay.B3B.DataTransferObject.FlightQuery.FlightView;

namespace ChinaPay.B3B.TransactionWeb.FlightReserveModule {
    public class ReserveViewConstuctor {
        public static DataTransferObject.Order.FlightView GetOrderFlightView(DataTransferObject.FlightQuery.FlightView flightView) {
            return new DataTransferObject.Order.FlightView {
                SerialNo = flightView.Serial,
                Airline = flightView.AirlineCode,
                FlightNo = flightView.FlightNo,
                Departure = flightView.Departure.Code,
                Arrival = flightView.Arrival.Code,
                AirCraft = flightView.Aircraft,
                TakeoffTime = flightView.Departure.Time,
                LandingTime = flightView.Arrival.Time,
                YBPrice = flightView.YBPrice,
                Bunk = flightView.BunkCode,
                Fare = flightView.Fare,
                Type = flightView.BunkType == null ? BunkType.Economic : flightView.BunkType.Value,
                IsShare = flightView.IsShare,
                ArrivalTerminal = flightView.Arrival.Terminal,
                DepartureTerminal = flightView.Departure.Terminal
            };
        }
        public static IEnumerable<DataTransferObject.Order.FlightView> GetOrderFlightView(IEnumerable<Segment> segments,
            ItineraryType itineraryType, PassengerType passengerType, bool isTeam) {
            var fv = GetQueryFlightView(segments, itineraryType, passengerType, isTeam,null);
            return fv.Select(GetOrderFlightView);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="segments"></param>
        /// <param name="itineraryType"></param>
        /// <param name="passengerType"></param>
        /// <param name="isTeam"></param>
        /// <param name="patPrice">在双重类型的仓位上用户区分仓位类型，null的情况下只会选择普通类型</param>
        /// <returns></returns>
        public static IEnumerable<DataTransferObject.FlightQuery.FlightView> GetQueryFlightView(IEnumerable<Segment> segments,
            ItineraryType itineraryType, PassengerType passengerType, bool isTeam, PriceView patPrice)
        {
            var index = 1;
            var result = segments.Select(s => GetFlightView(s, passengerType)).OrderBy(item => item.Departure.Time).ToList();
            var voyageType = GetVoyageTypeValue(itineraryType, result.Count);
            var passengerTypeValue = passengerType == PassengerType.Child ? PassengerTypeValue.Child : PassengerTypeValue.Adult;
            var travelTypeValue = isTeam ? TravelTypeValue.Team : TravelTypeValue.Individual;
            result.ForEach(item => {
                item.Serial = index++;
                setBunkInfo(item, voyageType, passengerTypeValue, travelTypeValue,patPrice);
            });
            CheckFlights(result, itineraryType);
            return result;
        }
        private static DataTransferObject.FlightQuery.FlightView GetFlightView(Segment flightView, PassengerType passengerType) {
            var airline = Service.FoundationService.QueryAirline(flightView.AirlineCode);
            if(airline == null) throw new CustomException("暂不支持航空公司 " + flightView.AirlineCode);
            var basicPrice = Service.FoundationService.QueryBasicPrice(airline.Code.Value,
                                                                       flightView.AirportPair.Departure,
                                                                       flightView.AirportPair.Arrival,
                                                                       flightView.Date);
            if(basicPrice == null) throw new CustomException("系统中无运价信息");
            var bafView = Service.FoundationService.QueryBAF(airline.Code.Value, basicPrice.Mileage);
            var result = new DataTransferObject.FlightQuery.FlightView {
                AirlineCode = airline.Code.Value,
                AirlineName = airline.ShortName,
                FlightNo = flightView.InternalNo,
                Aircraft = flightView.AircraftType,
                Departure = getAirportView(flightView.AirportPair.Departure, flightView.TerminalOfDeparture, flightView.Date, flightView.DepartureTime),
                Arrival = getAirportView(flightView.AirportPair.Arrival, flightView.TerminalOfArrival, flightView.Date, flightView.ArrivalTime),
                YBPrice = basicPrice.Price,
                AirportFee = Service.FoundationService.QueryAirportFee(flightView.AircraftType, passengerType),
                BAF = passengerType == PassengerType.Adult ? bafView.Adult : bafView.Child,
                AdultBAF = bafView.Adult,
                ChildBAF = bafView.Child,
                BunkCode = flightView.CabinSeat,
                SeatCount = flightView.SeatCount,
                IsShare = flightView.IsShared
            };
            result.Arrival.Time = result.Arrival.Time.AddDays(flightView.AddDays);
            return result;
        }
        private static void setBunkInfo(FlightView flightView, VoyageTypeValue voyageType, PassengerTypeValue passengerType, TravelTypeValue travelType, PriceView patPrice) {
            var bunks = Service.FoundationService.QueryBunk(flightView.AirlineCode,
                                                            flightView.Departure.Code, flightView.Arrival.Code,
                                                            flightView.Departure.Time.Date, flightView.BunkCode,
                                                            voyageType, travelType, passengerType);
            //更改退改签规定数据源
            var pattern = new Regex("^[a-zA-Z\\d/]+$");
            var refundDetail = FoundationService.QueryDetailList(flightView.AirlineCode, flightView.BunkCode).Where(item => pattern.IsMatch(item.Bunks));
            string refundRegulation = string.Empty;
            string changeRegulation = string.Empty;
            string endorseRegulation = string.Empty;
            string remark = string.Empty;
            foreach (var item in refundDetail)
            {
                refundRegulation += ("航班起飞前：" + item.ScrapBefore + "；航班起飞后：" + item.ScrapAfter).Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                changeRegulation += ("航班起飞前：" + item.ChangeBefore+ "；航班起飞后：" + item.ChangeAfter).Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                endorseRegulation += item.Endorse.Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                remark = item.Remark.Replace(" ", "").Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace( "\t", "" );
            }
            if (string.IsNullOrWhiteSpace(refundRegulation))
                refundRegulation = "以航司具体规定为准";
            if (string.IsNullOrWhiteSpace(changeRegulation))
                changeRegulation = "以航司具体规定为准";
            foreach (var item in bunks)
            {
                item.RefundRegulation = refundRegulation;
                item.ChangeRegulation = changeRegulation;
                item.EndorseRegulation = endorseRegulation;
                item.Remarks = remark;
            }
            var bunk = chooseBunk(bunks, voyageType, flightView.YBPrice, patPrice);
            if(bunk != null) {
                flightView.EI = bunk.EI;
                flightView.BunkType = bunk.Type;
                // 明折明扣舱
                var generalBunk = bunk as Service.Foundation.Domain.GeneralBunk;
                if(generalBunk != null) {
                    var adultDiscount = generalBunk.GetDiscount(flightView.BunkCode);
                    var adultFare = Utility.Calculator.Round(flightView.YBPrice * adultDiscount, 1);
                    if(passengerType == PassengerTypeValue.Child) {
                        flightView.Discount = Utility.Calculator.Round(adultDiscount / 2, -3);
                        flightView.Fare = Utility.Calculator.Round(adultFare / 2, 1);
                    } else {
                        flightView.Discount = adultDiscount;
                        flightView.Fare = adultFare;
                    }
                    var firstBusinessBunk = generalBunk as Service.Foundation.Domain.FirstBusinessBunk;
                    if(firstBusinessBunk != null) {
                        flightView.BunkDescription = firstBusinessBunk.Description;
                    }
                    return;
                }
                // 特价舱
                var promotionBunk = bunk as Service.Foundation.Domain.PromotionBunk;
                if(promotionBunk != null) {
                    flightView.BunkDescription = promotionBunk.Description;
                    return;
                }
                // 免票舱
                var freeBunk = bunk as Service.Foundation.Domain.FreeBunk;
                if(freeBunk != null) {
                    flightView.BunkDescription = freeBunk.Description;
                    return;
                }
                // 往返产品舱、联程舱、团队舱
                if(bunk is Service.Foundation.Domain.ProductionBunk || bunk is Service.Foundation.Domain.TransferBunk || bunk is Service.Foundation.Domain.TeamBunk) {
                    return;
                }
                throw new CustomException("不支持该舱位导入");
            }
            throw new CustomException("未获取到支持该行程的相关舱位信息");
        }
        private static DataTransferObject.FlightQuery.AirportView getAirportView(string code, string terminal, DateTime flightDate, Time time) {
            var result = new DataTransferObject.FlightQuery.AirportView() {
                Code = code,
                Terminal = terminal,
                Time = time.ToDateTime(flightDate.Year, flightDate.Month, flightDate.Day)
            };
            var airport = Service.FoundationService.QueryAirport(code);
            if(airport != null) {
                result.Name = airport.ShortName;
                result.City = airport.Location == null ? string.Empty : airport.Location.Name;
            }
            return result;
        }
        private static VoyageTypeValue GetVoyageTypeValue(ItineraryType itineraryType, int count) {
            switch(itineraryType) {
                case ItineraryType.OneWay:
                    return VoyageTypeValue.OneWay;
                case ItineraryType.Roundtrip:
                    return VoyageTypeValue.RoundTrip;
                case ItineraryType.Conjunction:
                    return count > 2 ? VoyageTypeValue.OneWayOrRound : VoyageTypeValue.TransitWay;
                case ItineraryType.Notch:
                    return VoyageTypeValue.Notch;
                default:
                    throw new NotSupportedException("itineraryType");
            }
        }
        private static Bunk chooseBunk(IEnumerable<Bunk> bunks, VoyageTypeValue voyageType, decimal YBPrice, PriceView price) {
            if (bunks.Any(b => b is Service.Foundation.Domain.GeneralBunk) &&
                bunks.Any(b=>b is Service.Foundation.Domain.PromotionBunk))
            {
                var generalBunk = bunks.FirstOrDefault(b => b is GeneralBunk) as GeneralBunk;
                if (price == null) return generalBunk;
                decimal nomalPrice = Utility.Calculator.Round(generalBunk.GetDiscount(generalBunk.Code.Value)*YBPrice, 1);
                if (price.Fare >= nomalPrice)
                {
                    return generalBunk;
                }
                return bunks.FirstOrDefault(b => b is PromotionBunk);
            }
            if(bunks.Count() < 2) return bunks.FirstOrDefault();
            switch(voyageType) {
                case VoyageTypeValue.OneWay:
                    var generalBunk = bunks.FirstOrDefault(b => b is Service.Foundation.Domain.GeneralBunk);
                    if(generalBunk == null) {
                        return bunks.First();
                    }
                    return generalBunk;
                case VoyageTypeValue.RoundTrip:
                    var productionBunk = bunks.FirstOrDefault(b => b is Service.Foundation.Domain.ProductionBunk);
                    if(productionBunk == null) {
                        return bunks.First();
                    }
                    return productionBunk;
                default:
                    return bunks.First();
            }
        }
        private static void CheckFlights(IEnumerable<DataTransferObject.FlightQuery.FlightView> flightViews, ItineraryType itineraryType) {
            if(flightViews.Count() > 1) {
                // 所有航段必须是同一航空公司
                var firstFlight = flightViews.First();
                if(flightViews.Any(f => f.AirlineCode != firstFlight.AirlineCode)) throw new CustomException("所有航段必须是同一乘运人乘运");
                // 往返产品舱不能与其他舱位搭配
                var firstProdutionBunk = flightViews.FirstOrDefault(f => f.BunkType == BunkType.Production);
                if(firstProdutionBunk != null && !flightViews.All(f => f.BunkType == BunkType.Production && f.BunkCode == firstProdutionBunk.BunkCode)) {
                    throw new CustomException("往返产品舱只能与往返产品舱搭配销售");
                }
            }
        }
    }
}