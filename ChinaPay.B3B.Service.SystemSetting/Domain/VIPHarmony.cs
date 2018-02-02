using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.SystemSetting.Domain {
    public class VIPHarmony {
        public VIPHarmony() {
            this.Id = Guid.NewGuid();
        }
        public VIPHarmony(Guid id) {
            this.Id = id;
        }
        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 区域id
        /// </summary>
        public Guid Area {
            get;
            set;
        }
        /// <summary>
        /// 受限航空公司
        /// </summary>
        public IEnumerable<string> AirlineLimit {
            get;
            set;
        }
        /// <summary>
        /// 受限出港城市
        /// </summary>
        public IEnumerable<string> CityLimit {
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
        /// 备注
        /// </summary>
        public string Remark {
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
    }
}
