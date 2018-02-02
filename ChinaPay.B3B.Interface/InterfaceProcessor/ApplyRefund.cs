using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Interface.PublicClass;

namespace ChinaPay.B3B.Interface.InterfaceProcessor
{
    /// <summary>
    /// 申请退废票
    /// </summary>
    class ApplyRefund : BaseProcessor
    {
        public string _orderId { get; set; }
        public string _passengers { get; set; }
        public string _voyages { get; set; }
        public string _refundType { get; set; }
        public string _reason { get; set; }
        public string _userName { get; set; }
        public ApplyRefund(string orderId, string passengers, string voyages, string refundType, string reason, string userName, string sign)
            : base(userName, sign)
        {
            _orderId = orderId;
            _passengers = passengers;
            _voyages = voyages;
            _refundType = refundType;
            _reason = reason;
            //_pnrCode = pnrCode;
            _userName = userName;
        }
        protected override System.Collections.Specialized.NameValueCollection GetBusinessParameterCollection()
        {
            var collection = new System.Collections.Specialized.NameValueCollection();
            collection.Add("orderId", _orderId);
            collection.Add("passengers", _passengers);
            //collection.Add("pnrCode", _pnrCode);
            collection.Add("voyages", _voyages);
            collection.Add("refundType", _refundType);
            collection.Add("reason", _reason);
            return collection;
        }

        protected override void ValidateBusinessParameters()
        {
            if (string.IsNullOrEmpty(_orderId)) throw new InterfaceInvokeException("1", "订单号");
            if (string.IsNullOrEmpty(_passengers)) throw new InterfaceInvokeException("1", "乘机人");
            //if (string.IsNullOrEmpty(_pnrCode)) throw new InterfaceInvokeException("1", "编码信息");
            if (string.IsNullOrEmpty(_voyages)) throw new InterfaceInvokeException("1", "航段信息");
            if (string.IsNullOrEmpty(_refundType)) throw new InterfaceInvokeException("1", "退票类型");
            if (string.IsNullOrEmpty(_reason)) throw new InterfaceInvokeException("1", "退票原因");
        }

