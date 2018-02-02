
namespace ChinaPay.B3B.DataTransferObject.Policy {
    using System;
    using B3B.Common.Enums;
    using Izual;
    using ObjectCreator = Izual.Creator;

    /// <summary>
    /// 政策基本信息
    /// </summary>
    public abstract class PolicyInfoBase {
        public PolicyInfoBase Copy() {
            var copy = ObjectCreator.Create(GetType());
            var properties = GetType().GetProperties();
            foreach(var property in properties) {
                if(property.CanRead && property.CanWrite)
                    copy.Set(property.Name, this.Get(property.Name));
            }
            return copy as PolicyInfoBase;
        }
        /// <summary>
        /// 政策编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 政策所有者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 政策类型
        /// </summary>
        public PolicyType PolicyType { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }
        /// <summary>
        /// 出发城市组
        /// </summary>
        public string Departure { get; set; }
        /// <summary>
        /// 到达城市组
        /// </summary>
        public string Arrival { get; set; }
        /// <summary>
        /// Office 号 
        /// </summary>
        public string OfficeCode { get; set; }
        /// <summary>
        /// 是否需要编码授权 
        /// </summary>
        public bool NeedAUTH { get; set; }
        /// <summary>
        /// 自定义编码
        /// </summary>
        public string CustomCode { get; set; }
        /// <summary>
        /// 行程类型
        /// </summary>
        public VoyageType VoyageType { get; set; }
        /// <summary>
        /// 去程班期起始日期
        /// </summary>
        public virtual DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程班期结束日期
        /// </summary>
        public virtual DateTime DepartureDateEnd { get; set; }
        /// <summary>
        /// 开始处理日期
        /// 即 出票开始日期 或 开始提供资源日期
        /// </summary>
        public DateTime StartProcessDate { get; set; }
        /// <summary>
        /// 排除日期
        /// </summary>
        public string DepartureDateFilter { get; set; }
        /// <summary>
        /// 班期周期限制
        /// </summary>
        public string DepartureWeekFilter { get; set; }
        /// <summary>
        /// 排除航线
        /// </summary>
        public string ExceptAirways { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public TicketType TicketType { get; set; }
        /// <summary>
        /// 舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 出票条件
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 是否贴点
        /// </summary>
        public bool MountPoint { get; set; }
        /// <summary>
        /// 是否扣点
        /// </summary>
        public bool DiscountPoint { get; set; }
        /// <summary>
        /// 去程航班过滤
        /// </summary>
        public string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 去程航班过滤类型
        /// </summary>
        public LimitType DepartureFlightsFilterType { get; set; }

        /// <summary>
        /// 是否已审核
        /// </summary>
        public bool Audited { get; set; }
        /// <summary>
        /// 政策是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 是否被挂起
        /// </summary>
        public bool Suspended { get; set; }
        /// <summary>
        /// 是否由平台挂起
        /// </summary>
        public bool SuspendByPlatform { get; set; }
        /// <summary>
        /// 是否被冻结（锁定）
        /// </summary>
        public bool Freezed { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 工作开始时间
        /// </summary>
        public Time WorkStart { get; set; }
        /// <summary>
        /// 工作结束时间
        /// </summary>
        public Time WorkEnd { get; set; }
        /// <summary>
        /// 退票开始时间
        /// </summary>
        public Time RefundStart { get; set; }
        /// <summary>
        /// 退票结束时间
        /// </summary>
        public Time RefundEnd { get; set; }

        /// <summary>
        /// 内部返点
        /// </summary>
        public decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级返点
        /// </summary>
        public decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行返点
        /// </summary>
        public decimal ProfessionCommission { get; set; }
        /// <summary>
        /// 同行
        /// </summary>
        public bool IsPeer { get; set; }
        /// <summary>
        /// 内部
        /// </summary>
        public bool IsInternal { get; set; }

        /// <summary>
        /// 飞机起飞前小时签B2B是否允许出票
        /// </summary>
        public virtual bool PrintBeforeTwoHours { get; set; }

        /// <summary>
        /// 获取出港地字串（多个）
        /// </summary>
        /// <param name="voyageIndex">第几程</param>
        public abstract string GetDeparture(int voyageIndex = 1);
        /// <summary>
        /// 获取到港地字串（多个）
        /// </summary>
        /// <param name="voyageIndex">第几程</param>
        public abstract string GetArrival(int voyageIndex = 1);
        /// <summary>
        /// 获取航班限制
        /// </summary>
        /// <param name="voyageIndex">第几程</param>
        public abstract string GetFlightNumberFilter(int voyageIndex = 1);
        /// <summary>
        /// 获取航班限制类型
        /// </summary>
        /// <param name="voyageIndex">第几程</param>
        public abstract LimitType GetFlightNumberFilterType(int voyageIndex = 1);
    }
}