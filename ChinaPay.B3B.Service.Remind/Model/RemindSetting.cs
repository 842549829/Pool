using System;
using System.Collections.Generic;
using System.Linq;

namespace ChinaPay.B3B.Service.Remind.Model {
    public class RemindSetting {
        IEnumerable<string> m_carriers = null;
        IEnumerable<RemindStatus> m_remindStatus = null;

        public RemindSetting(IEnumerable<string> carriers, IEnumerable<RemindStatus> remindStatus)
        {
            this.m_carriers = carriers ?? new List<string>();
            this.m_remindStatus = remindStatus ?? new List<RemindStatus>();
        }

        public IEnumerable<string> Carriers {
            get {
                return m_carriers;
            }
        }
        public IEnumerable<RemindStatus> RemindStatus {
            get {
                return m_remindStatus;
            }
        }

        public void UpdateCarriers(IEnumerable<string> carriers) {
            this.m_carriers = carriers ?? new List<string>();
        }
        public void UpdateRemindStatus(IEnumerable<RemindStatus> status) {
            this.m_remindStatus = status;
        }

        public bool ContainsCarrier(string carrier) {
            if(this.m_carriers.Any()) {
                return this.m_carriers.Any(item => System.String.Compare(item, carrier, System.StringComparison.OrdinalIgnoreCase) == 0);
            }
            return true;
        }
        public bool ContainsStatus(RemindStatus status) {
            if(this.m_remindStatus.Any()) {
                return this.m_remindStatus.Any(item => item == status);
            }
            return true;
        }

    }
}