using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Command.Domain.DataTransferObject;
using ChinaPay.B3B.Service.Command.Domain.FlightQuery;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.Command.Domain.Ticket;
using ChinaPay.B3B.Service.Queue.Domain;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Command.Domain.Utility
{
    public class Parser
    {
        public static RegexUtil RegexUtil = new RegexUtil();
        #region 原始字串处理

        /// <summary>
        /// 移除\n和\r，用于处理价格信息；
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string RemoveAllReturnFlag(string str)
        {
            if (str == null) return string.Empty;
            str = Regex.Replace(str, "\r", "");
            str = Regex.Replace(str, "\n", "");
            return str;
        }


        public static string PreProcess(string str)
        {
            if (str == null) return string.Empty;

            str = ProcessFirstSpace(str);
            str = ReplacePageTurningFlag(str);
            str = RemoveAsciiDataLinkEscape(str);
            str = RemoveReturnFlag(str);
            return str;
        }
        
        /// <summary>
        /// 用于PNR信息中的移除所有的非开始处的\n，如：
        ///  1.付绍智 2.寇晓鸣 3.赖宝贵 4.李河 5.罗凯 6.宋军 HGP8GP  7.  CZ3902 G   TU02APR 
        /// KMGPEK NO6   1335 1700          E --T2 
        /// </summary>
        /// <param name="str">待处理字串</param>
        /// <returns>处理后字串</returns>
        /// <remarks>
        /// 如果用户将pat信息一起贴进来，由于原本就要替换，应该不至于会出现问题；
        /// </remarks>
        public static string ProcessMiddleSpace(string str)
        {
            if (str == null) return string.Empty;
            // 这个正则表达捕获的是不以正常行号（如8）结束的起始位置，
            str = Regex.Replace(str, @"\n(?![\s\d]\d\.)", "");
            return str;
        }

        private static string ProcessFirstSpace(string str)
        {
            if (str == null) return string.Empty;

            var pattern = new Regex(@"(?:^[01]|^\s{2,}[01])");
            var match = pattern.Match(str);
            if (match.Success)
            {
                return string.Format("{0}{1}", " ", str.Trim());
            }
            return str;
        }

        /// <summary>
        /// 对翻页标志做替换
        /// </summary>
        /// <param name="str">待替换字串</param>
        /// <returns>替换后字串</returns>
        public static string ReplacePageTurningFlag(string str)
        {
            if (str == null) return string.Empty;
            str = Regex.Replace(str, @"[+-](?=[\d\s]\d\.)", "\n");
            return str;
        }

        /// <summary>
        /// 移除\r标记。
        /// </summary>
        /// <param name="str">待处理字串</param>
        /// <returns>处理后字串</returns>
        /// <remarks>
        /// 当在界面使用文本框后，会带有\r\n，此时若在正则中使用正则的多行模式，并在表达式中使用^来匹配行首；
        /// 只有\n才能被识别为行首；
        /// </remarks>
        public static string RemoveReturnFlag(string str)
        {
            if (str == null) return string.Empty;
            str = Regex.Replace(str, "\r", "");
            return str;
        }
        /// <summary>
        /// 移除ASCII码中的数据链路转义（0x10）；
        /// </summary>
        /// <param name="str">待处理字串</param>
        /// <returns> 替换后字串</returns>
        /// <remarks>
        /// 航信的指令中，会带有ascii码值为0x10的字符，替换之，防止此字符在后面的使用中影响解析（如转XML格式）；
        /// </remarks>
        public static string RemoveAsciiDataLinkEscape(string str)
        {
            if (str == null) return string.Empty;
            str = Regex.Replace(str, @"\x10", "");
            return str;
        }


        public static string RemoveCnNameLineBreakAndSpace(string str)
        {
            if (str == null) return string.Empty;
            // 2013-04-01 由于编码后面可能不带换行，在后面增加了?号
            str = Regex.Replace(str, @"(?<=[\u4e00-\u9fa5])\s*(\r|\n|\r\n)?(?=[\u4e00-\u9fa5])", "");
            return str;
        }

        /// <summary>
        /// 移除换行符和其前面的空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns>
        /// 这个如果碰到英文名字的话，可能会出错不？是不是只移除中文中的这种情况呢？
        /// </returns>
        public static string RemoveLineBreakAndSpace(string str)
        {
            if (str == null) return string.Empty;
            // 2013-04-01 由于编码后面可能不带换行，在后面增加了?号
            str = Regex.Replace(str, "\\s*\r?", "");
            str = Regex.Replace(str, "\\s*\n?", "");
            return str;
        }
        
        public static bool IsXmlFormat(string str)
        {
            if (str == null) return false;
            var pattern = new Regex(RegexUtil.GetRegexString("XMLFormat"), RegexOptions.IgnoreCase);
            var match = pattern.Match(str);
            return match.Success;
        }
        #endregion

        #region 航班解析
        /// <summary>
        /// 根据给出的字串，获取航班信息。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>航班列表</returns>
        public static List<Flight> GetFlights(string str)
        {
            var result = new List<Flight>();

            if (str == null) return result;
            var pattern = new Regex(RegexUtil.GetRegexString("AvhResult"), RegexOptions.Multiline);
            var matchCollection = pattern.Matches(str);

            // 取得日期：
            Match match = new Regex(@"^\s(?<Day>[0-3]\d)(?<Month>JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)").Match(str);
            if (!match.Success)
            {
                return result;
            }

            var date = DateTime.Parse(match.Captures[0].Value, new CultureInfo("en-US"));
            //转出来的日期，默认是当年的，如果跨年，则加一；
            if (date.DayOfYear < DateTime.Now.DayOfYear)
            {
                date = date.Date.AddYears(1);
            }

            if (matchCollection.Count > 0)
            {
                foreach (Match item in matchCollection)
                {
                    var flight = new Flight
                                     {
                                         Number =
                                             new FlightNumber(item.Groups["Carrier"].Value,
                                                              item.Groups["InternalNumber"].Value),
                                         Arrival = item.Groups["Arrival"].Value,
                                         Departure = item.Groups["Departure"].Value,
                                         TerminalOfDeparture = item.Groups["TerminalOfDeparture"].Value,
                                         TerminalOfArrival = item.Groups["TerminalOfArrival"].Value,
                                         DepartureTime =
                                             new Time(int.Parse(item.Groups["DepartureHour"].Value),
                                                      int.Parse(item.Groups["DepartureMinute"].Value)),
                                         ArrivalTime =
                                             new Time(int.Parse(item.Groups["ArrivalHour"].Value),
                                                      int.Parse(item.Groups["ArrivalMinute"].Value)),
                                         AddDays =
                                             string.IsNullOrWhiteSpace(item.Groups["AddDays"].Value)
                                                 ? 0
                                                 : int.Parse(item.Groups["AddDays"].Value),
                                         AirCraft = item.Groups["AircraftType"].Value,
                                         HasFood = !string.IsNullOrEmpty(item.Groups["Meal"].Value),
                                         TransitPoint = int.Parse(item.Groups["TransitPoint"].Value),
                                         ShareFlightNo =
                                             item.Groups["CodeShareFlightCarrier"].Value +
                                             item.Groups["CodeShareFlightInternalNumber"].Value,
                                         Bunks =
                                             (from Capture capture in item.Groups["OfferedService"].Captures
                                              select Bunk.Parse(capture.Value)).ToList(),
                                         FlightDate = date
                                     };
                    result.Add(flight);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据给出的字串，获取经停信息。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>经停点列表</returns>
        public static List<TransitPoint> GetTransitPoints(string str)
        {
            var result = new List<TransitPoint>();
            if (str == null) return result;

            var ffResult = RegexUtil.RegularExpresstions["FfResult"].Value;
            var pattern = new Regex(ffResult, RegexOptions.Multiline);
            MatchCollection matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                foreach (Match item in matchCollection)
                {
                    string airportCode = item.Groups["AirportCode"].Value;
                    var arrivalTime = new Time(int.Parse(item.Groups["ArrivalHour"].Value),
                                               int.Parse(item.Groups["ArrivalMinute"].Value));
                    var arrivalAddDays = string.IsNullOrEmpty(item.Groups["ArrivalAddDays"].Value)
                                             ? 0
                                             : int.Parse(item.Groups["ArrivalAddDays"].Value);
                    var departureTime = new Time(int.Parse(item.Groups["DepartureHour"].Value),
                                                 int.Parse(item.Groups["DepartureMinute"].Value));
                    var departureAddDays = string.IsNullOrEmpty(item.Groups["DepartureAddDays"].Value)
                                               ? 0
                                               : int.Parse(item.Groups["DepartureAddDays"].Value);

                    var transitPoint = new TransitPoint(
                        airportCode, arrivalTime, arrivalAddDays, departureTime, departureAddDays
                        );

                    result.Add(transitPoint);
                }
            }

            return result;
        }
        
        #endregion
        

        /// <summary>
        /// 根据给出的待解析字串，获取其中包含的价格内容。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>价格内容原始数据</returns>
        public static string GetPatRawDate(string str)
        {
            if (str == null) return string.Empty;
            var pattern = new Regex(RegexUtil.GetRegexString("PatContent"), RegexOptions.Singleline | RegexOptions.Multiline);
            var match = pattern.Match(str);
            return match.Groups["PatContent"].Value;
        }
        
        /// <summary>
        /// 根据给出的待解析字串，获取其中包含的旅客订座记录内容。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>旅客订座记录内容原始数据</returns>
        private static string GetPnrRawDate(string str)
        {
            if (str == null) return string.Empty;
            var pattern = new Regex(RegexUtil.GetRegexString("PnrContent"), RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var match = pattern.Match(str);
            return match.Groups["PnrContent"].Value;
        }
        
        /// <summary>
        /// 根据给出的字串，获取旅客订座信息内容。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>旅客订座信息</returns>
        internal static ReservedPnr GetPnrContent(string str)
        {
            str = PreProcess(str);
            
            // 此后两句，如果后面取得原始内容的方法没有问题，则可以合并；
            if (!ValidatePnrContent(str)) return null;
            // 获取原始数据；
            // 区别对待下，如果是获取pnr信息，将其中的非行首的\n去掉；
            var pnrRawData = GetPnrRawDate(RemoveCnNameLineBreakAndSpace(ProcessMiddleSpace(str)));
            var patRawData = GetPatRawDate(str);

            if (IsCanceled(str))
            {
                return new ReservedPnr
                           {
                               HasCanceled = true
                           };
            }
            
            // 取得团队信息；
            var reservedTermInfo = GetTermInformation(str);
            // 获取名称项信息（可能包含旅客订座记录编号，当为团队时或者只是NM返回时无信息），并对其中的换行做替换；
            var nameAndOfficeStr = RemoveLineBreakAndSpace(GetNameAndOfficeNoString(str));
            // 将名称项中的PnrCode去掉，以方便姓名项的解析；
            var nameItemPnrCode = GetNameItemPnrCode(nameAndOfficeStr);
            var nameStr = (nameItemPnrCode == null ? nameAndOfficeStr.Trim() : nameAndOfficeStr.Replace(nameItemPnrCode, "").Trim());
            
            // 分别获取各项基础信息；
            var pnrPair = GetPnrPair(str,reservedTermInfo == null
                           ? nameItemPnrCode
                           : reservedTermInfo.PnrCode);

            // 2013-05-12 配合新增标志值，赋值，判断在团编或者是名称项后的信息是否包含C系统编码信息；
            var usedCodeFlag = string.IsNullOrEmpty(pnrPair.PNR) || (reservedTermInfo == null
                                    ? nameAndOfficeStr
                                    : reservedTermInfo.PnrCode).Contains(pnrPair.PNR);

            var reservedPassengersInfos = GetPassengers(nameStr);
            var reservedCertificateInfos = GetCertificateNumbers(str);
            var reservedContractInfos = GetContractInformations(str);
            var reservedChildInfos = GetChildrenInfo(str);
            var reservedSegments = GetSegements(str);
            
            var passengers = new List<Passenger>();
            // 以解析出的旅客姓名项信息为基础，构建旅客信息；
            for (int i = 0; i < reservedPassengersInfos.Count; i++)
            {
                ReservedPassengerInfo passenger = reservedPassengersInfos[i];
                ReservedCertificateInfo certificate = (from ci in reservedCertificateInfos
                                                       where ci.PassengerId == passenger.LineNumber
                                                       select ci).FirstOrDefault();
                ReservedContractInfo contract = (from ci in reservedContractInfos
                                                 where ci.PassengerId == passenger.LineNumber
                                                 select ci).FirstOrDefault();
                ReservedChildInfo child = (from ci in reservedChildInfos
                                           where ci.PassengerId == passenger.LineNumber
                                           select ci).FirstOrDefault();

                passengers.Add(new Passenger(passenger.Name, child == null ? passenger.Type : PassengerType.Child,
                                             certificate == null ? null : certificate.Number,
                                             certificate == null ? CredentialsType.其他 : certificate.CertificateType,
                                             contract == null ? null : contract.Contract));
            }
            
            var segments = (from s in reservedSegments
                                select
                                    new Segment
                                        {
                                            AddDays = s.AddDays,
                                            AirportPair = new AirportPair(s.DepartureAirport,
                                                                          s.ArrivalAirport),
                                            AirlineCode = s.FlightNumber.Carrier,
                                            InternalNo = s.FlightNumber.InternalNumber,
                                            TerminalOfArrival = s.TerminalOfArrival,
                                            TerminalOfDeparture = s.TerminalOfDeparture,
                                            Date = s.FlightDate,
                                            Status = s.SeatStatus,
                                            SeatCount = s.Seatings,
                                            ArrivalTime = new Time(s.ArrivalDate),
                                            DepartureTime = new Time(s.DepartureDate),
                                            CabinSeat = s.ClassOfService,
                                            IsETicket = s.IsETicket,
                                            IsShared = s.IsCodeShareFlight
                                        }).ToList();
            var voyage = new Voyage(segments);
            
            var officeNo = GetOfficeNo(str);  //ok
            var authorizes =(from a in GetAuthorizes(str)
						select a.OfficeNo);   // ok

            var passengerConsistsType = (reservedTermInfo == null)
                                            ? PassengerConsistsType.Individual
                                            : PassengerConsistsType.Group;

            var needFill = false;
            if (voyage.Type == ItineraryType.Notch)
            {
                var voyageAirports = from s in reservedSegments
                                    select new ReservedFillAirports
                                               {
                                                   LineNumber = s.LineNumber,
                                                   AirportPair = new AirportPair(s.DepartureAirport, s.ArrivalAirport)
                                               };             
                var filledAirports = GetFilledAirportPairs(str);
                var allAirports = (from a in voyageAirports.Concat(filledAirports)
                                   orderby a.LineNumber
                                   select a.AirportPair).ToList();
                // 若为连续行程，则不需搭桥，反之，则需要；
                needFill = !AirportPair.IsContinuousAirports(allAirports);
            }

            // 验证下必须的参数；
            if (pnrPair == null || !passengers.Any())
            {
                return null;
            }

            var result = new ReservedPnr
                             {
                                 
                                 PnrPair = pnrPair,
                                 OfficeNo = officeNo,
                                 Authorizes = authorizes,
                                 Voyage = voyage,
                                 Passengers = passengers,
                                 PassengerConsistsType = passengerConsistsType,
                                 TotalNumber = reservedTermInfo == null ? passengers.Count : reservedTermInfo.TotalNumber,
                                 NeedFill = needFill,
                                 HasCanceled = false,
                                 PatRawData = patRawData,
                                 PnrRawData = pnrRawData,
                                 UsedCrsCode = usedCodeFlag
                             };
            return result;
        }
        
        private static bool ValidatePnrContent(string str)
        {
            if (str == null) return false;

            var pattern = new Regex(RegexUtil.GetRegexString("ValidatePnr"), RegexOptions.Multiline);
            MatchCollection matchs = pattern.Matches(str);
            return matchs.Count > 0;
        }

        /// <summary>
        /// 根据给出的字串，判断编码是否有效
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsCanceled(string str)
        {
            if (str == null) return false;
            return str.Contains("THIS PNR WAS ENTIRELY CANCELLED");
        }
        
        public static string GetRtxItemPnrCode(string str)
        {
            if (str == null) return string.Empty;
            var pattern = new Regex(RegexUtil.GetRegexString("RtxItemPnrCode"));
            var match = pattern.Match(str);
            return match.Success ? match.Groups["PnrCode"].Value : null;
        }

        private static string GetRtItemPnrCode(string str)
        {
            if (str == null) return string.Empty;
            var pattern = new Regex(RegexUtil.GetRegexString("RtItemPnrCode"));
            var match = pattern.Match(str);
            return match.Success ? match.Groups["PnrCode"].Value : null;
        }
        
        private static PNRPair GetPnrPair(string str, string tempPnrCode)
        {
            // 这里暂时没有做判断；

            // 尝试匹配，大系统提取小编码时的，内容中的大编码信息；
            var rtxPnrCode = GetRtxItemPnrCode(str);
            string icsCode;
            string crsCode;
            if (!string.IsNullOrEmpty(rtxPnrCode))  // 若能提取，则说明刚提取到了大编码；
            {
                icsCode = tempPnrCode;
                crsCode = rtxPnrCode;
            }
            else
            {
                // 若匹配不到，则传入的大编，再次提取小编码；
                icsCode = GetRtItemPnrCode(str);
                crsCode = tempPnrCode;
            }

            if (!IsCrsCode(crsCode) && !IsIcsCode(icsCode))
            {
                return new PNRPair(icsCode, crsCode);
            }

            return new PNRPair(crsCode, icsCode);
        }

        /// <summary>
        /// 根据给出的字串，获取旅客订座记录取消时的旅客订座记录编码。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>旅客订座记录编码</returns>
        public static string GetCanceledPnrCode(string str)
        {
            if (str == null) return string.Empty;
            var pattern = new Regex(RegexUtil.GetRegexString("CancelledPnrString"));
            Match match = pattern.Match(str);
            return match.Success ? match.Groups["PnrCode"].Value : null;
        }

        /// <summary>
        /// 判断XEPNR是否取消成功；
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CanceledPnrSuccess(string str)
        {
            if (str == null) throw new ArgumentNullException("str");
            var pattern = new Regex(RegexUtil.GetRegexString("CancelledPnrString"));
            Match match = pattern.Match(str);
            return match.Success;
        }
        
        /// <summary>
        /// 根据给出的字串，获取订座（或操作，如取消旅客和航段）成功时的旅客订座记录编码。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>旅客订座记录编码</returns>
        public static string GetSuccessPnrCode(string str)
        {
            if (str == null) return string.Empty;
            var pattern = new Regex(RegexUtil.GetRegexString("SucceededPnrString"), RegexOptions.Multiline);
            Match match = pattern.Match(str);
            return match.Success ? match.Groups["PnrCode"].Value : null;
        }
        
        /// <summary>
        /// 根据给出的字串，获取价格信息。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>价格列表</returns>
        public static List<PriceView> GetPatPrices(string str)
        {
            str = RemoveAllReturnFlag(str);
            var result = new List<PriceView>();

            if (str == null) return result;
            var pattern = new Regex(RegexUtil.GetRegexString("PatResult"));
            var matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                foreach (Match match in matchCollection)
                {
                    decimal fare = decimal.Parse(match.Groups["Fare"].Value);
                    decimal arirportTax = string.IsNullOrEmpty(match.Groups["AirportTax"].Value)
                                              ? 0
                                              : decimal.Parse(match.Groups["AirportTax"].Value);
                    decimal bunkerAdjustmentFactor = string.IsNullOrEmpty(match.Groups["BunkerAdjustmentFactor"].Value)
                                                         ? 0
                                                         : decimal.Parse(match.Groups["BunkerAdjustmentFactor"].Value);
                    decimal total = decimal.Parse(match.Groups["Total"].Value);

                    var price = new PriceView
                                    {
                                        AirportTax = arirportTax,
                                        BunkerAdjustmentFactor = bunkerAdjustmentFactor,
                                        Fare = fare,
                                        Total = total
                                    };
                    result.Add(price);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据给出的字串，获取旅客订座信息中的姓名及代理人编号字串。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks>
        /// 这个单独的字串将用于解析出姓名项和代理人编号；
        /// </remarks>
        public static string GetNameAndOfficeNoString(string str)
        {
            if (str == null) return string.Empty;
            var pattern = new Regex(RegexUtil.GetRegexString("RtResultNameAndOfficeNo"), RegexOptions.Singleline);
            var match = pattern.Match(str);
            return match.Success ? match.Groups["NameAndOfficeNo"].Value : null;
        }

        /// <summary>
        /// 根据给出的字串，获取旅客订座记录中的姓名项信息。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>姓名项列表</returns>
        /// <remarks>
        /// 此处的待解析字串应为处理过的，只包含名称项（多数情况下带有代理人编号）的信息。
        /// </remarks>
        public static List<ReservedPassengerInfo> GetPassengers(string str)
        {
            var result = new List<ReservedPassengerInfo>();
            if (str == null) return result;
            var pattern = new Regex(RegexUtil.GetRegexString("NmResult"), RegexOptions.Multiline);
            var matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                foreach (Match item in matchCollection)
                {
                    var lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    var name = item.Groups["Name"].Value;
                    var type = (item.Groups["PassengerType"].Value == "CHD") ? PassengerType.Child : PassengerType.Adult;
                    var passenger = new ReservedPassengerInfo
                    {
                        LineNumber = lineNumber,
                        Name = name,
                        Type = type
                    };
                    result.Add(passenger);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据旅客订座记录，获取其航段信息
        /// </summary>
        /// <param name="str">旅客订座记录</param>
        /// <returns>航段列表</returns>
        public static List<ReservedSegmentInfo> GetSegements(string str)
        {
            var result = new List<ReservedSegmentInfo>();
            if (str == null) return result;
            var pattern = new Regex(RegexUtil.GetRegexString("SsResult"), RegexOptions.Multiline);
            MatchCollection matchCollection = pattern.Matches(str);
            if (matchCollection.Count > 0)
            {
                foreach (Match item in matchCollection)
                {
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);

                    bool isCodeShareFlight = item.Groups["IsCodeShareFlight"].Value == "*";
                    var flightNumber = new FlightNumber(item.Groups["Carrier"].Value,
                                                        item.Groups["InternalNumber"].Value);
                    string departureAirport = item.Groups["Departure"].Value;
                    string arrivalAirport = item.Groups["Arrival"].Value;
                    string terminalOfDeparture = item.Groups["TerminalOfDeparture"].Value;
                    string terminalOfArrival = item.Groups["TerminalOfArrival"].Value;
                    int addDays = string.IsNullOrWhiteSpace(item.Groups["AddDays"].Value)
                                      ? 0
                                      : int.Parse(item.Groups["AddDays"].Value);

                    string classOfService = item.Groups["Class"].Value + item.Groups["SubClass"].Value;
                    string seatStatus = item.Groups["SeatStatus"].Value;
                    bool isETicket = item.Groups["IsETicket"].Value == "E";
                    int seatings = int.Parse(item.Groups["Seatings"].Value);

                    // 航班日期
                    DateTime flightDate;

                    // 若年份信息存在，则处理。
                    if (string.IsNullOrEmpty(item.Groups["Year"].Value))
                    {
                        flightDate = DateTime.Parse(item.Groups["Day"].Value + item.Groups["Month"].Value,
                                                    new CultureInfo("en-US"));
                        // 处理跨年
                        if (flightDate < DateTime.Today)
                        {
                            flightDate = flightDate.AddYears(1);
                        }
                    }
                    else
                    {
                        flightDate =
                            DateTime.Parse(
                                item.Groups["Day"].Value + item.Groups["Month"].Value + item.Groups["Year"].Value,
                                new CultureInfo("en-US"));
                    }

                    DateTime departureDate =
                        flightDate.AddHours(int.Parse(item.Groups["DepartureHour"].Value)).AddMinutes(
                            int.Parse(item.Groups["DepartureMinute"].Value));
                    DateTime arrivalDate = flightDate.AddDays(addDays).AddHours(
                        int.Parse(item.Groups["ArrivalHour"].Value)).AddMinutes(
                            int.Parse(item.Groups["ArrivalMinute"].Value));

                    string extendedInfo = item.Groups["ExtendedInformation"].Value.Trim();
                    Match extendedMatch;
                    if (!string.IsNullOrEmpty(extendedInfo) &&
                        (extendedMatch = Regex.Match(extendedInfo, @"^(?<Class>[A-Z])(?<SubClass>\d)$")).Success)
                    {
                        classOfService = extendedMatch.Groups["Class"].Value + extendedMatch.Groups["SubClass"].Value;
                    }

                    var segment = new ReservedSegmentInfo
                                      {
                                          LineNumber = lineNumber,
                                          FlightNumber = flightNumber,
                                          ArrivalAirport = arrivalAirport,
                                          DepartureAirport = departureAirport,
                                          ArrivalDate = arrivalDate,
                                          DepartureDate = departureDate,
                                          FlightDate = flightDate,
                                          AddDays = addDays,
                                          TerminalOfArrival = terminalOfArrival,
                                          TerminalOfDeparture = terminalOfDeparture,
                                          ClassOfService = classOfService,
                                          IsCodeShareFlight = isCodeShareFlight,
                                          IsETicket = isETicket,
                                          SeatStatus = seatStatus,
                                          Seatings = seatings
                                      };
                    result.Add(segment);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据旅客订座记录内容，获取其航程信息。
        /// </summary>
        /// <param name="str">旅客订座记录</param>
        /// <returns>航程</returns>
        public static Voyage GetVoyage(string str)
        {
            var reservedSegments = GetSegements(str);
            var segments = (from s in reservedSegments
                            select
                                new Segment
                                {
                                    AddDays = s.AddDays,
                                    AirportPair = new AirportPair(s.DepartureAirport,
                                                                  s.ArrivalAirport),
                                    AirlineCode = s.FlightNumber.Carrier,
                                    InternalNo = s.FlightNumber.InternalNumber,
                                    TerminalOfArrival = s.TerminalOfArrival,
                                    TerminalOfDeparture = s.TerminalOfDeparture,
                                    Date = s.FlightDate,
                                    Status = s.SeatStatus,
                                    SeatCount = s.Seatings,
                                    ArrivalTime = new Time(s.ArrivalDate),
                                    DepartureTime = new Time(s.DepartureDate),
                                    CabinSeat = s.ClassOfService,
                                    IsETicket = s.IsETicket,
                                    IsShared = s.IsCodeShareFlight
                                }).ToList();
            return new Voyage(segments);
        }

        /// <summary>
        /// 根据预订后的旅客订座记录，获取其证件信息。
        /// </summary>
        /// <param name="str">旅客订座记录</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns></returns>
        /// <remarks>
        /// 2012-12-27 出现用户重复输入身份证号，导致linq在做连接时出现重复数据，添加去重；
        /// </remarks>
        private static List<ReservedCertificateInfo> GetCertificateNumbers(string str)
        {
            var result = new List<ReservedCertificateInfo>();
            if (str == null) return result;
            var pattern = new Regex(RegexUtil.GetRegexString("SsrResultFoid"), RegexOptions.Multiline);
            var matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                foreach (Match item in matchCollection)
                {
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    var passengerId = int.Parse(item.Groups["PassengerId"].Value);
                    CredentialsType certificateType;
                    switch (item.Groups["CertificateType"].Value)
                    {
                        case "NI":
                            certificateType = CredentialsType.身份证;
                            break;
                        case "PP":
                            certificateType = CredentialsType.护照;
                            break;
                        case "ID":
                            certificateType = CredentialsType.其他;
                            break;
                        default:
                            certificateType = CredentialsType.其他;
                            break;
                    }
                    string certificateNumber = item.Groups["CertificateNumber"].Value;
                    var reservedCertificateInfo = new ReservedCertificateInfo
                                                      {
                                                          LineNumber = lineNumber,
                                                          PassengerId = passengerId,
                                                          CertificateType = certificateType,
                                                          Number = certificateNumber
                                                      };
                    result.Add(reservedCertificateInfo);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据旅客订座记录，获取其票面价格信息。
        /// </summary>
        /// <param name="str">旅客订座记录</param>
        /// <returns></returns>
        /// <remarks>
        /// 2012-12-27 出现用户重复输入身份证号，导致linq在做连接时出现重复数据，添加去重；
        /// </remarks>
        private static List<ReservedContractInfo> GetContractInformations(string str)
        {
            var result = new List<ReservedContractInfo>();
            if (str == null) return result;
            var pattern = new Regex(RegexUtil.GetRegexString("OsiResultCtct"), RegexOptions.Multiline);
            var matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                var i = 1;
                foreach (Match item in matchCollection)
                {
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    // 防止用户不输入编号；
                    int passengerId = string.IsNullOrEmpty(item.Groups["PassengerId"].Value)
                                          ? i++
                                          : int.Parse(item.Groups["PassengerId"].Value);
                    string mobilePhoneNumber = item.Groups["MobilePhoneNumber"].Value;
                    var reservedContractInfo = new ReservedContractInfo
                                                   {
                                                       LineNumber = lineNumber,
                                                       PassengerId = passengerId,
                                                       Contract = mobilePhoneNumber
                                                   };
                    result.Add(reservedContractInfo);
                }
            }

            return result;
        }
        
        /// <summary>
        /// 根据给出的字串，获取被授权的代理人编号。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>被授权的代理人编号列表</returns>
        private static List<ReservedAuthorizeInfo> GetAuthorizes(string str)
        {
            var result = new List<ReservedAuthorizeInfo>();
            if (str == null) return result;
            var pattern = new Regex(RegexUtil.GetRegexString("RmkAuthResult"));
            var matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                foreach (Match item in matchCollection)
                {
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    string officeNo = item.Groups["OfficeNo"].Value;
                    var reservedAuthorizeInfo = new ReservedAuthorizeInfo()
                    {
                        LineNumber = lineNumber,
                        OfficeNo = officeNo
                    };
                    result.Add(reservedAuthorizeInfo);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据给出的字串，获取旅客订座记录中订座的代理人编号。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>代理人编号</returns>
        private static string GetOfficeNo(string str)
        {
            if (str == null) return string.Empty;

            var pattern = new Regex(RegexUtil.GetRegexString("RtResultOfficeNo"));
            var match = pattern.Match(str);
            var result = match.Groups["OfficeNo"].Value;

            if (string.IsNullOrEmpty(result))
            {
                pattern = new Regex(RegexUtil.GetRegexString("RtxItemPnrCode"));
                match = pattern.Match(str);
                result = match.Success ? match.Groups["OfficeNo"].Value : string.Empty;
            }
            
            return result;
        }
        
        /// <summary>
        /// 根据给出的字串，获取旅客订座记录中团队信息。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>团队信息</returns>
        /// <remarks>
        /// 2012-12-27 出现用户重复输入身份证号，导致linq在做连接时出现重复数据，添加去重；
        /// </remarks>
        public static List<ReservedChildInfo> GetChildrenInfo(string str)
        {
            var result = new List<ReservedChildInfo>();
            if (str == null) return result;
            var pattern = new Regex(RegexUtil.GetRegexString("SsrResultChld"), RegexOptions.Multiline);
            var matchCollection = pattern.Matches(str);
            if (matchCollection.Count > 0)
            {
                foreach (Match item in matchCollection)
                {
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    int passengerId = int.Parse(item.Groups["PassengerId"].Value);
                    string birthday = item.Groups["Day"].Value + item.Groups["Month"].Value + item.Groups["Year"].Value;
                    var reservedChildInfo = new ReservedChildInfo
                                                   {
                                                       LineNumber = lineNumber,
                                                       PassengerId = passengerId,
                                                       Birthday = birthday
                                                   };
                    result.Add(reservedChildInfo);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据给出的字串，获取旅客订座记录中的机场对。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>机场对列表</returns>
        private static List<ReservedFillAirports> GetFilledAirportPairs(string str)
        {
            var result = new List<ReservedFillAirports>();
            if (str == null) return result;
            
            var pattern = new Regex(RegexUtil.GetRegexString("SaResult"), RegexOptions.Multiline);
            var matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                foreach (Match item in matchCollection)
                {
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    string departure = item.Groups["DepartureAirport"].Value;
                    string arrival = item.Groups["ArrivalAirport"].Value;
                    var airport = new AirportPair(departure, arrival);
                    result.Add(new ReservedFillAirports
                                   {
                                       LineNumber = lineNumber,
                                       AirportPair = airport
                                   });
                }
            }

            return result;
        }

        /// <summary>
        /// 根据给出的字串，获取旅客订座记录中团队信息。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>团队信息</returns>
        private static ReservedTermInfo GetTermInformation(string str)
        {
            if (str == null) return null;
            var pattern = new Regex(RegexUtil.GetRegexString("RtResultTerm"));
            var match = pattern.Match(str);

            return match.Success
                       ? new ReservedTermInfo
                             {
                                 TotalNumber = int.Parse(match.Groups["TotalNumber"].Value),
                                 ActualNumber = int.Parse(match.Groups["ActualNumber"].Value),
                                 Name = match.Groups["Name"].Value,
                                 PnrCode = match.Groups["PnrCode"].Value
                             }
                       : null;
        }

        /// <summary>
        /// 根据给出的字串，获取旅客订座记录中姓名项后的旅客订座记录编号。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>旅客订座记录编号</returns>
        /// <remarks>
        /// 此处的待解析字串应为处理过的，只包含名称项（）的信息。
        /// 多数情况下带有代理人编号，在当为团队时或者只是NM返回时，将无旅客订座记录编号,此时返回值为空；
        /// </remarks>
        private static string GetNameItemPnrCode(string str)
        {
            if (str == null) return string.Empty;
            var pattern = new Regex(RegexUtil.GetRegexString("RtResultNameItemPnrCode"));
            var match = pattern.Match(str);
            return match.Success ? match.Groups["PnrCode"].Value : null;
        }

        /// <summary>
        /// 判断一个编码是否为C系统编码（根据票务经验做的判定）
        /// </summary>
        /// <param name="code">待测试字串</param>
        /// <returns>是否为C系统编码</returns>
        public static bool IsCrsCode(string code)
        {
            return !string.IsNullOrEmpty(code) && (code.StartsWith("H") || code.StartsWith("J"));
        }

        /// <summary>
        /// 判断一个编码是否为B系统编码（根据票务经验做的判定）
        /// </summary>
        /// <param name="code">待测试字串</param>
        /// <returns>是否为C系统编码</returns>
        public static bool IsIcsCode(string code)
        {
            return !string.IsNullOrEmpty(code) && (code.StartsWith("N") || code.StartsWith("M"));
        }

        public static PNRPair SwitchPnrPair(PNRPair pnrPair)
        {

            if (!IsCrsCode(pnrPair.PNR) && !IsIcsCode(pnrPair.BPNR))
            {
                return new PNRPair(pnrPair.BPNR, pnrPair.PNR);
            }

            return pnrPair;
        }

        ///// <summary>
        ///// 根据出票后的旅客订座记录，获取其票号信息。(暂时未使用)
        ///// </summary>
        ///// <param name="str">旅客订座记录</param>
        ///// <returns>票号</returns>
        ///// <remarks>
        ///// 键为旅客编号，值为票号
        ///// </remarks>
        //private static Dictionary<int, string> GetTicketNumbers(string str)
        //{
        //    var result = new Dictionary<int, string>();
        //    var pattern = new Regex(RegexUtil.GetRegexString("PnrTicketNumber"));
        //    var matchCollection = pattern.Matches(str);

        //    if (matchCollection.Count > 0)
        //    {
        //        foreach (Match item in matchCollection)
        //        {
        //            var key = int.Parse(item.Groups["PassengerId"].Value);
        //            var ticketNumber = item.Groups["TicketNumber"].Value;
        //            result.Add(key, ticketNumber);
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// 根据出票后的旅客订座记录，获取其票面价格信息。（暂时未使用）
        ///// </summary>
        ///// <param name="str">旅客订座记录</param>
        ///// <returns>票面价格列表</returns>
        ///// <remarks>
        ///// 键为旅客编号，值为票面价格
        ///// </remarks>
        //private static Dictionary<int, decimal> GetTicketPrices(string str)
        //{
        //    var result = new Dictionary<int, decimal>();
        //    var pattern = new Regex(RegexUtil.GetRegexString("PnrTicketPrice"));
        //    var matchCollection = pattern.Matches(str);

        //    if (matchCollection.Count > 0)
        //    {
        //        foreach (Match item in matchCollection)
        //        {
        //            var key = int.Parse(item.Groups["PassengerId"].Value);
        //            var price = decimal.Parse(item.Groups["Price"].Value);
        //            result.Add(key, price);
        //        }
        //    }

        //    return result;
        //}
        
        public static List<QueueSummary> GetMailList(string str)
        {
            var result = new List<QueueSummary>();
            if (str == null) return result;
            var pattern = new Regex(@"(?<Name>[A-Z]{2})\s(?<UnprocessedNumber>\d{4})\s(?<TotalNumber>\d{4})", RegexOptions.Multiline);
            var matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                foreach (Match item in matchCollection)
                {
                    var name = item.Groups["Name"].Value;
                    var unprocessedNumber = int.Parse(item.Groups["UnprocessedNumber"].Value);
                    var totalNumber = int.Parse(item.Groups["TotalNumber"].Value);
                    var queue = new QueueSummary(name, unprocessedNumber, totalNumber);
                    result.Add(queue);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据给出的字串，获取运价列表。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>运价列表</returns>
        /// <remarks>
        /// 注意，此时并未处理往返票价，包括正则也是。
        /// </remarks>
        public static AirportPairFares GetFare(string str)
        {
            if (str == null) throw new ArgumentNullException("str");

            var commandPattern = new Regex(RegexUtil.GetRegexString("FdCommand"), RegexOptions.Multiline);
            var match = commandPattern.Match(str);
            var pattern = new Regex(RegexUtil.GetRegexString("FdResult"), RegexOptions.Multiline);
            var matchCollection = pattern.Matches(str);

            if (match.Success && matchCollection.Count > 0)
            {
                var airportPair = new AirportPair(match.Groups["DepartureAirport"].Value, match.Groups["ArrivalAirport"].Value);
                var date = DateTime.Parse(match.Groups["Date"].Value, new CultureInfo("en-US"));
                var airline = match.Groups["Airline"].Value;
                var mileage = int.Parse(match.Groups["Mileage"].Value);
                var graduatedFareList = new List<GraduatedFare>();

                foreach (Match item in matchCollection)
                {
                    var classOfService = item.Groups["ServiceClass"].Value;
                    var apply = item.Groups["Apply"].Value;

                    decimal oneWayFare;
                    decimal roundTripFare;
                    ApplyType applyType;
                    if (apply == "OW")
                    {
                        applyType = ApplyType.OneWay;
                        oneWayFare = decimal.Parse(item.Groups["OneWayFare"].Value);
                        roundTripFare = 0;
                    }
                    else if (apply == "RT")
                    {
                        applyType = ApplyType.Roundtrip;
                        oneWayFare = 0M;
                        roundTripFare = decimal.Parse(item.Groups["RoundTripFare"].Value);
                    }
                    else
                    {
                        applyType = ApplyType.All;
                        oneWayFare = decimal.Parse(item.Groups["OneWayFare"].Value);
                        roundTripFare = decimal.Parse(item.Groups["RoundTripFare"].Value);
                    }

                    var subClass = item.Groups["SubClass"].Value; 
                    var effectiveDate = DateTime.Parse(item.Groups["EffectiveDate"].Value, new CultureInfo("en-US"));
                    var serviceType = item.Groups["Type"].Value;
                    DateTime? expiryDate = null;
                    if (!string.IsNullOrEmpty(item.Groups["ExpiryDate"].Value))
                    {
                        expiryDate = DateTime.Parse(item.Groups["ExpiryDate"].Value, new CultureInfo("en-US"));
                    }

                    var fare = new GraduatedFare
                    {
                        ClassOfService = classOfService,
                        SubClass = subClass,
                        OneWayFare = oneWayFare,
                        EffectiveDate = effectiveDate,
                        ExpiryDate = expiryDate,
                        RoundTripFare = roundTripFare,
                        ServiceType = serviceType,
                        ApplyType = applyType
                    };
                    graduatedFareList.Add(fare);
                }

                return new AirportPairFares
                             {
                                 Airline = airline,
                                 AirportPair = airportPair,
                                 Date = date,
                                 GraduatedFareList = graduatedFareList,
                                 Mileage = mileage
                             };
            }

            return null;
        }
        
        /// <summary>
        /// 根据给出的字串，获取电子客票信息。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>电子客票信息</returns>
        public static ElectronicTicket GetElectronicTicket(string str)
        {
            if (str == null) throw new ArgumentNullException("str");

            var name = GetElectronicTicketPassengerName(str);
            var ticketNumber = GetElectronicTicketNumber(str);
            var exchangeList = GetExchangeList(str);

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(ticketNumber) || exchangeList == null)
            {
                return null;
            }
            else
            {
                return new ElectronicTicket()
                           {
                               ExchangeList = exchangeList,
                               Name = name,
                               TicketNumber = ticketNumber
                           };
            }
        }
        
        /// <summary>
        /// 根据给出的字串，获取到其中的电子客票兑换信息。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>电子客票兑换信息</returns>
        private static ExchangeList GetExchangeList(string str)
        {
            if (str == null) throw new ArgumentNullException("str");

            var fromAirports = new List<ExchangeDetail>();
            var toAirport = string.Empty;

            // 获取起点信息；
            var fromPattern = new Regex(RegexUtil.GetRegexString("DetrResultExchangeFrom"), RegexOptions.Multiline|RegexOptions.Singleline);
            var fromMatchCollection = fromPattern.Matches(str);
            foreach (Match item in fromMatchCollection)
            {
                var airport = item.Groups["Airport"].Value;
                var flightNumber = new FlightNumber(item.Groups["Carrier"].Value,
                                                    item.Groups["InternalNumber"].Value);
                var classOfService = item.Groups["ClassOfService"].Value;

                DateTime departureTime= default(DateTime);
                if (!item.Groups["Date"].Value.Contains("OPEN"))
                {
                    departureTime = DateTime.Parse(item.Groups["Day"].Value + item.Groups["Month"].Value,
                                                   new CultureInfo("en-US"));
                    departureTime = departureTime.AddHours(int.Parse(item.Groups["Hour"].Value)).AddMinutes(
                                int.Parse(item.Groups["Minute"].Value));
                }

                PNRPair pnrPair = null;
                if (!string.IsNullOrEmpty(item.Groups["PnrPair"].Value))
                {
                    pnrPair = new PNRPair(item.Groups["CrsPnrCode"].Value, item.Groups["IcsPnrCode"].Value);
                }

                var status = item.Groups["Status"].Value;

                fromAirports.Add(new ExchangeDetail
                             {
                                 Airport = airport,
                                 ClassOfService = classOfService,
                                 FlightNumber = flightNumber,
                                 DepartureTime = departureTime,
                                 PnrPair = pnrPair,
                                 Status = status
                             });
            }
            // 获取终点信息；
            var toPattern = new Regex(RegexUtil.GetRegexString("DetrResultExchangeTo"), RegexOptions.Multiline);
            var toMatch = toPattern.Match(str);
            if (toMatch.Success)
            {
                toAirport = toMatch.Groups["Airport"].Value;
            }
            
            // 根据获取的数据，得到返回值；
            if (fromAirports.Count == 2 && !string.IsNullOrEmpty(toAirport))
            {
                return new ExchangeList()
                           {
                               FirstStop = fromAirports[0],
                               SecondStop = fromAirports[1],
                               ThirdStop = toAirport
                           };
            }
            else if (fromAirports.Count == 1 && !string.IsNullOrEmpty(toAirport))
            {
                return new ExchangeList()
                {
                    FirstStop = fromAirports[0],
                    ThirdStop = toAirport
                };
            }
            else
            {
                return null;
            }
        }

        private static string GetElectronicTicketNumber(string str)
        {
            if (str == null) throw new ArgumentNullException("str");
            var pattern = new Regex(RegexUtil.GetRegexString("DetrResultTicketNumber"));
            var match = pattern.Match(str);
            return match.Success ? match.Groups["TicketNumber"].Value : string.Empty;
        }

        private static string GetElectronicTicketPassengerName(string str)
        {
            if (str == null) throw new ArgumentNullException("str");
            var pattern = new Regex(RegexUtil.GetRegexString("DetrResultName"));
            var match = pattern.Match(str);
            return match.Success ? match.Groups["Name"].Value : string.Empty;
        }
        
        /// <summary>
        /// 根据给出的字串，获取电子客票行程单信息。
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>行程单信息</returns>
        public  static  JourneySheet GetJourneySheet(string str)
        {
            if (str == null) throw new ArgumentNullException("str");

            var name = GetJourneySheetPassengerName(str);
            var numberInfo = GetJourneySheetNumber(str);
            var ticketNumber = GetJourneySheetTicketNumber(str);

            if (string.IsNullOrEmpty(ticketNumber))
            {
                return null;
            }
            else
            {
                if (numberInfo != null)
                {
                    var status = numberInfo.Status == "XX" ? JourneySheetStatus.Canceled : JourneySheetStatus.Used; 
                    return new JourneySheet()
                    {
                        TicketNumber = ticketNumber,
                        Name = name,
                        Number = numberInfo.Number,
                        Staus = status
                    };
                }
                else
                {
                    return new JourneySheet()
                    {
                        TicketNumber = ticketNumber,
                        Name = name,
                        Staus = JourneySheetStatus.NotUsed
                    };
                }
            }
        }
        
        /// <summary>
        /// 获取旅客姓名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string GetJourneySheetPassengerName(string str)
        {
            if (str == null) throw new ArgumentNullException("str");
            var pattern = new Regex(RegexUtil.GetRegexString("DetrfResultName"));
            var match = pattern.Match(str);
            return match.Success ? match.Groups["Name"].Value : string.Empty;
        }

        /// <summary>
        /// 获取旅客行程单单号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static JourneySheetNumberInfo GetJourneySheetNumber(string str)
        {
            if (str == null) throw new ArgumentNullException("str");
            var pattern = new Regex(RegexUtil.GetRegexString("DetrfResultNumber"));
            var match = pattern.Match(str);
            if (match.Success)
            {
                var status = match.Groups["Status"].Value;
                var number = match.Groups["Number"].Value;
                return new JourneySheetNumberInfo()
                {
                    Number = number,
                    Status = status
                };
            }
            else
            {
                return null;
            }

        }
        
        /// <summary>
        /// 获取旅客行程单票号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string GetJourneySheetTicketNumber(string str)
        {
            if (str == null) throw new ArgumentNullException("str");
            var pattern = new Regex(RegexUtil.GetRegexString("DetrfResultTicketNumber"));
            var match = pattern.Match(str);
            return match.Success ? match.Groups["TicketNumber"].Value : string.Empty;
        }
    }
}
