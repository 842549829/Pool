using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Interface.PublicClass;

namespace ChinaPay.B3B.Interface.InterfaceProcessor
{
    /// <summary>
    /// 查询申请单
    /// </summary>
    class QueryApplyform : BaseProcessor
    {
        private string _applyformId { get; set; }
        public QueryApplyform(string applyformId, string userName, string sign)
            : base(userName, sign)
        {
            _applyformId = applyformId;
        }
        protected override System.Collections.Specialized.NameValueCollection GetBusinessParameterCollection()
        {
            var collection = new System.Collections.Specialized.NameValueCollection();
            collection.Add("applyformId", _applyformId);
            return collection;
        }

        protected override void ValidateBusinessParameters()
        {
            if (string.IsNullOrEmpty(_applyformId)) throw new InterfaceInvokeException("1", "申请单号");
        }

        protected override string ExecuteCore()
        {
            decimal id;
            if (!decimal.TryParse(_applyformId, out id)) throw new InterfaceInvokeException("1", "申请单号");
            var obj = Service.ApplyformQueryService.QueryApplyform(id);
            if (obj == null) throw new InterfaceInvokeException("9", "暂无此申请单");
            //if (applyform.Passengers == null) throw new InterfaceInvokeException("9", "乘机人信息为空");
            //if (applyform.OriginalFlights == null) throw new InterfaceInvokeException("9", "航段信息为空");
            //if (applyform.PayBill == null) throw new InterfaceInvokeException("9", "账单信息为空"); 
            return GetApplyform(obj);
        }

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
                    str.AppendFormat("<channelTradeNo>{0}</channelTradeNo>", "");
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
    }
}