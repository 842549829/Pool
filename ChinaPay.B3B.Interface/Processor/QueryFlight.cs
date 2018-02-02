using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Interface.Cache;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.FlightQuery.Domain;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Interface.Processor
{
    class QueryFlight : RequestProcessor
    {
        protected override string ExecuteCore()
        {
            var batchNo = Context.GetParameterValue("batchNo");
            var airlineCode = Context.GetParameterValue("airlineCode");
            var flightNo = Context.GetParameterValue("flightNo");
            Guid id = Guid.Empty;
            id = Vaild(batchNo, airlineCode, flightNo, id);

            var context = ContextCenter.Instance[batchNo];
            if (context == null)
                InterfaceInvokeException.ThrowCustomMsgException("查询超时，请重新查询航班");
            var matchedFlights = context[Employee.Id.ToString()] as List<MatchedFlight>;

            StringBuilder str = new StringBuilder();
            str.Append("<bunks>");
            foreach (var item in matchedFlights)
            {
                var f = item.OriginalFlight;
                if (f.Airline.ToUpper() == airlineCode.ToUpper() && f.FlightNo == flightNo)
                {
                    var matchedBunks = Service.PolicyMatch.PolicyMatchServcie.MatchOneWayFlight(f, Company.CompanyId).ToList();
                    var basicPrice = Service.FoundationService.QueryBasicPrice(f.Airline, f.Departure.Code, f.Arrival.Code, f.FlightDate);
                    var s = (from matchedBunk in matchedBunks
                             where matchedBunk.Policies.Any()
                             from bunkInfo in constructBunkView(matchedBunk, basicPrice)
                             orderby bunkInfo.Amount
                             select bunkInfo);
                    foreach (var b in s)
                    {
                        str.Append("<bunk>");
                        str.AppendFormat("<code>{0}</code>", b.Code);
                        str.AppendFormat("<count>{0}</count>", b.SeatCount > 9 ? "A" : b.SeatCount.ToString());
                        str.AppendFormat("<type>{0}</type>", b.BunkType == null ? "" : ((byte)b.BunkType).ToString());
                        str.AppendFormat("<name>{0}</name>", b.Description);
                        str.AppendFormat("<discount>{0}</discount>", b.Discount);
                        str.AppendFormat("<ei>{0}</ei>", b.EI);
                        str.AppendFormat("<fare>{0}</fare>", b.Fare);
                        str.AppendFormat("<amount>{0}</amount>", b.Amount);
                        str.Append("</bunk>");
                    }

                    //    }
                }
            }
            str.Append("</bunks>");
            return str.ToString();
        }
        private IEnumerable<BunkInfo> constructBunkView(MatchedBunk matchedBunk, ChinaPay.B3B.Service.Foundation.Domain.BasicPrice basicPrice)
        {
            return from policy in matchedBunk.Policies
                   where policy != null
                   select constructBunkView(matchedBunk.OriginalBunk, policy, basicPrice);
        }
        private BunkInfo constructBunkView(Bunk bunk, MatchedPolicy policy, ChinaPay.B3B.Service.Foundation.Domain.BasicPrice price)
        {
            var result = new BunkInfo()
            {
                Policy = new ChinaPay.B3B.DataTransferObject.FlightQuery.PolicyView()
                {
                    Id = policy.OriginalPolicy == null ? Guid.Empty : policy.OriginalPolicy.Id,
                    Owner = policy.Provider,
                    Type = policy.PolicyType,
                    CustomerResource = false
                }
            };
            result.ShowPrice = policy.ParValue != 0;
            if (policy.PolicyType == PolicyType.Special)
            {
                // 特殊票是单独处理的
                var specialPolicy = policy.OriginalPolicy as DataTransferObject.Policy.SpecialPolicyInfo;
                result.Code = bunk == null ? string.Empty : bunk.Code;
                result.SeatCount = bunk == null ? specialPolicy.ResourceAmount : bunk.SeatCount; // 剩余位置数 从政策上获取
                result.Fare = policy.ParValue.TrimInvaidZero(); // 票面价从政策上取
                result.Rebate = string.Empty; // 无返点
                result.Amount = policy.SettleAmount;
                result.Description = "特殊票";
                result.BunkType = bunk == null ? new BunkType?() : bunk.Type;
                switch (specialPolicy.Type)
                {
                    case SpecialProductType.Singleness:
                    case SpecialProductType.Disperse:
                        result.Policy.CustomerResource = true;
                        break;
                    case SpecialProductType.CostFree:
                        result.Policy.CustomerResource = !specialPolicy.SynBlackScreen;
                        result.ShowPrice = true;
                        break;
                }
            }
            else
            {
                result.Code = bunk.Code;
                result.SeatCount = bunk.SeatCount;
                result.Fare = policy.ParValue.TrimInvaidZero();
                result.Rebate = policy.Commission.TrimInvaidZero();
                result.Amount = policy.SettleAmount;

                if (policy.PolicyType == PolicyType.Bargain && bunk is Service.FlightQuery.Domain.GeneralBunk)
                {
                    result.Description = "特价票";
                }
                else
                {
                    if (bunk is Service.FlightQuery.Domain.FirstOrBusinessBunk)
                    {
                        result.Description = (bunk as Service.FlightQuery.Domain.FirstOrBusinessBunk).Description;
                    }
                    else if (bunk is Service.FlightQuery.Domain.EconomicBunk)
                    {
                        result.Description = "经济舱";
                    }
                    else if (bunk is Service.FlightQuery.Domain.PromotionBunk)
                    {
                        result.Description = (bunk as Service.FlightQuery.Domain.PromotionBunk).Description;
                    }
                    else if (bunk is Service.FlightQuery.Domain.ProductionBunk)
                    {
                        result.Description = "往返产品";
                    }
                    else
                    {
                        result.Description = string.Empty;
                    }
                }
                result.BunkType = bunk.Type;
            }
            if (bunk != null && bunk is Service.FlightQuery.Domain.GeneralBunk)
            {
                result.Discount = ((bunk as Service.FlightQuery.Domain.GeneralBunk).Discount).TrimInvaidZero();
                if (policy.PolicyType == PolicyType.Special)
                {
                    result.RenderDiscount = price != null && policy.ParValue != 0 ? Math.Round(policy.ParValue / price.Price, 2).ToString() : string.Empty;
                }
                else
                {
                    result.RenderDiscount = ((bunk as Service.FlightQuery.Domain.GeneralBunk).Discount).TrimInvaidZero();
                }
            }
            else
            {
                result.RenderDiscount = result.Discount = string.Empty;
            }
            // 退改签规定
            // 普通政策时，获取基础数据中普通舱位的退改签信息
            // 其他情况，获取政策上的退改签信息
            if ((policy.PolicyType == PolicyType.Normal || policy.PolicyType == PolicyType.NormalDefault || policy.PolicyType == PolicyType.OwnerDefault) && bunk is Service.FlightQuery.Domain.GeneralBunk)
            {
                result.EI = GetGeneralBunkRegulation(bunk);
            }
            else
            {
                if (policy.OriginalPolicy is DataTransferObject.Policy.IHasRegulation)
                {
                    var regulation = policy.OriginalPolicy as DataTransferObject.Policy.IHasRegulation;
                    result.EI = GetRegulation(regulation);
                }
                else
                {
                    result.EI = string.Empty;
                }
            }
            result.SuportChild = bunk != null && bunk.SuportChild;

            return result;
        }

        public static string GetGeneralBunkRegulation(Bunk bunk)
        {
            var pattern = new Regex("^[a-zA-Z\\d/]+$");
            var refundDetail = ChinaPay.B3B.Service.FoundationService.QueryDetailList(bunk.Owner.Airline, bunk.Code).Where(item => pattern.IsMatch(item.Bunks));
            StringBuilder result = new StringBuilder();
            string refundRegulation = string.Empty;
            string changeRegulation = string.Empty;
            string endorseRegulation = string.Empty;
            foreach (var item in refundDetail)
            {
                refundRegulation += ("航班起飞前：" + item.ScrapBefore + "；航班起飞后：" + item.ScrapAfter).Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                changeRegulation += ("航班起飞前：" + item.ChangeBefore + "；航班起飞后：" + item.ChangeAfter).Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                endorseRegulation += item.Endorse.Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
            }
            if (string.IsNullOrWhiteSpace(refundRegulation))
                refundRegulation = "以航司具体规定为准";
            if (string.IsNullOrWhiteSpace(changeRegulation))
                changeRegulation = "以航司具体规定为准";
            result.AppendFormat("退票规定：{0} ", refundRegulation);
            result.AppendFormat("更改规定：{0} ", changeRegulation);
            result.AppendFormat("签转规定：{0} ", endorseRegulation);
            return result.ToString();
        }

        public static string GetRegulation(ChinaPay.B3B.DataTransferObject.Policy.IHasRegulation regulation)
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("更改规定：{0} ", regulation.ChangeRegulation);
            result.AppendFormat("作废规定：{0} ", regulation.InvalidRegulation);
            result.AppendFormat("退票规定：{0} ", regulation.RefundRegulation);
            result.AppendFormat("签转规定：{0} ", regulation.EndorseRegulation);
            return result.ToString();
        }

        private static Guid Vaild(string batchNo, string airlineCode, string flightNo, Guid id)
        {
            if (string.IsNullOrWhiteSpace(batchNo)) InterfaceInvokeException.ThrowParameterMissException("batchNo");
            if (string.IsNullOrWhiteSpace(airlineCode)) InterfaceInvokeException.ThrowParameterMissException("airlineCode");
            if (string.IsNullOrWhiteSpace(flightNo)) InterfaceInvokeException.ThrowParameterMissException("flightNo");
            if (!Guid.TryParse(batchNo, out id)) InterfaceInvokeException.ThrowParameterErrorException("batchNo");
            if (!Regex.IsMatch(airlineCode, "(\\w{2})")) InterfaceInvokeException.ThrowParameterErrorException("airlineCode");
            if (flightNo.Length >= 6) InterfaceInvokeException.ThrowParameterErrorException("flightNo");
            return id;
        }

    }

    class BunkInfo
    {
        public ChinaPay.B3B.DataTransferObject.FlightQuery.PolicyView Policy { get; set; }
        public string Code { get; set; }
        public int SeatCount { get; set; }
        public Common.Enums.BunkType? BunkType { get; set; }
        public string EI { get; set; }
        public string Description { get; set; }
        public string Discount { get; set; }
        public string RenderDiscount { get; set; }
        public string Fare { get; set; }
        public string Rebate { get; set; }
        public decimal Amount { get; set; }
        public bool ShowPrice { get; set; }
        public bool SuportChild { get; set; }
    }
}