using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Order.Domain.Bunk;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Order.Domain;

namespace ChinaPay.B3B.TransactionWeb.FlightHandlers
{
    /// <summary>
    /// ChangeProvider 的摘要说明
    /// </summary>
    public class ChangeProvider : BaseHandler
    {
        private const string _lockOrderRemark = "平台换出票方";      
        public object QueryPolicies(PolicyType policyType, int policyCount, string policyOwner,float rate,bool showTip)
        {
            var source = "5";
            var passengerType = getPassengerType();
            var policyFilterCondition = new Service.PolicyMatch.Domain.PolicyFilterConditions
            {
                PolicyType = policyType,
                Purchaser = CurrentCompany.CompanyId
            };
                // 换出票方时，排除原订单出票方、产品方 和 采购方
                var order = FlightReserveModule.ChoosePolicy.GetOriginalOrder(source);
                policyFilterCondition.ExcludeProviders.Add(order.Purchaser.CompanyId);
                if (order.Supplier != null)
                {
                    policyFilterCondition.ExcludeProviders.Add(order.Supplier.CompanyId);
                }
                if (order.Provider != null)
                {
                    policyFilterCondition.ExcludeProviders.Add(order.Provider.CompanyId);
                }
            // 特殊票时，只取航班查询处选择的价格
            if (policyType == PolicyType.Special)
            {
                var policyView = FlightReserveModule.ChoosePolicy.GetPolicyView(source);
                if (policyView != null)
                {
                    policyFilterCondition.PublishFare = policyView.PublishFare;
                }
            }
            var voyages = getVoyageFilterInfos(source);
            policyFilterCondition.Voyages.AddRange(voyages);
            policyFilterCondition.VoyageType = FlightReserveModule.ChoosePolicy.GetVoyageType(source);
            policyFilterCondition.SuitReduce = hasReduce(source);
            policyFilterCondition.PatPrice = getPatPrice(source);
            policyFilterCondition.Purchaser = order.Purchaser.CompanyId;
            policyFilterCondition.AllowTicketType = ChoosePolicy.FilterByTime(voyages.Min(f => f.Flight.TakeOffTime)); 
            IEnumerable<Service.PolicyMatch.MatchedPolicy> matchedPolicies = null;

