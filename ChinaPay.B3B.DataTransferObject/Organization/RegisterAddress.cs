namespace ChinaPay.B3B.DataTransferObject.Organization {
    /// <summary>
    /// 地址信息
    /// </summary>
    public class RegisterAddress {
        public RegisterAddress(string county, string location, string postCode) {
            this.County = county;
            this.Location = location;
            this.Postcode = postCode;
        }
        /// <summary>
        /// 县/县级市
        /// 地址中的第三级的代码
        /// </summary>
        public string County {
            get;
            private set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Location {
            get;
            private set;
        }

        /// <summary>
        /// 邮编
        /// </summary>
        public string Postcode {
            get;
            private set;
        }
    }
}