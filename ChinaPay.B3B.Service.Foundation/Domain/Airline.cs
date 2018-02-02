using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core;
using ChinaPay.B3B.DataTransferObject.Foundation;
using System;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.Service.Foundation.Domain {
    /// <summary>
    /// 航空公司
    /// </summary>
    public class Airline {
        internal Airline(UpperString code) {
            this.Code = code;
        }
        /// <summary>
        /// 代码
        /// </summary>
        public UpperString Code {
            get;
            private set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name {
            get;
            internal set;
        }
        /// <summary>
        /// 简称
        /// </summary>
        public string ShortName {
            get;
            internal set;
        }
        /// <summary>
        /// 结算代码
        /// </summary>
        public string SettleCode {
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
        /// <summary>
        /// 所有舱位
        /// </summary>
        public IEnumerable<Bunk> Bunks {
            get {
                return from item in BunkCollection.Instance.Values
                       where item.AirlineCode.Value == this.Code.Value
                       select item;
            }
        }
        ///// <summary>
        ///// 儿童可预订舱位
        ///// </summary>
        //public IEnumerable<ChildOrderableBunk> GetChildrenOrderableBunks(string departure, string arrival, DateTime flightDate) {
        //    var bunks = ChildOrderableBunkCollection.Instance.QueryChildrenOrderableBunk(this.Code);
        //    if(bunks.Any()) {
        //        return bunks;
        //    } else {
        //        return (from bunk in BunkCollection.Instance.QueryValidGeneralBunks(Code, departure, arrival, flightDate)
        //                where bunk != null && bunk.Discount >= 1
        //                orderby bunk.Level descending, bunk.FlightBeginDate descending
        //                group bunk by bunk.Code into g
        //                let b = g.First()
        //                select new ChildOrderableBunk {
        //                    AirlineCode = this.Code,
        //                    BunkCode = b.Code,
        //                    Bunk = b,
        //                    Discount = b.Discount * 0.5M
        //                }).ToList();
        //    }
        //}
        ///// <summary>
        ///// 儿童可预订舱位
        ///// </summary>
        //public IEnumerable<GeneralBunk> GetChildOrderableBunks(string departure, string arrival, DateTime flightDate) {
        //    var bunks = (from bunk in BunkCollection.Instance.QueryValidGeneralBunks(Code, departure, arrival, flightDate)
        //                 orderby bunk.Level descending, bunk.FlightBeginDate descending, bunk.ModifyTime descending
        //                 group bunk by bunk.Code into g
        //                 let b = g.First()
        //                 where (b.PassengerType & PassengerTypeValue.Child) == PassengerTypeValue.Child
        //                 select b.Copy()).ToList();
        //    bunks.ForEach(b => {
        //        b.Discount *= 0.5M;
        //        foreach(var eb in b.Extended) {
        //            eb.Discount *= 0.5M;
        //        }
        //    });
        //    return bunks.ToList();
        //}

        public override string ToString() {
            return string.Format("代码:{0} 名称:{1} 简称:{2} 结算代码:{3} 状态:{4}", Code.Value, Name, ShortName, SettleCode, Valid);
        }

        internal static Airline GetAirline(AirlineView airlineView) {
            if(null == airlineView) throw new ArgumentNullException("airlineView");
            airlineView.Validate();
            return new Airline(airlineView.Code.Trim()) {
                Name = Utility.StringUtility.Trim(airlineView.Name),
                ShortName = Utility.StringUtility.Trim(airlineView.ShortName),
                SettleCode = Utility.StringUtility.Trim(airlineView.SettleCode),
                Valid = airlineView.Valid
            };
        }
    }
}
