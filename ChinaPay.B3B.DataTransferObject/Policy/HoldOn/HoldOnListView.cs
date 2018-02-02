using System;
using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.DataTransferObject.Policy.HoldOn {
    using B3B.Common.Enums;

    /// <summary>
    /// 政策挂起列表信息
    /// </summary>
    public class HoldOnListView {
        public Guid Company { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        public CompanyType CompanyType { get; set; }
        /// <summary>
        /// 单位简称
        /// </summary>
        public string AbbreviateName { get; set; }
        /// <summary>
        /// 平台挂起项集合
        /// </summary>
        public IEnumerable<HoldOnItem> Platform { get; set; }
        /// <summary>
        /// 发布方挂起项集合
        /// </summary>
        public IEnumerable<HoldOnItem> Publisher { get; set; }
    }
}