using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Order.Repository {
    interface IApplyformRepository {
        void Insert(Domain.Applyform.BaseApplyform applyform);
        void Update(Domain.Applyform.BaseApplyform applyform);
        void UpdateApplyformForRevicePrice(Domain.Applyform.BaseApplyform applyform);
        Domain.Applyform.BaseApplyform QueryApplyform(decimal applyformId);
        IEnumerable<Domain.Applyform.BaseApplyform> QueryApplyforms(decimal orderId);
        IEnumerable<ApplyformListView> QueryApplyforms(ApplyformQueryCondition condition, Pagination pagination);
        List<ApplyAttachmentView> QueryApplyAttachmentView(decimal applyformId);
        ApplyAttachmentView QueryApplyAttachmentView(System.Guid applyAttachmentId);
        IEnumerable<ApplyformListView> QueryApplyformsNew(ApplyformQueryCondition condition, Pagination pagination);
        void DeleteApplyAttachmentView(System.Guid applyAttachmentId);
        void AddApplyAttachmentView(List<ApplyAttachmentView> applyAttachmentViews);
        void InsertBalanceRefundApplyform(BalanceRefundApplyform balanceRefundApplyform);
        void UpdateFlag(decimal applyform);
        BalanceRefundApplyform QueryBalanceRefundApplyform(decimal applyformId);
        void UpdateFlightBalanceRefunfFee(decimal applyformId, IEnumerable<RefundFlight> flights);
        void SaveBalanceRefundProcessStatus(BalanceRefundApplyform applyform);
    }
}