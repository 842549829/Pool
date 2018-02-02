namespace ChinaPay.B3B.Service {
    class Checker {
        public static void CheckOrderLocked(decimal orderId, string account) {
            if(!Service.LockService.Validate(orderId.ToString(), account))
                throw new ChinaPay.Core.CustomException("执行操作前，先锁定");
            }
        public static void CheckApplyformLocked(decimal applyformId, string account) {
            if(!Service.LockService.Validate(applyformId.ToString(), account))
                throw new ChinaPay.Core.CustomException("执行操作前，先锁定");
        }
    }
}
