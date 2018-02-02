using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.SMS.Service.Templete;
using System.Collections.Generic;
using System.Text;
using System;

namespace ChinaPay.B3B.Service.Order.Notify {
    abstract class Notifier {
        public abstract void Execute();

        protected FlightView GetFlightView(Flight flight) {
            return new FlightView {
                Departure = flight.Departure.City,
                Arrival = flight.Arrival.City,
                Airline = flight.Carrier.Code,
                FlightNo = flight.FlightNo,
                TakeoffTime = flight.TakeoffTime,
                Bunk = flight.Bunk.Code
            };
        }
        protected string GetPassenger(Passenger passenger) {
            return passenger.Name;
        }
        protected void SendNotifyRequest(decimal id, string type, string address, string securityCode, Dictionary<string, string> parameters)
        {
            var signContent = securityCode;
            var notifyUrl = new StringBuilder(address + "?");
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    signContent += p.Value;
                    notifyUrl.AppendFormat("{0}={1}&", p.Key, p.Value);
                }
            }
            var sign = Utility.MD5EncryptorService.MD5(signContent);
            notifyUrl.Append("sign=" + sign);
            OrderSender.Instance.Send(id, type, notifyUrl.ToString());
            //var requestTime = DateTime.Now;
            //var notifyResult = Utility.HttpRequestUtility.GetHttpResult(notifyUrl.ToString(), 3000);
            //var responseTime = DateTime.Now;
            //var log = new Log.Domain.NotifyLog
            //{
            //    OrderId = id,
            //    Type = type,
            //    Request = notifyUrl.ToString(),
            //    RequestTime = requestTime,
            //    Response = notifyResult,
            //    ResponseTime = responseTime,
            //    Success = notifyResult == "0"
            //};
            //LogService.SaveNotifyLog(log);
        }
    }
    public class NotifyRecord
    {
        public decimal Id { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
    }
}