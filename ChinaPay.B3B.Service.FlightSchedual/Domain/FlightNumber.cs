using System;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.FlightSchedual.Domain
{
    /// <summary>
    /// 航班号
    /// </summary>
    public class FlightNumber
    {
        public const string FormatString =
            @"(?<Carrier>[A-Z0-9]{2})(?<InternalNumber>(?:[0-9]{3}[A-Z]|[0-9]{3,4}))";

        public FlightNumber(string flightNumber)
        {
            if (!Validate(flightNumber))
            {
                throw new FormatException("航班号格式错误");
            }

            var pattern = new Regex(FormatString);
            Match match = pattern.Match(flightNumber);

            Carrier = match.Groups["Carrier"].Value;
            InternalNumber = match.Groups["InternalNumber"].Value;
        }

        public FlightNumber(string carrier, string internalNumber)
        {
            Carrier = carrier;
            InternalNumber = internalNumber;
        }

        /// <summary>
        /// 承运人
        /// </summary>
        public string Carrier { get; private set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string InternalNumber { get; private set; }

        /// <summary>
        /// 转换字符串为航班号
        /// </summary>
        /// <param name="str">待转换字串</param>
        /// <returns>航班号对象</returns>
        public static FlightNumber Parse(string str)
        {
            return new FlightNumber(str);
        }

        /// <summary>
        /// 验证一个字符串的格式是否为为航班号
        /// </summary>
        /// <param name="str">待检测字串</param>
        /// <returns>是否通过检测</returns>
        public static bool Validate(string str)
        {
            var pattern = new Regex(FormatString);
            return pattern.IsMatch(str);
        }

        protected bool Equals(FlightNumber other)
        {
            return string.Equals(Carrier, other.Carrier) && string.Equals(InternalNumber, other.InternalNumber);
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Carrier, InternalNumber);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FlightNumber) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Carrier != null ? Carrier.GetHashCode() : 0)*397) ^
                       (InternalNumber != null ? InternalNumber.GetHashCode() : 0);
            }
        }
    }
}
