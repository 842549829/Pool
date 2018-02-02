using System;

namespace ChinaPay.B3B.Service.Distribution.Domain.Bill {
    public class PlatformBill<TRoleBill, TRoleBillSource, TDetailBill>
        where TRoleBillSource : RoleBillSource<TDetailBill>
        where TRoleBill : RoleBill<TRoleBillSource, TDetailBill>
        where TDetailBill : DetailBill {

        private string _account = null;
        public string Account {
            get { return _account ?? (_account = TotalAmount < 0 ? SystemManagement.SystemParamService.PlatformPayoutAccount : SystemManagement.SystemParamService.PlatformIncomeAccount); }
            internal set {
                _account = value;
            }
        }
        public decimal TotalAmount {
            get {
                var deductionProfit = this.Deduction == null ? 0 : this.Deduction.Source.Anticipation;
                return this.TradeFee + deductionProfit + this.Premium;
            }
        }
        public decimal ProfitAmount {
            get { return TradeFee + Premium; }
        }
        public decimal TradeFee {
            get;
            internal set;
        }
        public decimal Premium {
            get;
            internal set;
        }
        public bool Success {
            get;
            internal set;
        }
        public TRoleBill Deduction {
            get;
            internal set;
        }

        internal void TradeSuccess(DateTime royaltyTime) {
            if(Deduction != null) {
                Deduction.TradeSuccess(royaltyTime);
            }
            Success = true;
        }
    }
}