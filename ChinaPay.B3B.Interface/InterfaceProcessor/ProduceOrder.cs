using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Interface.Cache;
using System.Text.RegularExpressions;
using System.Web;
using ChinaPay.B3B.Interface.PublicClass;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Interface.Processor;

namespace ChinaPay.B3B.Interface.InterfaceProcessor
{
    /// <summary>
    /// 生成订单
    /// </summary>
    internal class ProduceOrder : BaseProcessor
    {
        private string _pnrContent;
        private string _originalPNRContent;
        private string _policyId;
        private Guid _Id;
        private string _associatePNR;
        private string _contact;
        private ReservedPnr _pnr;
        private MatchedPolicy _matchedPolicy;
        private PNRImport _pnrImport;
        private string _batchNo;
        private string _orgbatchNo;
        private CustomContext _customContext;
        private List<DataTransferObject.Command.PNR.PriceView> _patPrices;
        private List<DataTransferObject.FlightQuery.FlightView> _flights;
        private string _isNeedPat;
        private PolicyType _policyType = PolicyType.Bargain;
        private bool fdSuccess;
        public ProduceOrder(string pnrContext, string associatePNR, string contact, string policyId, string batchNo, string userName, string sign)
            : base(userName, sign)
        {
            //if (string.IsNullOrWhiteSpace(batchNo)) throw new InterfaceInvokeException("1", "导入批次号");
            _pnrImport = new PNRImport(userName, sign);
            _policyId = policyId;
            _associatePNR = associatePNR;
            _contact = contact;
            _originalPNRContent = pnrContext;
            _pnrContent = HttpUtility.UrlDecode(pnrContext);
            _orgbatchNo = batchNo;
        }
        protected override void ValidateBusinessParameters()
        {
            if (string.IsNullOrWhiteSpace(_pnrContent)) throw new InterfaceInvokeException("1", "编码");
            if (string.IsNullOrWhiteSpace(_policyId)) throw new InterfaceInvokeException("1", "政策编号");
            if (!Guid.TryParse(_policyId, out _Id)) throw new InterfaceInvokeException("1", "政策编号");
            if (string.IsNullOrWhiteSpace(_orgbatchNo)) throw new InterfaceInvokeException("1", "导入批次号");
            _batchNo = _orgbatchNo.Substring(0, _orgbatchNo.Length - 1);
            _isNeedPat = _orgbatchNo.Substring(_orgbatchNo.Length - 1, 1);
            if (_contact.Trim() != "")
            {
                if (_contact.Split('|').Count() < 3) throw new InterfaceInvokeException("1", "联系信息不完整");
                if (_contact.Split('|')[0].Trim() == "") throw new InterfaceInvokeException("8", "联系信息中姓名");
                if (_contact.Split('|')[1].Trim() == "") throw new InterfaceInvokeException("8", "联系信息中手机");
                //if (_contact.Split('|')[2].Trim() == "") throw new InterfaceInvokeException("8", "联系信息中邮箱");
            }
            if (_associatePNR != "")
            {
                if (!Regex.IsMatch(_associatePNR, "(\\w)+")) throw new InterfaceInvokeException("1", "关联编码");
                if (_associatePNR.Split('|').Any() && _associatePNR.Split('|')[0] != "" && _associatePNR.Split('|')[0].Length != 6) throw new InterfaceInvokeException("1", "关联编码");
                if (_associatePNR.Split('|').Count() == 2 && _associatePNR.Split('|')[1] != "" && _associatePNR.Split('|')[1].Length != 6) throw new InterfaceInvokeException("1", "关联编码");
            }
            try
            {
                var result = CommandService.GetReservedPnr(_pnrContent);
                if (result.Success)
                {
                    _pnr = result.Result;
                }
            }
            catch (Exception)
            {
                throw new InterfaceInvokeException("1", "编码内容");
            }
            if (_pnr == null) throw new InterfaceInvokeException("1", "编码内容");
            //如果遇到证件号不全体提编码
            if (_pnr.Passengers.Any(p => string.IsNullOrEmpty(p.CertificateNumber)) && !PNRPair.IsNullOrEmpty(_pnr.PnrPair))
            {
                var rtResult = CommandService.GetReservedPnr(_pnr.PnrPair, Guid.Empty);
                if (rtResult.Success && !rtResult.Result.HasCanceled)
                {
                    _pnr = rtResult.Result;
                }
            }
            _flights = ReserveViewConstuctor.GetQueryFlightView(_pnr.Voyage.Segments, _pnr.Voyage.Type, _pnr.Passengers.First().Type, _pnr.IsTeam).ToList();
            if (!_flights.Any()) throw new InterfaceInvokeException("9", "编码中缺少航班信息");
            var flight = _flights.FirstOrDefault();
            if (flight.BunkType != null && flight.BunkType.Value == BunkType.Free)
            {
                _patPrices = new List<DataTransferObject.Command.PNR.PriceView> { new DataTransferObject.Command.PNR.PriceView { AirportTax = flight.AirportFee, BunkerAdjustmentFactor = flight.BAF, Fare = 0, Total = flight.AirportFee + flight.BAF } };
            }
            else
            {
                _patPrices = Service.Command.Domain.Utility.Parser.GetPatPrices(_pnrContent);
                if (_patPrices == null) throw new InterfaceInvokeException("9", "缺少PAT内容");
                if (_patPrices.Count == 0) throw new InterfaceInvokeException("9", "缺少PAT内容");
            }
            if (DataTransferObject.Common.PNRPair.IsNullOrEmpty(_pnr.PnrPair)) throw new InterfaceInvokeException("9", "内容中缺少编码");
            if (string.IsNullOrWhiteSpace(_pnr.PnrPair.PNR) && string.IsNullOrWhiteSpace(_pnr.PnrPair.BPNR)) throw new InterfaceInvokeException("9", "编码信息不全");
            //上次导入内容是没有传入pat信息，需要重新匹配政策
            if (_isNeedPat == "1")
            {
                PNRHelper.SaveImportInfo(_pnr, _pnr.PnrPair, _patPrices.MinOrDefaultElement(item => item.Fare), _pnr.Passengers.First().Type, _patPrices.MaxOrDefaultElement(item => item.Fare), out fdSuccess);
                var policyFilterCondition = GetPolicyFilter(_flights);
                //匹配政策
                List<MatchedPolicy> matchedPolicies = Service.PolicyMatch.PolicyMatchServcie.MatchBunk(policyFilterCondition, false, _pnr.Passengers.First().Type, 10).ToList();
                List<MatchedPolicy> matchedSpeciafPolicies = null;
                if ((_policyType & PolicyType.Special) != PolicyType.Special && (_policyType & PolicyType.Team) != PolicyType.Team)
                {
                    policyFilterCondition = GetPolicyFilter(_flights, PolicyType.Special);
                    matchedSpeciafPolicies = Service.PolicyMatch.PolicyMatchServcie.MatchBunk(policyFilterCondition, false, _pnr.Passengers.First().Type, 10).ToList();
                    if (!matchedPolicies.Any() && !matchedSpeciafPolicies.Any())
                        throw new InterfaceInvokeException("9", "没有找到相关政策");
                }
                if (!matchedPolicies.Any()) throw new InterfaceInvokeException("9", "没有找到相关政策");
                _matchedPolicy = matchedPolicies.FirstOrDefault(item => item.Id == _Id);
                if (_matchedPolicy == null)
                {
                    _matchedPolicy = matchedSpeciafPolicies.FirstOrDefault(item => item.Id == _Id);
                }
                if (_matchedPolicy == null && policyFilterCondition.SuitReduce) throw new InterfaceInvokeException("9", "您选择的政策不支持低打。请重新选择");
                if (_matchedPolicy == null) throw new InterfaceInvokeException("9", "没有找到相关政策");
            }
            else
            {
                //从缓存中取出政策 
                _customContext = ContextCenter.Instance[_batchNo];
                if (_customContext == null)
                    throw new InterfaceInvokeException("9", "政策选择超时，请重新导入pnr内容");
                var matchedPolicies = _customContext[_pnr.PnrPair.BPNR + _pnr.PnrPair.PNR] as List<MatchedPolicy>;
                if (matchedPolicies == null) throw new InterfaceInvokeException("9", "政策选择超时,请重新导入pnr内容");
                _matchedPolicy = matchedPolicies.FirstOrDefault(item => item.Id == _Id);
                if (_matchedPolicy == null) throw new InterfaceInvokeException("9", "没找到对应的政策,请重新导入pnr内容");
            }

        }

