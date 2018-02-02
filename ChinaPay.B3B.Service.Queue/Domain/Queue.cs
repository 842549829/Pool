using System;
using ChinaPay.B3B.Service.CommandBuilder.Domain.Queue;

namespace ChinaPay.B3B.Service.Queue.Domain
{
    /// <summary>
    /// 信箱
    /// </summary>
    public class Queue
    {
        public Queue(string content, QueueType type, string officeNumber)
        {
            Content = content;
            Type = type;
            OfficeNumber = officeNumber;
        }

        public Queue(DateTime proccessTime, int internalNumber, string content, QueueType type, string officeNumber)
        {
            ProcessTime = proccessTime;
            InternalNumber = internalNumber;
            Content = content;
            Type = type;
            OfficeNumber = officeNumber;
        }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime ProcessTime { get; set; }
        /// <summary>
        /// 内部编号
        /// </summary>
        public int InternalNumber { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 信箱类型
        /// </summary>
        public QueueType Type { get; set; }
        /// <summary>
        /// 代理人编号
        /// </summary>
        public string OfficeNumber { get; set; }

        /// <summary>
        /// 判断给出的内容字串是否是有效的信件内容；
        /// </summary>
        /// <param name="queueContent"></param>
        /// <param name="officNumber"> </param>
        public static bool Validate(string queueContent, string officNumber)
        {
            return queueContent.Contains(officNumber);
        }
    }
}
