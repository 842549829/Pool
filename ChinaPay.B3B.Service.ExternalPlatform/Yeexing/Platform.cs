using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.ExternalPlatform.Processor;

namespace ChinaPay.B3B.Service.ExternalPlatform.Yeexing {
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

        private Platform() {
            Address = System.Configuration.ConfigurationManager.AppSettings["yeexingAddress"];
            UserName = System.Configuration.ConfigurationManager.AppSettings["yeexingUserName"];
            Key = System.Configuration.ConfigurationManager.AppSettings["yeexingKey"];
            NotifyUrl = System.Configuration.ConfigurationManager.AppSettings["yeexingNotifyUrl"];
            Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings["yeexingTimeout"]);

            _payInterfaces = new Dictionary<PayInterface, string>
                             {
                                 {PayInterface.Alipay, "1"},
                                 {PayInterface.ChinaPnr, "2"}
                             };
            _autoPayInterfaces = new[] { PayInterface.Alipay, PayInterface.ChinaPnr };
        }

        public override Common.Enums.PlatformType PlatformInfo {
            get { return Common.Enums.PlatformType.Yeexing; }
        }
        internal override System.Text.Encoding Encoding {
            get { return System.Text.Encoding.UTF8; }
        }
        public override IEnumerable<PayInterface> AutoPayInterfaces {
            get { return _autoPayInterfaces; }
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

        internal string Address { get; private set; }
        internal string UserName { get; private set; }
        internal string Key { get; private set; }
        internal int Timeout { get; private set; }
        internal string NotifyUrl { get; private set; }
    }
}