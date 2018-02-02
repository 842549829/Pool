using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.B3B.Service.Foundation.Domain;
using ChinaPay.Core;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Foundation.Repository;

namespace ChinaPay.B3B.Service {
    /// <summary>
    /// 基础数据服务
    /// </summary>
    public static class FoundationService {
        #region "查询"
        public static IEnumerable<Area> Areas {
            get {
                return AreaCollection.Instance.Values;
            }
        }
        /// <summary>
        /// 省份
        /// </summary>
        public static IEnumerable<Province> Provinces {
            get {
                return ProvinceCollection.Instance.Values;
            }
        }
        /// <summary>
        /// 市
        /// </summary>
        public static IEnumerable<City> Cities {
            get {
                return CityCollection.Instance.Values;
            }
        }
        /// <summary>
        /// 县
        /// </summary>
        public static IEnumerable<County> Counties {
            get {
                return CountyCollection.Instance.Values;
            }
        }
        /// <summary>
        /// 机场
        /// </summary>
        public static IEnumerable<Airport> Airports {
            get {
                return AirportCollection.Instance.Values;
            }
        }
        /// <summary>
        /// 航空公司
        /// </summary>
        public static IEnumerable<Airline> Airlines {
            get {
                return AirlineCollection.Instance.Values;
            }
        }
        /// <summary>
        /// 机型
        /// </summary>
        public static IEnumerable<AirCraft> AirCrafts {
            get {
                return AirCraftCollection.Instance.Values;
            }
        }
        /// <summary>
        /// 燃油附加税
        /// </summary>
        public static IEnumerable<BAF> BAFs {
            get {
                return BAFCollection.Instance.Values;
            }
        }
        /// <summary>
        /// 基础运价
        /// </summary>
        public static IEnumerable<BasicPrice> BasicPrices {
            get {
                return BasicPriceCollection.Instance.Values;
            }
        }
        /// <summary>
        /// 舱位
        /// </summary>
        public static IEnumerable<Bunk> Bunks {
            get {
                return BunkCollection.Instance.Values;
            }
        }
        /// <summary>
        /// 儿童舱位
        /// </summary>
        public static IEnumerable<ChildOrderableBunk> ChildOrderableBunks {
            get {
                return ChildOrderableBunkCollection.Instance.Values;
            }
        }
        /// <summary>
        /// 退改签客规
        /// </summary>
        public static IEnumerable<RefundAndRescheduling> RefundAndReschedulings {
            get {
                var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingRepository();
                return repository.Query();
            }
        }
        /// <summary>
        /// 退改签客规
        /// </summary>
        public static IEnumerable<RefundAndReschedulingBase> NewRefundAndReschedulings {
            get {
                var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
                return repository.Query();
            }
        }
        public static IEnumerable<FixedNavigationView> FixedNavigations {
            get {
                return Foundation.Domain.FixedNavigations.Instance.Values;
            }
        }
        /// <summary>
        /// 值机
        /// </summary>
        public static IEnumerable<Check_In> Check_Ins {
            get {
                return Check_InCellection.Instance.Values;
            }
        }

