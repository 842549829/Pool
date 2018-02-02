using System;

namespace ChinaPay.B3B.Service.CommandBuilder.Domain.Ticket
{
    /// <summary>
    /// 电子客票票面提取指令
    /// </summary>
    public class DetrCommand : Command
    {
        /// <summary>
        /// 电子客票票面提取指令
        /// </summary>
        /// <param name="queryStr">查询字符串</param>
        /// <param name="queryType">查询方式</param>
        /// <param name="option">其它选项</param>
        /// <remarks>
        /// option可能有三种，空字串，F或者是S
        /// </remarks>
        public DetrCommand(string queryStr, DetrQeeryType queryType, string option)
        {
            if (string.IsNullOrEmpty(queryStr))
            {
                throw  new ArgumentException();
            }
            _queryStr = queryStr;
            _queryType = queryType;
            _option = option;
            Initialize();
        }
      
        private void Initialize()
        {
            string typeStr;
            switch (_queryType)
            {
                case DetrQeeryType.CN:
                    typeStr = "CN";
                    break;
                case DetrQeeryType.TN:
                    typeStr = "TN";
                    break;
                case DetrQeeryType.NM:
                    typeStr = "NM";
                    break;
                case DetrQeeryType.NI:
                    typeStr = "NI";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            CommandString = string.Format("DETR:{0}/{1}", typeStr, _queryStr);

            if (_option == "F" || _option == "S")
            {
                CommandString = string.Format("{0},{1}", CommandString, _option);
            }
        }

        private readonly string _queryStr;
        private readonly DetrQeeryType _queryType;
        private readonly string _option;
    }
}