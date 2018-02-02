using System;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain.PNR
{
    /// <summary>
    /// 提取机票价格的航信指令。
    /// </summary>
    public class PatCommand : Command
    {
        /// <summary>
        /// 提取机票价格的航信指令
        /// </summary>
        public PatCommand(PassengerType passengerType)
        {
            _passengerType = passengerType;
            Initialize();
        }
        
        private void Initialize()
        {
            switch (_passengerType)
            {
                case PassengerType.Adult:
                    CommandString = "PAT:A";
                    break;
                case PassengerType.Child:
                    CommandString = "PAT:A*CH";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }            
        }

        private readonly PassengerType _passengerType;
    }
}
