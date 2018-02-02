using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.SystemSetting.VIPHarmony
{
  public class VIPHarmonyView
    {
      public VIPHarmonyView(Guid id)
      {
          this.Id = id;
      }
        public Guid Id
        {
            get;
            private set;
        }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName
        {
            get;
            set;
        }
        /// <summary>
        /// 受限航空公司
        /// </summary>
        public IEnumerable<string> AirlineLimit
        {
            get;
            set;
        }
        /// <summary>
        /// 受限城市
        /// </summary>
        public IEnumerable<string> CityLimit
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
    }
}
