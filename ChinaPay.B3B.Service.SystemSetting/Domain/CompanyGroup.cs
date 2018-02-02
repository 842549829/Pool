namespace ChinaPay.B3B.Service.SystemSetting.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ChinaPay.B3B.DataTransferObject.SystemSetting.CompanyGroup;
    public class CompanyGroup
    {
        internal CompanyGroup()
        {
            this.Id = Guid.NewGuid();
         }
        internal CompanyGroup(Guid id)
        {
            this.Id = id;
        }
        public Guid Id
        {
            get;
            private set;
        }
        /// <summary>
        /// 组别名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 组别描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }
        /// <summary>
        /// 是否允许采购其他代理的政策
        /// </summary>
        public bool PurchaseMyPolicyOnly
        {
            get;
            set;
        }
        /// <summary>
        /// 操作者帐号
        /// </summary>
        public string  RegisterAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime RegisterTime
        {
            get;
            set;
        }
        public string UpdateAccount
        {
            get;
            set;
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? UpdateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid Company
        {
            get;
            set;
        }
        List<PurchaseLimitation> m_limitations = new List<PurchaseLimitation>();
        /// <summary>
        /// 限制信息集合
        /// </summary>
        public IEnumerable<PurchaseLimitation> Limitations { get { return  m_limitations.AsReadOnly(); } }
        public void AppendLimitaion(PurchaseLimitation item)
        {
            if (item == null) throw new ArgumentNullException("item");
            m_limitations.Add(item);
        }
    }
}
