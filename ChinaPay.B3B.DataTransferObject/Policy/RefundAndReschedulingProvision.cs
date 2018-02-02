namespace ChinaPay.B3B.DataTransferObject.Policy
{
    /// <summary>
    /// 退改签规定
    /// </summary>
    public class RefundAndReschedulingProvision
    {
        public RefundAndReschedulingProvision(string refund, string scrap, string alteration, string transfer)
        {
            this.Refund = refund;
            this.Scrap = scrap;
            this.Alteration = alteration;
            this.Transfer = transfer;
        }
        public RefundAndReschedulingProvision()
        {

        }
        /// <summary>
        /// 退票
        /// </summary>
        public string Refund
        {
            get;
            set;
        }
        /// <summary>
        /// 废票
        /// </summary>
        public string Scrap
        {
            get;
            set;
        }
        /// <summary>
        /// 改签
        /// </summary>
        public string Alteration
        {
            get;
            set;
        }
        /// <summary>
        /// 签转
        /// </summary>
        public string Transfer
        {
            get;
            set;
        }
    }
}
