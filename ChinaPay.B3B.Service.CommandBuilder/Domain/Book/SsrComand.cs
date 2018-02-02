using System;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Book
{
    /// <summary>
    /// 建立特殊服务组的航信指令
    /// </summary>
    public class SsrCommand : Command
    {
        /// <summary>
        /// 建立特殊服务组的航信指令；
        /// </summary>
        /// <param name="carrier">航空公司编码</param>
        /// <param name="message">证件号</param>
        /// <param name="passengerId">旅客编号</param>
        /// <param name="type">特殊服务类型</param>
        public SsrCommand(string carrier, string message, int passengerId, SpecialServiceRequirementType type)
        {
            _carrier = carrier;
            _message = message;
            _passengerId = passengerId;
            _type = type;
            Initialize();
        }

        /// <summary>
        /// 这段应该写成Switch比较好点呢？
        /// </summary>
        private void Initialize()
        {
            switch (_type)
            {
                case SpecialServiceRequirementType.FOID:
                    CommandString = string.Format("SSR: {0} {1} HK/NI{2}/P{3}",
                                                  _type.ToString(), _carrier, _message, _passengerId);
                    break;
                case SpecialServiceRequirementType.CHLD:
                    CommandString = string.Format("SSR: {0} {1} HK1/{2}/P{3}",
                                                                      _type.ToString(), _carrier, _message, _passengerId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        // 航空公司编号
        private readonly string _carrier;
        // 特殊服务信息
        private readonly string _message;
        // 对应旅客编号
        private readonly int _passengerId;
        // 类型
        private readonly SpecialServiceRequirementType _type;

    }
}
