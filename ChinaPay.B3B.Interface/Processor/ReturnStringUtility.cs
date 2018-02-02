using System;
using System.Linq;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Interface.PublicClass;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Policy;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Service.PolicyMatch.Domain;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.Order.Domain;

namespace ChinaPay.B3B.Interface.Processor
{
    public class ReturnStringUtility
    {
        /// <summary>
        /// 得到查询申请单的返回格式
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetApplyform(BaseApplyform obj)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<applyform><title>");
            //退票申请
            if (obj is RefundApplyform)
            {
                var applyform = obj as RefundApplyform;
                str.AppendFormat("<id>{0}</id>", applyform.Id);
                str.AppendFormat("<applyType>{0}</applyType>", "1");
                str.AppendFormat("<status>{0}</status>", (int)applyform.Status);
                str.AppendFormat("<statusDescription>{0}</statusDescription>", Service.Order.StatusService.GetRefundApplyformStatus(applyform.Status, OrderRole.Purchaser));
                str.AppendFormat("<refundType>{0}</refundType>", (int)applyform.RefundType);
                str.AppendFormat("<originalPNR>{0}</originalPNR>", applyform.OriginalPNR == null ? "" : applyform.OriginalPNR.BPNR + "|" + applyform.OriginalPNR.PNR);
                str.AppendFormat("<newPNR>{0}</newPNR>", applyform.NewPNR == null ? "" : applyform.NewPNR.BPNR + "|" + applyform.NewPNR.PNR);
                str.AppendFormat("<amount>{0}</amount>", applyform.Status == RefundApplyformStatus.Refunded ? FormatUtility.FormatAmount(applyform.RefundBill.Purchaser.Amount) : "");//
                str.AppendFormat("<applyTime>{0}</applyTime>", FormatUtility.FormatDatetime(applyform.AppliedTime));
                str.AppendFormat("<payTime>{0}</payTime>", "");//退票没有支付时间
                str.AppendFormat("<processedTime>{0}</processedTime>", FormatUtility.FormatDatetime(applyform.ProcessedTime));
                str.Append("</title><passengers>");
                foreach (var item in applyform.Passengers)
                {
                    str.Append("<p>");
                    str.AppendFormat("<name>{0}</name>", item.Name);
                    str.AppendFormat("<type>{0}</type>", (int)item.PassengerType);
                    str.AppendFormat("<credentitals>{0}</credentitals>", item.Credentials);
                    str.AppendFormat("<mobile>{0}</mobile>", item.Phone);
                    str.AppendFormat("<settleCode>{0}</settleCode>", item.Tickets.FirstOrDefault().SettleCode);
                    str.AppendFormat("<tickets>{0}</tickets>", item.Tickets.Join("|", num => num.No));
                    str.Append("</p>");
                }
                str.Append("</passengers><flights>");
                foreach (var item in applyform.Flights)
                {
                    str.Append("<f>");
                    str.AppendFormat("<departure>{0}</departure>", item.OriginalFlight.Departure.Code);
                    str.AppendFormat("<arrival>{0}</arrival>", item.OriginalFlight.Arrival.Code);
                    str.AppendFormat("<flightNo>{0}</flightNo>", item.OriginalFlight.FlightNo);
                    str.AppendFormat("<aircraft>{0}</aircraft>", item.OriginalFlight.AirCraft);
                    str.AppendFormat("<takeoffTime>{0}</takeoffTime>", FormatUtility.FormatFlightDatetime(item.OriginalFlight.TakeoffTime));
                    str.AppendFormat("<arrivalTime>{0}</arrivalTime>", FormatUtility.FormatFlightDatetime(item.OriginalFlight.LandingTime));
                    str.AppendFormat("<bunk>{0}</bunk>", item.OriginalFlight.Bunk.Code);
                    str.AppendFormat("<fare>{0}</fare>", FormatUtility.FormatAmount(item.OriginalFlight.Price.Fare));
                    str.AppendFormat("<discount>{0}</discount>", item.OriginalFlight.Bunk.Discount.TrimInvaidZero());
                    str.AppendFormat("<airportFee>{0}</airportFee>", FormatUtility.FormatAmount(item.OriginalFlight.AirportFee));
                    str.AppendFormat("<baf>{0}</baf>", FormatUtility.FormatAmount(item.OriginalFlight.Price.BAF));
                    str.AppendFormat("<refundRate>{0}</refundRate>", applyform.Status == RefundApplyformStatus.Refunded ? item.RefundRate.ToString() : "");
                    str.AppendFormat("<refundFee>{0}</refundFee>", applyform.Status == RefundApplyformStatus.Refunded ? FormatUtility.FormatAmount(applyform.RefundBill.Purchaser.Source.Details.FirstOrDefault(o => o.Flight.Id == item.OriginalFlight.Id) != null ? applyform.RefundBill.Purchaser.Source.Details.First(o => o.Flight.Id == item.OriginalFlight.Id).RefundFee : 0) : "");
                    str.AppendFormat("<refundServiceCharge>{0}</refundServiceCharge>", FormatUtility.FormatAmount(item.RefundServiceCharge));
                    str.AppendFormat("<newFlightNo>{0}</newFlightNo>", "");
                    str.AppendFormat("<newAircraft>{0}</newAircraft>", "");
                    str.AppendFormat("<newTakeoffTime>{0}</newTakeoffTime>", "");
                    str.AppendFormat("<newArrivalTime>{0}</newArrivalTime>", "");
                    str.AppendFormat("<postponeFee>{0}</postponeFee>", "");
                    str.Append("</f>");
                }
                str.Append("</flights><bills>");
                if (applyform.RefundBill != null && applyform.Status == RefundApplyformStatus.Refunded)
                {
                    str.AppendFormat("<b><type>{0}</type>", "2");
                    str.AppendFormat("<status>{0}</status>", applyform.RefundBill.Succeed ? "1" : "0");
                    str.AppendFormat("<amount>{0}</amount>", FormatUtility.FormatAmount(applyform.RefundBill.Tradement.Amount));
                    str.AppendFormat("<tradeNo>{0}</tradeNo>", applyform.RefundBill.Tradement.TradeNo);
                    str.AppendFormat("<channelTradeNo>{0}</channelTradeNo>","");
                    str.AppendFormat("<time>{0}</time>", FormatUtility.FormatDatetime(applyform.RefundBill.Purchaser.Time));
                    str.Append("</b>");
                }
                str.Append("</bills>");
            }
            //废票申请
            if (obj is ScrapApplyform)
            {
                var applyform = obj as ScrapApplyform;
                str.AppendFormat("<id>{0}</id>", applyform.Id);
                str.AppendFormat("<applyType>{0}</applyType>", "2");
                str.AppendFormat("<status>{0}</status>", (int)applyform.Status);
                str.AppendFormat("<statusDescription>{0}</statusDescription>", Service.Order.StatusService.GetRefundApplyformStatus(applyform.Status, OrderRole.Purchaser));
                str.AppendFormat("<refundType>{0}</refundType>", "");
                str.AppendFormat("<originalPNR>{0}</originalPNR>", applyform.OriginalPNR == null ? "" : applyform.OriginalPNR.BPNR + "|" + applyform.OriginalPNR.PNR);
                str.AppendFormat("<newPNR>{0}</newPNR>", applyform.NewPNR == null ? "" : applyform.NewPNR.BPNR + "|" + applyform.NewPNR.PNR);
                str.AppendFormat("<amount>{0}</amount>", applyform.Status == RefundApplyformStatus.Refunded ? FormatUtility.FormatAmount(applyform.RefundBill.Purchaser.Amount) : "");//
                str.AppendFormat("<applyTime>{0}</applyTime>", FormatUtility.FormatDatetime(applyform.AppliedTime));
                str.AppendFormat("<payTime>{0}</payTime>", "");//退票没有支付时间
                str.AppendFormat("<processedTime>{0}</processedTime>", FormatUtility.FormatDatetime(applyform.ProcessedTime));
                str.Append("</title><passengers>");
                foreach (var item in applyform.Passengers)
                {
                    str.Append("<p>");
                    str.AppendFormat("<name>{0}</name>", item.Name);
                    str.AppendFormat("<type>{0}</type>", (int)item.PassengerType);
                    str.AppendFormat("<credentitals>{0}</credentitals>", item.Credentials);
                    str.AppendFormat("<mobile>{0}</mobile>", item.Phone);
                    str.AppendFormat("<settleCode>{0}</settleCode>", item.Tickets.FirstOrDefault().SettleCode);
                    str.AppendFormat("<tickets>{0}</tickets>", item.Tickets.Join("|", num => num.No));
                    str.Append("</p>");
                }
                str.Append("</passengers><flights>");
                foreach (var item in applyform.Flights)
                {
                    str.Append("<f>");
                    str.AppendFormat("<departure>{0}</departure>", item.OriginalFlight.Departure.Code);
                    str.AppendFormat("<arrival>{0}</arrival>", item.OriginalFlight.Arrival.Code);
                    str.AppendFormat("<flightNo>{0}</flightNo>", item.OriginalFlight.FlightNo);
                    str.AppendFormat("<aircraft>{0}</aircraft>", item.OriginalFlight.AirCraft);
                    str.AppendFormat("<takeoffTime>{0}</takeoffTime>", FormatUtility.FormatFlightDatetime(item.OriginalFlight.TakeoffTime));
                    str.AppendFormat("<arrivalTime>{0}</arrivalTime>", FormatUtility.FormatFlightDatetime(item.OriginalFlight.LandingTime));
                    str.AppendFormat("<bunk>{0}</bunk>", item.OriginalFlight.Bunk.Code);
                    str.AppendFormat("<fare>{0}</fare>", FormatUtility.FormatAmount(item.OriginalFlight.Price.Fare));
                    str.AppendFormat("<discount>{0}</discount>", item.OriginalFlight.Bunk.Discount.TrimInvaidZero());
                    str.AppendFormat("<airportFee>{0}</airportFee>", FormatUtility.FormatAmount(item.OriginalFlight.AirportFee));
                    str.AppendFormat("<baf>{0}</baf>", FormatUtility.FormatAmount(item.OriginalFlight.Price.BAF));
                    str.AppendFormat("<refundRate>{0}</refundRate>", applyform.Status == RefundApplyformStatus.Refunded ? item.RefundRate.ToString() : "");
                    str.AppendFormat("<refundFee>{0}</refundFee>", applyform.Status == RefundApplyformStatus.Refunded ? FormatUtility.FormatAmount(applyform.RefundBill.Purchaser.Source.Details.First(o => o.Flight.Id == item.OriginalFlight.Id).RefundFee) : "");
                    str.AppendFormat("<refundServiceCharge>{0}</refundServiceCharge>", FormatUtility.FormatAmount(item.RefundServiceCharge));
                    str.AppendFormat("<newFlightNo>{0}</newFlightNo>", "");
                    str.AppendFormat("<newAircraft>{0}</newAircraft>", "");
                    str.AppendFormat("<newTakeoffTime>{0}</newTakeoffTime>", "");
                    str.AppendFormat("<newArrivalTime>{0}</newArrivalTime>", "");
                    str.AppendFormat("<postponeFee>{0}</postponeFee>", "");
                    str.Append("</f>");
                }
                str.Append("</flights><bills>");
                if (applyform.RefundBill != null && applyform.Status == RefundApplyformStatus.Refunded)
                {
                    str.AppendFormat("<b><type>{0}</type>", "2");
                    str.AppendFormat("<status>{0}</status>", applyform.RefundBill.Succeed ? "1" : "0");
                    str.AppendFormat("<amount>{0}</amount>", FormatUtility.FormatAmount(applyform.RefundBill.Tradement.Amount));
                    str.AppendFormat("<tradeNo>{0}</tradeNo>", applyform.RefundBill.Tradement.TradeNo);
                    str.AppendFormat("<channelTradeNo>{0}</channelTradeNo>", "");
                    str.AppendFormat("<time>{0}</time>", FormatUtility.FormatDatetime(applyform.RefundBill.Purchaser.Time));
                    str.Append("</b>");
                }
                str.Append("</bills>");
            }
            //改期申请
            if (obj is PostponeApplyform)
            {
                var applyform = obj as PostponeApplyform;
                str.AppendFormat("<id>{0}</id>", applyform.Id);
                str.AppendFormat("<applyType>{0}</applyType>", "4");
                str.AppendFormat("<status>{0}</status>", (int)applyform.Status);
                str.AppendFormat("<statusDescription>{0}</statusDescription>", Service.Order.StatusService.GetPostponeApplyformStatus(applyform.Status, OrderRole.Purchaser));
                str.AppendFormat("<refundType>{0}</refundType>", "");
                str.AppendFormat("<originalPNR>{0}</originalPNR>", applyform.OriginalPNR == null ? "" : applyform.OriginalPNR.BPNR + "|" + applyform.OriginalPNR.PNR);
                str.AppendFormat("<newPNR>{0}</newPNR>", applyform.NewPNR == null ? "" : applyform.NewPNR.BPNR + "|" + applyform.NewPNR.PNR);
                str.AppendFormat("<amount>{0}</amount>", applyform.Status == PostponeApplyformStatus.Applied || applyform.PayBill == null ? "" : FormatUtility.FormatAmount(applyform.PayBill.Applier.Amount));//
                str.AppendFormat("<applyTime>{0}</applyTime>", FormatUtility.FormatDatetime(applyform.AppliedTime));
                str.AppendFormat("<payTime>{0}</payTime>", applyform.Status == PostponeApplyformStatus.Applied || applyform.PayBill == null ? "" : FormatUtility.FormatDatetime(applyform.PayBill.Applier.Time));
                str.AppendFormat("<processedTime>{0}</processedTime>", FormatUtility.FormatDatetime(applyform.ProcessedTime));
                str.Append("</title><passengers>");
                foreach (var item in applyform.Passengers)
                {
                    str.Append("<p>");
                    str.AppendFormat("<name>{0}</name>", item.Name);
                    str.AppendFormat("<type>{0}</type>", (int)item.PassengerType);
                    str.AppendFormat("<credentitals>{0}</credentitals>", item.Credentials);
                    str.AppendFormat("<mobile>{0}</mobile>", item.Phone);
                    str.AppendFormat("<settleCode>{0}</settleCode>", item.Tickets.FirstOrDefault().SettleCode);
                    str.AppendFormat("<tickets>{0}</tickets>", item.Tickets.Join("|", num => num.No));
                    str.Append("</p>");
                }
                str.Append("</passengers><flights>");
                foreach (var item in applyform.Flights)
                {
                    str.Append("<f>");
                    str.AppendFormat("<departure>{0}</departure>", item.OriginalFlight.Departure.Code);
                    str.AppendFormat("<arrival>{0}</arrival>", item.OriginalFlight.Arrival.Code);
                    str.AppendFormat("<flightNo>{0}</flightNo>", item.OriginalFlight.FlightNo);
                    str.AppendFormat("<aircraft>{0}</aircraft>", item.OriginalFlight.AirCraft);
                    str.AppendFormat("<takeoffTime>{0}</takeoffTime>", FormatUtility.FormatFlightDatetime(item.OriginalFlight.TakeoffTime));
                    str.AppendFormat("<arrivalTime>{0}</arrivalTime>", FormatUtility.FormatFlightDatetime(item.OriginalFlight.LandingTime));
                    str.AppendFormat("<bunk>{0}</bunk>", item.OriginalFlight.Bunk.Code);
                    str.AppendFormat("<fare>{0}</fare>", FormatUtility.FormatAmount(item.OriginalFlight.Price.Fare));
                    str.AppendFormat("<discount>{0}</discount>", item.OriginalFlight.Bunk.Discount.TrimInvaidZero());
                    str.AppendFormat("<airportFee>{0}</airportFee>", FormatUtility.FormatAmount(item.OriginalFlight.AirportFee));
                    str.AppendFormat("<baf>{0}</baf>", FormatUtility.FormatAmount(item.OriginalFlight.Price.BAF));
                    str.AppendFormat("<refundRate>{0}</refundRate>", "");
                    str.AppendFormat("<refundFee>{0}</refundFee>", "");
                    str.AppendFormat("<refundServiceCharge>{0}</refundServiceCharge>", "");
                    str.AppendFormat("<newFlightNo>{0}</newFlightNo>", item.NewFlight.FlightNo);
                    str.AppendFormat("<newAircraft>{0}</newAircraft>", item.NewFlight.AirCraft);
                    str.AppendFormat("<newTakeoffTime>{0}</newTakeoffTime>", FormatUtility.FormatFlightDatetime(item.NewFlight.TakeoffTime));
                    str.AppendFormat("<newArrivalTime>{0}</newArrivalTime>", FormatUtility.FormatFlightDatetime(item.NewFlight.LandingTime));
                    str.AppendFormat("<postponeFee>{0}</postponeFee>", FormatUtility.FormatAmount(item.PostponeFee));
                    str.Append("</f>");
                }
                str.Append("</flights><bills>");
                if (applyform.PayBill != null)
                {
                    str.AppendFormat("<b><type>{0}</type>", "1");
                    str.AppendFormat("<status>{0}</status>", applyform.PayBill.Applier.Success ? "1" : "0");
                    str.AppendFormat("<amount>{0}</amount>", FormatUtility.FormatAmount(applyform.PayBill.Tradement.Amount));
                    str.AppendFormat("<tradeNo>{0}</tradeNo>", applyform.PayBill.Tradement.TradeNo);
                    str.AppendFormat("<channelTradeNo>{0}</channelTradeNo>", applyform.PayBill.Tradement.ChannelTradeNo);
                    str.AppendFormat("<time>{0}</time>", FormatUtility.FormatDatetime(applyform.PayBill.Applier.Time));
                    str.Append("</b>");

                    if (applyform.PayBill.RefundBill != null)
                    {
                        str.AppendFormat("<b><type>{0}</type>", "2");
                        str.AppendFormat("<status>{0}</status>", applyform.PayBill.RefundBill.Applier.Success ? "1" : "0");
                        str.AppendFormat("<amount>{0}</amount>", FormatUtility.FormatAmount(applyform.PayBill.RefundBill.Tradement.Amount));
                        str.AppendFormat("<tradeNo>{0}</tradeNo>", applyform.PayBill.RefundBill.Tradement.TradeNo);
                        str.AppendFormat("<channelTradeNo>{0}</channelTradeNo>", "");
                        str.AppendFormat("<time>{0}</time>", FormatUtility.FormatDatetime(applyform.PayBill.RefundBill.Applier.Time));
                        str.Append("</b>");
                    }
                }
                str.Append("</bills>");
            }
            str.Append("</applyform>");
            return str.ToString();
        }
        public static void releaseLock(decimal orderId, CompanyDetailInfo company, EmployeeDetailInfo employee)
        {
            Service.LockService.UnLock(orderId.ToString(), employee.UserName);
        }
        public static bool Lock(decimal key, Service.Locker.LockRole lockRole, CompanyDetailInfo company, EmployeeDetailInfo employee, string remark, out string errorMsg)
        {
            var lockInfo = new Service.Locker.LockInfo(key.ToString())
            {
                LockRole = lockRole,
                Company = company.CompanyId,
                CompanyName = company.AbbreviateName,
                Account = employee.UserName,
                Name = employee.Name,
                Remark = remark
            };
            return Service.LockService.Lock(lockInfo, out errorMsg);
        }
        public static void GetPolicy(List<MatchedPolicy> matchedPolicies, List<MatchedPolicy> matchedSpeciafPolicies, StringBuilder str, PolicyType policyType, List<DataTransferObject.FlightQuery.FlightView> flights, Service.Organization.Domain.ExternalInterfaceSetting interfaceSetting)
        {
            if ((policyType & PolicyType.Special) != PolicyType.Special)
            {
                var list = from item in matchedPolicies
                           let generalPolicy = item.OriginalPolicy as IGeneralPolicy
                           let regulation = item.OriginalPolicy as IHasRegulation
                           select new
                           {
                               Id = item.Id,
                               Type = item.PolicyType == PolicyType.BargainDefault ? (int)PolicyType.Bargain : item.PolicyType == PolicyType.NormalDefault ? (int)PolicyType.Normal : (int)item.PolicyType,
                               Fare = item.ParValue == 100000 ? "" : FormatUtility.FormatAmount(item.ParValue),
                               Rebate = FormatUtility.FormatAmount(item.Commission * 100),
                               Commission = item.ParValue == 100000 ? "" : FormatUtility.FormatAmount(item.ParValue - item.SettleAmount),
                               Amount = FormatUtility.FormatAmount(item.SettleAmount),
                               Ticket = (int)(item.OriginalPolicy == null ? TicketType.BSP : item.OriginalPolicy.TicketType),
                               EI = ReplaceEnter((regulation == null ? getEI(flights) : getProvision(regulation))),
                               OfficeNo = item.OriginalPolicy == null && item.NeedAUTH ? item.OfficeNumber : (item.OriginalPolicy != null && item.OriginalPolicy.NeedAUTH ? item.OriginalPolicy.OfficeCode : ""),
                               Condition = item.OriginalPolicy == null ? "" : item.OriginalPolicy.Condition,
                               Confirm = item.ConfirmResource ? "1" : "0",
                               ChangePNR = generalPolicy == null ? false : generalPolicy.ChangePNR,
                               EtdzTime = getTimeRange(item.WorkStart, item.WorkEnd),
                               RefundTime = getTimeRange(item.WorkStart, item.WorkEnd),
                               ScrapTime = getTimeRange(item.RefundStart, item.RefundEnd),
                               EtdzSpeed = item.Speed == null ? 0 : item.Speed.ETDZ / 60,
                               RefundSpeed = item.Speed == null ? 0 : item.Speed.Refund / 60
                           };
                foreach (var item in list)
                {
                    if (((interfaceSetting.PolicyTypes & PolicyType.Normal) == (PolicyType)item.Type) || ((interfaceSetting.PolicyTypes & PolicyType.Bargain) == (PolicyType)item.Type) || ((interfaceSetting.PolicyTypes & PolicyType.Team) == (PolicyType)item.Type))
                    {
                        str.Append("<policy>");
                        str.AppendFormat("<id>{0}</id>", item.Id);
                        str.AppendFormat("<type>{0}</type>", item.Type);
                        str.AppendFormat("<fare>{0}</fare>", item.Fare);
                        str.AppendFormat("<rebate>{0}</rebate>", item.Rebate);
                        str.AppendFormat("<commission>{0}</commission>", item.Commission);
                        str.AppendFormat("<amount>{0}</amount>", item.Amount);
                        str.AppendFormat("<ticket>{0}</ticket>", item.Ticket);
                        str.AppendFormat("<ei>{0}</ei>", item.EI);
                        //需要授权的office号才有，否则为空字符
                        str.AppendFormat("<officeNo>{0}</officeNo>", item.OfficeNo);
                        str.AppendFormat("<condition>{0}</condition>", item.Condition);
                        str.AppendFormat("<confirm>{0}</confirm>", item.Confirm);
                        str.AppendFormat("<changePNR>{0}</changePNR>", item.ChangePNR ? 1 : 0);
                        str.AppendFormat("<etdzTime>{0}</etdzTime>", item.EtdzTime);
                        str.AppendFormat("<refundTime>{0}</refundTime>", item.RefundTime);
                        str.AppendFormat("<scrapTime>{0}</scrapTime>", item.ScrapTime);
                        str.AppendFormat("<etdzSpeed>{0}</etdzSpeed>", item.EtdzSpeed);
                        str.Append("</policy>");
                    }
                }
                if ((policyType & PolicyType.Team) != PolicyType.Team)
                {
                    var queryList = from item in matchedSpeciafPolicies
                                    where item != null && item.OriginalPolicy != null
                                    let specialPolicy = item.OriginalPolicy as SpecialPolicyInfo
                                    let specialPolicyInfo = SpecialProductService.Query(specialPolicy.Type)
                                    select new
                                    {
                                        Id = item.Id,
                                        Type = (int)item.PolicyType,
                                        Fare = item.ParValue == 100000 ? "" : FormatUtility.FormatAmount(item.ParValue),
                                        Rebate = FormatUtility.FormatAmount(item.Commission * 100),
                                        Commission = item.ParValue == 100000 ? "" : FormatUtility.FormatAmount(item.ParValue - item.SettleAmount),
                                        Amount = FormatUtility.FormatAmount(item.SettleAmount),
                                        Ticket = (int)(item.OriginalPolicy == null ? TicketType.BSP : item.OriginalPolicy.TicketType),
                                        EI = ReplaceEnter(getProvision(item.OriginalPolicy as IHasRegulation)),
                                        OfficeNo = item.OriginalPolicy == null && item.NeedAUTH ? item.OfficeNumber : (item.OriginalPolicy != null && item.OriginalPolicy.NeedAUTH ? item.OriginalPolicy.OfficeCode : ""),
                                        Condition = item.OriginalPolicy == null ? "" : item.OriginalPolicy.Condition,
                                        Confirm = item.ConfirmResource ? "1" : "0",
                                        ChangePNR = "0",
                                        EtdzTime = getTimeRange(item.WorkStart, item.WorkEnd),
                                        RefundTime = getTimeRange(item.WorkStart, item.WorkEnd),
                                        ScrapTime = getTimeRange(item.RefundStart, item.RefundEnd),
                                        EtdzSpeed = item.Speed == null ? 0 : item.Speed.ETDZ / 60,
                                        RefundSpeed = item.Speed == null ? 0 : item.Speed.Refund / 60
                                    };
                    foreach (var item in queryList)
                    {
                        if (((interfaceSetting.PolicyTypes & PolicyType.Team) == (PolicyType)item.Type))
                        {
                            str.Append("<policy>");
                            str.AppendFormat("<id>{0}</id>", item.Id);
                            str.AppendFormat("<type>{0}</type>", item.Type);
                            str.AppendFormat("<fare>{0}</fare>", item.Fare);
                            str.AppendFormat("<rebate>{0}</rebate>", item.Rebate);
                            str.AppendFormat("<commission>{0}</commission>", item.Commission);
                            str.AppendFormat("<amount>{0}</amount>", item.Amount);
                            str.AppendFormat("<ticket>{0}</ticket>", item.Ticket);
                            str.AppendFormat("<ei>{0}</ei>", item.EI);
                            //需要授权的office号才有，否则为空字符
                            str.AppendFormat("<officeNo>{0}</officeNo>", item.OfficeNo);
                            str.AppendFormat("<condition>{0}</condition>", item.Condition);
                            str.AppendFormat("<confirm>{0}</confirm>", item.Confirm);
                            str.AppendFormat("<changePNR>{0}</changePNR>", item.ChangePNR);
                            str.AppendFormat("<etdzTime>{0}</etdzTime>", item.EtdzTime);
                            str.AppendFormat("<refundTime>{0}</refundTime>", item.RefundTime);
                            str.AppendFormat("<scrapTime>{0}</scrapTime>", item.ScrapTime);
                            str.AppendFormat("<etdzSpeed>{0}</etdzSpeed>", item.EtdzSpeed);
                            str.Append("</policy>");
                        }
                    }
                    foreach (var item in matchedSpeciafPolicies)
                    {
                        if (!matchedPolicies.Contains(item))
                        {
                            matchedPolicies.Add(item);
                        }
                    }
                }
            }
            else
            {
                var queryList = from item in matchedPolicies
                                let specialPolicy = item.OriginalPolicy as SpecialPolicyInfo
                                let specialPolicyInfo = SpecialProductService.Query(specialPolicy.Type)
                                where item != null && item.OriginalPolicy != null && !specialPolicy.ConfirmResource
                                select new
                                {
                                    Id = item.Id,
                                    Type = (int)item.PolicyType,
                                    Fare = item.ParValue == 100000 ? "" : FormatUtility.FormatAmount(item.ParValue),
                                    Rebate = FormatUtility.FormatAmount(item.Commission * 100),
                                    Commission = 0,
                                    Amount = FormatUtility.FormatAmount(item.SettleAmount),
                                    Ticket = (int)(item.OriginalPolicy == null ? TicketType.BSP : item.OriginalPolicy.TicketType),
                                    EI = ReplaceEnter(getProvision(item.OriginalPolicy as IHasRegulation)),
                                    OfficeNo = item.OriginalPolicy == null && item.NeedAUTH ? item.OfficeNumber : (item.OriginalPolicy != null && item.OriginalPolicy.NeedAUTH ? item.OriginalPolicy.OfficeCode : ""),
                                    Condition = item.OriginalPolicy == null ? "" : item.OriginalPolicy.Condition,
                                    Confirm = item.ConfirmResource ? "1" : "0",
                                    ChangePNR = "0",
                                    EtdzTime = getTimeRange(item.WorkStart, item.WorkEnd),
                                    RefundTime = getTimeRange(item.WorkStart, item.WorkEnd),
                                    ScrapTime = getTimeRange(item.RefundStart, item.RefundEnd),
                                    EtdzSpeed = item.Speed == null ? 0 : item.Speed.ETDZ / 60,
                                    RefundSpeed = item.Speed == null ? 0 : item.Speed.Refund / 60,
                                    OrganizaPolicy = item.OriginalPolicy
                                };
                foreach (var item in queryList)
                {
                    if (((interfaceSetting.PolicyTypes & PolicyType.Special) == (PolicyType)item.Type))
                    {
                        str.Append("<policy>");
                        str.AppendFormat("<id>{0}</id>", item.Id);
                        str.AppendFormat("<type>{0}</type>", item.Type);
                        str.AppendFormat("<fare>{0}</fare>", item.Fare);
                        str.AppendFormat("<rebate>{0}</rebate>", item.Rebate);
                        str.AppendFormat("<commission>{0}</commission>", item.Commission);
                        str.AppendFormat("<amount>{0}</amount>", item.Amount);
                        str.AppendFormat("<ticket>{0}</ticket>", item.Ticket);
                        str.AppendFormat("<ei>{0}</ei>", item.EI);
                        //需要授权的office号才有，否则为空字符
                        str.AppendFormat("<officeNo>{0}</officeNo>", item.OfficeNo);
                        str.AppendFormat("<condition>{0}</condition>", item.Condition);
                        str.AppendFormat("<confirm>{0}</confirm>", item.Confirm);
                        str.AppendFormat("<changePNR>{0}</changePNR>", item.ChangePNR);
                        str.AppendFormat("<etdzTime>{0}</etdzTime>", item.EtdzTime);
                        str.AppendFormat("<refundTime>{0}</refundTime>", item.RefundTime);
                        str.AppendFormat("<scrapTime>{0}</scrapTime>", item.ScrapTime);
                        str.AppendFormat("<etdzSpeed>{0}</etdzSpeed>", item.EtdzSpeed);
                        str.Append("</policy>");
                    }
                }
            }
        }
        public static AllowTicketType FilterByTime(DateTime takeOffTime)
        {
            var minutesBeforeTakeOff = (takeOffTime - DateTime.Now).TotalMinutes;
            if (minutesBeforeTakeOff <= SystemParamService.FlightDisableTime) return AllowTicketType.None;
            if (minutesBeforeTakeOff < 60) return AllowTicketType.BSP;
            if (minutesBeforeTakeOff < 2 * 60) return AllowTicketType.B2BOnPolicy;
            return AllowTicketType.Both;
        }
        public static bool hasReduce(IEnumerable<VoyageFilterInfo> voyages, decimal? fare)
        {
            if (voyages.Count() == 2)
            {
                if (fare.HasValue)
                {
                    return fare < voyages.Sum(item => item.Flight.StandardPrice);
                }
            }
            return false;
        }
        public static string GetOrder(Order orderInfo)
        {
            StringBuilder str = new StringBuilder();
            str.Append("<order><title>");
            str.AppendFormat("<id>{0}</id>", orderInfo.Id);
            str.AppendFormat("<status>{0}</status>", (int)orderInfo.Status);
            str.AppendFormat("<statusDescription>{0}</statusDescription>", Service.Order.StatusService.GetOrderStatus(orderInfo.Status, OrderRole.Purchaser));
            str.AppendFormat("<product>{0}</product>", (int)orderInfo.Product.ProductType);
            str.AppendFormat("<ticket>{0}</ticket>", (int)orderInfo.Product.TicketType);
            str.AppendFormat("<associatePNR>{0}</associatePNR>", orderInfo.AssociatePNR == null ? "" : orderInfo.AssociatePNR.BPNR + "|" + orderInfo.AssociatePNR.PNR);
            str.AppendFormat("<rebate>{0}</rebate>", orderInfo.Purchaser.Rebate.TrimInvaidZero());
            str.AppendFormat("<commission>{0}</commission>", FormatUtility.FormatAmount(orderInfo.Purchaser.Commission));
            str.AppendFormat("<amount>{0}</amount>", FormatUtility.FormatAmount(orderInfo.Purchaser.Amount));
            str.AppendFormat("<producedTime>{0}</producedTime>", FormatUtility.FormatDatetime(orderInfo.Purchaser.ProducedTime));
            str.AppendFormat("<payTime>{0}</payTime>", FormatUtility.FormatDatetime(orderInfo.Bill.PayBill.Purchaser.Time));
            str.AppendFormat("<etdzTime>{0}</etdzTime>", FormatUtility.FormatDatetime(orderInfo.ETDZTime));
            str.Append("</title>");

            str.Append("<pnrs>");

            foreach (var pnr in orderInfo.PNRInfos)
            {
                str.Append("<pnr>"); str.AppendFormat("<code>{0}</code>", pnr.Code == null ? "" : pnr.Code.BPNR + "|" + pnr.Code.PNR);
                str.Append("<passengers>");
                foreach (var person in pnr.Passengers)
                {
                    str.AppendFormat("<p><name>{0}</name><type>{1}</type><credentitals>{2}</credentitals><mobile>{3}</mobile><settleCode>{4}</settleCode><tickets>{5}</tickets></p>", person.Name, (int)person.PassengerType, person.Credentials, person.Phone, person.Tickets.FirstOrDefault().SettleCode, person.Tickets.Join("|", num => num.No));
                }
                str.Append("</passengers>");
                str.Append("<flights>");
                foreach (var filght in pnr.Flights)
                {
                    str.AppendFormat("<f><departure>{0}</departure><arrival>{1}</arrival><airline>{2}</airline><flightNo>{3}</flightNo><aircraft>{11}</aircraft><takeoffTime>{4}</takeoffTime><arrivalTime>{5}</arrivalTime><bunk>{6}</bunk><fare>{7}</fare><discount>{8}</discount><airportFee>{9}</airportFee><baf>{10}</baf></f>", filght.Departure.Code + "|" + filght.Departure.City, filght.Arrival.Code + "|" + filght.Arrival.City, filght.Carrier.Code + "|" + filght.Carrier.Name, filght.FlightNo, FormatUtility.FormatFlightDatetime(filght.TakeoffTime), FormatUtility.FormatFlightDatetime(filght.LandingTime), filght.Bunk.Code, FormatUtility.FormatAmount(filght.Price.Fare), filght.Bunk.Discount.TrimInvaidZero(), FormatUtility.FormatAmount(filght.AirportFee), FormatUtility.FormatAmount(filght.BAF), filght.AirCraft);
                }
                str.Append("</flights>");
                str.Append("</pnr>");
            }

            str.Append("</pnrs>");
            str.Append("<bills>");
            if (orderInfo.Bill.PayBill != null)
            {//支付
                str.Append("<b>");
                str.AppendFormat("<type>1</type>");
                str.AppendFormat("<status>{0}</status>", orderInfo.Bill.PayBill.Purchaser.Success ? "1" : "0");
                str.AppendFormat("<amount>{0}</amount>", FormatUtility.FormatAmount(orderInfo.Bill.PayBill.Purchaser.Amount));
                str.AppendFormat("<tradeNo>{0}</tradeNo>", orderInfo.Bill.PayBill.Tradement.TradeNo);
                str.AppendFormat("<channelTradeNo>{0}</channelTradeNo>", orderInfo.Bill.PayBill.Tradement.ChannelTradeNo);
                str.AppendFormat("<time>{0}</time>", FormatUtility.FormatDatetime(orderInfo.Bill.PayBill.Purchaser.Time));
                str.Append("</b>");
            }

            if (orderInfo.Bill.NormalRefundBills != null)
            {//退款
                foreach (var item in orderInfo.Bill.NormalRefundBills)
                {
                    str.Append("<b>");
                    str.AppendFormat("<type>2</type>");
                    str.AppendFormat("<status>{0}</status>", item.Purchaser.Success ? "1" : "0");
                    str.AppendFormat("<amount>{0}</amount>", FormatUtility.FormatAmount(item.Tradement.Amount));
                    str.AppendFormat("<tradeNo>{0}</tradeNo>", item.Tradement.TradeNo);
                    str.AppendFormat("<channelTradeNo>{0}</channelTradeNo>", "");
                    str.AppendFormat("<time>{0}</time>", FormatUtility.FormatDatetime(item.Purchaser.Time));
                    str.Append("</b>");
                }
            }
            if (orderInfo.Bill.PostponePayBills != null)
            {//支付
                foreach (var item in orderInfo.Bill.PostponePayBills)
                {
                    str.Append("<b>");
                    str.AppendFormat("<type>1</type>");
                    str.AppendFormat("<status>{0}</status>", item.Applier.Success ? "1" : "0");
                    str.AppendFormat("<amount>{0}</amount>", FormatUtility.FormatAmount(item.Applier.Amount));
                    str.AppendFormat("<tradeNo>{0}</tradeNo>", item.Tradement.TradeNo);
                    str.AppendFormat("<channelTradeNo>{0}</channelTradeNo>", item.Tradement.ChannelTradeNo);
                    str.AppendFormat("<time>{0}</time>", FormatUtility.FormatDatetime(item.Applier.Time));
                    str.Append("</b>");
                }
            }

            if (orderInfo.Bill.PostponeRefundBills != null)
            {//退款
                foreach (var item in orderInfo.Bill.PostponeRefundBills)
                {
                    str.Append("<b>");
                    str.AppendFormat("<type>2</type>");
                    str.AppendFormat("<status>{0}</status>", item.Applier.Success ? "1" : "0");
                    str.AppendFormat("<amount>{0}</amount>", FormatUtility.FormatAmount(item.Applier.Amount));
                    str.AppendFormat("<tradeNo>{0}</tradeNo>", item.Tradement.TradeNo);
                    str.AppendFormat("<channelTradeNo>{0}</channelTradeNo>", "");
                    str.AppendFormat("<time>{0}</time>", FormatUtility.FormatDatetime(item.Applier.Time));
                    str.Append("</b>");
                }
            }

            str.Append("</bills></order>");
            return str.ToString();
        }

