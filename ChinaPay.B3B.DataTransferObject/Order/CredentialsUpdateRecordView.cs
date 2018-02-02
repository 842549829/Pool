using System;
using ChinaPay.B3B.DataTransferObject.Log;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.DataTransferObject.Order {
    public class CredentialsUpdateRecordView {
        /// <summary>
        /// 乘机人
        /// </summary>
        public string Passenger { get; set; }
        /// <summary>
        /// 原证件号
        /// </summary>
        public string OriginalCredentials { get; set; }
        /// <summary>
        /// 新证件号
        /// </summary>
        public string NewCredentials { get; set; }
        /// <summary>
        /// 提交角色
        /// </summary>
        public OperatorRole CommitRole { get; set; }
        /// <summary>
        /// 提交账号
        /// </summary>
        public string CommitAccount { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CommitTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Success { get; set; }
    }
}