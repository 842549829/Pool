using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Foundation;
using System;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    /// <summary>
    /// 省份
    /// </summary>
    public class Province {
        internal Province(string code) {
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
        /// 所属区域代码
        /// </summary>
        public string AreaCode {
            get;
            internal set;
        }
        /// <summary>
        /// 所属区域
        /// </summary>
        public Area Area {
            get {
                return FoundationService.QueryArea(this.AreaCode);
            }
        }
        public IEnumerable<City> Cities {
            get {
                return from item in CityCollection.Instance.Values
                       where item.ProvinceCode == this.Code
                       select item;
            }
        }
        public override string ToString() {
            return string.Format("代码:{0} 名称:{1}", Code, Name);
        }

        internal static Province GetProvince(ProvinceView provinceView) {
            if(null == provinceView)
                throw new ArgumentNullException("provinceView");
            provinceView.Validate();
            return new Province(provinceView.Code.Trim()) {
                Name = ChinaPay.Utility.StringUtility.Trim(provinceView.Name),
                AreaCode = provinceView.AreaCode
            };
        }
    }
}