using System;

namespace ChinaPay.B3B.Service.Permission.Domain {
    public class Resource {
        internal Resource()
            : this(Guid.NewGuid()) {
        }
        internal Resource(Guid id) {
            this.Id = id;
        }

        public Guid Id { get; private set; }
        public string Name { get; internal set; }
        public string Address { get; internal set; }
        public string Remark { get; internal set; }
        public bool Valid { get; internal set; }

        public Resource Clone() {
            return new Resource(this.Id) {
                Name = this.Name,
                Address = this.Address,
                Remark = this.Remark,
                Valid = this.Valid
            };
        }
        public override string ToString() {
            return string.Format("地址:{0} 备注:{1} 状态:{2}", this.Address, this.Remark, this.Valid);
        }

        internal bool IsSameAddress(string address) {
            return System.String.Compare(this.Address, address, System.StringComparison.OrdinalIgnoreCase) == 0;
        }

        internal static Resource GetResource(ChinaPay.B3B.DataTransferObject.Permission.ResourceView view) {
            if(view == null) throw new ArgumentNullException("view");
            view.Validate();
            return new Resource {
                Name = view.Name,
                Address = view.Address,
                Remark = view.Remark,
                Valid = view.Valid
            };
        }
        internal static Resource GetResource(Guid id, ChinaPay.B3B.DataTransferObject.Permission.ResourceView view) {
            var result = GetResource(view);
            result.Id = id;
            return result;
        }
    }
}