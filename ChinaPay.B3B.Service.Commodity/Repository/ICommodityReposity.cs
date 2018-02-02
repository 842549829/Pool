using System;
using ChinaPay.Core;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Commodity;

namespace ChinaPay.B3B.Service.Commodity.Repository {
    interface ICommodityReposity {
        IEnumerable<CommodityView> GetCommodityList(bool falg, Pagination pagination);
        CommodityView GetCommodity(Guid id);

        void Update(CommodityView view);
        void Insert(CommodityView view);
        void Delete(CommodityView view);
        void UpdateShelvesNum(Guid id, int StockNumber);
        void UpdateBuyNum(Guid id, int Number);
        void UpdateState(Guid id, bool State);
    }
}