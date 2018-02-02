using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.Core;
using ChinaPay.Core.Exception;
using ChinaPay.Data;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform {
    /// <summary>
    /// 申请单
    /// </summary>
    public abstract class BaseApplyform {
        #region "Fields"
        LazyLoader<Order> _orderLoader;
        LazyLoader<OEMInfo> _OemInfoLoader;
        LazyLoader<DataTransferObject.Organization.CompanyInfo> _purchaserLoader;
        LazyLoader<DataTransferObject.Organization.CompanyInfo> _providerLoader;
        EnumerableLazyLoader<Log.Domain.OrderLog> _operationLoader;
        EnumerableLazyLoader<Coordination> _coordinationLoader;
        List<Passenger> _passengers;
        #endregion

        #region "Constuctors"
        protected BaseApplyform(decimal orderId, decimal applyformId) {
            this.OrderId = orderId;
            this.Id = applyformId;
            this._passengers = new List<Passenger>();
            initLazyLoaders();
        }
        protected BaseApplyform(Order order, ApplyformView applyformView) {
            this.OrderId = order.Id;
            this._passengers = new List<Passenger>();
            initLazyLoaders();
            this._orderLoader.SetData(order);
            this.Id = Sequence.SequenceService.GenerateTicketApplyformId();

            setPNRPair(applyformView);
            setPassengers(applyformView);
            this.ProductType = order.Product.ProductType;
            this.PurchaserId = order.Purchaser.CompanyId;
            this.OEMID = order.OEMID;
            this.PurchaserName = order.Purchaser.Name;
            this.ProviderId = order.Provider.CompanyId;
            this.ProviderName = order.Provider.Name;
            this.ApplyRemark = applyformView.Reason;
            this.IsInterior = order.IsInterior;
            this.AppliedTime = DateTime.Now;
        }
        void initLazyLoaders() {
            _operationLoader = new EnumerableLazyLoader<Log.Domain.OrderLog>(() => LogService.QueryApplyformLog(this.Id));
            _coordinationLoader = new EnumerableLazyLoader<Coordination>(() => CoordinationService.QueryApplyformCoordinations(this.Id));
            _orderLoader = new LazyLoader<Order>(() => OrderQueryService.QueryOrder(this.OrderId));
            _purchaserLoader = new LazyLoader<DataTransferObject.Organization.CompanyInfo>(() => Organization.CompanyService.GetCompanyDetail(this.PurchaserId));
            _providerLoader = new LazyLoader<DataTransferObject.Organization.CompanyInfo>(() => Organization.CompanyService.GetCompanyDetail(this.ProviderId));
            _OemInfoLoader = new LazyLoader<OEMInfo>(() => OEMService.QueryOEMById(OEMID.Value));
        }
        #endregion

        #region "Properties"
        /// <summary>
        /// 申请单号
        /// </summary>
        public decimal Id {
            get;
            private set;
        }
        /// <summary>
        /// 产品类型
        /// </summary>
        public ProductType ProductType {
            get;
            internal set;
        }
        /// <summary>
        /// 原订单号
        /// </summary>
        public decimal OrderId {
            get;
            private set;
        }

        /// <summary>
        /// 关联申请单号
        /// </summary>
        public virtual decimal AssociateApplyformId
        {
            get
            {
                return Id;
            }
            set { }
        }

        /// <summary>
        /// 原订单
        /// </summary>
        public Order Order {
            get {
                return _orderLoader.QueryData();
            }
        }
        /// <summary>
        /// 原申请编码
        /// </summary>
        public PNRPair OriginalPNR {
            get;
            internal set;
        }
        /// <summary>
        /// 新编码
        /// </summary>
        public PNRPair NewPNR {
            get;
            internal set;
        }
        /// <summary>
        /// 原乘机人信息
        /// </summary>
        public IEnumerable<Passenger> Passengers {
            get {
                return _passengers.AsReadOnly();
            }
        }
        /// <summary>
        /// 原航班信息
        /// </summary>
        public abstract IEnumerable<Flight> OriginalFlights { get; }
        /// <summary>
        /// 采购方单位Id
        /// </summary>
        public Guid PurchaserId {
            get;
            internal set;
        }
        /// <summary>
        /// 采购方单位名称
        /// </summary>
        public string PurchaserName {
            get;
            internal set;
        }
        /// <summary>
        /// 采购方单位信息
        /// </summary>
        public DataTransferObject.Organization.CompanyInfo Purchaser {
            get { return _purchaserLoader.QueryData(); }
        }
        /// <summary>
        /// 出票方单位Id
        /// </summary>
        public Guid ProviderId {
            get;
            internal set;
        }
        /// <summary>
        /// 出票方单位名称
        /// </summary>
        public string ProviderName {
            get;
            internal set;
        }
        /// <summary>
        /// 出票方单位信息
        /// </summary>
        public DataTransferObject.Organization.CompanyInfo Provider {
            get { return _providerLoader.QueryData(); }
        }
        /// <summary>
        /// 申请备注
        /// </summary>
        public string ApplyRemark {
            get;
            internal set;
        }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime AppliedTime {
            get;
            internal set;
        }
        /// <summary>
        /// 申请人操作员账号
        /// </summary>
        public string ApplierAccount {
            get;
            internal set;
        }
        /// <summary>
        /// 申请操作员名称
        /// </summary>
        public string ApplierAccountName {
            get;
            internal set;
        }
        /// <summary>
        /// 处理失败原因
        /// </summary>
        public string ProcessedFailedReason {
            get;
            internal set;
        }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessedTime {
            get;
            internal set;
        }
        /// <summary>
        /// 是否内部机构的申请
        /// </summary>
        public bool IsInterior {
            get;
            internal set;
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
        /// 协调信息
        /// </summary>
        public IEnumerable<Coordination> Coordinations {
            get {
                return _coordinationLoader.QueryDatas();
            }
        }
        /// <summary>
        /// 处理状态
        /// </summary>
        public abstract ApplyformProcessStatus ProcessStatus {
            get;
        }
        /// <summary>
        /// 是否需要分离编码
        /// </summary>
        public bool RequireSeparatePNR {
            get {
                var pnrInfo = this.Order.PNRInfos.FirstOrDefault(item => item.IsSamePNR(this.OriginalPNR));
                return pnrInfo != null && pnrInfo.RequireSeparate(this);
            }
        }
        /// <summary>
        /// 是否需要修改原订单的价格信息
        /// </summary>
        public bool RequireRevisePrice {
            get;
            internal set;
        }
        /// <summary>
        /// 申请处理操作员
        /// </summary>
        public string @Operator {
            get;
            set;
        }

        /// <summary>
        /// 申请处理帐号
        /// </summary>
        public string OperatorAccount {
            get;
            set;
        }

        /// <summary>
        /// 是否是OEM申请单
        /// </summary>
        public bool IsOEMApplyform
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
            get
            {
                if (!IsOEMApplyform) return null;
                return _OemInfoLoader.QueryData();
            }
        }


        #endregion

        #region "Methods"
        internal void AddPassenger(Passenger passenger) {
            if(passenger == null) throw new ArgumentNullException("passenger");
            if(_passengers.Exists(item => string.Compare(item.Name, passenger.Name, StringComparison.OrdinalIgnoreCase) == 0)) throw new RepeatedItemException("不同重复添加同一乘机人");
            _passengers.Add(passenger);
        }
        protected void Processed() {
            Processed(string.Empty);
        }
        protected void Processed(string reason) {
            this.ProcessedTime = DateTime.Now;
            this.ProcessedFailedReason = reason;
        }
        private void setPNRPair(ApplyformView applyformView) {
            if (applyformView is BalanceRefundApplyView)
            {
                OriginalPNR = applyformView.PNR;
                return;
            }
            if(PNRPair.IsNullOrEmpty(applyformView.PNR)) throw new CustomException("缺少编码信息 ");
            var pnrInfo = this.Order.PNRInfos.FirstOrDefault(item => item.IsSamePNR(applyformView.PNR));
            if(pnrInfo == null) throw new NotFoundException("原订单中不存在编码信息。" + applyformView.PNR.ToString());
            this.OriginalPNR = pnrInfo.Code;
        }
        private void setPassengers(ApplyformView applyformView) {
            if(applyformView.Passengers != null) {
                var pnrInfo = this.Order.PNRInfos.First(item => item.IsSamePNR(applyformView.PNR));
                foreach(var passenger in applyformView.Passengers) {
                    var originalPassenger = pnrInfo.Passengers.FirstOrDefault(item => item.Id == passenger);
                    if(originalPassenger == null) throw new NotFoundException("原编码中不存在乘机人信息。");
                    this.AddPassenger(originalPassenger);
                }
                if(!this.Passengers.Any()) throw new CustomException("缺少乘机人信息");
            }
        }
        internal IEnumerable<Guid> GetAppliedPassengers() {
            return this.Passengers.Select(item => item.Id);
        }
        internal abstract IEnumerable<Guid> GetAppliedFlights();
        internal bool Contains(Guid passenger, Guid flight) {
            return ContainsPassenger(passenger) && ContainsFlight(flight);
        }
        internal bool ContainsPassenger(Guid passenger) {
            return GetAppliedPassengers().Any(item => item == passenger);
        }
        internal bool ContainsFlight(Guid flight) {
            return GetAppliedFlights().Any(item => item == flight);
        }
        #endregion

        internal static BaseApplyform NewApplyform(Order order, ApplyformView applyformView, Guid oemId)
        {
            if(applyformView is RefundOrScrapApplyformView) {
                return RefundOrScrapApplyform.NewRefundOrScrapApplyform(order, applyformView as RefundOrScrapApplyformView,oemId);
            } else if(applyformView is PostponeApplyformView) {
                return new PostponeApplyform(order, applyformView as PostponeApplyformView);
            } else if(applyformView is UpgradeApplyformView) {
                return new UpgradeApplyform(applyformView as UpgradeApplyformView, order);
            }
            else if (applyformView is BalanceRefundApplyView)
            {
                return new BalanceRefundApplyform(order,applyformView as BalanceRefundApplyView);
            }
            throw new ChinaPay.Core.CustomException("未知申请类型");
        }

       
    }
}