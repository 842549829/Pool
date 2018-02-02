using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Command.Domain.FlightQuery;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.Core;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using Passenger = ChinaPay.B3B.Service.Command.Domain.PNR.Passenger;

namespace ChinaPay.B3B.Service.Command
{
    /// <summary>
    /// 指令结果转换类
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// 判断RRT的执行结果；
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool GetRRTResult(string str)
        {
            // 已经有小编或无此编码，
            return str.Contains("PNR ALREADY ON THIS SYSTEM") || str.Contains("NO PNR");            
        }
        
        /// <summary>
        /// 根据旅客订座记录内容，获取此内容中的编码是否被取消。
        /// </summary>
        /// <param name="pnrContent">旅客订座记录内容</param>
        /// <returns>是否取消</returns>
        public static bool IsCanceledPNR(string pnrContent)
        {
            return pnrContent.Contains("THIS PNR WAS ENTIRELY CANCELLED");
        }

        /// <summary>
        /// 根据旅客订座记录内容，获取编码信息。
        /// </summary>
        /// <param name="pnrContent">旅客订座记录内容</param>
        /// <returns>编码信息</returns>
        public static PNRPair GetPnrPair(string pnrContent)
        {
            Regex pattern = null;
            Match match = null;
            string pnrCode = null;
            string bPnrCode = null;
            string cPnrCode = null;

            switch (GetPassengerConsistsType(pnrContent))
            {
                case PassengerConsistsType.Individual:
                    pattern = new Regex(RegexUtil.CodeLineRegex);
                    match = pattern.Match(pnrContent);
                    string tempStr = match.Captures[0].Value;
                    pattern = new Regex(@"(?<PNRCode>[A-Z0-9]{6})(?<Flag>/[A-Z0-9]{2})?\s*$");
                    match = pattern.Match(tempStr);
                    if (match.Success)
                    {
                        pnrCode = match.Groups["PNRCode"].Value;
                    }
                    break;
                case PassengerConsistsType.Group:
                    pattern = new Regex(RegexUtil.TeamRegex);
                    match = pattern.Match(pnrContent);
                    if (match.Success)
                    {
                        pnrCode = match.Groups["PNRCode"].Value;
                    }
                    break;
                default:
                    break;
            }
            
            // B系统提小编码测试；
            pattern = new Regex(RegexUtil.RTXCmdPNRCode);
            match = pattern.Match(pnrContent);
            if (match.Success)
            {
                // 如果通过，说明是B系统，则第一行为大编码，此次提出来的是小编码;
                bPnrCode = pnrCode;
                cPnrCode = match.Groups["PNRCode"].Value;
            }
            else
            {
                // 如果未通过，说明是C系统，则第一行为小编码，
                cPnrCode = pnrCode;
                // 同时提取大编码信息；
                pattern = new Regex(@"(?<LineNumber>[\s\d]\d)\.RMK\sCA/(?<PNRCode>[A-Z0-9]{6})");
                match = pattern.Match(pnrContent);
                if (match.Success)
                {
                    bPnrCode = match.Groups["PNRCode"].Value;
                }
            }

            // 只要当两个编码都是错的时候，才交换两者顺序；
            if (!IsCrsCode(cPnrCode) && !IsIcsCode(bPnrCode))
            {
                return new PNRPair(bPnrCode, cPnrCode);
            }
            else
            {
                return new PNRPair(cPnrCode, bPnrCode);
            }
        }

        /// <summary>
        /// 根据旅客订座记录内容，获取此内容中的旅客组成类型。
        /// </summary>
        /// <param name="pnrContent">旅客订座记录内容</param>
        /// <returns>旅客组成类型</returns>
        public static PassengerConsistsType GetPassengerConsistsType(string pnrContent)
        {
            Regex pattern = new Regex(RegexUtil.TeamRegex);
            Match match = pattern.Match(pnrContent);
            if (match.Success)
            {
                return PassengerConsistsType.Group;
            }
            else
            {
                return PassengerConsistsType.Individual;
            }
        }

