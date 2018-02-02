using System;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    /// <summary>
    /// 机场
    /// </summary>
    public class Airport {
        internal Airport(UpperString code) {
            this.Code = code;
        }
        /// <summary>
        /// 代码
        /// </summary>
        public UpperString Code {
            get;
            private set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name {
            get;
            internal set;
        }
        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName {
            get;
            internal set;
        }
        /// <summary>
        /// 是否主机场
        /// 仅针对多机场时有效
        /// </summary>
        public bool IsMain {
            get;
            internal set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Valid {
            get;
            internal set;
        }
        /// <summary>
        /// 所在地代码
        /// </summary>
        public string LocationCode {
            get;
            internal set;
        }
        /// <summary>
        /// 所在地级别
        /// </summary>
        public LocationLevel LocationLevel {
            get;
            internal set;
        }
        /// <summary>
        /// 所在地
        /// </summary>
        public Location Location {
            get {
                if(this.LocationLevel == DataTransferObject.Foundation.LocationLevel.City) {
                    return FoundationService.QueryCity(this.LocationCode);
                } else if(this.LocationLevel == DataTransferObject.Foundation.LocationLevel.County) {
                    return FoundationService.QueryCounty(this.LocationCode);
                }
                return null;
            }
        }

        public override string ToString() {
            return string.Format("代码:{0} 名称:{1} 简称:{2} 状态:{3} 所在地代码:{4} 所在地级别:{5}", Code.Value, Name, ShortName, Valid, LocationCode, LocationLevel);
        }

        internal static Airport GetAirport(AirportView airportView) {
            if(null == airportView)
                throw new ArgumentNullException("airportView");
            airportView.Validate();
            return new Airport(airportView.Code.Trim()) {
                Name = ChinaPay.Utility.StringUtility.Trim(airportView.Name),
                ShortName = ChinaPay.Utility.StringUtility.Trim(airportView.ShortName),
                Valid = airportView.Valid,
                LocationCode = ChinaPay.Utility.StringUtility.Trim(airportView.LocationCode),
                LocationLevel = airportView.LocationLevel,
                IsMain = airportView.IsMain
            };
        }
    }
}
