using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.Service.Command.Domain.PNR;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// PNR详细
    /// </summary>
    public class PNRWithPrices
    {

        public PNRWithPrices(IssuedPNR pnrContent, List<PriceView> prices)
        {
            this.PnrContent = pnrContent;
            this.Prices = prices;
        }
        /// <summary>
        /// PNR信息；
        /// </summary>
        public IssuedPNR PnrContent { get; set; }

        /// <summary>
        /// PAT出来的价格；
        /// </summary>
        public List<PriceView> Prices { get; set; }
    }
}
