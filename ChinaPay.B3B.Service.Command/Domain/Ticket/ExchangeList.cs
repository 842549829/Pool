using ChinaPay.B3B.Service.Command.Domain.DataTransferObject;

namespace ChinaPay.B3B.Service.Command.Domain.Ticket
{
    /// <summary>
    /// 电子客票中的兑换信息列表。 
    /// 单张电子客票最多只有连续的两程（往返或联程）。
    /// </summary>
    public class ExchangeList
    {
        /// <summary>
        /// 第一站
        /// </summary>
        public ExchangeDetail FirstStop { get; set; }
        /// <summary>
        /// 第二站
        /// </summary>
        public ExchangeDetail SecondStop { get; set; }
        /// <summary>
        /// 第三站
        /// </summary>
        public string ThirdStop { get; set; }
    }
}
