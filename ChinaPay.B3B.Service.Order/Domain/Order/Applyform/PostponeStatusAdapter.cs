using ChinaPay.B3B.DataTransferObject.Order;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform {
    internal class PostponeStatusAdapter : StatusAdapter<PostponeApplyformStatus> {
        static object _locker = new object();
        static PostponeStatusAdapter _instance = null;
        public static PostponeStatusAdapter Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new PostponeStatusAdapter();
                        }
                    }
                }
                return _instance;
            }
        }

        private PostponeStatusAdapter()
            : base() {
        }

        protected override IEnumerable<StatusInfo<PostponeApplyformStatus>> GetStatusInfos() {
            var repository = Repository.Factory.CreateStatusRepository();
            return repository.GetPostponeApplyformStatusInfos();
        }
    }
}