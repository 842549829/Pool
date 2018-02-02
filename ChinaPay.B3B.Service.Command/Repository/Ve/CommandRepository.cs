using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Command.Domain;
using ChinaPay.B3B.Service.Command.Domain.DataTransferObject;
using ChinaPay.B3B.Service.Command.Domain.FlightQuery;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.Command.Domain.Ticket;
using ChinaPay.B3B.Service.Command.Domain.Utility;
using ChinaPay.B3B.Service.Command.VeWebReference;
using ChinaPay.B3B.Service.CommandBuilder;
using ChinaPay.B3B.Service.CommandBuilder.Domain.Queue;
using ChinaPay.B3B.Service.Queue.Domain;

namespace ChinaPay.B3B.Service.Command.Repository.Ve
{
    public class CommandRepository: ICommandRepository
    {
        private static string ReplaceUrl(string url)
        {
            return Regex.Replace(url, "http://.*?/veSWScn", "http://116.55.243.38:56065/veSWScn");
        }

        public ExecuteResult<JourneySheet> Detrf(string eTicketNumber, string userName)
        {
            throw new NotImplementedException();
        }

        public ExecuteResult<ElectronicTicket> Detr(string eTicketNumber, string userName)
        {
            throw new NotImplementedException();
        }

        public ExecuteResult<AirportPairFares> Fd(AirportPair airportPair, DateTime date, string carrier, string userName)
        {
            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);
            var returnString = veService.FD(airportPair.ToString(), date.ToString("ddMMMyy", CultureInfo.CreateSpecificCulture("en-US")),  carrier, "0", userName);
            var rawData = returnString;

            // 解析命令执行后的结果字串；
            var result = Domain.Utility.Parser.GetFare(rawData);

            // 根据解析结果返回
            return new ExecuteResult<AirportPairFares>
            {
                Result = result,
                Success = result != null,
                Message = rawData
            };
        }

        public ExecuteResult<IEnumerable<Flight>> Avh(string departureAirport, string arrivalAirport, DateTime flightDate, string carrier, bool isNoStop, string userName)
        {
            var veService = new veSWScnService();
            string airportPair = departureAirport + arrivalAirport;
            string date = flightDate.ToString("ddMMMyy", CultureInfo.CreateSpecificCulture("en-US"));
            string noStop = isNoStop ? "1" : "0";

            veService.Url = ReplaceUrl(veService.Url);
            string returnString = veService.AVH(airportPair, date, carrier, noStop, userName);
            XDocument xdoc = XDocument.Parse(returnString, LoadOptions.None);

            bool hasSuccessed = xdoc.Element("VEAVH") != null;
            string rawData = xdoc.Element("VEAVH") != null ? xdoc.Element("VEAVH").Element("Content").Value : returnString;
            List<Flight> result = hasSuccessed ? Domain.Utility.Parser.GetFlights(rawData) : null;

            var executeResult = new ExecuteResult<IEnumerable<Flight>>
            {
                Success = hasSuccessed,
                Message = rawData,
                Result = result
            };

            return executeResult;
        }
        
        public ExecuteResult<List<TransitPoint>> Ff(string flightNumber, DateTime flightDate, string userName)
        {
            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);

            string date = flightDate.ToString("ddMMMyy", CultureInfo.CreateSpecificCulture("en-US"));
            string returnString = veService.FF(flightNumber, date, userName);

            // 内容解析
            var result = Domain.Utility.Parser.GetTransitPoints(returnString);
            var rawData = returnString;
            var hasSuccessed = result != null && result.Count > 0;