        protected override string ExecuteCore()
        {
            decimal id;
            if (!decimal.TryParse(_orderId, out id)) throw new InterfaceInvokeException("1", "订单号");
            var order = Service.OrderQueryService.QueryOrder(id);
            if (order == null) throw new InterfaceInvokeException("9", "暂无此订单");
            if (order.Purchaser.CompanyId != Company.CompanyId) throw new InterfaceInvokeException("9", "暂无此订单");
            id = ApplyRefundObj(order);

            //StringBuilder str = new StringBuilder();
            var obj = Service.ApplyformQueryService.QueryApplyform(id);
            //str.Append("<applyform><title>");
            ////退票申请
            //if (obj is RefundApplyform)
            //{
            //    var applyform = obj as RefundApplyform;
            //    str.AppendFormat("<id>{0}</id>", applyform.Id);
            //    str.AppendFormat("<applyType>{0}</applyType>", "1");
            //    str.AppendFormat("<status>{0}</status>", (int)applyform.Status);
            //    str.AppendFormat("<statusDescription>{0}</statusDescription>", applyform.Status.GetDescription());
            //    str.AppendFormat("<refundType>{0}</refundType>", (int)applyform.RefundType);
            //    str.AppendFormat("<originalPNR>{0}</originalPNR>", applyform.OriginalPNR == null ? "" : applyform.OriginalPNR.BPNR + "|" + applyform.OriginalPNR.PNR);
            //    str.AppendFormat("<newPNR>{0}</newPNR>", applyform.NewPNR == null ? "" : applyform.NewPNR.BPNR + "|" + applyform.NewPNR.PNR);
            //    str.AppendFormat("<amount>{0}</amount>", applyform.Status == RefundApplyformStatus.Refunded ? applyform.RefundBill.Purchaser.Amount.ToString() : "");//
            //    str.AppendFormat("<applyTime>{0}</applyTime>", applyform.AppliedTime);
            //    str.AppendFormat("<payTime>{0}</payTime>", "");//退票没有支付时间
            //    str.AppendFormat("<processedTime>{0}</processedTime>", applyform.ProcessedTime);
            //    str.Append("</title><passengers>");
            //    foreach (var item in applyform.Passengers)
            //    {
            //        str.Append("<p>");
            //        str.AppendFormat("<name>{0}</name>", item.Name);
            //        str.AppendFormat("<type>{0}</type>", (int)item.PassengerType);
            //        str.AppendFormat("<credentitals>{0}</credentitals>", item.Credentials);
            //        str.AppendFormat("<mobile>{0}</mobile>", item.Phone);
            //        str.AppendFormat("<settleCode>{0}</settleCode>", item.Tickets.Count() != 0 ? item.Tickets.FirstOrDefault().SettleCode : "");
            //        str.AppendFormat("<tickets>{0}</tickets>", item.Tickets.Count() != 0 ? item.Tickets.Join("|", num => num.No) : "");
            //        str.Append("</p>");
            //    }
            //    str.Append("</passengers><flights>");
            //    foreach (var item in applyform.Flights)
            //    {
            //        str.Append("<f>");
            //        str.AppendFormat("<departure>{0}</departure>", item.OriginalFlight.Departure.Code);
            //        str.AppendFormat("<arrival>{0}</arrival>", item.OriginalFlight.Arrival.Code);
            //        str.AppendFormat("<flightNo>{0}</flightNo>", item.OriginalFlight.FlightNo);
            //        str.AppendFormat("<aircraft>{0}</aircraft>", item.OriginalFlight.AirCraft);
            //        str.AppendFormat("<takeoffTime>{0}</takeoffTime>", item.OriginalFlight.TakeoffTime);
            //        str.AppendFormat("<arrivalTime>{0}</arrivalTime>", item.OriginalFlight.LandingTime);
            //        str.AppendFormat("<bunk>{0}</bunk>", item.OriginalFlight.Bunk.Code);
            //        str.AppendFormat("<fare>{0}</fare>", item.OriginalFlight.Price.Fare);
            //        str.AppendFormat("<discount>{0}</discount>", item.OriginalFlight.Bunk.Discount);
            //        str.AppendFormat("<airportFee>{0}</airportFee>", item.OriginalFlight.AirportFee);
            //        str.AppendFormat("<baf>{0}</baf>", item.OriginalFlight.Price.BAF);
            //        str.AppendFormat("<refundRate>{0}</refundRate>", applyform.Status == RefundApplyformStatus.Refunded ? item.RefundRate.ToString() : "");
            //        str.AppendFormat("<refundFee>{0}</refundFee>", applyform.Status == RefundApplyformStatus.Refunded ? applyform.RefundBill.Purchaser.Source.Details.First(o => o.Flight.Id == item.OriginalFlight.Id).RefundFee.ToString() : "");
            //        str.AppendFormat("<refundServiceCharge>{0}</refundServiceCharge>", item.RefundServiceCharge);
            //        str.AppendFormat("<newFlightNo>{0}</newFlightNo>", "");
            //        str.AppendFormat("<newAircraft>{0}</newAircraft>", "");
            //        str.AppendFormat("<newTakeoffTime>{0}</newTakeoffTime>", "");
            //        str.AppendFormat("<newArrivalTime>{0}</newArrivalTime>", "");
            //        str.AppendFormat("<postponeFee>{0}</postponeFee>", "");
            //        str.Append("</f>");
            //    }
            //    str.Append("</flights><bills>");
            //    str.AppendFormat("<b><type>{0}</type>", "1");
            //    str.AppendFormat("<amount>{0}</amount>", applyform.Status == RefundApplyformStatus.Refunded ? applyform.RefundBill.Tradement.Amount.ToString() : "");
            //    str.AppendFormat("<tradeNo>{0}</tradeNo>", applyform.Status == RefundApplyformStatus.Refunded ? applyform.RefundBill.Tradement.TradeNo.ToString() : "");
            //    str.AppendFormat("<time>{0}</time>", applyform.Status == RefundApplyformStatus.Refunded ? applyform.RefundBill.Purchaser.Time.ToString() : "");
            //    str.Append("</b></bills>");
            //}
            ////废票申请
            //if (obj is ScrapApplyform)
            //{
            //    var applyform = obj as ScrapApplyform;
            //    str.AppendFormat("<id>{0}</id>", applyform.Id);
            //    str.AppendFormat("<applyType>{0}</applyType>", "2");
            //    str.AppendFormat("<status>{0}</status>", (int)applyform.Status);
            //    str.AppendFormat("<statusDescription>{0}</statusDescription>", applyform.Status.GetDescription());
            //    str.AppendFormat("<refundType>{0}</refundType>", "");
            //    str.AppendFormat("<originalPNR>{0}</originalPNR>", applyform.OriginalPNR == null ? "" : applyform.OriginalPNR.BPNR + "|" + applyform.OriginalPNR.PNR);
            //    str.AppendFormat("<newPNR>{0}</newPNR>", applyform.NewPNR == null ? "" : applyform.NewPNR.BPNR + "|" + applyform.NewPNR.PNR);
            //    str.AppendFormat("<amount>{0}</amount>", applyform.Status == RefundApplyformStatus.Refunded ? applyform.RefundBill.Purchaser.Amount.ToString() : "");//
            //    str.AppendFormat("<applyTime>{0}</applyTime>", applyform.AppliedTime);
            //    str.AppendFormat("<payTime>{0}</payTime>", "");//退票没有支付时间
            //    str.AppendFormat("<processedTime>{0}</processedTime>", applyform.ProcessedTime);
            //    str.Append("</title><passengers>");
            //    foreach (var item in applyform.Passengers)
            //    {
            //        str.Append("<p>");
            //        str.AppendFormat("<name>{0}</name>", item.Name);
            //        str.AppendFormat("<type>{0}</type>", (int)item.PassengerType);
            //        str.AppendFormat("<credentitals>{0}</credentitals>", item.Credentials);
            //        str.AppendFormat("<mobile>{0}</mobile>", item.Phone);
            //        str.AppendFormat("<settleCode>{0}</settleCode>", item.Tickets.Count() != 0 ? item.Tickets.FirstOrDefault().SettleCode : "");
            //        str.AppendFormat("<tickets>{0}</tickets>", item.Tickets.Count() != 0 ? item.Tickets.Join("|", num => num.No) : "");
            //        str.Append("</p>");
            //    }
            //    str.Append("</passengers><flights>");
            //    foreach (var item in applyform.Flights)
            //    {
            //        str.Append("<f>");
            //        str.AppendFormat("<departure>{0}</departure>", item.OriginalFlight.Departure.Code);
            //        str.AppendFormat("<arrival>{0}</arrival>", item.OriginalFlight.Arrival.Code);
            //        str.AppendFormat("<flightNo>{0}</flightNo>", item.OriginalFlight.FlightNo);
            //        str.AppendFormat("<aircraft>{0}</aircraft>", item.OriginalFlight.AirCraft);
            //        str.AppendFormat("<takeoffTime>{0}</takeoffTime>", item.OriginalFlight.TakeoffTime);
            //        str.AppendFormat("<arrivalTime>{0}</arrivalTime>", item.OriginalFlight.LandingTime);
            //        str.AppendFormat("<bunk>{0}</bunk>", item.OriginalFlight.Bunk.Code);
            //        str.AppendFormat("<fare>{0}</fare>", item.OriginalFlight.Price.Fare);
            //        str.AppendFormat("<discount>{0}</discount>", item.OriginalFlight.Bunk.Discount);
            //        str.AppendFormat("<airportFee>{0}</airportFee>", item.OriginalFlight.AirportFee);
            //        str.AppendFormat("<baf>{0}</baf>", item.OriginalFlight.Price.BAF);
            //        str.AppendFormat("<refundRate>{0}</refundRate>", applyform.Status == RefundApplyformStatus.Refunded ? item.RefundRate.ToString() : "");
            //        str.AppendFormat("<refundFee>{0}</refundFee>", applyform.Status == RefundApplyformStatus.Refunded ? applyform.RefundBill.Purchaser.Source.Details.First(o => o.Flight.Id == item.OriginalFlight.Id).RefundFee.ToString() : "");
            //        str.AppendFormat("<refundServiceCharge>{0}</refundServiceCharge>", item.RefundServiceCharge);
            //        str.AppendFormat("<newFlightNo>{0}</newFlightNo>", "");
            //        str.AppendFormat("<newAircraft>{0}</newAircraft>", "");
            //        str.AppendFormat("<newTakeoffTime>{0}</newTakeoffTime>", "");
            //        str.AppendFormat("<newArrivalTime>{0}</newArrivalTime>", "");
            //        str.AppendFormat("<postponeFee>{0}</postponeFee>", "");
            //        str.Append("</f>");
            //    }
            //    str.Append("</flights><bills>");
            //    str.AppendFormat("<b><type>{0}</type>", "1");
            //    str.AppendFormat("<amount>{0}</amount>", applyform.Status == RefundApplyformStatus.Refunded ? applyform.RefundBill.Tradement.Amount.ToString() : "");
            //    str.AppendFormat("<tradeNo>{0}</tradeNo>", applyform.Status == RefundApplyformStatus.Refunded ? applyform.RefundBill.Tradement.TradeNo.ToString() : "");
            //    str.AppendFormat("<time>{0}</time>", applyform.Status == RefundApplyformStatus.Refunded ? applyform.RefundBill.Purchaser.Time.ToString() : "");
            //    str.Append("</b></bills>");
            //}
            //str.Append("</applyform>");
            return QueryApplyform.GetApplyform(obj);
        }

