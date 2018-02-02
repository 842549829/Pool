using System.ComponentModel;

namespace ChinaPay.B3B.DataTransferObject.Command.PNR
{
    /// <summary>
    /// 行程类型
    /// </summary>
    public enum ItineraryType
    {
        /// <summary>
        /// 单程
        /// </summary>
        [Description("单程")]
        OneWay,
        /// <summary>
        /// 往返程
        /// </summary>
        [Description("往返")]
        Roundtrip,
        /// <summary>
        /// 联程
        /// </summary>
        [Description("联程")]
        Conjunction,
        /// <summary>
        /// 缺口程
        /// </summary>
        [Description("缺口程")]
        Notch
    }
}
