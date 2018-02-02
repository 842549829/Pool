using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Interface.PublicClass;

namespace ChinaPay.B3B.Interface.Processor
{
    class ApplyRefund : RequestProcessor
    {
        protected override string ExecuteCore()
        {
            var orderId = Context.GetParameterValue("orderId");
            var passengers = Context.GetParameterValue("passengers");
            var voyages = Context.GetParameterValue("voyages");
            var refundType = Context.GetParameterValue("refundType");
            var reason = Context.GetParameterValue("reason");

            if (string.IsNullOrWhiteSpace(orderId)) InterfaceInvokeException.ThrowParameterMissException("orderId");
            if (string.IsNullOrWhiteSpace(passengers)) InterfaceInvokeException.ThrowParameterMissException("passengers");
            if (string.IsNullOrWhiteSpace(voyages)) InterfaceInvokeException.ThrowParameterMissException("voyages");
            if (string.IsNullOrWhiteSpace(refundType)) InterfaceInvokeException.ThrowParameterMissException("refundType");
            if (string.IsNullOrWhiteSpace(reason)) InterfaceInvokeException.ThrowParameterMissException("reason");

            decimal id;
            if (!decimal.TryParse(orderId, out id)) InterfaceInvokeException.ThrowParameterErrorException("订单号");
            var order = Service.OrderQueryService.QueryOrder(id);
            if (order == null) InterfaceInvokeException.ThrowCustomMsgException("暂无此订单");
            if (order.Purchaser.CompanyId != Company.CompanyId) InterfaceInvokeException.ThrowCustomMsgException("暂无此订单");
            id = ApplyRefundObj(order, refundType, passengers, voyages, reason);
            var obj = Service.ApplyformQueryService.QueryApplyform(id);
            return ReturnStringUtility.GetApplyform(obj);

        }
        private decimal ApplyRefundObj(ChinaPay.B3B.Service.Order.Domain.Order order, string refundType, string passengers, string voyages, string reason)
        {

            RefundOrScrapApplyformView applyformView = null;
            if (refundType == "-1")
            {
                applyformView = new ScrapApplyformView();
            }
            else
            {
                applyformView = new RefundApplyformView()
                {
                    RefundType = (RefundType)int.Parse(refundType)
                };
            }
            var app = Service.ApplyformQueryService.QueryApplyforms(order.Id);
            string pid = "";
            string vid = "";
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
                        foreach (var f in voyages.Split('^'))
                        {
                            if (f.Split('|').Count() < 3)
                            {
                                InterfaceInvokeException.ThrowParameterErrorException("航班信息");
                            }
                            if (f.Split('|')[0].Length != 6)
                            {
                                InterfaceInvokeException.ThrowParameterErrorException("城市对");
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
            if (pid == "") InterfaceInvokeException.ThrowCustomMsgException("当前订单中没有找到对应的乘机人信息");
            if (vid == "") InterfaceInvokeException.ThrowCustomMsgException("当前订单中没有找到对应的航班信息");
            if (!_pnrCode.Any()) InterfaceInvokeException.ThrowCustomMsgException("当前订单中没有找到对应的编码信息");

            applyformView.PNR = _pnrCode.FirstOrDefault();
            applyformView.Passengers = ApplyOrder.getPassengers(pid);
            applyformView.Reason = reason;
            foreach (var item in ApplyOrder.getRefundVoyages(vid))
            {
                applyformView.AddVoyage(item);
            }
            string lockErrorMsg = "";
            ReturnStringUtility.Lock(order.Id, Service.Locker.LockRole.Purchaser, Company, Employee, "申请退改签", out lockErrorMsg);
            if (!string.IsNullOrEmpty(lockErrorMsg)) InterfaceInvokeException.ThrowCustomMsgException(lockErrorMsg);
            var apply = Service.OrderProcessService.Apply(order.Id, applyformView, Employee, Guid.Empty);
            ReturnStringUtility.releaseLock(order.Id, Company, Employee);
            return apply.Id;
        }

    }
}