using System;

namespace ChinaPay.B3B.DataTransferObject.Common {
    /// <summary>
    /// 编码对信息
    /// </summary>
    public sealed class PNRPair {
        public PNRPair(string pnr, string bpnr) {
            PNR = Utility.StringUtility.Trim(pnr);
            BPNR = Utility.StringUtility.Trim(bpnr);
            if (!string.IsNullOrEmpty(PNR))
            {
                PNR = PNR.ToUpper();
            }
            if (!string.IsNullOrEmpty(BPNR))
            {
                BPNR = BPNR.ToUpper();
            }
        }
        public PNRPair() {
        }

        /// <summary>
        /// 小编码
        /// </summary>
        public string PNR {
            get;
            set;
        }
        /// <summary>
        /// 大编码
        /// </summary>
        public string BPNR {
            get;
            set;
        }

        public bool Equals(PNRPair other) {
            if(other != null) {
                return PNRPair.Equals(this.BPNR, other.BPNR) || PNRPair.Equals(this.PNR, other.PNR);
            }
            return false;
        }
        public override bool Equals(object obj) {
            if(obj != null && obj is PNRPair) {
                return this.Equals(obj as PNRPair);
            }
            return false;
        }
        public override int GetHashCode() {
            return string.Format("{0}|{1}", this.PNR, this.BPNR).GetHashCode();
        }
        public override string ToString() {
            var result = string.Empty;
            if(!string.IsNullOrWhiteSpace(this.PNR)) {
                result += "小编码:" + this.PNR;
            }
            if(!string.IsNullOrWhiteSpace(this.BPNR)) {
                result += "大编码:" + this.BPNR;
            }
            return result;
        }
        public string ToListString() {
            return ToListString("<br/>");
        }
        public string ToListString(string separator) {
            var result = string.Empty;
            if(!string.IsNullOrWhiteSpace(this.PNR)) {
                result += this.PNR + "(小)" + separator;
            }
            if(!string.IsNullOrWhiteSpace(this.BPNR)) {
                result += this.BPNR + "(大)";
            }
            return result.Trim(separator.ToCharArray());
        }

        public static bool IsNullOrEmpty(PNRPair value) {
            if(value != null) {
                return string.IsNullOrWhiteSpace(value.PNR) && string.IsNullOrWhiteSpace(value.BPNR);
            }
            return true;
        }
        public static bool Equals(PNRPair first, PNRPair second) {
            if(first == null || second == null)
                return false;
            return first.Equals(second);
        }
        public static bool Equals(string first, string second) {
            return !string.IsNullOrWhiteSpace(first) && !string.IsNullOrWhiteSpace(second) && string.Compare(first, second, true) == 0;
        }
    }
}