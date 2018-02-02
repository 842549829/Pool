using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.Core;
using System.Text;

namespace ChinaPay.B3B.Service.ExternalPlatform.Processor {
    public abstract class PlatformBase {
        private Data.Cache<Common.Enums.PlatformType, Setting> _settingCache;
        protected PlatformBase() {
            _settingCache = new Data.Cache<Common.Enums.PlatformType, Setting> {
                Timeout = 300
            };
        }

        /// <summary>
        /// 平台
        /// </summary>
        public abstract Common.Enums.PlatformType PlatformInfo { get; }
        /// <summary>
        /// 编码格式
        /// </summary>
        internal abstract Encoding Encoding { get; }
        /// <summary>
        /// 支持的自动支付方式
        /// </summary>
        public abstract IEnumerable<PayInterface> AutoPayInterfaces { get; }
        /// <summary>
        /// 是否支持手动支付
        /// </summary>
        public virtual bool SuportManualPay() {
            return true;
        }
        /// <summary>
        /// 支付的手动支付方式
        /// </summary>
        public virtual IEnumerable<PayInterface> ManualPayInterfaces { get { return PayInterfaces.Keys; } }
        /// <summary>
        /// 支持的支付方式
        /// </summary>
        internal abstract Dictionary<PayInterface, string> PayInterfaces { get; }
        /// <summary>
        /// 设置信息
        /// </summary>
        public Setting Setting {
            get {
                var setting = _settingCache[PlatformInfo];
                if(setting == null) {
                    setting = ManageService.QuerySetting(PlatformInfo);
                    _settingCache.Add(PlatformInfo, setting);
                }
                return setting;
            }
        }

        internal string GetPayInterfaceValue(PayInterface payInterface) {
            if(PayInterfaces.ContainsKey(payInterface)) {
                return PayInterfaces[payInterface];
            } else {
                throw new NotSupportedException();
            }
        }
        internal PayInterface GetPayInterface(string value) {
            if(PayInterfaces.ContainsValue(value)) {
                return PayInterfaces.First(p => p.Value == value).Key;
            } else {
                return PayInterface.Virtual;
            }
        }

        internal abstract IPolicyProcessor GetPolicyProcessor();

        internal abstract IOrderProcessor GetOrderProcessor();

        public static PlatformBase GetPlatform(Common.Enums.PlatformType platform) {
            if(platform == Yeexing.Platform.Instance.PlatformInfo) {
                return Yeexing.Platform.Instance;
            } else if(platform == _517Na.Platform.Instance.PlatformInfo) {
                return _517Na.Platform.Instance;
            }
            throw new CustomException("不支持的外接平台");
        }
    }
}