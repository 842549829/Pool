namespace ChinaPay.B3B.Service.FlightQuery.Domain {
    /// <summary>
    /// 机场信息
    /// </summary>
    public class Airport {
        public Airport(string code) {
            this.Code = code;
        }
        /// <summary>
        /// 代码
        /// </summary>
        public string Code {
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
        public string AbbrivateName {
            get;
            internal set;
        }
        /// <summary>
        /// 航站楼
        /// </summary>
        public string Terminal{
            get;
            internal set;
        }
        /// <summary>
        /// 所在地
        /// </summary>
        public string City {
            get;
            internal set;
        }
    }
}