        /// <summary>
        /// 根据给出经停点文本内容，获取经停点列表。
        /// </summary>
        /// <param name="str">文本内容</param>
        /// <returns>经停点列表</returns>
        public static IEnumerable<TransitPoint> GetTransitPoints(string str)
        {
            List<TransitPoint> result = default(List<TransitPoint>);

            Regex pattern = new Regex(TransitPoint.FormatString);
            MatchCollection matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                result = new List<TransitPoint>();
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
        
        ///// <summary>
        ///// 返回被授权的Office号，现在暂时没有用到，因为封口后都只给出pnrcode；
        ///// </summary>
        ///// <param name="str">待解析字串</param>
        ///// <returns>被授权的Office号</returns>
        //private static string GetAuthorizeInfo(string str)
        //{
        //    string officeNo = null;
        //    Regex pattern = new Regex(RegexUtil.RMKCmdRegex);
        //    Match match = pattern.Match(str);
        //    if (match.Success)
        //    {
        //        officeNo = match.Groups["OfficeNo"].Value;
        //    }
        //    return officeNo;
        //}
        
        public static TicketView GetTickets(string str)
        {
            TicketView ticket = new TicketView();
            Regex pattern = null;
            Match match = null;

            pattern = new Regex(ETicketUtil.ETicketNumberRegExp);
            match = pattern.Match(str);
            if (match.Success)
            {
                ticket.Number = match.Groups["TicketNumber"].Value;
            }

            pattern = new Regex(ETicketUtil.PassangerRegExp);
            match = pattern.Match(str);
            if (match.Success)
            {
                ticket.Name = match.Groups["Name"].Value;
            }

            pattern = new Regex(ETicketUtil.PNRPairRegExp);
            match = pattern.Match(str);
            if (match.Success)
            {
                ticket.PNRCode = new PNRPair(match.Groups["CPNR"].Value, match.Groups["BPNR"].Value);
            }

            pattern = new Regex(ETicketUtil.StatusRegExp);
            match = pattern.Match(str);
            if (match.Success)
            {
                ticket.Status = match.Groups["Status"].Value;
            }

            pattern = new Regex(ETicketUtil.AirlineCompanyRegExp);
            match = pattern.Match(str);
            if (match.Success)
            {
                ticket.AirlineCompany = match.Groups["AirlineCompany"].Value.Trim();
            }
            
            return ticket;
        }
        
        /// <summary>
        /// 根据PAT文本内容，获取价格信息。
        /// </summary>
        /// <param name="str">文本内容</param>
        /// <returns>价格列表</returns>
        internal static List<PriceView> GetPNRPrices(string str)
        {
            // 如果中间有换行，这里将无法处理，所以再替换一下，把所有的分号再替换回来；
            str = Regex.Replace(str, ";", "");

            List<PriceView> prices = null;
            Regex pattern = new Regex(RegexUtil.PATCmdRegex);
            MatchCollection matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                prices = new List<PriceView>();
                PriceView price = null;
                foreach (Match match in matchCollection)
                {
                    price = new PriceView();
                    price.Fare = decimal.Parse(match.Groups["Fare"].Value);
                    if (!string.IsNullOrEmpty(match.Groups["AirportTax"].Value))
                    {
                        price.AirportTax = decimal.Parse(match.Groups["AirportTax"].Value);
                    }
                    else
                    {
                        price.AirportTax = 0;
                    }
                    
                    price.BunkerAdjustmentFactor = decimal.Parse(match.Groups["BunkerAdjustmentFactor"].Value);
                    price.Total = decimal.Parse(match.Groups["Total"].Value);
                    prices.Add(price);
                }
            }

            return prices;
        }

        /// <summary>
        /// 得到订座成功的后的旅客订座记录编号
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>旅客订座记录编号</returns>
        public static PNRPair GetSuccessPNRCode(string str)
        {
            PNRPair pnrPair = null;
            Regex pattern = new Regex(RegexUtil.EOTCmdRegex);
            Match match = pattern.Match(str);

            if (match.Success)
            {
                pnrPair = new PNRPair(match.Groups["PNRCode"].Value, null);
            }

            return pnrPair;
        }