        public static PolicyType QueryPolicyType(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights, ReservedPnr _pnr)
        {
            PolicyType policyType = PolicyType.Bargain;
            if (_pnr.IsTeam)
            {
                policyType = PolicyType.Team;
            }
            else
            {
                // 根据舱位的类型来决定政策类型
                if (flights.Any(f => f.BunkType == BunkType.Promotion || f.BunkType == BunkType.Production || f.BunkType == BunkType.Transfer))
                {
                    policyType = PolicyType.Bargain;
                }
                else
                {
                    switch (flights.First().BunkType)
                    {
                        case BunkType.Economic:
                        case BunkType.FirstOrBusiness:
                            policyType = PolicyType.Normal | PolicyType.Bargain;
                            break;
                        case BunkType.Promotion:
                        case BunkType.Production:
                        case BunkType.Transfer:
                            policyType = PolicyType.Bargain;
                            break;
                        default:
                            policyType = PolicyType.Special;
                            break;
                    }
                }
            }
            return policyType;
        }
        private static string getTimeRange(Izual.Time start, Izual.Time end)
        {
            return start.ToString("HH:mm") + "-" + end.ToString("HH:mm");
        }
        private static string ReplaceEnter(string input)
        {
            string result = input.Replace("\n", "<br />").Replace("\"", string.Empty).Replace("'", string.Empty);
            return result.Replace("\r", "");
        }
        private static object getProvisionList(IHasRegulation regulation)
        {
            if (regulation == null) return new[] { new { key = string.Empty, value = string.Empty } };
            return new[]
            {
                new {key="更改规定" ,value= regulation.ChangeRegulation},
                new {key="作废规定",value= regulation.InvalidRegulation},
                new {key="退票规定" ,value= regulation.RefundRegulation},
                new {key="签转规定" ,value= regulation.EndorseRegulation}
            };
        }
        private static string getEI(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights)
        {
            Dictionary<string, string> RenderedEI = new Dictionary<string, string>();
            var htmlReg = new Regex("<.+?>");
            var voyages = flights;
            var flightView = voyages.ElementAt(0);
            string ei = "(" + flightView.BunkCode + ")" + htmlReg.Replace(flightView.EI, string.Empty);
            RenderedEI.Add(flightView.BunkCode, flightView.EI);
            foreach (var item in voyages)
            {
                if (!RenderedEI.Any(b => b.Key == item.BunkCode && b.Value == item.EI))
                {
                    ei += "|" + "(" + item.BunkCode + ")" + htmlReg.Replace(flightView.EI, string.Empty);
                    RenderedEI.Add(item.BunkCode, item.EI);
                }
            }
            return ei;
        }
        private static string getProvision(IHasRegulation regulation)
        {
            if (regulation == null) return string.Empty;
            return "更改规定：" + regulation.ChangeRegulation + "|作废规定：" + regulation.InvalidRegulation +
                   "|退票规定：" + regulation.RefundRegulation + "|签转规定：" + regulation.EndorseRegulation;
        }

    }
}