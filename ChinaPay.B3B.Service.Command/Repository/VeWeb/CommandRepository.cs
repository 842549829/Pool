using System;
using System.Collections.Generic;
using System.Xml.Linq;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Command.Domain;
using ChinaPay.B3B.Service.Command.Domain.DataTransferObject;
using ChinaPay.B3B.Service.Command.Domain.FlightQuery;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.Command.Domain.Ticket;
using ChinaPay.B3B.Service.CommandBuilder.Domain;
using ChinaPay.B3B.Service.CommandBuilder.Domain.FlightQuery;
using ChinaPay.B3B.Service.CommandBuilder.Domain.Queue;
using ChinaPay.B3B.Service.CommandBuilder.Domain.Ticket;
using ChinaPay.B3B.Service.CommandExecutor;
using ChinaPay.B3B.Service.PidManagement;
using ChinaPay.B3B.Service.PidManagement.Domain;
using ChinaPay.B3B.Service.Queue.Domain;

namespace ChinaPay.B3B.Service.Command.Repository.VeWeb
{
    class CommandRepository:ICommandRepository
    {
        /// <summary>
        /// 获取XML中的原始数据。
        /// </summary>
        /// <param name="xdoc">XML</param>
        /// <param name="elementName">元素名称</param>
        /// <returns>命令执行后的原始字串</returns>
        /// <returns>
        /// 
        /// </returns>
        private static string GetRawDate(XDocument xdoc, string elementName)
        {
            if (xdoc == null) throw new ArgumentNullException("xdoc");
            if (string.IsNullOrEmpty(elementName)) throw new ArgumentNullException("elementName");

            var pidElement = xdoc.Element("PID");
            if (pidElement!= null)
            {
                var resultElement = pidElement.Element("RESULT");
                if (resultElement!= null)
                {
                    var xElement = xdoc.Element(elementName);
                    if (xElement!= null)
                    {
                        return xElement.Value;
                    }
                    return resultElement.Value;
                }
                return string.Empty;
            }
            return string.Empty;
        }
        
        public ExecuteResult<JourneySheet> Detrf(string eTicketNumber, string userName)
        {
            // 参数验证；
            if (string.IsNullOrEmpty(eTicketNumber)) throw new ArgumentNullException("eTicketNumber");
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("userName");

            // 构建命令并执行；
            var detrCommand = new DetrCommand(eTicketNumber, DetrQeeryType.TN, "F");
            var user = new User("10000", "123456");
            var returnString = CommandExecutorService.Execute(detrCommand, user);
            var xdoc = XDocument.Parse(returnString, LoadOptions.None);
            var rawData = GetRawDate(xdoc, "DETR");

            // 解析命令执行后的结果字串；
            var result = Domain.Utility.Parser.GetJourneySheet(rawData);

            // 根据解析结果返回
            return new ExecuteResult<JourneySheet>
            {
                Result = result,
                Success = result != null,
                Message = rawData
            };
        }

        public ExecuteResult<ElectronicTicket> Detr(string eTicketNumber, string userName)
        {
            // 参数验证；
            if (string.IsNullOrEmpty(eTicketNumber)) throw new ArgumentNullException("eTicketNumber");
            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("userName");

            // 构建命令并执行；
            var detrCommand = new DetrCommand(eTicketNumber, DetrQeeryType.TN, "");
            var user = new User("10000", "123456");
            var returnString = CommandExecutorService.Execute(detrCommand, user);
            var xdoc = XDocument.Parse(returnString, LoadOptions.None);
            var rawData = GetRawDate(xdoc, "DETR");

            // 解析命令执行后的结果字串；
            var result = Domain.Utility.Parser.GetElectronicTicket(rawData);

            // 根据解析结果返回
            return new ExecuteResult<ElectronicTicket>
                       {
                           Result = result,
                           Success = (result != null && result.TicketNumber != null),
                           Message = rawData
                       };
        }

