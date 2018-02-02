using System;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.Command.Domain.FlightQuery
{
    /// <summary>
    /// 舱位
    /// </summary>
    public class OfferedService
    {
        public const string FormatString = @"(?<Class>[A-Z])(?<SeatStatus>[A-Z1-9])";

        /// <summary>
        /// 舱位等级
        /// </summary>
        public string Class { get; private set; }

        /// <summary>
        /// 舱位状态
        /// </summary>
        public string Status { get; private set; }

        public OfferedService(string rank, string status)
        {
            Class = rank;
            Status = status;
        }

        public OfferedService(string offeredService)
        {
            if (!Validate(offeredService))
            {
                throw new FormatException("舱位格式错误");
            }

            var pattern = new Regex(FormatString);
            Match match = pattern.Match(offeredService);

            Class = match.Groups["Class"].Value;
            Status = match.Groups["SeatStatus"].Value;
        }

        public static bool Validate(string str)
        {
            var pattern = new Regex(FormatString);
            return pattern.IsMatch(str);
        }
        
        public static OfferedService Parse(string str)
        {
            return new OfferedService(str);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Class, Status);
        }
    }
}
