using System.Linq;
using ChinaPay.B3B.Common.Enums;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Statistic {
    public class GeneralProductSpeedInfo {
        public Item B2B { get; internal set; }
        public Item BSP { get; internal set; }

        internal static GeneralProductSpeedInfo Parse(IEnumerable<GeneralProductSpeedStatisticInfo> info) {
            if(!info.Any()) return new GeneralProductSpeedInfo {
                B2B = Item.Default,
                BSP = Item.Default
            };
            return new GeneralProductSpeedInfo {
                B2B = Item.Parse(info.Where(s => s.TicketType == TicketType.B2B)),
                BSP = Item.Parse(info.Where(s => s.TicketType == TicketType.BSP))
            };
        }

        public class Item {
            private Item(){}
            
            public Item(int? etdz, int? refund)
            {
                ETDZ = etdz.HasValue ? etdz.Value : defaultETDZ;
                Refund = refund.HasValue ? refund.Value : defaultRefund;
            }
            /// <summary>
            /// 出票速度
            /// </summary>
            public int ETDZ { get; internal set; }

            /// <summary>
            /// 退票速度
            /// </summary>
            public int Refund { get; internal set; }

            private const int defaultETDZ = 180;
            private const int defaultRefund = 3600;

            internal static Item Default {
                get {
                    return new Item {
                        ETDZ = defaultETDZ,
                        Refund = defaultRefund,
                    };
                }
            }
            internal static Item Parse(IEnumerable<GeneralProductSpeedStatisticInfo> infos) {
                if(!infos.Any()) return Item.Default;
                var etdz = infos.FirstOrDefault(s => s.Type == 1);
                var refund = infos.FirstOrDefault(s => s.Type == 2);
                return new Item {
                    ETDZ = etdz == null ? defaultETDZ : etdz.Speed,
                    Refund = refund == null ? defaultRefund : refund.Speed
                };
            }
        }
    }
}