using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Foundation.Domain;

namespace ChinaPay.B3B.Service.SystemSetting.Domain {
    public class SellArea {
        public SellArea() {
            this.Id = Guid.NewGuid();
        }
        public SellArea(Guid id) {
            this.Id = id;
        }
        public Guid Id {
            get;
            private set;
        }
        public string Name {
            get;
            set;
        }
        public string Remark {
            get;
            set;
        }
        public IEnumerable<Province> Provinces {
            get { throw new NotImplementedException(); }
        }
    }
}
