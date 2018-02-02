namespace ChinaPay.B3B.Service.Policy.Domain{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 政策设置类
    /// </summary>
    public class SetPolicy
    {
        public SetPolicy(Guid companyId)
        {
            this.Company = companyId;
        }
        public Guid Company {
            get;
            private set;
        }
        /// <summary>
        /// 可发布特价产品条数
        /// </summary>
        public int PromotionCount
        {
            get;
            set;
        }
        ///// <summary>
        ///// 可发布特殊产品条数
        ///// </summary>
        //public int SpecialCount
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 可发布单程控位政策条数
        /// </summary>
        public int SinglenessCount { get; set; }

        /// <summary>
        /// 可发布散冲团政策条数
        /// </summary>
        public int DisperseCount { get; set; }
        /// <summary>
        /// 可发布免票政策条数
        /// </summary>
        public int CostFreeCount { get; set; }
        /// <summary>
        /// 可发布集体票政策条数
        /// </summary>
        public int BlocCount { get; set; }
        /// <summary>
        /// 可发布商旅卡政策条数
        /// </summary>
        public int BusinessCount { get; set; }
        /// <summary>
        /// 可发布低打高返政策条数
        /// </summary>
        public int LowToHighCount { get; set; }

        /// <summary>
        /// 可发布其他特殊政策条数
        /// </summary>
        public int OtherSpecialCount { get; set; }

        public IEnumerable<string> Departure
        {
            get;
            set;
        }
        public IEnumerable<string> Airlines
        {
            get;
            set;
        }
        public string Remark
        {
            get;
            set;
        }
    }
}