            if (FlightReserveModule.ChoosePolicy.ChangeProviderSource == source && FlightReserveModule.ChoosePolicy.GetOriginalOrder(source).IsSpecial)
            {
                matchedPolicies = Service.PolicyMatch.PolicyMatchServcie.MatchBunkForSpecial(policyFilterCondition,false, policyCount).ToList();
            }
            else
            {
                matchedPolicies = Service.PolicyMatch.PolicyMatchServcie.MatchBunk(policyFilterCondition,false, passengerType, policyCount).ToList();
            }
            MatchedPolicyCache = matchedPolicies.ToList();
            var YBPrice = order.PNRInfos.First().Passengers.First().Tickets.Sum(p => p.Price.Fare);
            if (policyType == PolicyType.Special)
            {
                return from item in matchedPolicies
                       where item != null && item.OriginalPolicy != null
                       let specialPolicy = item.OriginalPolicy as SpecialPolicyInfo
                       let specialPolicyInfo = SpecialProductService.Query(specialPolicy.Type)
                       let Provider = GetCompanyInfo(item.Provider)
                       select new
                       {
                           PolicyId = item.Id,
                           PolicyDesc = ChoosePolicy.ReplaceEnter(specialPolicyInfo.Description),
                           spType = ChoosePolicy.ReplaceEnter(specialPolicyInfo.Name),
                           specialPolicy = ChoosePolicy.ReplaceEnter(specialPolicy.Type.ToString()),
                           PolicyOwner = item.Provider,
                           PolicyType = (int)PolicyType.Special,
                           Fare = YBPrice.TrimInvaidZero(),
                           SettleAmount = (YBPrice*(1-item.Commission)).TrimInvaidZero(),
                           EI = ChoosePolicy.ReplaceEnter(getProvision(item.OriginalPolicy as IHasRegulation)),
                           EIList = getProvisionList(item.OriginalPolicy as IHasRegulation),
                           Condition = ChoosePolicy.ReplaceEnter(item.OriginalPolicy.Condition ?? "无"),
                           SuccessOrderCount = item.Statistics.Total.SuccessTicketCount,
                           WorkingTime = getTimeRange(item.WorkStart, item.WorkEnd),
                           VoyageSuccessOrderCount = item.Statistics.Voyage.SuccessTicketCount,
                           OrderSuccessRate = (item.Statistics.Total.OrderSuccessRate * 100).TrimInvaidZero() + "%",
                           item.NeedAUTH,
                           gradeFirst = Math.Floor(item.CompannyGrade),
                           gradeSecond = item.CompannyGrade / 0.1m % 10,
                           needApplication = specialPolicy.ConfirmResource,
                           RenderTicketPrice = specialPolicy.Type == SpecialProductType.CostFree || item.ParValue != 0,
                           PolicyTypes = item.PolicyType.GetDescription(),
                           ProviderName = Provider.AbbreviateName,
                           ProviderAccount = Provider.UserName,
                           IsBusy = Service.Remind.OrderRemindService.QueryProviderRemindInfo(item.Provider).ETDZ > 5,
                           IsHigher = item.Rebate * 100 > (decimal) rate,
                           TipInfo = string.Empty,
                           RelationType = (int)item.RelationType
                       };
            }
            return from item in matchedPolicies
                   let generalPolicy = item.OriginalPolicy as IGeneralPolicy
                   let regulation = item.OriginalPolicy as IHasRegulation
                   let Provider = GetCompanyInfo(item.Provider)
                   select new
                   {
                       Fare = YBPrice.TrimInvaidZero(),
                       Rebate = (item.Commission * 100).TrimInvaidZero() + "%",
                       Commission = (YBPrice  -item.SettleAmount).TrimInvaidZero(),
                       SettleAmount = item.SettleAmount.TrimInvaidZero(),
                       WorkingTime = getTimeRange(item.WorkStart, item.WorkEnd),
                       ScrapTime = getTimeRange(item.RefundStart, item.RefundEnd),
                       ETDZEfficiency = (item.Speed.ETDZ / 60) + "分 ",
                       RefundEfficiency = (item.Speed.Refund / 60) + "分钟",
                       TicketType = (item.OriginalPolicy == null ? TicketType.BSP : item.OriginalPolicy.TicketType).ToString(),
                       PolicyId = item.Id,
                       PolicyOwner = item.Provider,
                       PolicyType = (int)item.PolicyType,
                       OfficeNo = item.OriginalPolicy == null ? item.OfficeNumber : item.OriginalPolicy.OfficeCode,
                       EI = ChoosePolicy.ReplaceEnter((regulation == null ? getEI(source) : getProvision(regulation))),
                       EIList = getProvisionList(item.OriginalPolicy as IHasRegulation),
                       Condition = (item.OriginalPolicy == null ? "无" : ChoosePolicy.ReplaceEnter(item.OriginalPolicy.Condition) ?? "无")
                                   + (generalPolicy != null && generalPolicy.ChangePNR ? "。需要换编码出票" : string.Empty),
                       NeedAUTH = item.OriginalPolicy == null ? item.NeedAUTH : item.OriginalPolicy.NeedAUTH,
                       PolicyTypes = item.PolicyType.GetDescription(),
                       ProviderName = Provider.AbbreviateName,
                       ProviderAccount = Provider.UserName,
                       IsHigher = item.Commission * 100 > (decimal)rate,
                       TipInfo = !showTip ? string.Empty : item.Commission * 100 > (decimal)rate ? "<span class=\"obvious-b\">提示：该供应商返点高于原供应商，选择此供应商出票后平台将能够得到" + Math.Round(item.Commission * 100 - (decimal)rate, 2).ToString() + "%（" + ((item.Commission - (decimal)rate / 100) * item.ParValue).TrimInvaidZero() + "元）</span>" : item.Commission * 100 < (decimal)rate ? "<span class=\"obvious\">请注意：该供应商的返点低于原供应商的返点，选择此政策后平台需要补差额" + Math.Round((decimal)rate - item.Commission * 100, 2).ToString() + "%（" + (((decimal)rate / 100 - item.Commission) * item.ParValue).TrimInvaidZero() + "元）</span>" : string.Empty,
                       RelationType = (int)item.RelationType,
                       setChangePNREnable = !item.IsExternal && (generalPolicy == null || !generalPolicy.ChangePNR)    //采购是否能设置是否允许换编码
                   };
        }

