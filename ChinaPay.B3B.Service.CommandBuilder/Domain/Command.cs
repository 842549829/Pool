namespace ChinaPay.B3B.Service.CommandBuilder
{
    /// <summary>
    /// 航信指令父类
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// 指令字串
        /// </summary>
        public string CommandString { get; protected set; }

        /// <summary>
        /// 返回字串
        /// </summary>
        public string ReturnString { get; protected set; }
    }
}
