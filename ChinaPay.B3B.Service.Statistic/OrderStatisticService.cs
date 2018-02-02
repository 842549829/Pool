using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.Statistic {
    public static class OrderStatisticService {
        /// <summary>
        /// 保存特殊票处理信息
        /// </summary>
        public static void SaveSpecialProductSupplyInfo(Guid company, string departure, string arrival, int ticketCount, bool success, DateTime supplyDate) {
            var supplyInfo = new SpecialProductSupplyInfo() {
                Company = company,
                Departure = departure,
                Arrival = arrival,
                TicketCount = ticketCount,
                Success = success,
                SupplyDate = supplyDate
            };
            var repository = Repository.Factory.CreateStatisticRepository();
            repository.SaveSpecialProductSupplyInfo(supplyInfo);
        }
        /// <summary>
        /// 保存非特殊票出票速度信息
        /// </summary>
        public static void SaveGeneralOrderETDZSpeed(Guid company, decimal orderId, int speed, DateTime etdzDate, string carrier, TicketType ticketType) {
            var repository = Repository.Factory.CreateStatisticRepository();
            repository.SaveGeneralOrderSpeedInfo(company, speed, etdzDate, carrier, ticketType, orderId, 1);
        }
        /// <summary>
        /// 保存非特殊票退票速度信息
        /// </summary>
        public static void SaveGeneralOrderRefundSpeed(Guid company, decimal applyformId, int speed, DateTime refundDate, string carrier, TicketType ticketType) {
            var repository = Repository.Factory.CreateStatisticRepository();
            repository.SaveGeneralOrderSpeedInfo(company, speed, refundDate, carrier, ticketType, applyformId, 2);
        }

        /// <summary>
        /// 查询出票/退票速度
        /// </summary>
        public static Dictionary<Guid, GeneralProductSpeedInfo> QuerySpeed(IEnumerable<Guid> companies, string carrier) {
            var result = new Dictionary<Guid, GeneralProductSpeedInfo>();
            if(companies.Any()) {
                var repository = Repository.Factory.CreateStatisticRepository();
                var speedInfos = repository.QuerySpeed(companies, carrier, Service.SystemManagement.SystemParamService.ProductStatisticDays);
                foreach(var company in companies) {
                    var sis = speedInfos.Where(s => s.Company == company);
                    result.Add(company, GeneralProductSpeedInfo.Parse(sis));
                }
            }
            return result;
        }

        /// <summary>
        /// 查询特殊票提供资源的统计信息
        /// </summary>
        public static Dictionary<Guid, SpecialProductStatisticOnVoyage> QuerySupplyStatisticInfo(IEnumerable<Guid> companies, string departure, string arrival) {
            if(!companies.Any()) return new Dictionary<Guid, SpecialProductStatisticOnVoyage>();
            if(string.IsNullOrWhiteSpace(departure)) throw new ArgumentNullException("departure");
            if(string.IsNullOrWhiteSpace(arrival)) throw new ArgumentNullException("arrival");
            var repository = Repository.Factory.CreateStatisticRepository();
            var supplyStatisticInfos = repository.QuerySupplyStatisticInfo(companies, Service.SystemManagement.SystemParamService.ProductStatisticDays);
            var companyStatisticInfos = supplyStatisticInfos.GroupBy(item => item.Company).ToDictionary(
                item => item.Key,
                item => SpecialProductStatisticOnVoyage.Parse(item, departure, arrival));
            foreach(var company in companies) {
                if(!companyStatisticInfos.ContainsKey(company)) {
                    companyStatisticInfos.Add(company, SpecialProductStatisticOnVoyage.GetDefault(departure, arrival));
                }
            }
            return companyStatisticInfos;
        }
    }
}