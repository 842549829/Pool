using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Command;
using ChinaPay.B3B.Service.Command.PNR;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.PolicyMatch;
using ChinaPay.Core;
using ChinaPay.Core.Exception;
using ChinaPay.Core.Extension;
using ChinaPay.Data;
using ChinaPay.B3B.Service.Organization;
using Izual;

namespace ChinaPay.B3B.Service.Order.Domain {
    /// <summary>
    /// 订单
    /// </summary>
    public class Order {
        #region "Fields"
        EnumerableLazyLoader<BaseApplyform> _applyformLoader;
        LazyLoader<Service.Distribution.Domain.OrderBill> _distributionBillLoader;
        LazyLoader<OEMInfo> _OemInfoLoader;
        EnumerableLazyLoader<Log.Domain.OrderLog> _operationLoader;
        LazyLoader<Order> _associateOrderLoader;
        EnumerableLazyLoader<Coordination> _coordinationLoader;
        List<PNRInfo> _pnrInfos;
        decimal? _associateOrderId;
        #endregion

        #region "Constructors"
        protected Order()
            : this(Sequence.SequenceService.GenerateTicketOrderId(), true) {
        }
        internal Order(decimal id)
            : this(id, false) {
        }
        private Order(decimal id, bool newOrder) {
            this.Id = id;
            _pnrInfos = new List<PNRInfo>();
            initLazyLoaders(newOrder);
        }
        void initLazyLoaders(bool newOrder) {
            _OemInfoLoader = new LazyLoader<OEMInfo>(() => OEMService.QueryOEMById(OEMID.Value));
            if(newOrder) {
                _applyformLoader = new EnumerableLazyLoader<BaseApplyform>();
                _operationLoader = new EnumerableLazyLoader<Log.Domain.OrderLog>();
                _coordinationLoader = new EnumerableLazyLoader<Coordination>();
            } else {
                _applyformLoader = new EnumerableLazyLoader<BaseApplyform>(() => {
                    return ApplyformQueryService.QueryApplyforms(this.Id);
                });
                _operationLoader = new EnumerableLazyLoader<Log.Domain.OrderLog>(() => {
                    return LogService.QueryOrderLog(this.Id);
                });
                _coordinationLoader = new EnumerableLazyLoader<Coordination>(() => {
                    return CoordinationService.QueryOrderCoordinations(this.Id);
                });
            }
            _associateOrderLoader = new LazyLoader<Order>();
            _distributionBillLoader = new LazyLoader<Service.Distribution.Domain.OrderBill>(() => {
                return DistributionQueryService.QueryOrderBill(this.Id);
            });
        }
        #endregion

