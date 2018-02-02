using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ChinaPay.B3B.Service.Command.FlightQuery
{
    /// <summary>
    /// 查询航班的经停城市,起降时间和机型的航信指令。
    /// </summary>
    public class FFCommand : Command
    {
        /// <summary>
        /// 查询航班的经停城市,起降时间和机型
        /// </summary>
        /// <param name="flightNumber">航班号</param>
        /// <param name="date">起飞时间</param>
        public FFCommand(string flightNumber, DateTime date)
        {
            this.flightNumber = flightNumber;
            this.date = date;
            Initialize();
        }

        private void Initialize()
        {
            this.commandType = CommandType.FlightQuery;
            this.commandString = string.Format("FF:{0}/{1}", flightNumber, date.ToString("ddMMM", CultureInfo.CreateSpecificCulture("en-US")));
        }

        // 航班号
        private string flightNumber;
        // 日期
        private DateTime date;
    }
}
