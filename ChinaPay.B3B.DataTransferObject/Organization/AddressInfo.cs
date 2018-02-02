using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Web.Script.Serialization;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
    /// <summary>
    /// 所在地
    /// </summary>
    public class AddressInfo
    {
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public static AddressInfo GetAddress(string strAddress)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Deserialize<AddressInfo>(strAddress);
        }
    }
}