        protected override string ExecuteCore()
        {
            StringBuilder str = new StringBuilder();
            var flights = ReserveViewConstuctor.GetQueryFlightView(_pnr.Voyage.Segments, _pnr.Voyage.Type, _pnr.Passengers.First().Type, _pnr.IsTeam);
            var orderView = GetOrderView(OrderSource.InterfaceOrder, _pnr, new ChinaPay.B3B.DataTransferObject.Common.PNRPair() { BPNR = _associatePNR == "" ? "" : _associatePNR.Split('|')[0], PNR = _associatePNR == "" ? "" : (_associatePNR.Split('|').Count() < 2 ? "" : _associatePNR.Split('|')[1]) }, flights, _contact);

            var orderInfo = Service.OrderProcessService.ProduceOrder(orderView, _matchedPolicy, Employee, Guid.Empty, false);
            str.AppendFormat("<id>{0}</id>", orderInfo.Id);
            str.AppendFormat("<officeNo>{0}</officeNo>", _matchedPolicy.OriginalPolicy == null && _matchedPolicy.NeedAUTH ? _matchedPolicy.OfficeNumber : (_matchedPolicy.OriginalPolicy != null && _matchedPolicy.OriginalPolicy.NeedAUTH ? _matchedPolicy.OriginalPolicy.OfficeCode : ""));
            str.AppendFormat("<payable>{0}</payable>", _matchedPolicy.ConfirmResource ? 0 : 1);
            str.Append(QueryOrder.GetOrder(orderInfo));
            //str.Append("<order><title>");
            //str.AppendFormat("<id>{0}</id>", orderInfo.Id);
            //str.AppendFormat("<status>{0}</status>", (int)orderInfo.Status);
            //str.AppendFormat("<statusDescription>{0}</statusDescription>", orderInfo.Status.GetDescription());
            //str.AppendFormat("<product>{0}</product>", (int)orderInfo.Product.ProductType);
            //str.AppendFormat("<ticket>{0}</ticket>", (int)orderInfo.Product.TicketType);
            //str.AppendFormat("<associatePNR>{0}</associatePNR>", orderInfo.AssociatePNR == null ? "" : orderInfo.AssociatePNR.BPNR + "|" + orderInfo.AssociatePNR.PNR);
            //str.AppendFormat("<rebate>{0}</rebate>", orderInfo.Purchaser.Rebate);
            //str.AppendFormat("<commission>{0}</commission>", orderInfo.Purchaser.Commission);
            //str.AppendFormat("<amount>{0}</amount>", orderInfo.Purchaser.Amount);
            //str.AppendFormat("<producedTime>{0}</producedTime>", orderInfo.Purchaser.ProducedTime);
            //str.AppendFormat("<payTime>{0}</payTime>", orderInfo.Bill.PayBill.Purchaser.Time);
            //str.AppendFormat("<etdzTime>{0}</etdzTime>", orderInfo.ETDZTime);
            //str.Append("</title>");

            //str.Append("<pnrs>");

            //foreach (var pnr in orderInfo.PNRInfos)
            //{
            //    str.Append("<pnr>"); str.AppendFormat("<code>{0}</code>", pnr.Code == null ? "" : pnr.Code.BPNR + "|" + pnr.Code.PNR);
            //    str.Append("<passengers>");
            //    foreach (var person in pnr.Passengers)
            //    {
            //        str.AppendFormat("<p><name>{0}</name><type>{1}</type><credentitals>{2}</credentitals><mobile>{3}</mobile><settleCode>{4}</settleCode><tickets>{5}</tickets></p>", person.Name, (int)person.PassengerType, person.Credentials, person.Phone, person.Tickets.FirstOrDefault().SettleCode, person.Tickets.Join("|", num => num.No));
            //    }
            //    str.Append("</passengers>");
            //    str.Append("<flights>");
            //    foreach (var filght in pnr.Flights)
            //    {
            //        str.AppendFormat("<f><departure>{0}</departure><arrival>{1}</arrival><airline>{2}</airline><flightNo>{3}</flightNo><aircraft>{11}</aircraft><takeoffTime>{4}</takeoffTime><arrivalTime>{5}</arrivalTime><bunk>{6}</bunk><fare>{7}</fare><discount>{8}</discount><airportFee>{9}</airportFee><baf>{10}</baf></f>", filght.Departure.Code + "|" + filght.Departure.City, filght.Arrival.Code + "|" + filght.Arrival.City, filght.Carrier.Code + "|" + filght.Carrier.Name, filght.FlightNo, filght.TakeoffTime, filght.LandingTime, filght.Bunk.Code, filght.Price.Fare, filght.Bunk.Discount, filght.AirportFee, filght.BAF, filght.AirCraft);
            //    }
            //    str.Append("</flights>");
            //    str.Append("</pnr>");
            //}

            //str.Append("</pnrs>");
            //str.Append("<bills>");
            //if (orderInfo.Bill.PayBill != null)
            //{//支付
            //    str.Append("<b>");
            //    str.AppendFormat("<type>1</type>");
            //    str.AppendFormat("<amount>{0}</amount>", orderInfo.Bill.PayBill.Purchaser.Amount);
            //    str.AppendFormat("<tradeNo>{0}</tradeNo>", orderInfo.Bill.PayBill.Tradement.TradeNo);
            //    str.AppendFormat("<time>{0}</time>", orderInfo.Bill.PayBill.Purchaser.Time);
            //    str.Append("</b>");
            //}

            //if (orderInfo.Bill.NormalRefundBills != null)
            //{//退款
            //    foreach (var item in orderInfo.Bill.NormalRefundBills)
            //    {
            //        str.Append("<b>");
            //        str.AppendFormat("<type>2</type>");
            //        str.AppendFormat("<amount>{0}</amount>", item.Tradement.Amount);
            //        str.AppendFormat("<tradeNo>{0}</tradeNo>", item.Tradement.TradeNo);
            //        str.AppendFormat("<time>{0}</time>", item.Purchaser.Time);
            //        str.Append("</b>");
            //    }
            //}
            //if (orderInfo.Bill.PostponePayBills != null)
            //{//支付
            //    foreach (var item in orderInfo.Bill.PostponePayBills)
            //    {
            //        str.Append("<b>");
            //        str.AppendFormat("<type>1</type>");
            //        str.AppendFormat("<amount>{0}</amount>", item.Tradement.Amount);
            //        str.AppendFormat("<tradeNo>{0}</tradeNo>", item.Tradement.TradeNo);
            //        str.AppendFormat("<time>{0}</time>", item.Applier.Time);
            //        str.Append("</b>");
            //    }
            //}

            //if (orderInfo.Bill.PostponeRefundBills != null)
            //{//退款
            //    foreach (var item in orderInfo.Bill.PostponeRefundBills)
            //    {
            //        str.Append("<b>");
            //        str.AppendFormat("<type>2</type>");
            //        str.AppendFormat("<amount>{0}</amount>", item.Tradement.Amount);
            //        str.AppendFormat("<tradeNo>{0}</tradeNo>", item.Tradement.TradeNo);
            //        str.AppendFormat("<time>{0}</time>", item.Applier.Time);
            //        str.Append("</b>");
            //    }
            //}

            //str.Append("</bills></order>");
            //清空缓存
            ContextCenter.Instance.Remove(_orgbatchNo);
            return str.ToString();
        }
        private DataTransferObject.Order.OrderView GetOrderView(DataTransferObject.Order.OrderSource orderSource, ReservedPnr pnrContent, DataTransferObject.Common.PNRPair associatePNR, IEnumerable<DataTransferObject.FlightQuery.FlightView> flightViews, string Contact)
        {
            var patPrice = _patPrices.First();
            return new DataTransferObject.Order.OrderView
            {
                Source = orderSource,
                PNR = pnrContent.PnrPair,
                Passengers = pnrContent.Passengers.OrderBy(p => p.Name).Select(p => new DataTransferObject.Order.PassengerView
                {
                    Name = p.Name,
                    Credentials = p.CertificateNumber,
                    CredentialsType = p.CertificateType,
                    PassengerType = p.Type,
                    Phone = p.Mobilephone
                }),
                Flights = flightViews.Select(ReserveViewConstuctor.GetOrderFlightView).ToList(),
                Contact = new DataTransferObject.Order.Contact
                {
                    Name = Contact == "" ? Company.Contact : Contact.Split('|')[0],
                    Mobile = Contact == "" ? Company.ContactPhone : Contact.Split('|')[1],
                    Email = Contact == "" ? Company.ContactEmail : Contact.Split('|')[2]
                },
                AssociatePNR = associatePNR,
                IsTeam = pnrContent.IsTeam,
                TripType = pnrContent.Voyage.Type,
                PATPrice = new DataTransferObject.Command.PNR.PriceView
                {
                    AirportTax = patPrice.AirportTax,
                    BunkerAdjustmentFactor = patPrice.BunkerAdjustmentFactor,
                    Fare = _matchedPolicy.ParValue,
                    Total = patPrice.AirportTax + patPrice.BunkerAdjustmentFactor + _matchedPolicy.ParValue
                }
            };
        }

