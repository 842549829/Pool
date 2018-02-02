using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Command.PNR;

namespace ChinaPay.B3B.Service.Command.PNR
{
    /// <summary>
    /// 电子客票票面提取指令
    /// </summary>
    public class DETRCommand : Command
    {
        /// <summary>
        /// 电子客票票面提取指令
        /// </summary>
        /// <param name="queryStr">和类型相对应的查询字符串</param>
        /// <param name="queryType">查询方式</param>
        public DETRCommand(string queryStr, DETRQeeryType queryType)
        {
            if (queryStr != null)
            {
                this.queryStr = queryStr;
                this.queryType = queryType;
                switch (queryType)
                {
                    case DETRQeeryType.CN:
                        this.commandString = string.Format("DETR:CN/{0}", this.queryStr); 
                        break;
                    case DETRQeeryType.TN:
                        this.commandString = string.Format("DETR:TN/{0}", this.queryStr);
                        break;
                    case DETRQeeryType.NM:
                        this.commandString = string.Format("DETR:NM/{0}", this.queryStr); 
                        break;
                    case DETRQeeryType.NI:
                        this.commandString = string.Format("DETR:NI/{0}", this.queryStr); 
                        break;
                    default:
                        break;
                }
            }
            Initialize();
        }
      
        private void Initialize()
        {
            this.commandType = CommandType.ETicket;
        }

        private string queryStr;
        private DETRQeeryType queryType;
    }
}
