using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.Order.Domain.Bunk {
    /// <summary>
    /// 舱位
    /// </summary>
    public abstract class BaseBunk {
        protected BaseBunk(string code, string ei) {
            this.Code = code;
            this.EI = ei ?? string.Empty;
        }
        /// <summary>
        /// 代码
        /// </summary>
        public string Code {
            get;
            private set;
        }
        /// <summary>
        /// 舱位EI项描述信息
        /// </summary>
        public string EI {
            get;
            private set;
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public abstract decimal Discount { get; }
        /// <summary>
        /// 价格
        /// </summary>
        public abstract decimal Fare { get; }
        /// <summary>
        /// 舱位类型
        /// </summary>
        public abstract Common.Enums.BunkType Type { get; }
        /// <summary>
        /// 调整价格
        /// </summary>
        internal abstract void ReviseFare(decimal fare);

        private decimal _ybPrice;
        internal void SetYBPrice(decimal ybPrice) {
            _ybPrice = ybPrice;
        }
        internal void ModifyCode(string code, string ei) {
            this.Code = code;
            this.EI = ei;
        }

        protected decimal GetFareByDiscount(decimal discount) {
            return FoundationService.CalculateFare(_ybPrice, discount);
        }
        protected decimal GetDiscountByFare(decimal fare) {
            return FoundationService.CalculateDiscount(_ybPrice, fare);
        }

        internal static BaseBunk CreateBunk(string airline, string bunkCode, Common.Enums.BunkType bunkType, string departure, string arrival, DateTime flightDate,
                    PassengerType passengerType, PolicyMatch.MatchedPolicy policy, decimal ybPrice, decimal? patFare) {
            if(string.IsNullOrWhiteSpace(bunkCode) && policy.PolicyType != PolicyType.Special) throw new CustomException("缺少舱位代码");
            if(passengerType == PassengerType.Child) {
                return CreateChildrenBunk(airline, bunkCode, departure, arrival, flightDate, policy, ybPrice, patFare);
            } else {
                var bunks = FoundationService.QueryBunk(airline, departure, arrival, flightDate, bunkCode);
                //更改EI项
                var pattern = new Regex("^[a-zA-Z\\d/]+$");
                var details = FoundationService.QueryDetailList(airline, bunkCode).Where(item => pattern.IsMatch(item.Bunks));
                string refundRegulation = string.Empty;
                string changeRegulation = string.Empty;
                string endorseRegulation = string.Empty;
                foreach(var item in details) {
                    refundRegulation += ("航班起飞前：" + item.ScrapBefore+ "；航班起飞后：" + item.ScrapAfter).Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                    changeRegulation += ("航班起飞前：" + item.ChangeBefore+ "；航班起飞后：" + item.ChangeAfter).Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                    endorseRegulation += item.Endorse.Replace("<br/>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
                }
                if(string.IsNullOrWhiteSpace(refundRegulation))
                    refundRegulation = "以航司具体规定为准";
                if(string.IsNullOrWhiteSpace(changeRegulation))
                    changeRegulation = "以航司具体规定为准";
                foreach(var item in bunks) {
                    item.ChangeRegulation = changeRegulation;
                    item.EndorseRegulation = endorseRegulation;
                    item.RefundRegulation = refundRegulation;
                }

                if(policy.PolicyType == PolicyType.Special) {
                    return CreateSpecialBunk(bunks, bunkCode, bunkType, policy.OriginalPolicy as DataTransferObject.Policy.SpecialPolicyInfo, ybPrice, policy.RelationType);
                } else if(policy.PolicyType == PolicyType.Normal) {
                    return CreateGeneralBunk(bunks, bunkCode);
                } else if(policy.PolicyType == PolicyType.Bargain) {
                    var promotionPolicy = policy.OriginalPolicy as DataTransferObject.Policy.BargainPolicyInfo;
                    if(bunkType == Common.Enums.BunkType.Production) {
                        var fare = promotionPolicy.Price > 0 ? promotionPolicy.Price / 2 : (patFare.HasValue ? patFare.Value : 0);
                        return CreateProductionBunk(bunks, bunkCode, promotionPolicy, fare);
                    } else if(bunkType == Common.Enums.BunkType.Transfer) {
                        if(!patFare.HasValue) throw new CustomException("缺少价格信息");
                        return CreateTransferBunk(bunks, bunkCode, patFare.Value);
                    } else {
                        return CreatePromotionBunk(bunks, bunkCode, promotionPolicy, patFare);
                    }
                } else if(policy.PolicyType == PolicyType.Team) {
                    switch(bunkType) {
                        case Common.Enums.BunkType.Economic:
                        case Common.Enums.BunkType.FirstOrBusiness:
                            return CreateGeneralBunk(bunks, bunkCode);
                        case Common.Enums.BunkType.Team:
                            if(!patFare.HasValue) throw new CustomException("缺少价格信息");
                            return CreateTeamBunk(bunks, bunkCode, patFare.Value);
                        case Common.Enums.BunkType.Promotion:
                            if(!patFare.HasValue) throw new CustomException("缺少价格信息");
                            return CreatePromotionBunk(bunks, bunkCode, null, patFare);
                        case Common.Enums.BunkType.Production:
                            if(!patFare.HasValue) throw new CustomException("缺少价格信息");
                            return CreateProductionBunk(bunks, bunkCode, null, patFare.Value);
                        case Common.Enums.BunkType.Transfer:
                            if(!patFare.HasValue) throw new CustomException("缺少价格信息");
                            return CreateTransferBunk(bunks, bunkCode, patFare.Value);
                        default:
                            throw new CustomException("团队政策仅支持普通、特价、联程和团队舱位");
                    }
                } else if(policy.PolicyType == PolicyType.NormalDefault) {
                    switch(bunkType) {
                        case Common.Enums.BunkType.Economic:
                        case Common.Enums.BunkType.FirstOrBusiness:
                            return CreateGeneralBunk(bunks, bunkCode);
                        case Common.Enums.BunkType.Team:
                            if(!patFare.HasValue) throw new CustomException("缺少价格信息");
                            return CreateTeamBunk(bunks, bunkCode, patFare.Value);
                        default:
                            throw new CustomException("普通默认政策仅支持普通舱位、团队舱位");
                    }
                } else if(policy.PolicyType == PolicyType.BargainDefault) {
                    if(!patFare.HasValue) throw new CustomException("缺少价格信息");
                    switch(bunkType) {
                        case Common.Enums.BunkType.Economic:
                        case Common.Enums.BunkType.FirstOrBusiness:
                            return CreateGeneralBunk(bunks, bunkCode);
                        case Common.Enums.BunkType.Promotion:
                            return CreatePromotionBunk(bunks, bunkCode, null, patFare);
                        case Common.Enums.BunkType.Production:
                            return CreateProductionBunk(bunks, bunkCode, null, patFare.Value);
                        case Common.Enums.BunkType.Transfer:
                            return CreateTransferBunk(bunks, bunkCode, patFare.Value);
                        default:
                            throw new CustomException("特价默认政策仅支持特价舱位、往返产品舱和中转或多段联程舱");
                    }
                } else if(policy.PolicyType == PolicyType.OwnerDefault) {
                    switch(bunkType) {
                        case Common.Enums.BunkType.Economic:
                        case Common.Enums.BunkType.FirstOrBusiness:
                            return CreateGeneralBunk(bunks, bunkCode);
                        case Common.Enums.BunkType.Promotion:
                            if(!patFare.HasValue) throw new CustomException("缺少价格信息");
                            return CreatePromotionBunk(bunks, bunkCode, null, patFare);
                        case Common.Enums.BunkType.Production:
                            if(!patFare.HasValue) throw new CustomException("缺少价格信息");
                            return CreateProductionBunk(bunks, bunkCode, null, patFare.Value);
                        case Common.Enums.BunkType.Transfer:
                            if(!patFare.HasValue) throw new CustomException("缺少价格信息");
                            return CreateTransferBunk(bunks, bunkCode, patFare.Value);
                        case Common.Enums.BunkType.Team:
                            if(!patFare.HasValue) throw new CustomException("缺少价格信息");
                            return CreateTeamBunk(bunks, bunkCode, patFare.Value);
                        default:
                            throw new CustomException("默认政策仅支持普通舱位、团队舱位、特价舱位、往返产品舱和中转或多段联程舱");
                    }
                }
                throw new NotSupportedException("未知政策类型");
            }
        }
        private static BaseBunk CreateChildrenBunk(string airline, string bunkCode, string departure, string arrival, DateTime flightDate, PolicyMatch.MatchedPolicy policy, decimal ybPrice, decimal? patPrice) {
            if(policy.PolicyType == PolicyType.Normal || policy.PolicyType == PolicyType.NormalDefault) {
                var childBunk = FoundationService.QueryBunk(airline, departure, arrival, flightDate, bunkCode).FirstOrDefault();
                if(childBunk is Foundation.Domain.GeneralBunk) {
                    var adultDiscount = (childBunk as Foundation.Domain.GeneralBunk).Discount;
                    decimal fare = 0M;
                    if(adultDiscount >= 1) {
                        var adultFare = Utility.Calculator.Round(ybPrice * adultDiscount, 1);
                        fare = Utility.Calculator.Round(adultFare / 2, 1);
                    } else {
                        fare = Utility.Calculator.Round(ybPrice / 2, 1);
                    }
                    var discount = Utility.Calculator.Round(fare / ybPrice, -3);
                    return new EconomicBunk(bunkCode, discount, fare, childBunk.EI);
                } else if(childBunk is Foundation.Domain.PromotionBunk) {
                    if(patPrice == null) throw new CustomException("缺少儿童价格信息");
                    var promotionBunk = childBunk as Foundation.Domain.PromotionBunk;
                    return new PromotionBunk(bunkCode, patPrice.Value, PriceType.Price, promotionBunk.EI, promotionBunk.Description);
                } else if(childBunk is Foundation.Domain.ProductionBunk) {
                    if(patPrice == null) throw new CustomException("缺少儿童价格信息");
                    return new TransferBunk(bunkCode, patPrice.Value, childBunk.EI);
                } else if(childBunk is Foundation.Domain.TeamBunk) {
                    if(patPrice == null) throw new CustomException("缺少儿童价格信息");
                    return new TeamBunk(bunkCode, patPrice.Value, PriceType.Price, childBunk.EI, string.Empty);
                } else if(childBunk is Foundation.Domain.TransferBunk) {
                    if(patPrice == null) throw new CustomException("缺少儿童价格信息");
                    return new TransferBunk(bunkCode, patPrice.Value, childBunk.EI);
                } else if(childBunk is Foundation.Domain.FreeBunk) {
                    if(patPrice == null) throw new CustomException("缺少儿童价格信息");
                    var freeBunk = childBunk as Foundation.Domain.FreeBunk;
                    return new FreeBunk(bunkCode, patPrice.Value, freeBunk.EI, freeBunk.Description);
                }
                throw new CustomException("当前舱位不支持儿童");
            }
            throw new CustomException("儿童票只能匹配普通政策");
        }
        private static GeneralBunk CreateGeneralBunk(IEnumerable<Foundation.Domain.Bunk> bunks, string bunkCode) {
            var bunk = bunks.FirstOrDefault(item => item is Foundation.Domain.GeneralBunk);
            if(bunk == null) throw new CustomException("未找到相应的明折明扣舱");
            var generalBunk = bunk as Foundation.Domain.GeneralBunk;
            var discount = generalBunk.GetDiscount(bunkCode);
            if(bunk is Foundation.Domain.FirstBusinessBunk) {
                var firstBusinessBunk = bunk as Foundation.Domain.FirstBusinessBunk;
                return new FirstOrBusinessBunk(bunkCode, discount, firstBusinessBunk.EI, firstBusinessBunk.Description);
            } else {
                return new EconomicBunk(bunkCode, discount, generalBunk.EI);
            }
        }
        private static ProductionBunk CreateProductionBunk(IEnumerable<Foundation.Domain.Bunk> bunks, string bunkCode, DataTransferObject.Policy.BargainPolicyInfo policy, decimal? patFare) {
            var bunk = bunks.FirstOrDefault(item => item is Foundation.Domain.ProductionBunk);
            if(bunk == null) throw new CustomException("未找到相应的往返产品舱");
            decimal fare = 0;
            if(policy == null || policy.Price < 0) {
                if(patFare.HasValue) {
                    fare = patFare.Value;
                } else {
                    throw new CustomException("缺少价格信息");
                }
            } else {
                fare = Utility.Calculator.Round(policy.Price, 1) / 2;
            }
            return new ProductionBunk(bunkCode, fare, bunk.EI);
        }
        private static PromotionBunk CreatePromotionBunk(IEnumerable<Foundation.Domain.Bunk> bunks, string bunkCode, DataTransferObject.Policy.BargainPolicyInfo policy, decimal? patFare) {
            var ei = string.Empty;
            var description = string.Empty;
            var bunk = bunks.FirstOrDefault(item => item is Foundation.Domain.PromotionBunk);
            if(bunk != null) {
                var promotionBunk = bunk as Foundation.Domain.PromotionBunk;
                ei = promotionBunk.EI;
                description = promotionBunk.Description;
            }
            var priceOrDiscount = 0M;
            var priceType = PriceType.Price;
            if(policy == null || policy.PriceType == PriceType.Commission || policy.VoyageType == VoyageType.TransitWay) {
                if(patFare == null) throw new CustomException("缺少PAT价格");
                priceOrDiscount = patFare.Value;
            } else if(policy.VoyageType == VoyageType.RoundTrip && policy.PriceType == PriceType.Price) {
                priceOrDiscount = policy.Price / 2;
                priceType = policy.PriceType;
            } else {
                priceOrDiscount = policy.Price;
                priceType = policy.PriceType;
            }
            return new PromotionBunk(bunkCode, priceOrDiscount, priceType, ei, description);
        }
        private static TransferBunk CreateTransferBunk(IEnumerable<Foundation.Domain.Bunk> bunks, string bunkCode, decimal patFare) {
            var bunk = bunks.FirstOrDefault(b => b is Foundation.Domain.TransferBunk);
            if(bunk == null) throw new CustomException("未找到相应的中转联程舱");
            return new TransferBunk(bunkCode, patFare, bunk.EI);
        }
        private static SpecialBunk CreateSpecialBunk(IEnumerable<Foundation.Domain.Bunk> bunks, string bunkCode, Common.Enums.BunkType bunkType, DataTransferObject.Policy.SpecialPolicyInfo policy, decimal ybPrice, RelationType relation) {
            decimal discount = 0;
            if(policy.Type == SpecialProductType.CostFree) {
                var ei = string.Empty;
                var description = string.Empty;
                var code = string.Empty;
                if(policy.SynBlackScreen) {
                    var freeBunk = bunks.FirstOrDefault(b => b is Foundation.Domain.FreeBunk && b.Code.Value == bunkCode);
                    code = freeBunk == null ? string.Empty : freeBunk.Code.Value;
                    ei = freeBunk == null ? string.Empty : freeBunk.EI;
                    description = freeBunk == null ? string.Empty : (freeBunk as Foundation.Domain.FreeBunk).Description;
                }
                var settleAmount = PolicyMatch.Domain.Calculator.ComputeSpecialSettlementPrice(policy, ybPrice, discount, PolicyMatch.Domain.Calculator.GetDeductionType(relation));
                return new FreeBunk(code, settleAmount, ei, description);
            } else if(policy.Type == SpecialProductType.Business || policy.Type == SpecialProductType.Bloc) {
                if(bunkType == Common.Enums.BunkType.Economic || bunkType == Common.Enums.BunkType.FirstOrBusiness) {
                    var generalBunk = CreateGeneralBunk(bunks, bunkCode);
                    generalBunk.SetYBPrice(ybPrice);
                    var fare = PolicyMatch.Domain.Calculator.ComputeSpecialParValue(policy, ybPrice, generalBunk.Discount, PolicyMatch.Domain.Calculator.GetDeductionType(relation));
                    var releasedFare = PolicyMatch.Domain.Calculator.ComputeSpecialSettlementPrice(policy, ybPrice, generalBunk.Discount, PolicyMatch.Domain.Calculator.GetDeductionType(relation));
                    generalBunk.ReviseFare(fare);
                    return new SpecialBunk(bunkCode, generalBunk.Discount, generalBunk.Fare, releasedFare, generalBunk.EI);
                } else if(bunkType == Common.Enums.BunkType.Promotion) {
                    var fare = PolicyMatch.Domain.Calculator.ComputeSpecialParValue(policy, ybPrice, discount, PolicyMatch.Domain.Calculator.GetDeductionType(relation));
                    var promotionBunk = CreatePromotionBunk(bunks, bunkCode, null, fare);
                    promotionBunk.SetYBPrice(ybPrice);
                    var releasedFare = PolicyMatch.Domain.Calculator.ComputeSpecialSettlementPrice(policy, ybPrice, discount, PolicyMatch.Domain.Calculator.GetDeductionType(relation));
                    return new SpecialBunk(bunkCode, promotionBunk.Discount, promotionBunk.Fare, releasedFare, promotionBunk.EI);
                } else {
                    throw new CustomException("集团票和商旅卡仅支持普通舱和特价舱");
                }
            } else if(policy.Type == SpecialProductType.LowToHigh) {
                var bunk = bunks.First();
                if(bunk.Type == Common.Enums.BunkType.FirstOrBusiness || bunk.Type == Common.Enums.BunkType.Economic) {
                    var generalBunk = CreateGeneralBunk(bunks, bunkCode);
                    generalBunk.SetYBPrice(ybPrice);
                    discount = generalBunk.Discount;
                    return new SpecialBunk(bunk.Code.Value, discount, ybPrice, ybPrice * discount, string.Empty);//TODO 暂不确定的这里的退改签规定是否需要构造
                } else {
                    throw new CustomException("低打高返政策仅支持普通舱位");
                }
            }
            var releasedSettleAmount = PolicyMatch.Domain.Calculator.ComputeSpecialSettlementPrice(policy, ybPrice, discount, PolicyMatch.Domain.Calculator.GetDeductionType(relation));
            return new SpecialBunk(releasedSettleAmount);
        }
        private static TeamBunk CreateTeamBunk(IEnumerable<Foundation.Domain.Bunk> bunks, string bunkCode, decimal patFare) {
            var bunk = bunks.FirstOrDefault(b => b is Foundation.Domain.TeamBunk);
            if(bunk == null) throw new CustomException("未找到相应的团队舱位");
            return new TeamBunk(bunkCode, patFare, PriceType.Price, bunk.EI, string.Empty);
        }
    }
}