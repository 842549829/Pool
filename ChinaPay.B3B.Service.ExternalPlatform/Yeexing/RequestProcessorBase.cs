using System;
using System.Collections.Generic;
using System.Xml;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core.Extension;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.ExternalPlatform.Yeexing {
    abstract class RequestProcessorBase : ProcessorBase {
        private string _errorMessagePattern = @"^(?<code>\d*)\^(?<msg>.*?)$";

        protected bool ResponseSuccess(XmlDocument doc) {
            var responseStatusNode = doc.SelectSingleNode("/result/is_success");
            return responseStatusNode != null && responseStatusNode.InnerText == "T";
        }
        protected bool ResponseSuccess(XmlDocument doc, out string message) {
            var responseStatusNode = doc.SelectSingleNode("/result/is_success");
            if(responseStatusNode != null && responseStatusNode.InnerText == "T") {
                message = string.Empty;
                return true;
            } else {
                var errorNode = doc.SelectSingleNode("/result/error");
                message = errorNode == null ? string.Empty : GetErrorMessage(errorNode.InnerText);
                return false;
            }
        }

        protected string GetRequestValue(Dictionary<string, string> contents, string sign) {
            var result = contents.Join("&", item => item.Key + "=" + item.Value);
            if(result.Length > 0) {
                result += "&sign=" + sign;
            }
            return result;
        }

        protected string GetPnrParameter(string rtContent, string patContent) {
            return (rtContent + patContent).ReplaceETermFlg('>')
                .RemoveSpecial()
                .Replace(" \r\n", " ").Replace("\r\n ", " ").Replace("\r\n", " ")
                .Replace(" \r", " ").Replace("\r ", " ").Replace("\r", " ")
                .Replace(" \n", " ").Replace("\n ", " ").Replace("\n", " ").Trim();
        }

        protected void SaveRequestLog(string response, string request, string remark) {
            LogService.SaveExternalPlatformAlternatingLog(new ExternalPlatformAlternatingLog {
                Platform = Platform.PlatformInfo,
                Request = request,
                Response = response,
                Type = "请求",
                Remark = remark,
                Time = DateTime.Now
            });
        }

        protected string GetErrorMessage(string message) {
            var match = Regex.Match(message, _errorMessagePattern);
            if(match.Success) {
                return match.Groups["msg"].Value;
            } else {
                return message;
            }
        }
        protected string GetErrorCode(string message) {
            var match = Regex.Match(message, _errorMessagePattern);
            if(match.Success) {
                return match.Groups["code"].Value;
            } else {
                return message;
            }
        }
    }
}