        /// <summary>
        /// 查询区域
        /// </summary>
        /// <param name="code">区域代码</param>
        public static Area QueryArea(string code) {
            if(string.IsNullOrWhiteSpace(code))
                return null;
            return AreaCollection.Instance[code];
        }
        /// <summary>
        /// 查询省份
        /// </summary>
        /// <param name="code">省份代码</param>
        public static Province QueryProvice(string code) {
            if(string.IsNullOrWhiteSpace(code))
                return null;
            return ProvinceCollection.Instance[code];
        }
        /// <summary>
        /// 查询市
        /// </summary>
        /// <param name="code">市代码</param>
        public static City QueryCity(string code) {
            if(string.IsNullOrWhiteSpace(code))
                return null;
            return CityCollection.Instance[code];
        }
        /// <summary>
        /// 查询县
        /// </summary>
        /// <param name="code">县代码</param>
        public static County QueryCounty(string code) {
            if(string.IsNullOrWhiteSpace(code))
                return null;
            return CountyCollection.Instance[code];
        }
        /// <summary>
        /// 查询机场
        /// </summary>
        /// <param name="code">机场代码</param>
        public static Airport QueryAirport(UpperString code) {
            if(code.IsNullOrEmpty())
                return null;
            return AirportCollection.Instance[code];
        }
        /// <summary>
        /// 查询航空公司
        /// </summary>
        /// <param name="code">航空公司代码</param>
        public static Airline QueryAirline(UpperString code) {
            if(code.IsNullOrEmpty())
                return null;
            return AirlineCollection.Instance[code];
        }
        /// <summary>
        /// 查询机型
        /// </summary>
        /// <param name="id">机型ID</param>
        public static AirCraft QueryAirCraft(Guid id) {
            return AirCraftCollection.Instance[id];
        }
        /// <summary>
        /// 查询燃油附加税
        /// </summary>
        /// <param name="id">燃油附加税ID</param>
        public static BAF QueryBAF(Guid id) {
            return BAFCollection.Instance[id];
        }
        /// <summary>
        /// 查询基础运价(缓存中取)
        /// </summary>
        /// <param name="id">基础运价ID</param>
        public static BasicPrice QueryBasicPrice(Guid id) {
            return BasicPriceCollection.Instance[id];
        }
        /// <summary>
        /// 查询基础运价
        /// </summary>
        /// <param name="id">基础运价ID</param>
        public static BasicPrice QueryBasicPriceNew(Guid basicPriceId)
        {
            var repository = Factory.CreateBasicPriceRepository();
            return repository.QueryBasicPrice(basicPriceId);
        }
        /// <summary>
        /// 查询舱位
        /// </summary>
        /// <param name="id">舱位ID</param>
        public static Bunk QueryBunk(Guid id) {
            return BunkCollection.Instance[id];
        }
        /// <summary>
        /// 查询舱位
        /// </summary>
        /// <param name="id">舱位ID</param>
        public static Bunk QueryBunkNew(Guid id)
        {
            var repository = Factory.CreateBunkRepository();
            return repository.QueryBunkNew(id);
        }
        /// <summary>
        /// 查询儿童舱位
        /// </summary>
        /// <param name="id">儿童舱位ID</param>
        public static ChildOrderableBunk QueryChildOrderableBunk(Guid id) {
            return ChildOrderableBunkCollection.Instance[id];
        }
        /// <summary>
        /// 查询退改签客规
        /// </summary>
        /// <param name="id">客规ID</param>
        public static RefundAndRescheduling QueryRefundAndRescheduling(UpperString airline) {
            if(airline.IsNullOrEmpty())
                throw new ArgumentNullException("airline");
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingRepository();
            return repository.Query(airline);
        }
        /// <summary>
        /// 查询退改签客规
        /// </summary>
        /// <param name="id">客规ID</param>
        public static RefundAndReschedulingBase QueryRefundAndReschedulingNewBase(UpperString airline) {
            if(airline.IsNullOrEmpty())
                throw new ArgumentNullException("airline");
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
            return repository.Query(airline);
        }
        public static RefundAndReschedulingBase QueryRefundAndReschedulingNew(UpperString airline) {
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
            return repository.QueryRefundAndRescheduling(airline);
        }
        public static RefundAndReschedulingDetail QueryRefundAndReschedulingNewDetail(Guid detailId) {
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
            return repository.Query(detailId);
        }
        public static IEnumerable<RefundAndReschedulingDetail> QueryDetailList(string airline) {
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
            return repository.Query(airline);
        }
        public static FixedNavigationView QueryFixedNavigation(UpperString departure, UpperString arrival) {
            if(departure.IsNullOrEmpty()) throw new ArgumentNullException("departure");
            if(arrival.IsNullOrEmpty()) throw new ArgumentNullException("arrival");
            return Foundation.Domain.FixedNavigations.Instance.Query(departure, arrival);
        }
        /// <summary>
        /// 查询退改签客规
        /// </summary>
        /// <param name="airline">航空公司</param>
        /// <param name="bunk">舱位</param>
        public static IEnumerable<RefundAndReschedulingDetailView> QueryDetailList(string airline, string bunk) {
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
            return repository.Query(airline, bunk);
        }
        /// <summary>
        /// 查询机场所在省份
        /// </summary>
        /// <param name="airportCode">机场代码</param>
        public static string QueryPrvinceCodeByAirport(string airportCode) {
            var airport = QueryAirport(airportCode);
            if(airport != null) {
                var location = airport.Location;
                City city = null;
                if(location is City) {
                    city = location as City;
                } else if(location is County) {
                    city = (location as County).City;
                }
                if(city != null) {
                    return city.ProvinceCode;
                }
            }
            return string.Empty;
        }
        public static Area QueryAreaByName(string name) {
            return AreaCollection.Instance.Values.FirstOrDefault(item => item.Name == name);
        }
        public static Province QueryProvinceByName(string name) {
            return ProvinceCollection.Instance.Values.FirstOrDefault(item => item.Name == name);
        }
        public static City QueryCityByName(string name) {
            return CityCollection.Instance.Values.FirstOrDefault(item => item.Name == name);
        }
        public static County QueryCountyByName(string name) {
            return CountyCollection.Instance.Values.FirstOrDefault(item => item.Name == name);
        }
        public static string QueryCityNameByAirportCode(string code) {
            var airport = QueryAirport(code);
            if(airport != null) {
                var location = airport.Location;
                if(location != null) return location.Name;
            }
            return code;
        }
        public static string QueryAirportName(string code) {
            var airport = QueryAirport(code);
            return airport == null ? code : airport.Name;
        }
        /// <summary>
        /// 查询航空公司退改签规定
        /// </summary>
        public static IEnumerable<AirlineRulesView> QueryAllRefundAndReschedulings(string airline) {
            var repository = Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
            return repository.QueryAllRefundAndReschedulings(airline);
        }
        private static Airline QueryAirlineByName(string name) {
            return AirlineCollection.Instance.Values.FirstOrDefault(item => item.Name == name);
        }
        private static Airline QueryAirlineByShortName(string shortName) {
            return AirlineCollection.Instance.Values.FirstOrDefault(item => item.ShortName == shortName);
        }
        private static Airport QueryAirportByName(string name) {
            return AirportCollection.Instance.Values.FirstOrDefault(item => item.Name == name);
        }
        private static Airport QueryAirportByShortName(string shortName) {
            return AirportCollection.Instance.Values.FirstOrDefault(item => item.ShortName == shortName);
        }
        private static AirCraft QueryAirCraft(UpperString code) {
            return AirCraftCollection.Instance.Values.FirstOrDefault(item => item.Code.Value == code.Value);
        }
        private static ChildOrderableBunk QueryChildOrderableBunk(UpperString airline, UpperString bunk) {
            return ChildOrderableBunkCollection.Instance.Values.FirstOrDefault(item => item.AirlineCode.Value == airline.Value && item.BunkCode.Value == bunk.Value);
        }

