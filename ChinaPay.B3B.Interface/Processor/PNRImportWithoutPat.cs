using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Interface.PublicClass;
using ChinaPay.B3B.Service.PolicyMatch;
using System.Text;
using ChinaPay.B3B.Interface.Cache;

namespace ChinaPay.B3B.Interface.Processor
{
    class PNRImportWithoutPat : RequestProcessor
    {
        private ReservedPnr _pnr;
        private List<PriceView> _patPrices;
        private List<DataTransferObject.FlightQuery.FlightView> _flights;
        private PolicyType _policyType = PolicyType.Bargain;
        protected override string ExecuteCore()
        {
            var pnrContext = Context.GetParameterValue("pnrContent");
            ValidateBusinessParameters(HttpUtility.UrlDecode(pnrContext));

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
            if (!matchedPolicies.Any())
                InterfaceInvokeException.ThrowNotFindPolicyException();

            StringBuilder str = new StringBuilder();
            str.Append("<policies>");
            ReturnStringUtility.GetPolicy(matchedPolicies, matchedSpeciafPolicies, str, _policyType, _flights,InterfaceSetting);
            str.Append("</policies>");
            //将匹配出来的政策存入缓存中 
            CustomContext context = CustomContext.NewContext();
            context[_pnr.PnrPair.BPNR + _pnr.PnrPair.PNR] = matchedPolicies;
            ContextCenter.Instance.Save(context);
            str.AppendFormat("<batchNo>{0}</batchNo>", context.Id + "1");
            return str.ToString();
        }
        protected  void ValidateBusinessParameters(string _pnrContent)
        {
            if (string.IsNullOrWhiteSpace(_pnrContent)) InterfaceInvokeException.ThrowParameterMissException("编码内容");
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
                InterfaceInvokeException.ThrowParameterErrorException("编码内容");
            }
            if (_pnr == null) InterfaceInvokeException.ThrowParameterErrorException("编码内容");
            if (DataTransferObject.Common.PNRPair.IsNullOrEmpty(_pnr.PnrPair)) InterfaceInvokeException.ThrowParameterErrorException("内容中缺少编码");
            if (string.IsNullOrWhiteSpace(_pnr.PnrPair.PNR) && string.IsNullOrWhiteSpace(_pnr.PnrPair.BPNR)) InterfaceInvokeException.ThrowParameterErrorException("编码信息不全");
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
            _policyType = ReturnStringUtility.QueryPolicyType(_flights, _pnr);
            var flight = _flights.FirstOrDefault();
            if (flight.BunkType != null && flight.BunkType.Value == BunkType.Free)
            {
                _patPrices = new List<PriceView> { new PriceView { AirportTax = flight.AirportFee, BunkerAdjustmentFactor = flight.BAF, Fare = 0, Total = flight.AirportFee + flight.BAF } };
            }
            else
            {
                //没有传入pat信息  就虚拟一个高价的pat 
                _patPrices = new List<PriceView> { new PriceView { AirportTax = 100000, BunkerAdjustmentFactor = 100000, Fare = 100000, Total = 300000 } };
            }
            //验证
            CommandService.ValidatePNR(_pnr, _pnr.Passengers.First().Type);
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
            policyFilterCondition.SuitReduce = ReturnStringUtility.hasReduce(voyages, policyFilterCondition.PatPrice);
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
            policyFilterCondition.SuitReduce = ReturnStringUtility.hasReduce(voyages, policyFilterCondition.PatPrice);
            policyFilterCondition.NeedSubsidize = true;
            policyFilterCondition.IsUsePatPrice = false;
            return policyFilterCondition;
        }

    }
}