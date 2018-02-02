using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.Utility;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    /// <summary>
    /// 市
    /// </summary>
    public class City : Location {
        internal City(string code)
            : base(code) {
        }
        /// <summary>
        /// 所属省份代码
        /// </summary>
        public string ProvinceCode {
            get;
            internal set;
        }
        /// <summary>
        /// 所属省份
        /// </summary>
        public Province Province {
            get {
                return FoundationService.QueryProvice(ProvinceCode);
            }
        }
        public IEnumerable<County> Counties {
            get {
                return from item in CountyCollection.Instance.Values
                       where item.CityCode == this.Code
                       select item;
            }
        }
        public override string ToString() {
            return string.Format("{0} 所属省份:{1}", base.ToString(), ProvinceCode);
        }
        internal static City GetCity(CityView cityView) {
            if(null == cityView)
                throw new ArgumentNullException("cityView");
            cityView.Validate();
            return new City(cityView.Code.Trim()) {
                ProvinceCode = StringUtility.Trim(cityView.ProvinceCode),
                Name = StringUtility.Trim(cityView.Name),
                Spelling = StringUtility.Trim(cityView.Spelling),
                ShortSpelling = StringUtility.Trim(cityView.ShortSpelling),
                HotLevel = cityView.HotLevel
            };
        }
    }
}