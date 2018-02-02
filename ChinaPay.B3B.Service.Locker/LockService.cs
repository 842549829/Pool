using System.Collections.Generic;
using ChinaPay.B3B.Service.Locker;

namespace ChinaPay.B3B.Service {
    /// <summary>
    /// 锁定服务
    /// </summary>
    public static class LockService {
        static ILocker _locker = new RepositoryLocker();

        /// <summary>
        /// 锁定
        /// </summary>
        public static bool Lock(LockInfo lockInfo, out string errorMsg) {
            if(lockInfo == null) {
                errorMsg = "锁定信息不能为空";
                return false;
            }
            if(lockInfo.Validate(out errorMsg)) {
                try {
                    var lockedInfo = _locker.Lock(lockInfo);
                    if(lockedInfo == null) {
                        errorMsg = "系统错误";
                        return false;
                    } else if(lockedInfo.Company == lockInfo.Company && lockedInfo.Account == lockInfo.Account) {
                        return true;
                    } else {
                        if(lockedInfo.LockRole == lockInfo.LockRole) {
                            errorMsg = "用户[" + lockedInfo.Account + "]正在处理该订单";
                        } else {
                            errorMsg = "平台用户正在处理该订单";
                        }
                        return false;
                    }
                } catch {
                    errorMsg = "系统错误";
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="key">标识</param>
        /// <param name="account">账号</param>
        public static bool UnLock(string key, string account) {
            if(string.IsNullOrWhiteSpace(key)) throw new System.ArgumentNullException("key");
            if(string.IsNullOrWhiteSpace(account)) throw new System.ArgumentNullException("account");
            return _locker.UnLock(key, account);
        }
        /// <summary>
        /// 强制解锁
        /// </summary>
        /// <param name="key">标识</param>
        public static void UnLockForcibly(string key) {
            if(string.IsNullOrWhiteSpace(key)) throw new System.ArgumentNullException("key");
            _locker.UnLockForcibly(key);
        }
        /// <summary>
        /// 查询锁定信息
        /// </summary>
        /// <param name="key">标识</param>
        public static LockInfo Query(string key) {
            if(string.IsNullOrWhiteSpace(key)) throw new System.ArgumentNullException("key");
            return _locker.Query(key);
        }
        /// <summary>
        /// 批量查询锁定信息
        /// </summary>
        /// <param name="keys">标识</param>
        public static IEnumerable<LockInfo> Query(IEnumerable<string> keys) {
            return _locker.Query(keys);
        }
        /// <summary>
        /// 验证锁定信息
        /// </summary>
        /// <param name="key">标识</param>
        /// <param name="account">账号</param>
        /// <returns>true:由该账号锁定 false:未锁或非该账号锁定</returns>
        public static bool Validate(string key, string account) {
            if(string.IsNullOrWhiteSpace(key)) throw new System.ArgumentNullException("key");
            if(string.IsNullOrWhiteSpace(account)) throw new System.ArgumentNullException("account");
            var lockInfo = Query(key);
            if(lockInfo != null) {
                return lockInfo.Account == account;
            }
            return false;
        }
    }
}