using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Policy;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 扣/贴点信息
    /// </summary>
    public class Deduction {
        /// <summary>
        /// 出票方
        /// </summary>
        public decimal Provider {
            get;
            internal set;
        }
        /// <summary>
        /// 供应方
        /// </summary>
        public decimal Supplier {
            get;
            internal set;
        }
        /// <summary>
        /// 采购方
        /// </summary>
        public decimal Purchaser {
            get;
            internal set;
        }
        /// <summary>
        /// 运营方
        /// </summary>
        public decimal Platform {
            get;
            internal set;
        }
        internal static Deduction Empty {
            get {
                return new Deduction() {
                    Purchaser = 0,
                    Provider = 0,
                    Platform = 0,
                    Supplier = 0
                };
            }
        }

        internal static Deduction GetDeduction(PolicyMatch.MatchedPolicy policy) {
            if(policy.PolicyType == PolicyType.Special&&((SpecialPolicyInfo)policy.OriginalPolicy).Type!=SpecialProductType.LowToHigh) { //Xie. 2013-03-07
                return Deduction.Empty;
            } else {
                return new Deduction() {
                    Provider = policy.Rebate,
                    Purchaser = policy.Commission,
                    Platform = policy.Deduction
                };
            }
        }
    }
}
