
using System.Text.RegularExpressions;
namespace ChinaPay.B3B.DataTransferObject.Foundation {
    public class AirCraftView {
        internal const string AirCraftCodePattern = "^[a-z,A-Z,0-9]{1,5}$";

        /// <summary>
        /// 代码
        /// </summary>
        public string Code {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name {
            get;
            set;
        }
        /// <summary>
        /// 机场建设费
        /// </summary>
        public decimal AirportFee {
            get;
            set;
        }
        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer {
            get;
            set;
        }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Valid {
            get;
            set;
        }

        public void Validate() {
            if(string.IsNullOrWhiteSpace(this.Code))
                throw new System.ArgumentNullException("Code");
            if(!Regex.IsMatch(this.Code.Trim(), AirCraftCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("代码只能是最多5个数字或字母");
            if(!string.IsNullOrWhiteSpace(this.Name) && this.Name.Trim().Length > 25)
                throw new ChinaPay.Core.Exception.InvalidValueException("名称最多25个字");
            if(!string.IsNullOrWhiteSpace(this.Manufacturer) && this.Manufacturer.Trim().Length > 30)
                throw new ChinaPay.Core.Exception.InvalidValueException("制造商最多30个字");
            if(!string.IsNullOrWhiteSpace(this.Description) && this.Description.Trim().Length > 1000)
                throw new ChinaPay.Core.Exception.InvalidValueException("描述信息最多1000个字");
        }
    }
}