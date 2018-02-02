using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform
{
    internal class BalanceRefundStatusAdapter : StatusAdapter<BalanceRefundProcessStatus>
    {
        static object _locker = new object();
        static BalanceRefundStatusAdapter _instance = null;
        public static BalanceRefundStatusAdapter Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (_locker)
                    {
                        if (null == _instance)
                        {
                            _instance = new BalanceRefundStatusAdapter();
                        }
                    }
                }
                return _instance;
            }
        }

        private BalanceRefundStatusAdapter()
            : base()
        {
        }

        protected override IEnumerable<StatusInfo<BalanceRefundProcessStatus>> GetStatusInfos()
        {
            var repository = Repository.Factory.CreateStatusRepository();
            return repository.GetBalanceRefundApplyformStatusInfos();
        }
    }
}