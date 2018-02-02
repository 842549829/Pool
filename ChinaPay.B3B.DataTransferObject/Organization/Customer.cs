namespace ChinaPay.B3B.DataTransferObject.Organization {
    using System;
    using B3B.Common.Enums;
    using ChinaPay.B3B.DataTransferObject.Common;

    public class Customer {
        public Customer()
            : this(Guid.NewGuid()) {
        }
        public Customer(Guid id) {
            this.Id = id;
        }
        public Guid Id { get; private set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Core.Sex? Sex { get; set; }
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
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 出身日期
        /// </summary>
        public DateTime? BirthDay { get; set; }
    }
}