        #region "Properties"
        /// <summary>
        /// 订单号
        /// </summary>
        public decimal Id {
            get; internal set;
        }
        /// <summary>
        /// 关联订单号
        /// 目前用于升舱，记录原订单号
        /// </summary>
        public decimal? AssociateOrderId {
            get {
                return _associateOrderId;
            }
            internal set {
                if(null != value) {
                    _associateOrderLoader = new LazyLoader<Order>(() => {
                        return OrderQueryService.QueryOrder(value.Value);
                    });
                    _associateOrderId = value;
                }
            }
        }
        /// <summary>
        /// 关联订单
        /// </summary>
        public Order AssociateOrder {
            get {
                return _associateOrderLoader.QueryData();
            }
        }
        /// <summary>
        /// 关联编码
        /// 如果是儿童订单，则记的是成人编码
        /// 如果升舱订单，则是原订单中的编码
        /// </summary>
        public PNRPair AssociatePNR { get; internal set; }
        /// <summary>
        /// 订座编码
        /// </summary>
        public PNRPair ReservationPNR {
            get;
            internal set;
        }
        /// <summary>
        /// 出票编码
        /// </summary>
        public PNRPair ETDZPNR {
            get;
            internal set;
        }
        /// <summary>
        /// 编码集合
        /// </summary>
        public IEnumerable<PNRInfo> PNRInfos {
            get { return _pnrInfos.AsReadOnly(); }
        }
        /// <summary>
        /// 资源方
        /// </summary>
        public SupplierInfo Supplier {
            get;
            internal set;
        }
        /// <summary>
        /// 出票方
        /// </summary>
        public ProviderInfo Provider {
            get;
            internal set;
        }
        /// <summary>
        /// 采购方
        /// </summary>
        public PurchaserInfo Purchaser {
            get;
            internal set;
        }
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status {
            get;
            internal set;
        }
        /// <summary>
        /// 订单来源
        /// </summary>
        public OrderSource Source {
            get;
            internal set;
        }
        /// <summary>
        /// 联系信息
        /// </summary>
        public Contact Contact {
            get;
            internal set;
        }
        /// <summary>
        /// 产品信息
        /// </summary>
        public ProductInfo Product {
            get {
                if(this.Supplier != null) {
                    return this.Supplier.Product;
                }
                if(this.Provider != null) {
                    return this.Provider.Product;
                }
                throw new InvalidOperationException("出票方与资源方至少存在一方");
            }
        }
        /// <summary>
        /// 行程类型
        /// </summary>
        public DataTransferObject.Command.PNR.ItineraryType TripType {
            get;
            internal set;
        }
        /// <summary>
        /// 备注
        /// 主要记录订单非正常结束的原因
        /// </summary>
        public string Remark {
            get;
            internal set;
        }
        /// <summary>
        /// 提供资源时间
        /// </summary>
        public DateTime? SupplyTime {
            get;
            internal set;
        }
        /// <summary>
        /// 出票时间
        /// </summary>
        public DateTime? ETDZTime {
            get;
            internal set;
        }
        /// <summary>
        /// 是否是特殊票订单
        /// </summary>
        public bool IsSpecial {
            get {
                return this.Product.ProductType == ProductType.Special;
            }
        }
        /// <summary>
        /// 是否儿童票订单
        /// </summary>
        public bool IsChildrenOrder {
            get {
                return this._pnrInfos.Exists(item => item.Type == DataTransferObject.Command.PNR.PNRType.Child);
            }
        }
        /// <summary>
        /// 是否需要确认
        /// </summary>
        public bool RequireConfirm {
            get {
                if(Product is SpeicalProductInfo) {
                    return (Product as SpeicalProductInfo).RequireConfirm;
                }
                return false;
            }
        }
        /// <summary>
        /// 是否客户自定义资源
        /// </summary>
        public bool IsCustomerResource {
            get;
            internal set;
        }
        /// <summary>
        /// 是否是团队
        /// </summary>
        public bool IsTeam {
            get;
            internal set;
        }
        /// <summary>
        /// 是否是三方关系
        /// </summary>
        public bool IsThirdRelation {
            get {
                return this.IsSpecial && this.Supplier != null;
            }
        }
        /// <summary>
        /// 是否内部机构订单
        /// </summary>
        public bool IsInterior {
            get {
                if(this.Provider != null) {
                    return this.Provider.PurchaserRelationType == Common.Enums.RelationType.Interior;
                }
                return false;
            }
        }
        /// <summary>
        /// 是否修改过价格
        /// </summary>
        public bool RevisedPrice {
            get;
            internal set;
        }
        /// <summary>
        /// 是否降舱订单
        /// </summary>
        public bool IsReduce {
            get;
            internal set;
        }
        /// <summary>
        /// 是否候补
        /// </summary>
        public bool IsStandby {
            get;
            internal set;
        }
        /// <summary>
        /// 是否向乘机人发送了短信
        /// </summary>
        public bool PassengerMsgSended { get; set; }

        /// <summary>
        /// 采购是否已催单
        /// </summary>
        public DateTime? RemindTime { get; set; }

        /// <summary>
        /// 催单内容
        /// </summary>
        public string RemindContent { get; set; }

        /// <summary>
        /// 是否需要平台催单
        /// </summary>
        public bool IsNeedReminded { get; set; }
        /// <summary>
        /// 是否是B3B订单
        /// </summary>
        public virtual bool IsB3BOrder { get; set; }
        /// <summary>
        /// 是否超出支付时限（true代表没有超出，false超出了支付时限）
        /// </summary>
        internal bool IsPayTimeout {
            get {
                if(Status != OrderStatus.Ordered) return false;
                var startTime = IsSpecial && RequireConfirm && SupplyTime.HasValue ? SupplyTime.Value : Purchaser.ProducedTime;
                var payableLimit = IsSpecial ? SystemManagement.SystemParamService.SpecialPayableLimit : SystemManagement.SystemParamService.GeneralPayableLimit;
                var expressedTime = startTime.AddMinutes(payableLimit);
                return expressedTime <= DateTime.Now;
            }
        }
        /// <summary>
        /// 对于需要编码授权的选择
        /// </summary>
        public AuthenticationChoise Choise {
            get;
            internal set;
        }
        /// <summary>
        /// 产品自定义编号
        /// </summary>
        public string CustomNo {
            get;
            internal set;
        }
        /// <summary>
        /// 不允许换编码出票
        /// </summary>
        public bool ForbidChangPNR { get; internal set; }

        internal OrderRole VisibleRole {
            get;
            set;
        }

