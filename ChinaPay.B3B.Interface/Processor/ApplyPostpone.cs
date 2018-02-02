using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Interface.PublicClass;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service;

namespace ChinaPay.B3B.Interface.Processor
{
    class ApplyPostpone : RequestProcessor
    {
        
        protected override string ExecuteCore()
        {
            var orderId = Context.GetParameterValue("orderId");
            var passengers = Context.GetParameterValue("passengers");
            var voyages = Context.GetParameterValue("voyages");
            var reason = Context.GetParameterValue("reason");
            if (string.IsNullOrWhiteSpace(orderId)) InterfaceInvokeException.ThrowParameterMissException("orderId");
            if (string.IsNullOrWhiteSpace(passengers)) InterfaceInvokeException.ThrowParameterMissException("passengers");
            if (string.IsNullOrWhiteSpace(voyages)) InterfaceInvokeException.ThrowParameterMissException("voyages");
            if (string.IsNullOrWhiteSpace(reason)) InterfaceInvokeException.ThrowParameterMissException("reason");

            decimal id;
            if (!decimal.TryParse(orderId, out id)) InterfaceInvokeException.ThrowParameterErrorException("订单号");
            var order = Service.OrderQueryService.QueryOrder(id);
            if (order == null) InterfaceInvokeException.ThrowCustomMsgException("暂无此订单");
            if (order.Purchaser.CompanyId != Company.CompanyId) InterfaceInvokeException.ThrowCustomMsgException("暂无此订单");
            id = ApplyPostponeOrder(order, passengers, reason, voyages);
            var obj = Service.ApplyformQueryService.QueryApplyform(id);
            return ReturnStringUtility.GetApplyform(obj);
        }
        private decimal ApplyPostponeOrder(ChinaPay.B3B.Service.Order.Domain.Order order, string passengers, string reason, string voyages)
        {
            string pid = "";
            List<PNRPair> _pnrCode = new List<PNRPair>();
            foreach (var item in passengers.Split('^'))
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
                        if (!_pnrCode.Contains(o.Code)) InterfaceInvokeException.ThrowCustomMsgException("当前订单中存在不同的编码");
                        break;
                    }
                }
            }
            if (pid == "") InterfaceInvokeException.ThrowCustomMsgException("当前订单中没有找到对应的乘机人信息");
            if (!_pnrCode.Any()) InterfaceInvokeException.ThrowCustomMsgException("当前订单中没有找到对应的编码信息");
            var ids = ApplyOrder.getPassengers(pid);
            var applyformView = new PostponeApplyformView()
            {
                PNR = _pnrCode.FirstOrDefault(),
                Passengers = ids,
                Reason = reason
            };
            var flightChangeLimit = SystemDictionaryService.Query(SystemDictionaryType.FlightChangeLimit);
            var limit = LimitItem.Parse(flightChangeLimit);



            foreach (var item in getPostponeVoyages(voyages, order))
            {
                bool isTodayFlight = DateTime.Today == item.NewFlightDate.Date;
                foreach (LimitItem l in limit)
                {
                    if (order.PNRInfos.First().Flights.First().Carrier.Code != l.Carrair.ToUpper()) continue;
                    if (item.NewFlightDate >= l.LimitFrom && item.NewFlightDate <= l.LimitTo && (!isTodayFlight || !l.ToTodayEnable))
                    {
                        var aline = FoundationService.Airlines.FirstOrDefault(p => p.Code.Value == l.Carrair);
                        InterfaceInvokeException.ThrowCustomMsgException("由于[" + aline.ShortName + "]原因，该客票已被航空公司限制改期，无法改期，请让乘机人自行致电航空公司或到航空公司直营柜台办理改期");
                    }
                }
                applyformView.AddItem(item);

            }
            string lockErrorMsg = "";
            Lock(order.Id, Service.Locker.LockRole.Purchaser, "申请退改签", out lockErrorMsg);
            if (!string.IsNullOrEmpty(lockErrorMsg)) InterfaceInvokeException.ThrowCustomMsgException(lockErrorMsg);
            var apply = Service.OrderProcessService.Apply(order.Id, applyformView, Employee, Guid.Empty);
            releaseLock(order.Id);
            return apply.Id;
        }
        private void releaseLock(decimal orderId)
        {
            Service.LockService.UnLock(orderId.ToString(), Employee.UserName);
        }
        private List<PostponeApplyformView.Item> getPostponeVoyages(string voyages, ChinaPay.B3B.Service.Order.Domain.Order order)
        {
            var result = new List<PostponeApplyformView.Item>();
            foreach (var item in voyages.Split('^'))
            {
                foreach (var o in order.PNRInfos)
                {
                    var s = item.Split('|');
                    if (s.Count() != 5)
                    {
                        InterfaceInvokeException.ThrowParameterErrorException("航班信息");
                    }
                    if (s[0].Length != 6)
                    {
                        InterfaceInvokeException.ThrowParameterErrorException("城市对");
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
                            InterfaceInvokeException.ThrowCustomMsgException("当前申请的改期存在两个以上的相同航班信息，请核对航班信息");
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
                        InterfaceInvokeException.ThrowCustomMsgException("当前订单中不存在[" + item + "]的航班信息，请核对航班信息是否有误");
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