using System.Text.RegularExpressions;

namespace ChinaPay.B3B.DataTransferObject.Foundation {
    public class ProvinceView {
        internal const string ProvinceCodePattern = @"^\d{1,6}$";

        /// <summary>
        /// 所属区域代码
        /// </summary>
        public string AreaCode {
            get;
            set;
        }
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
        public void Validate() {
            if(string.IsNullOrWhiteSpace(this.Code))
                throw new System.ArgumentNullException("Code");
            if(string.IsNullOrWhiteSpace(this.Name))
                throw new System.ArgumentNullException("Name");
            if(string.IsNullOrWhiteSpace(this.AreaCode))
                throw new System.ArgumentNullException("AreaCode");
            if(!Regex.IsMatch(this.Code.Trim(), ProvinceCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("代码必须为1到6位数字");
            if(this.Name.Trim().Length > 20)
                throw new ChinaPay.Core.Exception.InvalidValueException("名称不能超过20个字");
            if(!Regex.IsMatch(this.AreaCode.Trim(), AreaView.AreaCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("所属区域代码格式错误");
        }
    }
}
