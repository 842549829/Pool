using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command
{
    public static class ETicketUtil
    {
        /// <summary>
        /// 票号
        /// </summary>
        public const string ETicketNumberRegExp = @"DETR:TN/(?<TicketNumber>\d{3}-\d{10})";

        /// <summary>
        /// 旅客
        /// </summary>
        public const string PassangerRegExp = @"(?<=PASSENGER:\s)" +
                 @"(?:" +
                 @"(?<Name>[A-Z]+[\s/][A-Z]+(\s[A-Z]+)?(?<PassengerType>\sCHD)?)|" +
                 @"(?<Name>[\u4e00-\u9fa5]{2,})(?<PassengerType>CHD)?|" +
                 @"(?<Name>[\u4e00-\u9fa5]{1,}[A-Z]+)(?<PassengerType>\sCHD)?|" +
                 @"(?<Name>[A-Z]+[\u4e00-\u9fa5]{1,})(?<PassengerType>CHD)?" +
                 @")";
        
        /// <summary>
        /// PNR
        /// </summary>
        public const string PNRPairRegExp = @"(?<=RL:)(?<BPNR>[A-Z0-9]{6})\s{2}/(?<CPNR>[A-Z0-9]{6})";

        /// <summary>
        /// 航空公司（未去空格）
        /// </summary>
        public const string AirlineCompanyRegExp = @"(?<=ISSUED BY:\s)(?<AirlineCompany>[\w\s]*)(?=ORG/DST)";

        /// <summary>
        /// 状态
        /// </summary>
        public const string StatusRegExp = @"(?<Status>(OPEN\sFOR\sUSE|VOID|REFUNDED|CHECKED\sIN|USED/FLOWN|SUSPENDED|PRINT/EXCH|EXCHANGED|LIFT/BOARDED|FIM\sEXCH|AIRPCNTL/|CPN\sNOTE))";
    }
}
