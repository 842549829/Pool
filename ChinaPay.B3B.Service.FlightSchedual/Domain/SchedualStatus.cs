
namespace ChinaPay.B3B.Service.FlightSchedual.Domain
{
    /// <summary>
    /// 航班时刻表状态
    /// </summary>
    public enum SchedualStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal,
        /// <summary>
        /// 延误
        /// </summary>
        Delay,
        /// <summary>
        /// 变更
        /// </summary>
        Change,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel
    }
}
