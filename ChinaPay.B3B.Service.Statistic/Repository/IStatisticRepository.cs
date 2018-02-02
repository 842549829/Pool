using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Statistic.Repository {
    interface IStatisticRepository {
        void SaveGeneralOrderSpeedInfo(Guid company, int speed, DateTime processDate, string carrier, Common.Enums.TicketType ticketType, decimal bunisessId, byte type);
        void SaveSpecialProductSupplyInfo(SpecialProductSupplyInfo supplyInfo);
        List<GeneralProductSpeedStatisticInfo> QuerySpeed(IEnumerable<Guid> companies, string carrier, int statisticDays);
        List<SpecialProductSupplyInfo> QuerySupplyStatisticInfo(IEnumerable<Guid> companies, int statisticDays);
    }
}