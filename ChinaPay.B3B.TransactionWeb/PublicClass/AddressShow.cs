using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.DataTransferObject.Organization;
using System.Web.Script.Serialization;

namespace ChinaPay.B3B.TransactionWeb.PublicClass
{
    public class AddressShow
    {
        /// <summary>
        ///  显示详细地址信息
        /// </summary>
        public static string GetAddressText(string area,string province,string city,string county){
            string address = "";
            if (!string.IsNullOrWhiteSpace(area)){
                var areaObj = FoundationService.QueryArea(area);
                if(areaObj != null){
                address += areaObj.Name;
                }
            }
            if (!string.IsNullOrWhiteSpace(province)) {
                var provinceObj = FoundationService.QueryProvice(province);
                if(provinceObj != null){
                    address += provinceObj.Name;
                }
            }
            if (!string.IsNullOrWhiteSpace(city)) {
                var cityObj = FoundationService.QueryCity(city);
                if(cityObj != null){
                    address += cityObj.Name;
                }
            }
            if (!string.IsNullOrWhiteSpace(county)){
                var countyObj =  FoundationService.QueryCounty(county);
                if(countyObj != null){
                    address+= countyObj.Name;
                }
            }
            return address;
               
        }
        /// <summary>
        /// 输出json字符串的地址信息
        /// </summary>
        public static string GetAddressJson(string area, string province, string city, string county)
        {
            AddressInfo addressinfo = new AddressInfo();
                if (!string.IsNullOrWhiteSpace(area)) {
                    var areaObj = FoundationService.QueryArea(area);
                    if (areaObj != null) {
                        addressinfo.AreaCode = area;
                        addressinfo.AreaName = areaObj.Name;
                    }
                }
                if (!string.IsNullOrWhiteSpace(province)) {
                    var provinceObj = FoundationService.QueryProvice(province);
                    if (provinceObj != null)  {
                        addressinfo.ProvinceCode = province;
                        addressinfo.ProvinceName= provinceObj.Name;
                    }
                }
                if (!string.IsNullOrWhiteSpace(city)){
                    var cityObj = FoundationService.QueryCity(city);
                    if (cityObj != null) {
                        addressinfo.CityCode = city;
                        addressinfo.CityName= cityObj.Name;
                    }
                }
                if (!string.IsNullOrWhiteSpace(county)) {
                    var countyObj = FoundationService.QueryCounty(county);
                    if (countyObj != null)
                    {
                        addressinfo.CountyCode = county;
                        addressinfo.CountyName= countyObj.Name;
                    }
                }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(addressinfo);
        }
        /// <summary>
        /// 根据名称获取代码
        /// </summary>
        public static AddressInfo GetAddressInfo(string address) 
        {
            string[] strAddress = address.Split('|');
            return new AddressInfo
            {
                AreaName = strAddress[0],
                AreaCode = FoundationService.QueryAreaByName(strAddress[0]).Code,
                ProvinceName = strAddress[1],
                ProvinceCode = FoundationService.QueryProvinceByName(strAddress[1]).Code,
                CityName = strAddress[2],
                CityCode = FoundationService.QueryCityByName(strAddress[2]).Code,
                CountyCode =FoundationService.QueryCountyByName(strAddress[3]).Code,
                CountyName = strAddress[3]
            };
        }
        public static AddressInfo GetAddressBaseInfo(string address) 
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<AddressInfo>(address);
        }
        /// <summary>
        /// 根据代码查询省
        /// </summary>
        public static string GetProvince(string provinceCode)
        {
            var province = FoundationService.QueryProvice(provinceCode);
            return province != null ? province.Name : string.Empty;
        }
        /// <summary>
        /// 根据代码获取城市
        /// </summary>
        public static string GetCity(string cityCode) {
            var city = FoundationService.QueryCity(cityCode);
            return city != null ? city.Name : string.Empty;
        }
        /// <summary>
        /// 根据代码查询县城
        /// </summary>
        public static string GetCounty(string countyCode) {
            var county = FoundationService.QueryCounty(countyCode);
            return county != null ? county.Name : string.Empty;
        }
    }
}