using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Order.Repository {
    interface IFreezeRepository {
        void Save(FreezeInfo info);
        void Save(UnfreezeInfo info);
        void Update(Domain.Applyform.UnfreezeInfo info);
        FreezeInfo QueryFreezeInfo(decimal applyformId);
        UnfreezeInfo QueryUnfreezeInfo(string freezeNo);
        IEnumerable<FreezeBaseInfo> Query(FreezeQueryCondition condition, Pagination pagination);
    }
}