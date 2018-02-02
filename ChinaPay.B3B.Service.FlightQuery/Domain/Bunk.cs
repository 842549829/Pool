using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.FlightQuery.Domain {
    public abstract class Bunk {
        internal Bunk(string code) {
            this.Code = code;
        }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code {
            get;
            private set;
        }
        /// <summary>
        /// 剩余座位数
        /// </summary>
        public int SeatCount {
            get;
            internal set;
        }
        /// <summary>
        /// EI项
        /// </summary>
        public string EI {
            get;
            internal set;
        }
        /// <summary>
        /// 退票条件
        /// </summary>
        public string RefundRegulation { get; internal set; }
        /// <summary>
        /// 改期条件
        /// </summary>
        public string ChangeRegulation { get; internal set; }
        /// <summary>
        /// 改签条件
        /// </summary>
        public string EndorseRegulation { get; internal set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; internal set; }
        /// <summary>
        /// 所属航班
        /// </summary>
        public Flight Owner { get; internal set; }
        /// <summary>
        /// 舱位类型
        /// </summary>
        public abstract BunkType Type { get; }
        /// <summary>
        /// 是否支持儿童
        /// </summary>
        public bool SuportChild { get; internal set; }
    }
    public abstract class GeneralBunk : Bunk {
        internal GeneralBunk(string code)
            : base(code) {
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount {
            get;
            internal set;
        }
    }
    public class EconomicBunk : GeneralBunk {
        internal EconomicBunk(string code)
            : base(code) {
        }

        public override BunkType Type {
            get { return BunkType.Economic; }
        }
    }
    public class FirstOrBusinessBunk : GeneralBunk {
        internal FirstOrBusinessBunk(string code)
            : base(code) {
        }

        /// <summary>
        /// 舱位描述
        /// </summary>
        public string Description {
            get;
            internal set;
        }

        public override BunkType Type {
            get { return BunkType.FirstOrBusiness; }
        }
    }
    public class PromotionBunk : Bunk {
        internal PromotionBunk(string code)
            : base(code) {
        }

        /// <summary>
        /// 舱位描述
        /// </summary>
        public string Description {
            get;
            internal set;
        }

        public override BunkType Type {
            get { return BunkType.Promotion; }
        }
    }
    public class ProductionBunk : Bunk {
        internal ProductionBunk(string code)
            : base(code) {
        }

        public override BunkType Type {
            get { return BunkType.Production; }
        }
    }
    public class TransferBunk : Bunk {
        internal TransferBunk(string code)
            : base(code) {
        }

        public override BunkType Type {
            get { return BunkType.Transfer; }
        }
    }
    public class FreeBunk : Bunk {
        internal FreeBunk(string code)
            : base(code) {
        }

        /// <summary>
        /// 舱位描述
        /// </summary>
        public string Description {
            get;
            internal set;
        }

        public override BunkType Type {
            get { return BunkType.Free; }
        }
    }

    public class TeamBunk : Bunk {
        internal TeamBunk(string code)
            : base(code) {
        }

        public override BunkType Type {
            get { return BunkType.Team; }
        }
    }
}