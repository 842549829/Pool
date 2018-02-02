using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Policy
{
    /// <summary>
    /// 单独政策设置贴点扣点信息
    /// </summary>
    public class NormalPolicySetting
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 政策编号
        /// </summary>
        public Guid PolicyId { get; set; }
        /// <summary>
        /// 扣点/贴点航线
        /// </summary>
        public string FlightsFilter { get; set; }
        /// <summary>
        /// 适用舱位
        /// </summary>
        public string Berths { get; set; }
        /// <summary>
        /// 扣点/贴点数值
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        ///  扣点/贴点开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        ///  扣点/贴点结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// true是贴点，false是扣点
        /// </summary>
        public bool Type { get; set; }
        /// <summary>
        /// true启用/false禁用
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 操作者
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }
    }
}
