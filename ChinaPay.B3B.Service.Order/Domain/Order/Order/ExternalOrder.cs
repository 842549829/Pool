using System;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core;

namespace ChinaPay.B3B.Service.Order.Domain
{
    public class ExternalOrder : Order
    {
        public ExternalOrder(decimal id):base(id) {
        }

        private ExternalOrder():base()
        {
        }

        public string ExternalOrderId { get; set; }
        public decimal ECommission { get; set; }
        public PayStatus PayStatus { get; set; }
        public DateTime? PayTime { get; set; }
        public string PayTradNO { get; set; }
        public PlatformType Platform {get;set;}
        public bool IsAutoPay { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal? Amount
        {
            get;
            set;
        }
        public override bool IsB3BOrder
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 支付失败信息
        /// </summary>
        public string FaildInfo { get; set; }

        internal static ExternalOrder NewExternalOrder(OrderView orderView, PolicyMatch.MatchedPolicy matchedPolicy, DataTransferObject.Organization.EmployeeDetailInfo employee,AuthenticationChoise choise = AuthenticationChoise.NoNeedAUTH)
        {
            if (!orderView.Flights.Any()) throw new ArgumentNullException("orderView", "缺少航段信息");
            if (!orderView.Passengers.Any()) throw new ArgumentNullException("orderView", "缺少乘机人信息");
            if (matchedPolicy == null) throw new CustomException("无相关政策信息");

            #region  构造BASE
            var result = new ExternalOrder
                {
                    Contact = orderView.Contact,
                    ReservationPNR = PNRPair.IsNullOrEmpty(orderView.PNR) ? null : orderView.PNR,
                    IsReduce = orderView.IsReduce,
                    IsTeam = orderView.IsTeam,
                    AssociateOrderId = orderView.AssociateOrderId,
                    AssociatePNR = orderView.AssociatePNR,
                    Source = orderView.Source,
                    Choise = choise,
                    CustomNo = matchedPolicy.OriginalPolicy == null ? string.Empty : matchedPolicy.OriginalPolicy.CustomCode,
                    VisibleRole = OrderRole.Platform | OrderRole.Purchaser,
                    ForbidChangPNR = false,
                    NeedAUTH = matchedPolicy.OriginalPolicy == null ? matchedPolicy.NeedAUTH : matchedPolicy.OriginalPolicy.NeedAUTH
                };
            var deduction = Deduction.GetDeduction(matchedPolicy);
            var product = ProductInfo.GetProductInfo(matchedPolicy);
            var specialProduct = product as SpeicalProductInfo;
            if (specialProduct != null && !hasETDZPermission(matchedPolicy.Provider, specialProduct))
            {
                result.Supplier = getSupplierInfo(matchedPolicy, specialProduct);
            }
            else
            {
                result.Provider = getProvider(matchedPolicy, product);
            }
            result.IsCustomerResource = ProductInfo.IsCustomerResource(matchedPolicy);
            result.IsStandby = ProductInfo.IsStandby(matchedPolicy);
            result.Purchaser = getPurchaserInfo(employee, deduction);
            var pnrInfo = PNRInfo.GetPNRInfo(orderView, matchedPolicy);
            result.AddPNRInfo(pnrInfo);
            result.TripType = pnrInfo.TripType;
            result.Status = result.RequireConfirm ? OrderStatus.Applied : OrderStatus.Ordered;
            if (result.Status == OrderStatus.Applied)
            {
                result.VisibleRole |= result.IsThirdRelation ? OrderRole.Supplier : OrderRole.Provider;
            }
            #endregion
            return result;
        }
    }
}