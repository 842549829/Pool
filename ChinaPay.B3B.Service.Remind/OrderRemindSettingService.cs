using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Remind {
    public static class OrderRemindSettingService {
        public static Model.RemindSetting QuerySetting(Guid user) {
            var repository = Repository.Factory.CreateRemindSettingRepository();
            return repository.QuerySetting(user);
        }
        public static void SaveCarriers(Guid user, IEnumerable<string> carriers) {
            var repository = Repository.Factory.CreateRemindSettingRepository();
            repository.SaveCarrierSetting(user, carriers);
        }
        public static void SaveStatus(Guid user, IEnumerable<Model.RemindStatus> status) {
            var repository = Repository.Factory.CreateRemindSettingRepository();
            repository.SaveStatusSetting(user, status);
        }
    }
}