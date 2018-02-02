using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Order.Repository {
    interface IRefundRepository {
        void Save(RefundFailedRecord record);
        void Delete(decimal applyformId);
        void Update(RefundFailedRecord record);
        IEnumerable<RefundFailedRecord> Query(RefundFailedRecordQueryCondition condition, Pagination pagination);
        IEnumerable<RefundFailedRecord> Query();
        RefundFailedRecord Query(decimal applyformId);
    }
}