        /// <summary>
        /// 得到被取消的旅客订座记录编号
        /// </summary>
        /// <param name="str">待解析字串</param>
        /// <returns>旅客订座记录编号</returns>
        public static PNRPair GetCanceledPNRCode(string str)
        {
            PNRPair pnrPair = null;
            Regex pattern = new Regex(RegexUtil.XEPNRCmdRegex);
            Match match = pattern.Match(str);

            if (match.Success)
            {
                pnrPair = new PNRPair(match.Groups["PNRCode"].Value, null);
            }

            return pnrPair;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pnrContent"></param>
        /// <returns></returns>
        public static IssuedPNR GetPNRDetail(string pnrContent)
        {
            IssuedPNR issuedPNR = null;
            string tempStr = null;

            //将已经出过票的信息出票信息替换掉；
            pnrContent = pnrContent.Replace("  **ELECTRONIC TICKET PNR** ;", "");            

            // 取得第一行；
            Regex pattern = new Regex(RegexUtil.CodeLineRegex);
            Match match = pattern.Match(pnrContent);
            if (!match.Success)
            {
                if (IsCanceledPNR(pnrContent))
                {
                    issuedPNR = new IssuedPNR();
                    issuedPNR.IsCanceled = true;
                }
                return issuedPNR;
            }
            else
	        {
                issuedPNR = new IssuedPNR();
                issuedPNR.IsCanceled = false;
                tempStr = match.Captures[0].Value;
	        }
            
            issuedPNR.IsTeam = GetPassengerConsistsType(pnrContent) == PassengerConsistsType.Group;
            if (issuedPNR.IsTeam)
            {
                pattern = new Regex(RegexUtil.TeamRegex);
                match = pattern.Match(pnrContent);
                issuedPNR.NumberOfTerm = int.Parse(match.Groups["ActualNumber"].Value);
            }
            else
            {
                // 如果是非团队，去空格后去掉最后的编码部分；
                tempStr = tempStr.Trim();
                tempStr = Regex.Replace(tempStr, RegexUtil.PNRCodeRegex + @"$", "");
            }

            issuedPNR.Code = GetPnrPair(pnrContent);

            #region 名称组
            pattern = new Regex(RegexUtil.NMCmdRegex);
            tempStr = Regex.Replace(tempStr, @"(\s{1,})?;", "");
            MatchCollection matchCollection = pattern.Matches(tempStr);
            if (matchCollection.Count > 0)
            {
                issuedPNR.Passenges = new Dictionary<int, Passenger>();
                foreach (Match item in matchCollection)
                {
                    Passenger passenger = new Passenger();
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    passenger.Name = item.Groups["Name"].Value;
                    passenger.Type = (item.Groups["PassengerType"].Value == "CHD") ? PassengerType.Child : PassengerType.Adult;
                    issuedPNR.Passenges.Add(lineNumber, passenger);
                }
            }
            else
            {
                throw new CustomException("编码内容错误，姓名项不存在！");
            }
            #endregion

            #region 航段
            pattern = new Regex(RegexUtil.SSCmdRegex);
            matchCollection = pattern.Matches(pnrContent);
            if (matchCollection.Count > 0)
            {
                issuedPNR.Segments = new Dictionary<int, Segment>();

                foreach (Match item in matchCollection)
                {
                    int id = int.Parse(item.Groups["LineNumber"].Value);
                    Segment segment = new Segment();
                    segment.AirlineCode = item.Groups["AirlineCode"].Value;
                    segment.InternalNo = item.Groups["FlightNo"].Value;
                    segment.CabinSeat = item.Groups["CabinSeat"].Value;
                    segment.AirportPair = new AirportPair(item.Groups["Departure"].Value, item.Groups["Arrival"].Value);
                    segment.Date = DateTime.Parse(item.Groups["Day"].Value + item.Groups["Month"].Value, new CultureInfo("en-US"));
                    segment.Status = item.Groups["SeatStatus"].Value;
                    segment.SeatCount = int.Parse(item.Groups["SeatCount"].Value);
                    segment.IsETicket = (item.Groups["ETicketFlag"].Value == "E");
                    segment.AircraftType = item.Groups["AircraftType"].Value;
                    segment.IsShared = (item.Groups["IsShared"].Value == "*");
                    segment.TerminalOfArrival = item.Groups["TerminalOfArrival"].Value;
                    segment.TerminalOfDeparture = item.Groups["TerminalOfDeparture"].Value;

                    string extendedInfo = item.Groups["ExtendedInformation"].Value;

                    //转出来的日期，默认是当年的，如果跨年，则加一；
                    if (segment.Date < DateTime.Today)
                    {
                        segment.Date = segment.Date.AddYears(1);
                    }

                    //出发时间有可能为OPEN，此时时间不赋值；
                    string departureTime = item.Groups["DepartureTime"].Value;
                    if (departureTime != "OPEN")
                    {
                        int hour = int.Parse(departureTime.Substring(0, 2));
                        int minute = int.Parse(departureTime.Substring(2, 2));
                        segment.DepartureTime = new Time(hour, minute);
                    }
                    // 到达时间有可能为OPEN，此时时间不赋值，同时注意，到达时间有可能跨天；
                    string ArrivalTime = item.Groups["ArrivalTime"].Value;
                    //先取下是否跨天；
                    string days = item.Groups["AddDays"].Value;
                    segment.AddDays = string.IsNullOrWhiteSpace(days) ? 0 : int.Parse(days);
                    if (departureTime != "OPEN")
                    {
                        int hour = int.Parse(ArrivalTime.Substring(0, 2));
                        int minite = int.Parse(ArrivalTime.Substring(2, 2));
                        segment.ArrivalTime = new Time(hour, minite);
                    }

                    // 对扩展信息的处理，没有用正则；
                    // 2.  8L9933 M   WE14NOV  KMGLUM HK1   1310 1405      E      M1
                    // 6. *MU9287 V   TH22NOV  TSNKMG RR5   1710 2030      E      OP-FM9287
                    Match extendedMatch;
                    if (!string.IsNullOrEmpty(extendedInfo.Trim()) && (extendedMatch = Regex.Match(extendedInfo.Trim(), @"^(?<CabinSeat>[A-Z]\d)$")).Success)
                    {
                        segment.CabinSeat = extendedMatch.Groups["CabinSeat"].Value;
                    }

                    issuedPNR.Segments.Add(id, segment);
                }
            }
            #endregion
            
            #region 行程类型
            // 获取一个按照飞行时间排序的列表
            var voyages = issuedPNR.Segments.OrderBy(s => s.Value.Date).Select(s => s.Value.AirportPair).ToList();

            issuedPNR.ItineraryType = GetItineraryType(voyages);
            #endregion

            #region 缺口程搭桥信息处理
            if (issuedPNR.ItineraryType == ItineraryType.Notch)
            {
                // 生成一个包含搭桥信息的键值列表；
                Dictionary<int, AirportPair> temp = issuedPNR.Segments.OrderBy(s => s.Value.Date).Select(s => s).ToDictionary(s => s.Key, s => s.Value.AirportPair);
                pattern = new Regex(RegexUtil.SARegex);
                matchCollection = pattern.Matches(pnrContent);
                foreach (Match item in matchCollection)
                {
                    int key = int.Parse(item.Groups["LineNumber"].Value);
                    AirportPair airportPair = new AirportPair(item.Groups["Departure"].Value, item.Groups["Arrival"].Value);
                    temp.Add(key, airportPair);
                }
                voyages = temp.OrderBy(s => s.Key).Select(s => s.Value).ToList();

                // 判断搭桥后的行程是否连续；
                issuedPNR.IsFilled = IsContinuousVoyages(voyages);
            }
            else
            {
                issuedPNR.IsFilled = false;
            }
            #endregion
            
            #region 票号
            pattern = new Regex(RegexUtil.PNRTicketsRegex);
            matchCollection = pattern.Matches(pnrContent);
            if (matchCollection.Count > 0)
            {
                issuedPNR.TicketNumbers = new Dictionary<int, string>();

                foreach (Match item in matchCollection)
                {
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    int key = int.Parse(item.Groups["PassengerId"].Value);
                    string ticketNumber = item.Groups["TicketNumber"].Value;
                    if (issuedPNR.Passenges[key].TicketNumbers == null)
                    {
                        issuedPNR.Passenges[key].TicketNumbers = new List<string>();
                    }
                    issuedPNR.Passenges[key].TicketNumbers.Add(ticketNumber);
                    issuedPNR.TicketNumbers.Add(lineNumber, ticketNumber);
                }
            } 
            #endregion

            #region 特殊信息
            pattern = new Regex(RegexUtil.SSRCmdRegex);
            matchCollection = pattern.Matches(pnrContent);
            if (matchCollection.Count > 0)
            {
                issuedPNR.CertificateNumbers = new Dictionary<int, string>();
                foreach (Match item in matchCollection)
                {
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    int key = int.Parse(item.Groups["PassengerId"].Value);
                    string certificateNumber = item.Groups["CertificateNumber"].Value;
                    issuedPNR.Passenges[key].CertificateNumber = certificateNumber;
                    issuedPNR.CertificateNumbers.Add(lineNumber, certificateNumber);
                }
            }

            // 南航儿童票处理
            pattern = new Regex(RegexUtil.CZChildSSRRegex);
            matchCollection = pattern.Matches(pnrContent);
            if (issuedPNR.Segments.First().Value.AirlineCode == "CZ" && matchCollection.Count > 0)
            {
                foreach (Match item in matchCollection)
                {
                    int key = int.Parse(item.Groups["PassengerId"].Value);
                    issuedPNR.Passenges[key].Type = PassengerType.Child;
                }
            }


            #endregion
            
            #region 票价组
            pattern = new Regex(RegexUtil.PriceRegExp);
            match = pattern.Match(pnrContent);
            if (match.Success)
            {
                issuedPNR.Price = new PriceView();
                issuedPNR.Price.Fare = decimal.Parse(match.Groups["Fare"].Value);
                if (!string.IsNullOrEmpty(match.Groups["AirportTax"].Value))
                {
                    issuedPNR.Price.AirportTax = decimal.Parse(match.Groups["AirportTax"].Value);
                }
                else
                {
                    issuedPNR.Price.AirportTax = 0;
                }
                
                issuedPNR.Price.BunkerAdjustmentFactor = decimal.Parse(match.Groups["BunkerAdjustmentFactor"].Value);
                issuedPNR.Price.Total = decimal.Parse(match.Groups["Total"].Value);
            } 
            #endregion

            #region 联系组
            pattern = new Regex(RegexUtil.CTCmdRegex);
            matchCollection = pattern.Matches(pnrContent);
            if (matchCollection.Count > 0)
            {
                issuedPNR.ContractInformations = new Dictionary<int, string>();
                foreach (Match item in matchCollection)
                {
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    string phoneNumber = item.Groups["PhoneNumber"].Value;
                    issuedPNR.ContractInformations.Add(lineNumber, phoneNumber);
                }
            } 
            #endregion

            #region 特殊服务组
            pattern = new Regex(RegexUtil.OSICmdRegex);
            matchCollection = pattern.Matches(pnrContent);
            if (matchCollection.Count > 0)
            {
                int ctctCount = 0;
                foreach (Match item in matchCollection)
                {
                    if (item.Groups["OSIType"].Value == "CTCT")
                    {
                        ctctCount++;
                        // 这个是防止用户多输内容
                        if (ctctCount > issuedPNR.Passenges.Count)
                        {
                            break;
                        }
                        int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                        string mobilephone = item.Groups["PassengerInformation"].Value;
                        //有可能操作时不书写用户对应编号，即如下格式：
                        //7.OSI MU CTCT 13529033863，而非7.OSI MU CTCT 13529033863/P1
                        if (!string.IsNullOrEmpty(item.Groups["PassengerId"].Value))
                        {
                            int key = int.Parse(item.Groups["PassengerId"].Value);
                            issuedPNR.Passenges[key].Mobilephone = mobilephone;
                        }
                        else
                        {
                            //给默认值，以循环计数器为默认值,即依次赋值；
                            issuedPNR.Passenges[ctctCount].Mobilephone = mobilephone;
                        }
                    }
                    else if (item.Groups["OSIType"].Value == "CTC")
                    {
                        int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                        string phoneNumber = item.Groups["PassengerInformation"].Value;

                        if (issuedPNR.ContractInformations == null)
                        {
                            issuedPNR.ContractInformations = new Dictionary<int, string>();
                        }

                        issuedPNR.ContractInformations.Add(lineNumber, phoneNumber);
                    }
                    else
                    {

                    }
                }
            } 
            #endregion

            return issuedPNR;
        }

        /// <summary>
        /// 获取航班信息（已无引用，废弃）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public static List<Flight> GetFlights(string str)
        //{
        //    List<Flight> flights = null;
        //    DateTime date = default(DateTime);

        //    // 取得日期：
        //    Regex pattern = new Regex(@"(?<Day>[0-3]\d)(?<Month>JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)");
        //    Match match = pattern.Match(str);
        //    if (!match.Success)
        //    {
        //        return flights;
        //    }
        //    else
        //    {
        //        date = DateTime.Parse(match.Captures[0].Value, new CultureInfo("en-US"));
        //    }
            
        //    //转出来的日期，默认是当年的，如果跨年，则加一；
        //    if (date.DayOfYear < DateTime.Now.DayOfYear)
        //    {
        //        date = date.Date.AddYears(1);
        //    }

        //    // 取得航班信息；
        //    pattern = new Regex(RegexUtil.AVHCmdRegex); 
        //    MatchCollection matchCollection = pattern.Matches(str);
        //    if (matchCollection.Count > 0)
        //    {
        //         flights = new List<Flight>();
        //        foreach (Match item in matchCollection)
        //        {
        //            Flight flight = new Flight();
        //            flight.Number = new FlightNumber(item.Groups["AirlineCode"].Value, item.Groups["FlightNo"].Value);
        //            flight.Departure = item.Groups["Departure"].Value;
        //            flight.TerminalOfDeparture = item.Groups["TerminalOfDeparture"].Value;
        //            if (flight.TerminalOfDeparture == "--")
        //            {
        //                flight.TerminalOfDeparture = "";
        //            }
        //            flight.Arrival = item.Groups["Arrival"].Value;

        //            flight.TerminalOfArrival = item.Groups["TerminalOfArrival"].Value;
        //            if (flight.TerminalOfArrival == "--")
        //            {
        //                flight.TerminalOfArrival = "";
        //            }
        //            //flight.FlightDate = date;
        //            //flight.TakeoffTime = new Time(int.Parse(item.Groups["DepartureHour"].Value), int.Parse(item.Groups["DepartureMinute"].Value));
        //            //flight.LandingTime = new Time(int.Parse(item.Groups["ArrivalHour"].Value), int.Parse(item.Groups["ArrivalMinute"].Value));
                    
        //            // 处理跨天；
        //            string days = item.Groups["AddDays"].Value;
        //            flight.AddDays = string.IsNullOrWhiteSpace(days) ? 0 : int.Parse(days);

        //            flight.AirCraft = flight.AirCraft = item.Groups["AircraftType"].Value;
        //            flight.HasFood = (item.Groups["Meal"].Value != null);
        //            //flight.IsStop = int.Parse(item.Groups["TransitPoint"].Value) > 0;
        //            flight.ShareFlightNo = item.Groups["ShareAirlineCode"].Value + item.Groups["ShareFlightNo"].Value;
                    
        //            List<Bunk> bunks = new List<Bunk>();
        //            foreach (Capture capture in item.Groups["Bunks"].Captures)
        //            {
        //                bunks.Add(Bunk.Parse(capture.Value));
        //            }

        //            flight.Bunks = bunks;

        //            flights.Add(flight);
        //        }
        //    }
        //    return flights;
        //}

        /// <summary>
        /// 获取身份证信息；
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetCertificateNumbers(string str)
        {
            Dictionary<int, string> certificateNumbers = null;
            Regex pattern = new Regex(RegexUtil.SSRCmdRegex);
            MatchCollection matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                certificateNumbers = new Dictionary<int, string>();
                foreach (Match item in matchCollection)
                {
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    string certificateNumber = item.Groups["CertificateNumber"].Value;
                    certificateNumbers.Add(lineNumber, certificateNumber);
                }
            }
            return certificateNumbers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetContractInformations(string str)
        {
            Dictionary<int, string> contractInformations = null;
            Regex pattern = new Regex(RegexUtil.CTCmdRegex);
            MatchCollection matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                contractInformations = new Dictionary<int, string>();
                foreach (Match item in matchCollection)
                {
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    string phoneNumber = item.Groups["PhoneNumber"].Value;
                    contractInformations.Add(lineNumber, phoneNumber);
                }
            }
            return contractInformations;
        }

