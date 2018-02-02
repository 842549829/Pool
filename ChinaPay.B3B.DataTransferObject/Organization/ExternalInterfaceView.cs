using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.DataTransferObject.Organization
{
    public class ExternalInterfaceView
    {
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
        /// 是否开通外接口
        /// </summary>
        public bool IsOpenExternalInterface { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string OperatorAccount { get; set; }
        /// <summary>
        /// 可调用方法
        /// </summary>
        public IEnumerable<string> InterfaceInvokeMethod { get; set; }
        /// <summary>
        /// 绑定使用者IP
        /// </summary>
        public string BindIP { get; set; }
        /// <summary>
        /// 取消出票退款成功
        /// </summary>
        public string CanceldulingAddress { get; set; }
        /// <summary>
        /// 拒绝改期退款成功
        /// </summary>
        public string RefundApplySuccessAddress { get; set; }
        /// <summary>
        /// 可使用的政策类型
        /// </summary>
        public ChinaPay.B3B.Common.Enums.PolicyType PolicyTypes { get; set; }
        //public override string ToString()
        //{
        //    string.Format(@"公司Id:{0},安全码:{1},确认成功通知地址:{2},确认失败通知地址:{3},支付成功通知地址:{4},出票成功通知地址:{5},拒绝出票通知地址:{6},同意改期通知地址:{7}, 退废票退款成功通知地址:{8},退废票处理成功通知地址:{9},改期成功通知地址:{10},拒绝退废票通知地址:{11},改期支付成功通知地址:{12},拒绝改期通知地址:{13},取消出票退款成功:{14},取消出票退款成功:{15},开通时间:{16},可调用方法:{17},指定IP：{18}",
        //                              CompanyId, SecurityCode, ConfirmSuccessAddress, ConfirmFailAddress, PaySuccessAddress,
        //                              DrawSuccessAddress, RefuseAddress, AgreedAddress, RefundSuccessAddress, ReturnTicketSuccessAddress,
        //                              ReschedulingAddress, RefuseTicketAddress, ReschPaymentAddress, RefuseChangeAddress,
        //                              CanceldulingAddress, RefundApplySuccessAddress, OpenTime, InterfaceInvokeMethod.Join(","), BindIP);
        //    return base.ToString();
        //}


    }
}
