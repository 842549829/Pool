using System;
using System.Collections.Generic;

namespace ChinaPay.B3B.Remind.Server.LogonModel {
    class Settings {
        private static Settings m_instance = null;
        private static object m_locker = new object();
        public static Settings Instance {
            get {
                if(m_instance == null) {
                    lock(m_locker) {
                        if(m_instance == null) {
                            m_instance = new Settings();
                        }
                    }
                }
                return m_instance;
            }
        }

        Dictionary<Guid, Service.Remind.Model.RemindSetting> m_userSettings;
        private Settings() {
            m_userSettings = new Dictionary<Guid, Service.Remind.Model.RemindSetting>();
        }

        public Service.Remind.Model.RemindSetting this[Guid user] {
            get {
                Service.Remind.Model.RemindSetting result = null;
                if(!m_userSettings.TryGetValue(user, out result)) {
                    result = Service.Remind.OrderRemindSettingService.QuerySetting(user) ?? new Service.Remind.Model.RemindSetting(null, null);
                    m_userSettings.Add(user, result);
                }
                return result;
            }
        }
    }
}