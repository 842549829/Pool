using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Command.Domain;
using ChinaPay.B3B.Service.Command.Domain.DataTransferObject;
using ChinaPay.B3B.Service.Command.Domain.FlightQuery;
using ChinaPay.B3B.Service.Foundation.Domain;

namespace ChinaPay.B3B.Service.PriceCheck
{
    public static class PriceCheckService
    {
        private const string _systemauto = "SystemAuto";

        /// <summary>
        /// 使用指令检查运价并返回指定舱位的运价
        /// </summary>
        /// <param name="carrier">承运人</param>
        /// <param name="departure">出发地</param>
        /// <param name="arrival">到达地</param>
        /// <param name="bunkCode">舱位代码</param>
        /// <param name="flightDate">航班日期</param>
        /// <returns>指定舱位价格</returns>
        public static decimal CheckFd(string carrier, string departure, string arrival, string bunkCode, DateTime flightDate)
        {
            var fareInfo = CommandService.GetFare(new AirportPair(departure, arrival), flightDate, carrier, Guid.Empty);
            if (!fareInfo.Success) throw new DataException("查询运价信息失败");
            var fare = fareInfo.Result.GraduatedFareList.FirstOrDefault(f => (f.ClassOfService+f.SubClass) == bunkCode.ToUpper());
            if (fare == null) throw new DataException("不存在舱位运价");
            System.Threading.ThreadPool.QueueUserWorkItem((obj) =>
            {
                try
                {
                    FixBunkPrice(carrier, departure, arrival, flightDate, fareInfo.Result.GraduatedFareList,fareInfo.Result.Mileage);
                }
                catch (Exception ex)
                {
                    LogService.SaveExceptionLog(ex, carrier + departure + arrival + flightDate);
                }
            });
            return fare.OneWayFare;
        }

        /// <summary>
        /// 使用指令检查运价并返回指定舱位的运价,使用异步的方式，在航班预订时候使用
        /// </summary>
        /// <param name="carrier"></param>
        /// <param name="departure"></param>
        /// <param name="arrival"></param>
        /// <param name="flightDate"></param>
        public static void CheckFd(string carrier, string departure, string arrival, DateTime flightDate)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((obj) =>
            {
                try
                {
                    CheckFdAndFix(carrier, departure, arrival, flightDate);
                }
                catch (Exception ex)
                {
                    LogService.SaveExceptionLog(ex,carrier+departure+arrival+flightDate);
                }
            });
        }

        /// <summary>
        /// 使用异步方式调用指令检查运价并返回指定舱位的运价,并修改运价
        /// </summary>
        /// <param name="carrier">承运人</param>
        /// <param name="departure">出发地</param>
        /// <param name="arrival">到达地</param>
        /// <param name="flightDate">航班日期</param>
        /// <returns>指定舱位价格</returns>
        private static void CheckFdAndFix(string carrier, string departure, string arrival, DateTime flightDate)
        {
            var fareInfo = CommandService.GetFare(new AirportPair(departure, arrival), flightDate, carrier, Guid.Empty);
            if (!fareInfo.Success && !fareInfo.Result.GraduatedFareList.Any()) return;
            FixBunkPrice(carrier, departure, arrival, flightDate, fareInfo.Result.GraduatedFareList,fareInfo.Result.Mileage);
        }

