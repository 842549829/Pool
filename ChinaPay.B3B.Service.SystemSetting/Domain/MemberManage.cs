namespace ChinaPay.B3B.Service.SystemSetting.Domain
{
    using System;
    using System.Collections.Generic;
    public class MemberManage
    {
        public MemberManage()
            :this(Guid.NewGuid()){
        }
        public MemberManage(Guid id)
        {
            this.Id = id;
        }
        public Guid Id
        {
            get;
            private set;
        }
        /// <summary>
        /// 成员说明
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 排序字段
        /// </summary>
        public int SortLevel { get; set; }
        public IEnumerable<string> QQ
        {
            get;
            set;
        }
    }
}
