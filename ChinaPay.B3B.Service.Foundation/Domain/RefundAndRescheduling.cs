using System;
using ChinaPay.Core;
using ChinaPay.B3B.DataTransferObject.Foundation;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    /// <summary>
    /// 退改签规定
    /// </summary>
    public class RefundAndRescheduling {
        internal RefundAndRescheduling(UpperString airlineCode) {
            this.AirlineCode = airlineCode;
        }
        /// <summary>
        /// 航空公司代码
        /// </summary>
        public UpperString AirlineCode {
            get;
            private set;
        }
        /// <summary>
        /// 航空公司
        /// </summary>
        public Airline Airline {
            get {
                return FoundationService.QueryAirline(AirlineCode);
            }
        }
        /// <summary>
        /// 退票规定
        /// </summary>
        public string Refund {
            get;
            internal set;
        }
        /// <summary>
        /// 废票规定
        /// </summary>
        public string Scrap {
            get;
            internal set;
        }
        /// <summary>
        /// 更改规定
        /// </summary>
        public string Change {
            get;
            internal set;
        }
        /// <summary>
        /// 航空公司电话
        /// </summary>
        public string AirlineTel {
            get;
            internal set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark {
            get;
            internal set;
        }
        /// <summary>
        /// 排序值
        /// </summary>
        public int Level {
            get;
            internal set;
        }

        internal static RefundAndRescheduling GetRefundAndRescheduling(RefundAndReschedulingView refundAndReschedulingView) {
            if(null == refundAndReschedulingView)
                throw new ArgumentNullException("refundAndReschedulingView");
            refundAndReschedulingView.Validate();
            return new RefundAndRescheduling(ChinaPay.Utility.StringUtility.Trim(refundAndReschedulingView.Airline)) {
                Refund = ChinaPay.Utility.StringUtility.Trim(refundAndReschedulingView.Refund),
                Scrap = ChinaPay.Utility.StringUtility.Trim(refundAndReschedulingView.Scrap),
                Change = ChinaPay.Utility.StringUtility.Trim(refundAndReschedulingView.Change),
                AirlineTel = ChinaPay.Utility.StringUtility.Trim(refundAndReschedulingView.AirlineTel),
                Remark = ChinaPay.Utility.StringUtility.Trim(refundAndReschedulingView.Remark),
                Level = refundAndReschedulingView.Level
            };
        }
    }
}