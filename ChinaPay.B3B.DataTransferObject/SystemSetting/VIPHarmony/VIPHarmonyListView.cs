using System;

namespace ChinaPay.B3B.DataTransferObject.SystemSetting.VIPHarmony {
    public class VIPHarmonyListView {
        public VIPHarmonyListView(Guid id) {
            this.Id = id;
        }
        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName {
            get;
            set;
        }
        /// <summary>
        /// 受限航空公司
        /// </summary>
        public string AirlineLimit {
            get;
            set;
        }
        /// <summary>
        /// 受限城市
        /// </summary>
        public string CityLimit {
            get;
            set;
        }
        /// <summary>
        /// 添加帐号
        /// </summary>
        public string Account {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark {
            get;
            set;
        }
    }
}
