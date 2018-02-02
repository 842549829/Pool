using System;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.B3B.Service.Log.Domain;
using System.Web.Script.Serialization;

namespace ChinaPay.B3B.Service.ExternalPlatform._51book {
    abstract class RequestProcessorBase {
        public Platform Platform { get { return Platform.Instance; } }

        protected string Sign(Dictionary<string, string> contents) {
            var sortedValues = getSortedValues(contents);
            return Utility.MD5EncryptorService.MD5(sortedValues + Platform.SecurityCode).ToLower();
        }
        private string getSortedValues(Dictionary<string, string> contents) {
            return string.Join("", from item in contents
                                   orderby item.Key
                                   select item.Value);
        }

        protected void SaveRequestLog(string request, string response, string remark) {
            LogService.SaveExternalPlatformAlternatingLog(new ExternalPlatformAlternatingLog {
                Platform = Platform.PlatformInfo,
                Request = request,
                Response = response,
                Type = "请求",
                Remark = remark,
                Time = DateTime.Now
            });
        }

        protected string GetModelString(object obj) {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }
    }
}