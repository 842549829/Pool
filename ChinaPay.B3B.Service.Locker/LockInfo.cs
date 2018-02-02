using System;

namespace ChinaPay.B3B.Service.Locker {
    public class LockInfo {
        public LockInfo(string key) {
            this.Key = key;
            this.Time = DateTime.Now;
        }
        public string Key { get; private set; }
        /// <summary>
        /// 锁定角色
        /// </summary>
        public LockRole LockRole { get; set; }
        /// <summary>
        /// 锁定单位Id
        /// </summary>
        public Guid Company { get; set; }
        /// <summary>
        /// 锁定单位名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 锁定账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 锁定时间
        /// </summary>
        public DateTime Time { get; internal set; }
        /// <summary>
        /// 锁定人姓名
        /// </summary>
        public string Name { get; set; }

        internal bool Validate(out string errorMessage) {
            if(this.Company == Guid.Empty) {
                errorMessage = "锁定单位id不能为空";
                return false;
            }
            if(string.IsNullOrWhiteSpace(this.Account)) {
                errorMessage = "锁定账号不能为空";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
    }
}