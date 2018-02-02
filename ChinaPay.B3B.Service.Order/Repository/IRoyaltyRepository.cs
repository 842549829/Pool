using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Order.Repository {
    internal interface IRoyaltyRepository {
        void Save(RoyaltyFailedRecord record);
        void Delete(decimal orderId);
        IEnumerable<RoyaltyFailedRecord> Query(RoyaltyFailedRecordQueryCondition condition, Pagination pagination);
        IEnumerable<decimal> Query();
    }
}