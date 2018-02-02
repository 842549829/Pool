using System.Collections.Generic;
using ChinaPay.B3B.Service.Order.Domain;

namespace ChinaPay.B3B.Service.Order.Repository {
    interface ICoordinationRepository {
        void Save(decimal orderId,Coordination coordination);
        void Save(decimal orderId, decimal applyformId, Coordination coordination);
        IEnumerable<Coordination> QueryByOrderId(decimal orderId);
        IEnumerable<Coordination> QueryByApplyformId(decimal applyformId);
        EmergentOrder QueryEmergentOrder(decimal id, ChinaPay.B3B.DataTransferObject.Order.OrderStatus status);
        void SvaeEmergentOrder(EmergentOrder emergentOrder);
        Dictionary<string,string> QueryAccountNames(IEnumerable<string> accounts);
        EmergentOrder QueryEmergentOrder(decimal id, OrderIdType orderIdType);
    }
}
