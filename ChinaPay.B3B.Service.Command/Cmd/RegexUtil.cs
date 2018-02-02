using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Service.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core;
using System.Globalization;

namespace ChinaPay.B3B.Service.Command
{
    public class RegexUtil
    {
        // 南航儿童票解析
        public static string CZChildSSRRegex = @"(?<LineNumber>\d{1,2})\.SSR\sCHLD\sCZ\s" + 
                        @"(?<SeatStatus>[A-Z]{2})(?<SeatCount>\d)\s" + 
                        @"(?<Day>\d{2})(?<Month>JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)" +
                        @"(?<Year>\d{2})/P(?<PassengerId>\d{1,2})";

        /// <summary>
        /// SA指令结果；
        /// </summary>
        public static string SARegex = @"(?<LineNumber>\d{1,2})\.\s{4}ARNK\s{14}(?<Departure>[a-zA-Z]{3})(?<Arrival>[a-zA-Z]{3})";
        
        /// <summary>
        /// 团队信息
        /// </summary>
        //public static string TeamRegex = @"0\.(?<TermName>\w*)\sNM(?<NumberOfTerm>\d{1,2})\s(?<PNRCode>[A-Z0-9]{6})";
        public static string TeamRegex = @"0\.(?<TotalNumber>\d{1,3})(?<TermName>\w*)\sNM(?<ActualNumber>\d{1,2})\s(?<PNRCode>[A-Z0-9]{6})";

        public const string FFCmdRegex = @"(?<=;)(?<CityCode>[A-Z]{3})\s{3}" +
                @"(?:(?<ArrivalTime>\d{4})|OPEN|\s{4})(?:\+\d|\s{2})" +
                @"(?:\s{2}(?:(?<DepartureTime>\d{4})|OPEN|\s{4})(?:\+\d|\s{2}))?";

        /// <summary>
        /// RTX指令提取编码时，小编码格式；（2012-11-01，空港龙返回格式错误，-76.EK1E/JGFCL7/KMG215），所以相应的[\w;]后面改过来；
        /// </summary>
        public const string RTXCmdPNRCode = @"(?<LineNumber>\d{1,2})\.[\w;]{5}/(?<PNRCode>[A-Z0-9]{6})/(?<OfficeNo>[A-Z]{3}\d{3})";

        /// <summary>
        /// 其他服务组返回信息
        /// </summary>
        public const string OSICmdRegex = @"(?<LineNumber>\d{1,2})\.OSI\s(?<AirlineCode>\w{2})\s(?<OSIType>[A-Z]{3,4})\s?(?<PassengerInformation>[\w-]+)(?:/P(?<PassengerId>\d{1,2}))?";

        /// <summary>
        /// 取消后的pnr
        /// </summary>
        public const string RTCmdCanceledRegex = @"(?<IsCanceled>THIS\sPNR\sWAS\sENTIRELY\sCANCELLED)";
        
        /// <summary>
        /// RMK返回的授权信息；
        /// </summary>
        public const string RMKCmdRegex = @"(?<LineNumber>\d{1,2})\.RMK\sTJ\sAUTH\s(?<OfficeNo>[A-Z]{3}\d{3})";
        
        /// <summary>
        /// 这个必须要改，人多的时候，姓名可能分两行。
        /// 取得第一行，行以分号分隔，第一行比较特殊。
        /// 之所以有它，是因为姓名在订座信息中无特殊标志，但当有姓名信息时，其必在第一行，
        /// 同时，旅客订座记录编号的读取，也不好辨识，但它会紧跟在姓名后，放在第一行。
        /// </summary>
        public const string FirstLineRegex = @"^\s?\d.*?(?=;)";

        /// <summary>
        /// 取得包含编码和姓名的行
        /// </summary>
        public const string CodeLineRegex = @"(?<=(?:[\^b];)|^)\s?\d.*?(?=;[\s\d]\d\.\s[*\s])";
        // 2012-10-29
        //public const string CodeLineRegex = @"(?<=(?:[\^b];)|^)[\s\d]\d.*?(?=;[\s\d]\d\.\s[*\s])";
        
