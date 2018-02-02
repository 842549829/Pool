using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Xml;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.Utility;

namespace ChinaPay.Gateway
{
    public abstract class RequestProcessor : Processor
    {
        private static readonly string RequestDomain = ConfigurationManager.AppSettings["PoolPayRequestDomain"];
        private static readonly Encoding Encoder = Encoding.GetEncoding(DefaultEncodingName);
        protected bool Success;

        /// <summary>
        /// 处理结果提示
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 处理结果信息
        /// </summary>
        public string Result { get; private set; }

        protected abstract string Target { get; }

        protected virtual bool RequireResquest
        {
            get { return true; }
        }

        public abstract TradementBusinessType BusinessType { get; }

        /// <summary>
        /// 执行请求操作
        /// </summary>
        public bool Execute()
        {
            Success = false;
            string response = string.Empty;
            Dictionary<string, string> parameters = constructParameters();
            string request = string.Format("{0}/{1}?{2}", RequestDomain, Target, parameters.Join("&", p => p.Key + "=" + UrlEncode(p.Value)));
            if (RequireResquest)
            {
                try
                {
                    Result = response = HttpRequestUtility.GetHttpResult(request, 10000);
                    Success = parseResponse(response);
                }
                catch (Exception e)
                {
                    Message = "失败。" + e.Message;
                    LogService.SaveExceptionLog(e, "请求");
                }
            }
            else
            {
                Result = response = request;
                Success = true;
                Message = "成功";
            }
            //LogService.SaveTextLog(request + "\r\n\r\n" + Result); 
            SaveLog(request, response, DateTime.Now);
            return Success;
        }

        protected abstract Dictionary<string, string> ConstructParametersCore();
        protected abstract void ParseResponseCore(XmlDocument doc);
        protected virtual void SaveLog(string request, string response, DateTime time) { }

        private Dictionary<string, string> constructParameters()
        {
            Dictionary<string, string> parameters = ConstructParametersCore();
            parameters.Add("pattern", PatternCode);
            parameters.Add("charset", Encoder.BodyName);
            parameters.Add("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add(SignKey, Sign(parameters));
            return parameters;
        }

        protected string UrlEncode(string content) { return HttpUtility.UrlEncode(content, Encoder); }

        private bool parseResponse(string response)
        {
            var doc = new XmlDocument();
            doc.LoadXml(response);
            string status = GetXmlNodeValue(doc.SelectSingleNode("/poolpay/success"));
            Message = GetXmlNodeValue(doc.SelectSingleNode("/poolpay/message"));

            if (status == "T")
            {
                ParseResponseCore(doc);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected string GetXmlNodeValue(XmlNode node) { return node == null ? string.Empty : node.InnerText; }
    }
}