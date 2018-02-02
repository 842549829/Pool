using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.SystemManagement
{
    /// <summary>
    /// 特殊产品管理
    /// </summary>
    public class SpecialProductView
    {
        public SpecialProductView(SpecialProductType id)
        {
            this.SpecialProductType = id;
        }
        /// <summary>
        /// 产品Id
        /// </summary>
        public SpecialProductType SpecialProductType { get; private set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Explain { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Enabled { get; set; }
    }
}
