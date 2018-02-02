using System.Text.RegularExpressions;

namespace ChinaPay.B3B.DataTransferObject.Foundation {
    public class RefundAndReschedulingView {
        /// <summary>
        /// 航空公司代码
        /// </summary>
        public string Airline {
            get;
            set;
        }
        /// <summary>
        /// 退票规定
        /// </summary>
        public string Refund {
            get;
            set;
        }
        /// <summary>
        /// 废票规定
        /// </summary>
        public string Scrap {
            get;
            set;
        }
        /// <summary>
        /// 更改规定
        /// </summary>
        public string Change {
            get;
            set;
        }
        /// <summary>
        /// 航空公司电话
        /// </summary>
        public string AirlineTel {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark {
            get;
            set;
        }
        /// <summary>
        /// 排序值
        /// </summary>
        public int Level {
            get;
            set;
        }

        public void Validate() {
            if(string.IsNullOrWhiteSpace(this.Airline))
                throw new System.ArgumentNullException("Airline");
            if(!Regex.IsMatch(this.Airline.Trim(), AirlineView.AirlineCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("航空公司代码格式错误");
            if(!string.IsNullOrWhiteSpace(this.Refund) && this.Refund.Trim().Length > 2000)
                throw new ChinaPay.Core.Exception.InvalidValueException("退票规定不能超过2000个字");
            if(!string.IsNullOrWhiteSpace(this.Scrap) && this.Scrap.Trim().Length > 2000)
                throw new ChinaPay.Core.Exception.InvalidValueException("废票规定不能超过2000个字");
            if(!string.IsNullOrWhiteSpace(this.Change) && this.Change.Trim().Length > 2000)
                throw new ChinaPay.Core.Exception.InvalidValueException("更改规定不能超过2000个字");
            if(!string.IsNullOrWhiteSpace(this.AirlineTel) && this.AirlineTel.Trim().Length > 100)
                throw new ChinaPay.Core.Exception.InvalidValueException("航空公司电话不能超过100个字符");
            if(!string.IsNullOrWhiteSpace(this.Remark) && this.Remark.Trim().Length > 2000)
                throw new ChinaPay.Core.Exception.InvalidValueException("备注不能超过2000个字");
        }
    }
}
