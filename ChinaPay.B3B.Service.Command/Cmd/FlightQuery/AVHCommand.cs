using System;
using System.Globalization;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command.FlightQuery
{
    /// <summary>
    /// 提取座位可用情况的航信指令（C系统指令）。
    /// </summary>
    public class AVHCommand : Command
    {
        /// <summary>
        /// 提取座位可用情况的航信指令
        /// </summary>
        /// <param name="airportPair">航程</param>
        /// <param name="date">日期</param>
        public AVHCommand(AirportPair airportPair, DateTime date)
        {
            this.airportPair = airportPair;
            this.date = date;
            this.time = null;
            Initialize();
        }

        /// <summary>
        /// 提取座位可用情况的航信指令
        /// </summary>
        /// <param name="airportPair">航程</param>
        /// <param name="date">日期</param>
        /// <param name="transactionId">事务编号</param>
        public AVHCommand(AirportPair airportPair, DateTime date, string transactionId)
        {
            this.airportPair = airportPair;
            this.date = date;
            this.transactionId = transactionId;
            Initialize();
        }

        public AVHCommand(AirportPair airportPair, DateTime date, Time time)
        {
            this.airportPair = airportPair;
            this.date = date;
            this.time = time;
            Initialize();
        }


        private void Initialize()
        {
            this.commandType = CommandType.FlightQuery;
            if (this.time.HasValue)
            {
                string strTime = string.Format("{0:D2}{1:D2}", time.Value.Hour,  time.Value.Minute);
                this.commandString = string.Format("AVH/{0}/{1}/{2}/D", airportPair, date.ToString("ddMMM", CultureInfo.CreateSpecificCulture("en-US")), strTime);
            }
            else
            {
                this.commandString = string.Format("AVH/{0}/{1}/D", airportPair, date.ToString("ddMMM", CultureInfo.CreateSpecificCulture("en-US")));
            }
        }

        private AirportPair airportPair;
        private DateTime date;
        private Time? time;
    }    
}
