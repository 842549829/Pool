using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.B3B.TransactionWeb.FlightReserveModule;
using ChinaPay.Core;
using ChinaPay.B3B.TransactionWeb.PublicClass;

namespace ChinaPay.B3B.TransactionWeb.OrderHandlers
{
    /// <summary>
    /// Apply 的摘要说明
    /// </summary>
    public class Apply : BaseHandler
    {
        private const string OK = "OK";

        /// <summary>
        /// 申请退/废票
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="pnrCode">编码(小编码|大编码)</param>
        /// <param name="passengers">乘机人(乘机人id,以','隔开)</param>
        /// <param name="voyages">航段(航段id,以','隔开)</param>
        /// <param name="refundType">退票类型(当日作废:-1,升舱全退:0,自愿按客规退票:1,非自愿退票:2,特殊原因退票:3)</param>
        /// <param name="reason">退/废票原因</param>
        /// <param name="DelegageCancelPNR">是否委托平台取消编码</param>
        public string ApplyRefund(decimal orderId, string pnrCode, string passengers, string voyages, int refundType, string reason, bool DelegageCancelPNR, string filePath)
        {
            try
            {
                RefundOrScrapApplyformView applyformView = null;
                if (refundType == -1)
                {
                    applyformView = new ScrapApplyformView();
                }
                else
                {
                   applyformView = new RefundApplyformView();
                   var view = applyformView as RefundApplyformView;
                   view.RefundType = (RefundType)refundType;
                   if (!string.IsNullOrEmpty(filePath) && (view.RefundType == RefundType.SpecialReason || view.RefundType == RefundType.Involuntary))
                   {
                       var bytes = FileService.GetFileBytes(filePath);
                       Thumbnail thumbnail = new Thumbnail();
                       view.ApplyAttachmentView = new List<ApplyAttachmentView>();
                       view.ApplyAttachmentView.Add(new ApplyAttachmentView { 
                         FilePath = filePath,
                         Thumbnail = thumbnail.MakeThumb(100 ,bytes)
                       });
                       //view.Thumbnail = thumbnail.MakeThumb(100, "");
                   }
                   applyformView = view;
                }
                applyformView.PNR = getPNRPair(pnrCode);
                applyformView.Passengers = getPassengers(passengers);
                applyformView.Reason = reason;
                applyformView.DelegageCancelPNR = DelegageCancelPNR;
                foreach (var item in getRefundVoyages(voyages))
                {
                    applyformView.AddVoyage(item);
                }
                if (Session["NeedPlatformCancelPNR"] != null && Session["NeedPlatformCancelPNR"].ToString() == orderId.ToString())
                {
                    applyformView.NeedPlatfromCancelPNR = true;
                }
                var applyform = OrderProcessService.Apply(orderId, applyformView, CurrentUser, BasePage.OwnerOEMId) as RefundOrScrapApplyform;
                releaseLock(orderId);
                //if (applyform.Status == RefundApplyformStatus.AppliedForProvider && (applyform.Order.Source == OrderSource.ContentImport || applyform.Order.Source == OrderSource.CodeImport || applyform.Order.Source == OrderSource.InterfaceOrder))
                //{
                //    var pnr = CommandService.GetReservedPnr(applyformView.PNR, Guid.Empty);
                //    if (!pnr.Success || pnr.Success && !pnr.Result.HasCanceled)
                //    {
                //        return "OK1";
                //    }
                //}

                return OK;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 申请改期
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="pnrCode">编码(小编码|大编码)</param>
        /// <param name="passengers">乘机人(乘机人id,以','隔开)</param>
        /// <param name="voyages">航段(航段id|新航班号|新航班日期,以','隔开)</param>
        /// <param name="remark">改期备注</param>
        public string ApplyPostpone(decimal orderId, string pnrCode, string passengers, string voyages, string remark, string carrair)
        {
            try
            {
                var applyformView = new PostponeApplyformView()
                {
                    PNR = getPNRPair(pnrCode),
                    Passengers = getPassengers(passengers),
                    Reason = remark
                };
                var flightChangeLimit = SystemDictionaryService.Query(SystemDictionaryType.FlightChangeLimit);
                var limit = LimitItem.Parse(flightChangeLimit);
                foreach (var item in getPostponeVoyages(voyages))
                {
                    bool isTodayFlight = DateTime.Today == item.NewFlightDate.Date;
                    foreach (LimitItem l in limit)
                    {
                        if (carrair.ToUpper() != l.Carrair.ToUpper()) continue;
                        if (item.NewFlightDate >= l.LimitFrom && item.NewFlightDate <= l.LimitTo && (!isTodayFlight || !l.ToTodayEnable))
                        {
                            var aline = FoundationService.Airlines.FirstOrDefault(p => p.Code.Value == l.Carrair);
                            throw new CustomException("由于[" + aline.ShortName + "]原因，该客票已被航空公司限制改期，无法改期，请让乘机人自行致电航空公司或到航空公司直营柜台办理改期");
                        }
                    }
                    applyformView.AddItem(item);

                }
                var applyform = OrderProcessService.Apply(orderId, applyformView, CurrentUser, BasePage.OwnerOEMId) as PostponeApplyform;
                releaseLock(orderId);
                return OK;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 申请升舱
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="pnrCode">编码(小编码|大编码)</param>
        /// <param name="passengers">乘机人(乘机人id,以','隔开)</param>
        /// <param name="voyages">航段(航段id|新航班号|新航班日期,以','隔开)</param>
        /// <param name="originalPNR">原始编码 </param>
        public object ApplyUpgrade(decimal orderId, string pnrCode, List<PassengerViewEx> passengers, List<FlihgtInfo> voyages, string originalPNR)
        {
            try
            {
                var pnrPair = originalPNR.Split('|');
                if (originalPNR.ToUpper().IndexOf(pnrCode.ToUpper(), StringComparison.Ordinal) > -1) throw new CustomException("编码与原编码不能相同");
                var flightViews = ImportHelper.AnalysisPNR(pnrCode, HttpContext.Current);
                if (flightViews.Item2.Count() != passengers.Count) throw new CustomException("所选乘客与编码中的乘客数量不一致");
                if (flightViews.Item1.Count() != voyages.Count) throw new CustomException("所选航班与编码中的航班数量不一致");
                var ValidateInfo = passengers.Join(flightViews.Item2, p => p.Name, p => p.Name, (p, q) => 1);
                if (ValidateInfo.Count() != passengers.Count) throw new CustomException("编码中的乘客姓名与所选乘客姓名不匹配！");
                var order = OrderQueryService.QueryOrder(orderId);
                if (order == null) throw new ArgumentNullException("订单不存在");
                List<Flight> allOrderFlights = new List<Flight>();
                foreach (PNRInfo info in order.PNRInfos)
                {
                    allOrderFlights.AddRange(info.Flights);
                }
                var applyformView = new UpgradeApplyformView()
                {
                    NewPNR = new PNRPair(pnrCode, string.Empty),
                    Passengers = passengers.Select(p => p.PassengerId),
                    PNRSource = OrderSource.CodeImport,
                    PNR = new PNRPair(pnrPair[0], pnrPair[1])
                };
                foreach (var item in voyages)
                {
                    var flight = flightViews.Item1.FirstOrDefault(f => f.Departure.Code == item.Departure && f.Arrival.Code == item.Arrival);
                    if (flight == null) throw new NullReferenceException("所选择的航程与编码提取航程不对应！");
                    applyformView.AddItem(new UpgradeApplyformView.Item()
                    {
                        Voyage = item.flightId,
                        Flight = new DataTransferObject.Order.FlightView()
                            {
                                SerialNo = flight.Serial,
                                Airline = flight.AirlineCode,
                                FlightNo = flight.FlightNo,
                                Departure = flight.Departure.Code,
                                Arrival = flight.Arrival.Code,
                                AirCraft = flight.Aircraft,
                                TakeoffTime = flight.Departure.Time,
                                LandingTime = flight.Arrival.Time,
                                YBPrice = flight.YBPrice,
                                Bunk = flight.BunkCode,
                                Type = flight.BunkType == null ? BunkType.Economic : flight.BunkType.Value,
                                Fare = flight.Fare
                            }
                    });
                }

                HttpContext.Current.Session["ApplyformView"] = applyformView;
                HttpContext.Current.Session["Passengers"] = passengers;
                HttpContext.Current.Session["ReservedFlights"] = flightViews.Item1;



                //Service.OrderProcessService.Apply(orderId, applyformView, CurrentUser.UserName);
                //releaseLock(orderId);
                return new
                {
                    IsSuccess = true,
                    QueryString = string.Format("?source=3&orderId={0}&provider={1}", orderId, order.Provider.CompanyId)
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    IsSuccess = false,
                    QueryString = ex.Message
                };
            }
        }

        /// <summary>
        /// 同意改期
        /// </summary>
        public void AgreePostponeWithoutFee(decimal applyformId, PostponeView postponeView)
        {
            Service.ApplyformProcessService.AgreePostpone(applyformId, postponeView, CurrentUser.UserName, CurrentUser.Name);
            BasePage.ReleaseLock(applyformId);
        }
        /// <summary>
        /// 收取改期费
        /// </summary>
        public void AgreePostponeWithFee(decimal applyformId, IEnumerable<PostponeFeeView> postponeFeeViews)
        {
            Service.ApplyformProcessService.AgreePostponeForFee(applyformId, postponeFeeViews, CurrentUser.UserName);
            BasePage.ReleaseLock(applyformId);
        }
        /// <summary>
        /// 拒绝改期
        /// </summary>
        public void DenyPostpone(decimal applyformId, string reason)
        {
            Service.ApplyformProcessService.DenyPostpone(applyformId, reason, CurrentUser.UserName);
            BasePage.ReleaseLock(applyformId);
        }

        /// <summary>
        /// 获取字典表子项
        /// </summary>
        /// <param name="sdType"> </param>
        /// <returns></returns>
        public object GetDictionaryItems(SystemDictionaryType sdType)
        {
            return SystemDictionaryService.Query(sdType);
        }

        public bool DeleteApplyAttachmentView(Guid applyAttachmentId) 
        {
            return ApplyformQueryService.DeleteApplyAttachmentView(applyAttachmentId, CurrentUser.UserName);
        }

        public object CheckRefundCondition(decimal orderId, string passenger, string voyages, int refundType, string pnr, bool DelegageCancelPNR)
        {

            var passgners = getPassengers(passenger);
            var _voyages = getRefundVoyages(voyages);
            var pnrPair = getPNRPair(pnr);
            var order = OrderQueryService.QueryOrder(orderId);


            RefundOrScrapApplyformView applyformView = null;
            if (refundType == -1)
            {
                applyformView = new ScrapApplyformView();
            }
            else
            {
                applyformView = new RefundApplyformView();
                var view = applyformView as RefundApplyformView;
                view.RefundType = (RefundType)refundType;

                applyformView = view;
            }
            applyformView.PNR = pnrPair;
            applyformView.Passengers = passgners;
            applyformView.Reason = "退票验证";
            applyformView.DelegageCancelPNR = DelegageCancelPNR;
            foreach (var item in getRefundVoyages(voyages))
            {
                applyformView.AddVoyage(item);
            }
            OrderProcessService.ApplyValidate(orderId, applyformView, CurrentUser, BasePage.OwnerOEMId);
            if (order==null)
            {
                throw new CustomException("订单不存在!");
            }
            if (!SystemParamService.ValidateRefundCondition)
            {
                return new
                {
                    PNRCancled = true,
                    TicketUnUse = true,
                    IsNotPrinted = true,
                    IsSameName = true,
                    Successed = true,
                    NeedPlatfromDeal = false,
                    CheckCondition = false
                };
            }
            var result = OrderProcessService.CheckRefundCondition(order, passgners, _voyages, pnrPair, DelegageCancelPNR,BasePage.OwnerOEMId);
            if (result.Item5)
            {
                return new
                {
                    PNRCancled = result.Item1,
                    TicketUnUse = result.Item2,
                    IsNotPrinted = result.Item3,
                    IsSameName = result.Item4,
                    Successed = result.Item1 && result.Item2 && result.Item3 && result.Item4,
                    NeedPlatfromDeal = false,
                    CheckCondition = true
                };
                
            }
            Session["NeedPlatformCancelPNR"] = orderId;
            return new
            {
                PNRCancled = result.Item1,
                TicketUnUse = false,
                IsNotPrinted = false,
                IsSameName = false,
                Successed = true,
                NeedPlatfromDeal = true,
                CheckCondition = true
            };
        }




        private PNRPair getPNRPair(string pnrCode)
        {
            var codes = pnrCode.Split('|');
            return new PNRPair(codes[0], codes[1]);
        }
        private List<Guid> getPassengers(string passengers)
        {
            return getGuidList(passengers, ',');
        }
        private List<Guid> getRefundVoyages(string voyages)
        {
            return getGuidList(voyages, ',');
        }
        private List<PostponeApplyformView.Item> getPostponeVoyages(string voyages)
        {
            var result = new List<PostponeApplyformView.Item>();
            foreach (var item in voyages.Split(','))
            {
                var dataArray = item.Split('|');
                if (dataArray.Length == 3)
                {
                    result.Add(new PostponeApplyformView.Item()
                    {
                        Voyage = Guid.Parse(dataArray[0]),
                        NewFlightNo = dataArray[1],
                        NewFlightDate = DateTime.Parse(dataArray[2])
                    });
                }
            }
            return result;
        }
        private List<Guid> getGuidList(string value, char separator)
        {
            var result = new List<Guid>();
            foreach (var item in value.Split(separator))
            {
                result.Add(Guid.Parse(item));
            }
            return result;
        }
        private void releaseLock(decimal orderId)
        {
            Service.LockService.UnLock(orderId.ToString(), CurrentUser.UserName);
        }

    }

    public class FlihgtInfo
    {
        public Guid flightId
        {
            get;
            set;
        }
        public string Departure
        {
            get;
            set;
        }
        public string Arrival
        {
            get;
            set;
        }
        public string Airline
        {
            get;
            set;
        }
        public string FlightNo
        {
            get;
            set;
        }
        public string AirCraft
        {
            get;
            set;
        }
        public string Bunk
        {
            get;
            set;
        }
    }

    public class PassengerViewEx : PassengerView
    {
        public Guid PassengerId
        {
            get;
            set;
        }
    }

    public class LimitItem
    {
        public Guid Id
        {
            get;
            set;
        }
        public string Carrair
        {
            get;
            set;
        }
        public DateTime LimitFrom
        {
            get;
            set;
        }
        public DateTime LimitTo
        {
            get;
            set;
        }
        private bool _toTodayEnable = true;
        public bool ToTodayEnable
        {
            get
            {
                return _toTodayEnable;
            }
            set
            {
                _toTodayEnable = value;
            }
        }

        public static List<LimitItem> Parse(IEnumerable<SystemDictionaryItem> param)
        {
            var reg = new Regex(@"(?<carrair>\w{2})/(?<from>.*?)/(?<to>.*)/(?<ToTodayEnable>.*)");
            var result = new List<LimitItem>();
            foreach (var item in param)
            {
                var match = reg.Match(item.Value);
                if (match.Success)
                {
                    result.Add(new LimitItem
                    {
                        Id = item.Id,
                        Carrair = match.Groups["carrair"].Value,
                        LimitFrom = DateTime.Parse(match.Groups["from"].Value),
                        LimitTo = DateTime.Parse(match.Groups["to"].Value).AddDays(1).AddSeconds(-1),
                        ToTodayEnable = match.Groups["ToTodayEnable"].Value == "1"
                    });
                }
            }
            return result;
        }

    }
}