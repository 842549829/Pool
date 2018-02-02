using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Book
{
    /// <summary>
    /// 建立姓名组的航信指令。
    /// </summary>
    public class NmCommand : Command
    {
        public NmCommand(List<string> names, PassengerType passengerType,string carrier)
        {
            _names = names;
            _passengerType = passengerType;
            _carrier = carrier;
            Initialize();
        }

        private void Initialize()
        {
            var cmdStr = new StringBuilder("NM:");
            foreach (string name in _names)
            {
                switch (_passengerType)
                {
                    case PassengerType.Child:
                        if (_carrier != "CZ")
                        {
                            //如果是中文，直接chd，否则空格；
                            var pattern = new Regex("^[\u4e00-\u9fa5]{2,}$");
                            cmdStr.AppendFormat(pattern.Match(name).Success ? "{0} {1}CHD" : "{0} {1} CHD", 1, name);
                        }
                        else
                        {
                            cmdStr.AppendFormat("{0}{1} ", 1, name);
                        }
                        break;
                    case PassengerType.Adult:
                        cmdStr.AppendFormat("{0}{1} ", 1, name);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("旅客类型");
                }
            }
            CommandString  = cmdStr.ToString();
        }

        // 旅客姓名列表；
        private readonly List<string> _names;
        // 旅客类型；
        private readonly PassengerType _passengerType;

        private readonly string _carrier;
    }
}