        /// <summary>
        /// Office号
        /// </summary>
        public const string OfficeNoRegExp =  @"(?<OfficeNo>[A-Z]{3}\d{3})";
        /// <summary>
        /// 电子客票号；
        /// </summary>
        public const string ETicketNumberRegExp = @"(?<TicketNumber>\d{3}-\d{10}";

        /// <summary>
        /// 电子客票状态；
        /// </summary>
        public const string ETicketStatusRegExp = @"(?<ETicketStatus>OPENFORUSE|VOID|REFUNDED|CHECKED-IN|USED/FLOWN|SUSPENDED|PRINT/EXCH|EXCHANGED|LIFT/BOARDED|FIM EXCH|AIRPCNTL/|CPN NOTE)";

        /// <summary>
        /// PNR票号
        /// 格式：
        ///     TN/774-3372725562/P1
        ///     TN/999-3374141477-78/P1
        ///     TN/781-3372725304/P1
        /// </summary>
        public const string PNRTicketNumberRegExp = @"(?<TicketNumber>\d{3}-\d{10}-{0,1}\d{0,2})";

        /// <summary>
        /// 电话号码；
        /// </summary>
        public const string PhoneNumberRegex = @"(?<PhoneNumber>\d{11}|\d{7,8}|\d{3,4}-{7,8})";
        
        /// <summary>
        /// 旅客订座记录编号
        /// </summary>
        public const string PNRCodeRegex = @"(?<Code>\w{6})";

        /// <summary>
        /// 行号
        /// </summary>
        public const string LineNumberRegex = @"(?<LineNumber>\d{1,2})\.";

        #region 座位格式
		/// <summary>
        /// 订座状态
        /// </summary>
        public const string SeatStatusRegex = @"(?<SeatStatus>[A-Z]{2})";
        /// <summary>
        /// 订座数量
        /// </summary>
        public const string SeatCountRegex = @"(?<SeatCount>\d{1,2})";
        /// <summary>
        /// 订座信息
        /// </summary>
        public const string SeatRegex = SeatStatusRegex + SeatCountRegex; 
	    #endregion

        #region 日期格式
        // 时间
        public const string TimeRegex = @"(?<Hour>\d{2})(?<Minute>\d{2})";
        // 出发时间
        public const string DepartureTimeRegex = @"(?<DepartureTime>(?:\d{4}|OPEN))";
        // 到达时间
        public const string ArrivalTimeRegex = @"(?<ArrivalTime>(?:\d{4}\+(?<AddDays>)\d|\d{4}|OPEN))";
        // 日期
        public const string DayRegex = @"(?<Day>[0-3]\d)";
        // 月份
        public const string MonthRegex = @"(?<Month>JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)";
        // 星期两位缩写
        public const string DayOfWeekTwoRegex = @"(?<DayOfWeek>MO|TU|WE|TH|FR|SA|SU)";
        // 星期几的三位缩写
        public const string DayOfWeekThreeRegex = @"(?<DayOfWeek>MON|TUE|WED|THU|FRI|SAT|SUN)";
        /// <summary>
        /// 日期格式一：WE01AUG
        /// </summary>
        public const string DateRegexOne = DayOfWeekTwoRegex + DayRegex + MonthRegex;
        /// <summary>
        /// 日期格式二：WED01AUG
        /// </summary>
        public const string DateRegexTwo = DayOfWeekThreeRegex + DayRegex + MonthRegex; 
        #endregion

        #region 舱位格式
        /// <summary>
        /// 舱位类型
        /// </summary>
        public const string BunkCodeRegex = @"(?<BunkCode>[A-Z])";
        /// <summary>
        /// 舱位
        /// </summary>
        public const string BunkRegex = @"(?<BunkCode>[A-Z])(?<BunkStatus>[A-Z1-9])"; 
        #endregion

        #region 航程格式
        /// <summary>
        /// 城市编码
        /// </summary>
        public const string CityCode = @"(?<CityCode>[A-Z]{3})";
        /// <summary>
        /// 航程
        /// </summary>
        public const string VoyageRegex = @"(?<Departure>[A-Z]{3})(?<Arrival>[A-Z]{3})"; 
        #endregion

