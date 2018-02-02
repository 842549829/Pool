using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Order
{
    public class OrderView
    {
        /// <summary>
        /// 编码
        /// </summary>
        public PNRPair PNR { get; set; }

        public bool UseBPNR { get; set; }

        /// <summary>
        /// 关联订单号
        /// </summary>
        public decimal? AssociateOrderId { get; set; }

        /// <summary>
        /// 关联编码
        /// </summary>
        public PNRPair AssociatePNR { get; set; }

        /// <summary>
        /// 航班集合
        /// </summary>
        public IEnumerable<FlightView> Flights { get; set; }

        /// <summary>
        /// 乘机人集合
        /// </summary>
        public IEnumerable<PassengerView> Passengers { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public OrderSource Source { get; set; }

        /// <summary>
        /// 联系信息
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// PAT价格
        /// </summary>
        public Command.PNR.PriceView PATPrice { get; set; }

        /// <summary>
        /// 是否降舱
        /// </summary>
        public bool IsReduce
        {
            get
            {
                if (Flights.Count() == 2 && PATPrice != null)
                {
                    return PATPrice.Fare < Flights.Sum(item => item.Fare);
                }
                return false;
            }
        }

        /// <summary>
        /// 是否团队
        /// </summary>
        public bool IsTeam { get; set; }

        /// <summary>
        /// 行程类型
        /// </summary>
        public ItineraryType TripType { get; set; }

        public bool FdSuccess { get; set; }
        public string PnrContent { get; set; }
        public string PatContent { get; set; }
    }
}