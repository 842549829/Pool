using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.AirlineConfig;
using ChinaPay.B3B.Service.Command.Domain;
using ChinaPay.B3B.Service.Command.Domain.DataTransferObject;
using ChinaPay.B3B.Service.Command.Domain.Exception;
using ChinaPay.B3B.Service.Command.Domain.FlightQuery;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.Command.Domain.Ticket;
using ChinaPay.B3B.Service.Command.PNR;
using ChinaPay.B3B.Service.Command.Repository;
using ChinaPay.B3B.Service.CommandBuilder;
using ChinaPay.B3B.Service.CommandBuilder.Domain.Queue;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.PidManagement;
using ChinaPay.B3B.Service.Queue.Domain;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.XAPI.Service.Pid;
using ChinaPay.XAPI.Service.Pid.Domain;
using Flight = ChinaPay.B3B.Service.Command.Domain.FlightQuery.Flight;
using FlightNumber = ChinaPay.B3B.Service.Command.Domain.FlightQuery.FlightNumber;
using Passenger = ChinaPay.B3B.Service.Command.Domain.PNR.Passenger;

namespace ChinaPay.B3B.Service.Command
{
    /// <summary>
    /// 指令服务类
    /// </summary>
    public static class CommandService
    {
        // 注意，这里的流量统计不是很准确，是否要在那边记录一个值，看指令是否执行；
        // 还有，清Q时结束的话，如果没有获取到，不会并发问题吧？

        /// <summary>
        /// 根据给出的OEM编号和类型，得到PID用户名；
        /// </summary>
        /// <param name="oemId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetUserName(Guid oemId, ConfigUseType type)
        {
            // 取得配置信息；
            var config = OEMAirlineConfigService.QueryConfig(oemId);
            // 在配置信息中查找指定类型的配置，若存在，赋值，否则为空；
            var userName = config.Config.ContainsKey(type) ? config.Config[type].Item1 : null;

            if (userName == string.Empty)
            {
                return null;
            }

            // 若用户名为空，但要求使用平台配置，则此时取得平台配置；
            if (userName == null && OEMService.QueryOEMById(oemId).UseB3BConfig)
            {
                config = OEMAirlineConfigService.QueryConfig(Guid.Empty);
                userName = config.Config.ContainsKey(type) ? config.Config[type].Item1 : null;
            }

            return userName;
        }
        
        /// <summary>
        /// 根据给出的OEM编号和类型，得到其Office号；
        /// </summary>
        /// <param name="oemId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetOfficeNumber(Guid oemId, ConfigUseType type)
        {
            var config = OEMAirlineConfigService.QueryConfig(oemId);
            var officeNumber = config.Config.ContainsKey(type) ? config.Config[type].Item2 : null;

            if (string.IsNullOrEmpty(officeNumber) && OEMService.QueryOEMById(oemId).UseB3BConfig)
            {
                config = OEMAirlineConfigService.QueryConfig(Guid.Empty);
                officeNumber = config.Config.ContainsKey(type) ? config.Config[type].Item2 : null;
            }

            return officeNumber;
        }

