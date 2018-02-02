using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Remind.Repository {
    interface IOrderRemindSettingRepository {
        void SaveCarrierSetting(Guid user, IEnumerable<string> carriers);
        void SaveStatusSetting(Guid user, IEnumerable<Model.RemindStatus> status);
        Model.RemindSetting QuerySetting(Guid user);
    }
}