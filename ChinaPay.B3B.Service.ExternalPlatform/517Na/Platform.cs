using System;
using System.Collections.Generic;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.Service.ExternalPlatform.Processor;

namespace ChinaPay.B3B.Service.ExternalPlatform._517Na {
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
            Address = System.Configuration.ConfigurationManager.AppSettings["517NaAddress"];
            UserName = System.Configuration.ConfigurationManager.AppSettings["517NaUserName"];
            Password = System.Configuration.ConfigurationManager.AppSettings["517NaPassword"];
            EncriedPassword = Utility.MD5EncryptorService.MD5(Password).ToUpper();
            SecurityCode = System.Configuration.ConfigurationManager.AppSettings["517NaSecurityCode"];
            NotifyCode = System.Configuration.ConfigurationManager.AppSettings["517NaNotifyCode"];
            PatternId = System.Configuration.ConfigurationManager.AppSettings["517NaPatternId"];
            Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings["517NaTimeout"]);
            Contact = System.Configuration.ConfigurationManager.AppSettings["517NaContact"];
            ContactPhone = System.Configuration.ConfigurationManager.AppSettings["517NaContactPhone"];

            _payInterfaces = new Dictionary<PayInterface, string>
                             {
                                 {PayInterface.Alipay, "1"}
                             };
            _autoPayInterfaces = new[] { PayInterface.Alipay };
        }

        public override PlatformType PlatformInfo {
            get { return PlatformType.CD517; }
        }
        internal override System.Text.Encoding Encoding {
            get { return System.Text.Encoding.UTF8; }
        }
        public override IEnumerable<PayInterface> AutoPayInterfaces {
            get { return _autoPayInterfaces; }
        }
        public override IEnumerable<PayInterface> ManualPayInterfaces {
            get {
                return new PayInterface[] { };
            }
        }
        internal override Dictionary<PayInterface, string> PayInterfaces {
            get { return _payInterfaces; }
        }
        public override bool SuportManualPay() {
            return false;
        }
        internal override IPolicyProcessor GetPolicyProcessor() {
            return new PolicyProcessor();
        }
        internal override IOrderProcessor GetOrderProcessor() {
            return new OrderProcessor();
        }

        internal string Address { get; private set; }
        internal string UserName { get; private set; }
        internal string Password { get; private set; }
        internal string EncriedPassword { get; private set; }
        internal string SecurityCode { get; private set; }
        internal string NotifyCode { get; private set; }
        internal string PatternId { get; private set; }
        internal int Timeout { get; private set; }
        internal string Contact { get; private set; }
        internal string ContactPhone { get; private set; }
    }
}