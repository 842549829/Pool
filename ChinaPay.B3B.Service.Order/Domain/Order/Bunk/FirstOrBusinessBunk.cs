namespace ChinaPay.B3B.Service.Order.Domain.Bunk {
    /// <summary>
    /// 头等/公务舱
    /// </summary>
    public class FirstOrBusinessBunk : GeneralBunk {
        string _description;

        internal FirstOrBusinessBunk(string code, decimal discount, string ei, string description)
            : base(code, discount, ei) {
            _description = description ?? string.Empty;
        }
        internal FirstOrBusinessBunk(string code, decimal discount, decimal fare, string ei, string description)
            : base(code, discount, fare, ei) {
            _description = description ?? string.Empty;
        }

        public string Description {
            get {
                return _description;
            }
        }

        public override Common.Enums.BunkType Type {
            get { return Common.Enums.BunkType.FirstOrBusiness; }
        }
    }
}