        /// <summary>
        /// 得到航段信息
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>航段信息</returns>
        public static Dictionary<int, Segment> GetSegments(string str)
        {
            Dictionary<int, Segment> segments = null;
            Regex pattern = new Regex(RegexUtil.SSCmdRegex);
            MatchCollection matchCollection = pattern.Matches(str);

            if (matchCollection.Count > 0)
            {
                segments = new Dictionary<int, Segment>();

                foreach (Match item in matchCollection)
                {
                    int id = int.Parse(item.Groups["LineNumber"].Value);
                    Segment segment = new Segment();
                    segment.AirlineCode = item.Groups["AirlineCode"].Value;
                    segment.InternalNo = item.Groups["FlightNo"].Value;
                    segment.CabinSeat = item.Groups["CabinSeat"].Value;
                    segment.AirportPair = new AirportPair(item.Groups["Departure"].Value, item.Groups["Arrival"].Value);
                    segment.Date = DateTime.Parse(item.Groups["Day"].Value + item.Groups["Month"].Value, new CultureInfo("en-US"));
                    segment.Status = item.Groups["SeatStatus"].Value;
                    segment.SeatCount = int.Parse(item.Groups["SeatCount"].Value);
                    segment.IsETicket = (item.Groups["ETicketFlag"].Value == "E");
                    segment.AircraftType = item.Groups["AircraftType"].Value;
                    segment.IsShared = (item.Groups["IsShared"].Value == "*");
                    segment.TerminalOfArrival = item.Groups["TerminalOfArrival"].Value;
                    segment.TerminalOfDeparture = item.Groups["TerminalOfDeparture"].Value;

                    //转出来的日期，默认是当年的，如果跨年，则加一；
                    if (segment.Date < DateTime.Today)
                    {
                        segment.Date = segment.Date.AddYears(1);
                    }

                    //出发时间有可能为OPEN，此时时间不赋值；
                    string departureTime = item.Groups["DepartureTime"].Value;
                    if (departureTime != "OPEN")
                    {
                        int hour = int.Parse(departureTime.Substring(0, 2));
                        int minute = int.Parse(departureTime.Substring(2, 2));
                        segment.DepartureTime = new Time(hour, minute);
                    }
                    // 到达时间有可能为OPEN，此时时间不赋值，同时注意，到达时间有可能跨天；
                    string ArrivalTime = item.Groups["ArrivalTime"].Value;
                    //先取下是否跨天；
                    string days = item.Groups["AddDays"].Value;
                    segment.AddDays = string.IsNullOrWhiteSpace(days) ? 0 : int.Parse(days);
                    if (departureTime != "OPEN")
                    {
                        int hour = int.Parse(ArrivalTime.Substring(0, 2));
                        int minite = int.Parse(ArrivalTime.Substring(2, 2));
                        segment.ArrivalTime = new Time(hour, minite);
                    }
                    segments.Add(id, segment);
                }
            }
            return segments;
        }