        public static bool HasUsePlatformConfig(Guid oemId, ConfigUseType type)
        {
            if (oemId == Guid.Empty)
            {
                return true;
            }

            var config = OEMAirlineConfigService.QueryConfig(oemId);
            if (config.Config.ContainsKey(type))
            {
                return false;
            }
            else
            {
                if (OEMService.QueryOEMById(oemId).UseB3BConfig)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        #region 航班查询

        /// <summary>
        /// 根据出发机场、到达机场以及航班日期，获取符合条件的航班信息
        /// </summary>
        /// <param name="departureAirport">出发机场</param>
        /// <param name="arrivalAirport">到达机场</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="oemId"> </param>
        /// <returns>航班列表</returns>
        public static ExecuteResult<IEnumerable<Flight>> QueryFlight(string departureAirport, string arrivalAirport, DateTime flightDate, Guid oemId)
        {
            // 参数验证
            if (string.IsNullOrEmpty(departureAirport) || string.IsNullOrEmpty(arrivalAirport))
            {
                throw new ArgumentException("航段信息不能为空");
            }

            const ConfigUseType type = ConfigUseType.Query;
            var userName = GetUserName(oemId, type);

            if (userName == null)
            {
                return  new ExecuteResult<IEnumerable<Flight>>()
                            {
                                Result = null,
                                Success = false,
                                Message = "OEM未设置可用配置，且没有设置使用平台配置。"
                            };
            }

            var repository = Factory.CreateCommandRepository();
            var carrier = "";
            var result = repository.Avh(departureAirport, arrivalAirport, flightDate, carrier, true, userName);

            if (result.Success)
            {
                PidManagementService.SaveCounter(oemId, HasUsePlatformConfig(oemId, type));
            }

            return result;
        }

        /// <summary>
        /// 根据航班号和航班日期，获取经停点列表信息
        /// </summary>
        /// <param name="flightNumber">航班号</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="oemId"> </param>
        /// <returns>经停点列表</returns>
        public static ExecuteResult<List<TransitPoint>> GetTransitPoints(string flightNumber, DateTime flightDate, Guid oemId)
        {
            // 参数验证
            if (flightNumber == null)
            {
                throw new ArgumentException("航班号不能为空");
            }

            const ConfigUseType type = ConfigUseType.Query;
            var userName = GetUserName(oemId, type);
            if (userName == null)
            {
                return new ExecuteResult<List<TransitPoint>>()
                {
                    Result = null,
                    Success = false,
                    Message = "OEM未设置可用配置，且没有设置使用平台配置。"
                };
            }

            var repository = Factory.CreateCommandRepository();
            var result = repository.Ff(flightNumber, flightDate, userName);
            if (result.Success)
            {
                PidManagementService.SaveCounter(oemId, HasUsePlatformConfig(oemId, type));
            }

            return result;
        }

        /// <summary>
        /// 查询单页航班数据，用于免票时获取机型
        /// </summary>
        /// <param name="departure"></param>
        /// <param name="arrival"></param>
        /// <param name="takeoffTime"></param>
        /// <param name="oemId"> </param>
        /// <returns></returns>
        public static ExecuteResult<IEnumerable<Flight>> QuerySingleFlight(string departure, string arrival, DateTime takeoffTime, Guid oemId)
        {
            const ConfigUseType type = ConfigUseType.Query;
            var userName = GetUserName(oemId, type);
            var executeResult = new ExecuteResult<IEnumerable<Flight>>()
            {
                Message = "暂时不支持此方法",
                Success = false
            };
            return executeResult;
        }
        #endregion

        #region 旅客订座记录生成

        /// <summary>
        /// 机票预定
        /// </summary>
        /// <param name="reservationInfo">预订信息</param>
        /// <param name="oemId"> </param>
        /// <param name="officeNumber"> </param>
        /// <returns></returns>
        public static ExecuteResult<ReservedPnr> ReserveTickets(ReservationInfo reservationInfo, Guid oemId)
        {
            if (reservationInfo == null) throw new ArgumentException("预订信息不能为空");

            const ConfigUseType type = ConfigUseType.Reserve;
            var userName = GetUserName(oemId, type);
            var officeNumber = GetOfficeNumber(oemId, type);

            if (userName == null || officeNumber == null)
            {
                return new ExecuteResult<ReservedPnr>
                {
                    Result = null,
                    Success = false,
                    Message = "OEM未设置可用配置，且没有设置使用平台配置。"
                };
            }

            var repository = Factory.CreateCommandRepository();
            var executeResult = repository.Book(reservationInfo, officeNumber, userName);

            if (executeResult.Success)
            {
                var reservedPnr = executeResult.Result;
                var pnrHistory = new PNRHistory(0, DateTime.Now, reservedPnr.PnrPair.PNR, reservedPnr.OfficeNo, 1);
                HistoryService.Save(pnrHistory);
                PidManagementService.SaveCounter(oemId, HasUsePlatformConfig(oemId, type));
            }

            return executeResult;
        }

        /// <summary>
        /// 根据给出的订座信息和OEM编号，生成订座指令字串。
        /// </summary>
        /// <param name="reservationInfo">订座信息</param>
        /// <param name="oemId">OEM编号</param>
        /// <returns>订座指令字串</returns>
        public static string GetBookingTicketsString(ReservationInfo reservationInfo, Guid oemId)
        {
            const ConfigUseType type = ConfigUseType.Reserve;
            var officeNumber = GetOfficeNumber(oemId, type);
            return CommandBuilderService.GetBookInstructionSet(reservationInfo, officeNumber);
        }

        #endregion

        #region 旅客订座记录取消

        /// <summary>
        /// 根据旅客订座记录编号取消预订，如果编码在他处被取消，也被视为取消成功；
        /// </summary>
        /// <param name="pnrPair">旅客订座记录编号</param>
        /// <param name="oemId"></param>
        /// <returns>执行结果</returns>
        public static bool CancelPNR(PNRPair pnrPair, Guid oemId)
        {
            const ConfigUseType type = ConfigUseType.Reserve;
            // 参数验证
            if (PNRPair.IsNullOrEmpty(pnrPair))
            {
                throw new ArgumentException("旅客订座记录编码");
            }

            var userName = GetUserName(oemId, type);
            if (userName == null)
            {
                return false;
            }
            var repository = Factory.CreateCommandRepository();
            
            var result =  repository.Xepnr(pnrPair.PNR, userName);

            PidManagementService.SaveCounter(oemId, HasUsePlatformConfig(oemId, type));
            return result;
        }

        /// <summary>
        /// 根据航程取消航段，如果编码被取消，或者要取消的航段已不存在，仍被认为是取消成功。
        /// </summary>
        /// <param name="pnrPair">PNR编号</param>
        /// <param name="cancelSegments">航程</param>
        /// <param name="oemId"> </param>
        /// <returns>PNR</returns>
        public static bool CancelVoyagesByPNR(CancelSegmentInfo[] cancelSegments, PNRPair pnrPair, Guid oemId)
        {
            // 参数验证
            if (PNRPair.IsNullOrEmpty(pnrPair))
            {
                throw new ArgumentException("旅客订座记录编码");
            }

            const ConfigUseType type = ConfigUseType.Reserve;
            var userName = GetUserName(oemId, type);
            if (userName == null)
            {
                return false;
            }
            var repository = Factory.CreateCommandRepository();
            var result = repository.Xe(pnrPair.PNR, cancelSegments, userName);

            PidManagementService.SaveCounter(oemId, HasUsePlatformConfig(oemId, type));
            return result;
        }

        /// <summary>
        /// 根据PNR编号，取消指定姓名的旅客的预定
        /// </summary>
        /// <param name="pnrPair">PNR编号</param>
        /// <param name="voyages">姓名</param>
        /// <param name="oemId"> </param>
        /// <returns>执行结果</returns>
        public static bool CancelPassengersByPNR(string[] passengerNames, PNRPair pnrPair, Guid oemId)
        {
            // 参数验证
            if (PNRPair.IsNullOrEmpty(pnrPair))
            {
                throw new ArgumentException("旅客订座记录编码");
            }

            const ConfigUseType type = ConfigUseType.Reserve;
            var userName = GetUserName(oemId, type);
            if (userName == null)
            {
                return false;
            }
            var repository = Factory.CreateCommandRepository();
            var result = repository.Xe(pnrPair.PNR, passengerNames, userName);
            PidManagementService.SaveCounter(oemId, HasUsePlatformConfig(oemId, type));
            return result;
        }

        #endregion

        #region 旅客订座记录修改
        /// <summary>
        /// 根据给出的旅客订座记录编号和姓名，修改旅客证件号码。
        /// </summary>
        /// <param name="pnrPair">旅客订座记录编号</param>
        /// <param name="name">旅客姓名</param>
        /// <param name="oldNumber">原证件号</param>
        /// <param name="newNumber">新证件号</param>
        /// <param name="type">证件类型</param>
        /// <param name="oemId">OEM编号</param>
        /// <returns></returns>
        public static ExecuteResult<ReservedPnr> ModifyCertificateNumber(PNRPair pnrPair, string name, string oldNumber, string newNumber, CredentialsType type, Guid oemId)
        {
            // 参数验证
            if (PNRPair.IsNullOrEmpty(pnrPair))
            {
                throw new ArgumentException("旅客订座记录编码");
            }

            var userName = "9000";

            try
            {
                var repository = Factory.CreateCommandRepository();
                PidManagementService.SaveCounter(oemId, true);
                var result = repository.SsrFoid(pnrPair.PNR, name, oldNumber, newNumber, type, userName);

                if (result.Success)
                {
                    PidManagementService.SaveCounter(oemId, true);
                }

                return result;
            }
            catch(Exception e)
            {
                return new ExecuteResult<ReservedPnr>()
                {
                    Success = false,
                    Message =  e.Message,
                    Result =  null
                };
            }
        }
        #endregion


        /// <summary>
        /// 根据给出的航空公司系统编码，得到订座记录；
        /// </summary>
        /// <param name="pnrPair">编码对</param>
        /// <param name="flightNumber">航班号</param>
        /// <param name="flightDate">航班日期</param>
        /// <param name="oemId"> </param>
        /// <returns></returns>
        public static ExecuteResult<ReservedPnr> TransferPnrCode(PNRPair pnrPair, FlightNumber flightNumber, DateTime flightDate, Guid oemId)
        {
            if (pnrPair == null || string.IsNullOrEmpty(pnrPair.BPNR))
            {
                throw new ArgumentException("旅客订座记录编码");
            }

            var userName = "9000";
            var repository = Factory.CreateCommandRepository();
            var result = repository.RrtOk(pnrPair.BPNR, flightNumber.ToString(), flightDate, userName);
            if (result.Success)
            {
                PidManagementService.SaveCounter(oemId, true);
            }
            return result;
        }

        /// <summary>
        /// 根据旅客订座记录编号,验证编码是否有效。
        /// </summary>
        /// <param name="pnrPair">旅客订座记录编号</param>
        /// <param name="passengerType"> </param>
        /// <param name="oemId"> </param>
        public static void ValidatePNR(PNRPair pnrPair, Common.Enums.PassengerType passengerType, Guid oemId)
        {
            ExecuteResult<ReservedPnr> executeResult = GetReservedPnr(pnrPair, oemId);
            if (executeResult.Success)
            {
                ValidatePNR(executeResult.Result, passengerType);
            }
            else
            {
                throw new ParseException(passengerType.GetDescription() + "编码解析失败");
            }
        }

        public static void ValidatePNR(ReservedPnr reservedPnr, Common.Enums.PassengerType passengerType)
        {
            if (reservedPnr.HasCanceled)
            {
                throw new PNRCanceledExceptioin(passengerType.GetDescription() + "订座记录已取消");
            }
            if (PNRPair.IsNullOrEmpty(reservedPnr.PnrPair))
            {
                throw new ArgumentException("缺少编码");
            }

            if (reservedPnr.Passengers == null || reservedPnr.Passengers.Count == 0)
            {
                throw new CustomException(passengerType.GetDescription() + "编码缺少乘机人信息");
            }
            if (reservedPnr.Passengers.First().Type != passengerType)
            {
                throw new CustomException(passengerType.GetDescription() + "编码错误(不是" + passengerType.GetDescription() + "编码)");
            }

            if (reservedPnr.Voyage.Segments == null || reservedPnr.Voyage.Segments.Count == 0)
            {
                throw new CustomException(passengerType.GetDescription() + "编码缺少航班信息");
            }
            var errorPassenger = reservedPnr.Passengers.FirstOrDefault(item => string.IsNullOrWhiteSpace(item.CertificateNumber));
            if (errorPassenger != null)
            {
                throw new CustomException(passengerType.GetDescription() + "编码乘机人 [" + errorPassenger.Name + "] 缺少有效的 SSR FOID 项");
            }

            foreach (var item in reservedPnr.Voyage.Segments)
            {
                if (item.Status == "DW")
                {
                    throw new PNRAlternateStateException(passengerType.GetDescription() + "订座记录处于候补状态");
                }
                if (!(item.Status.Contains("K") || item.Status == "RR" ||
                    (item.Status == "NN" && System.Configuration.ConfigurationManager.AppSettings["NNStatusAirline"].Contains(item.AirlineCode))))
                {
                    throw new SellOutException("用户所定航班已销售完毕");
                }
            }
            if (reservedPnr.IsTeam && reservedPnr.TotalNumber != reservedPnr.ActualNumber)
            {
                throw new CustomException("团队成员数量与团队编码成员数不一致");
            }
            if (reservedPnr.NeedFill)
            {
                throw new CustomException("缺口程编码，需要搭桥");
            }
        }

        /// <summary>
        /// 将待授权的旅客订座记录编号的权限授予指定的OfficeNo
        /// </summary>
        /// <param name="pnrPair">待授权的旅客订座记录编号</param>
        /// <param name="officeNo">待授权的OfficeNo</param>
        /// <param name="oemId"> </param>
        /// <returns></returns>
        /// <remarks>
        /// 使用在订好编码后，向出票方授权；
        /// </remarks>
        public static ExecuteResult<string> AuthorizeByOfficeNo(PNRPair pnrPair, string officeNo, Guid oemId)
        {
            // 参数验证
            if (PNRPair.IsNullOrEmpty(pnrPair))
            {
                throw new ArgumentException("旅客订座记录编码");
            }
            const ConfigUseType type = ConfigUseType.Reserve;
            var userName = GetUserName(oemId, type);
            
            var officeNumber = GetOfficeNumber(oemId, type);
            if (userName == null || officeNumber == null)
            {
                return new ExecuteResult<string>
                {
                    Result = null,
                    Success = false,
                    Message = "OEM未设置可用配置，且没有设置使用平台配置。"
                };
            }
            var repository = Factory.CreateCommandRepository();
            var result = repository.Authorize(pnrPair.PNR, officeNumber, new string[] { officeNo }, userName);
            if (result.Success)
            {
                PidManagementService.SaveCounter(oemId, true);
            }

            return result;
        }

        #region PNR信息
        /// <summary>
        /// 根据旅客订座记录编号，得到旅客信息。
        /// </summary>
        /// <param name="pnrPair">旅客订座记录编号</param>
        /// <returns>执行结果</returns>
        public static ExecuteResult<IEnumerable<Passenger>> GetTicketNumbersByPnr(PNRPair pnrPair, Guid oemId)
        {
            ExecuteResult<ReservedPnr> reservedPnr = GetReservedPnr(pnrPair, oemId);
            if (reservedPnr.Success)
            {
                return new ExecuteResult<IEnumerable<Passenger>>
                {
                    Success = true,
                    Message = reservedPnr.Message,
                    Result = reservedPnr.Result.Passengers
                };
            }
            else
            {
                return new ExecuteResult<IEnumerable<Passenger>>
                {
                    Success = false,
                    Message = reservedPnr.Message
                };
            }
        }

        ///// <summary>
        ///// 根据旅客订座记录编号，获取其详细信息。（此方法没有被外部调用）
        ///// </summary>
        ///// <param name="pnrCode">编码</param>
        ///// <returns>执行结果</returns>
        //private static ExecuteResult<IssuedPNR> GetTeamPNRDetail(string pnrCode)
        //{
        //    var rtnCommand = new RTNCommand(pnrCode);
        //    rtnCommand.Execute();
        //    var issuedPNR = Parser.GetPNRDetail(rtnCommand.ReturnString);
        //    return GetExecuteResult(issuedPNR, rtnCommand.ReturnString);
        //}

        ///// <summary>
        ///// 根据旅客订座记录编号，获取其详细信息。
        ///// </summary>
        ///// <param name="pnrPair">PNR</param>
        ///// <returns>执行结果</returns>
        //private static ExecuteResult<IssuedPNR> GetPNRDetail(PNRPair pnrPair, bool isTeam)
        //{
        //    if (PNRPair.IsNullOrEmpty(pnrPair))
        //    {
        //        throw new ArgumentException("编码错误");
        //    }
        //    // 优先通过大编码获取信息，
        //    return isTeam && !string.IsNullOrWhiteSpace(pnrPair.BPNR) ? GetTeamPNRDetail(pnrPair.BPNR) : GetPNRDetail(pnrPair);
        //}

        ///// <summary>
        ///// 根据旅客订座记录编号，获取其详细信息。
        ///// </summary>
        ///// <param name="pnrPair">PNR</param>
        ///// <returns>执行结果</returns>
        //private static ExecuteResult<IssuedPNR> GetPNRDetail(PNRPair pnrPair)
        //{
        //    if (PNRPair.IsNullOrEmpty(pnrPair))
        //    {
        //        throw new ArgumentException("编码错误");
        //    }
            
        //    var commandString = string.Empty;
        //    IssuedPNR issuedPNR = null;

        //    if (!string.IsNullOrWhiteSpace(pnrPair.BPNR))
        //    {
        //        var rtCommand = new RTCommand(pnrPair.BPNR, CommandType.PNRExtraction, ReturnResultType.All);
        //        rtCommand.Execute();
        //        commandString = rtCommand.ReturnString;
        //    }
        //    else
        //    {
        //        var rtxCommand = new RTXCommand(pnrPair.PNR);
        //        rtxCommand.Execute();
        //        commandString = rtxCommand.ReturnString;
        //    }

        //    switch (Parser.GetPassengerConsistsType(commandString))
        //    {
        //        case PassengerConsistsType.Individual:
        //            issuedPNR = Parser.GetPNRDetail(commandString);
        //            break;
        //        case PassengerConsistsType.Group:
        //            // 通过上面的数据，解析出PNRPair；
        //            PNRPair result = Parser.GetPnrPair(commandString);
        //            // 这里用大编提取数据，切记切记，用小编提取不到；
        //            return GetTeamPNRDetail(string.IsNullOrWhiteSpace(result.BPNR) ? result.PNR : result.BPNR);
        //        default:
        //            break;
        //    }

        //    return GetExecuteResult<IssuedPNR>(issuedPNR, commandString);
        //}
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pnrPair"></param>
        /// <param name="oemId"> </param>
        /// <returns></returns>
        /// <remarks>
        /// 由于在订座后立即提取了编码信息，所在所有的调用此方法时都应该是用大系统提取，所以，这里先写死了；
        /// </remarks>
        public static ExecuteResult<ReservedPnr> GetReservedPnr(PNRPair pnrPair, Guid oemId)
        {
            if (PNRPair.IsNullOrEmpty(pnrPair)) throw new ArgumentException("编码错误");

            var userName = "9000";
            
            ExecuteResult<ReservedPnr> reservedPnrInfo;
            var repository = Factory.CreateCommandRepository();

            // 防止出现问题，检查输入编码；
            pnrPair = Domain.Utility.Parser.SwitchPnrPair(pnrPair);

            if (!string.IsNullOrWhiteSpace(pnrPair.BPNR))//若大系统编码为空，则使用大系统提大编码
            {
                reservedPnrInfo = repository.Rt(pnrPair.BPNR, userName);
            }
            else
            {
                reservedPnrInfo = repository.Rtx(pnrPair.PNR, userName);
            }

            if (reservedPnrInfo.Success)
            {
                PidManagementService.SaveCounter(oemId, true);
            }

            return reservedPnrInfo;
        }

        /// <summary>
        /// 根据内容，转换成订座后的旅客订座信息。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ExecuteResult<ReservedPnr> GetReservedPnr(string str)
        {
            ReservedPnr reservedPnr = Domain.Utility.Parser.GetPnrContent(str);
            if (reservedPnr != null)
            {
                return new ExecuteResult<ReservedPnr>
                           {
                               Success = true,
                               Result = reservedPnr,
                               Message = str
                           };
            }

            return new ExecuteResult<ReservedPnr>
                                    {
                                        Success = false,
                                        Message = str
                                    };
        }

        /// <summary>
        /// 查询订票后价格
        /// </summary>
        /// <returns>执行结果</returns>
        public static ExecuteResult<IEnumerable<PriceView>> QueryPriceByPNR(PNRPair pnrPair, PassengerType passengerType, Guid oemId)
        {
            // 参数验证
            if (PNRPair.IsNullOrEmpty(pnrPair))
            {
                throw new ArgumentException("旅客订座记录编码");
            }

            // 这里到底是执行了RT还是RTX呢，PAT现在只在执行订座时会提，所以提取时应使用的订座配置；
            const ConfigUseType type = ConfigUseType.Reserve;
            var userName = GetUserName(oemId, type);
            if (userName == null )
            {
                return new ExecuteResult<IEnumerable<PriceView>>
                {
                    Result = null,
                    Success = false,
                    Message = "OEM未设置可用配置，且没有设置使用平台配置。"
                };
            }

            var repository = Factory.CreateCommandRepository();
            var result =  repository.PnrPat(pnrPair.PNR, passengerType, userName);
            if (result.Success)
            {
                PidManagementService.SaveCounter(oemId, HasUsePlatformConfig(oemId, type));
            }

            return result;
        }

        #region 邮箱处理

        //注意，这里用的是VeWeb
        /// <summary>
        /// 获取邮箱列表
        /// </summary>
        /// <param name="oemId"> </param>
        /// <returns></returns>
        public static List<QueueSummary> GetMailList(Guid oemId)
        {
            const ConfigUseType type = ConfigUseType.QS;
            var userName = GetUserName(oemId, type);
            var officeNumber = GetOfficeNumber(oemId, type);
            if (userName == null || officeNumber == null)
            {
               return new List<QueueSummary>();
            }
            var repository = Factory.CreateVeWebCommandRepository();
            PidManagementService.SaveCounter(oemId, HasUsePlatformConfig(oemId, type));
            return repository.Qt(userName, "123", officeNumber);
        }

        /// <summary>
        /// 根据邮箱类型，开始处理这种类型的邮箱，并得到此邮箱中的第一封信。
        /// </summary>
        /// <param name="queueType">邮箱类型</param>
        /// <param name="oemId"> </param>
        /// <returns>信件内容</returns>
        public static string StartProcessQueueAndGetTheFirst(QueueType queueType, Guid oemId)
        {
            const ConfigUseType type = ConfigUseType.QS;
            var userName = GetUserName(oemId, type);
            if (userName == null)
            {
                return "";
            }
            PidManagementService.SaveCounter(oemId, HasUsePlatformConfig(oemId, type));
            var repository = Factory.CreateVeWebCommandRepository();
            return repository.Qs(queueType, userName, "123");
        }

        /// <summary>
        /// 获取下一封信箱内容，同时根据参数，决定当前邮件是否删除。
        /// </summary>
        /// <param name="deleteCurrentQueue">是否删除当前邮件</param>
        /// <param name="oemId"> </param>
        /// <returns>信件内容</returns>
        public static string GetNextQueue(bool deleteCurrentQueue,Guid oemId)
        {
            const ConfigUseType type = ConfigUseType.QS;
            var userName = GetUserName(oemId, type);
            if (userName == null)
            {
                return "";
            }

            var repository = Factory.CreateVeWebCommandRepository();
            const string option = "";
            PidManagementService.SaveCounter(oemId, HasUsePlatformConfig(oemId, type));
            return deleteCurrentQueue ? repository.Qn(option, userName, "123") : repository.Qd(option, userName, "123");
        }

        /// <summary>
        /// 结束信箱处理，并根据参数，决定当前邮件是否删除。
        /// </summary>
        /// <param name="deleteCurrentQueue">是否删除当前信件</param>
        /// <param name="oemId"> </param>
        public static void EndProcessQueue(bool deleteCurrentQueue, Guid oemId)
        {
            const ConfigUseType type = ConfigUseType.QS;
            var userName = GetUserName(oemId, type);
            if (userName == null)
            {
                return;
            }
            PidManagementService.SaveCounter(oemId, HasUsePlatformConfig(oemId, type));
            var repository = Factory.CreateVeWebCommandRepository();
            const string option = "E";
            var returnString = deleteCurrentQueue ? repository.Qn(option, userName, "123") : repository.Qd(option, userName, "123");
        }
        
        /// <summary>
        /// 获取航班变动信息列表，并根据参数，决定是否保留此信箱信件。
        /// </summary>
        /// <param name="deleteCurrentQueue">是否删除信件</param>
        /// <returns>信件列表</returns>
        public static List<Service.Queue.Domain.Queue> GetScheduleChangeQueue(bool deleteCurrentQueue, Guid oemId)
        {
            const  ConfigUseType type = ConfigUseType.QS;
            var officeNumber = GetOfficeNumber(oemId, type);
            // 设定处理的邮箱类型为航班变动；
            const QueueType queueType = QueueType.SC;
            // 获取邮件列表；
            var queueSummaries = GetMailList(oemId);
            // 获取航班变动信箱信息；
            var scQueue = (from q in queueSummaries
                           where q.Name == "SC"
                           select q).FirstOrDefault();
            
            var queues = new List<Service.Queue.Domain.Queue>();

            // 若有未处理邮件，则执行清理操作；
            if (scQueue != null && scQueue.UnprocessedNumber > 0)
            {
                // 打开航班变更信箱，并获取第一封信；
                var rawData = StartProcessQueueAndGetTheFirst(queueType,oemId);
                var count = scQueue.UnprocessedNumber;

                // 循环取得信件；
                var now = DateTime.Now;
                while (count > 0 && Service.Queue.Domain.Queue.Validate(rawData, officeNumber))
                {
                    var queue = new Service.Queue.Domain.Queue(now, count, rawData, queueType, officeNumber);
                    queues.Add(queue);
                    rawData = GetNextQueue(deleteCurrentQueue, oemId);
                    count--;
                }

                EndProcessQueue(deleteCurrentQueue, oemId);
            }

            return queues;
        }
        
        #endregion

        /// <summary>
        /// 获取指定航空公司下的某城市对的运价信息。
        /// </summary>
        /// <param name="airportPair">机场对</param>
        /// <param name="flightDate">航班时间</param>
        /// <param name="carrier">承运人</param>
        /// <param name="oemId">OEM编号</param>
        /// <returns>运价</returns>
        public static ExecuteResult<AirportPairFares> GetFare(AirportPair airportPair, DateTime flightDate, string carrier, Guid oemId)
        {
            const ConfigUseType type = ConfigUseType.Reserve;
            var userName =GetUserName(oemId, type);

            var repository = Factory.CreateCommandRepository();
            return repository.Fd(airportPair, flightDate, carrier, userName);
        }

        /// <summary>
        /// 根据给出的电子客票票号，出发机场和OEM编号，获取电子客票信息
        /// </summary>
        /// <param name="eTicketNumber">电子客票票号</param>
        /// <param name="oemId">OEM编号</param>
        /// <returns>执行结果</returns>
        public static ExecuteResult<ETicketInfo> GetTicketDetails(string eTicketNumber, Guid oemId)
        {
            if (!ElectronicTicket.ValidateTicketNumber(eTicketNumber)) throw new ArgumentException("电子客票号码格式错误！");
            var userName = "9000";

            var repository = Factory.CreateVeWebCommandRepository();

            var eTicketResult = repository.Detr(eTicketNumber, userName);
            var jSheetResult = repository.Detrf(eTicketNumber, userName);

            var status = DetrErrorStatus.None;
            
            // 这里是两次请求的状态都要处理；
            if (!eTicketResult.Success)
            {
                if (eTicketResult.Message.Contains("没有权限") || eTicketResult.Message.Contains("AUTHORITY"))
                {
                    status = DetrErrorStatus.Authority;
                }
                else if (eTicketResult.Message.Contains("ET TICKET NUMBER IS NOT EXIST"))
                {
                    status = DetrErrorStatus.TickerNumber;
                }
                else
                {
                    status = DetrErrorStatus.Format;
                }
            }
            
            var result = new ExecuteResult<ETicketInfo>
                       {
                           Result = new ETicketInfo
                                        {
                                            JourneySheet = jSheetResult.Result,
                                            ElectronicTicket = eTicketResult.Result,
                                            Status = status
                                        },
                           Message = "DETR:" + eTicketResult.Message + ", DETRF" + jSheetResult.Message,
                           Success = eTicketResult.Success && jSheetResult.Success
                       };

            if (result.Success)
            {
                PidManagementService.SaveCounter(oemId, true);
            }
            return result;
        }
        
    }
}
