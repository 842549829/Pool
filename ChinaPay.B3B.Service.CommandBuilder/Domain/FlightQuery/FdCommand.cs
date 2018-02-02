using System;
using System.Globalization;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain.FlightQuery
{
    public class FdCommand : Command
    {
        /// <summary>
        /// 执行查询运价的指令
        /// </summary>
        /// <param name="airportPair">城市对</param>
        /// <param name="date">日期</param>
        /// <param name="carrier">承运人</param>
        public FdCommand(AirportPair airportPair, DateTime date, string carrier)
        {
            _airportPair = airportPair;
            _date = date;
            _carrier = carrier;
            Initialize();
        }

        private void Initialize()
        {
            CommandString = string.Format("FD:{0}/{1}/{2}", _airportPair,
                                          _date.ToString("ddMMMyy", CultureInfo.CreateSpecificCulture("en-US")),
                                          _carrier);
        }

        private readonly AirportPair _airportPair;
        private readonly DateTime _date;
        private readonly string _carrier;
    }
}