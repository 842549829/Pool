using System.Collections.Generic;
using System.Text;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain.PNR
{
    /// <summary>
    /// 取消PNR记录的航信指令（没有对参数有效性进行判断呢）
    /// </summary>
    public class XeCommad : Command
    {
        /// <summary>
        /// 取消PNR记录的航信指令
        /// </summary>
        /// <param name="lineNumbers">待取消记录的行号</param>
        public XeCommad(int[] lineNumbers)
        {
            _lineNumbers = lineNumbers;
            Initialize();
        }

        private void Initialize()
        {
            var cmdStr = new StringBuilder("XE:");

            foreach (int item in _lineNumbers)
            {
                cmdStr.Append(item.ToString() + "/");
            }

            CommandString = cmdStr.ToString().TrimEnd("/".ToCharArray());
        }
        
        // 待取消记录的行号
        private readonly int[] _lineNumbers;
    }
}
