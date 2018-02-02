using System.Collections.Generic;
using System.Linq;

namespace ChinaPay.B3B.Service.Statistic {
    public class SpecialProductStatisticOnVoyage {
        /// <summary>
        /// 总信息
        /// </summary>
        public SpecialProductStatisticInfo Total { get; internal set; }
        /// <summary>
        /// 本航线信息
        /// </summary>
        public SpecialProductStatisticInfo Voyage { get; internal set; }

        public string Departure { get; internal set; }
        public string Arrival { get; internal set; }

        internal static SpecialProductStatisticOnVoyage GetDefault(string departure, string arrival) {
            return new SpecialProductStatisticOnVoyage() {
                Total = new SpecialProductStatisticInfo() {
                    OrderCount = 20,
                    SuccessOrderCount = 12,
                    TicketCount = 20,
                    SuccessTicketCount = 12
                },
                Voyage = new SpecialProductStatisticInfo() {
                    OrderCount = 10,
                    SuccessOrderCount = 6,
                    TicketCount = 10,
                    SuccessTicketCount = 6
                },
                Departure = departure,
                Arrival = arrival
            };
        }
        internal static SpecialProductStatisticOnVoyage Parse(IEnumerable<SpecialProductSupplyInfo> supplyInfos, string departure, string arrival) {
            var result = new SpecialProductStatisticOnVoyage() {
                Departure = departure,
                Arrival = arrival
            };
            var totalSuccessSupplyInfos = supplyInfos.Where(item => item.Success);
            result.Total = new SpecialProductStatisticInfo() {
                OrderCount = supplyInfos.Count(),
                SuccessOrderCount = totalSuccessSupplyInfos.Count(),
                TicketCount = supplyInfos.Sum(item => item.TicketCount),
                SuccessTicketCount = totalSuccessSupplyInfos.Sum(item => item.TicketCount)
            };
            var supplyInfosOnVoyage = supplyInfos.Where(item => item.Departure == departure && item.Arrival == arrival).ToList();
            if(supplyInfosOnVoyage.Count == 0) {
                result.Voyage = new SpecialProductStatisticInfo();
            } else {
                var successSupplyInfosOnVoyage = supplyInfosOnVoyage.Where(item => item.Success);
                result.Voyage = new SpecialProductStatisticInfo() {
                    OrderCount = supplyInfosOnVoyage.Count(),
                    SuccessOrderCount = successSupplyInfosOnVoyage.Count(),
                    TicketCount = supplyInfosOnVoyage.Sum(item => item.TicketCount),
                    SuccessTicketCount = successSupplyInfosOnVoyage.Sum(item => item.TicketCount)
                };
            }
            return result;
        }
    }
}