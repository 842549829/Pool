namespace ChinaPay.B3B.Service.SystemSetting.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public class PurchaseLimitation
    {
        public PurchaseLimitation()
        {
            this.Id = Guid.NewGuid();
        }
        public PurchaseLimitation(Guid id)
        {
            this.Id = id;
        }
        public Guid Id
        {
            get;
            private set;
        }
        /// <summary>
        /// 航空公司集合
        /// </summary>
        public IEnumerable<string> Airlines { get; set; }
        /// <summary>
        /// 出发机场集合
        /// </summary>
        public IEnumerable<string> Departures { get; set; }
        /// <summary>
        /// 是否只能采购自己的政策
        /// 如果是，则需设置默认返点
        /// 用于未发布政策时
        /// </summary>
        public bool PurchaseMyPolicyOnlyForNonePolicy { get; set; }
        /// <summary>
        /// 默认返点
        /// 用于未发布政策时
        /// </summary>
        public decimal? DefaultRebateForNonePolicy { get; set; }
    }
}
