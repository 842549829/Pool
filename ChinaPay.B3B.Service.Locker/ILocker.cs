using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Locker {
    interface ILocker {
        LockInfo Lock(LockInfo lockInfo);
        bool UnLock(string key, string account);
        void UnLockForcibly(string key);
        LockInfo Query(string key);
        IEnumerable<LockInfo> Query(IEnumerable<string> keys);
    }
}
