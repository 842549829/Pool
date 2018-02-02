using System.Text;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain.PNR
{
    /// <summary>
    /// 取消PNR旅客记录的航信指令
    /// </summary>
    public class XepCommad : Command
    {
        /// <summary>
        /// 取消PNR旅客记录的航信指令
        /// </summary>
        /// <param name="lineNumbers">旅客对应的行号</param>
        public XepCommad(int[] lineNumbers)
        {
            _lineNumbers = lineNumbers;
            Initialize();
        }
        
        private void Initialize()
        {
            var cmdStr = new StringBuilder("XEP");
            foreach (int item in _lineNumbers)
            {
                cmdStr.Append(item.ToString() + "/");
            }
            CommandString = cmdStr.ToString().TrimEnd("/".ToCharArray());
        }

        // 待取消旅客对应的行号
        private readonly int[] _lineNumbers;
    }
}
