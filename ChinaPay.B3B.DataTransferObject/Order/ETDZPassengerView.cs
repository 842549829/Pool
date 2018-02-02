using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Order
{
  public class ETDZPassengerView
    {
        public string SettleCode
        {
            get;
            set;
        }
        public int TicketNoCount
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string PassengerType
        {
            get;
            set;
        }
    }
}
