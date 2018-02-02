using System.Text.RegularExpressions;

namespace ChinaPay.B3B.DataTransferObject.Foundation {
    public class AreaView {
        internal const string AreaCodePattern = @"^\d{1,6}$";

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
            if(!Regex.IsMatch(this.Code.Trim(), AreaCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("代码必须为1到6位数字");
            if(this.Name.Trim().Length > 20)
                throw new ChinaPay.Core.Exception.InvalidValueException("名称不能超过20个字");
        }
    }
}