using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Command.PNR
{
    public class PriceView
    {
        /// <summary>
        /// 总价
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 票面价格
        /// </summary>
        public decimal Fare { get; set; }

        /// <summary>
        /// 机场建设费
        /// </summary>
        public decimal AirportTax { get; set; }

        /// <summary>
        /// 燃油附加费
        /// </summary>
        public decimal BunkerAdjustmentFactor { get; set; }
    }
}
