
namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Book
{
    /// <summary>
    /// 建立联系组的航信指令。
    /// </summary>
    public class CtCommand: Command
    {
        /// <summary>
        /// 建立联系组的航信指令
        /// </summary>
        /// <param name="phoneNumber">电话号码</param>
        public CtCommand(string phoneNumber)
        {
            _phoneNumber = phoneNumber;
            Initialize();
        }
        
        private void Initialize()
        {
            CommandString = string.Format("CT:{0}", _phoneNumber);
        }

        // 联系电话；
        private readonly string _phoneNumber;
    }
}
