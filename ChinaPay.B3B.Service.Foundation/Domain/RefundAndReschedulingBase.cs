using System;
using ChinaPay.Core;
using ChinaPay.B3B.DataTransferObject.Foundation;
using System.Collections;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Foundation.Domain
{
    /// <summary>
    /// 退改签规定
    /// </summary>
    public class RefundAndReschedulingBase
    {
        internal RefundAndReschedulingBase(UpperString airlineCode)
        {
            this.AirlineCode = airlineCode;
        }
        /// <summary>
        /// 航空公司代码
        /// </summary>
        public UpperString AirlineCode
        {
            get;
            private set;
        }
        /// <summary>
        /// 航空公司
        /// </summary>
        public Airline Airline
        {
            get
            {
                return FoundationService.QueryAirline(AirlineCode);
            }
        }
        /// <summary>
        /// 废票规定
        /// </summary>
        public string Scrap
        {
            get;
            internal set;
        }
        /// <summary>
        /// 升舱规定
        /// </summary>
        public string Upgrade
        {
            get;
            internal set;
        }
        /// <summary>
        /// 适用条件
        /// </summary>
        public string Condition
        {
            get;
            internal set;
        }
        /// <summary>
        /// 航空公司电话
        /// </summary>
        public string AirlineTel
        {
            get;
            internal set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get;
            internal set;
        }
        /// <summary>
        /// 排序值
        /// </summary>
        public int Level
        {
            get;
            internal set;
        }
        /// <summary>
        /// 详细信息
        /// </summary>
        public IEnumerable<RefundAndReschedulingDetail> RefundAndReschedulingDetail
        {
            get;
            set;
        }

        internal static RefundAndReschedulingBase GetRefundAndRescheduling(RefundAndReschedulingBaseView refundAndReschedulingView)
        {
            if (null == refundAndReschedulingView)
                throw new ArgumentNullException("refundAndReschedulingView");
            refundAndReschedulingView.Validate();
            return new RefundAndReschedulingBase(ChinaPay.Utility.StringUtility.Trim(refundAndReschedulingView.Airline))
            {
                Condition = ChinaPay.Utility.StringUtility.Trim(refundAndReschedulingView.Condition),
                Upgrade = ChinaPay.Utility.StringUtility.Trim(refundAndReschedulingView.Upgrade),
                Scrap = ChinaPay.Utility.StringUtility.Trim(refundAndReschedulingView.Scrap),
                AirlineTel = ChinaPay.Utility.StringUtility.Trim(refundAndReschedulingView.AirlineTel),
                Remark = ChinaPay.Utility.StringUtility.Trim(refundAndReschedulingView.Remark),
                Level = refundAndReschedulingView.Level
            };
        }
    }
}