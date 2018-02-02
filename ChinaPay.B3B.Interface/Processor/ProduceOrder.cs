using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Interface.Cache;
using ChinaPay.B3B.Interface.PublicClass;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Interface.Processor
{
    class ProduceOrder : RequestProcessor
    {
        private string _policyId;
        private Guid _Id;
        private string _associatePNR;
        private string _contact;
        private ReservedPnr _pnr;
        private MatchedPolicy _matchedPolicy;
        private string _batchNo;
        private string _orgbatchNo;
        private CustomContext _customContext;
        private List<DataTransferObject.Command.PNR.PriceView> _patPrices;
        private List<DataTransferObject.FlightQuery.FlightView> _flights;
        private string _isNeedPat;
        private PolicyType _policyType = PolicyType.Bargain;
        private bool fdSuccess;


        protected override string ExecuteCore()
        {
            var pnrContext = Context.GetParameterValue("pnrContent");
            var patContent = Context.GetParameterValue("patContent");
            _policyId = Context.GetParameterValue("policyId");
            _associatePNR = Context.GetParameterValue("associatePNR");
            _contact = Context.GetParameterValue("contact");
            _orgbatchNo = Context.GetParameterValue("batchNo");
            ValidateBusinessParameters(HttpUtility.UrlDecode(pnrContext), HttpUtility.UrlDecode(patContent));

            StringBuilder str = new StringBuilder();
            var flights = ReserveViewConstuctor.GetQueryFlightView(_pnr.Voyage.Segments, _pnr.Voyage.Type, _pnr.Passengers.First().Type, _pnr.IsTeam);
            var orderView = GetOrderView(OrderSource.InterfaceOrder, _pnr, new ChinaPay.B3B.DataTransferObject.Common.PNRPair() { BPNR = _associatePNR == "" ? "" : _associatePNR.Split('|')[0], PNR = _associatePNR == "" ? "" : (_associatePNR.Split('|').Count() < 2 ? "" : _associatePNR.Split('|')[1]) }, flights, _contact);

            var orderInfo = Service.OrderProcessService.ProduceOrder(orderView, _matchedPolicy, Employee, Guid.Empty, false);
            str.AppendFormat("<id>{0}</id>", orderInfo.Id);
            str.AppendFormat("<officeNo>{0}</officeNo>", _matchedPolicy.OriginalPolicy == null && _matchedPolicy.NeedAUTH ? _matchedPolicy.OfficeNumber : (_matchedPolicy.OriginalPolicy != null && _matchedPolicy.OriginalPolicy.NeedAUTH ? _matchedPolicy.OriginalPolicy.OfficeCode : ""));
            str.AppendFormat("<payable>{0}</payable>", _matchedPolicy.ConfirmResource ? 0 : 1);
            str.Append(ReturnStringUtility.GetOrder(orderInfo));
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


        protected  void ValidateBusinessParameters(string pnrContent, string patContent)
        {
            if (string.IsNullOrWhiteSpace(pnrContent)) InterfaceInvokeException.ThrowParameterErrorException("编码");
            if (string.IsNullOrWhiteSpace(patContent)) InterfaceInvokeException.ThrowParameterErrorException("PAT内容");
            if (string.IsNullOrWhiteSpace(_policyId)) InterfaceInvokeException.ThrowParameterErrorException("政策编号");
            if (!Guid.TryParse(_policyId, out _Id)) InterfaceInvokeException.ThrowParameterErrorException("政策编号");
            if (string.IsNullOrWhiteSpace(_orgbatchNo)) InterfaceInvokeException.ThrowParameterErrorException("导入批次号");
            _batchNo = _orgbatchNo.Substring(0, _orgbatchNo.Length - 1);
            _isNeedPat = _orgbatchNo.Substring(_orgbatchNo.Length - 1, 1);
            if (_contact.Trim() != "")
            {
                if (_contact.Split('|').Count() < 3) InterfaceInvokeException.ThrowParameterErrorException("联系信息不完整");
                if (_contact.Split('|')[0].Trim() == "") InterfaceInvokeException.ThrowParameterMissException("联系信息中姓名");
                if (_contact.Split('|')[1].Trim() == "") InterfaceInvokeException.ThrowParameterMissException("联系信息中手机");
            }
            if (_associatePNR != "")
            {
                if (!Regex.IsMatch(_associatePNR, "(\\w)+")) InterfaceInvokeException.ThrowParameterErrorException("关联编码");
                if (_associatePNR.Split('|').Any() && _associatePNR.Split('|')[0] != "" && _associatePNR.Split('|')[0].Length != 6) InterfaceInvokeException.ThrowParameterErrorException("关联编码");
                if (_associatePNR.Split('|').Count() == 2 && _associatePNR.Split('|')[1] != "" && _associatePNR.Split('|')[1].Length != 6) InterfaceInvokeException.ThrowParameterErrorException("关联编码");
            }
            try
            {
                var result = CommandService.GetReservedPnr(pnrContent);
                if (result.Success)
                {
                    _pnr = result.Result;
                }
            }
            catch (Exception)
            {
                InterfaceInvokeException.ThrowParameterErrorException("编码内容");
            }
            if (_pnr == null) InterfaceInvokeException.ThrowParameterErrorException("编码内容");
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
            if (!_flights.Any()) InterfaceInvokeException.ThrowCustomMsgException("编码中缺少航班信息");
            var flight = _flights.FirstOrDefault();
            if (flight.BunkType != null && flight.BunkType.Value == BunkType.Free)
            {
                _patPrices = new List<DataTransferObject.Command.PNR.PriceView> { new DataTransferObject.Command.PNR.PriceView { AirportTax = flight.AirportFee, BunkerAdjustmentFactor = flight.BAF, Fare = 0, Total = flight.AirportFee + flight.BAF } };
            }
            else
            {
                _patPrices = Service.Command.Domain.Utility.Parser.GetPatPrices(patContent);
                if (_patPrices == null) InterfaceInvokeException.ThrowCustomMsgException("缺少PAT内容");
                if (_patPrices.Count == 0) InterfaceInvokeException.ThrowCustomMsgException("缺少PAT内容");
            }
            if (DataTransferObject.Common.PNRPair.IsNullOrEmpty(_pnr.PnrPair)) InterfaceInvokeException.ThrowCustomMsgException("内容中缺少编码");
            if (string.IsNullOrWhiteSpace(_pnr.PnrPair.PNR) && string.IsNullOrWhiteSpace(_pnr.PnrPair.BPNR)) InterfaceInvokeException.ThrowCustomMsgException("编码信息不全");

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
                        InterfaceInvokeException.ThrowNotFindPolicyException();
                }
                if (!matchedPolicies.Any()) InterfaceInvokeException.ThrowNotFindPolicyException();
                _matchedPolicy = matchedPolicies.FirstOrDefault(item => item.Id == _Id);
                if (_matchedPolicy == null)
                {
                    _matchedPolicy = matchedSpeciafPolicies.FirstOrDefault(item => item.Id == _Id);
                }
                if (_matchedPolicy == null && policyFilterCondition.SuitReduce) InterfaceInvokeException.ThrowCustomMsgException("您选择的政策不支持低打。请重新选择");
                if (_matchedPolicy == null) InterfaceInvokeException.ThrowNotFindPolicyException();
            }
            else
            {
                //从缓存中取出政策 
                _customContext = ContextCenter.Instance[_batchNo];
                if (_customContext == null)
                    InterfaceInvokeException.ThrowCustomMsgException("政策选择超时，请重新导入pnr内容");
                var matchedPolicies = _customContext[_pnr.PnrPair.BPNR + _pnr.PnrPair.PNR] as List<MatchedPolicy>;
                if (matchedPolicies == null) InterfaceInvokeException.ThrowCustomMsgException("政策选择超时,请重新导入pnr内容");
                _matchedPolicy = matchedPolicies.FirstOrDefault(item => item.Id == _Id);
                if (_matchedPolicy == null) InterfaceInvokeException.ThrowNotFindPolicyException();
            }
        }

        private Service.PolicyMatch.Domain.PolicyFilterConditions GetPolicyFilter(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights, PolicyType type)
        {
            var policyFilterCondition = new Service.PolicyMatch.Domain.PolicyFilterConditions
            {
                PolicyType = type,
                Purchaser = Company.CompanyId,
                AllowTicketType = ReturnStringUtility.FilterByTime(flights.Min(item => item.Departure.Time))
            };
            var voyages = getVoyageFilterInfos(flights);
            policyFilterCondition.Voyages.AddRange(voyages);
            policyFilterCondition.VoyageType = _pnr.Voyage.Type == ItineraryType.Conjunction ? VoyageType.TransitWay : (_pnr.Voyage.Type == ItineraryType.Notch ? VoyageType.Notch : (_pnr.Voyage.Type == ItineraryType.OneWay ? VoyageType.OneWay : VoyageType.RoundTrip));
            policyFilterCondition.PatPrice = _patPrices.Min(item => item.Fare);
            policyFilterCondition.NeedSubsidize = true;
            policyFilterCondition.IsUsePatPrice = false;
            return policyFilterCondition;
        }
        private Service.PolicyMatch.Domain.PolicyFilterConditions GetPolicyFilter(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights)
        {
            PolicyType policyType = ReturnStringUtility.QueryPolicyType(flights, _pnr);
            var policyFilterCondition = new Service.PolicyMatch.Domain.PolicyFilterConditions
            {
                PolicyType = policyType,
                Purchaser = Company.CompanyId,
                AllowTicketType = ReturnStringUtility.FilterByTime(flights.Min(item => item.Departure.Time))
            };
            var voyages = getVoyageFilterInfos(flights);
            policyFilterCondition.Voyages.AddRange(voyages);
            policyFilterCondition.VoyageType = _pnr.Voyage.Type == ItineraryType.Conjunction ? VoyageType.TransitWay : (_pnr.Voyage.Type == ItineraryType.Notch ? VoyageType.Notch : (_pnr.Voyage.Type == ItineraryType.OneWay ? VoyageType.OneWay : VoyageType.RoundTrip));
            policyFilterCondition.PatPrice = _patPrices.Min(item => item.Fare);
            policyFilterCondition.SuitReduce = ProduceOrder2.hasReduce(voyages, policyFilterCondition.PatPrice);
            policyFilterCondition.NeedSubsidize = true;
            policyFilterCondition.IsUsePatPrice = false;
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

    }
}