        protected List<MatchedPolicy> MatchedPolicyCache
        {
            get
            {
                if (Session["MatchedPolicy"] != null)
                {
                    return Session["MatchedPolicy"] as List<MatchedPolicy>;
                }
                return new List<MatchedPolicy>();
            }
            set
            {
                if (Session["MatchedPolicy"] == null)
                {
                    Session["MatchedPolicy"] = value;
                }
                else
                {
                    if (value.Count == 0) return;
                    var cache = Session["MatchedPolicy"] as List<MatchedPolicy>;
                    foreach (MatchedPolicy policy in value)
                    {
                        if (cache.Any(p => p.Id == policy.Id))
                        {
                            cache.RemoveAll(p => p.Id == policy.Id);
                        }
                        cache.Add(policy);
                    }
                    Session["MatchedPolicy"] = cache;
                }
            }
        }

        /// <summary>
        /// 获取出票方的公司信息
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        private CompanyDetailInfo GetCompanyInfo(Guid companyId) {
            var result = CompanyService.GetCompanyDetail(companyId);
            if (result == null)
            {
                result = new CompanyDetailInfo
                {
                    AbbreviateName = string.Empty,
                    UserName =  string.Empty
                };
            }
            return result;
        }

        /// <summary>
        /// 换出票方
        /// </summary>
        public string ChangeProviderETDZ(Guid policyId, PolicyType policyType, Guid provider, string officeNo, decimal orderId, bool needAUTH, bool forbidChangePNR)
        {
            //string Msg = string.Empty;
            //BasePage.Lock(orderId,LockRole.Platform, "平台换出票方",out Msg);
            //if (Msg != string.Empty) throw new CustomException(Msg);
            var matchedPolicy = MatchedPolicyCache.FirstOrDefault(p => p.Id == policyId);
            if (matchedPolicy == null) throw new CustomException("政策选择超时,请刷新政策后重新选择");
            //var order = Service.OrderProcessService.ChangeProvider(orderId, provider, policyId, policyType, CurrentUser.UserName);
            var order = Service.OrderProcessService.ChangeProvider(orderId, matchedPolicy, 
                CurrentUser.UserName,forbidChangePNR,needAUTH);
            BasePage.ReleaseLock(orderId);
            if (!order.IsSpecial && order.Source == OrderSource.PlatformOrder &&
                authorize(order.ReservationPNR, officeNo, FlightReserveModule.ChoosePolicy.ChangeProviderSource, order.OEMID??Guid.Empty))
            {
                return string.Empty;
            }
            else
            {
                return order.ReservationPNR.PNR;
            }
        }
        /// <summary>
        /// /锁定订单
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns>锁定失败返回失败描述成功则返回string.Empty</returns>
        public string LockOrder(decimal orderId) 
        {
            string message = string.Empty;
            BasePage.Lock(orderId, LockRole.Platform, _lockOrderRemark, out message);
            return message;
        }
        /// <summary>
        /// 对锁定的订单解锁
        /// </summary>
        /// <param name="orderId">订单号</param>
        public void UnLockOrder(decimal orderId) 
        {
            BasePage.ReleaseLock(orderId);
        }
        internal static IEnumerable<Flight> GetFlights(string source)
        {
            return System.Web.HttpContext.Current.Session["ReservedFlights"] as IEnumerable<Flight>;
        }

        private string getTimeRange(Izual.Time start, Izual.Time end)
        {
            return start.ToString("HH:mm") + "-" + end.ToString("HH:mm");
        }

