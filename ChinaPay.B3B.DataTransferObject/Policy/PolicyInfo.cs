
namespace ChinaPay.B3B.DataTransferObject.Policy
{
    using System;
    using System.Collections.Generic;
    using B3B.Common.Enums;

    /// <summary>
    /// 多联程适用接口
    /// </summary>
    public interface IMultiConjunctionSuitable
    {
        /// <summary>
        /// 多联程是否适用
        /// </summary>
        bool MultiConjunctionSuitable { get; set; }
    }

    public interface IRoundTrip
    {
        /// <summary>
        /// 回程航班过滤
        /// </summary>
        string ReturnFlightsFilter { get; }
        /// <summary>
        /// 回程航班过滤类型
        /// </summary>
        LimitType? ReturnFlightsFilterType { get; }
    }

    public interface IHasRegulation
    {
        /// <summary>
        /// 作废规定
        /// </summary>
        string InvalidRegulation { get; }
        /// <summary>
        /// 改签规定
        /// </summary>
        string ChangeRegulation { get; }
        /// <summary>
        /// 签转规定
        /// </summary>
        string EndorseRegulation { get; }
        /// <summary>
        /// 退票规定
        /// </summary>
        string RefundRegulation { get; }
    }
    public interface IGeneralPolicy
    {
        /// <summary>
        /// 中转城市
        /// </summary>
        string Transit { get; set; }
        /// <summary>
        /// 是否需要换编码
        /// </summary>
        bool ChangePNR { get; }
        /// <summary>
        /// 是否自动出票
        /// </summary>
        bool AutoPrint { get; }
    }

    /// <summary>
    /// 提前天数接口
    /// </summary>
    public interface IHasBeforehandDays
    {
        /// <summary>
        /// 最少提前天数
        /// </summary>
        int BeforehandDays { get; set; }
        /// <summary>
        /// 最大提前天数
        /// </summary>
        int MaxBeforehandDays { get; set; }
    }

    public class NormalPolicyInfo : PolicyInfoBase, IRoundTrip, IGeneralPolicy, IMultiConjunctionSuitable
    {
        #region IGernalPolicy
        /// <summary>
        /// 中转城市
        /// </summary>
        public string Transit { get; set; }
        /// <summary>
        /// 是否换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        /// <summary>
        /// 是否自动出票
        /// </summary>
        public bool AutoPrint { get; set; }
        #endregion

        #region IRoundTrip
        /// <summary>
        /// 回程航班过滤
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 回程航班过滤类型
        /// </summary>
        public LimitType? ReturnFlightsFilterType { get; set; }
        #endregion

        /// <summary>
        /// 多段联程是否适用
        /// </summary>
        public bool MultiConjunctionSuitable { get; set; }
        /// <summary>
        /// VIP返点
        /// </summary>
        public decimal VipCommission { get; set; }
        /// <summary>
        /// 适用于往返降舱
        /// </summary>
        public bool SuitReduce { get; set; }

        /// <summary>
        /// 飞机起飞前小时签B2B是否允许出票
        /// </summary>
        public override bool PrintBeforeTwoHours
        {
            get;
            set;
        }

        private void CheckVoyage(int voyageIndex)
        {
            if (voyageIndex > 1 && VoyageType == VoyageType.OneWay || voyageIndex > 2)
                throw new InvalidOperationException(string.Format("缺少第 {0} 航段的政策信息。", voyageIndex));
        }

        public override string GetDeparture(int voyageIndex = 1)
        {
            CheckVoyage(voyageIndex);
            if (VoyageType == VoyageType.TransitWay)
                return voyageIndex == 1 ? Departure : Transit;
            else
                return voyageIndex == 1 ? Departure : Arrival;
        }

        public override string GetArrival(int voyageIndex = 1)
        {
            CheckVoyage(voyageIndex);
            if (VoyageType == VoyageType.TransitWay)
                return voyageIndex == 1 ? Transit : Arrival;
            else
                return voyageIndex == 1 ? Arrival : Departure;
        }

        public override string GetFlightNumberFilter(int voyageIndex = 1)
        {
            CheckVoyage(voyageIndex);
            return voyageIndex == 1 ? DepartureFlightsFilter : ReturnFlightsFilter;
        }

        public override LimitType GetFlightNumberFilterType(int voyageIndex = 1)
        {
            CheckVoyage(voyageIndex);
            if (voyageIndex == 1)
                return DepartureFlightsFilterType;

            if (ReturnFlightsFilterType == null)
                throw new InvalidOperationException(string.Format("缺少第 {0} 航段航班过滤类型。", voyageIndex));
            return ReturnFlightsFilterType.Value;
        }
    }
    /// <summary>
    /// 团队政策
    /// </summary>
    public class TeamPolicyInfo : PolicyInfoBase, IRoundTrip, IGeneralPolicy, IMultiConjunctionSuitable
    {
        #region IGernalPolicy
        /// <summary>
        /// 中转城市
        /// </summary>
        public virtual string Transit { get; set; }
        /// <summary>
        /// 是否换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        /// <summary>
        /// 是否自动出票
        /// </summary>
        public bool AutoPrint { get; set; }
        #endregion

