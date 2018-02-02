using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.ExternalPlatform.Processor;

namespace ChinaPay.B3B.Service.ExternalPlatform._51book {
    public class Platform : Processor.PlatformBase {
        private static Platform _instance = null;
        private static object _locker = new object();
        public static Platform Instance {
            get {
                if(_instance == null) {
                    lock(_locker) {
                        if(_instance == null) {
                            _instance = new Platform();
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly Dictionary<PayInterface, string> _payInterfaces;
        private readonly IEnumerable<PayInterface> _autoPayInterfaces;
        private readonly IEnumerable<PayInterface> _manualPayInterfaces;

        private Platform() {
            Address_GetPolicyByPnrText = System.Configuration.ConfigurationManager.AppSettings["51bookGetPolicyByPnrTextAddress"];
            Address_CreateOrderByPnrText = System.Configuration.ConfigurationManager.AppSettings["51bookCreateOrderByPnrTextAddress"];
            Address_AutoPayOrder = System.Configuration.ConfigurationManager.AppSettings["51bookAutoPayOrderAddress"];
            Address_ManualPayOrder = System.Configuration.ConfigurationManager.AppSettings["51bookManualPayOrderAddress"];
            Address_CancelOrder = System.Configuration.ConfigurationManager.AppSettings["51bookCancelOrderAddress"];
            Address_OrderDetail = System.Configuration.ConfigurationManager.AppSettings["51bookOrderDetailAddress"];

            UserName = System.Configuration.ConfigurationManager.AppSettings["51bookUserName"];
            SecurityCode = System.Configuration.ConfigurationManager.AppSettings["51bookSecurityCode"];
            Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings["51bookTimeout"]);
            Contact = System.Configuration.ConfigurationManager.AppSettings["51bookContact"];
            ContactPhone = System.Configuration.ConfigurationManager.AppSettings["51bookContactPhone"];
            PayReturnUrl = System.Configuration.ConfigurationManager.AppSettings["51bookPayReturnUrl"];
            ETDZNotifyUrl = System.Configuration.ConfigurationManager.AppSettings["51bookNotifyUrl"];
            PayNotifyUrl = System.Configuration.ConfigurationManager.AppSettings["51bookPayNotifyUrl"];

            _payInterfaces = new Dictionary<PayInterface, string>
                             {
                                 {PayInterface.Alipay, "1"},
                                 {PayInterface.Tenpay, "2"}
                             };
            _autoPayInterfaces = new[] { PayInterface.Alipay, PayInterface.Tenpay };
            _manualPayInterfaces = new[] { PayInterface.Alipay };
        }

        public override Common.Enums.PlatformType PlatformInfo {
            get { return Common.Enums.PlatformType.WYbook; }
        }
        internal override System.Text.Encoding Encoding {
            get { return System.Text.Encoding.Default; }
        }
        public override IEnumerable<PayInterface> AutoPayInterfaces {
            get { return _autoPayInterfaces; }
        }
        public override IEnumerable<PayInterface> ManualPayInterfaces {
            get { return _manualPayInterfaces; }
        }
        internal override Dictionary<PayInterface, string> PayInterfaces {
            get {
                return _payInterfaces;
            }
        }

        internal override IPolicyProcessor GetPolicyProcessor() {
            return new PolicyProcessor();
        }

        internal override IOrderProcessor GetOrderProcessor() {
            return new OrderProcessor();
        }

        internal string Address_GetPolicyByPnrText { get; private set; }
        internal string Address_CreateOrderByPnrText { get; private set; }
        internal string Address_AutoPayOrder { get; private set; }
        internal string Address_ManualPayOrder { get; private set; }
        internal string Address_CancelOrder { get; private set; }
        internal string Address_OrderDetail { get; private set; }

        internal string UserName { get; private set; }
        internal string SecurityCode { get; private set; }
        internal int Timeout { get; private set; }
        internal string Contact { get; private set; }
        internal string ContactPhone { get; private set; }
        internal string PayReturnUrl { get; private set; }
        internal string ETDZNotifyUrl { get; private set; }
        internal string PayNotifyUrl { get; private set; }
    }
}