        /// <summary>
        /// 返回姓名信息
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetNames(string str)
        {
            Dictionary<int, string> names = null;
            // 取得第一行；
            Regex pattern = new Regex(RegexUtil.CodeLineRegex);
            Match match = pattern.Match(str);
            if (!match.Success)
            {
                return names;
            }

            string nameStr = match.Captures[0].Value;
            pattern = new Regex(RegexUtil.NMCmdRegex);
            MatchCollection matchCollection = pattern.Matches(nameStr);
            if (matchCollection.Count > 0)
            {
                names = new Dictionary<int, string>();
                foreach (Match item in matchCollection)
                {
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    string name = item.Groups["Name"].Value;
                    names.Add(lineNumber, name);
                }
            }
            return names;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Dictionary<int, Passenger> GetPassagers(string str)
        {
            Dictionary<int, Passenger> passengers = null;
            // 取得第一行；
            Regex pattern = new Regex(RegexUtil.CodeLineRegex);
            Match match = pattern.Match(str);
            if (!match.Success)
            {
                //throw new FormatException();
                return passengers;
            }

            string strTemp = match.Captures[0].Value;
            pattern = new Regex(RegexUtil.NMCmdRegex);
            MatchCollection matchCollection = pattern.Matches(strTemp);
            if (matchCollection.Count > 0)
            {
                passengers = new Dictionary<int, Passenger>();
                foreach (Match item in matchCollection)
                {
                    Passenger passenger = new Passenger();
                    int lineNumber = int.Parse(item.Groups["LineNumber"].Value);
                    passenger.Name = item.Groups["Name"].Value;
                    passenger.Type = (item.Groups["PassengerType"].Value == "CHD") ? PassengerType.Child : PassengerType.Adult;
                    passengers.Add(lineNumber, passenger);
                }
            }

            // 票号
            pattern = new Regex(RegexUtil.PNRTicketsRegex);
            matchCollection = pattern.Matches(str);
            if (matchCollection.Count > 0)
            {
                foreach (Match item in matchCollection)
                {
                    int key = int.Parse(item.Groups["PassengerId"].Value);
                    string ticketNumber = item.Groups["TicketNumber"].Value;
                    passengers[key].TicketNumbers.Add(ticketNumber);
                }
            }

            // 特殊信息（身份证号）
            pattern = new Regex(RegexUtil.SSRCmdRegex);
            matchCollection = pattern.Matches(str);
            if (matchCollection.Count > 0)
            {
                foreach (Match item in matchCollection)
                {
                    int key = int.Parse(item.Groups["PassengerId"].Value);
                    string certificateNumber = item.Groups["CertificateNumber"].Value;
                    passengers[key].CertificateNumber = certificateNumber;
                }
            }
            
            return passengers;
        }

        /// <summary>
        /// 获取PNR中的出票日期；
        /// </summary>
        public static DateTime GetIssuedDate(string str)
        {
            DateTime issueDatetime = default(DateTime);
            Regex pattern = new Regex(RegexUtil.TKCmdRegex);
            Match match = pattern.Match(str);

            if (match.Success)
            {
                int hour = int.Parse(match.Groups["Hour"].Value);
                int minute = int.Parse(match.Groups["Minute"].Value);
                issueDatetime = DateTime.Parse(match.Groups["Day"].Value + match.Groups["Month"].Value, new CultureInfo("en-US"));
                issueDatetime = issueDatetime.AddHours(hour).AddMinutes(minute);
            }

            return issueDatetime;
        }

        /// <summary>
        /// 根据航程列表，得到行程类型；
        /// </summary>
        /// <param name="voyages">航程列表</param>
        /// <returns>行程类型</returns>
        /// <remarks>
        /// 给出的航程列表必须按照按照时间排列好。
        /// </remarks>
        public static ItineraryType GetItineraryType(List<AirportPair> voyages)
        {
            ItineraryType itineraryType = default(ItineraryType);

            if (voyages.Count == 1)
            {
                itineraryType = ItineraryType.OneWay;
            }
            else
            {
                if (voyages.Count == 2 && voyages[0].Departure == voyages[1].Arrival && voyages[1].Departure == voyages[0].Arrival)
                {
                    itineraryType = ItineraryType.Roundtrip;
                }
                else
                {
                    itineraryType = IsContinuousVoyages(voyages) ? ItineraryType.Conjunction : ItineraryType.Notch;
                }
            }

            return itineraryType;
        }

        /// <summary>
        /// 根据航程列表，判断航程是否连续；
        /// </summary>
        /// <param name="voyages">航程列表</param>
        /// <returns>行程类型</returns>
        /// <remarks>
        /// 给出的航程列表必须按照按照时间排列好。
        /// </remarks>
        public static bool IsContinuousVoyages(List<AirportPair> voyages)
        {
            bool flag = true;
            for (int i = 1; i < voyages.Count; i++)
            {
                if (voyages[i].Departure != voyages[i - 1].Arrival)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
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
    }
}