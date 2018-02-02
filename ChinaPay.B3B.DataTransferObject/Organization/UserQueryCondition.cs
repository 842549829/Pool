namespace ChinaPay.B3B.DataTransferObject.Organization {
    /// <summary>
    /// 查询员工列表条件
    /// </summary>
    public class UserQueryCondition {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name {
            get;
            set;
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName {
            get;
            set;
        }
        /// <summary>
        /// 状态
        /// 是否启用
        /// </summary>
        public bool? Enabled {
            get;
            set;
        }
    }
}
