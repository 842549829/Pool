using System.Text.RegularExpressions;
using System;

namespace ChinaPay.B3B.Service.Command.Domain.FlightQuery {
    public class Bunk {
        public Bunk(string code, string status) {
            this.Code = code;
            this.Status = status;
        }
        /// <summary>
        /// 舱位代码
        /// </summary>
        public string Code { get; private set; }
        /// <summary>
        /// 舱位状态
        /// </summary>
        public string Status { get; private set; }

        public static Bunk Parse(string s)
        {
            Regex pattern = new Regex("(?<Code>[A-Z])(?<Status>[A-Z1-9])");

            Match match = pattern.Match(s);
            if (!match.Success)
            {
                throw new FormatException();
            }

            return new Bunk(match.Groups["Code"].Value, match.Groups["Status"].Value);
        }
    }
}