        public ExecuteResult<AirportPairFares> Fd(AirportPair airportPair, DateTime date, string carrier,
                                                       string userName)
        {
            // 构建命令并执行；
            var fdCommand = new FdCommand(airportPair, date, carrier);
            var user = new User("8000", "123");
            var returnString = CommandExecutorService.Execute(fdCommand, user);
            var xdoc = XDocument.Parse(returnString, LoadOptions.None);
            var rawData = GetRawDate(xdoc, "FD");

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

        public ExecuteResult<IEnumerable<Domain.FlightQuery.Flight>> Avh(string departureAirport, string arrivalAirport, DateTime flightDate, string carrier, bool isNoStop, string userName)
        {
            throw new NotImplementedException();
        }

        public ExecuteResult<List<Domain.FlightQuery.TransitPoint>> Ff(string flightNumber, DateTime flightDate, string userName)
        {
            throw new NotImplementedException();
        }

        public ExecuteResult<Domain.PNR.ReservedPnr> Book(DataTransferObject.Command.PNR.ReservationInfo reservationInfo, string officeNo, string userName)
        {
            throw new NotImplementedException();
        }

        public ExecuteResult<Domain.PNR.ReservedPnr> Rt(string pnrCode, string userName)
        {
            throw new NotImplementedException();
        }

        public ExecuteResult<Domain.PNR.ReservedPnr> Rrt(string pnrCode, string flightNumber, DateTime flightDate, string userName)
        {
            throw new NotImplementedException();
        }

        public ExecuteResult<ReservedPnr> RrtOk(string pnrCode, string flightNumber, DateTime flightDate, string userName)
        {
            throw new NotImplementedException();
        }

        public Domain.ExecuteResult<Domain.PNR.ReservedPnr> Rtx(string pnrCode, string userName)
        {
            throw new NotImplementedException();
        }

        public Domain.ExecuteResult<string> Fd(string departureAirport, string arrivalAirport, DateTime flightDate, string carrier)
        {
            throw new NotImplementedException();
        }

        public Domain.ExecuteResult<string> Wf(string airPortCode)
        {
            throw new NotImplementedException();
        }

        public ExecuteResult<string> Authorize(string pnrCode, string officeNo, string[] addOffices, string userName)
        {
            throw new NotImplementedException();
        }

        public bool Xepnr(string pnrCode, string userName)
        {
            throw new NotImplementedException();
        }

        public bool Xe(string pnrCode, string[] names, string userName)
        {
            throw new NotImplementedException();
        }

        public bool Xe(string pnrCode, CancelSegmentInfo[] cancelSegments, string userName)
        {
            throw new NotImplementedException();
        }

        public ExecuteResult<Domain.PNR.ReservedPnr> SsrFoid(string pnrCode, string name, string oldNumber, string newNumber, Common.Enums.CredentialsType certificateType, string userName)
        {
            throw new NotImplementedException();
        }

        public ExecuteResult<IEnumerable<DataTransferObject.Command.PNR.PriceView>> Pat(string pnrCode)
        {
            throw new NotImplementedException();
        }

        public ExecuteResult<IEnumerable<DataTransferObject.Command.PNR.PriceView>> PnrPat(string pnrCode, Common.Enums.PassengerType passengerType, string userName)
        {
            throw new NotImplementedException();
        }

        public List<QueueSummary> Qt(string userName, string password, string officeNo)
        {
            CommandBuilder.Command qtCommand = new QtCommand();
            var user = PidManagementService.GetUser();
            var returnString = CommandExecutorService.Execute(qtCommand, user);

            XDocument xdoc = XDocument.Parse(returnString, LoadOptions.None);
            var rawData = GetRawDate(xdoc, "QT");
            return Domain.Utility.Parser.GetMailList(rawData);
        }

        public string Qs(QueueType queueType, string userName, string password)
        {
            var qsCommand = new QsCommand(queueType);
            var user = PidManagementService.GetUser();
            var returnString = CommandExecutorService.Execute(qsCommand, user);

            XDocument xdoc = XDocument.Parse(returnString, LoadOptions.None);
            var rawData = GetRawDate(xdoc, "QS");
            return rawData;
        }
        
        public string Qd(string option, string userName, string password)
        {
            var qdCommand = new QdCommand(option);
            var user = PidManagementService.GetUser();
            var returnString = CommandExecutorService.Execute(qdCommand, user);

            XDocument xdoc = XDocument.Parse(returnString, LoadOptions.None);
            var rawData = GetRawDate(xdoc, "QD");
            return rawData;
        }
        
        public string Qn(string option, string userName, string password)
        {
            var qnCommand = new QnCommand(option);
            var user = PidManagementService.GetUser();
            var returnString = CommandExecutorService.Execute(qnCommand, user);

            XDocument xdoc = XDocument.Parse(returnString, LoadOptions.None);
            var rawData = GetRawDate(xdoc, "QN");
            return rawData;
        }
    }
}
