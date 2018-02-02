using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.Core;
using ChinaPay.Data;
using ChinaPay.Sequence;

namespace ChinaPay.B3B.Service.Order.Domain.Applyform
{
    /// <summary>
    /// ����˿����뵥
    /// </summary>
    public class BalanceRefundApplyform : BaseApplyform
    {
        private RefundOrScrapApplyform _applyform;
        private LazyLoader<RefundOrScrapApplyform> _applyformLoader;
        private LazyLoader<NormalRefundBill> _refundBillLoader;

        #region "Constructors"


        internal BalanceRefundApplyform(decimal orderId,decimal applyformId) : base(orderId,applyformId)
        {
            initLazyLoaders();
        }

        internal BalanceRefundApplyform(Order order, BalanceRefundApplyView applyView)
            : base(order, applyView)
        {
            initLazyLoaders();
            var applyform = ApplyformQueryService.QueryRefundOrScrapApplyform(applyView.AssociateApplyformId);
            if (applyform == null) throw new ArgumentNullException("���뵥��Ϣ�����ڣ�");
            if (applyform.HasBalanceRefund) throw new CustomException("ÿ�����뵥ֻ������һ�β���˿");
            if (applyform.Status != RefundApplyformStatus.Refunded) throw new CustomException("��������ɵ����뵥�����������˿�");
            AssociateApplyformId = applyform.Id;
            ApplyRemark = applyView.Reason;
            BalanceRefundStatus = BalanceRefundProcessStatus.AppliedForPlatform;
            AppliedTime = DateTime.Now;
            PurchaserName = applyform.PurchaserName;
            PurchaserId = applyform.PurchaserId;
            ProviderId = applyform.ProviderId;
            ProviderName = applyform.ProviderName;
            OriginalPNR = applyform.OriginalPNR;
            _applyform = applyform;
            IsInterior = applyform.IsInterior;

        }

        private void initLazyLoaders()
        {
            _applyformLoader = new LazyLoader<RefundOrScrapApplyform>(() => ApplyformQueryService.QueryRefundOrScrapApplyform(AssociateApplyformId));
            _refundBillLoader = new LazyLoader<NormalRefundBill>(() => DistributionQueryService.QueryNormalRefundBill(AssociateApplyformId));
        }

        #endregion

        /// <summary>
        /// ���뵥��
        /// </summary>
        public override decimal AssociateApplyformId { get; set; }

        /// <summary>
        /// ����״̬
        /// </summary>
        public BalanceRefundProcessStatus BalanceRefundStatus { get; set; }

        /// <summary>
        /// �������ʺ�
        /// </summary>
        //public string ProcessorAccount { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        //public string Processor { get; set; }

        /// <summary>
        /// �������ʺ�
        /// </summary>
        //public string ApplyAccount { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        //public string Applyer { get; set; }

        /// <summary>
        /// �ɹ���˾����
        /// </summary>
        //public string PurchaserName { get; set; }

        /// <summary>
        /// ԭ������Ϣ
        /// </summary>
        public override IEnumerable<Flight> OriginalFlights
        {
            get
            {
                return new List<Flight>();
            }
        }

        /// <summary>
        /// �ɹ�Id
        /// </summary>
        //public Guid PurchaserId { get; set; }

        /// <summary>
        /// ��Ʊ��Id
        /// </summary>
        //public Guid ProviderId { get; set; }

        /// <summary>
        /// ��Ʊ����˾����
        /// </summary>
        //public string ProviderName { get; set; }

        /// <summary>
        /// ����״̬
        /// </summary>
        public override ApplyformProcessStatus ProcessStatus
        {
            get
            {
                switch (BalanceRefundStatus)
                {
                    case BalanceRefundProcessStatus.AppliedForPlatform:
                        return ApplyformProcessStatus.Applied;
                    case BalanceRefundProcessStatus.AppliedForProvider:
                        case BalanceRefundProcessStatus.AgreedByProviderBusiness:
                        case BalanceRefundProcessStatus.DeniedByProviderTreasurer:
                        case BalanceRefundProcessStatus.DeniedByProviderBusiness:
                        return ApplyformProcessStatus.Processing;
                    case BalanceRefundProcessStatus.Finished:
                    case BalanceRefundProcessStatus.DenyRefund:
                        return ApplyformProcessStatus.Finished;
                    default: throw new CustomException("״̬����");
                }
            }
        }

