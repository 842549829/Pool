namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 乘运人
    /// </summary>
    public sealed class Carrier {
        string _name = null, _settleCode = null;
        internal Carrier(string code) {
            this.Code = code;
        }
        internal Carrier(string code, string name, string settleCode) {
            this.Code = code;
            this._name = name ?? string.Empty;
            this._settleCode = settleCode ?? string.Empty;
        }
        /// <summary>
        /// 二字码
        /// </summary>
        public string Code {
            get;
            private set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name {
            get {
                if (null == _name) {
                    initFoundationDatas();
                }
                return _name;
            }
        }

        /// <summary>
        /// 结算代码
        /// </summary>
        public string SettleCode {
            get {
                if (null == _settleCode) {
                    initFoundationDatas();
                }
                return _settleCode;
            }
        }
        private void initFoundationDatas() {
            var airline = FoundationService.QueryAirline(this.Code);
            if(airline != null) {
                this._name = airline.ShortName;
                this._settleCode = airline.SettleCode;
            }
        }

        internal static bool Equals(Carrier first, Carrier second) {
            if(first == null || second == null)
                return false;
            return first.Code == second.Code;
        }
    }
}