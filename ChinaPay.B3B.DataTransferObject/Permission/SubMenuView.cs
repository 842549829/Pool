using System;

namespace ChinaPay.B3B.DataTransferObject.Permission {
    /// <summary>
    /// 子菜单
    /// </summary>
    public class SubMenuView : MenuView {
        /// <summary>
        /// 页面路径
        /// </summary>
        public string Address {
            get;
            set;
        }
        public override void Validate() {
            base.Validate();
            if(string.IsNullOrWhiteSpace(this.Address))
                throw new ArgumentNullException("address");
        }
    }
}