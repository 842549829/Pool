using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Foundation;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    public class Area {
        internal Area(string code) {
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
        public IEnumerable<Province> Provinces {
            get {
                return from item in ProvinceCollection.Instance.Values
                       where item.AreaCode == this.Code
                       select item;
            }
        }

        public override string ToString() {
            return string.Format("代码:{0} 名称:{1}", Code, Name);
        }
        internal static Area GetArea(AreaView areaView) {
            if(null == areaView)
                throw new ArgumentNullException("provinceView");
            areaView.Validate();
            return new Area(areaView.Code.Trim()) {
                Name = ChinaPay.Utility.StringUtility.Trim(areaView.Name)
            };
        }
    }
}