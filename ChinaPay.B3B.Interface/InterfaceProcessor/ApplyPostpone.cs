using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Interface.PublicClass;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Interface.InterfaceProcessor
{
    /// <summary>
    /// 申请改期
    /// </summary>
    class ApplyPostpone : BaseProcessor
    {
        public string _orderId { get; set; }
        public string _passengers { get; set; }
        public string _voyages { get; set; }
        public string _reason { get; set; }
        public ApplyPostpone(string orderId, string passengers, string voyages, string reason, string userName, string sign)
            : base(userName, sign)
        {
            _orderId = orderId;
            _passengers = passengers;
            _voyages = voyages;
            _reason = reason;
        }
        protected override System.Collections.Specialized.NameValueCollection GetBusinessParameterCollection()
        {
            var collection = new System.Collections.Specialized.NameValueCollection();
            collection.Add("orderId", _orderId);
            collection.Add("passengers", _passengers);
            collection.Add("voyages", _voyages);
            collection.Add("reason", _reason);
            return collection;
        }

        protected override void ValidateBusinessParameters()
        {
            if (string.IsNullOrEmpty(_orderId)) throw new InterfaceInvokeException("1", "订单号");
            if (string.IsNullOrEmpty(_passengers)) throw new InterfaceInvokeException("1", "乘机人");
            if (string.IsNullOrEmpty(_voyages)) throw new InterfaceInvokeException("1", "航段信息");
            if (string.IsNullOrEmpty(_reason)) throw new InterfaceInvokeException("1", "申请原因");
        }

        protected override string ExecuteCore()
        {
            decimal id;
            if (!decimal.TryParse(_orderId, out id)) throw new InterfaceInvokeException("1", "订单号");
            var order = Service.OrderQueryService.QueryOrder(id);
            if (order == null) throw new InterfaceInvokeException("9", "暂无此订单");
            if (order.Purchaser.CompanyId != Company.CompanyId) throw new InterfaceInvokeException("9", "暂无此订单");
            id = ApplyPostponeOrder(order);
            var obj = Service.ApplyformQueryService.QueryApplyform(id);
            //StringBuilder str = new StringBuilder();
            //str.Append("<applyform><title>");
            ////改期申请
            //if (obj is PostponeApplyform)
            //{
            //    var applyform = obj as PostponeApplyform;
            //    str.AppendFormat("<id>{0}</id>", applyform.Id);
            //    str.AppendFormat("<applyType>{0}</applyType>", "4");
            //    str.AppendFormat("<status>{0}</status>", (int)applyform.Status);
            //    str.AppendFormat("<statusDescription>{0}</statusDescription>", applyform.Status.GetDescription());
            //    str.AppendFormat("<refundType>{0}</refundType>", "");
            //    str.AppendFormat("<originalPNR>{0}</originalPNR>", applyform.OriginalPNR == null ? "" : applyform.OriginalPNR.BPNR + "|" + applyform.OriginalPNR.PNR);
            //    str.AppendFormat("<newPNR>{0}</newPNR>", applyform.NewPNR == null ? "" : applyform.NewPNR.BPNR + "|" + applyform.NewPNR.PNR);
            //    str.AppendFormat("<amount>{0}</amount>", "");//
            //    str.AppendFormat("<applyTime>{0}</applyTime>", applyform.AppliedTime);
            //    str.AppendFormat("<payTime>{0}</payTime>", applyform.Status == PostponeApplyformStatus.Applied ? "" : (applyform.PayBill.Accepter.Time.HasValue ? applyform.PayBill.Accepter.Time.Value.ToString() : ""));
            //    str.AppendFormat("<processedTime>{0}</processedTime>", applyform.ProcessedTime);
            //    str.Append("</title><passengers>");
            //    foreach (var item in applyform.Passengers)
            //    {
            //        str.Append("<p>");
            //        str.AppendFormat("<name>{0}</name>", item.Name);
            //        str.AppendFormat("<type>{0}</type>", (int)item.PassengerType);
            //        str.AppendFormat("<credentitals>{0}</credentitals>", item.Credentials);
            //        str.AppendFormat("<mobile>{0}</mobile>", item.Phone);
            //        str.AppendFormat("<settleCode>{0}</settleCode>", item.Tickets.FirstOrDefault().SettleCode);
            //        str.AppendFormat("<tickets>{0}</tickets>", item.Tickets.Join("|", num => num.No));
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
            //        str.AppendFormat("<refundRate>{0}</refundRate>", "");
            //        str.AppendFormat("<refundFee>{0}</refundFee>", "");
            //        str.AppendFormat("<refundServiceCharge>{0}</refundServiceCharge>", "");
            //        str.AppendFormat("<newFlightNo>{0}</newFlightNo>", item.NewFlight.FlightNo);
            //        str.AppendFormat("<newAircraft>{0}</newAircraft>", item.NewFlight.AirCraft);
            //        str.AppendFormat("<newTakeoffTime>{0}</newTakeoffTime>", item.NewFlight.TakeoffTime);
            //        str.AppendFormat("<newArrivalTime>{0}</newArrivalTime>", item.NewFlight.LandingTime);
            //        str.AppendFormat("<postponeFee>{0}</postponeFee>", item.PostponeFee);
            //        str.Append("</f>");
            //    }
            //    str.Append("</flights><bills>");
            //    str.AppendFormat("<b><type>{0}</type>", "1");
            //    str.AppendFormat("<amount>{0}</amount>", applyform.Status == PostponeApplyformStatus.Applied ? "" : applyform.PayBill.Tradement.Amount.ToString());
            //    str.AppendFormat("<tradeNo>{0}</tradeNo>", applyform.Status == PostponeApplyformStatus.Applied ? "" : applyform.PayBill.Tradement.TradeNo.ToString());
            //    str.AppendFormat("<time>{0}</time>", applyform.Status == PostponeApplyformStatus.Applied ? "" : applyform.PayBill.Applier.Time.HasValue ? applyform.PayBill.Applier.Time.ToString() : "");
            //    str.Append("</b></bills>");
            //}
            //str.Append("</applyform>");
            return QueryApplyform.GetApplyform(obj);
        }

        private decimal ApplyPostponeOrder(Order order)
        {
            string pid = "";
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
                        break;
                    }
                }
            }
            if (pid == "") throw new InterfaceInvokeException("9", "当前订单中没有找到对应的乘机人信息");
            if (!_pnrCode.Any()) throw new InterfaceInvokeException("9", "当前订单中没有找到对应的编码信息");
            var ids = ApplyOrder.getPassengers(pid);
            var applyformView = new PostponeApplyformView()
            {
                PNR = _pnrCode.FirstOrDefault(),
                Passengers = ids,
                Reason = _reason
            };
            var flightChangeLimit = SystemDictionaryService.Query(SystemDictionaryType.FlightChangeLimit);
            var limit = LimitItem.Parse(flightChangeLimit);



            foreach (var item in getPostponeVoyages(_voyages, order))
            {
                bool isTodayFlight = DateTime.Today == item.NewFlightDate.Date;
                foreach (LimitItem l in limit)
                {
                    if (order.PNRInfos.First().Flights.First().Carrier.Code != l.Carrair.ToUpper()) continue;
                    if (item.NewFlightDate >= l.LimitFrom && item.NewFlightDate <= l.LimitTo && (!isTodayFlight || !l.ToTodayEnable))
                    {
                        var aline = FoundationService.Airlines.FirstOrDefault(p => p.Code.Value == l.Carrair);
                        throw new InterfaceInvokeException("9", "由于[" + aline.ShortName + "]原因，该客票已被航空公司限制改期，无法改期，请让乘机人自行致电航空公司或到航空公司直营柜台办理改期");
                    }
                }
                applyformView.AddItem(item);

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
        private List<PostponeApplyformView.Item> getPostponeVoyages(string voyages, Order order)
        {
            var result = new List<PostponeApplyformView.Item>();
            foreach (var item in _voyages.Split('^'))
            {
                foreach (var o in order.PNRInfos)
                {
                    var s = item.Split('|');
                    if (s.Count() < 5)
                    {
                        throw new InterfaceInvokeException("1", "航班信息");
                    }
                    if (s[0].Length != 6)
                    {
                        throw new InterfaceInvokeException("1", "城市对");
                    }
                    var departure = s[0].Substring(0, 3).ToUpper();
                    var arrival = s[0].Substring(3, 3).ToUpper();
                    var flightNo = s[1];
                    var flightTime = s[2];
                    var j = o.Flights.FirstOrDefault(i => i.Departure.Code == departure && i.Arrival.Code == arrival && i.FlightNo == flightNo && i.TakeoffTime.Date == DateTime.Parse(flightTime));
                    if (j != null)
                    {
                        if (result.FirstOrDefault(f => f.Voyage == j.Id) != null)
                        {
                            throw new InterfaceInvokeException("9", "当前申请的改期存在两个以上的相同航班信息，请核对航班信息");
                        }
                        result.Add(new PostponeApplyformView.Item()
                        {
                            Voyage = j.Id,
                            NewFlightNo = s[3],
                            NewFlightDate = DateTime.Parse(s[4])
                        });
                        break;
                    }
                    else
                    {
                        throw new InterfaceInvokeException("9", "当前订单中不存在[" + item + "]的航班信息，请核对航班信息是否有误");
                    }
                }
            }
            return result;
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