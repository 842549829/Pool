using System;
using ChinaPay.B3B.DataTransferObject;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using System.Linq;
using System.Collections.Generic;

namespace ChinaPay.B3B.Service.Organization.Domain
{
    public class OEMContract
    {
        static object locker = new object();
        private static OEMContract _B3BDefault;
        public static OEMContract B3BDefault {
            get {
                if (_B3BDefault==null)
                {
                    lock (locker)
                    {
                        if (_B3BDefault==null)
                        {
                            string qq = getOemContractValue("EnterpriseQQ");
                            string fax = getOemContractValue("Fax");
                            string servicePhone = getOemContractValue("ServicePhone");
                            string refundPhone = getOemContractValue("RefundPhone");
                            string scrapPhone = getOemContractValue("ScrapPhone");
                            string payServicePhone = getOemContractValue("PayServicePhone");
                            string emergencyPhone = getOemContractValue("EmergencyPhone");
                            string complainPhone = getOemContractValue("ComplainPhone");
                            string printTicketPhone = getOemContractValue("PrintTicketPhone");
                            _B3BDefault = new OEMContract(qq,fax,servicePhone,refundPhone,scrapPhone,payServicePhone,emergencyPhone,complainPhone,printTicketPhone);
                        }
                    }
                }
                return _B3BDefault;
            }
        }
        private static string getOemContractValue(string name) {
            IEnumerable<SystemDictionaryItem> oemContract = SystemDictionaryService.Query(SystemDictionaryType.OEMContract);
            SystemDictionaryItem result = oemContract.Where(r => r.Name == name).FirstOrDefault();
            return result != null && !string.IsNullOrEmpty(result.Value) ? result.Value : string.Empty;
        }
        public OEMContract() {
            RefreshService.ServicePhoneChanged += FlushDefaultPhone;
        }
        void FlushDefaultPhone() {
            _B3BDefault = null;
        }

        internal OEMContract(string qq,string fax,string servicePhone,string refundPhone,string scrapPhone,
            string payServicePhone, string emergencyPhone, string complainPhone, string printTicketPhone, bool allowPlatformContractPurchaser, bool useB3BServicePhone)
        {
                EnterpriseQQ = qq;
                Fax = fax;
                ServicePhone = servicePhone;
                RefundPhone = refundPhone;
                ScrapPhone = scrapPhone;
                PayServicePhone = payServicePhone;
                EmergencyPhone = emergencyPhone;
                ComplainPhone = complainPhone;
                PrintTicketPhone = printTicketPhone;
                AllowPlatformContractPurchaser = allowPlatformContractPurchaser;
                UseB3BServicePhone = useB3BServicePhone;
                RefreshService.ServicePhoneChanged += FlushDefaultPhone;
        }
        internal OEMContract(string qq, string fax, string servicePhone, string refundPhone, string scrapPhone,
            string payServicePhone,string emergencyPhone, string complainPhone, string printTicketPhone)
            : this(qq, fax, servicePhone, refundPhone, scrapPhone, payServicePhone, emergencyPhone, complainPhone, printTicketPhone, true, true) { }
        /// <summary>
        /// OEMID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 企业QQ
        /// </summary>
        public string EnterpriseQQ { get; set; }

        /// <summary>
        /// 儿童机票受理传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 票务综合处理电话
        /// </summary>
        public string ServicePhone { get; set; }

        /// <summary>
        /// 退票电话
        /// </summary>
        public string RefundPhone { get; set; }
        /// <summary>
        /// 废票电话
        /// </summary>
        public string ScrapPhone { get; set; }
        /// <summary>
        /// 支付相关服务电话
        /// </summary>
        public string PayServicePhone { get; set; }
        /// <summary>
        /// 紧急业务受理电话
        /// </summary>
        public string EmergencyPhone { get; set;}
        /// <summary>
        /// 投诉监督电话
        /// </summary>
        public string ComplainPhone { get; set; }
        /// <summary>
        /// 出票电话
        /// </summary>
        public string PrintTicketPhone { get; set; }
        /// <summary>
        /// 是否允许平台联系采购
        /// </summary>
        public bool AllowPlatformContractPurchaser { get; set; }
        /// <summary>
        /// 是否采用B3B客服电话
        /// </summary>
        public bool UseB3BServicePhone { get; set; }
        public static  string ContractInfoReplace(string content)
        {
            return content.Replace("_", "").Replace(" ", "").Replace("&nbsp;", "");
        }
    }
}