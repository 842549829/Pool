using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.AirlineConfig.Domain
{
    public class OEMAirlineConfig
    {
        /// <summary>
        /// OEMID
        /// </summary>
        public Guid OEMID { get; set; }
        /// <summary>
        /// 是否使用B3B配置
        /// </summary>
        public bool UserB3bConfig { get; set; }
        /// <summary>
        /// 使用情况配置
        /// </summary>
        public Dictionary<ConfigUseType, Tuple<string, string>> Config
        {
            get;
            set;
        }
    }
}
