using System;

namespace ChinaPay.B3B.DataTransferObject.Permission {
    public class ResourceView {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public bool Valid { get; set; }

        public void Validate() {
            if(string.IsNullOrWhiteSpace(this.Name)) throw new ArgumentNullException("name");
            if(string.IsNullOrWhiteSpace(this.Address)) throw new ArgumentNullException("address");
        }
    }
}