        private bool authorize(PNRPair pnr, string officeNo, string source, Guid oemId)
        {
            try
            {
                if (FlightReserveModule.ChoosePolicy.ReservateSource == source
                    || FlightReserveModule.ChoosePolicy.UpgradeByQueryFlightSource == source
                    || FlightReserveModule.ChoosePolicy.ChangeProviderSource == source)
                {
                    CommandService.AuthorizeByOfficeNo(pnr, officeNo,oemId);
                }
                return true;
            }
            catch (Exception ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                return false;
            }
        }
        private PassengerType getPassengerType()
        {
            var originalOrder = FlightReserveModule.ChoosePolicy.GetOriginalOrder("5");
            return originalOrder.PNRInfos.First().Passengers.First().PassengerType;
        }
        private IEnumerable<Service.PolicyMatch.Domain.VoyageFilterInfo> getVoyageFilterInfos(string source)
        {
            return (from item in GetFlights(source)
                    select new Service.PolicyMatch.Domain.VoyageFilterInfo
                    {
                        Flight = new Service.PolicyMatch.Domain.FlightFilterInfo
                        {
                            Airline = item.Carrier.Code,
                            Departure = item.Departure.Code,
                            Arrival = item.Arrival.Code,
                            FlightDate = item.TakeoffTime.Date,
                            FlightNumber = item.FlightNo,
                            StandardPrice = item.YBPrice,
                            TakeOffTime = item.TakeoffTime,
                            Fare = item.ReleaseFare
                        },
                        Bunk = new Service.PolicyMatch.Domain.BunkFilterInfo
                        {
                            Code = item.Bunk.Code,
                            Discount = item.Bunk.Discount,
                            Type = item.Bunk is SpecialBunk?BunkType.Economic:item.Bunk.Type
                        } 
                    }).ToList();
        }
        private bool hasReduce(string source)
        {
            if (source == FlightReserveModule.ChoosePolicy.ChangeProviderSource)
            {
                return FlightReserveModule.ChoosePolicy.GetOriginalOrder(source).IsReduce;
            }
            else
            {
                var voyages = FlightReserveModule.ChoosePolicy.GetFlights(source);
                if (voyages.Count() == 2)
                {
                    var patPrice = FlightReserveModule.ChoosePolicy.GetPATPrice(source);
                    if (patPrice != null)
                    {
                        return patPrice.Fare < voyages.Sum(item => item.Fare);
                    }
                }
                return false;
            }
        }
        private decimal? getPatPrice(string source)
        {
            if (source != FlightReserveModule.ChoosePolicy.ChangeProviderSource)
            {
                var patInfo = FlightReserveModule.ChoosePolicy.GetPATPrice(source);
                if (patInfo != null) return patInfo.Fare;
            }
            return null;
        }
        private string getProvision(IHasRegulation regulation)
        {
            if (regulation == null) return string.Empty;
            return " <span><span class='b'>废票规定：</span>"
                   + regulation.InvalidRegulation + "</span> <span> <span><span class='b'>退票规定：</span>" + regulation.RefundRegulation
                   + "</span>   <span class='b'>改签规定：</span>"
                   + regulation.ChangeRegulation + "</span>    <span class='b'>签转规定：</span>"
                   + regulation.EndorseRegulation + "</span>";
        }
        private object getProvisionList(IHasRegulation regulation)
        {
            if (regulation == null) return new[] { new { key = string.Empty, value = string.Empty } };
            return new[]
            {
                new {key="废票规定",value= regulation.InvalidRegulation},
                new {key="退票规定" ,value= regulation.RefundRegulation},
                new {key="改签规定" ,value= regulation.ChangeRegulation},
                new {key="签转规定" ,value= regulation.EndorseRegulation}
            };
        }

        private string getEI(string source)
        {

            var voyages = GetFlights(source);
            var flightView = voyages.ElementAt(0);
            string ei = "(" + flightView.Bunk.Code + ")" + flightView.Bunk.EI;
            foreach (var item in voyages)
            {
                if (flightView.Bunk.Code != item.Bunk.Code)
                    ei += "<br />" + "(" + item.Bunk.Code + ")" + item.Bunk.EI;
            }
            return ei;
        }

    }
}