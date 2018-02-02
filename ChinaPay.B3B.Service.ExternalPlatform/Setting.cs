using System;

namespace ChinaPay.B3B.Service.ExternalPlatform {
    public class Setting {
        /// <summary>
        /// 平台
        /// </summary>
        public ChinaPay.B3B.Common.Enums.PlatformType Platform { get; set; }
        /// <summary>
        /// 扣点
        /// </summary>
        public decimal Deduct { get; set; }
        /// <summary>
        /// 出票方
        /// </summary>
        public Guid Provider { get; set; }
        /// <summary>
        /// 返点差
        /// 外平台返点 - 本平台返点
        /// 如果上述差值小于该值，则政策不适用
        /// </summary>
        public decimal RebateBalance { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 自动支付方式
        /// </summary>
        public ChinaPay.B3B.DataTransferObject.Common.PayInterface[] PayInterface { get; set; }
        /// <summary>
        /// 出票方账号
        /// </summary>
        public string ProviderAccount { get; set; }
        /// <summary>
        /// 自动支付方式文字
        /// </summary>
        public string StrPayInterface { get; set; }
    }
}