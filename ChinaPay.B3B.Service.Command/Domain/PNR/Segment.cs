using System;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command.Domain.PNR
{
    /// <summary>
    /// 航段
    /// </summary>
    public class Segment : IComparable<Segment>
    {
        internal Segment()
        {
            
        }

        public Segment(string airlineCode, string internalNo,  string cabinSeat, DateTime flightDate, AirportPair airportPair, int seatCount)
        {
            AirlineCode = airlineCode;
            InternalNo = internalNo;
            CabinSeat = cabinSeat;
            Date = flightDate;
            AirportPair = airportPair;
            SeatCount = seatCount;
        }
        
        /// <summary>
        /// 出发机场
        /// </summary>
        public string DepartureAirport
        {
            get { return AirportPair.Departure; }
        }

        /// <summary>
        /// 到达机场
        /// </summary>
        public string ArrivalAirport
        {
            get { return AirportPair.Arrival; }
        }

        /// <summary>
        /// 餐食代码，可能为空；
        /// </summary>
        public string Meal { get; internal set; }

        /// <summary>
        /// 是否共享航班
        /// </summary>
        public bool IsShared { get; internal set; }

        /// <summary>
        /// 是否电子客票
        /// </summary>
        public bool IsETicket{ get; internal set; }

        /// <summary>
        /// 机型
        /// </summary>
        public string AircraftType { get; internal set; }

        /// <summary>
        /// 经停点个数
        /// </summary>
        public int TransitPoint { get; internal set; }
        
        /// <summary>
        /// 行动代码，即订票状态；
        /// </summary>
        public string Status { get; internal set; }
        
        /// <summary>
        /// 出发机场航站楼
        /// </summary>
        public string TerminalOfDeparture { get; internal set; }

        /// <summary>
        /// 到达机场航站楼
        /// </summary>
        public string TerminalOfArrival { get; internal set; }
        
        /// <summary>
        /// 航空公司
        /// </summary>
        public string AirlineCode { get; internal set; }

        /// <summary>
        /// 航班号，临时航班有可能为OPEN；
        /// </summary>
        public string InternalNo { get; internal set; }
        
        /// <summary>
        /// 舱位
        /// </summary>
        public string CabinSeat { get; internal set; }

        /// <summary>
        /// 飞行日期
        /// </summary>
        public DateTime Date { get; internal set; }

        /// <summary>
        /// 航段
        /// </summary>
        public AirportPair AirportPair { get; internal set; }

        /// <summary>
        /// 起飞时间，临时航班在返回字串中为OPEN,则此字段不会赋值；
        /// </summary>
        public Time DepartureTime { get; internal set; }
        /// <summary>
        /// 降落时间，临时航班在返回字串中为OPEN,则此字段不会赋值；
        /// </summary>
        public Time ArrivalTime { get; internal set; }

        /// <summary>
        /// 订座个数
        /// </summary>
        public int SeatCount { get; internal set; }

        /// <summary>
        /// 航班可能跨单日或多日飞行，此为跨越的天数；
        /// </summary>
        public int AddDays { get; internal set; }

        /// <summary>
        /// 到达时间，可能有跨天；
        /// </summary>
        public DateTime ArrivalDate
        {
            get { return Date.AddDays(AddDays).AddHours(ArrivalTime.Hour).AddMinutes(ArrivalTime.Minute); }
        }

        /// <summary>
        /// 起飞时间，不跨天；
        /// </summary>
        public DateTime DepartureDate
        {
            get { return Date.AddHours(DepartureTime.Hour).AddMinutes(DepartureTime.Minute); }
        }

        /// <summary>
        /// 比较大小的方法
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Segment other)
        {
            return DepartureDate.CompareTo(other.DepartureDate);
        }

        public static bool IsSaveVoyage(Segment first, Segment second)
        {
            if(first != null && second != null) {
                return first.AirlineCode == second.AirlineCode
                       && first.InternalNo == second.InternalNo
                       && first.Date.Date == second.Date.Date
                       && first.AirportPair.Equals(second.AirportPair);
            }
            return false;
        }
    }
}
