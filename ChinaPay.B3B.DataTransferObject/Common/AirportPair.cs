
using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.DataTransferObject.Common {
    /// <summary>
    /// 航程
    /// </summary>
    public class AirportPair {
        public AirportPair() {
        }

        public AirportPair(string departure, string arrival) {
            if(!string.IsNullOrWhiteSpace(departure)) Departure = departure.ToUpper();
            if(!string.IsNullOrWhiteSpace(arrival)) Arrival = arrival.ToUpper();
        }
        /// <summary>
        /// 出发机场
        /// </summary>
        public string Departure { get; set; }

        /// <summary>
        /// 到达机场
        /// </summary>
        public string Arrival { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}", Departure, Arrival);
        }
        
        public string ToString(char seperator)
        {
            return string.Format("{0}{1}{2}", Departure, seperator, Arrival);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((AirportPair)obj);
        }

        protected bool Equals(AirportPair other)
        {
            return string.Equals(Departure, other.Departure) && string.Equals(Arrival, other.Arrival);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Departure != null ? Departure.GetHashCode() : 0) * 397) ^ (Arrival != null ? Arrival.GetHashCode() : 0);
            }
        }

        /// <summary>
        /// 判断城市对是否连续。
        /// </summary>
        /// <param name="airports"></param>
        /// <returns></returns>
        /// <remarks>
        /// 此方法不对传入的城市对做任何排序处理；
        /// </remarks>
        public static bool IsContinuousAirports(List<AirportPair> airports)
        {
            if (airports.Count == 0) throw new ArgumentException("机场");

            bool flag = true;
            for (int i = 1; i < airports.Count; i++)
            {
                if (airports[i].Departure != airports[i - 1].Arrival)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

    }
}