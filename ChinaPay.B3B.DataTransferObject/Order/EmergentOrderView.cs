using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Order
{
    public class EmergentOrderView
    {
        public decimal Id { get; set; }
        public OrderStatus Type { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
}
