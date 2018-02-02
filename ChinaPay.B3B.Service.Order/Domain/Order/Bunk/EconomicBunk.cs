namespace ChinaPay.B3B.Service.Order.Domain.Bunk {
    /// <summary>
    /// 经济舱
    /// </summary>
    public class EconomicBunk : GeneralBunk {
        internal EconomicBunk(string code, decimal discount, string ei)
            : base(code, discount, ei) {
        }
        internal EconomicBunk(string code, decimal discount, decimal fare, string ei)
            : base(code, discount, fare, ei) {
        }

        public override Common.Enums.BunkType Type {
            get { return Common.Enums.BunkType.Economic; }
        }
    }
}