        #region 航班格式
        /// <summary>
        /// 航空公司编码
        /// </summary>
        public const string AirlineCodeRegex = @"(?<AirlineCode>\w{2})";
        /// <summary>
        /// 航班号 3U886R，增加了最后个
        /// </summary>
        public const string FlightNoRegex = @"(?<FlightNo>(?:OPEN|[0-9]{4}|[0-9]{3}|[0-9]{4}[A,K]|[0-9]{3}[A,K]|[0-9]{3}[A-Z]))";

        public const string AirlineNoRegex = AirlineCodeRegex + FlightNoRegex;
        #endregion
        
        #region 旅客格式
        /// <summary>
        /// 姓名
        /// </summary>
        public const string NameRegex = @"(?<Name>[A-Z]+/?[A-Z]+|[A-Z]{2,}|[\u4e00-\u9fa5]{2,})";
        /// <summary>
        /// 旅客类型
        /// </summary>
        public const string PassengerTypeRegex = @"(?<PassengerType>CHD)"; 
        #endregion
        
        public const string AircraftTypeRegex = @"(?<AircraftType>[0-9A-Z]{3})";
        public const string TransitPointRegex = @"(?<TransitPoint>\d)";
        public const string MealRegex = @"(?<Meal>[A-Z])";
        public const string ETicketFlagRegex = @"(?<ETicketFlag>E)";
        public const string TerminalOfDepartureRegex = @"(?:(?<TerminalOfDeparture>[\w]{2}|\w\s)|[\s-]{2}|\w\s)";
        public const string TerminalOfArrivalRegex = @"(?:(?<TerminalOfArrival>[\w]{2}|\w\s)|[\s-]{2}|\w\s)";

        /// <summary>
        /// 获取PNR中的机票信息；
        /// </summary>
        public const string PNRTicketsRegex = @"(?<LineNumber>\d{1,2})\.TN/(?<TicketNumber>\d{3}-\d{10}-{0,1}\d{0,2})/P(?<PassengerId>\d{1,2})\s";
        
        /// <summary>
        /// 航段解析，此信息由SS命令返回；
        /// 格式：
        ///     8. UA858 L1 MO23MAR09PVGSFO RR7 1245 0818 E
        ///     9. UA322 L1 MO23MAR09SFOSEA RR7 1036 1246 E
        ///     10. UA821 L2 SU29MAR SEASFO RR7 0735 0945 E
        ///     11. UA889 L2 SU29MAR SFOPEK RR7 1113 1440+1 E
        ///     2.  KY8230 W   MO19NOV  LUMKMG HL1   1640 1735  /6  E
        /// </summary>
        public const string SSCmdRegex = @"(?<LineNumber>\d{1,2})\.\s(?<IsShared>[*\s])" +
                @"(?<AirlineCode>\w{2})" +
                @"(?<FlightNo>(?:OPEN|[0-9]{4}|[0-9]{3}|[0-9]{4}[A,K]|[0-9]{3}[A,K]|[0-9]{3}[A-Z]))\s{1,2}" +
                @"(?<CabinSeat>[A-Z]\d?)\s{2,3}" +
                @"(?<Weekday>MO|TU|WE|TH|FR|SA|SU)(?<Day>\d{2})(?<Month>JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)(?<Year>[\d\s]{2})" +
                @"(?<Departure>[a-zA-Z]{3})(?<Arrival>[a-zA-Z]{3})\s" +
                @"(?<SeatStatus>[A-Z]{2})(?<SeatCount>\d{1,2})\s{2,3}" +
                @"(?<DepartureTime>(?:\d{4}|OPEN))\s" +
                @"(?<ArrivalTime>(?:\d{4}\+(?<AddDays>\d)|\d{4}\s{2}|OPEN\s{2}))(?:[\s\d/]{4,8})" +
                @"(?<ETicketFlag>E)" +
                @"(?:\s(?<TerminalOfDeparture>[\w]{2}|\w\s)|[\s-]{2}|\w\s)?" +
                @"(?:(?<TerminalOfArrival>[\w]{2}|\w\s)|[\s-]{2}|\w\s)?" +
                @"(?:\s{1,2}(?<ExtendedInformation>[\w-]*))?";       // 2012-11-13 增加对扩展信息的处理
        
        /// <summary>
        /// 联系方式解析，此信息由CT命令返回；
        /// 格式：
        /// </summary>
        public const string ContractRegex = LineNumberRegex + CityCode + "?/?" + PhoneNumberRegex;
        
