using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.Gateway
{
    /// <summary>
    /// 通知处理类
    /// </summary>
    public abstract class NotifyProcessor : Processor
    {
        public const string ResponseFormat = "<b3b><success>{0}</success><message>{1}</message></b3b>";
        private Dictionary<string, string> _parameters;

        protected NotifyProcessor(HttpRequest request) { Request = request; }

        /// <summary>
        /// 请求信息
        /// </summary>
        protected HttpRequest Request { get; private set; }

        /// <summary>
        /// 请求参数
        /// </summary>                                                                                                                                                                                                                                                                                                                              
        protected Dictionary<string, string> Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    parseRequest();
                }
                return _parameters;
            }
        }

        private Encoding Decorder
        {
            get
            {
                string encodeName = Parameters.ContainsKey("charset") ? Parameters["charset"] : DefaultEncodingName;
                return Encoding.GetEncoding(encodeName);
            }
        }

        /// <summary>
        /// 处理结果提示
        /// </summary>
        public string Message { get; private set; }

        public abstract TradementBusinessType BusinessType { get; }

        /// <summary>
        /// 处理通知
        /// </summary>
        public bool Execute()
        {
            string response = string.Empty;
            string requestContent = Parameters.Join("&", p => p.Key + "=" + p.Value);
            if (Validate())
            {
                try
                {
                    ParseCore();
                    SaveLog(requestContent, response);
                    Message = "成功";
                    return true;
                }
                catch (Exception e)
                {
                    Message = e.Message;
                    LogService.SaveExceptionLog(e, "通知处理");
                    //LogService.SaveTextLog("通知处理失败。请求参数:" + requestContent + Environment.NewLine + "失败原因:" + e.Message);
                }
            }
            else
            {
                Message = "签名验证失败";
                //LogService.SaveTextLog("通知签名验证失败。请求参数:" + requestContent);
            }
            return false;
        }

        /// <summary>
        /// 验证合法性
        /// </summary>
        protected bool Validate()
        {
            if (!Parameters.ContainsKey(SignKey)) return false;
            string signValue = Parameters[SignKey];
            string localSingValue = Sign(Parameters);
            return signValue == localSingValue;
        }

        protected abstract void ParseCore();
        protected virtual void SaveLog(string request, string response) { }

        /// <summary>
        /// 解析请求信息
        /// </summary>
        private void parseRequest()
        {
            var parameters = new Dictionary<string, string>();
            foreach (string pKey in Request.QueryString.AllKeys)
            {
                if (!parameters.ContainsKey(pKey))
                {
                    parameters.Add(pKey, Request.Params.Get(pKey));
                }
            }
            _parameters = parameters;
        }

        protected string UrlDecode(string content) { return HttpUtility.UrlDecode(content, Decorder); }
    }
}