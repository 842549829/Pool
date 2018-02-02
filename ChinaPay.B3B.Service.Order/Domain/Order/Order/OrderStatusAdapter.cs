using System;
using ChinaPay.B3B.DataTransferObject.Order;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Order.Repository;

namespace ChinaPay.B3B.Service.Order.Domain {
    internal class OrderStatusAdapter : StatusAdapter<OrderStatus> {
        static object _locker = new object();
        static OrderStatusAdapter _instance = null;
        public static OrderStatusAdapter Instance {
            get {
                if(null == _instance) {
                    lock(_locker) {
                        if(null == _instance) {
                            _instance = new OrderStatusAdapter();
                        }
                    }
                }
                return _instance;
            }
        }
        
        private OrderStatusAdapter()
            : base() {
        }

        protected override IEnumerable<StatusInfo<OrderStatus>> GetStatusInfos() {
            var repository = Repository.Factory.CreateStatusRepository();
            return repository.GetOrderStatusInfos();
        }

        public static int GetOrderTodoCount(OrderStatus orderstatus,Guid employeeId, Guid providerId) {
            var repository = Repository.Factory.CreateOrderRepository(Factory.CreateCommand());
            return repository.GetOrderTodoCount(orderstatus, employeeId, providerId);
        }
    }
}