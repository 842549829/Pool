using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 机场信息
    /// </summary>
    public sealed class Airport {
        internal Airport(string code) {
            this.Code = code;
            var airportModel = FoundationService.QueryAirport(code);
            if(airportModel == null)
                throw new CustomException("机场不存在");
            this.Name = airportModel.ShortName;
            this.City = airportModel.Location == null ? string.Empty : airportModel.Location.Name;
        }
        internal Airport(string code, string name, string city) {
            this.Code = code;
            this.Name = name;
            this.City = city;
        }
        /// <summary>
        /// 三字码
        /// </summary>
        public string Code {
            get;
            private set;
        }
        /// <summary>
        /// 机场名称
        /// </summary>
        public string Name {
            get;
            private set;
        }
        /// <summary>
        /// 城市名
        /// </summary>
        public string City {
            get;
            private set;
        }
    }
}
