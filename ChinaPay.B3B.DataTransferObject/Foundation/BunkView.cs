using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.DataTransferObject.Foundation {
    /// <summary>
    /// 舱位
    /// </summary>
    public abstract class BunkView {
        internal const string BunkCodePattern = "^[a-z,A-Z][1-9]?$";

        /// <summary>
        /// 航空公司代码
        /// </summary>
        public string Airline {
            get;
            set;
        }
        /// <summary>
        /// 航班开始日期
        /// </summary>
        public DateTime FlightBeginDate {
            get;
            set;
        }
        /// <summary>
        /// 航班截止日期
        /// </summary>
        public DateTime? FlightEndDate {
            get;
            set;
        }
        /// <summary>
        /// 出票日期
        /// </summary>
        public DateTime ETDZDate {
            get;
            set;
        }
        /// <summary>
        /// 代码
        /// </summary>
        public string Code {
            get;
            set;
        }
        /// <summary>
        /// 退票条件
        /// </summary>
        public string RefundRegulation { get; set; }
        /// <summary>
        /// 改期条件
        /// </summary>
        public string ChangeRegulation { get; set; }
        /// <summary>
        /// 改签条件
        /// </summary>
        public string EndorseRegulation { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Valid {
            get;
            set;
        }
        /// <summary>
        /// 适用行程
        /// </summary>
        public ChinaPay.B3B.Common.Enums.VoyageTypeValue VoyageType { get; set; }
        /// <summary>
        /// 旅客类型
        /// </summary>
        public ChinaPay.B3B.Common.Enums.PassengerTypeValue PassengerType { get; set; }
        /// <summary>
        /// 旅行类型
        /// </summary>
        public ChinaPay.B3B.Common.Enums.TravelTypeValue TravelType { get; set; }

        public virtual void Validate() {
            if(string.IsNullOrWhiteSpace(this.Code))
                throw new ChinaPay.Core.CustomException("请输入舱位代码");
            if(!string.IsNullOrWhiteSpace(this.Airline) && !Regex.IsMatch(this.Airline.Trim(), AirlineView.AirlineCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("航空公司格式错误");
            if(this.FlightEndDate.HasValue && this.FlightEndDate.Value.Date < this.FlightBeginDate.Date)
                throw new ChinaPay.Core.Exception.InvalidValueException("航班结束日期不能小于航班开始日期");
            if(this.FlightBeginDate.Date < this.ETDZDate.Date)
                throw new ChinaPay.Core.Exception.InvalidValueException("出票日期不能大于航班开始日期");
            if(!Regex.IsMatch(this.Code.Trim(), BunkCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("舱位代码格式错误");
            if(!string.IsNullOrWhiteSpace(this.RefundRegulation) && this.RefundRegulation.Trim().Length > 2000)
                throw new ChinaPay.Core.Exception.InvalidValueException("退票条件不能超过2000个字");
            if (!string.IsNullOrWhiteSpace(this.ChangeRegulation)&&this.ChangeRegulation.Trim().Length > 2000)
                throw new ChinaPay.Core.Exception.InvalidValueException("改期条件不能超过2000个字");
            if (!string.IsNullOrWhiteSpace(this.EndorseRegulation)&&this.EndorseRegulation.Trim().Length > 2000)
                throw new ChinaPay.Core.Exception.InvalidValueException("改签条件不能超过2000个字");
            if (!string.IsNullOrWhiteSpace(this.Remarks) && this.Remarks.Trim().Length > 2000)
                throw new ChinaPay.Core.Exception.InvalidValueException("备注不能超过2000个字");
        }
    }
    /// <summary>
    /// 明折明扣舱位
    /// </summary>
    public abstract class GeneralBunkView : BunkView {
        /// <summary>
        /// 出发机场代码
        /// </summary>
        public string Departure {
            get;
            set;
        }
        /// <summary>
        /// 到达机场代码
        /// </summary>
        public string Arrival {
            get;
            set;
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount {
            get;
            set;
        }
        List<ExtendedWithDiscountBunkView> _extended = new List<ExtendedWithDiscountBunkView>();
        /// <summary>
        /// 扩展子舱位
        /// </summary>
        public IEnumerable<ExtendedWithDiscountBunkView> Extended {
            get { return _extended.AsReadOnly(); }
        }
        public void AddExtended(ExtendedWithDiscountBunkView item) {
            if(null != item) {
                if(_extended.Exists(data => data.Code == item.Code)) {
                    throw new ChinaPay.Core.Exception.RepeatedItemException("不能添加重复的子舱位代码");
                }
                _extended.Add(item);
            }
        }

        public override void Validate() {
            base.Validate();

            if(!string.IsNullOrWhiteSpace(this.Departure) && !Regex.IsMatch(this.Departure.Trim(), AirportView.AirportCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("出发地代码格式错误");
            if(!string.IsNullOrWhiteSpace(this.Arrival) && !Regex.IsMatch(this.Arrival.Trim(), AirportView.AirportCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("到达地代码格式错误");
            if(0 >= this.Discount || this.Discount > 9.999M)
                throw new ChinaPay.Core.Exception.InvalidValueException("折扣范围为(0, 9.999]");
            foreach(var item in this._extended) {
                if(item != null) {
                    item.Validate();
                }
            }
        }
    }
    public class ExtendedWithDiscountBunkView {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code {
            get;
            set;
        }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount {
            get;
            set;
        }

        public void Validate() {
            if(string.IsNullOrWhiteSpace(this.Code))
                throw new ArgumentNullException("Code");
            if(!Regex.IsMatch(this.Code.Trim(), BunkView.BunkCodePattern))
                throw new ChinaPay.Core.Exception.InvalidValueException("子舱位代码格式错误");
            if(0 >= this.Discount || this.Discount > 9.999M)
                throw new ChinaPay.Core.Exception.InvalidValueException("折扣范围为(0, 9.999]");
        }
    }
    /// <summary>
    /// 经济舱
    /// </summary>
    public class EconomicBunkView : GeneralBunkView {
    }
    /// <summary>
    /// 头等公务舱
    /// </summary>
    public class FirstBusinessBunkView : GeneralBunkView {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description {
            get;
            set;
        }

        public override void Validate() {
            base.Validate();

            if(string.IsNullOrWhiteSpace(this.Description))
                throw new ChinaPay.Core.CustomException("请输入描述信息");
            if(this.Description.Trim().Length > 25)
                throw new ChinaPay.Core.Exception.InvalidValueException("描述信息不能超过25个字");
        }
    }
    /// <summary>
    /// 特价舱
    /// </summary>
    public class PromotionBunkView : BunkView {
        /// <summary>
        /// 描述
        /// </summary>
        public string Description {
            get;
            set;
        }
        List<string> _extended = new List<string>();
        /// <summary>
        /// 扩展子舱位
        /// </summary>
        public IEnumerable<string> Extended {
            get { return _extended.AsReadOnly(); }
        }
        public void AddExtended(string item) {
            if(!string.IsNullOrWhiteSpace(item)) {
                if(_extended.Contains(item)) {
                    throw new ChinaPay.Core.Exception.RepeatedItemException("不能添加重复的子舱位代码");
                }
                _extended.Add(item);
            }
        }

        public override void Validate() {
            base.Validate();

            if(string.IsNullOrWhiteSpace(this.Description))
                throw new ChinaPay.Core.CustomException("请输入描述信息");
            if(this.Description.Trim().Length > 25)
                throw new ChinaPay.Core.Exception.InvalidValueException("描述信息不能超过25个字");
            foreach(var item in this._extended) {
                if(!string.IsNullOrWhiteSpace(item) && !Regex.IsMatch(item.Trim(), BunkView.BunkCodePattern))
                    throw new ChinaPay.Core.Exception.InvalidValueException("子舱位[" + item + "]格式错误");
            }
        }
    }
    /// <summary>
    /// 往返产品舱
    /// </summary>
    public class ProductionBunkView : BunkView {
    }
    /// <summary>
    /// 中转联程舱
    /// </summary>
    public class TransferBunkView : BunkView {
    }
    /// <summary>
    /// 免票舱
    /// </summary>
    public class FreeBunkView : BunkView {
        /// <summary>
        /// 舱位描述
        /// </summary>
        public string Description { get; set; }

        public override void Validate() {
            base.Validate();

            if(string.IsNullOrWhiteSpace(Description))
                throw new ChinaPay.Core.CustomException("请输入描述信息");
        }
    }
    /// <summary>
    /// 团队舱
    /// </summary>
    public class TeamBunkView : BunkView {
    }
}