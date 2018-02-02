namespace ChinaPay.B3B.Service.Order.Domain.Bunk {
    /// <summary>
    /// 免票舱
    /// </summary>
    public class FreeBunk : SpecialBunk {
        private string _description;
        // 免票舱，票面价、折扣 为 0
        internal FreeBunk(string code, decimal releasedFare, string ei, string description)
            : base(code, 0, 0, releasedFare, ei) {
            _description = description;
        }

        public override Common.Enums.BunkType Type {
            get {
                return Common.Enums.BunkType.Free;
            }
        }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description {
            get { return _description; }
        }
    }
}