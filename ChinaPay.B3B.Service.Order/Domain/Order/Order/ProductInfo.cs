using System;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.Data;
using ChinaPay.B3B.DataTransferObject.Order;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 产品信息
    /// </summary>
    public abstract class ProductInfo {
        LazyLoader<RefundAndReschedulingProvision> _refundAndReschedulingProvisionLoader = null;

        protected ProductInfo() {
            _refundAndReschedulingProvisionLoader = new LazyLoader<RefundAndReschedulingProvision>();
        }
        protected ProductInfo(decimal orderId) {
            _refundAndReschedulingProvisionLoader = new LazyLoader<RefundAndReschedulingProvision>(() => QueryRefundAndReschedulingProvision(orderId));
        }

        /// <summary>
        /// 产品Id
        /// </summary>
        public Guid Id {
            get;
            internal set;
        }
        /// <summary>
        /// 产品类型
        /// </summary>
        public ProductType ProductType {
            get;
            internal set;
        }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType TicketType {
            get;
            internal set;
        }
        /// <summary>
        /// Office号
        /// </summary>
        public string OfficeNo {
            get;
            internal set;
        }
        ///// <summary>
        ///// 是否需要授权
        ///// </summary>
        //public bool NeedAuth
        //{
        //    get;
        //    internal set;
        //}

        /// <summary>
        /// 退改签规定
        /// </summary>
        public RefundAndReschedulingProvision RefundAndReschedulingProvision {
            get {
                return _refundAndReschedulingProvisionLoader.QueryData();
            }
            internal set {
                _refundAndReschedulingProvisionLoader.SetData(value);
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark {
            get;
            internal set;
        }
        /// <summary>
        /// 条件
        /// </summary>
        public string Condition {
            get;
            internal set;
        }

        /// <summary>
        /// 是否是默认政策
        /// </summary>
        public bool IsDefaultPolicy { get; set; }

        protected abstract RefundAndReschedulingProvision QueryRefundAndReschedulingProvision(decimal orderId);

        internal static ProductInfo GetProductInfo(PolicyMatch.MatchedPolicy policy) {
            ProductInfo productInfo = null;
            if(policy.PolicyType == PolicyType.NormalDefault || policy.PolicyType == PolicyType.BargainDefault || policy.PolicyType == PolicyType.OwnerDefault ||
                (policy.PolicyType == PolicyType.Normal && policy.OriginalPolicy == null && policy.OriginalExternalPolicy == null)) {
                return new CommonProductInfo {
                    OfficeNo = policy.OfficeNumber,
                    //NeedAuth = policy.NeedAUTH,
                    ProductType = policy.PolicyType == PolicyType.BargainDefault ? ProductType.Promotion : ProductType.General,
                    RequireChangePNR = false,
                    ETDZMode = ETDZMode.Manual,
                    TicketType = TicketType.BSP,
                    Id = policy.Id,
                    Remark = string.Empty,
                    Condition = string.Empty,
                    IsDefaultPolicy = policy.PolicyType == PolicyType.NormalDefault || policy.PolicyType == PolicyType.BargainDefault || policy.PolicyType == PolicyType.OwnerDefault,
                    RefundAndReschedulingProvision = new RefundAndReschedulingProvision()
                };
            }
            if(policy.PolicyType == PolicyType.Special) {
                var specialPolicy = policy.OriginalPolicy as SpecialPolicyInfo;
                productInfo = new SpeicalProductInfo {
                    RequireConfirm = specialPolicy.ConfirmResource,
                    SpeicalProductType = specialPolicy.Type,
                    ProductType = ProductType.Special,
                    OfficeNo = specialPolicy.OfficeCode,
                    //NeedAuth = policy.NeedAUTH
                };
            } else if(policy.IsExternal) {
                productInfo = new CommonProductInfo {
                    ProductType = GetProductType(policy),
                    RequireChangePNR = policy.OriginalExternalPolicy.RequireChangePNR,
                    ETDZMode = ETDZMode.Manual
                };
            } else {
                var generalPolicy = policy.OriginalPolicy as IGeneralPolicy;
                productInfo = new CommonProductInfo {
                    ProductType = GetProductType(policy),
                    RequireChangePNR = generalPolicy.ChangePNR,
                    ETDZMode = generalPolicy.AutoPrint ? ETDZMode.Auto : ETDZMode.Manual
                };
            }
            if(policy.IsExternal) {
                productInfo.Id = Guid.NewGuid();
                productInfo.OfficeNo = policy.OriginalExternalPolicy.OfficeNo;
                //productInfo.NeedAuth = policy.OriginalExternalPolicy.RequireAuth;
                productInfo.TicketType = policy.OriginalExternalPolicy.TicketType;
                productInfo.Remark = policy.OriginalExternalPolicy.Remark;
                productInfo.Condition = policy.OriginalExternalPolicy.Condition;
            } else {
                productInfo.Id = policy.OriginalPolicy.Id;
                productInfo.OfficeNo = policy.OriginalPolicy.OfficeCode;
                //productInfo.NeedAuth = policy.OriginalPolicy.NeedAUTH;
                productInfo.TicketType = policy.OriginalPolicy.TicketType;
                productInfo.Remark = policy.OriginalPolicy.Remark;
                productInfo.Condition = policy.OriginalPolicy.Condition;
            }
            if(policy.OriginalPolicy is IHasRegulation) {
                var regulation = (IHasRegulation)policy.OriginalPolicy;
                productInfo.RefundAndReschedulingProvision = new RefundAndReschedulingProvision() {
                    Refund = regulation.RefundRegulation,
                    Scrap = regulation.InvalidRegulation,
                    Alteration = regulation.ChangeRegulation,
                    Transfer = regulation.EndorseRegulation
                };
            } else {
                productInfo.RefundAndReschedulingProvision = new RefundAndReschedulingProvision();
            }
            return productInfo;
        }
        private static ProductType GetProductType(PolicyMatch.MatchedPolicy policy) {
            switch(policy.PolicyType) {
                case PolicyType.Normal:
                    return ProductType.General;
                case PolicyType.Bargain:
                    return ProductType.Promotion;
                case PolicyType.Special:
                    return ProductType.Special;
                case PolicyType.Team:
                    return ProductType.Team;
                    case PolicyType.Notch:
                    return ProductType.Notch;
                default:
                    throw new NotSupportedException(policy.PolicyType.ToString());
            }
        }
        internal static bool IsCustomerResource(PolicyMatch.MatchedPolicy policy) {
            if(policy.PolicyType == PolicyType.Special) {
                var specialPolicy = policy.OriginalPolicy as SpecialPolicyInfo;
                switch(specialPolicy.Type) {
                    case SpecialProductType.CostFree:
                        return !specialPolicy.SynBlackScreen;
                    case SpecialProductType.Bloc:
                    case SpecialProductType.Business:
                    case SpecialProductType.OtherSpecial:
                    case SpecialProductType.LowToHigh:
                        return false;
                    default:
                        return true;
                }
            }
            return false;
        }
        internal static bool IsStandby(PolicyMatch.MatchedPolicy policy) {
            if(policy.PolicyType == PolicyType.Special) {
                var specialPolicy = policy.OriginalPolicy as SpecialPolicyInfo;
                return specialPolicy.Type == SpecialProductType.CostFree && !specialPolicy.IsSeat;
            }
            return false;
        }
    }
}