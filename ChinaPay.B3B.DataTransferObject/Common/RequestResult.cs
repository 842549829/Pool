namespace ChinaPay.B3B.DataTransferObject.Common {
    public class RequestResult<TResult> {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 错误原因
        /// </summary>
        public string ErrMessage { get; set; }
        /// <summary>
        /// 结果信息
        /// </summary>
        public TResult Result { get; set; }
    }
}