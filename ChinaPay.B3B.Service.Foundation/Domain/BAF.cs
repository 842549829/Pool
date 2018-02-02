using System;
using ChinaPay.Core;
using ChinaPay.B3B.DataTransferObject.Foundation;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    /// <summary>
    /// 燃油附加税
    /// </summary>
    public class BAF {
        internal BAF()
            : this(Guid.NewGuid()) {
        }
        internal BAF(Guid id) {
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
        /// 里程
        /// </summary>
        public decimal Mileage {
            get;
            internal set;
        }
        /// <summary>
        /// 成人
        /// </summary>
        public decimal Adult {
            get;
            internal set;
        }
        /// <summary>
        /// 儿童
        /// </summary>
        public decimal Child {
            get;
            internal set;
        }
        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime EffectiveDate {
            get;
            internal set;
        }
        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? ExpiredDate {
            get;
            internal set;
        }
        public override string ToString() {
            return string.Format("航空公司:{0} 里程:{1} 生效日期:{2} 失效日期:{3} 成人:{4} 儿童:{5}",
                AirlineCode.Value, Mileage, EffectiveDate.ToShortDateString(),
                ExpiredDate.HasValue ? ExpiredDate.Value.ToShortDateString() : string.Empty, Adult, Child);
        }

        internal static BAF GetBAF(BAFView bafView) {
            if(null == bafView)
                throw new ArgumentNullException("bafView");
            bafView.Validate();
            return new BAF() {
                AirlineCode = ChinaPay.Utility.StringUtility.Trim(bafView.Airline),
                Mileage = bafView.Mileage,
                Adult = bafView.Adult,
                Child = bafView.Child,
                EffectiveDate = bafView.EffectiveDate,
                ExpiredDate = bafView.ExpiredDate
            };
        }
        internal static BAF GetBAF(Guid id, BAFView bafView) {
            var baf = GetBAF(bafView);
            baf.Id = id;
            return baf;
        }
    }
}