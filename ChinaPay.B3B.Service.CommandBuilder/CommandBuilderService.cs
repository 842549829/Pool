using System;
using System.Collections.Generic;
using System.Globalization;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.CommandBuilder.Domain;
using ChinaPay.B3B.Service.CommandBuilder.Domain.Book;
using System.Linq;
using ChinaPay.B3B.Service.CommandBuilder.Domain.PNR;
using ChinaPay.B3B.Service.CommandBuilder.Domain.Ticket;

namespace ChinaPay.B3B.Service.CommandBuilder
{
    /// <summary>
    /// 指令生成服务类
    /// </summary>
    public class CommandBuilderService
    {
        /// <summary>
        /// 通过旅客订座信息和订座的代理人编号，获取订座指令
        /// </summary>
        /// <param name="info">旅客订座信息</param>
        /// <param name="officeNo">代理人编号</param>
        /// <returns>指令内容</returns>
        public static string GetBookInstructionSet(ReservationInfo info, string officeNo)
        {
            var commandSet = new CommandSet();
            List<string> names = (from n in info.Passengers
                                  select n.Name).ToList();
            string carrier = info.Segements.First().Carrier;
            PassengerType passengerType = info.Passengers.First().Type;
            var firstFlightDate = info.Segements.Min(p => p.Date);

            // 航段组
            foreach (var item in info.Segements)
            {
                var ssCommand = new SsCommand(item.Number, item.ClassOfService, item.Date,
                                              new AirportPair(item.DepartureAirportCode, item.ArrivalAirportCode), 
                                              names.Count);
                commandSet.Add(ssCommand);
            }
            
            // 姓名组，南航的儿童票在特殊服务组中处理；
            var nmCommand = new NmCommand(names, passengerType, carrier);
            commandSet.Add(nmCommand);

            // 联系组（代理人）
            var ctCommand = new CtCommand(info.AgentPhoneNumber);
            commandSet.Add(ctCommand);

            // 特殊服务组（身份证号）和其它服务组（旅客联系方式）
            int count = 0;
            
            foreach (ReservationPassengerInfo passenger in info.Passengers)
            {
                var ssrFoidCommand = new SsrCommand(carrier, passenger.CertificateNumber, count + 1,SpecialServiceRequirementType.FOID);
                commandSet.Add(ssrFoidCommand);

                // 若不在东航、联合、昆航、上航之列，则按下面的执行，否则只在后面执行一次；
                if (!(carrier == "MU" || carrier == "FM" || carrier == "KN" || carrier == "KY"))
                {
                    var osiCommand = new OsiCommand(carrier, passenger.MobilephoneNumber, count + 1, OtherServiceInformationType.CTCT);
                    commandSet.Add(osiCommand);
                }
                
                // 南航儿童票
                if (passengerType == PassengerType.Child && carrier.ToUpper() == "CZ")
                {
                    if (passenger.Birthday == null)
                    {
                        throw new ArgumentNullException();
                    }
                    var ssrChldCommand = new SsrCommand(carrier, passenger.Birthday.Value.ToString("ddMMMyy", CultureInfo.CreateSpecificCulture("en-US")), count + 1, SpecialServiceRequirementType.CHLD);
                    commandSet.Add(ssrChldCommand);
                }
                count++;
            }

            // 2013-03-26 根据东航的规定，改成这样；
            if (carrier == "MU" || carrier == "FM" || carrier == "KN" || carrier == "KY")
            {
                // 这里的旅客编号没有用；
                var osiCommand = new OsiCommand(carrier, info.Passengers[0].MobilephoneNumber, 0, OtherServiceInformationType.CTCT);
                commandSet.Add(osiCommand);
            }

            // 票号组
            var tkCommand = new TkCommand(firstFlightDate.AddMinutes(-30), officeNo);
            commandSet.Add(tkCommand);
            
            // 封口指令
            var eotCommand = new EotCommand();
            commandSet.Add(eotCommand);

            return commandSet.CommandString;
        }
        
        /// <summary>
        /// 根据旅客类型，获取PAT指令
        /// </summary>
        /// <param name="passengerType"></param>
        /// <returns></returns>
        public static string GetPatInstruction(PassengerType passengerType)
        {
            var patCommand = new PatCommand(passengerType);
            return patCommand.CommandString;
        }
        
        /// <summary>
        /// 根据待取消航段的行号，获取取消旅客订座记录航段项的指令字串。
        /// </summary>
        /// <param name="lineNumbers">待取消航段的行号</param>
        /// <returns>指令字串</returns>
        public static string GetCancelSegmentInstruction(int[] lineNumbers)
        {
            var xeCommad = new XeCommad(lineNumbers);
            return xeCommad.CommandString;
        }

        /// <summary>
        /// 根据待取消旅客的行号，获取取消旅客订座记录姓名项的指令字串。
        /// </summary>
        /// <param name="lineNumbers">待取消旅客的行号</param>
        /// <returns>指令字串</returns>
        public static string GetCancelPassengerInstruction(int[] lineNumbers)
        {
            var xepCommad = new XepCommad(lineNumbers);
            return xepCommad.CommandString;
        }

        /// <summary>
        /// 根据查询类型和相应的查询字串，获取查询票号的指令字串。
        /// </summary>
        /// <param name="queryStr">查询字串</param>
        /// <param name="queryType">查询类型</param>
        /// <param name="option">选项</param>
        /// <returns>指令字串</returns>
        /// <remarks>
        /// option参数如果采用S或F选项，则会得到行程单的信息；
        /// </remarks>
        public  static  string GetDetrInstrction(string queryStr, DetrQeeryType queryType, string option = "")
        {
            var detrCommand = new DetrCommand(queryStr, queryType, option);
            return detrCommand.CommandString;
        }
    }
}