            // 返回结果
            var executeResult = new ExecuteResult<List<TransitPoint>>
            {
                Success = hasSuccessed,
                Message = rawData,
                Result = result
            };
            return executeResult;
        }
        
        public ExecuteResult<ReservedPnr> Book(ReservationInfo reservationInfo, string officeNo, string userName)
        {
            var commandString = CommandBuilderService.GetBookInstructionSet(reservationInfo, officeNo);
            
            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);
            var returnString = veService.Book(commandString, userName, "KMG215");

            var xdoc = XDocument.Parse(returnString, LoadOptions.None);
            XElement content = xdoc.Element("P");
            var hasSuccessed = content!= null && content.Element("R").Value == "1";
            string rawData = content.Element("C").Value;
            
            var executeResult = new ExecuteResult<ReservedPnr>
            {
                Success = hasSuccessed,
                Result = hasSuccessed ? Domain.Utility.Parser.GetPnrContent(rawData) : null,
                Message = rawData
            };
            return executeResult;
        }

        public ExecuteResult<ReservedPnr> Rt(string pnrCode, string userName)
        {
            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);
            var returnString = veService.RtPnr(pnrCode, userName, "");
            
            // 内容解析
            if (Domain.Utility.Parser.IsXmlFormat(returnString))
            {
                var xdoc = XDocument.Parse(returnString, LoadOptions.None);
                var content = xdoc.Element("P");
                var flag = content.Element("R").Value == "1";
                var rawData = content.Element("Q").Value;
                var reservedPnr = Domain.Utility.Parser.GetPnrContent(rawData);

                if (flag && reservedPnr != null)
                {
                    reservedPnr.PnrRawData = rawData;
                    return new ExecuteResult<ReservedPnr>
                               {
                                   Success = true,
                                   Result = reservedPnr,
                                   Message = reservedPnr.HasCanceled ? "编码已取消" : rawData
                               };
                }
            }

            return new ExecuteResult<ReservedPnr>
                       {
                           Success = false,
                           Message = ErrorMessageUtil.ReplaceErrorMessage(returnString)
                       };
        }
        
        public ExecuteResult<string> Fd(string departureAirport, string arrivalAirport, DateTime flightDate, string carrier)
        {
            var veService = new veSWScnService();
            const string userName = "8000";
            veService.Url = ReplaceUrl(veService.Url);

            string airportPair = departureAirport + arrivalAirport;
            string date = flightDate.ToString("ddMMMyy", CultureInfo.CreateSpecificCulture("en-US"));
            string result = veService.FD(airportPair, date, carrier, "0", userName);

            var returnDate = new ExecuteResult<string>
            {
                Success = true,
                Message = result
            };
            return returnDate;
        }

        public ExecuteResult<string> Authorize(string pnrCode, string officeNo, string[] addOffices, string userName)
        {
            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);

            string returnString = veService.PnrAuth(pnrCode, officeNo, string.Join(",", addOffices), null, userName);

            var xdoc = XDocument.Parse(returnString, LoadOptions.None);
            XElement content = xdoc.Element("PNRAUTH");
            var hasSuccessed = content != null && content.Element("FLAG").Value == "1";

            string rawData = content.Element("CONTENT").Value;
            var result = hasSuccessed ? rawData : null;

            // 返回结果
            var executeResult = new ExecuteResult<string>
            {
                Success = hasSuccessed,
                Message = rawData,
                Result = result
            };
            return executeResult;
        }
        
        public ExecuteResult<string> Wf(string airPortCode)
        {
            throw new NotImplementedException();
        }

        public ExecuteResult<ReservedPnr> Rrt(string pnrCode, string flightNumber, DateTime flightDate, string userName)
        {
            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);
            string returnString = veService.RRTPNR(pnrCode, flightNumber, flightDate.ToString("ddMMMyy", CultureInfo.CreateSpecificCulture("en-US")), userName, "");

            throw new NotImplementedException();
        }

        public ExecuteResult<ReservedPnr> RrtOk(string pnrCode, string flightNumber, DateTime flightDate, string userName)
        {
            var officeNo = "";
            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);
            string inputXml =
                string.Format(
                    "<INPUT><COMMAND>VERRTOK</COMMAND><PARA><USER>{0}</USER><PNRNO>{1}</PNRNO><FLIGHT>{2}</FLIGHT><DATE>{3}</DATE><OFFICE>{4}</OFFICE></PARA></INPUT>",
                    userName, pnrCode, flightNumber, flightDate.ToString("ddMMMyy", CultureInfo.CreateSpecificCulture("en-US")), officeNo);
            string returnString = veService.GeneralCmdProcess(inputXml);
            
            // 内容解析
            var xdoc = XDocument.Parse(returnString, LoadOptions.None);
            XElement content = xdoc.Element("RESULT");
            var hasSuccessed = content != null && content.Element("STATUS").Value == "0";
            var rawData = hasSuccessed ? content.Element("DESCRIPTION").Element("RRTOKresult").Value : null;
            var message = hasSuccessed ? content.Element("DESCRIPTION").Element("RRTOKresult").Value : content.Element("DESCRIPTION").Value;

            var executeResult = new ExecuteResult<ReservedPnr>
            {
                Success = hasSuccessed,
                Result = hasSuccessed ? Domain.Utility.Parser.GetPnrContent(rawData) : null,
                Message = message
            };
            return executeResult;
        }

        public ExecuteResult<ReservedPnr> Rtx(string pnrCode, string userName)
        {
            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);
            string returnString = veService.RtxPnr(pnrCode, userName, "");
             //ExecuteResult<ReservedPnr> executeResult;

            // 内容解析
             if (Domain.Utility.Parser.IsXmlFormat(returnString))
             {
                 var xdoc = XDocument.Parse(returnString, LoadOptions.None);
                 var content = xdoc.Element("P");
                 var flag = content.Element("R").Value == "1";
                 var rawData = content.Element("Q").Value;

                 var reservedPnr = Domain.Utility.Parser.GetPnrContent(rawData);

                 if (flag && reservedPnr != null)
                 {
                     reservedPnr.PnrRawData = rawData;
                     return new ExecuteResult<ReservedPnr>
                     {
                         Success = true,
                         Result = reservedPnr,
                         Message = reservedPnr.HasCanceled ? "编码已取消": rawData
                     };
                 }
             }
             return new ExecuteResult<ReservedPnr>()
                 {
                     Success = false,
                     Message = ErrorMessageUtil.ReplaceErrorMessage(returnString)
                 };
        }

        public bool Xepnr(string pnrCode, string userName)
        {
            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);
            string rawData = veService.CancelPNR(pnrCode, userName);
            return Domain.Utility.Parser.CanceledPnrSuccess(rawData);
        }

        /// <summary>
        /// 根据给出旅客订座记录编号和待取旅客的名称，取消相应的旅客。
        /// </summary>
        /// <param name="pnrCode">旅客订座记录编号</param>
        /// <param name="names">待取消旅客的姓名数组</param>
        /// <param name="userName">用户名</param>
        /// <returns>是否成功</returns>
        public bool Xe(string pnrCode, string[] names, string userName)
        {
            if (names.Length == 0)
            {
                throw new ArgumentException("旅客姓名");
            }

            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);

            var returnString = veService.RtPnr(pnrCode, userName, "KMG215");
            var xdoc = XDocument.Parse(returnString, LoadOptions.None);
            XElement content = xdoc.Element("P");
            var hasSuccessed = content != null && content.Element("R").Value == "1";

            // 若提取编码失败,直接退出；
            if (!hasSuccessed)
            {
                return false;
            }

            string rawData = content.Element("Q").Value;

            // 判断编码是否被取消，这种直接视为取消成功；
            if (Domain.Utility.Parser.IsCanceled(rawData))
            {
                return true;
            }

            // 获取姓名字串
            var nameAndOfficeStr = Domain.Utility.Parser.RemoveLineBreakAndSpace(Domain.Utility.Parser.GetNameAndOfficeNoString(rawData));
            // 获取名称项
            var passengers = Domain.Utility.Parser.GetPassengers(nameAndOfficeStr);

            // 没有信息；
            if (passengers.Count == 0 )
            {
                return false;
            }

            // 找出待取消的编号列表；
            var lineNumbers = (from n in names
                                          join p in passengers on n equals  p.Name
                                          select p.LineNumber).ToList();

            if (lineNumbers.Count == 0)
            {
                return true;
            }

            // 若待取消取消的数量和现有数量相等，即所有的都取消，此时取消编码；
            if (lineNumbers.Count == passengers.Count)
            {
                return Xepnr(pnrCode, userName);
            }

            // 发送取消编码的指令；
            string cmd = CommandBuilderService.GetCancelPassengerInstruction(lineNumbers.ToArray());
            rawData = veService.EditPNR(pnrCode, cmd, "1", userName);
            rawData = Domain.Utility.Parser.RemoveLineBreakAndSpace(rawData);
            return rawData == "1";
        }

        /// <summary>
        /// 根据给出旅客订座记录编号和待取消航段的航段信息，取消相应的航段。
        /// </summary>
        /// <param name="pnrCode">旅客订座记录编号</param>
        /// <param name="cancelSegments">待取消航段的航段信息</param>
        /// <param name="userName">用户名</param>
        /// <returns>是否成功</returns>
        public bool Xe(string pnrCode, CancelSegmentInfo[] cancelSegments, string userName)
        {
            if (cancelSegments.Length == 0)
            {
                throw new ArgumentException("航段信息");
            }

            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);

            var returnString = veService.RtPnr(pnrCode, userName, "KMG215");
            var xdoc = XDocument.Parse(returnString, LoadOptions.None);
            XElement content = xdoc.Element("P");
            var hasSuccessed = content != null && content.Element("R").Value == "1";

            // 若提取编码失败,直接退出；
            if (!hasSuccessed)
            {
                return false;
            }

            string rawData = content.Element("Q").Value;

            // 判断编码是否被取消，这种直接视为取消成功；
            if (Domain.Utility.Parser.IsCanceled(rawData))
            {
                return true;
            }

            var segments = Domain.Utility.Parser.GetSegements(rawData);

            // 没有信息；
            if (segments.Count == 0)
            {
                return false;
            }

            // 以下核对信息，主要是防止编码在线下被处理后，和平台的信息不符；
            // 找出待取消的编号列表；
            List<int> lineNumbers = new List<int>();
            bool hasRescheduled = false;
            foreach (var cs in cancelSegments)
            {
                foreach (var rs in segments)
                {
                    // 先匹配航班号
                    if (cs.FlightNumber.Equals(rs.FlightNumber))
                    {
                        // 若航班号和时间都能匹配，则说明没有改期；
                        if (cs.FlightDate.Date == rs.FlightDate.Date)
                        {
                            lineNumbers.Add(rs.LineNumber);
                        }
                        else
                        {
                            hasRescheduled =  true;
                        }
                    }
                }
            }

            // 若有改期，则返回假，不做操作，由此，此结果将指向到平台，
            if (hasRescheduled)
            {
                return false;
            }

            // 若行号中没有数据，则没有匹配到值，返回成功；
            if (lineNumbers.Count == 0)
            {
                return true;
            }
 
            // 若待取消取消的数量和现有数量相等，即所有的都取消，此时取消编码；
            if (lineNumbers.Count() == segments.Count)
            {
                return Xepnr(pnrCode, userName);
            }

            // 获取取消航段的指令；
            string cmd = CommandBuilderService.GetCancelSegmentInstruction(lineNumbers.ToArray());
            // 发送取消编码的指令；
            rawData = veService.EditPNR(pnrCode, cmd, "1", userName);
            rawData = Domain.Utility.Parser.RemoveLineBreakAndSpace(rawData);
            return rawData == "1";
        }

        public ExecuteResult<ReservedPnr> SsrFoid(string pnrCode, string name, string oldNumber, string newNumber, CredentialsType certificateType, string userName)
        {
            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);
            string returnString = veService.SSRFOID(pnrCode, name, oldNumber, newNumber, certificateType.ToString(),
                                                    "KMG215", userName);

            XDocument xdoc = XDocument.Parse(returnString, LoadOptions.None);
            returnString = xdoc.Element("VESSRFOID") != null
                               ? xdoc.Element("VESSRFOID").Element("PNR").Value
                               : returnString;

            string rawData = returnString;
            ReservedPnr result = Domain.Utility.Parser.GetPnrContent(rawData);

            return new ExecuteResult<ReservedPnr>
                       {
                           Success = (result != null),
                           Result = result,
                           Message = rawData
                       };
        }

        public ExecuteResult<IEnumerable<PriceView>> Pat(string pnrCode)
        {
            var veService = new veSWScnService();
            const string userName = "8000";
            veService.Url = ReplaceUrl(veService.Url);

            string returnString = veService.PAT(pnrCode, userName, "");

            // 内容解析
            var xdoc = XDocument.Parse(returnString, LoadOptions.None);
            XElement content = xdoc.Element("P");
            var hasSuccessed = content != null && content.Element("R").Value == "0";

            string rawData = content.Element("C").Value;

            var executeResult = new ExecuteResult<IEnumerable<PriceView>>
            {
                Success = hasSuccessed,
                Result = hasSuccessed ? Domain.Utility.Parser.GetPatPrices(rawData) : null,
                Message = rawData
            };
            return executeResult;
        }
  
        public ExecuteResult<IEnumerable<PriceView>> PnrPat(string pnrCode, PassengerType passengerType, string userName)
        {
            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);
            var cmd = CommandBuilder.CommandBuilderService.GetPatInstruction(passengerType);
            string rawData = veService.PNRPAT(pnrCode, cmd, userName, "");

            var result = Domain.Utility.Parser.GetPatPrices(rawData);

            // 内容解析
            return new ExecuteResult<IEnumerable<PriceView>>
            {
                Success = result != null && result.Count > 0,
                Result = result,
                Message = rawData
            };
        }
        
        public List<QueueSummary> Qt(string userName, string password, string officeNo)
        {
            var veService = new veSWScnService();
            veService.Url = ReplaceUrl(veService.Url);
            throw new NotImplementedException();
        }

        public string Qs(QueueType queueType, string userName, string password)
        {
            throw new NotImplementedException();
        }
        
        public string Qd(string option, string userName, string password)
        {
            throw new NotImplementedException();
        }
        
        public string Qn(string option, string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}