        /// <summary>
        /// 根据FD数据更新舱位运价
        /// </summary>
        /// <param name="carrier">承运人</param>
        /// <param name="departure">出发地</param>
        /// <param name="arrival">到达地</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="fareData"></param>
        /// <param name="mileage"> </param>
        public static void FixBunkPrice(string carrier, string departure, string arrival, DateTime flightDate, List<GraduatedFare> fareData, decimal mileage)
        {
            var basePrice = fareData.FirstOrDefault(f => f.ClassOfService == "Y");
            if (basePrice == null) return;
            var b3bBasePrice = FoundationService.QueryBasicPrice(carrier, departure, arrival, flightDate);
            if (b3bBasePrice.Price != basePrice.OneWayFare)
            {
                var basicPriceView = new BasicPriceView()
                    {
                        Airline =b3bBasePrice.Airline!=null? b3bBasePrice.Airline.Code.Value:null,
                        Departure = b3bBasePrice.Departure!=null?b3bBasePrice.Departure.Code.Value:null,
                        Arrival = b3bBasePrice.Arrival!=null?b3bBasePrice.Arrival.Code.Value:null,
                        FlightDate = b3bBasePrice.FlightDate,
                        ETDZDate = b3bBasePrice.ETDZDate,
                        Price = basePrice.OneWayFare,
                        Mileage = b3bBasePrice.Mileage,//TODO   没有更新到最新里程数
                        ModifyTime = DateTime.Now
                    };
                if (b3bBasePrice.Airline==null||b3bBasePrice.FlightDate!=basePrice.EffectiveDate)
                {
                    basicPriceView.Airline = carrier;
                    basicPriceView.Departure = departure;
                    basicPriceView.Arrival = arrival;
                    basicPriceView.FlightDate = basePrice.EffectiveDate;
                    basicPriceView.ETDZDate = DateTime.Today < basePrice.EffectiveDate ? DateTime.Today : basePrice.EffectiveDate;
                    basicPriceView.Mileage = mileage;
                    FoundationService.AddBasicPrice(basicPriceView, _systemauto);
                }
                else
                {
                    FoundationService.UpdateBasicPrice(b3bBasePrice.Id, basicPriceView, _systemauto);
                }
            }
            var bunks = FoundationService.QueryBunk(carrier, departure, arrival, flightDate).OfType<GeneralBunk>();
            var diffPriceBunks = new Dictionary<Guid,BunkView>();
            List<string> FixedBunk = new List<string>();
            var newBunks = new List<BunkView>();
            //var subBunkReg = new Regex("\\w\\d");
            foreach (GraduatedFare fare in fareData)
            {
                if (fare.ApplyType == ApplyType.Roundtrip) continue;
                var isSubBunk = !string.IsNullOrEmpty(fare.SubClass);//subBunkReg.IsMatch(fare.ClassOfService);
                var bunk = bunks.FirstOrDefault(b => b.Code.Value == fare.ClassOfService);
                if (!isSubBunk)
                {
                    if(FixedBunk.Contains(fare.ClassOfService)) continue;
                    if (bunk == null||bunk.Airline==null||bunk.Departure==null||bunk.Arrival==null) 
                        newBunks.Add(CreatBunkFromFDData(fare,carrier,departure,arrival, basePrice.OneWayFare));
                    else if (FoundationService.CalculateFare(basePrice.OneWayFare, bunk.Discount) != fare.OneWayFare)
                    {
                        //var bunkView = GeneralBunkView(bunk,fare, basePrice);
                        //diffPriceBunks.Add(bunk.Id,bunkView);
                        var bunkView = CreatBunkFromFDData(fare, carrier, departure, arrival, basePrice.OneWayFare);
                        newBunks.Add(bunkView);
                    }
                    FixedBunk.Add(fare.ClassOfService);
                }
                else
                {
                    var extendedBunkView = new ExtendedWithDiscountBunkView()
                    {
                        Code = fare.ClassOfService+fare.SubClass,
                        Discount = FoundationService.CalculateDiscount(basePrice.OneWayFare, fare.OneWayFare)
                    };
                    if (bunk == null || bunk.Airline == null || bunk.Departure == null || bunk.Arrival == null)
                    {//在舱位信息不够具体的时候添加子舱位
                        var tobeaddedbunkView = newBunks.FirstOrDefault(b => b.Code == fare.ClassOfService);
                        if (tobeaddedbunkView != null)
                        {//子舱位所在的舱位代码是新添加的
                            if (tobeaddedbunkView is FirstBusinessBunkView)
                            {
                                var fbBunkView = tobeaddedbunkView as FirstBusinessBunkView;
                                fbBunkView.AddExtended(extendedBunkView);
                            }
                            else
                            {
                                var eBunkView = tobeaddedbunkView as EconomicBunkView;
                                eBunkView.AddExtended(extendedBunkView);
                            }
                        }
                    }
                    else
                    {
                        BunkView bunkview = newBunks.FirstOrDefault(b => b.Code == fare.ClassOfService) ?? diffPriceBunks.FirstOrDefault(b=>b.Value.Code==fare.ClassOfService).Value;
                        bunkview = bunkview ?? GeneralBunkView(bunk, fare, basePrice,false);
                        if (bunkview is FirstBusinessBunkView)
                        {
                            var fbBunkView = bunkview as FirstBusinessBunkView;
                            var existsExtBunk = fbBunkView.Extended.FirstOrDefault(b => b.Code == extendedBunkView.Code);
                            if (existsExtBunk != null)
                            {
                                existsExtBunk.Discount = extendedBunkView.Discount;
                                if (!diffPriceBunks.ContainsKey(bunk.Id)) diffPriceBunks.Add(bunk.Id, fbBunkView);
                            }
                            else fbBunkView.AddExtended(extendedBunkView);
                        }
                        else
                        {
                            var fbBunkView = bunkview as EconomicBunkView;
                            var existsExtBunk = fbBunkView.Extended.FirstOrDefault(b => b.Code == extendedBunkView.Code);
                            if (existsExtBunk != null)
                            {
                                existsExtBunk.Discount = extendedBunkView.Discount;
                                if (!diffPriceBunks.ContainsKey(bunk.Id)) diffPriceBunks.Add(bunk.Id, fbBunkView);
                            }
                            else fbBunkView.AddExtended(extendedBunkView);
                        }
                    }
                }

            }
            foreach (KeyValuePair<Guid, BunkView> bunk in diffPriceBunks)
            {
                FoundationService.UpdateBunk(bunk.Key, bunk.Value, _systemauto);
            }
            newBunks.ForEach(b => FoundationService.AddBunk(b, _systemauto));
        }

