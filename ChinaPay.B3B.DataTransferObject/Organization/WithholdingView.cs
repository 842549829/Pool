using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
    /// <summary>
    /// 代扣信息
    /// </summary>
    public class WithholdingView
    {
        public string AccountNo { get; set; }
        public WithholdingAccountType AccountType { get; set; }
        public DateTime Time { get; set; }
        public WithholdingProtocolStatus Status { get; set; }
        public decimal Amount { get; set; }
        public Guid Company { get; set; }
    }
}
