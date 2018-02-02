using System;

namespace ChinaPay.B3B.DataTransferObject.SystemSetting.MarketingArea {
    /// <summary>
    /// 销售区域设置
    /// </summary>
    public class AreaListView {
        public Guid Id {
            get;
            set;
        }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string Name {
            get;
            set;
        }
        /// <summary>
        /// 区域备注
        /// </summary>
        public string Remark {
            get;
            set;
        }
    }
}
