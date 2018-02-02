using System.Text.RegularExpressions;
namespace ChinaPay.B3B.DataTransferObject.Foundation {
    public class ChildOrderableBunkView {
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline {
            get;
            set;
        }
        /// <summary>
        /// 舱位
        /// </summary>
        public string Bunk {
            get;
            set;
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount {
            get;
            set;
        }

        public void Validate() {
            if(string.IsNullOrWhiteSpace(this.Airline))
                throw new System.ArgumentNullException("Airline");
            if(string.IsNullOrWhiteSpace(this.Bunk))
                throw new System.ArgumentNullException("Bunk");
            if(!Regex.IsMatch(this.Airline.Trim(), AirlineView.AirlineCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("航空公司代码格式错误");
            if(!Regex.IsMatch(this.Bunk.Trim(), BunkView.BunkCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("舱位代码格式错误");
            if(0 >= this.Discount || this.Discount > 9.999M)
                throw new ChinaPay.Core.Exception.InvalidValueException("折扣范围为(0, 9.999]");
        }
    }
}
