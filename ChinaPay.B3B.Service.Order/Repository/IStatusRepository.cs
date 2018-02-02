using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain;

namespace ChinaPay.B3B.Service.Order.Repository {
    interface IStatusRepository {
        IEnumerable<StatusInfo<OrderStatus>> GetOrderStatusInfos();
        IEnumerable<StatusInfo<PostponeApplyformStatus>> GetPostponeApplyformStatusInfos();
        IEnumerable<StatusInfo<RefundApplyformStatus>> GetRefundApplyformStatusInfos();
        IEnumerable<StatusInfo<BalanceRefundProcessStatus>> GetBalanceRefundApplyformStatusInfos();
    }
}