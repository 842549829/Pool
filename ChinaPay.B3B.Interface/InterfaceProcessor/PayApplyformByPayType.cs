using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using ChinaPay.Gateway.Tradement;

namespace ChinaPay.B3B.Interface.InterfaceProcessor
{
    internal class PayApplyformByPayType : BaseProcessor
    {
         private string _id;
        private string _payType;
        public PayApplyformByPayType(string id, string userName, string payType, string sign)
            : base(userName, sign)
        {
            this._id = id;
            this._payType = payType;
        }
        protected override System.Collections.Specialized.NameValueCollection GetBusinessParameterCollection()
        {
            var collection = new System.Collections.Specialized.NameValueCollection();
            collection.Add("Id", _id);
            collection.Add("PayType", _payType);
            return collection;
        }

        protected override void ValidateBusinessParameters()
        {
            if (string.IsNullOrWhiteSpace(_id)) throw new InterfaceInvokeException("1", "订单号");
            if (string.IsNullOrWhiteSpace(_payType)) throw new InterfaceInvokeException("1","支付通道");
        }

        protected override string ExecuteCore()
        {
            decimal id;
            string url = string.Empty;
            if (decimal.TryParse(_id, out id))
            {
                string message = "";
                if (Service.ApplyformProcessService.Payable(id, out message))
                {
                    string clientIP = ChinaPay.AddressLocator.IPAddressLocator.GetRequestIP(_context.Request).ToString();
                    string payType = GetBankInfo(_payType);
                    //url = Service.Tradement.PaymentService.OnlinePayPostponeFee(id, payType, clientIP, Employee.UserName);
                    url = string.Format("{0}?id={1}&bank={2}&userName={3}&type=2&payIp={4}",
    ConfigurationManager.AppSettings["PayUrl"], id, payType, Employee.UserName, clientIP);

                }
                else
                {
                    if (!string.IsNullOrEmpty(message)) throw new InterfaceInvokeException("9", "改期申请单："+id+" 已经不能进行支付了");
                }
                if (!string.IsNullOrEmpty(message)) throw new InterfaceInvokeException("9", message);
            }
            else {
                throw new InterfaceInvokeException("1", "订单号");
            }
            return "<payUrl>" + System.Web.HttpUtility.UrlEncode(url) + "</payUrl>";
        }

        private string GetBankInfo(string bankInfo) {
            PayChannelQueryProcessor processor = new PayChannelQueryProcessor();
            if (processor.Execute())
            {
                bankInfo = bankInfo == "0" ? "255" : bankInfo;
                var channels = processor.Channels.FirstOrDefault(item => item.Code == bankInfo);
                if (channels == null) throw new InterfaceInvokeException("1", "支付通道");
                return string.Format("{0}|{1}|{2}", channels.Code, string.Empty, channels.Name);
            }
            return string.Empty;
        }
    }
}