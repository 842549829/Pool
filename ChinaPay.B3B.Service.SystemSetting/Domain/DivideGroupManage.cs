namespace ChinaPay.B3B.Service.SystemSetting.Domain
{
    using System;
    using System.Collections.Generic;
using ChinaPay.Data;
    public class DivideGroupManage
    {
        EnumerableLazyLoader<MemberManage> _memberLoader;
        public DivideGroupManage()
            :this(new Guid())  {
        }
        public DivideGroupManage(Guid id)
        {
            this.Id = id;

        }
        void initLazyLoaders()
        {
            _memberLoader = new EnumerableLazyLoader<MemberManage>(() =>
            {
                return OnLineCustomerService.QueryMembers(this.Id);
            });
        }
        public Guid Id
        {
            get;
            private set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public int SortLevel
        {
            get;
            set;
        }
        public IEnumerable<MemberManage> MemberManage
        {
            get;
            set;
        }
    }
}
