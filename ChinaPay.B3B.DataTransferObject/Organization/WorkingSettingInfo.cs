using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.DataTransferObject.Organization {
    using Data.DataMapping;
    using Izual;

    public class WorkingSettingInfo {
        private WorkingSetting workingSetting;
        private WorkingHours workingHours;
        //private 
        private WorkingSettingInfo() {
            workingSetting = new WorkingSetting {};
            workingHours = new WorkingHours {
                RestdayRefundStart = new Time(0,0,0),
                RestdayRefundEnd = new Time(23,59,59),
                RestdayWorkStart = new Time(0, 0, 0),
                RestdayWorkEnd = new Time(23,59,59),
                WorkdayRefundStart = new Time(0, 0, 0),
                WorkdayRefundEnd = new Time(23, 59, 59),
                WorkdayWorkStart = new Time(0, 0, 0),
                WorkdayWorkEnd = new Time(23, 59, 59),
            };
        }
        public WorkingSettingInfo(WorkingSetting workingSetting,WorkingHours workingHours) {
            this.workingSetting = workingSetting;
            this.workingHours = workingHours;
        }

        public static readonly WorkingSettingInfo Default = new WorkingSettingInfo();
        

        public WorkingSetting WorkingSetting {
            get { return workingSetting; }
        }
        public WorkingHours WorkHours {
            get { return workingHours; }
        }
    }
}