        /// <summary>
        /// 账单信息
        /// </summary>
        public Service.Distribution.Domain.OrderBill Bill {
            get {
                return _distributionBillLoader.QueryData();
            }
        }
        /// <summary>
        /// 操作信息
        /// </summary>
        public IEnumerable<Log.Domain.OrderLog> Operations {
            get {
                return _operationLoader.QueryDatas();
            }
        }
        /// <summary>
        /// 所有申请
        /// </summary>
        public IEnumerable<BaseApplyform> Applyforms {
            get {
                return _applyformLoader.QueryDatas();
            }
        }
        /// <summary>
        /// 待处理的申请
        /// </summary>
        public IEnumerable<BaseApplyform> AppliedApplyforms {
            get {
                return _applyformLoader.QueryDatas(af => af.ProcessStatus == ApplyformProcessStatus.Applied);
            }
        }
        /// <summary>
        /// 进行中的申请
        /// </summary>
        public IEnumerable<BaseApplyform> HandlingApplyforms {
            get {
                return _applyformLoader.QueryDatas(af => af.ProcessStatus == ApplyformProcessStatus.Processing);
            }
        }
        /// <summary>
        /// 已完成的申请
        /// </summary>
        public IEnumerable<BaseApplyform> FinishedApplyforms {
            get {
                return _applyformLoader.QueryDatas(af => af.ProcessStatus == ApplyformProcessStatus.Finished);
            }
        }
        /// <summary>
        /// 协调信息
        /// </summary>
        public IEnumerable<Coordination> Coordinations {
            get {
                return _coordinationLoader.QueryDatas();
            }
        }

        /// <summary>
        /// 是否是OEM订单
        /// </summary>
        public bool IsOEMOrder
        {
            get
            {
                return OEMID.HasValue;
            }
        }
        /// <summary>
        /// 所属OEM的ID
        /// </summary>
        public Guid? OEMID
        {
            get;
            internal set;
        }
        /// <summary>
        /// 订单所属的OEM信息
        /// </summary>
        public OEMInfo OemInfo
        {
            get {
                if (!IsOEMOrder) return null;
                return _OemInfoLoader.QueryData();
            }
        }
        /// <summary>
        /// 是否是订单标识
        /// </summary>
        public bool IsEmergentOrder { get; set; }
        /// <summary>
        /// 是否需要授权
        /// </summary>
        public bool NeedAUTH
        {
            get;
            set;
        }

        #endregion

        #region "Static Methods"
        internal static Order NewOrder(OrderView orderView, MatchedPolicy matchedPolicy, EmployeeDetailInfo employee, bool forbidChnagePnr, Guid oemid, AuthenticationChoise choise = AuthenticationChoise.NoNeedAUTH) {
            if(!orderView.Flights.Any()) throw new ArgumentNullException("orderView", "缺少航段信息");
            if(!orderView.Passengers.Any()) throw new ArgumentNullException("orderView", "缺少乘机人信息");
            if(matchedPolicy == null) throw new CustomException("无相关政策信息");

            var result = new Order {
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
                IsB3BOrder = true,
                ForbidChangPNR = forbidChnagePnr,
                NeedAUTH = matchedPolicy.OriginalPolicy == null ? matchedPolicy.NeedAUTH : matchedPolicy.OriginalPolicy.NeedAUTH
            };
            if (oemid!=Guid.Empty)
            {
                result.OEMID = oemid;
            }
            var deduction = Deduction.GetDeduction(matchedPolicy);
            var product = ProductInfo.GetProductInfo(matchedPolicy);
            var specialProduct = product as SpeicalProductInfo;
            if(specialProduct != null && !hasETDZPermission(matchedPolicy.Provider, specialProduct)) {
                result.Supplier = getSupplierInfo(matchedPolicy, specialProduct);
            } else {
                result.Provider = getProvider(matchedPolicy, product);
            }
            result.IsCustomerResource = ProductInfo.IsCustomerResource(matchedPolicy);
            result.IsStandby = ProductInfo.IsStandby(matchedPolicy);
            result.Purchaser = getPurchaserInfo(employee, deduction);
            var pnrInfo = PNRInfo.GetPNRInfo(orderView, matchedPolicy);
            result.AddPNRInfo(pnrInfo);
            result.TripType = pnrInfo.TripType;
            result.Status = result.RequireConfirm ? OrderStatus.Applied : OrderStatus.Ordered;
            if(result.Status == OrderStatus.Applied) {
                result.VisibleRole |= result.IsThirdRelation ? OrderRole.Supplier : OrderRole.Provider;
            }
            return result;
        }
        protected static ProviderInfo getProvider(MatchedPolicy policy, ProductInfo product) {
            var provider = CompanyService.GetCompanyDetail(policy.Provider);
            if(!publisherIsValid(provider)) throw new CustomException("出票方已无效，无法生成订单");
            return new Domain.ProviderInfo(policy.Provider, provider.AbbreviateName) {
                Rebate = Deduction.GetDeduction(policy).Provider,
                Product = product,
                PurchaserRelationType = policy.RelationType,
            };
        }
        protected static SupplierInfo getSupplierInfo(MatchedPolicy specialPolicy, SpeicalProductInfo product) {
            var supplier = CompanyService.GetCompanyDetail(specialPolicy.Provider);
            if(!publisherIsValid(supplier)) throw new CustomException("产品发布方已无效，无法生成订单");
            return new Domain.SupplierInfo(specialPolicy.Provider, supplier.AbbreviateName) {
                Rebate = Deduction.GetDeduction(specialPolicy).Supplier,
                Product = product
            };
        }
        protected static PurchaserInfo getPurchaserInfo(EmployeeDetailInfo employee, Deduction deduction) {
            var purchaser = CompanyService.GetCompanyInfo(employee.Owner);
            return new Domain.PurchaserInfo(employee.Owner, purchaser.AbbreviateName) {
                Rebate = deduction.Purchaser,
                OperatorAccount = employee.UserName,
                OperatorName = employee.Name,
                ProducedTime = DateTime.Now
            };
        }
        protected static bool hasETDZPermission(Guid publisherId, SpeicalProductInfo speicalProduct) {
            var publisher = CompanyService.GetCompanyInfo(publisherId);
            if(publisher.Type == CompanyType.Provider) return true;
            switch(speicalProduct.SpeicalProductType) {
                case SpecialProductType.Singleness:
                case SpecialProductType.Disperse:
                case SpecialProductType.CostFree:
                case SpecialProductType.Bloc:
                    return false;
                case SpecialProductType.Business:
                    return true;
                default:
                    return false;
            }
        }
        protected static bool publisherIsValid(CompanyDetailInfo company) {
            if(!company.Enabled) return false;
            if(company.PeriodStartOfUse.HasValue && company.PeriodStartOfUse.Value.Date > DateTime.Today) {
                return false;
            }
            if(company.PeriodEndOfUse.HasValue && company.PeriodEndOfUse.Value.Date < DateTime.Today) {
                return false;
            }
            return true;
        }
        #endregion

