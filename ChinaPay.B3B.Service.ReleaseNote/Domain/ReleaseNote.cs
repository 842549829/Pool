using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.ReleaseNote.Domain
{
    /// <summary>
    /// 更新日志类
    /// </summary>
    public class ReleaseNote
    {
        /// <summary>
        /// 编号
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 操作者
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 系统日志更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 更新标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 更新内容
        /// </summary>
        public string Context { get; set; }
        /// <summary>
        /// 显示类型
        /// </summary>
        public CompanyType? Type { get; set; }
        /// <summary>
        /// 更新日志类型
        /// </summary>
        public ReleaseNoteType ReleaseType { get; set; }
    }
}