        #region IRoundTrip
        /// <summary>
        /// 回程航班过滤
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 回程航班过滤类型
        /// </summary>
        public LimitType? ReturnFlightsFilterType { get; set; }
        #endregion

        /// <summary>
        /// 多段联程是否适用
        /// </summary>
        public bool MultiConjunctionSuitable { get; set; }
        /// <summary>
        /// 适用于往返降舱
        /// </summary>
        public bool SuitReduce { get; set; }

        /// <summary>
        /// 飞机起飞前小时签B2B是否允许出票
        /// </summary>
        public override bool PrintBeforeTwoHours
        {
            get;
            set;
        }

        private void CheckVoyage(int voyageIndex)
        {
            if (voyageIndex > 1 && VoyageType == VoyageType.OneWay || voyageIndex > 2)
                throw new InvalidOperationException(string.Format("缺少第 {0} 航段的政策信息。", voyageIndex));
        }

        public override string GetDeparture(int voyageIndex = 1)
        {
            CheckVoyage(voyageIndex);
            if (VoyageType == VoyageType.TransitWay)
                return voyageIndex == 1 ? Departure : Transit;
            else
                return voyageIndex == 1 ? Departure : Arrival;
        }

        public override string GetArrival(int voyageIndex = 1)
        {
            CheckVoyage(voyageIndex);
            if (VoyageType == VoyageType.TransitWay)
                return voyageIndex == 1 ? Transit : Arrival;
            else
                return voyageIndex == 1 ? Arrival : Departure;
        }

        public override string GetFlightNumberFilter(int voyageIndex = 1)
        {
            CheckVoyage(voyageIndex);
            return voyageIndex == 1 ? DepartureFlightsFilter : ReturnFlightsFilter;
        }

        public override LimitType GetFlightNumberFilterType(int voyageIndex = 1)
        {
            CheckVoyage(voyageIndex);
            if (voyageIndex == 1)
                return DepartureFlightsFilterType;

            if (ReturnFlightsFilterType == null)
                throw new InvalidOperationException(string.Format("缺少第 {0} 航段航班过滤类型。", voyageIndex));
            return ReturnFlightsFilterType.Value;
        }
    }
    /// <summary>
    /// 特价政策
    /// </summary>
    public class BargainPolicyInfo : PolicyInfoBase, IRoundTrip, IHasRegulation, IGeneralPolicy, IHasBeforehandDays, IMultiConjunctionSuitable
    {
        #region IGernalPolicy
        /// <summary>
        /// 中转城市
        /// </summary>
        public string Transit { get; set; }
        /// <summary>
        /// 是否换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        public bool AutoPrint { get { return false; } }
        #endregion

        #region IRoundTrip
        /// <summary>
        /// 回程航班过滤
        /// </summary>
        public string ReturnFlightsFilter { get; set; }
        /// <summary>
        /// 回程航班过滤类型
        /// </summary>
        public LimitType? ReturnFlightsFilterType { get; set; }
        #endregion

        #region IHasBeforehandDays
        /// <summary>
        /// 最小提前天数
        /// </summary>
        public int BeforehandDays { get; set; }

        /// <summary>
        /// 最大提前天数
        /// </summary>
        public int MaxBeforehandDays { get; set; }
        #endregion
        /// <summary>
        /// 多段联程是否适用
        /// </summary>
        public bool MultiConjunctionSuitable { get; set; }
        /// <summary>
        /// 发布价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 价格类型
        /// </summary>
        public PriceType PriceType { get; set; }

        #region IHasRegulation
        /// <summary>
        /// 作废规定
        /// </summary>
        public string InvalidRegulation { get; set; }
        /// <summary>
        /// 改签规定
        /// </summary>
        public string ChangeRegulation { get; set; }
        /// <summary>
        /// 签转规定
        /// </summary>
        public string EndorseRegulation { get; set; }
        /// <summary>
        /// 退票规定
        /// </summary>
        public string RefundRegulation { get; set; }
        #endregion

        /// <summary>
        /// 飞机起飞前小时签B2B是否允许出票
        /// </summary>
        public override bool PrintBeforeTwoHours
        {
            get;
            set;
        }

        /// <summary>
        /// 出行天数
        /// </summary>
        public int TravelDays { get; set; }

        private void CheckVoyage(int voyageIndex)
        {
            if (voyageIndex > 1 && VoyageType == VoyageType.OneWay || voyageIndex > 2) throw new InvalidOperationException(string.Format("缺少第 {0} 航段政策信息。", voyageIndex));
        }

