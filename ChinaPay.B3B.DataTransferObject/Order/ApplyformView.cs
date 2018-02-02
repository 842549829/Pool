using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core;
using System.Linq;

namespace ChinaPay.B3B.DataTransferObject.Order {
    /// <summary>
    /// 退改签申请
    /// </summary>
    public abstract class ApplyformView {
        /// <summary>
        /// 编码信息
        /// </summary>
        public PNRPair PNR { get; set; }
        /// <summary>
        /// 乘机人集合
        /// </summary>
        public IEnumerable<Guid> Passengers { get; set; }
        public abstract IEnumerable<Guid> Voyages { get; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
    }
    public abstract class RefundOrScrapApplyformView : ApplyformView {
        List<Guid> _items = new List<Guid>();
        /// <summary>
        /// 航段信息集合
        /// </summary>
        public IEnumerable<Guid> Items { get { return _items; } }
        public override IEnumerable<Guid> Voyages {
            get { return this.Items; }
        }

        /// <summary>
        /// 委托平台取消编码
        /// </summary>
        public bool DelegageCancelPNR { get; set; }

        public bool NeedPlatfromCancelPNR
        {
            get;
            set;
        }

        public void AddVoyage(Guid voyage) {
            _items.Add(voyage);
        }
    }
    /// <summary>
    /// 退票申请
    /// </summary>
    public class RefundApplyformView : RefundOrScrapApplyformView {
        /// <summary>
        /// 退票类型
        /// </summary>
        public RefundType RefundType { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public List<ApplyAttachmentView> ApplyAttachmentView { get; set; }
        
    }
    /// <summary>
    /// 附件
    /// </summary
    [Serializable]
    public class ApplyAttachmentView {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 申请单
        /// </summary>
        public decimal ApplyformId { get; set; }
        /// <summary>
        /// 源文件相对路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public byte[] Thumbnail { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
    }
    /// <summary>
    /// 废票申请
    /// </summary>
    public class ScrapApplyformView : RefundOrScrapApplyformView {
    }
    /// <summary>
    /// 改期申请
    /// </summary>
    public class PostponeApplyformView : ApplyformView {
        List<Item> _items = new List<Item>();

        /// <summary>
        /// 改期信息
        /// </summary>
        public IEnumerable<Item> Items { get { return _items; } }

        public override IEnumerable<Guid> Voyages {
            get {
                return this._items.Select(item => item.Voyage);
            }
        }
        public void AddItem(Item item) {
            if(item == null)
                throw new ArgumentNullException("item");
            _items.Add(item);
        }
        public class Item {
            /// <summary>
            /// 航段
            /// </summary>
            public Guid Voyage { get; set; }
            /// <summary>
            /// 新航班号
            /// </summary>
            public string NewFlightNo { get; set; }
            /// <summary>
            /// 新航班日期
            /// </summary>
            public DateTime NewFlightDate { get; set; }
        }
    }
    /// <summary>
    /// 升舱申请
    /// </summary>
    public class UpgradeApplyformView : ApplyformView {
        List<Item> _items = new List<Item>();

        /// <summary>
        /// 新编码
        /// </summary>
        public PNRPair NewPNR { get; set; }
        /// <summary>
        /// 编码来源
        /// </summary>
        public OrderSource PNRSource { get; set; }
        /// <summary>
        /// PAT价格
        /// 仅普通往返时需要
        /// </summary>
        public Command.PNR.PriceView PATPrice { get; set; }
        /// <summary>
        /// 升舱后航班信息
        /// </summary>
        public IEnumerable<Item> Items { get { return _items; } }

        public override IEnumerable<Guid> Voyages {
            get {
                return this._items.Select(item => item.Voyage);
            }
        }
        public void AddItem(Item item) {
            if(item == null)
                throw new ArgumentNullException("item");
            _items.Add(item);
        }
        public class Item {
            /// <summary>
            /// 航段信息
            /// </summary>
            public Guid Voyage { get; set; }
            /// <summary>
            /// 航班信息
            /// </summary>
            public FlightView Flight { get; set; }
        }
    }

    /// <summary>
    /// 差额退款申请
    /// </summary>
    public class BalanceRefundApplyView : ApplyformView
    {
        public BalanceRefundApplyView(IEnumerable<Guid> flights, decimal associateApplyformId, string applyRemark)
        {
            _Voyages.AddRange(flights);
            AssociateApplyformId = associateApplyformId;
            Reason = applyRemark;
        }
        public BalanceRefundApplyView(decimal associateApplyformId)
        {
            AssociateApplyformId = associateApplyformId;
        }
        private List<Guid> _Voyages = new List<Guid>();
        public override IEnumerable<Guid> Voyages
        {
            get
            {
                return _Voyages;
            }
        }

        public decimal AssociateApplyformId
        {
            get;
            private set;
        }

    }
}