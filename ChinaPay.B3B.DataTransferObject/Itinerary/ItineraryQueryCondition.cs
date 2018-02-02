using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Itinerary
{
   public class ItineraryQueryCondition
    {
       public DateTime? ETDZStartTime { get; set; }
       public DateTime? ETDZEndTime { get; set; }
       public decimal? OrderId { get; set; }
       public string PNR { get; set; }
       public DateTime? TakeOffStartTime { get; set; }
       public DateTime? TakeOffEndTime { get; set; }

    }
}
