using System.Text.RegularExpressions;

namespace ChinaPay.B3B.DataTransferObject.Foundation {
    public class CityView {
        internal const string CityCodePattern = @"^\d{1,6}$";

        /// <summary>
        /// 所属省份代码
        /// </summary>
        public string ProvinceCode {
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
        /// <summary>
        /// 全拼
        /// </summary>
        public string Spelling {
            get;
            set;
        }
        /// <summary>
        /// 简拼
        /// </summary>
        public string ShortSpelling {
            get;
            set;
        }
        /// <summary>
        /// 热点级别
        /// 默认为0
        /// </summary>
        public int HotLevel {
            get;
            set;
        }

        public void Validate() {
            if(string.IsNullOrWhiteSpace(this.Code))
                throw new System.ArgumentNullException("Code");
            if(string.IsNullOrWhiteSpace(this.Name))
                throw new System.ArgumentNullException("Name");
            if(string.IsNullOrWhiteSpace(this.ProvinceCode))
                throw new System.ArgumentNullException("ProvinceCode");
            if(!Regex.IsMatch(this.Code.Trim(), CityCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("代码必须为1到6位数字");
            if(this.Name.Trim().Length > 20)
                throw new ChinaPay.Core.Exception.InvalidValueException("名称不能超过20个字");
            if(!Regex.IsMatch(this.ProvinceCode.Trim(), ProvinceView.ProvinceCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("所属省份代码格式错误");
        }
    }
}