        /// <summary>
        /// ͬ���˿����ʺ�
        /// </summary>
        //public string AgreeRefunAccount { get; set; }

        /// <summary>
        /// �������뵥��Ϣ
        /// </summary>
        public RefundOrScrapApplyform Applyform
        {
            get
            {
                if (_applyform == null)
                {
                    _applyform = _applyformLoader.QueryData();
                }
                return _applyform;
            }
            set { _applyform = value; }
        }

        /// <summary>
        /// �˿��˵�
        /// </summary>
        public NormalRefundBill RefundBill
        {
            get { return _refundBillLoader.QueryData(); }
            internal set
            {
                if (value == null) throw new ArgumentNullException("refundBill");
                _refundBillLoader.SetData(value);
            }
        }

        internal override IEnumerable<Guid> GetAppliedFlights() { return Applyform.Flights.Select(f => f.OriginalFlight.Id); }

        #region  ״̬����
        /// <summary>
        /// ƽ̨ͬ���˿�
        /// </summary>
        /// <param name="processerAccount"></param>
        /// <param name="processorName"></param>
        internal void PlatformAgreeRefund(string processerAccount, string processorName)
        {
            BalanceRefundStatus = BalanceRefundProcessStatus.AppliedForProvider;
            OperatorAccount = processerAccount;
            @Operator = processorName;
            ProcessedTime = DateTime.Now;
        }

        /// <summary>
        /// ƽ̨�ܾ�����˿�
        /// </summary>
        /// <param name="processerAccount"></param>
        /// <param name="processorName"></param>
        /// <param name="reason"> </param>
        internal void PlatformNotAgreeRefund(string processerAccount, string processorName,string reason)
        {
            BalanceRefundStatus = BalanceRefundProcessStatus.DenyRefund;
            OperatorAccount = processerAccount;
            @Operator = processorName;
            ProcessedFailedReason = reason;
            ProcessedTime = DateTime.Now;
        }

        /// <summary>
        /// ��Ʊ��ҵ��ͬ�����˿�
        /// </summary>
        /// <param name="processerAccount"></param>
        /// <param name="processorName"></param>
        internal void ProviderBusinessAgreeRefund(string processerAccount, string processorName)
        {
            BalanceRefundStatus = BalanceRefundProcessStatus.AgreedByProviderBusiness;
            OperatorAccount = processerAccount;
            @Operator = processorName;
            ProcessedTime = DateTime.Now;
        }
        
        /// <summary>
        /// ��Ʊ��ҵ��ܾ���
        /// </summary>
        /// <param name="processerAccount"></param>
        /// <param name="processorName"></param>
        internal void ProviderBusinessNotAgreeRefund(string processerAccount, string processorName,string reason)
        {
            BalanceRefundStatus = BalanceRefundProcessStatus.DeniedByProviderBusiness;
            OperatorAccount = processerAccount;
            @Operator = processorName;
            ProcessedTime = DateTime.Now;
            ProcessedFailedReason = reason;
        }

        /// <summary>
        /// ��Ʊ������ͬ���˿�
        /// </summary>
        /// <param name="processerAccount"></param>
        /// <param name="processorName"></param>
        internal void ProviderTreasurerAgreeRefund(string processerAccount, string processorName)
        {
            BalanceRefundStatus = BalanceRefundProcessStatus.Finished;
            OperatorAccount = processerAccount;
            @Operator = processorName;
            ProcessedTime = DateTime.Now;
        }

        /// <summary>
        /// ��Ʊ������ܾ��˿�
        /// </summary>
        /// <param name="processerAccount"></param>
        /// <param name="processorName"></param>
        /// <param name="reason"> </param>
        internal void ProviderTreasurerNotAgreeRefund(string processerAccount, string processorName,string reason)
        {
            BalanceRefundStatus = BalanceRefundProcessStatus.DeniedByProviderTreasurer;
            OperatorAccount = processerAccount;
            @Operator = processorName;
            ProcessedTime = DateTime.Now;
            ProcessedFailedReason = reason;
        }

        #endregion




        public override string ToString()
        {
            return "����˿�";
        }
    }
}