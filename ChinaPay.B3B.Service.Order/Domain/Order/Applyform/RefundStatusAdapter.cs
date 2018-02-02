using ChinaPay.B3B.DataTransferObject.Order;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform {
    internal class RefundStatusAdapter : StatusAdapter<RefundApplyformStatus> {
        static object _locker = new object();
        static RefundStatusAdapter _instance = null;
        public static RefundStatusAdapter Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new RefundStatusAdapter();
                        }
                    }
                }
                return _instance;
            }
        }

        private RefundStatusAdapter()
            : base() {
        }

        protected override IEnumerable<StatusInfo<RefundApplyformStatus>> GetStatusInfos() {
            var repository = Repository.Factory.CreateStatusRepository();
            return repository.GetRefundApplyformStatusInfos();
        }
    }
}

