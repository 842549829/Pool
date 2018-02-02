using System;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.Utility;
namespace ChinaPay.B3B.Service.Foundation.Domain {
    /// <summary>
    /// 县
    /// </summary>
    public class County : Location {
        internal County(string code)
            : base(code) {
        }
        /// <summary>
        /// 所属市代码
        /// </summary>
        public string CityCode {
            get;
            internal set;
        }
        /// <summary>
        /// 所属市
        /// </summary>
        public City City {
            get {
                return FoundationService.QueryCity(CityCode);
            }
        }
        public override string ToString() {
            return string.Format("{0} 所属市:{1}", base.ToString(), CityCode);
        }
        internal static County GetCounty(CountyView countyView) {
            if(null == countyView)
                throw new ArgumentNullException("countyView");
            countyView.Validate();
            return new County(countyView.Code.Trim()) {
                CityCode = StringUtility.Trim(countyView.CityCode),
                Name = StringUtility.Trim(countyView.Name),
                Spelling = StringUtility.Trim(countyView.Spelling),
                ShortSpelling = StringUtility.Trim(countyView.ShortSpelling),
                HotLevel = countyView.HotLevel
            };
        }
    }
}
