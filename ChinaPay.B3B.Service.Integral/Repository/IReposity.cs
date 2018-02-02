using System;

namespace ChinaPay.B3B.Service.Integral.Repository
{
    using System.Collections.Generic;
    using ChinaPay.B3B.DataTransferObject.Integral;
    using ChinaPay.Core;
    using ChinaPay.B3B.Common.Enums;
    using ChinaPay.B3B.DataTransferObject.Common;
    interface IReposity
    {
        IEnumerable<IntegralInfoView> GetIntegralList(Range<DateTime>? time, Guid? accountID, IntegralWay? way, Pagination pagination);
        IEnumerable<IntegralConsumptionView> GetIntegralConsumptionList(Range<DateTime>? time, IntegralWay? way, Guid? accountId, ExchangeState state, Common.Enums.OEMCommodityState? oemstate, string falg, Guid? oemId, Pagination pagination);
        ChinaPay.B3B.Service.Integral.Domain.IntegralCount GetIntegralCount(Guid id);

        //IntegralInfoView GetIntegralByAccountId(Guid AccountId, IntegralWay? way);
        IntegralInfoView GetIntegral(Guid id);
        IntegralParameterView GetIntegralParameter();
        IntegralConsumptionView GetIntegralConsumption(Guid id);

        void InsertIntegralCount(ChinaPay.B3B.Service.Integral.Domain.IntegralCount info);
        void UpdateIntegralCount(ChinaPay.B3B.Service.Integral.Domain.IntegralCount info);
        void UpdateIntegralCountByConsumption(ChinaPay.B3B.Service.Integral.Domain.IntegralCount orginalInfo, ChinaPay.B3B.Service.Integral.Domain.IntegralCount info);
        void DeleteIntegralCount();


        void UpdateIntegralInfo(IntegralInfoView view);
        void InsertIntegralInfo(ChinaPay.B3B.Service.Integral.Domain.IntegralInfo view);
        void DeleteIntegralInfo(IntegralInfoView view);

        void UpdateIntegralConsumption(Guid id, ChinaPay.B3B.Common.Enums.ExchangeState State, string no, string company, string address, string reason);
        void InsertIntegralConsumption(ChinaPay.B3B.Service.Integral.Domain.IntegralConsumption view);
        void InsertIntegralConsumption(ChinaPay.B3B.Service.Integral.Domain.IntegralConsumption view, int days, int integral, int integralSurplus);
        void DeleteIntegralConsumption(IntegralConsumptionView view);

        void UpdateIntegralParameter(IntegralParameterView view);
        void UpdateShelvesNum(Guid id, int StockNumber);
        void UpdateIntegralConsumption(Guid id, ChinaPay.B3B.Common.Enums.OEMCommodityState State);
    }
}
