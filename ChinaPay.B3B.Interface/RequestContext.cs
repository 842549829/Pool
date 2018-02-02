using System.Web;
using System.Xml;
using System.Collections.Generic;

namespace ChinaPay.B3B.Interface {
    internal class RequestContext {
        /// <summary>
        /// 解析请求上下文信息
        /// </summary>
        public static RequestContext Parse(string request) {
            try {
                var document = new XmlDocument();
                document.LoadXml(request);

                var requestIP = AddressLocator.IPAddressLocator.GetRequestIP(HttpContext.Current.Request).ToString();

                return new RequestContext {
                    UserName = getRequestData(document, "userName"),
                    Sign = getRequestData(document, "sign"),
                    Service = getRequestData(document, "service"),
                    Params = getRequestNode(document, "params"),
                    ClientIP = requestIP,
                    Original = request
                };
            } catch {
                return null;
            }
        }
        private static XmlNode getRequestNode(XmlDocument document, string key) {
            return document.SelectSingleNode("b3b/" + key);
        }
        private static string getRequestData(XmlDocument document, string key) {
            var node = getRequestNode(document, key);
            return node == null ? string.Empty : node.InnerText;
        }

        private Dictionary<string, string> _parameters = null;
        /// <summary>
        /// 获取业务参数值
        /// </summary>
        /// <param name="key">参数名</param>
        public string GetParameterValue(string key) {
            if(_parameters == null) {
                _parameters = new Dictionary<string, string>();
                if(Params != null) {
                    foreach(XmlNode item in Params.ChildNodes) {
                        _parameters.Add(item.Name, item.InnerText);
                    }
                }
            }
            string value;
            _parameters.TryGetValue(key, out value);
            return value;
        }

        public string UserName { get; private set; }
        public string Sign { get; private set; }
        public string Service { get; private set; }
        public XmlNode Params { get; private set; }
        public string ClientIP { get; private set; }
        public string Original { get; set; }
    }
}