        /// <summary>
        /// 构造舱位修改信息
        /// </summary>
        /// <param name="bunk"></param>
        /// <param name="fare"></param>
        /// <param name="basePrice"></param>
        /// <param name="useNewPrice"> 是否使用新价格，在为子舱位寻找载体的时候不修改舱位价格</param>
        /// <returns></returns>
        private static GeneralBunkView GeneralBunkView(GeneralBunk bunk, GraduatedFare fare, GraduatedFare basePrice,bool useNewPrice=true)
        {
            GeneralBunkView bunkView = fare.ServiceType == "Y"
                                           ? (GeneralBunkView)new EconomicBunkView()
                                               {
                                                   Discount = useNewPrice?FoundationService.CalculateDiscount(basePrice.OneWayFare, fare.OneWayFare):bunk.Discount
                                               }
                                           : new FirstBusinessBunkView()
                                               {
                                                   Description = fare.ServiceType == "C" ? "公务舱" : "头等舱",
                                                   Discount = useNewPrice?FoundationService.CalculateDiscount(basePrice.OneWayFare, fare.OneWayFare):bunk.Discount
                                               };
            bunkView.Departure = bunk.Departure.Code.Value;
            bunkView.Arrival = bunk.Arrival.Code.Value;
            bunkView.Code = fare.ClassOfService;
            bunkView.Remarks = bunk.Remarks;
            bunkView.Valid = bunk.Valid;
            bunkView.Airline = bunk.Airline.Code.Value;
            bunkView.Code = bunk.Code.Value;
            bunkView.RefundRegulation = bunk.RefundRegulation;
            bunkView.ChangeRegulation = bunk.ChangeRegulation;
            bunkView.EndorseRegulation = bunk.EndorseRegulation;
            bunkView.FlightBeginDate = fare.EffectiveDate;
            bunkView.FlightEndDate = fare.ExpiryDate;
            bunkView.ETDZDate = DateTime.Today < fare.EffectiveDate ? DateTime.Today : fare.EffectiveDate;
            bunkView.VoyageType = bunk.VoyageType;
            bunkView.PassengerType = bunk.PassengerType;
            bunkView.TravelType = bunk.TravelType;
            foreach (ExtendedWithDiscountBunk extBunk in bunk.Extended)
            {
                bunkView.AddExtended(new ExtendedWithDiscountBunkView
                {
                    Code = extBunk.Code.Value,
                    Discount = extBunk.Discount
                });
            }
            return bunkView;
        }

        /// <summary>
        /// 构造新的舱位信息
        /// </summary>
        /// <param name="fare">舱位运价信息</param>
        /// <param name="arrival"> </param>
        /// <param name="basePrice"> </param>
        /// <param name="carrier"> </param>
        /// <param name="departure"> </param>
        /// <returns></returns>
        private static BunkView CreatBunkFromFDData(GraduatedFare fare,string carrier,string departure,string arrival, decimal basePrice)
        {
            decimal bunkDiscount = FoundationService.CalculateDiscount(basePrice, fare.OneWayFare);
            GeneralBunkView bunkView = fare.ServiceType == "Y"
                                           ? (GeneralBunkView)new EconomicBunkView()
                                           {
                                               Discount = bunkDiscount
                                           }
                                           : new FirstBusinessBunkView()
                                           {
                                               Description = fare.ServiceType == "C" ? "公务舱" : "头等舱",
                                               Discount = bunkDiscount
                                           };
            bunkView.Departure = departure;
            bunkView.Arrival = arrival;
            bunkView.Code = fare.ClassOfService;
            bunkView.Remarks = string.Empty;
            bunkView.Valid = true;
            bunkView.Airline = carrier;
            bunkView.Code = fare.ClassOfService;
            bunkView.RefundRegulation = string.Empty;
            bunkView.ChangeRegulation = string.Empty;
            bunkView.EndorseRegulation = string.Empty;
            bunkView.ETDZDate = DateTime.Today < fare.EffectiveDate ? DateTime.Today : fare.EffectiveDate;
            bunkView.FlightBeginDate = fare.EffectiveDate;
            bunkView.FlightEndDate = fare.ExpiryDate;
            bunkView.PassengerType = PassengerTypeValue.Adult;
            if (fare.OneWayFare>=basePrice) bunkView.PassengerType|=PassengerTypeValue.Child;
            bunkView.VoyageType = VoyageTypeValue.OneWay | VoyageTypeValue.RoundTrip 
                | VoyageTypeValue.TransitWay | VoyageTypeValue.OneWayOrRound | VoyageTypeValue.Notch;
            bunkView.TravelType = TravelTypeValue.Individual|TravelTypeValue.Team;
            return bunkView;
        }
    }
}
