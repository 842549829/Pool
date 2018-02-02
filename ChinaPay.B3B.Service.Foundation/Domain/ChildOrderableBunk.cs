using System;
using ChinaPay.Core;
using ChinaPay.B3B.DataTransferObject.Foundation;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    /// <summary>
    /// 可预订儿童的舱位
    /// </summary>
    public class ChildOrderableBunk {
        internal ChildOrderableBunk()
            : this(Guid.NewGuid()) {
        }
        internal ChildOrderableBunk(Guid id) {
            this.Id = id;
        }
        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 航空公司代码
        /// </summary>
        public UpperString AirlineCode {
            get;
            internal set;
        }
        /// <summary>
        /// 航空公司
        /// </summary>
        public Airline Airline {
            get {
                return FoundationService.QueryAirline(AirlineCode);
            }
        }
        /// <summary>
        /// 舱位代码
        /// </summary>
        public UpperString BunkCode {
            get;
            internal set;
        }
        /// <summary>
        /// 原始舱位信息
        /// </summary>
        public Bunk Bunk {
            get;
            internal set;
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount {
            get;
            internal set;
        }
        public override string ToString() {
            return string.Format("航空公司:{0} 舱位:{1} 折扣{2}", AirlineCode.Value, BunkCode.Value, Discount);
        }

        internal static ChildOrderableBunk GetChildOrderableBunk(ChildOrderableBunkView childOrderableBunkView) {
            if(null == childOrderableBunkView)
                throw new ArgumentNullException("childOrderableBunkView");
            childOrderableBunkView.Validate();
            return new ChildOrderableBunk() {
                AirlineCode = ChinaPay.Utility.StringUtility.Trim(childOrderableBunkView.Airline),
                BunkCode = ChinaPay.Utility.StringUtility.Trim(childOrderableBunkView.Bunk),
                Discount = childOrderableBunkView.Discount
            };
        }
        internal static ChildOrderableBunk GetChildOrderableBunk(Guid id, ChildOrderableBunkView childOrderableBunkView) {
            var childOrderableBunk = GetChildOrderableBunk(childOrderableBunkView);
            childOrderableBunk.Id = id;
            return childOrderableBunk;
        }
    }
}