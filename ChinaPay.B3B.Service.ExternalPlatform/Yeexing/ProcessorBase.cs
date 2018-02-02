using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ChinaPay.B3B.Service.ExternalPlatform.Yeexing {
    abstract class ProcessorBase {
        public Platform Platform { get { return Platform.Instance; } }

        protected string Sign(Dictionary<string, string> contents) {
            var sortedValue = getSortedValues(contents);
            return Sign(sortedValue);
        }
        protected string Sign(string contents) {
            var encodedValue = System.Web.HttpUtility.UrlEncode(contents + Platform.Key, Platform.Encoding);
            return Utility.MD5EncryptorService.MD5(encodedValue.ToUpper(), Platform.Encoding.BodyName);
        }
        private string getSortedValues(Dictionary<string, string> contents) {
            return string.Join("", from item in contents
                                   orderby item.Key
                                   select item.Value);
        }
        protected string GetAttributeValue(XmlNode node, string attrName) {
            var attr = node.Attributes[attrName];
            return attr == null ? string.Empty : attr.Value.Trim();
        }
    }
}