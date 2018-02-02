using System;
using System.Text.RegularExpressions;
using ChinaPay.B3B.DataTransferObject.Common;

namespace ChinaPay.B3B.Service.Command.Domain.Ticket
{
    /// <summary>
    /// 电子客票信息；
    /// </summary>
    public class ElectronicTicket
    {
        public const string FormatString = @"(?<ETicketNumber>\d{3}-\d{10})";

        /// <summary>
        /// 票号
        /// </summary>
        public string TicketNumber { get; set; }

        /// <summary>
        /// 旅客姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电子客票兑换信息
        /// </summary>
        public ExchangeList ExchangeList { get; set; }


        public static bool ValidateTicketNumber(string str)
        {
            var pattern = new Regex(FormatString);
            return pattern.IsMatch(str);
        }
        
        /// <summary>
        /// 判断对应的起飞机场的旅客订座记录是否被取消。
        /// </summary>
        /// <param name="airport"></param>
        /// <returns></returns>
        public bool PnrCodeCancelled(string airport)
        {
            if (airport == null) throw new ArgumentNullException("airport");
            if (ExchangeList.FirstStop.Airport != airport && (ExchangeList.SecondStop==null || ExchangeList.SecondStop.Airport != airport))
            {
                //throw new ArgumentNullException("airport");
                return true;   //在航段不存在的情况下 默认为验证通过 
            }

            var exchangeDetail = ExchangeList.FirstStop.Airport == airport ? ExchangeList.FirstStop : ExchangeList.SecondStop;

            return PNRPair.IsNullOrEmpty(exchangeDetail.PnrPair);
        }
        
        public bool TicketStatusIsOpen(string airport)
        {
            if (airport == null) throw new ArgumentNullException("airport");
            if (ExchangeList.FirstStop.Airport != airport &&(ExchangeList.SecondStop==null|| ExchangeList.SecondStop.Airport != airport))
            {
                //throw new ArgumentNullException("airport");
                return true;   //在航段不存在的情况下 默认为验证通过
            }

            var exchangeDetail = ExchangeList.FirstStop.Airport == airport ? ExchangeList.FirstStop : ExchangeList.SecondStop;

            return exchangeDetail.Status == "OPEN FOR USE";
        }

        public bool TicketNumberAndPassengerIsMatch(string ticketNumber, string name)
        {
            return ticketNumber == TicketNumber && name == Name;
        }
    }
}
