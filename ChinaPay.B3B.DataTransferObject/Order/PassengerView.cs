using System;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.DataTransferObject.Order {
    using B3B.Common.Enums;

    public class PassengerView {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 乘机人类型
        /// </summary>
        public PassengerType PassengerType { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public CredentialsType CredentialsType { get; set; }
        /// <summary>
        /// 证件号
        /// </summary>
        public string Credentials { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 出身日期
        /// </summary>
        public DateTime? BirthDay{get;set;}
    }
}