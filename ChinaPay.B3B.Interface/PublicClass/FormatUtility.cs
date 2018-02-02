using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChinaPay.B3B.Interface.PublicClass
{
    static class FormatUtility
    {
        public static string FormatDatetime(DateTime? datetime)
        {
            return datetime.HasValue ? datetime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "";
        }
        public static string FormatDatetime(DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string FormatFlightDatetime(DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm");
        }
        public static string FormatAmount(decimal amount)
        {
            return amount.ToString("F2");
        }
        public static string FormatAmount(decimal? amount)
        {
            return amount.HasValue ? FormatAmount(amount.Value) : "";
        }
    }
}