using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 建立航段组的航信指令
    /// </summary>
    public class SSCommand : Command
    {
        /// <summary>
        /// 建立航段组的航信指令
        /// </summary>
        /// <param name="flightNo">航班号</param>
        /// <param name="cabinSeat">舱位</param>
        /// <param name="flightDate">飞行日期</param>
        /// <param name="airportPair">航程</param>
        /// <param name="seatCount">座位数</param>
        public SSCommand(string flightNo, string cabinSeat, DateTime flightDate, AirportPair airportPair, int seatCount)
        {
            this.flightNo = flightNo;
            this.cabinSeat = cabinSeat;
            this.flightDate = flightDate;
            this.airportPair = airportPair;
            this.seatCount = seatCount;
            Initialize();
        }

        /// <summary>
        /// 建立航段组的航信指令
        /// </summary>
        /// <param name="flightNo">航班号</param>
        /// <param name="cabinSeat">舱位</param>
        /// <param name="flightDate">飞行日期</param>
        /// <param name="airportPair">航程</param>
        /// <param name="seatCount">座位数</param>
        /// <param name="transactionId">事务编号</param>
        public SSCommand(string flightNo, string cabinSeat, DateTime flightDate, AirportPair airportPair, int seatCount, string transactionId)
        {
            this.flightNo = flightNo;
            this.cabinSeat = cabinSeat;
            this.flightDate = flightDate;
            this.seatCount = seatCount;
            this.airportPair = airportPair;
            this.transactionId = transactionId;
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.PNRCreation;
            this.commandString =  string.Format("SS:{0}/{1}/{2}/{3}/{4}{5}", flightNo,cabinSeat,
                flightDate.ToString("ddMMM", CultureInfo.CreateSpecificCulture("en-US")), 
                airportPair,"LL", seatCount);
        }

        // 航班号
        private string flightNo;
        // 舱位
        private string cabinSeat;
        // 飞行日期
        private DateTime flightDate;
        // 航程
        private AirportPair airportPair;
        // 座位数
        private int seatCount;
    }
}
