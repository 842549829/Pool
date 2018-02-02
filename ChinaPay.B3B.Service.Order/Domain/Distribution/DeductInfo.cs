using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Distribution.Domain {
    /// <summary>
    /// 交易信息
    /// </summary>
    public class TradeInfo {
        /// <summary>
        /// 交易号
        /// </summary>
        public decimal Id { get; set; }
        /// <summary>
        /// 是否三方关系
        /// </summary>
        public bool IsThirdRelation { get; set; }
        /// <summary>
        /// 是否特殊产品
        /// </summary>
        public bool IsSpecialProduct { get; set; }
        /// <summary>
        /// 特殊产品类型
        /// </summary>
        public Common.Enums.SpecialProductType SpecialProductType { get; set; }
        /// <summary>
        /// 出票方与采购方关系
        /// </summary>
        public Common.Enums.RelationType ProviderRelationWithPurchaser { get; set; }
        /// <summary>
        /// 乘机人列表
        /// </summary>
        public IEnumerable<Guid> Passengers { get; set; }
        /// <summary>
        /// 航班列表
        /// </summary>
        public IEnumerable<Flight> Flights { get; set; }
    }
    public class TradeDeduction {
        /// <summary>
        /// 出票方
        /// </summary>
        public UserDeduction Provider { get; set; }
        /// <summary>
        /// 采购方
        /// </summary>
        public UserDeduction Purchaser { get; set; }
        /// <summary>
        /// 产品方
        /// </summary>
        public UserDeduction Supplier { get; set; }
        /// <summary>
        /// 平台贴/扣点值
        /// </summary>
        public Deduction Platform { get; set; }

        private List<UserDeduction> _royalties = null;
        /// <summary>
        /// 分润方
        /// </summary>
        public IEnumerable<UserDeduction> Royalties { get { return _royalties ?? (_royalties = new List<UserDeduction>()); } }

        public void AddRoyalty(UserDeduction royalty) {
            if(royalty == null) throw new ArgumentNullException("royalty");
            if(_royalties == null) {
                _royalties = new List<UserDeduction>();
            }
            _royalties.Add(royalty);
        }
    }
    public class UserDeduction {
        /// <summary>
        /// 所属者
        /// </summary>
        public Guid Owner { get; set; }
        /// <summary>
        /// 返点信息
        /// </summary>
        public Deduction Deduction { get; set; }
    }
    public class Deduction {
        /// <summary>
        /// 返点
        /// </summary>
        public decimal Rebate { get; set; }
        /// <summary>
        /// 加价
        /// </summary>
        public decimal Increasing { get; set; }
    }
}