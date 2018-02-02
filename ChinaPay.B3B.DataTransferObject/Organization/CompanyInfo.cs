namespace ChinaPay.B3B.DataTransferObject.Organization
{
    using System;
    using B3B.Common.Enums;

    /// <summary>
    /// 公司信息
    /// </summary>
    public class CompanyInfo
    {
        /// <summary>
        /// 公司 Id
        /// </summary>
        public Guid CompanyId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPassword { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// 使用期限开始时间
        /// </summary>
        public DateTime? PeriodStartOfUse { get; set; }
        /// <summary>
        /// 使用期限结束时间
        /// </summary>
        public DateTime? PeriodEndOfUse { get; set; }
        /// <summary>
        /// <para>公司类型 </para>
        /// <para></para>
        /// </summary>
        public CompanyType CompanyType { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 缩写名称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 公司电话
        /// </summary>
        public string OfficePhones { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Faxes { get; set; }
        /// <summary>
        /// 组织机构代码
        /// </summary>
        public string OrginationCode { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string CertNo { get; set; }
        /// <summary>
        /// 所在地省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 所在地城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 所在地区县
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string ManagerName { get; set; }
        /// <summary>
        /// 负责人手机
        /// </summary>
        public string ManagerCellphone { get; set; }
        /// <summary>
        /// 负责人 Email
        /// </summary>
        public string ManagerEmail { get; set; }
        /// <summary>
        /// 负责人 MSN
        /// </summary>
        public string ManagerMsn { get; set; }
        /// <summary>
        /// 负责人 QQ
        /// </summary>
        public string ManagerQQ { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string EmergencyContact { get; set; }
        /// <summary>
        /// 紧急电话
        /// </summary>
        public string EmergencyCall { get; set; }
        /// <summary>
        /// 账户类型
        /// </summary>
        public AccountBaseType AccountType { get; set; }
        /// <summary>
        /// 推广人
        /// </summary>
        public string OperatorAccount { get; set; }

        /// <summary>
        /// 当前登陆公司是否使用自定义编号、订单提醒服务端使用
        /// </summary>
        public bool CustomNO_On
        {
            get;
            set;
        }
    }
}
