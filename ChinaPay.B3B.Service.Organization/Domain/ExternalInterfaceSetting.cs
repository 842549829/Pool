using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    public class ExternalInterfaceSetting
    {
        public ExternalInterfaceSetting(Guid companyId)
        {
            this.CompanyId = companyId;
        }
        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 安全码
        /// </summary>
        public string SecurityCode { get; set; }
        /// <summary>
        /// 产品提供方确认座位成功后，会通知到设置的地址上
        /// </summary>
        public string ConfirmSuccessAddress { get; set; }
        /// <summary>
        /// 当产品方确认座位失败时，会通知到设置的地址上
        /// </summary>
        public string ConfirmFailAddress { get; set; }
        /// <summary>
        /// 当用户支付成功后，会通知到设置的地址上
        /// </summary>
        public string PaySuccessAddress { get; set; }
        /// <summary>
        /// 当出票方出票成功后，会通知到设置的地址上
        /// </summary>
        public string DrawSuccessAddress { get; set; }
        /// <summary>
        /// 当平台最终拒绝出票，取消订单时，会通知到设置的地址上
        /// </summary>
        public string RefuseAddress { get; set; }

        /// <summary>
        /// 退废票退款成功通知地址
        /// </summary>
        public string RefundSuccessAddress { get; set; }
        /// <summary>
        /// 退废票处理成功通知地址
        /// </summary>
        public string ReturnTicketSuccessAddress { get; set; }
        /// <summary>
        /// 拒绝退废票通知地址
        /// </summary>
        public string RefuseTicketAddress { get; set; }
        /// <summary>
        /// 拒绝改期退款通知地址
        /// </summary>
        public string RefundApplySuccessAddress { get; set; }
        /// <summary>
        /// 同意改期通知地址
        /// </summary>
        public string AgreedAddress { get; set; }
        /// <summary>
        /// 拒绝改期通知地址
        /// </summary>
        public string RefuseChangeAddress { get; set; }
        /// <summary>
        /// 改期支付成功通知地址
        /// </summary>
        public string ReschPaymentAddress { get; set; }
        /// <summary>
        /// 改期成功通知地址
        /// </summary>
        public string ReschedulingAddress { get; set; }
        /// <summary>
        /// 取消出票退款成功通知地址
        /// </summary>
        public string CanceldulingAddress { get; set; }
        /// <summary>
        /// 开通时间
        /// </summary>
        public DateTime? OpenTime { get; set; }
        /// <summary>
        /// 绑定使用者IP
        /// </summary>
        public string BindIP { get; set; }
        /// <summary>
        /// 用户可调用的接口
        /// </summary>
        public IEnumerable<string> InterfaceInvokeMethod { get; set; }
        /// <summary>
        /// 可得到的政策类型
        /// </summary>
        public ChinaPay.B3B.Common.Enums.PolicyType PolicyTypes { get; set; }
        public string GetPolicyTypesStr 
        {
            get
            {
                string str = "";
                if ((PolicyTypes & Common.Enums.PolicyType.Normal) == Common.Enums.PolicyType.Normal)
                    str += "普通政策/";
                if ((PolicyTypes & Common.Enums.PolicyType.Bargain) == Common.Enums.PolicyType.Bargain)
                    str += "特价政策/";
                if ((PolicyTypes & Common.Enums.PolicyType.Team) == Common.Enums.PolicyType.Team)
                    str += "团队政策/";
                if ((PolicyTypes & Common.Enums.PolicyType.Special) == Common.Enums.PolicyType.Special)
                    str += "特殊政策/";
                return str == "" ? "" : str.Substring(0, str.Length - 1);
            }
        }
    }
}