        /// <summary>
        /// 查询机场建设费
        /// </summary>
        /// <param name="code">机型代码</param>
        public static decimal QueryAirportFee(UpperString code, PassengerType passengerType = PassengerType.Adult) {
            if(passengerType == PassengerType.Child) return 0;
            var airCraft = QueryAirCraft(code);
            return null == airCraft ? 50m : airCraft.AirportFee;
        }
        /// <summary>
        /// 查询基础运价信息
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        /// <param name="departure">出发机场代码</param>
        /// <param name="arrival">到达机场代码</param>
        /// <param name="flightDate">航班日期</param>
        public static BasicPrice QueryBasicPrice(string airline, string departure, string arrival, DateTime flightDate) {
            return BasicPriceCollection.Instance.QueryBasicPrice(airline, departure, arrival, flightDate);
        }
        /// <summary>
        /// 查询舱位价格
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        /// <param name="departure">出发机场代码</param>
        /// <param name="arrival">到达机场代码</param>
        /// <param name="flightDate">航班日期</param>
        public static decimal QueryBasicPriceValue(string airline, string departure, string arrival, DateTime flightDate) {
            var basicPrice = QueryBasicPrice(airline, departure, arrival, flightDate);
            return basicPrice == null ? 0 : basicPrice.Price;
        }
        /// <summary>
        /// 查询里程数
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        /// <param name="departure">出发机场代码</param>
        /// <param name="arrival">到达机场代码</param>
        /// <param name="flightDate">航班日期</param>
        public static decimal QueryMileage(string airline, string departure, string arrival, DateTime flightDate) {
            var basicPrice = QueryBasicPrice(airline, departure, arrival, flightDate);
            return basicPrice == null ? 0 : basicPrice.Mileage;
        }
        /// <summary>
        /// 查询燃油附加税
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        /// <param name="mileage">里程数</param>
        public static BAFValueView QueryBAF(string airline, decimal mileage) {
            var bafModel = BAFCollection.Instance.QueryBAF(airline, mileage, DateTime.Now);
            if(bafModel == null) {
                return new DataTransferObject.Foundation.BAFValueView(0, 0);
            } else {
                return new DataTransferObject.Foundation.BAFValueView(bafModel.Adult, bafModel.Child);
            }
        }
        /// <summary>
        /// 查询燃油附加税
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        /// <param name="departure">出发机场代码</param>
        /// <param name="arrival">到达机场代码</param>
        /// <param name="flightDate">航班日期</param>
        public static BAFValueView QueryBAF(string airline, string departure, string arrival, DateTime flightDate) {
            var basicPrice = QueryBasicPrice(airline, departure, arrival, flightDate);
            if(basicPrice != null) {
                return QueryBAF(airline, basicPrice.Mileage);
            }
            return new DataTransferObject.Foundation.BAFValueView(0, 0);
        }
        ///// <summary>
        ///// 查询舱位信息
        ///// </summary>
        ///// <param name="airline">航空公司代码</param>
        ///// <param name="bunkCode">舱位代码</param>
        ///// <param name="departure">出发机场代码</param>
        ///// <param name="arrival">到达机场代码</param>
        ///// <param name="flightDate">航班日期</param>
        //public static IEnumerable<Bunk> QueryBunk(string airline, string bunkCode, string departure, string arrival, DateTime flightDate) {
        //    return BunkCollection.Instance.QueryBunk(airline, bunkCode, departure, arrival, flightDate);
        //}
        /// <summary>
        /// 查询舱位信息
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        /// <param name="departure">出发机场代码</param>
        /// <param name="arrival">到达机场代码</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="bunkCode">舱位代码</param>
        public static IEnumerable<Bunk> QueryBunk(string airline, string departure, string arrival, DateTime flightDate, string bunkCode) {
            return BunkCollection.Instance.QueryBunk(airline, departure, arrival, flightDate, bunkCode);
        }

