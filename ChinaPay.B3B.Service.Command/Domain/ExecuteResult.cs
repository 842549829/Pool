namespace ChinaPay.B3B.Service.Command.Domain {
    public class ExecuteResult<TResult> {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; internal set; }
        /// <summary>
        /// 成功时的结果；
        /// </summary>
        public TResult Result { get; internal set; }
        /// <summary>
        /// 当成功时存放从航信取得的原始字串（暂只对航班查询有效），当失败时存放错误信息；
        /// </summary>
        public string Message { get; internal set; }
    }
}
