using System.Collections.Generic;

namespace ChinaPay.B3B.DataTransferObject.Command.PNR
{
    /// <summary>
    /// 旅客预订信息
    /// </summary>
    public class ReservationInfo
    {
        /// <summary>
        /// 旅客信息
        /// </summary>
        public List<ReservationPassengerInfo> Passengers { get; set; }

        /// <summary>
        /// 航班信息
        /// </summary>
        public List<ReservationSegmentInfo> Segements { get; set; }

        /// <summary>
        /// 代理人电话
        /// </summary>
        public string AgentPhoneNumber { get; set; }
    }
}
