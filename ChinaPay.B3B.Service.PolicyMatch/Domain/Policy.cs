
namespace ChinaPay.B3B.Service.PolicyMatch.Domain {
    using System;
    using Common.Enums;
    using Organization.Domain;

    public abstract class PolicyBase {
        /// <summary> 
        /// 政策编号
        /// </summary>
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 政策所有者
        /// </summary>
        public virtual Company Owner { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        public virtual string Airline { get; set; }
        /// <summary>
        /// 政策类型
        /// </summary>
        public virtual PolicyType PolicyType { get; set; }
        /// <summary>
        /// Office 号
        /// </summary>
        public virtual string OfficeCode { get; set; }
        /// <summary>
        /// 客票类型
        /// </summary>
        public virtual TicketType TickeType { get; set; }
        /// <summary>
        /// 行程类型
        /// </summary>
        public virtual VoyageType VoyageType { get; set; }
        /// <summary>
        /// 出发城市
        /// </summary>
        public virtual string Departure { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public virtual string Arrival { get; set; }

        /// <summary>
        /// 去程班期起始日期
        /// </summary>
        public DateTime DepartureDateStart { get; set; }
        /// <summary>
        /// 去程班期结束日期
        /// </summary>
        public DateTime DepartureDateEnd { get; set; }

        /// <summary>
        /// 去程班期过滤
        /// </summary>
        public virtual string DepartureDatesFilter { get; set; }
        /// <summary>
        /// 去程班期过滤类型
        /// </summary>
        public virtual DateMode DepartureDatesFilterType { get; set; }

        /// <summary>
        /// 去程航班过滤
        /// </summary>
        public virtual string DepartureFlightsFilter { get; set; }
        /// <summary>
        /// 去程航班过滤类型
        /// </summary>
        public virtual LimitType DepartureFlightsFilterType { get; set; }
        /// <summary>
        /// 排除航线
        /// </summary>
        public virtual string ExceptAirways { get; set; }

        /// <summary>
        /// 是否已审核
        /// </summary>
        public virtual bool Audited { get; set; }
        /// <summary>
        /// 是否被挂起
        /// </summary>
        public virtual bool Suspended { get; set; }
        /// <summary>
        /// 是否被冻结（锁定）
        /// </summary>
        public virtual bool Freezed { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public bool AuditTime { get; set; }

        /// <summary>
        /// 舱位
        /// </summary>
        public virtual string Berths { get; set; }

        /// <summary>
        /// 内部返点
        /// </summary>
        public virtual decimal InternalCommission { get; set; }
        /// <summary>
        /// 下级返点
        /// </summary>
        public virtual decimal SubordinateCommission { get; set; }
        /// <summary>
        /// 同行返点
        /// </summary>
        public virtual decimal ProfessionCommission { get; set; }
        /// <summary>
        /// VIP 返点
        /// </summary>
        public virtual decimal VipCommission { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string Createor { get; set; }
    }

    public class NormalPolicy : PolicyBase {
        /// <summary>
        /// 回程班期起始日期
        /// </summary>
        public DateTime? ReturnDateStart { get; set; }
        /// <summary>
        /// 回程班期结束日期
        /// </summary>
        public DateTime? ReturnDateEnd { get; set; }
        /// <summary>
        /// 回程班期过滤
        /// </summary>
        public string ReturnDatesFilter { get; set; }
        /// <summary>
        /// 回程班期过滤类型
        /// </summary>
        public DateMode ReturnDatesFilterType { get; set; }

        /// <summary>
        /// 回程航班过滤
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 回程航班过滤类型
        /// </summary>
        public LimitType ReturnFlightsFilterType { get; set; }

        /// <summary>
        /// 旅游天数
        /// </summary>
        public virtual int TravelDays { get; set; }

        /// <summary>
        /// 提前天数
        /// </summary>
        public int BeforehandDays { get; set; }
        /// <summary>
        /// 开始出票时间
        /// </summary>
        public DateTime StartPrintDate { get; set; }
    }

    public class BargainPolicy : PolicyBase {        
        /// <summary>
        /// 提前天数
        /// </summary>
        public int BeforehandDays { get; set; }
        /// <summary>
        /// 发布价格
        /// </summary>
        public virtual decimal Price { get; set; }
        /// <summary>
        /// 价格类型
        /// </summary>
        public virtual PriceType PriceType { get; set; }
    }

    public class SpecialPolicy : PolicyBase {
        /// <summary>
        /// 发布价格
        /// </summary>
        public virtual decimal Price { get; set; }
        /// <summary>
        /// 提前天数
        /// </summary>
        public int BeforehandDays { get; set; }
        /// <summary>
        /// 特殊政策类型
        /// </summary>
        public SpecialProductType Type { get; set; }
        /// <summary>
        /// 提供资源日期
        /// </summary>
        public DateTime ProvideDate { get; set; }
        /// <summary>
        /// 可提供资源数量 
        /// </summary>
        public int ResourceAmount { get; set; }

    }

    public class RoundTripPolicy : PolicyBase {
        /// <summary>
        /// 回程班期
        /// </summary>
        public string ReturnDates { get; set; }
        /// <summary>
        /// 回程班期过滤
        /// </summary>
        public string ReturnDatesFilter { get; set; }
        /// <summary>
        /// 回程班期过滤类型
        /// </summary>
        public DateMode ReturnDatesFilterType { get; set; }

        /// <summary>
        /// 回程航班过滤
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 回程航班过滤类型
        /// </summary>
        public LimitType ReturnFlightsFilterType { get; set; }

        /// <summary>
        /// 发布价格
        /// </summary>
        public virtual decimal Price { get; set; }
        /// <summary>
        /// 提前天数
        /// </summary>
        public virtual int BeforehandDays { get; set; }
        /// <summary>
        /// 旅游天数
        /// </summary>
        public virtual int TravelDays { get; set; }

    }
}