        /// <summary>
        /// 查询舱位信息
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        /// <param name="departure">出发机场代码</param>
        /// <param name="arrival">到达机场代码</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="bunkCode">舱位代码</param>
        public static IEnumerable<Bunk> QueryBunk(string airline, string departure, string arrival, DateTime flightDate)
        {
            return BunkCollection.Instance.QueryAllGeneralBunks(airline, departure, arrival, flightDate);
        }
        /// <summary>
        /// 查询舱位信息
        /// </summary>
        /// <param name="airline">航空公司代码</param>
        /// <param name="departure">出发机场代码</param>
        /// <param name="arrival">到达机场代码</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="bunkCode">舱位代码</param>
        /// <param name="itineraryType">行程类型</param>
        /// <param name="travelType">旅行类型</param>
        /// <param name="passengerType">旅客类型</param>
        public static IEnumerable<Bunk> QueryBunk(string airline, string departure, string arrival, DateTime flightDate, string bunkCode,
            VoyageTypeValue itineraryType, TravelTypeValue travelType, PassengerTypeValue passengerType) {
            return BunkCollection.Instance.QueryBunk(airline, departure, arrival, flightDate, bunkCode, itineraryType, travelType, passengerType);
        }
        /// <summary>
        /// 查询基础运价
        /// </summary>
        /// <param name="airline">航空公司</param>
        /// <param name="departure">出发城市</param>
        /// <param name="arrival">到达城市</param>
        /// <param name="pagination">分页信息</param>
        /// <returns></returns>
        public static IEnumerable<BasicPriceView> QueryBasicPriceView(string airline,string departure, string arrival,Pagination pagination)
        {
            var repository = Factory.CreateBasicPriceRepository();
            return repository.QueryBasicPrice(airline, departure, arrival, pagination);
        }
        /// <summary>
        /// 查询舱位列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="pagination">分页信息</param>
        public static IEnumerable<Bunk> QueryBunkListView(BunkQueryCondition condition,Pagination pagination)
        {
            var repository = Factory.CreateBunkRepository();
            return repository.QueryBunkListView(condition, pagination);
        }
        #endregion
        #region "新增"
        public static void AddArea(AreaView areaView, string account) {
            var area = Area.GetArea(areaView);
            if(QueryArea(area.Code) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("代码[" + area.Code + "]已存在");
            if(QueryAreaByName(area.Name) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("名称[" + area.Name + "]已存在");
            AreaCollection.Instance.Add(area.Code, area);
            saveAddLog("区域", area.ToString(), area.Code, account);
        }
        public static void AddProvince(ProvinceView provinceView, string account) {
            var province = Province.GetProvince(provinceView);
            if(QueryProvice(province.Code) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("代码[" + province.Code + "]已存在");
            if(QueryProvinceByName(province.Name) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("名称[" + province.Name + "]已存在");
            ProvinceCollection.Instance.Add(province.Code, province);
            saveAddLog("省", province.ToString(), province.Code, account);
        }
        public static void AddCity(CityView cityView, string account) {
            var city = City.GetCity(cityView);
            if(QueryCity(city.Code) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("代码[" + city.Code + "]已存在");
            var sameNameCity = QueryCityByName(city.Name);
            if(sameNameCity != null && sameNameCity.ProvinceCode == city.ProvinceCode)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("名称[" + city.Name + "]已存在");
            CityCollection.Instance.Add(city.Code, city);
            saveAddLog("市", city.ToString(), city.Code, account);
        }
        public static void AddCounty(CountyView countyView, string account) {
            var county = County.GetCounty(countyView);
            if(QueryCounty(county.Code) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("代码[" + county.Code + "]已存在");
            var sameNameCounty = QueryCountyByName(county.Name);
            if(sameNameCounty != null && sameNameCounty.CityCode == county.CityCode)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("名称[" + county.Name + "]已存在");
            CountyCollection.Instance.Add(county.Code, county);
            saveAddLog("县", county.ToString(), county.Code, account);
        }
        public static void AddAirport(AirportView airportView, string account) {
            var airport = Airport.GetAirport(airportView);
            if(QueryAirport(airport.Code) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("代码[" + airport.Code + "]已存在");
            if(QueryAirportByName(airport.Name) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("名称[" + airport.Name + "]已存在");
            if(QueryAirportByShortName(airport.ShortName) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("简称[" + airport.ShortName + "]已存在");
            AirportCollection.Instance.Add(airport.Code, airport);
            saveAddLog("机场", airport.ToString(), airport.Code.Value, account);
        }
        public static void AddAirline(AirlineView airlineView, string account) {
            var airline = Airline.GetAirline(airlineView);
            if(QueryAirline(airline.Code) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("代码[" + airline.Code + "]已存在");
            if(QueryAirlineByName(airline.Name) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("名称[" + airline.Name + "]已存在");
            if(QueryAirlineByShortName(airline.ShortName) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("简称[" + airline.ShortName + "]已存在");
            AirlineCollection.Instance.Add(airline.Code, airline);
            saveAddLog("航空公司", airline.ToString(), airline.Code.Value, account);
        }
        public static void AddAirCraft(AirCraftView airCraftView, string account) {
            var airCraft = AirCraft.GetAirCraft(airCraftView);
            if(QueryAirCraft(airCraft.Code) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("机型[" + airCraft.Code.Value + "]已存在");
            AirCraftCollection.Instance.Add(airCraft.Id, airCraft);
            saveAddLog("机型", airCraft.ToString(), airCraft.Id.ToString(), account);
        }
        public static void AddBAF(BAFView bafView, string account) {
            var baf = BAF.GetBAF(bafView);
            BAFCollection.Instance.Add(baf.Id, baf);
            saveAddLog("燃油附加税", baf.ToString(), baf.Id.ToString(), account);
        }
        public static void AddBasicPrice(BasicPriceView basicPriceView, string account) {
            basicPriceView.ModifyTime = DateTime.Now;
            var basicPrice = BasicPrice.GetBasicPrice(basicPriceView);
            BasicPriceCollection.Instance.Add(basicPrice.Id, basicPrice);
            saveAddLog("基础运价", basicPrice.ToString(), basicPrice.Id.ToString(), account);
        }
        public static void AddBunk(BunkView bunkView, string account) {
            var bunk = Bunk.CreateBunk(bunkView);
            BunkCollection.Instance.Add(bunk.Id, bunk);
            saveAddLog("舱位", bunk.ToString(), bunk.Id.ToString(), account);
        }
        public static void AddChildrenOrderableBunk(ChildOrderableBunkView childOrderableBunkView, string account) {
            var childOrderableBunk = ChildOrderableBunk.GetChildOrderableBunk(childOrderableBunkView);
            if(QueryChildOrderableBunk(childOrderableBunk.AirlineCode, childOrderableBunk.BunkCode) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException(string.Format("航空公司[{0}]的舱位[{1}]已存在", childOrderableBunk.AirlineCode.Value, childOrderableBunk.BunkCode.Value));
            ChildOrderableBunkCollection.Instance.Add(childOrderableBunk.Id, childOrderableBunk);
            saveAddLog("儿童可预订舱位", childOrderableBunk.ToString(), childOrderableBunk.Id.ToString(), account);
        }
        public static void AddRefundAndRescheduling(RefundAndReschedulingView refundAndReschedulingView, string account) {
            var refundAndRescheduling = RefundAndRescheduling.GetRefundAndRescheduling(refundAndReschedulingView);
            if(QueryRefundAndRescheduling(refundAndRescheduling.AirlineCode) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("航空公司[" + refundAndRescheduling.AirlineCode + "]的退改签客规已存在");
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingRepository();
            repository.Insert(refundAndRescheduling);
            saveLog(OperationType.Insert, "添加退改签客规", refundAndRescheduling.AirlineCode.Value, account);
        }
        public static void AddRefundAndReschedulingNewBase(RefundAndReschedulingBaseView refundAndReschedulingView, string account) {
            var refundAndRescheduling = RefundAndReschedulingBase.GetRefundAndRescheduling(refundAndReschedulingView);
            if(QueryRefundAndRescheduling(refundAndRescheduling.AirlineCode) != null)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("航空公司[" + refundAndRescheduling.AirlineCode + "]的退改签客规已存在");
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
            repository.Insert(refundAndRescheduling);
            saveLog(OperationType.Insert, "添加退改签客规基础信息", refundAndRescheduling.AirlineCode.Value, account);
        }
        public static void AddFixedNavigation(FixedNavigationView fixedNavigationView, string account) {
            if(Service.Foundation.Domain.FixedNavigations.Instance.Query(fixedNavigationView.Departure, fixedNavigationView.Arrival) != null) {
                throw new ChinaPay.Core.Exception.KeyRepeatedException("[" + fixedNavigationView.Departure + "]-[" + fixedNavigationView.Arrival + "] 此条航线已经存在");
            }
            Service.Foundation.Domain.FixedNavigations.Instance.Insert(fixedNavigationView);
            saveAddLog("非固定航线", fixedNavigationView.ToString(), fixedNavigationView.Departure + fixedNavigationView.Arrival, account);
        }
        public static void AddRefundAndReschedulingNewDetail(RefundAndReschedulingDetail detail, string account) {
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
            repository.Insert(detail);
            saveLog(OperationType.Insert, "添加退改签客规详细信息", detail.Id.ToString(), account);
        }
        public static void AddCheck_In(Check_In check_in, string account) 
        {
            ChinaPay.B3B.Service.Foundation.Domain.Check_InCellection.Instance.Add(check_in.Id, check_in);
            saveLog(OperationType.Insert, "添加值机详细信息", check_in.Id.ToString(), account);
        }
             
        #endregion
        #region "修改"
        public static void UpdateArea(AreaView areaView, string account) {
            var area = Area.GetArea(areaView);
            var originalArea = QueryArea(area.Code);
            if(null == originalArea)
                throw new ChinaPay.Core.CustomException("原区域不存在");
            var sameNameArea = QueryAreaByName(area.Name);
            if(sameNameArea != null && sameNameArea.Code != area.Code)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("名称[" + area.Name + "]已存在");
            var originalContent = originalArea.ToString();
            AreaCollection.Instance.Update(area.Code, area);
            saveUpdateLog("区域", originalContent, area.ToString(), area.Code, account);
        }
        public static void UpdateProvince(ProvinceView provinceView, string account) {
            var province = Province.GetProvince(provinceView);
            var originalProvince = QueryProvice(province.Code);
            if(null == originalProvince)
                throw new ChinaPay.Core.CustomException("原省份不存在");
            var sameNameProvince = QueryProvinceByName(province.Name);
            if(sameNameProvince != null && sameNameProvince.Code != province.Code)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("名称[" + province.Name + "]已存在");
            var originalContent = originalProvince.ToString();
            ProvinceCollection.Instance.Update(province.Code, province);
            saveUpdateLog("省份", originalContent, province.ToString(), province.Code, account);
        }
        public static void UpdateCity(CityView cityView, string account) {
            var city = City.GetCity(cityView);
            var originalCity = QueryCity(city.Code);
            if(null == originalCity)
                throw new ChinaPay.Core.CustomException("原市不存在");
            var sameNameCity = QueryCityByName(city.Name);
            if(sameNameCity != null && sameNameCity.Code != city.Code && sameNameCity.ProvinceCode == city.ProvinceCode)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("名称[" + city.Name + "]已存在");
            var originalContent = originalCity.ToString();
            CityCollection.Instance.Update(city.Code, city);
            saveUpdateLog("市", originalContent, city.ToString(), city.Code, account);
        }
        public static void UpdateCounty(CountyView countyView, string account) {
            var county = County.GetCounty(countyView);
            var originalCounty = QueryCounty(county.Code);
            if(null == originalCounty)
                throw new ChinaPay.Core.CustomException("原县不存在");
            var sameNameCounty = QueryCountyByName(county.Name);
            if(sameNameCounty != null && sameNameCounty.Code != county.Code && sameNameCounty.CityCode == county.CityCode)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("名称[" + county.Name + "]已存在");
            var originalContent = originalCounty.ToString();
            CountyCollection.Instance.Update(county.Code, county);
            saveUpdateLog("县", originalContent, county.ToString(), county.Code, account);
        }
        public static void UpdateAirport(AirportView airportView, string account) {
            var airport = Airport.GetAirport(airportView);
            var originalAirport = QueryAirport(airport.Code);
            if(null == originalAirport)
                throw new ChinaPay.Core.CustomException("原机场不存在");
            var sameNameAirport = QueryAirportByName(airport.Name);
            if(sameNameAirport != null && sameNameAirport.Code.Value != airport.Code.Value)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("名称[" + airport.Name + "]已存在");
            var sameShortNameAirport = QueryAirportByShortName(airport.ShortName);
            if(sameShortNameAirport != null && sameShortNameAirport.Code.Value != airport.Code.Value)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("简称[" + airport.ShortName + "]已存在");
            var originalContent = originalAirport.ToString();
            AirportCollection.Instance.Update(airport.Code, airport);
            saveUpdateLog("机场", originalContent, airport.ToString(), airport.Code.Value, account);
        }
        public static void UpdateAirline(AirlineView airlineView, string account) {
            var airline = Airline.GetAirline(airlineView);
            var originalAirline = QueryAirline(airline.Code);
            if(null == originalAirline)
                throw new ChinaPay.Core.CustomException("原航空公司不存在");
            var sameNameAirline = QueryAirlineByName(airline.Name);
            if(sameNameAirline != null && sameNameAirline.Code.Value != airline.Code.Value)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("名称[" + airline.Name + "]已存在");
            var sameShortNameAirline = QueryAirlineByShortName(airline.ShortName);
            if(sameShortNameAirline != null && sameShortNameAirline.Code.Value != airline.Code.Value)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("简称[" + airline.ShortName + "]已存在");
            var originalContent = originalAirline.ToString();
            AirlineCollection.Instance.Update(airline.Code, airline);
            saveUpdateLog("航空公司", originalContent, airline.ToString(), airline.Code.Value, account);
        }
        public static void UpdateAirCraft(Guid id, AirCraftView airCraftView, string account) {
            var airCraft = AirCraft.GetAirCraft(id, airCraftView);
            var originalAirCraft = QueryAirCraft(id);
            if(null == originalAirCraft)
                throw new ChinaPay.Core.CustomException("原机型不存在");
            var sameCodeAirCraft = QueryAirCraft(airCraft.Code);
            if(sameCodeAirCraft != null && sameCodeAirCraft.Id != id)
                throw new ChinaPay.Core.Exception.KeyRepeatedException("机型[" + airCraft.Code.Value + "]已存在");
            var originalContent = originalAirCraft.ToString();
            AirCraftCollection.Instance.Update(airCraft.Id, airCraft);
            saveUpdateLog("机型", originalContent, airCraft.ToString(), airCraft.Id.ToString(), account);
        }
        public static void UpdateBAF(Guid id, BAFView bafView, string account) {
            var baf = BAF.GetBAF(id, bafView);
            var originalBAF = QueryBAF(id);
            if(null == originalBAF)
                throw new ChinaPay.Core.CustomException("原燃油附加税不存在");
            var originalContent = originalBAF.ToString();
            BAFCollection.Instance.Update(baf.Id, baf);
            saveUpdateLog("燃油附加税", originalContent, baf.ToString(), baf.Id.ToString(), account);
        }
        public static void UpdateBasicPrice(Guid id, BasicPriceView basicPriceView, string account) {
            var originalBasicPrice = QueryBasicPrice(id);
            if(null == originalBasicPrice)
                throw new ChinaPay.Core.CustomException("原基础运价不存在");
            var originalContent = originalBasicPrice.ToString();
            basicPriceView.ModifyTime = DateTime.Now;
            var basicPrice = BasicPrice.GetBasicPrice(id, basicPriceView);
            BasicPriceCollection.Instance.Update(basicPrice.Id, basicPrice);
            saveUpdateLog("基础运价", originalContent, basicPrice.ToString(), basicPrice.Id.ToString(), account);
        }
        public static void UpdateBunk(Guid id, BunkView bunkView, string account) {
            var originalBunk = QueryBunkNew(id);
            if(null == originalBunk)
                throw new ChinaPay.Core.CustomException("原舱位不存在");
            var originalContent = originalBunk.ToString();
            var bunk = Bunk.CreateBunk(id, bunkView);
            BunkCollection.Instance.Update(bunk.Id, bunk);
            saveUpdateLog("舱位", originalContent, bunk.ToString(), bunk.Id.ToString(), account);
        }
        public static void UpdateChildrenOrderableBunk(Guid id, ChildOrderableBunkView childOrderableBunkView, string account) {
            var childOrderableBunk = ChildOrderableBunk.GetChildOrderableBunk(id, childOrderableBunkView);
            var originalChildOrderableBunk = QueryChildOrderableBunk(id);
            if(null == originalChildOrderableBunk)
                throw new ChinaPay.Core.CustomException("原儿童可预订舱位不存在");
            var sameBunk = QueryChildOrderableBunk(childOrderableBunk.AirlineCode, childOrderableBunk.BunkCode);
            if(sameBunk != null && sameBunk.Id != id)
                throw new ChinaPay.Core.Exception.KeyRepeatedException(string.Format("航空公司[{0}]的舱位[{1}]已存在", childOrderableBunk.AirlineCode.Value, childOrderableBunk.BunkCode.Value));
            var originalContent = originalChildOrderableBunk.ToString();
            ChildOrderableBunkCollection.Instance.Update(childOrderableBunk.Id, childOrderableBunk);
            saveUpdateLog("儿童可预订舱位", originalContent, childOrderableBunk.ToString(), childOrderableBunk.Id.ToString(), account);
        }
        public static void UpdateRefundAndRescheduling(RefundAndReschedulingView refundAndReschedulingView, string account) {
            var refundAndRescheduling = RefundAndRescheduling.GetRefundAndRescheduling(refundAndReschedulingView);
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingRepository();
            repository.Update(refundAndRescheduling);
            saveLog(OperationType.Update, "修改退改签客规", refundAndRescheduling.AirlineCode.Value, account);
        }
        public static void UpdateRefundAndReschedulingNewBase(RefundAndReschedulingBaseView refundAndReschedulingView, string account) {
            var refundAndRescheduling = RefundAndReschedulingBase.GetRefundAndRescheduling(refundAndReschedulingView);
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
            repository.Update(refundAndRescheduling);
            saveLog(OperationType.Update, "修改退改签客规基础信息", refundAndRescheduling.AirlineCode.Value, account);
        }
        public static void UpdateRefundAndReschedulingNewDetail(RefundAndReschedulingDetail detail, string account) {
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
            repository.Update(detail);
            saveLog(OperationType.Update, "修改退改签客规详细信息", detail.Id.ToString(), account);
        }
        public static void UpdateCheck_In(Check_In check_in, string account)
        {
            ChinaPay.B3B.Service.Foundation.Domain.Check_InCellection.Instance.Update(check_in.Id,check_in);
            saveLog(OperationType.Update, "修改值机详细信息", check_in.Id.ToString(), account);
        }
        #endregion
        #region "删除"
        public static void DeleteArea(string area, string account) {
            if(string.IsNullOrWhiteSpace(area))
                throw new ArgumentNullException("area");
            ProvinceCollection.Instance.Remove(area);
            saveDeleteLog("区域", area, area, account);
        }
        public static void DeleteProvince(string province, string account) {
            if(string.IsNullOrWhiteSpace(province))
                throw new ArgumentNullException("province");
            ProvinceCollection.Instance.Remove(province);
            saveDeleteLog("省份", province, province, account);
        }
        public static void DeleteCity(string city, string account) {
            if(string.IsNullOrWhiteSpace(city))
                throw new ArgumentNullException("city");
            var model = CityCollection.Instance.Remove(city);
            saveDeleteLog("市", model == null ? city : model.ToString(), city, account);
        }
        public static void DeleteCounty(string county, string account) {
            if(string.IsNullOrWhiteSpace(county))
                throw new ArgumentNullException("county");
            var model = CountyCollection.Instance.Remove(county);
            saveDeleteLog("县", model == null ? county : model.ToString(), county, account);
        }
        public static void DeleteAirport(string airport, string account) {
            if(string.IsNullOrWhiteSpace(airport))
                throw new ArgumentNullException("airport");
            var model = AirportCollection.Instance.Remove(airport);
            saveDeleteLog("机场", model == null ? airport : model.ToString(), airport, account);
        }
        public static void DeleteAirline(string airline, string account) {
            if(string.IsNullOrWhiteSpace(airline))
                throw new ArgumentNullException("airline");
            var model = AirlineCollection.Instance.Remove(airline);
            saveDeleteLog("航空公司", model == null ? airline : model.ToString(), airline, account);
        }
        public static void DeleteAirCraft(Guid airCraft, string account) {
            var model = AirCraftCollection.Instance.Remove(airCraft);
            saveDeleteLog("机型", model == null ? airCraft.ToString() : model.ToString(), airCraft.ToString(), account);
        }
        public static void DeleteBAF(Guid baf, string account) {
            var model = BAFCollection.Instance.Remove(baf);
            saveDeleteLog("燃油附加税", model == null ? baf.ToString() : model.ToString(), baf.ToString(), account);
        }
        public static void DeleteBasicPrice(Guid basicPrice, string account) {
            var model = BasicPriceCollection.Instance.Remove(basicPrice);
            saveDeleteLog("基础运价", model == null ? basicPrice.ToString() : model.ToString(), basicPrice.ToString(), account);
        }
        public static void DeleteBunk(Guid bunk, string account) {
            var model = BunkCollection.Instance.Remove(bunk);
            saveDeleteLog("舱位", model == null ? bunk.ToString() : model.ToString(), bunk.ToString(), account);
        }
        public static void DeleteChildrenOrderableBunk(Guid childOrderableBunk, string account) {
            var model = ChildOrderableBunkCollection.Instance.Remove(childOrderableBunk);
            saveDeleteLog("儿童可预订舱位", model == null ? childOrderableBunk.ToString() : model.ToString(), childOrderableBunk.ToString(), account);
        }
        public static void DeleteRefundAndRescheduling(string airline, string account) {
            if(string.IsNullOrWhiteSpace(airline))
                throw new ArgumentNullException("airline");
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingRepository();
            repository.Delete(airline);
            saveLog(OperationType.Delete, "删除退改签客规", airline, account);
        }
        public static void DeleteRefundAndReschedulingNewBase(string airline, string account) {
            if(string.IsNullOrWhiteSpace(airline))
                throw new ArgumentNullException("airline");
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
            repository.Delete(airline);
            saveLog(OperationType.Delete, "删除退改签客规基础信息", airline, account);
        }
        public static void DeleteFixedNavigation(FixedNavigationView fixedNavigationView, string account) {
            Service.Foundation.Domain.FixedNavigations.Instance.Delete(fixedNavigationView);
            saveDeleteLog("非固定航线", fixedNavigationView.ToString(), fixedNavigationView.Departure + fixedNavigationView.Arrival, account);
        }
        public static void DeleteRefundAndReshedulingNewDetail(Guid detailId, string account) {
            if(string.IsNullOrWhiteSpace(detailId.ToString()))
                throw new ArgumentNullException("detailId");
            var repository = ChinaPay.B3B.Service.Foundation.Repository.Factory.CreateRefundAndReschedulingNewRepository();
            repository.Delete(detailId);
            saveLog(OperationType.Delete, "删除退改签客规详细信息", detailId.ToString(), account);
        }
        public static void DeleteCheck_In(Guid id,string account) {
            if (string.IsNullOrEmpty(id.ToString())) throw new ArgumentNullException("Id");
            ChinaPay.B3B.Service.Foundation.Domain.Check_InCellection.Instance.Remove(id);
            saveDeleteLog("值机", "删除值机", id.ToString(), account);
        }
        #endregion
        #region "日志"
        static void saveAddLog(string itemName, string content, string key, string account) {
            saveLog(OperationType.Insert, "添加" + itemName + "。" + content, key, account);
        }
        static void saveUpdateLog(string itemName, string originalContent, string newContent, string key, string account) {
            saveLog(OperationType.Update, string.Format("修改{0}。由 {1} 修改为 {2}", itemName, originalContent, newContent), key, account);
        }
        static void saveDeleteLog(string itemName, string content, string key, string account) {
            saveLog(OperationType.Delete, "删除" + itemName + "。" + content, key, account);
        }
        static void saveLog(OperationType operationType, string content, string key, string account) {
            var log = new Service.Log.Domain.OperationLog(OperationModule.基础数据, operationType, account, OperatorRole.Platform, key, content);
            Service.LogService.SaveOperationLog(log);
        }
        #endregion

        /// <summary>
        /// 计算票面价
        /// </summary>
        public static decimal CalculateFare(decimal standardFare, decimal discount) {
            return BasicPrice.CalcFare(standardFare, discount);
        }
        /// <summary>
        /// 计算折扣
        /// </summary>
        public static decimal CalculateDiscount(decimal standardFare, decimal fare) {
            return BasicPrice.CalcDiscount(standardFare, fare);
        }
    }
}