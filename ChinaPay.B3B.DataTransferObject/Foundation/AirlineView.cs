using System.Text.RegularExpressions;
namespace ChinaPay.B3B.DataTransferObject.Foundation {
    public class AirlineView {
        internal static string AirlineCodePattern = "^[a-z,A-Z,1-9]{2}$";
        internal static string SettleCodePattern = @"^\d{3}$";

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
        /// 简称
        /// </summary>
        public string ShortName {
            get;
            set;
        }
        /// <summary>
        /// 结算代码
        /// </summary>
        public string SettleCode {
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
            if(string.IsNullOrWhiteSpace(this.Name))
                throw new System.ArgumentNullException("Name");
            if(string.IsNullOrWhiteSpace(this.ShortName))
                throw new System.ArgumentNullException("ShortName");
            if(string.IsNullOrWhiteSpace(this.SettleCode))
                throw new System.ArgumentNullException("SettleCode");
            if(!Regex.IsMatch(this.Code.Trim(), AirlineCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("代码必须为2位字母或数字");
            if(this.Name.Trim().Length > 25)
                throw new ChinaPay.Core.Exception.InvalidValueException("名称不能超过25个字");
            if(this.ShortName.Trim().Length > 10)
                throw new ChinaPay.Core.Exception.InvalidValueException("简称不能超过10个字");
            if(!Regex.IsMatch(this.SettleCode.Trim(), SettleCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("结算代码必须为3位数字");
        }
    }
}