        /// <summary>
        /// 价格解析
        /// 格式：
        ///     36.FN/M/FCNY1380.00/XCNY200.00/TCNY100.00CN/TCNY100.00YQ/ACNY1580.00
        /// </summary>
        public const string PriceRegExp = @"(?<LineNumber>\d{1,2})\." + 
                        @"FN/\w/" + 
                        @"FCNY(?<Fare>\d{2,}\.\d{2})/" + 
                        @"SCNY(?:\d{2,}\.\d{2})/(?:C(?:\d\.\d{2})/)?" + 
                        @"XCNY(?:\d{2,}\.\d{2})/" +
                        @"(?:(?:TCNY(?<AirportTax>\d{2,}\.\d{2})CN)|(?:TEXEMPTCN))/" +
                        @"TCNY(?<BunkerAdjustmentFactor>\d{2,}\.\d{2})YQ/" + 
                        @"[\s;]*ACNY(?<Total>\d{2,}\.\d{2})";

        public const string PassengerIdRegex = @"P(?<PassengerId>\d{1,2})";
        public const string CertificateNumber = @"(?<CertificateNumber>\w{6,18})";
        
        /// <summary>
        /// 在对订座成功后，获取价格指令的信息进行解析；
        ///     小飞机往返低打，解析有问题，
        ///     01 RT/Y+RT/Y FARE:CNY1540.00 TAX:TEXEMPTCN YQ:CNY160.00  TOTAL:1700.00 
        /// </summary>
        public const string PATCmdRegex = 
                @"FARE:CNY(?<Fare>\d{2,}\.\d{2})\s" +
                @"TAX:(?:CNY(?<AirportTax>\d{2,}\.\d{2})|(?:TEXEMPTCN))\s" +
                @"YQ:CNY(?<BunkerAdjustmentFactor>\d{2,}\.\d{2})\s+" +
                @"TOTAL:(?<Total>\d{2,}\.\d{2})";

        /// <summary>
        /// 对订座成功后的信息进行解析，可获取成功的PNR编号，在封口指令执行后可得到；
        /// </summary>
        //public const string EOTCmdRegex = @"(?:(?<PNRCode>[A-Z0-9]{6})(?=\s-\s{3}航空公司)|(?<PNRCode>[A-Z0-9]{6})(?=\s-EOT\s))";
        public const string EOTCmdRegex = @"(?<=(^|;))(?<PNRCode>[A-Z0-9]{6})(?=\s[\s-])";

        /// <summary>
        /// 对取消订座后的信息进行解析，可获取被取消的PNR编号，在XEPNR指令执行后可得到；
        /// </summary>
        public const string XEPNRCmdRegex = @"(?<=PNR\sCANCELLED\s)(?<PNRCode>[A-Za-z0-9]{6})";

        /// <summary>
        /// 对PNR中的姓名组进行解析，PNR中的姓名组由NM指令产生；
        /// </summary>

        /// <summary>
        /// 对PNR中的姓名组进行解析，PNR中的姓名组由NM指令产生；
        /// </summary>
        public const string NMCmdRegex = @"(?<LineNumber>\d{1,2})\." +
                                 @"(?:" +
                                 @"(?<Name>[\u4e00-\u9fa5]{1,}[A-Z]*(?=CHD))(?<PassengerType>CHD)|" +
                                 @"(?<Name>[A-Z]+[\u4e00-\u9fa5]{1,}(?=CHD))(?<PassengerType>CHD)|" +
                                 @"(?<Name>[A-Z]+(?:[\s/][A-Z]+)*(?=CHD))(?<PassengerType>CHD)|" +
                                 @"(?<Name>[\u4e00-\u9fa5]{1,}[A-Z]*)|" +
                                 @"(?<Name>[A-Z]+[\u4e00-\u9fa5]{1,})|" +
                                 @"(?<Name>[A-Z]+(?:[\s/][A-Z]+)*)" +
                                 @")";

