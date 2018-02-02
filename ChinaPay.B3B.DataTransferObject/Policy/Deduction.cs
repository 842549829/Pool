namespace ChinaPay.B3B.DataTransferObject.Policy {
    /// <summary>
    /// 返点信息
    /// </summary>
    public class Deduction {
        /// <summary>
        /// 内部
        /// </summary>
        public float Interior {
            get;
            set;
        }
        /// <summary>
        /// 下级
        /// </summary>
        public float Junior {
            get;
            set;
        }
        /// <summary>
        /// 同行
        /// </summary>
        public float Brother {
            get;
            set;
        }
    }
    public class GeneralPolicyDeduction : Deduction {
        /// <summary>
        /// VIP加点
        /// </summary>
        public float VIP {
            get;
            set;
        }
    }
}