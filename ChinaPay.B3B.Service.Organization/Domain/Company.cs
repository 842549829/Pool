using System;
using Izual.Data.Mapping;
using ChinaPay.B3B.Data;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.Service.Organization.Domain {
    using Common.Enums;

    /// <summary>
    /// 表示一个 "公司" 类型的实体。
    /// </summary>
    //[Mapping("T_Company")]
    //   public class Company : IPersistable<Company> {
    public class Company {
        /// <summary>
        /// 获取或设置 公司唯一表示
        /// </summary>
        [Identifier]
        public Guid Id { get; set; }
        /// <summary>
        /// 获取或设置公司名称。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置公司简写的名称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 获取或设置公司类型。
        /// </summary>
        public CompanyType Type { get; set; }
        /// <summary>
        /// 获取或设置公司地址
        /// </summary>
        public Address Address { get; set; }
        /// <summary>
        /// 公司电话（多个）
        /// </summary>
        public string OfficePhones { get; set; }
        /// <summary>
        /// 传真（多个）
        /// </summary>
        public string Faxes { get; set; }

        /// <summary>
        /// 获取或设置负责人
        /// </summary>
        public Contact Manager { get; set; }
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public Contact EmergencyContact { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public Contact Contact { get; set; }
        /// <summary>
        /// 获取注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime EffectTime { get; set; }
        ///// <summary>
        ///// 获取或设置有效期（天数）
        ///// </summary>
        //public int Validity { get; set; }
        /// <summary>
        /// 是否通过审核
        /// </summary>
        public bool Audited { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 是否 VIP
        /// </summary>
        public bool IsVip { get; set; }
        /// <summary>
        /// 是否 OEM
        /// </summary>
        public bool IsOem { get; set; }
        /// <summary>
        /// 工作信息
        /// </summary>
        public WorkingSetting WorkingSetting { get; set; }
        /// <summary>
        /// 工作时间
        /// </summary>
        public WorkingHours WorkingHours { get; set; }
        /// <summary>
        /// 政策设置
        /// </summary>
        public SettingPolicy PolicySetting { get; set; }
    }
}