        public override string GetDeparture(int voyageIndex = 1)
        {
            CheckVoyage(voyageIndex);
            if (VoyageType == VoyageType.TransitWay)
                return voyageIndex == 1 ? Departure : Transit;
            else
                return voyageIndex == 1 ? Departure : Arrival;
        }

        public override string GetArrival(int voyageIndex = 1)
        {
            CheckVoyage(voyageIndex);
            if (VoyageType == VoyageType.TransitWay)
                return voyageIndex == 1 ? Transit : Arrival;
            else
                return voyageIndex == 1 ? Arrival : Departure;
        }

        public override string GetFlightNumberFilter(int voyageIndex = 1)
        {
            CheckVoyage(voyageIndex);
            return voyageIndex == 1 ? DepartureFlightsFilter : ReturnFlightsFilter;
        }

        public override LimitType GetFlightNumberFilterType(int voyageIndex = 1)
        {
            CheckVoyage(voyageIndex);
            if (voyageIndex == 1)
                return DepartureFlightsFilterType;

            if (ReturnFlightsFilterType == null)
                throw new InvalidOperationException(string.Format("缺少第 {0} 航段航班过滤类型。", voyageIndex));
            return ReturnFlightsFilterType.Value;
        }
    }

    public class SpecialPolicyInfo : PolicyInfoBase, IHasRegulation, IHasBeforehandDays
    {
        #region IHasBeforehandDays
        /// <summary>
        /// 最小提前天数
        /// </summary>
        public int BeforehandDays { get; set; }

        /// <summary>
        /// 最大提前天数
        /// </summary>
        public int MaxBeforehandDays { get; set; }
        #endregion

        #region IHasRegulation
        /// <summary>
        /// 作废规定
        /// </summary>
        public string InvalidRegulation { get; set; }
        /// <summary>
        /// 改签规定
        /// </summary>
        public string ChangeRegulation { get; set; }
        /// <summary>
        /// 签转规定
        /// </summary>
        public string EndorseRegulation { get; set; }
        /// <summary>
        /// 退票规定
        /// </summary>
        public string RefundRegulation { get; set; }
        #endregion

        /// <summary>
        /// 是否是有位出票（false是无位，代表true有位）
        /// </summary>
        public bool IsSeat { get; set; }
        /// <summary>
        /// 特殊政策类型
        /// </summary>
        public SpecialProductType Type { get; set; }
        /// <summary>
        /// 可提供资源数量 
        /// </summary>
        public int ResourceAmount { get; set; }
        /// <summary>
        /// 是否需要确认资源
        /// </summary>
        public bool ConfirmResource { get; set; }
        /// <summary>
        /// 是否已通过平台审核
        /// </summary>
        public bool PlatformAudited { get; set; }
        /// <summary>
        /// 产品价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 价格类型
        /// </summary>
        public PriceType PriceType { get; set; }

        /// <summary>
        /// 飞机起飞前小时签B2B是否允许出票
        /// </summary>
        public override bool PrintBeforeTwoHours
        {
            get;
            set;
        }

        /// <summary>
        /// 是否黑屏同步
        /// </summary>
        public bool SynBlackScreen { get; set; }

        /// <summary>
        /// 低价出票类型
        /// </summary>
        public LowNoType LowNoType { get; set; }
        /// <summary>
        /// 价格限制的上限（不包含）
        /// </summary>
        public decimal LowNoMaxPrice { get; set; }
        /// <summary>
        /// 价格限制的下限（包含）
        /// </summary>
        public decimal LowNoMinPrice { get; set; }

        public bool IsBargainBerths { get; set; }

        public override string GetDeparture(int voyageIndex = 1)
        {
            return Departure;
        }

        public override string GetArrival(int voyageIndex = 1)
        {
            return Arrival;
        }

        public override string GetFlightNumberFilter(int voyageIndex = 1)
        {
            return DepartureFlightsFilter;
        }

        public override LimitType GetFlightNumberFilterType(int voyageIndex = 1)
        {
            return DepartureFlightsFilterType;
        }
    }

    /// <summary>
    /// 缺口程政策
    /// </summary>
    public class NotchPolicyInfo : PolicyInfoBase
    {
        /// <summary>
        /// 是否换编码出票
        /// </summary>
        public bool ChangePNR { get; set; }
        
        /// <summary>
        /// 飞机起飞前小时签B2B是否允许出票
        /// </summary>
        public override bool PrintBeforeTwoHours
        {
            get;
            set;
        }

        public override string GetDeparture(int voyageIndex = 1)
        {
            throw new NotImplementedException();
        }

        public override string GetArrival(int voyageIndex = 1)
        {
            throw new NotImplementedException();
        }

        public override string GetFlightNumberFilter(int voyageIndex = 1)
        {
            throw new NotImplementedException();
        }

        public override LimitType GetFlightNumberFilterType(int voyageIndex = 1)
        {
            throw new NotImplementedException();
        }
    }
}