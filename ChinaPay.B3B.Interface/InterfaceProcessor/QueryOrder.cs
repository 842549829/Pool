using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using ChinaPay.B3B.Service;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Common;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Interface.PublicClass;
using ChinaPay.B3B.DataTransferObject.Order;


namespace ChinaPay.B3B.Interface.InterfaceProcessor
{
    internal class QueryOrder : BaseProcessor
    {
        private string _id;

        public QueryOrder(string id, string userName, string sign)
            : base(userName, sign)
        {
            _id = id;
        }

        protected override void ValidateBusinessParameters()
        {
            if (string.IsNullOrWhiteSpace(_id)) throw new InterfaceInvokeException("1", "订单号");
        }

        protected override string ExecuteCore()
        {//待确定
            decimal id = 0M;
            if (decimal.TryParse(_id, out id))
            {
                var orderInfo = OrderQueryService.QueryOrder(id);
                if (orderInfo == null)
                    throw new InterfaceInvokeException("9", "暂无此订单");
                if (orderInfo.Bill == null)
                    throw new InterfaceInvokeException("9", "暂无账单信息");
                if (orderInfo.Purchaser.CompanyId != Company.CompanyId)
                    throw new InterfaceInvokeException("9", "暂无此订单");

                return GetOrder(orderInfo);
            }
            else
            {
                throw new InterfaceInvokeException("1", "订单号");
            }
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

        protected override System.Collections.Specialized.NameValueCollection GetBusinessParameterCollection()
        {
            var collection = new System.Collections.Specialized.NameValueCollection();
            collection.Add("id", _id);
            return collection;
        }

    }
}
