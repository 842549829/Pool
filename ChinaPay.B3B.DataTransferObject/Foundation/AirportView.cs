using System.Text.RegularExpressions;
namespace ChinaPay.B3B.DataTransferObject.Foundation {
    public class AirportView {
        internal const string AirportCodePattern = "^[a-z,A-Z]{3}$";

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
        /// 状态
        /// </summary>
        public bool Valid {
            get;
            set;
        }
        /// <summary>
        /// 所在地代码
        /// </summary>
        public string LocationCode {
            get;
            set;
        }
        /// <summary>
        /// 所在地级别
        /// </summary>
        public LocationLevel LocationLevel {
            get;
            set;
        }
        /// <summary>
        /// 是否主机场
        /// </summary>
        public bool IsMain {
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
            if(string.IsNullOrWhiteSpace(this.LocationCode))
                throw new System.ArgumentNullException("LocationCode");
            if(!Regex.IsMatch(this.Code.Trim(), AirportCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("代码必须为3位字母");
            if(this.Name.Trim().Length > 25)
                throw new ChinaPay.Core.Exception.InvalidValueException("名称不能超过25个字");
            if(this.ShortName.Trim().Length > 10)
                throw new ChinaPay.Core.Exception.InvalidValueException("简称不能超过10个字");
            if(this.LocationLevel != Foundation.LocationLevel.City && this.LocationLevel != Foundation.LocationLevel.County)
                throw new ChinaPay.Core.Exception.InvalidValueException("LocationLevel");
        }
    }
}