        // 2012-10-30 修改；
        //public const string NMCmdRegex = @"(?<LineNumber>\d{1,2})\." +
        // @"(?:" +
        // @"(?<Name>[A-Z]+[\s/][A-Z]+(\s[A-Z]+)?(?<PassengerType>\sCHD)?)|" +
        // @"(?<Name>[\u4e00-\u9fa5]{2,})(?<PassengerType>CHD)?|" +
        // @"(?<Name>[\u4e00-\u9fa5]{1,}[A-Z]+)(?<PassengerType>\sCHD)?|" +
        // @"(?<Name>[A-Z]+[\u4e00-\u9fa5]{1,})(?<PassengerType>CHD)?" +
        // @")";

        /// <summary>
        /// 对PNR中的特殊服务组进行解析，PNR中的特殊服务组由SSR指令产生；
        /// </summary>
        public const string SSRCmdRegex = 
                @"(?<LineNumber>\d{1,2})\.SSR\s[A-Za-z]{4}\s(?<AirlineCode>\w{2})\s" +
                @"(?<SeatStatus>[A-Za-z]{2})(?<SeatCount>\d)\s" +
                @"NI(?<CertificateNumber>[\w-/]{1,18})/" +
                @"P(?<PassengerId>\d{1,2})";

        /// <summary>
        /// 对PNR中的联系组进行解析，PNR中的联系组由CT指令产生；
        /// </summary>
        public const string CTCmdRegex = @"(?<LineNumber>\d{1,2})\.(?:(?<CityCode>[A-Za-z]{3})/)?(?<PhoneNumber>\d{11}|\d{7,8}|\d{3,4}-{7,8})";
        
        /// <summary>
        /// 对PNR中的票号组进行解析，PNR中的票号组由TK指令产生；
        /// </summary>
        public const string TKCmdRegex = @"(?<LineNumber>\d{1,2})\.TL/(?<Hour>\d{2})(?<Minute>\d{2})/(?<Day>[0-3]\d)(?<Month>JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)/(?<OfficeNo>[A-Z]{3}\d{3})";
        
        /// <summary>
        /// 对航班信息进行解析，航班信息可由AVH指令产生；
        /// </summary>
        public const string AVHCmdRegex = 
                @"(?<LineNumber>\d)[-+\s]\s(?<IsShared>[*\s])" +
                @"(?<AirlineCode>\w{2})(?<FlightNo>(?:OPEN|[0-9]{4}|[0-9]{3}|[0-9]{4}[A,K]|[0-9]{3}[A,K]|[0-9]{3}[A-Z]))\s{2,3}" +
                @"(?<CRS>[AD]S[#!*])\s" +
                @"(?:(?:(?<Bunks>[A-Z][A-Z1-9])|\s{2})\s){10}\s" +
                @"(?<Departure>[A-Z]{3})(?<Arrival>[A-Z]{3})\s" +
                @"(?<DepartureHour>\d{2})(?<DepartureMinute>\d{2})\s{3}" +
                @"(?<ArrivalHour>\d{2})(?<ArrivalMinute>\d{2})(?<AddDays>\+\d)?\s{1,3}" +
                @"(?<AircraftType>[0-9A-Z]{3})\s" +
                @"(?<TransitPoint>\d)(?<ASR>[\^\s])" +
                @"(?<Meal>[A-Z\s])\s{2}" +
                @"(?<ETicketFlag>E)\s{2}>\s{3}" +
                @"(?:(?<ShareAirlineCode>\w{2})(?<ShareFlightNo>\d{3,4})|\s{6})\s{6,7}" +
                @"(?:(?:(?<Bunks>[A-Z][A-Z1-9])|\s{2})\s){16}\s{4}" +
                @"(?:(?<TerminalOfDeparture>[\w]{2}|\w\s)|[\s-]{2}|\w\s)\s" +
                @"(?:(?<TerminalOfArrival>[\w]{2}|\w\s)|[\s-]{2}|\w\s)\s" +
                @"(?<FlightTime>[\s\d]\d:\d{2})";

        /// <summary>
        /// 格式化航信指令的返回值，
        /// 将起始处的\r去除，其它的\r将替换为分号。
        /// </summary>
        /// <returns></returns>
        public static string FormatReturnValue(string s)
        {
            s = Regex.Replace(s, "^(\\r)+", "");
            s = Regex.Replace(s, "(\\r)+", ";");
            s = Regex.Replace(s, "(\\^$)", "");
            s = s.Replace("`", " ");
            //s = Regex.Replace(s, "\\s+", " ");
            return s;
        }
    }
}
