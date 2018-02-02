using System;
using ChinaPay.Core;
using ChinaPay.B3B.DataTransferObject.Foundation;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    /// <summary>
    /// 机型
    /// </summary>
    public class AirCraft {
        internal AirCraft()
            : this(Guid.NewGuid()) {
        }
        internal AirCraft(Guid id) {
            this.Id = id;
        }
        public Guid Id {
            get;
            private set;
        }
        /// <summary>
        /// 代码
        /// </summary>
        public UpperString Code {
            get;
            internal set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name {
            get;
            internal set;
        }
        /// <summary>
        /// 机场建设费
        /// </summary>
        public decimal AirportFee {
            get;
            internal set;
        }
        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer {
            get;
            internal set;
        }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description {
            get;
            internal set;
        }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Valid {
            get;
            internal set;
        }
        public override string ToString() {
            return string.Format("代码:{0} 名称:{1} 机建费:{2} 制造商:{3} 描述信息:{4} 状态:{5}", Code.Value, Name, AirportFee, Manufacturer, Description, Valid);
        }

        internal static AirCraft GetAirCraft(AirCraftView airCraftView) {
            if(null == airCraftView)
                throw new ArgumentNullException("airCraftView");
            airCraftView.Validate();
            return new AirCraft() {
                Code = ChinaPay.Utility.StringUtility.Trim(airCraftView.Code),
                Name = ChinaPay.Utility.StringUtility.Trim(airCraftView.Name),
                AirportFee = airCraftView.AirportFee,
                Manufacturer = ChinaPay.Utility.StringUtility.Trim(airCraftView.Manufacturer),
                Description = ChinaPay.Utility.StringUtility.Trim(airCraftView.Description),
                Valid = airCraftView.Valid
            };
        }
        internal static AirCraft GetAirCraft(Guid id, AirCraftView airCraftView) {
            var airCraft = GetAirCraft(airCraftView);
            airCraft.Id = id;
            return airCraft;
        }
    }
}
