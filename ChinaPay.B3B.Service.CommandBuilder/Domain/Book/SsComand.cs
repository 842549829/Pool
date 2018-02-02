using System;
using System.Globalization;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Book
{
    /// <summary>
    /// 建立航段组的航信指令
    /// </summary>
    public class SsCommand : Command
    {
        /// <summary>
        /// 建立航段组的航信指令
        /// </summary>
        /// <param name="flightNo">航班号</param>
        /// <param name="cabinSeat">舱位</param>
        /// <param name="flightDate">飞行日期</param>
        /// <param name="airport">机场对</param>
        /// <param name="seatCount">座位数</param>
        public SsCommand(string flightNo, string cabinSeat, DateTime flightDate, AirportPair airport, int seatCount)
        {
            _flightNo = flightNo;
            _cabinSeat = cabinSeat;
            _flightDate = flightDate;
            _airport = airport;
            _seatCount = seatCount;
            Initialize();
        }
        
        private void Initialize()
        {
            CommandString =  string.Format("SS:{0}/{1}/{2}/{3}/{4}{5}", _flightNo,_cabinSeat,
                _flightDate.ToString("ddMMM", CultureInfo.CreateSpecificCulture("en-US")), 
                _airport,"LL", _seatCount);
        }

        // 航班号
        private readonly string _flightNo;
        // 舱位
        private readonly string _cabinSeat;
        // 飞行日期
        private DateTime _flightDate;
        // 航程
        private readonly AirportPair _airport;
        // 座位数
        private readonly int _seatCount;
    }
}
