using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.DataTransferObject.Foundation;
using ChinaPay.Utility;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    /// <summary>
    /// 舱位
    /// </summary>
    [System.Serializable]
    public abstract class Bunk {
        protected Bunk()
            : this(Guid.NewGuid()) {
        }
        protected Bunk(Guid id) {
            this.Id = id;
        }
        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 航空公司代码
        /// </summary>
        public UpperString AirlineCode {
            get;
            internal set;
        }
        /// <summary>
        /// 航空公司
        /// </summary>
        public Airline Airline {
            get {
                return FoundationService.QueryAirline(AirlineCode);
            }
        }
        /// <summary>
        /// 航班开始日期
        /// </summary>
        public DateTime FlightBeginDate {
            get;
            internal set;
        }
        /// <summary>
        /// 航班截止日期
        /// </summary>
        public DateTime? FlightEndDate {
            get;
            internal set;
        }
        /// <summary>
        /// 出票日期
        /// </summary>
        public DateTime ETDZDate {
            get;
            internal set;
        }
        /// <summary>
        /// 代码
        /// </summary>
        public UpperString Code {
            get;
            internal set;
        }
        /// <summary>
        /// EI项
        /// </summary>
        public string EI {
            get {
                return string.Format("退票规定:{0} 改签规定:{1} 签转规定:{2} 备注:{3}", RefundRegulation, ChangeRegulation, EndorseRegulation, Remarks);
            }
        }
        /// <summary>
        /// 退票条件
        /// </summary>
        public string RefundRegulation { get; set; }
        /// <summary>
        /// 改期条件
        /// </summary>
        public string ChangeRegulation { get; set; }
        /// <summary>
        /// 改签条件
        /// </summary>
        public string EndorseRegulation { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Valid {
            get;
            internal set;
        }

        private int? _level;
        /// <summary>
        /// 排序级别
        /// </summary>
        public int Level {
            get {
                if(_level == null) {
                    _level = GetLevel();
                    if(!this.AirlineCode.IsNullOrEmpty()) _level += 1;
                }
                return _level.Value;
            }
        }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime ModifyTime {
            get;
            internal set;
        }
        /// <summary>
        /// 舱位类型
        /// </summary>
        public abstract BunkType Type {
            get;
        }
        protected virtual int GetLevel() {
            return 0;
        }
        /// <summary>
        /// 适用行程
        /// </summary>
        public VoyageTypeValue VoyageType { get; internal set; }
        /// <summary>
        /// 旅客类型
        /// </summary>
        public PassengerTypeValue PassengerType { get; internal set; }
        /// <summary>
        /// 旅行类型
        /// </summary>
        public TravelTypeValue TravelType { get; internal set; }

        public override string ToString() {
            return string.Format("航空公司:{0} 航班开始日期:{1} 航班结束日期:{2} 出票日期:{3} 退票规定:{4} 改签规定:{5} 签转规定:{6} 备注:{7} 状态:{8},舱位Id:{9},舱位类型:{10},使用行程:{11},旅客类型:{12},旅行类型:{13}",
                AirlineCode.Value, FlightBeginDate.ToShortDateString(), FlightEndDate.HasValue ? FlightEndDate.Value.ToShortDateString() : string.Empty,
                ETDZDate.ToShortDateString(), RefundRegulation, ChangeRegulation, EndorseRegulation, Remarks, Valid, Id,  Type.GetDescription(), 
                    string.Join("\\",from VoyageTypeValue v in Enum.GetValues(typeof(VoyageTypeValue))
                                     where (v&VoyageType)==v
                                     select v.GetDescription())
                ,
                string.Join("\\", from PassengerTypeValue p in Enum.GetValues(typeof(PassengerTypeValue))
                                  where (PassengerType & p) == p
                                  select p.GetDescription())
            , string.Join("\\", from TravelTypeValue t in Enum.GetValues(typeof(TravelTypeValue))
                                where (TravelType & t) == t
                                select t.GetDescription()));
        }

        
        internal static Bunk CreateBunk(Guid id, BunkView bunkView) {
            var bunk = CreateBunk(bunkView);
            bunk.Id = id;
            return bunk;
        }
        internal static Bunk CreateBunk(BunkView bunkView) {
            if(null == bunkView) throw new ArgumentNullException("bunkView");
            bunkView.Validate();
            Bunk bunk = null;
            if(bunkView is GeneralBunkView) {
                if(bunkView is FirstBusinessBunkView) {
                    bunk = new FirstBusinessBunk {
                        Description = StringUtility.Trim(((FirstBusinessBunkView)bunkView).Description)
                    };
                } else {
                    bunk = new EconomicBunk();
                }
                var generalBunkView = bunkView as GeneralBunkView;
                var generalBunk = bunk as GeneralBunk;
                generalBunk.DepartureCode = StringUtility.Trim(generalBunkView.Departure) ?? string.Empty;
                generalBunk.ArrivalCode = StringUtility.Trim(generalBunkView.Arrival) ?? string.Empty;
                generalBunk.Discount = generalBunkView.Discount;
                foreach(var item in generalBunkView.Extended) {
                    generalBunk.AddExtended(new ExtendedWithDiscountBunk(StringUtility.Trim(item.Code), item.Discount));
                }
            } else if(bunkView is PromotionBunkView) {
                var promotionBunkView = bunkView as PromotionBunkView;
                bunk = new PromotionBunk {
                    Description = StringUtility.Trim(promotionBunkView.Description)
                };
                var promotionBunk = bunk as PromotionBunk;
                foreach(var item in promotionBunkView.Extended) {
                    promotionBunk.AddExtended(StringUtility.Trim(item));
                }
            } else if(bunkView is ProductionBunkView) {
                bunk = new ProductionBunk();
            } else if(bunkView is TransferBunkView) {
                bunk = new TransferBunk();
            } else if(bunkView is FreeBunkView) {
                bunk = new FreeBunk {
                    Description = StringUtility.Trim((bunkView as FreeBunkView).Description)
                };
            } else if(bunkView is TeamBunkView) {
                bunk = new TeamBunk();
            } else {
                throw new NotSupportedException("未知舱位类型");
            }
            bunk.AirlineCode = StringUtility.Trim(bunkView.Airline);
            bunk.Code = StringUtility.Trim(bunkView.Code);
            bunk.ETDZDate = bunkView.ETDZDate;
            bunk.FlightBeginDate = bunkView.FlightBeginDate;
            bunk.FlightEndDate = bunkView.FlightEndDate;
            //bunk.EI = StringUtility.Trim(bunkView.EI);
            bunk.RefundRegulation = StringUtility.Trim(bunkView.RefundRegulation);
            bunk.ChangeRegulation = StringUtility.Trim(bunkView.ChangeRegulation);
            bunk.EndorseRegulation = StringUtility.Trim(bunkView.EndorseRegulation);
            bunk.Remarks = StringUtility.Trim(bunkView.Remarks);
            bunk.Valid = bunkView.Valid;
            bunk.ModifyTime = DateTime.Now;
            bunk.VoyageType = bunkView.VoyageType;
            bunk.TravelType = bunkView.TravelType;
            bunk.PassengerType = bunkView.PassengerType;
            return bunk;
        }
    }
    /// <summary>
    /// 明折明扣舱位
    /// </summary>
    [System.Serializable]
    public abstract class GeneralBunk : Bunk {
        protected GeneralBunk()
            : base() {
        }
        protected GeneralBunk(Guid id)
            : base(id) {
        }
        /// <summary>
        /// 出发机场代码
        /// </summary>
        public UpperString DepartureCode {
            get;
            internal set;
        }
        /// <summary>
        /// 出发
        /// </summary>
        public Airport Departure {
            get {
                return FoundationService.QueryAirport(DepartureCode);
            }
        }
        /// <summary>
        /// 到达机场代码
        /// </summary>
        public UpperString ArrivalCode {
            get;
            internal set;
        }
        /// <summary>
        /// 到达
        /// </summary>
        public Airport Arrival {
            get {
                return FoundationService.QueryAirport(ArrivalCode);
            }
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount {
            get;
            internal set;
        }
        List<ExtendedWithDiscountBunk> _extended = new List<ExtendedWithDiscountBunk>();
        /// <summary>
        /// 扩展子舱位
        /// </summary>
        public IEnumerable<ExtendedWithDiscountBunk> Extended {
            get {
                return _extended.AsReadOnly();
            }
        }
        internal void AddExtended(ExtendedWithDiscountBunk item) {
            if(null != item) {
                if(_extended.Exists(data => data.Code.Value == item.Code.Value)) {
                    throw new Core.Exception.RepeatedItemException("不能添加重复的子舱位代码");
                }
                _extended.Add(item);
            }
        }
        public override string ToString() {
            return string.Format("{0} 出发机场:{1} 到达机场:{2} 舱位:{3} 子舱位:{4},折扣:{5}",
                base.ToString(), DepartureCode.Value, ArrivalCode.Value, Code.Value + "|" + Discount.ToString(), getExtendedInfo(),Discount.ToString());
        }
        string getExtendedInfo() {
            return _extended.Join(",", item => item.ToString());
        }

        protected override int GetLevel() {
            var result = 0;
            if(!this.DepartureCode.IsNullOrEmpty()) result += 1;
            if(!this.ArrivalCode.IsNullOrEmpty()) result += 1;
            return result;
        }
        public decimal GetDiscount(string code) {
            if(string.Compare(Code.Value, code, StringComparison.OrdinalIgnoreCase) == 0)
                return Discount;
            foreach(var extend in _extended.Where(extend => string.Compare(extend.Code.Value, code, StringComparison.OrdinalIgnoreCase) == 0)) {
                return extend.Discount;
            }
            return -1;
        }
    }
    [System.Serializable]
    public class ExtendedWithDiscountBunk {
        internal ExtendedWithDiscountBunk(UpperString code, decimal discount) {
            this.Code = code;
            this.Discount = discount;
        }
        /// <summary>
        /// 代码
        /// </summary>
        public UpperString Code {
            get;
            private set;
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount {
            get;
            internal set;
        }
        public override string ToString() {
            return Code.Value + "|" + Discount;
        }
    }
    /// <summary>
    /// 经济舱
    /// </summary>
    [System.Serializable]
    public class EconomicBunk : GeneralBunk {
        internal EconomicBunk()
            : base() {
        }
        internal EconomicBunk(Guid id)
            : base(id) {
        }
        public override BunkType Type {
            get {
                return BunkType.Economic;
            }
        }
    }
    /// <summary>
    /// 头等公务舱
    /// </summary>
    [System.Serializable]
    public class FirstBusinessBunk : GeneralBunk {
        internal FirstBusinessBunk()
            : base() {
        }
        internal FirstBusinessBunk(Guid id)
            : base(id) {
        }
        public override BunkType Type {
            get {
                return BunkType.FirstOrBusiness;
            }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description {
            get;
            internal set;
        }
        public override string ToString() {
            return base.ToString() + " 描述:" + Description;
        }
    }
    /// <summary>
    /// 特价舱
    /// </summary>
    [System.Serializable]
    public class PromotionBunk : Bunk {
        internal PromotionBunk()
            : base() {
        }
        internal PromotionBunk(Guid id)
            : base(id) {
        }
        public override BunkType Type {
            get {
                return BunkType.Promotion;
            }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description {
            get;
            internal set;
        }
        List<string> _extended = new List<string>();
        /// <summary>
        /// 扩展子舱位
        /// </summary>
        public IEnumerable<string> Extended {
            get {
                return _extended.AsReadOnly();
            }
        }
        internal void AddExtended(UpperString item) {
            if(!item.IsNullOrEmpty()) {
                if(_extended.Contains(item.Value)) {
                    throw new Core.Exception.RepeatedItemException("不能添加重复的子舱位代码");
                }
                _extended.Add(item.Value);
            }
        }
        public override string ToString() {
            return string.Format("{0} 子舱位:{1} 描述:{2}", base.ToString(), getExtendedInfo(), Description);
        }
        string getExtendedInfo() {
            return _extended.Join(",");
        }
    }
    /// <summary>
    /// 往返产品舱
    /// </summary>
    [System.Serializable]
    public class ProductionBunk : Bunk {
        internal ProductionBunk()
            : base() {
        }
        internal ProductionBunk(Guid id)
            : base(id) {
        }
        public override BunkType Type {
            get {
                return BunkType.Production;
            }
        }
    }
    /// <summary>
    /// 中转联程舱
    /// </summary>
    [System.Serializable]
    public class TransferBunk : Bunk {
        internal TransferBunk()
            : base() {
        }
        internal TransferBunk(Guid id)
            : base(id) {
        }
        public override BunkType Type {
            get {
                return BunkType.Transfer;
            }
        }
    }
    [System.Serializable]
    public class FreeBunk : Bunk {
        internal FreeBunk()
            : base() {
        }
        internal FreeBunk(Guid id)
            : base(id) {
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description {
            get;
            internal set;
        }

        public override BunkType Type {
            get {
                return BunkType.Free;
            }
        }
        public override string ToString() {
            return base.ToString() + " 描述:" + Description;
        }
    }
    [System.Serializable]
    public class TeamBunk : Bunk {
        internal TeamBunk()
            : base() {
        }
        internal TeamBunk(Guid id)
            : base(id) {
        }
        public override BunkType Type {
            get {
                return BunkType.Team;
            }
        }
    }
}