        #region "Methods"
        /// <summary>
        /// 添加编码信息
        /// </summary>
        internal void AddPNRInfo(PNRInfo pnrInfo) {
            if(null == pnrInfo)
                throw new ArgumentNullException("pnrInfo", "编码信息不能为空");
            if(_pnrInfos.Exists(item => PNRPair.Equals(item.Code, pnrInfo.Code)))
                throw new RepeatedItemException("不能重复加入同一编码");
            _pnrInfos.Add(pnrInfo);
        }
        /// <summary>
        /// 修改价格
        /// </summary>
        internal decimal ReviseReleasedFare(decimal releasedFare) {
            checkReleasedFareRevisalbe(releasedFare);
            var pnrInfo = this.PNRInfos.First();
            var originalReleasedFare = pnrInfo.ReviseReleasedFare(releasedFare);
            this.RevisedPrice = true;
            return originalReleasedFare;
        }
        /// <summary>
        /// 修改价格信息
        /// </summary>
        internal void ReviseFare(IEnumerable<PriceView> priceViews) {
            if(Status != OrderStatus.Finished) throw new StatusException(Id.ToString());
            if(RevisedPrice) throw new CustomException("已修改过价格，不能再次修改");
            if(FinishedApplyforms.Any(a => a is RefundOrScrapApplyform)) throw new CustomException("已有完成的退废票申请单，不能修改价格");
            PNRInfos.First().ReviseFare(priceViews);
            RevisedPrice = true;
            foreach(var applyform in Applyforms) {
                applyform.RequireRevisePrice = false;
            }
        }
        /// <summary>
        /// 资源方确认成功
        /// </summary>
        internal bool ConfirmResourceSuccessful(PNRPair pnrCode, decimal? patPrice, Guid oemId)
        {
            checkConfirmResource();
            var fareRevised = updateOrderForResource(pnrCode, patPrice,oemId);
            if(!IsThirdRelation||patPrice.HasValue)Status = OrderStatus.Ordered;
            return fareRevised;
        }
        /// <summary>
        /// 资源方确认失败
        /// </summary>
        internal void ConfirmResourceFailed(string reason) {
            checkConfirmResource();
            if(string.IsNullOrWhiteSpace(reason))
                throw new CustomException("必须提供确认失败的原因");
            this.Status = OrderStatus.ConfirmFailed;
            this.Remark = reason;
        }
        /// <summary>
        /// 检查订单是否可支付
        /// </summary>
        internal bool IsPayable(out string message) {
            if(this.Status == OrderStatus.Ordered) {
                message = string.Empty;
                try {
                    if(IsPayTimeout) {
                        message = "订单已超出支付时限，请重新预订";
                        return false;
                    }
                    checkPublisherIsWorking();
                    //if (!IsStandby)
                    //{
                    //    var result = checkReservationPNRValidity(Guid.Empty);
                    //    if (!string.IsNullOrEmpty(result))
                    //    {
                    //        message = result;
                    //    }
                    //}
                    return true;
                } catch(CustomException ex) {
                    message = ex.Message;
                }
            } else {
                message = "仅待支付的订单才需要支付";
            }
            return false;
        }
        /// <summary>
        /// 提供资源
        /// </summary>
        internal bool SupplyResource(PNRPair pnrCode, decimal? patPrice, Guid oemId)
        {
            checkSupplyResource();
            var fareRevised = updateOrderForResource(pnrCode, patPrice,oemId);
            if(!IsThirdRelation || patPrice.HasValue) Status = Status == OrderStatus.PaidForSupply ? OrderStatus.PaidForETDZ : OrderStatus.Ordered;
            VisibleRole |= OrderRole.Provider;
            this.IsEmergentOrder = false;
            return fareRevised;
        }
        /// <summary>
        /// 拒绝提供资源
        /// </summary>
        internal void DenySupplyResource(string reason) {
            checkSupplyResource();
            if(string.IsNullOrWhiteSpace(reason)) throw new CustomException("必须提供确认失败的原因");
            this.Status = OrderStatus.DeniedWithSupply;
            this.Remark = reason;
        }
        /// <summary>
        /// 匹配出票方
        /// </summary>
        internal void MatchProvider(PolicyMatch.MatchedPolicy policy) {
            if(this.IsSpecial) {
                if(this.Status == OrderStatus.Ordered || this.Status == OrderStatus.PaidForETDZ) {
                    if(this.IsThirdRelation) {
                        var deduction = Deduction.GetDeduction(policy);
                        var product = ProductInfo.GetProductInfo(policy);
                        this.Supplier.Rebate = deduction.Purchaser;
                        this.Provider = getProvider(policy, product);
                        if (policy.PolicyType == PolicyType.BargainDefault || policy.PolicyType == PolicyType.NormalDefault)
                            this.Provider.Product.IsDefaultPolicy = true;
                        CustomNo = policy.OriginalPolicy == null ? string.Empty : policy.OriginalPolicy.CustomCode;
                        return;
                    }
                    throw new CustomException("资源方具有出票资质，不能选择其他出票方出票");
                }
                throw new StatusException(this.Id.ToString());
            }
            throw new CustomException("特殊票才需要匹配出票方出票");
        }
        /// <summary>
        /// 换出票方
        /// </summary>
        internal void ChangeProvider(MatchedPolicy policy, bool forbidChangePNR) {
            checkCanChangeProvider(policy);
            var newProvider = getProvider(policy, ProductInfo.GetProductInfo(policy));
            if(newProvider.PurchaserRelationType == RelationType.Interior) throw new CustomException("该政策是采购的上级发布的政策，不允许指向给该出票方，请重新选择");
            this.Provider = newProvider;
            this.ForbidChangPNR = forbidChangePNR;
            this.IsB3BOrder = !policy.IsExternal; 
            if(this.IsSpecial) {
                var deduction = Deduction.GetDeduction(policy);
                this.Supplier.Rebate = deduction.Purchaser;
            }
            this.Status = OrderStatus.PaidForETDZ;
            CustomNo = policy.OriginalPolicy == null ? string.Empty : policy.OriginalPolicy.CustomCode;
        }
        /// <summary>
        /// 支付成功
        /// </summary>
        internal void PaySuccess(DateTime? payTime) {
            if(this.Status == OrderStatus.Ordered) {
                if(IsSpecial) {
                    var specialProduct = Product as SpeicalProductInfo;
                    if(specialProduct.RequireConfirm) {
                        Status = OrderStatus.PaidForETDZ;
                    } else {
                        switch(specialProduct.SpeicalProductType) {
                            case SpecialProductType.CostFree:
                            case SpecialProductType.Bloc:
                            case SpecialProductType.Business:
                            case SpecialProductType.LowToHigh:
                                Status = PNRPair.IsNullOrEmpty(ReservationPNR) ? OrderStatus.PaidForSupply : OrderStatus.PaidForETDZ;
                                break;
                            default:
                                Status = OrderStatus.PaidForSupply;
                                break;
                        }
                    }
                } else {
                    this.Status = OrderStatus.PaidForETDZ;
                }
                Purchaser.PayTime = payTime;
                VisibleRole |= Status == OrderStatus.PaidForETDZ || !IsThirdRelation ? OrderRole.Provider : OrderRole.Supplier;
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }
        /// <summary>
        /// 出票成功，填票号
        /// </summary>
        internal void FillTicketNo(DateTime etdzTime, TicketNoView ticketNoView, string operatorAccount, string operatorName, Guid oemId)
        {
            if(this.Status == OrderStatus.PaidForETDZ) {
                if(ticketNoView == null) throw new ArgumentNullException("ticketNoView");
                var ticketNos = from p in ticketNoView.Items
                                from t in p.TicketNos
                                select t;
                if(ticketNos.Count() > ticketNos.Distinct().Count()) throw new CustomException("票号不能重复");
                this.ETDZPNR = this.PNRInfos.First().FillTicketNos(ticketNoView.ETDZPNR, ticketNoView.Mode, ticketNoView.NewSettleCode, ticketNoView.Items,oemId);
                if(!string.IsNullOrWhiteSpace(ticketNoView.OfficeNo)) {
                    Provider.Product.OfficeNo = ticketNoView.OfficeNo;
                }
                Provider.Product.TicketType = ticketNoView.TicketType;
                this.Status = OrderStatus.Finished;
                this.ETDZTime = etdzTime;
                this.IsEmergentOrder = false;
                if (!string.IsNullOrEmpty(operatorName))
                {
                    Provider.OperatorName = operatorName;
                }
                if (!string.IsNullOrEmpty(operatorAccount))
                {
                    Provider.OperatorAccount = operatorAccount;
                }
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }
        /// <summary>
        /// 拒绝出票
        /// </summary>
        internal void DenyOutticket(string reason) {
            if(this.Status == OrderStatus.PaidForETDZ) {
                if(string.IsNullOrWhiteSpace(reason))
                    throw new CustomException("必须提供确认失败的原因");
                this.Status = OrderStatus.DeniedWithETDZ;
                this.Remark = reason;
                RefuseETDZTime = DateTime.Now;
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }

        /// <summary>
        /// 已经向乘机人发送了出票短信
        /// </summary>
        internal void MessageSended() {
            PassengerMsgSended = true;
        }

        /// <summary>
        /// 采购已经催过单
        /// </summary>
        /// <param name="remindContent">催到内容</param>
        internal void Reminded(string remindContent) {
            RemindTime = DateTime.Now;
            RemindContent = remindContent;
        }

        protected DateTime? RefuseETDZTime { get; set; }

        /// <summary>
        /// 重新提供资源
        /// </summary>
        internal void ReSupplyResource() {
            if(this.IsSpecial) {
                if(this.Status == OrderStatus.DeniedWithETDZ) {
                    if(this.IsThirdRelation) {
                        this.Status = OrderStatus.PaidForSupply;
                    } else {
                        this.Status = OrderStatus.PaidForETDZ;
                    }
                } else {
                    throw new StatusException(this.Id.ToString());
                }
            } else {
                throw new CustomException("特殊票才需要提供资源");
            }
        }
        /// <summary>
        /// 重新出票
        /// </summary>
        internal void ReOutticket() {
            if(this.Status == OrderStatus.DeniedWithETDZ) {
                this.Status = OrderStatus.PaidForETDZ;
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        internal void Cancel(string reason = null) {
            if(this.Status == OrderStatus.DeniedWithETDZ || this.Status == OrderStatus.Ordered) {
                this.Status = OrderStatus.Canceled;
                if(!string.IsNullOrWhiteSpace(reason)) this.Remark = reason.Trim();
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }
        /// <summary>
        /// 修改票号
        /// </summary>
        internal List<Passenger> UpdateTicketNo(string[] originalTicketNo, string[] newTicketNo, string settleCode) {
            if(this.Status == OrderStatus.Finished) {
                var result = new List<Passenger>();
                //if(originalTicketNo == newTicketNo) throw new CustomException("新旧票号不能相同");
                for(int i = 0; i < originalTicketNo.Length; i++) {
                    foreach(var pnr in this.PNRInfos) {
                        if(pnr.ContainsTicket(newTicketNo[i])) throw new CustomException("票号不能重复");
                        if(pnr.ContainsTicket(originalTicketNo[i])) {
                            result.Add(pnr.UpdateTicketNo(originalTicketNo[i], settleCode, newTicketNo[i]));
                        }
                    }

                }
                if(!result.Any()) throw new NotFoundException(string.Join(",", originalTicketNo));
                return result;
            }
            throw new StatusException(this.Id.ToString());
        }
        /// <summary>
        /// 修改证件号
        /// </summary>
        internal Passenger UpdateCredentitials(string passengerName, string originalCredentials, string newCredentials, bool execCommand, out bool success,Guid oemId) {
            if(this.Status == OrderStatus.Finished) {
                if(originalCredentials == newCredentials) throw new CustomException("新旧证件号不能相同");
                foreach(var pnr in this.PNRInfos) {
                    if(pnr.ContainsCredentials(newCredentials)) throw new CustomException("证件号不能重复");
                    if(pnr.ContainsPassenger(passengerName, originalCredentials)) {
                        return pnr.UpdateCredentials(passengerName, originalCredentials, newCredentials, execCommand, out success,oemId);
                    }
                }
                throw new NotFoundException("乘机人:" + passengerName + " 证件号:" + originalCredentials);
            }
            throw new StatusException(this.Id.ToString());
        }

        internal void SetBill(Service.Distribution.Domain.Bill.Pay.NormalPayBill payBill) {
            if(payBill != null) {
                if(this.Purchaser != null && payBill.Purchaser != null) {
                    this.Purchaser.Commission = Math.Abs(payBill.Purchaser.Source.Commission);
                    this.Purchaser.Amount = Math.Abs(payBill.Purchaser.Source.Anticipation);
                }
                if(this.Provider != null && payBill.Provider != null) {
                    this.Provider.Commission = Math.Abs(payBill.Provider.Source.Commission);
                    this.Provider.Amount = payBill.Provider.Source.Anticipation;
                }
                if(this.Supplier != null && payBill.Supplier != null) {
                    this.Supplier.Commission = Math.Abs(payBill.Supplier.Source.Commission);
                    this.Supplier.Amount = payBill.Supplier.Source.Anticipation;
                }
            }
            this.Bill.PayBill = payBill;
        }
        /// <summary>
        /// 申请退改签
        /// </summary>
        internal BaseApplyform Apply(ApplyformView applyformView, Guid oemId)
        {
            var checker = ApplyChecker.GetChecker(this, applyformView);
            checker.Execute();
            var applyform = BaseApplyform.NewApplyform(this, applyformView,oemId);
            this._applyformLoader.AppendData(applyform);
            return applyform;
        }
        /// <summary>
        /// 根据申请单修改订单内容
        /// </summary>
        internal void Update(BaseApplyform applyform) {
            var originalPNRInfo = this.PNRInfos.FirstOrDefault(item => PNRPair.Equals(item.Code, applyform.OriginalPNR));
            if(originalPNRInfo == null) throw new CustomException("未找到原编码信息");
            PNRInfo newPnrInfo = null;
            if(applyform is RefundOrScrapApplyform) {
                newPnrInfo = originalPNRInfo.UpdateContentForRefund(applyform as RefundOrScrapApplyform);
            } else if(applyform is PostponeApplyform) {
                newPnrInfo = originalPNRInfo.UpdateContentForPostpone(applyform as PostponeApplyform);
            }
            if(newPnrInfo != null) {
                AddPNRInfo(newPnrInfo);
            }
        }
        internal PNRInfo GetPNRInfo(PNRPair pnrCode) {
            return _pnrInfos.FirstOrDefault(item => PNRPair.Equals(item.Code, pnrCode));
        }
        void checkConfirmResource() {
            if(this.RequireConfirm) {
                if(this.Status == OrderStatus.Applied) {
                    return;
                }
                throw new StatusException(this.Id.ToString());
            }
            throw new CustomException("该订单无需确认资源");
        }
        void checkSupplyResource() {
            if(this.IsSpecial) {
                if(this.Status == OrderStatus.PaidForSupply) {
                    return;
                }
                throw new StatusException(this.Id.ToString());
            }
            throw new CustomException("特殊票才需要提供资源");
        }
        void checkPublisherIsWorking() {
            var company = this.Supplier == null ? this.Provider.CompanyId : this.Supplier.CompanyId;
            var workingHours = CompanyService.GetWorkinghours(company);
            Izual.Time startTime, endTime;
            bool isWeekend = DateTime.Today.IsWeekend();
            if (!IsB3BOrder)
            {
                var externalPolicy = OrderProcessService.LoadExternalPolicy(Id);
                if (externalPolicy==null)
                {
                    startTime = new Izual.Time(8, 0, 0);
                    endTime = new Izual.Time(18, 0, 0);
                }
                else if(!isWeekend)
                {
                    startTime = externalPolicy.WorkTimeStart;
                    endTime = externalPolicy.WorkTimeEnd;
                }
                else
                {
                    startTime = externalPolicy.RestWorkTimeStart;
                    endTime = externalPolicy.RestWorkTimeEnd;
                }
            }
            else
            {
                if (isWeekend)
                {
                    startTime = workingHours.RestdayWorkStart;
                    endTime = workingHours.RestdayWorkEnd;
                }
                else
                {
                    startTime = workingHours.WorkdayWorkStart;
                    endTime = workingHours.WorkdayWorkEnd;
                }
            }
            if(Izual.Time.Now < startTime || Izual.Time.Now > endTime) throw new CustomException("供应商已下班");
        }
        void checkCanChangeProvider(PolicyMatch.MatchedPolicy policy) {
            if(this.Status == OrderStatus.DeniedWithETDZ) {
                if(this.IsSpecial) {
                    if(this.IsThirdRelation) {
                        if(policy.PolicyType != PolicyType.Normal && policy.PolicyType != PolicyType.NormalDefault && policy.PolicyType != PolicyType.OwnerDefault) {
                            throw new CustomException("特殊产品只能匹配出票方的普通政策");
                        }
                    } else {
                        throw new CustomException("该特殊产品发布方具有出票资质，不能更换出票方");
                    }
                } else {
                    if(this.Product.ProductType == ProductType.General) {
                        if(policy.PolicyType != PolicyType.Normal && policy.PolicyType != PolicyType.NormalDefault && policy.PolicyType != PolicyType.OwnerDefault) {
                            throw new CustomException("普通票只能更换到普通政策");
                        }
                    } else if(this.Product.ProductType == ProductType.Team) {
                        if(policy.PolicyType != PolicyType.Team && policy.PolicyType != PolicyType.NormalDefault && policy.PolicyType != PolicyType.OwnerDefault) {
                            throw new CustomException("团队票只能更换到团队政策");
                        }
                    } else {
                        throw new CustomException("仅普通政策订单可以更换出票方");
                    }
                }
            } else {
                throw new StatusException(this.Id.ToString());
            }
        }
        string checkReservationPNRValidity(Guid oemId) {
            if(!PNRPair.IsNullOrEmpty(this.ReservationPNR)) {
                var executeResult = CommandService.GetReservedPnr(ReservationPNR,oemId);
                if(executeResult.Success) {
                    CommandService.ValidatePNR(executeResult.Result, IsChildrenOrder ? PassengerType.Child : PassengerType.Adult);
                    if(Source == OrderSource.PlatformOrder && executeResult.Result.Passengers != null && executeResult.Result.Passengers.Join(PNRInfos.First().Passengers, p => p.Name, p => p.Name, (p, q) => p).Count() != executeResult.Result.Passengers.Count) {
                        LogService.SaveExceptionLog(new Exception("乘机人信息不符") { Source = "编码验证" },
                            "订单号：" + Id + ",编码内容中的乘机人姓名为：" + string.Join(",", executeResult.Result.Passengers.Select(p => p.Name)));
                    }
                } else {
                    //throw new CustomException("编码提取失败");
                    if (OrderSource.PlatformOrder!=Source)
                    {
                        return "暂时无法验证您导入编码的位置是否有效,请您自行确认";
                    }
                    else
                    {
                        LogService.SaveExceptionLog(new CustomException(string.Format("由于编码内容提取失败，平台预订的编码：{0} 未进行有效性验证，时间：{1:yyyy-MM-dd HH:mm}",ReservationPNR.ToListString(),DateTime.Now)));
                    }
                }
            }
            return string.Empty;
        }
        void checkReleasedFareRevisalbe(decimal releasedFare) {
            if(this.IsSpecial && this.RequireConfirm && this.IsThirdRelation) {
                if(this.Status != OrderStatus.Applied) throw new StatusException(this.Id.ToString());
            } else {
                throw new CustomException("仅产品方可修改需要确认的特殊票的价格");
            }
        }
        bool updateOrderForResource(PNRPair pnrCode, decimal? patPrice, Guid oemId)
        {
            if(PNRPair.IsNullOrEmpty(pnrCode)) throw new CustomException("编码信息不能为空");
            var pnrInfo = this._pnrInfos.First();
            bool fareRevised = false;
                fareRevised = pnrInfo.UpdateContentForResource(pnrCode, (Product as SpeicalProductInfo).SpeicalProductType != SpecialProductType.OtherSpecial, patPrice, IsStandby, oemId,PNRInfos.First().PNRContent != string.Empty,IsThirdRelation);
            ReservationPNR = pnrInfo.Code;
            SupplyTime = DateTime.Now;
            return fareRevised;
        }
        #endregion

        public void UpdateProvision(MatchedPolicy policy) {
            if (policy.PolicyType == PolicyType.Bargain || policy.PolicyType == PolicyType.BargainDefault)
            {
                var regulation = policy as IHasRegulation;
                Provider.Product.RefundAndReschedulingProvision.Refund = regulation.RefundRegulation;
                Provider.Product.RefundAndReschedulingProvision.Scrap = regulation.InvalidRegulation;
                Provider.Product.RefundAndReschedulingProvision.Transfer = regulation.EndorseRegulation;
                Provider.Product.RefundAndReschedulingProvision.Alteration = regulation.ChangeRegulation;
            }
            else
            {
                var flight = PNRInfos.First().Flights.First();
                var pattern = new Regex("^[a-zA-Z\\d/]+$");
                var details = FoundationService.QueryDetailList(flight.Carrier.Code,flight.Bunk.Code).Where(item => pattern.IsMatch(item.Bunks));
                string refundRegulation = string.Empty;
                string changeRegulation = string.Empty;
                string endorseRegulation = string.Empty;
                foreach (var item in details)
                {
                    refundRegulation += "航班起飞前：" + item.ScrapBefore + "；航班起飞后：" + item.ScrapAfter;
                    changeRegulation += "航班起飞前：" + item.ChangeBefore + "；航班起飞后：" + item.ChangeAfter;
                    endorseRegulation += item.Endorse;
                }
                if (string.IsNullOrWhiteSpace(refundRegulation))
                    refundRegulation = "以航司具体规定为准";
                if (string.IsNullOrWhiteSpace(changeRegulation))
                    changeRegulation = "以航司具体规定为准";
                Provider.Product.RefundAndReschedulingProvision.Refund = refundRegulation;
                Provider.Product.RefundAndReschedulingProvision.Scrap = details.Any() ? details.First().Scrap : "以航司具体规定为准";
                Provider.Product.RefundAndReschedulingProvision.Transfer = endorseRegulation;
                Provider.Product.RefundAndReschedulingProvision.Alteration = changeRegulation;

            }
        }
    }
}