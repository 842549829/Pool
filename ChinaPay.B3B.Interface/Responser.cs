using System;
using System.Xml;
using ChinaPay.Core;

namespace ChinaPay.B3B.Interface {
    internal static class Responser {
        private static XmlDocument _noneProcessorResponse = null;
        /// <summary>
        /// 无有效请求处理器的响应
        /// </summary>
        public static XmlDocument NoneProcessorResponse {
            get {
                if(_noneProcessorResponse == null) {
                    _noneProcessorResponse = Format("79", string.Empty, string.Empty);
                }
                return _noneProcessorResponse;
            }
        }
        private static XmlDocument _errorContextResponse = null;
        /// <summary>
        /// 无效上下文信息的响应
        /// </summary>
        public static XmlDocument ErrorContextResponse {
            get {
                if(_errorContextResponse == null) {
                    _errorContextResponse = Format("79", string.Empty, string.Empty);
                }
                return _errorContextResponse;
            }
        }
        public static XmlDocument Format(string result) {
            return Format("0", string.Empty, result);
        }
        public static XmlDocument Format(InterfaceInvokeException exception) {
            return Format(exception.Code, exception.Parameter, string.Empty);
        }
        public static XmlDocument Format(CustomException exception) {
            return Format("9", exception.Message, string.Empty);
        }
        public static XmlDocument Format(Exception exception) {
            if(exception is InterfaceInvokeException) return Format(exception as InterfaceInvokeException);
            if(exception is CustomException) return Format(exception as CustomException);
            return Format("99", string.Empty, string.Empty);
        }
        /// <summary>
        /// 格式化请求响应结果
        /// </summary>
        public static XmlDocument Format(string code, string parameter, string result) {
            var doc = new XmlDocument();
            var message = Message.Get(code, parameter);
            doc.LoadXml(string.Format("<b3b><code>{0}</code><message>{1}</message>{2}</b3b>", code, message, result));
            return doc;
        }
    }
}