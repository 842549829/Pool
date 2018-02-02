using System;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.DataTransferObject.Foundation {
    public class BAFView {
        /// <summary>
        /// 航空公司代码
        /// </summary>
        public string Airline {
            get;
            set;
        }
        /// <summary>
        /// 里程
        /// </summary>
        public decimal Mileage {
            get;
            set;
        }
        /// <summary>
        /// 成人
        /// </summary>
        public decimal Adult {
            get;
            set;
        }
        /// <summary>
        /// 儿童
        /// </summary>
        public decimal Child {
            get;
            set;
        }
        /// <summary>
        /// 生效日期
        /// </summary>
        public DateTime EffectiveDate {
            get;
            set;
        }
        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime? ExpiredDate {
            get;
            set;
        }

        public void Validate() {
            if(!string.IsNullOrWhiteSpace(this.Airline) && !Regex.IsMatch(this.Airline.Trim(), AirlineView.AirlineCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("航空公司代码错误");
            if(this.ExpiredDate.HasValue && this.ExpiredDate.Value.Date < this.EffectiveDate.Date)
                throw new ChinaPay.Core.Exception.InvalidValueException("失效日期不能小于生效日期");
        }
    }
}
