using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.ExternalPlatform.Repository
{
    interface ISettingReposity
    {
        IEnumerable<Setting> QuerySettings();
        Setting QuerySetting(ChinaPay.B3B.Common.Enums.PlatformType platform);
        int InsertSetting(Setting setting);
        int UpdateSetting(Setting setting);
        int UpdateStatus(ChinaPay.B3B.Common.Enums.PlatformType platform, bool enabled);
        IEnumerable<ChinaPay.B3B.DataTransferObject.Policy.ExternalPolicyLog> QueryExternalPolicys(decimal? orderId, DateTime stratDate, DateTime endDate, ChinaPay.Core.Pagination pagination);
    }
}
