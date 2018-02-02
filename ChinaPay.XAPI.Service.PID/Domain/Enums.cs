using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.XAPI.Service.Pid.Domain
{
    /// <summary>
    /// 认证类型
    /// </summary>
    public enum CertificationType
    {
        /// <summary>
        /// 口令认证
        /// </summary>
        ByPassword,
        /// <summary>
        /// 地址认证；
        /// </summary>
        ByAddress,
        /// <summary>
        /// 信天游一代
        /// </summary>
        TravelSkyFirstGeneration,
        /// <summary>
        /// 信天游二代
        /// </summary>
        TravelSkySecondGeneration    
    }

    /// <summary>
    /// 规则类型
    /// </summary>
    public enum RuleType
    {
        /// <summary>
        /// 仅公有
        /// </summary>
        OnlyPublic,
        /// <summary>
        /// 公有优先
        /// </summary>
        PublicFirst,
        /// <summary>
        /// 仅私有
        /// </summary>
        OnlyPrivate,
        /// <summary>
        /// 私有优先
        /// </summary>
        PrivateFisrt,
        /// <summary>
        /// 同订座配置
        /// </summary>
        SameAsReservation,
        /// <summary>
        /// 任意
        /// </summary>
        AnyOne
    }
}
