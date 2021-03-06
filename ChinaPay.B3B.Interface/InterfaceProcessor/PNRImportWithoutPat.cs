﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.DataTransferObject.Command.PNR;
using ChinaPay.B3B.Service.Command.Domain.PNR;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Interface.PublicClass;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Interface.Cache;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.PolicyMatch;
using System.Text;
using ChinaPay.Core.Extension;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Interface.InterfaceProcessor
{
    /// <summary>
    /// 导入内容不需要pat信息
    /// </summary>
    class PNRImportWithoutPat : BaseProcessor
    {
        //存储导入变量（不转换）
        private string _pnrContent;
        private string _originalPNRContent;
        private ReservedPnr _pnr;
        private List<PriceView> _patPrices;
        private List<DataTransferObject.FlightQuery.FlightView> _flights;
        private PolicyType _policyType = PolicyType.Bargain;
        public PNRImportWithoutPat(string pnrContext, string userName, string sign)
            : base(userName, sign)
        {
            _originalPNRContent = pnrContext;
            _pnrContent = HttpUtility.UrlDecode(pnrContext);
        }

        protected override System.Collections.Specialized.NameValueCollection GetBusinessParameterCollection()
        {
            var collection = new System.Collections.Specialized.NameValueCollection();
            collection.Add("pnrContext", _originalPNRContent);
            return collection;
        }

        protected override void ValidateBusinessParameters()
        {
            if (string.IsNullOrWhiteSpace(_pnrContent)) throw new InterfaceInvokeException("8", "编码内容");
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
            if (DataTransferObject.Common.PNRPair.IsNullOrEmpty(_pnr.PnrPair)) throw new InterfaceInvokeException("1", "内容中缺少编码");
            if (string.IsNullOrWhiteSpace(_pnr.PnrPair.PNR) && string.IsNullOrWhiteSpace(_pnr.PnrPair.BPNR)) throw new InterfaceInvokeException("1", "编码信息不全");
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
            _policyType = queryPolicyType(_flights);
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

        protected override string ExecuteCore()
        {
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
            if (!matchedPolicies.Any())
                throw new InterfaceInvokeException("9", "没有找到相关政策");
            StringBuilder str = new StringBuilder();
            str.Append("<policies>");
            PNRImport.GetPolicy(matchedPolicies, matchedSpeciafPolicies, str, _policyType, _flights,InterfaceSetting);
            #region
            //if ((_policyType & PolicyType.Special) != PolicyType.Special)
            //{
            //    var list = from item in matchedPolicies
            //               let generalPolicy = item.OriginalPolicy as IGeneralPolicy
            //               let regulation = item.OriginalPolicy as IHasRegulation
            //               select new
            //               {
            //                   Id = item.Id,
            //                   Type = item.PolicyType == PolicyType.BargainDefault ? (int)PolicyType.Bargain : item.PolicyType == PolicyType.NormalDefault ? (int)PolicyType.Normal : (int)item.PolicyType,
            //                   Fare = item.ParValue.TrimInvaidZero(),
            //                   Rebate = (item.Commission * 100).TrimInvaidZero(),
            //                   Commission = (item.ParValue - item.SettleAmount).TrimInvaidZero(),
            //                   Amount = item.SettleAmount,
            //                   Ticket = (int)(item.OriginalPolicy == null ? TicketType.BSP : item.OriginalPolicy.TicketType),
            //                   EI = ReplaceEnter((regulation == null ? getEI(_flights) : getProvision(regulation))),
            //                   OfficeNo = item.OriginalPolicy == null && item.NeedAUTH ? item.OfficeNumber : (item.OriginalPolicy != null && item.OriginalPolicy.NeedAUTH ? item.OriginalPolicy.OfficeCode : ""),
            //                   Condition = item.OriginalPolicy == null ? "" : item.OriginalPolicy.Condition,
            //                   Confirm = item.ConfirmResource,
            //                   ChangePNR = generalPolicy == null ? false : generalPolicy.ChangePNR,
            //                   EtdzTime = getTimeRange(item.WorkStart, item.WorkEnd),
            //                   RefundTime = getTimeRange(item.WorkStart, item.WorkEnd),
            //                   ScrapTime = getTimeRange(item.RefundStart, item.RefundEnd),
            //                   EtdzSpeed = item.Speed == null ? 0 : item.Speed.ETDZ / 60,
            //                   RefundSpeed = item.Speed == null ? 0 : item.Speed.Refund / 60
            //               };
            //    foreach (var item in list)
            //    {
            //        str.Append("<policy>");
            //        str.AppendFormat("<id>{0}</id>", item.Id);
            //        str.AppendFormat("<type>{0}</type>", item.Type);
            //        str.AppendFormat("<fare>{0}</fare>", item.Fare);
            //        str.AppendFormat("<rebate>{0}</rebate>", item.Rebate);
            //        str.AppendFormat("<commission>{0}</commission>", item.Commission);
            //        str.AppendFormat("<amount>{0}</amount>", item.Amount);
            //        str.AppendFormat("<ticket>{0}</ticket>", item.Ticket);
            //        str.AppendFormat("<ei>{0}</ei>", item.EI);
            //        //需要授权的office号才有，否则为空字符
            //        str.AppendFormat("<officeNo>{0}</officeNo>", item.OfficeNo);
            //        str.AppendFormat("<condition>{0}</condition>", item.Condition);
            //        str.AppendFormat("<confirm>{0}</confirm>", item.Confirm);
            //        str.AppendFormat("<changePNR>{0}</changePNR>", item.ChangePNR ? 1 : 0);
            //        str.AppendFormat("<etdzTime>{0}</etdzTime>", item.EtdzTime);
            //        str.AppendFormat("<refundTime>{0}</refundTime>", item.RefundTime);
            //        str.AppendFormat("<scrapTime>{0}</scrapTime>", item.ScrapTime);
            //        str.AppendFormat("<etdzSpeed>{0}</etdzSpeed>", item.EtdzSpeed);
            //        str.Append("</policy>");
            //    }
            //    if ((_policyType & PolicyType.Team) != PolicyType.Team)
            //    {
            //        var queryList = from item in matchedSpeciafPolicies
            //                        where item != null && item.OriginalPolicy != null
            //                        let specialPolicy = item.OriginalPolicy as SpecialPolicyInfo
            //                        let specialPolicyInfo = SpecialProductService.Query(specialPolicy.Type)
            //                        select new
            //                        {
            //                            Id = item.Id,
            //                            Type = (int)item.PolicyType,
            //                            Fare = item.ParValue.TrimInvaidZero(),
            //                            Rebate = (item.Commission * 100).TrimInvaidZero(),
            //                            Commission = (item.ParValue - item.SettleAmount).TrimInvaidZero(),
            //                            Amount = item.SettleAmount,
            //                            Ticket = (int)(item.OriginalPolicy == null ? TicketType.BSP : item.OriginalPolicy.TicketType),
            //                            EI = ReplaceEnter(getProvision(item.OriginalPolicy as IHasRegulation)),
            //                            OfficeNo = item.OriginalPolicy == null && item.NeedAUTH ? item.OfficeNumber : (item.OriginalPolicy != null && item.OriginalPolicy.NeedAUTH ? item.OriginalPolicy.OfficeCode : ""),
            //                            Condition = item.OriginalPolicy == null ? "" : item.OriginalPolicy.Condition,
            //                            Confirm = item.ConfirmResource,
            //                            ChangePNR = "0",
            //                            EtdzTime = getTimeRange(item.WorkStart, item.WorkEnd),
            //                            RefundTime = getTimeRange(item.WorkStart, item.WorkEnd),
            //                            ScrapTime = getTimeRange(item.RefundStart, item.RefundEnd),
            //                            EtdzSpeed = item.Speed == null ? 0 : item.Speed.ETDZ / 60,
            //                            RefundSpeed = item.Speed == null ? 0 : item.Speed.Refund / 60
            //                        };
            //        foreach (var item in queryList)
            //        {
            //            str.Append("<policy>");
            //            str.AppendFormat("<id>{0}</id>", item.Id);
            //            str.AppendFormat("<type>{0}</type>", item.Type);
            //            str.AppendFormat("<fare>{0}</fare>", item.Fare);
            //            str.AppendFormat("<rebate>{0}</rebate>", item.Rebate);
            //            str.AppendFormat("<commission>{0}</commission>", item.Commission);
            //            str.AppendFormat("<amount>{0}</amount>", item.Amount);
            //            str.AppendFormat("<ticket>{0}</ticket>", item.Ticket);
            //            str.AppendFormat("<ei>{0}</ei>", item.EI);
            //            //需要授权的office号才有，否则为空字符
            //            str.AppendFormat("<officeNo>{0}</officeNo>", item.OfficeNo);
            //            str.AppendFormat("<condition>{0}</condition>", item.Condition);
            //            str.AppendFormat("<confirm>{0}</confirm>", item.Confirm);
            //            str.AppendFormat("<changePNR>{0}</changePNR>", item.ChangePNR);
            //            str.AppendFormat("<etdzTime>{0}</etdzTime>", item.EtdzTime);
            //            str.AppendFormat("<refundTime>{0}</refundTime>", item.RefundTime);
            //            str.AppendFormat("<scrapTime>{0}</scrapTime>", item.ScrapTime);
            //            str.AppendFormat("<etdzSpeed>{0}</etdzSpeed>", item.EtdzSpeed);
            //            str.Append("</policy>");
            //        }
            //        foreach (var item in matchedSpeciafPolicies)
            //        {
            //            if (!matchedPolicies.Contains(item))
            //            {
            //                matchedPolicies.Add(item);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    var queryList = from item in matchedPolicies
            //                    let specialPolicy = item.OriginalPolicy as SpecialPolicyInfo
            //                    let specialPolicyInfo = SpecialProductService.Query(specialPolicy.Type)
            //                    where item != null && item.OriginalPolicy != null && !specialPolicy.ConfirmResource
            //                    select new
            //                    {
            //                        Id = item.Id,
            //                        Type = (int)item.PolicyType,
            //                        Fare = item.ParValue.TrimInvaidZero(),
            //                        Rebate = (item.Commission * 100).TrimInvaidZero(),
            //                        Commission = 0,
            //                        Amount = item.SettleAmount,
            //                        Ticket = (int)(item.OriginalPolicy == null ? TicketType.BSP : item.OriginalPolicy.TicketType),
            //                        EI = ReplaceEnter(getProvision(item.OriginalPolicy as IHasRegulation)),
            //                        OfficeNo = item.OriginalPolicy == null && item.NeedAUTH ? item.OfficeNumber : (item.OriginalPolicy != null && item.OriginalPolicy.NeedAUTH ? item.OriginalPolicy.OfficeCode : ""),
            //                        Condition = item.OriginalPolicy == null ? "" : item.OriginalPolicy.Condition,
            //                        Confirm = item.ConfirmResource,
            //                        ChangePNR = "0",
            //                        EtdzTime = getTimeRange(item.WorkStart, item.WorkEnd),
            //                        RefundTime = getTimeRange(item.WorkStart, item.WorkEnd),
            //                        ScrapTime = getTimeRange(item.RefundStart, item.RefundEnd),
            //                        EtdzSpeed = item.Speed == null ? 0 : item.Speed.ETDZ / 60,
            //                        RefundSpeed = item.Speed == null ? 0 : item.Speed.Refund / 60,
            //                        OrganizaPolicy = item.OriginalPolicy
            //                    };
            //    foreach (var item in queryList)
            //    {
            //        str.Append("<policy>");
            //        str.AppendFormat("<id>{0}</id>", item.Id);
            //        str.AppendFormat("<type>{0}</type>", item.Type);
            //        str.AppendFormat("<fare>{0}</fare>", item.Fare);
            //        str.AppendFormat("<rebate>{0}</rebate>", item.Rebate);
            //        str.AppendFormat("<commission>{0}</commission>", item.Commission);
            //        str.AppendFormat("<amount>{0}</amount>", item.Amount);
            //        str.AppendFormat("<ticket>{0}</ticket>", item.Ticket);
            //        str.AppendFormat("<ei>{0}</ei>", item.EI);
            //        //需要授权的office号才有，否则为空字符
            //        str.AppendFormat("<officeNo>{0}</officeNo>", item.OfficeNo);
            //        str.AppendFormat("<condition>{0}</condition>", item.Condition);
            //        str.AppendFormat("<confirm>{0}</confirm>", item.Confirm);
            //        str.AppendFormat("<changePNR>{0}</changePNR>", item.ChangePNR);
            //        str.AppendFormat("<etdzTime>{0}</etdzTime>", item.EtdzTime);
            //        str.AppendFormat("<refundTime>{0}</refundTime>", item.RefundTime);
            //        str.AppendFormat("<scrapTime>{0}</scrapTime>", item.ScrapTime);
            //        str.AppendFormat("<etdzSpeed>{0}</etdzSpeed>", item.EtdzSpeed);
            //        str.Append("</policy>");
            //    }
            //}
            #endregion
            str.Append("</policies>");
            //将匹配出来的政策存入缓存中 
            CustomContext context = CustomContext.NewContext();
            context[_pnr.PnrPair.BPNR + _pnr.PnrPair.PNR] = matchedPolicies;
            ContextCenter.Instance.Save(context);
            str.AppendFormat("<batchNo>{0}</batchNo>", context.Id + "1");

            return str.ToString();
        }

        #region 私有方法

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
            //policyFilterCondition.SuitReduce = _pnr.Price == null ? false : (_pnr.Price != null && flights.Count() == 2 ? _pnr.Price.Fare < flights.Sum(item => item.Fare) : false);
            //policyFilterCondition.PatPrice = _pnr.Price == null ? (decimal?)null : _pnr.Price.Fare;
            policyFilterCondition.PatPrice = _patPrices.Min(item => item.Fare);
            policyFilterCondition.SuitReduce = ChinaPay.B3B.Interface.Processor.ProduceOrder2.hasReduce(voyages, policyFilterCondition.PatPrice);
            policyFilterCondition.NeedSubsidize = true;
            policyFilterCondition.IsUsePatPrice = false;
            return policyFilterCondition;
        }

        private static AllowTicketType FilterByTime(DateTime takeOffTime)
        {
            var minutesBeforeTakeOff = (takeOffTime - DateTime.Now).TotalMinutes;
            if (minutesBeforeTakeOff <= SystemParamService.FlightDisableTime) return AllowTicketType.None;
            if (minutesBeforeTakeOff < 60) return AllowTicketType.BSP;
            if (minutesBeforeTakeOff < 2 * 60) return AllowTicketType.B2BOnPolicy;
            return AllowTicketType.Both;
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

        private string getTimeRange(Izual.Time start, Izual.Time end)
        {
            return start.ToString("HH:mm") + "-" + end.ToString("HH:mm");
        }
        private static string ReplaceEnter(string input)
        {
            string result = input.Replace("\n", "<br />").Replace("\"", string.Empty).Replace("'", string.Empty);
            return result.Replace("\r", "");
        }

        private object getProvisionList(IHasRegulation regulation)
        {
            if (regulation == null) return new[] { new { key = string.Empty, value = string.Empty } };
            return new[]
            {
                new {key="更改规定" ,value= regulation.ChangeRegulation},
                new {key="作废规定",value= regulation.InvalidRegulation},
                new {key="退票规定" ,value= regulation.RefundRegulation},
                new {key="签转规定" ,value= regulation.EndorseRegulation}
            };
        }

        private string getEI(IEnumerable<DataTransferObject.FlightQuery.FlightView> flights)
        {
            Dictionary<string, string> RenderedEI = new Dictionary<string, string>();
            var htmlReg = new Regex("<.+?>");
            var voyages = flights;
            var flightView = voyages.ElementAt(0);
            string ei = "(" + flightView.BunkCode + ")" + htmlReg.Replace(flightView.EI, string.Empty);
            RenderedEI.Add(flightView.BunkCode, flightView.EI);
            foreach (var item in voyages)
            {
                if (!RenderedEI.Any(b => b.Key == item.BunkCode && b.Value == item.EI))
                {
                    ei += "|" + "(" + item.BunkCode + ")" + htmlReg.Replace(flightView.EI, string.Empty);
                    RenderedEI.Add(item.BunkCode, item.EI);
                }
            }
            return ei;
        }
        private string getProvision(IHasRegulation regulation)
        {
            if (regulation == null) return string.Empty;
            return "更改规定：" + regulation.ChangeRegulation + "|作废规定：" + regulation.InvalidRegulation +
                   "|退票规定：" + regulation.RefundRegulation + "|签转规定：" + regulation.EndorseRegulation;
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
            policyFilterCondition.SuitReduce = ChinaPay.B3B.Interface.Processor.ProduceOrder2.hasReduce(voyages, policyFilterCondition.PatPrice);
            policyFilterCondition.NeedSubsidize = true;
            policyFilterCondition.IsUsePatPrice = false;
            return policyFilterCondition;
        }

        #endregion
    }
}