        public static AllowTicketType FilterByTime(DateTime takeOffTime)
        {
            var minutesBeforeTakeOff = (takeOffTime - DateTime.Now).TotalMinutes;
            if (minutesBeforeTakeOff <= SystemParamService.FlightDisableTime) return AllowTicketType.None;
            if (minutesBeforeTakeOff < 60) return AllowTicketType.BSP;
            if (minutesBeforeTakeOff < 2 * 60) return AllowTicketType.B2BOnPolicy;
            return AllowTicketType.Both;
        }
        private Service.PolicyMatch.Domain.PolicyFilterConditions GetPolicyFilter(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights)
        {
            PolicyType policyType = queryPolicyType(flights);
            var policyFilterCondition = new Service.PolicyMatch.Domain.PolicyFilterConditions
            {
                PolicyType = policyType,
                Purchaser = Company.CompanyId,
                AllowTicketType = FilterByTime(flights.Min(item => item.Departure.Time))
            };
            var voyages = getVoyageFilterInfos(flights);
            policyFilterCondition.Voyages.AddRange(voyages);
            policyFilterCondition.VoyageType = _pnr.Voyage.Type == ItineraryType.Conjunction ? VoyageType.TransitWay : (_pnr.Voyage.Type == ItineraryType.Notch ? VoyageType.Notch : (_pnr.Voyage.Type == ItineraryType.OneWay ? VoyageType.OneWay : VoyageType.RoundTrip));
            policyFilterCondition.PatPrice = _patPrices.Min(item => item.Fare);
            policyFilterCondition.SuitReduce = ProduceOrder2.hasReduce(voyages, policyFilterCondition.PatPrice);
            policyFilterCondition.NeedSubsidize = true;
            policyFilterCondition.IsUsePatPrice = fdSuccess || !(flights.Count() == 1 && (flights.FirstOrDefault().BunkType.Value == BunkType.Economic || flights.FirstOrDefault().BunkType.Value == BunkType.FirstOrBusiness));
            return policyFilterCondition;
        }
        private IEnumerable<Service.PolicyMatch.Domain.VoyageFilterInfo> getVoyageFilterInfos(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights)
        {
            return (from item in flights
                    select new Service.PolicyMatch.Domain.VoyageFilterInfo
                    {
                        Flight = new Service.PolicyMatch.Domain.FlightFilterInfo
                        {
                            Airline = item.AirlineCode,
                            Departure = item.Departure.Code,
                            Arrival = item.Arrival.Code,
                            FlightDate = item.Departure.Time.Date,
                            FlightNumber = item.FlightNo,
                            StandardPrice = item.YBPrice,
                            IsShare = item.IsShare
                        },
                        Bunk = item.BunkType.HasValue ? new Service.PolicyMatch.Domain.BunkFilterInfo
                        {
                            Code = item.BunkCode,
                            Discount = item.Discount.HasValue ? item.Discount.Value : 0,
                            Type = item.BunkType.Value
                        } : null
                    }).ToList();
        }
        private PolicyType queryPolicyType(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights)
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
        protected override System.Collections.Specialized.NameValueCollection GetBusinessParameterCollection()
        {
            var collection = new System.Collections.Specialized.NameValueCollection();
            collection.Add("pnrContext", _originalPNRContent);
            collection.Add("associatePNR", _associatePNR);
            collection.Add("contact", _contact);
            collection.Add("policyId", _policyId);
            collection.Add("batchNo", _orgbatchNo);
            return collection;
        }
        private Service.PolicyMatch.Domain.PolicyFilterConditions GetPolicyFilter(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights, PolicyType type)
        {
            var policyFilterCondition = new Service.PolicyMatch.Domain.PolicyFilterConditions
            {
                PolicyType = type,
                Purchaser = Company.CompanyId,
                AllowTicketType = FilterByTime(flights.Min(item => item.Departure.Time))
            };
            var voyages = getVoyageFilterInfos(flights);
            policyFilterCondition.Voyages.AddRange(voyages);
            policyFilterCondition.VoyageType = _pnr.Voyage.Type == ItineraryType.Conjunction ? VoyageType.TransitWay : (_pnr.Voyage.Type == ItineraryType.Notch ? VoyageType.Notch : (_pnr.Voyage.Type == ItineraryType.OneWay ? VoyageType.OneWay : VoyageType.RoundTrip));
            policyFilterCondition.PatPrice = _patPrices.Min(item => item.Fare);
            policyFilterCondition.SuitReduce = ProduceOrder2.hasReduce(voyages, policyFilterCondition.PatPrice);
            policyFilterCondition.NeedSubsidize = true;
            policyFilterCondition.IsUsePatPrice = fdSuccess || !(flights.Count() == 1 && (flights.FirstOrDefault().BunkType.Value == BunkType.Economic || flights.FirstOrDefault().BunkType.Value == BunkType.FirstOrBusiness));
            return policyFilterCondition;
        }
    }
}