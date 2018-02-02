using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Command.Domain;
using ChinaPay.B3B.Service.Command.Domain.DataTransferObject;
using ChinaPay.B3B.Service.Command.Domain.FlightQuery;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.Command.Domain.Ticket;
using ChinaPay.B3B.Service.CommandBuilder.Domain.Queue;
using ChinaPay.B3B.Service.Queue.Domain;

namespace ChinaPay.B3B.Service.Command.Repository
{
    internal interface ICommandRepository
    {
        ExecuteResult<JourneySheet> Detrf(string eTicketNumber, string userName);
        
        ExecuteResult<ElectronicTicket> Detr(string eTicketNumber, string userName);
            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="airportPair"></param>
        /// <param name="date"></param>
        /// <param name="carrier"></param>
        /// <param name="userName"> </param>
        /// <returns></returns>
        ExecuteResult<AirportPairFares> Fd(AirportPair airportPair, DateTime date, string carrier, string userName);
            
            /// <summary>
        /// 根据给出的航班出发机场、到达机场、航班时间、承运人及是否直达，用给定的指令系统用户名执行AVH指令，获取相应的航班信息。
        /// </summary>
        /// <param name="departureAirport">离港</param>
        /// <param name="arrivalAirport">到港</param>
        /// <param name="flightDate">航班时间</param>
        /// <param name="carrier">承运人</param>
        /// <param name="isNoStop">是否直达</param>
        /// <param name="userName">指令系统用户名</param>
        /// <returns>航班信息列表</returns>
        ExecuteResult<IEnumerable<Flight>> Avh(string departureAirport, string arrivalAirport, DateTime flightDate, string carrier, bool isNoStop, string userName);

        /// <summary>
        /// 根据给出的航班号，航班日期，用给定的指令系统用户名执行FF指令，获取相应的航班经停信息。
        /// </summary>
        /// <param name="flightNumber">航班号</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="userName">指令系统用户名</param>
        /// <returns>经停信息列表</returns>
        ExecuteResult<List<TransitPoint>> Ff(string flightNumber, DateTime flightDate, string userName);

        /// <summary>
        /// 根据给出的旅客预订信息，生成订座指令序列，用给定的指令系统用户名执行此指令序列预订航班，并获取相应的旅客订座信息。
        /// </summary>
        /// <param name="reservationInfo">旅客预订信息</param>
        /// <param name="officeNo">期望封口时使用的代理人编号</param>
        /// <param name="userName">指令系统用户名</param>
        /// <returns>旅客订座信息</returns>
        /// <remarks>
        /// 暂不支持缺口程预定，没有生成搭桥指令。
        /// </remarks>
        ExecuteResult<ReservedPnr> Book(ReservationInfo reservationInfo, string officeNo, string userName);
        
        /// <summary>
        /// 根据给出的旅客订座记录编号，用给定的指令系统用户名执行RT指令，获取相应的旅客订座信息。
        /// </summary>
        /// <param name="pnrCode">旅客订座记录编号</param>
        /// <param name="userName">指令系统用户名</param>
        /// <returns>旅客订座信息</returns>
        ExecuteResult<ReservedPnr> Rt(string pnrCode, string userName);

        ExecuteResult<ReservedPnr> Rrt(string pnrCode, string flightNumber, DateTime flightDate, string userName);


        ExecuteResult<ReservedPnr> RrtOk(string pnrCode, string flightNumber, DateTime flightDate, string userName);


        ExecuteResult<ReservedPnr> Rtx(string pnrCode, string userName);
        
        ExecuteResult<string> Fd(string departureAirport, string arrivalAirport, DateTime flightDate, string carrier);

        ExecuteResult<string> Wf(string airPortCode);

        /// <summary>
        /// 执行RMK TJ AUTH 指令
        /// </summary>
        /// <param name="pnrCode"></param>
        /// <param name="officeNo"></param>
        /// <param name="addOffices"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        ExecuteResult<string> Authorize(string pnrCode, string officeNo, string[] addOffices, string userName);

        /// <summary>
        /// 执行XEPNR指令
        /// </summary>
        /// <param name="pnrCode"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool Xepnr(string pnrCode, string userName);

        /// <summary>
        /// 取消乘机人
        /// </summary>
        /// <param name="pnrCode"></param>
        /// <param name="names"></param>
        /// <param name="userName"> </param>
        /// <returns></returns>
        bool Xe(string pnrCode, string[] names, string userName);

        /// <summary>
        /// 取消航段
        /// </summary>
        /// <param name="pnrCode"></param>
        /// <param name="cancelSegments"></param>
        /// <param name="?"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool Xe(string pnrCode, CancelSegmentInfo[] cancelSegments, string userName);

        /// <summary>
        /// 修改证件号
        /// </summary>
        /// <param name="pnrCode"></param>
        /// <param name="name"> </param>
        /// <param name="oldNumber"> </param>
        /// <param name="newNumber"> </param>
        /// <param name="certificateType"> </param>
        /// <param name="userName"> </param>
        /// <returns></returns>
        ExecuteResult<ReservedPnr> SsrFoid(string pnrCode, string name, string oldNumber, string newNumber, CredentialsType certificateType, string userName);

        ExecuteResult<IEnumerable<PriceView>> Pat(string pnrCode);

        ExecuteResult<IEnumerable<PriceView>> PnrPat(string pnrCode, PassengerType passengerType, string userName);

        /// <summary>
        /// 执行QT指令
        /// </summary>
        /// <returns>信箱概要信息</returns>
        List<QueueSummary> Qt(string userName, string password, string officeNo);

        /// <summary>
        /// 执行QS指令
        /// </summary>
        /// <returns>信箱概要信息</returns>
        string Qs(QueueType queueType,  string userName, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="officeNo"></param>
        /// <returns></returns>
        string Qd(string option, string userName, string password);
        
        string Qn(string option, string userName, string password);
    }
}