        private decimal ApplyRefundObj(Order order)
        {

            RefundOrScrapApplyformView applyformView = null;
            if (_refundType == "-1")
            {
                applyformView = new ScrapApplyformView();
            }
            else
            {
                applyformView = new RefundApplyformView()
                {
                    RefundType = (RefundType)int.Parse(_refundType)
                };
            }
            var app = Service.ApplyformQueryService.QueryApplyforms(order.Id);
            string pid = "";
            string vid = "";
            List<PNRPair> _pnrCode = new List<PNRPair>();
            foreach (var item in _passengers.Split('^'))
            {
                foreach (var o in order.PNRInfos)
                {
                    var j = o.Passengers.FirstOrDefault(i => i.Name == item);
                    if (j != null)
                    {
                        if (pid != "")
                        {
                            pid += "," + j.Id.ToString();
                        }
                        else
                        {
                            pid += j.Id.ToString();
                        }
                        _pnrCode.Add(o.Code);
                        if (!_pnrCode.Contains(o.Code)) throw new InterfaceInvokeException("9", "当前订单中存在不同的编码");
                        foreach (var f in _voyages.Split('^'))
                        {
                            if (f.Split('|').Count() < 3)
                            {
                                throw new InterfaceInvokeException("1", "航班信息");
                            }
                            if (f.Split('|')[0].Length!=6)
                            {
                                throw new InterfaceInvokeException("1", "城市对");
                            }
                            var departure = f.Split('|')[0].Substring(0, 3).ToUpper();
                            var arrival = f.Split('|')[0].Substring(3, 3).ToUpper();
                            var flightNo = f.Split('|')[1];
                            var flightTime = f.Split('|')[2];
                            var ff = o.Flights.FirstOrDefault(i => i.Departure.Code == departure && i.Arrival.Code == arrival && i.FlightNo == flightNo && i.TakeoffTime.Date == DateTime.Parse(flightTime));
                            if (ff != null)
                            {
                                if (!vid.Split(',').Contains(ff.Id.ToString()))
                                {
                                    if (vid != "")
                                    {
                                        vid += "," + ff.Id.ToString();
                                    }
                                    else
                                    {
                                        vid += ff.Id.ToString();
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
            }
            if (pid == "") throw new InterfaceInvokeException("9", "当前订单中没有找到对应的乘机人信息");
            if (vid == "") throw new InterfaceInvokeException("9", "当前订单中没有找到对应的航班信息");
            if (!_pnrCode.Any()) throw new InterfaceInvokeException("9", "当前订单中没有找到对应的编码信息");

            applyformView.PNR = _pnrCode.FirstOrDefault();
            applyformView.Passengers = ApplyOrder.getPassengers(pid);
            applyformView.Reason = _reason;
            foreach (var item in ApplyOrder.getRefundVoyages(vid))
            {
                applyformView.AddVoyage(item);
            }
            string lockErrorMsg = "";
            Lock(order.Id, Service.Locker.LockRole.Purchaser, "申请退改签", out lockErrorMsg);
            if (!string.IsNullOrEmpty(lockErrorMsg)) throw new InterfaceInvokeException("9", lockErrorMsg);
            var apply = Service.OrderProcessService.Apply(order.Id, applyformView, Employee,Guid.Empty);
            releaseLock(order.Id);
            return apply.Id;
        }
        private void releaseLock(decimal orderId)
        {
            Service.LockService.UnLock(orderId.ToString(), Employee.UserName);
        }

        private bool Lock(decimal key, Service.Locker.LockRole lockRole, string remark, out string errorMsg)
        {
            var lockInfo = new Service.Locker.LockInfo(key.ToString())
            {
                LockRole = lockRole,
                Company = Company.CompanyId,
                CompanyName = Company.AbbreviateName,
                Account = Employee.UserName,
                Name = Employee.Name,
                Remark = remark
            };
            return Service.LockService.Lock(lockInfo, out errorMsg);
        }
    }
}