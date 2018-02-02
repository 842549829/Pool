using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.AirlineConfig.Domain;

namespace ChinaPay.B3B.Service.AirlineConfig.Repository {

    interface IAirlineConfigRepository{
        /// <summary>
        /// 查询公司OEM信息
        /// </summary>
        /// <param name="oemId">OEMID</param>
        /// <returns></returns>
        OEMAirlineConfig QueryConfig(Guid? oemId);
        /// <summary>
        /// 保存OEM配置信息
        /// </summary>
        /// <param name="oemId"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        bool SaveConfig(Guid oemId, Dictionary<ConfigUseType, Tuple<string, string>> config);
    }
}