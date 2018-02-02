using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using ChinaPay.AddressLocator;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core;

namespace ChinaPay.B3B.TransactionWeb.PublicClass
{
    /// <summary>
    /// 签名算法类
    /// </summary>
    public class Signature
    {
        private static string m_key; 
        private string m_time;

        static Signature() {
            m_key = System.Configuration.ConfigurationManager.AppSettings["SignKey"];
        }
        public Signature(string time)
        {
            if (string.IsNullOrEmpty(m_key)) throw new ApplicationException("未找到SignKey配置节点");
            m_time = time;
        }
        /// <summary>
        /// 获取签名
        /// </summary>
        public string GetSign(string account, string type)
        {
            string sign = account + type + m_time + m_key;
            return Utility.MD5EncryptorService.MD5FilterZero(sign, "UTF-8");
        }

        public static string GetSignture(string time, string inComeAccount, decimal amount) {
            var sign = string.Format("{0}{1}{2}{3}", inComeAccount, amount, time, m_key);
            return Utility.MD5EncryptorService.MD5FilterZero(sign, "UTF-8");
        }
    }
    /// <summary>
    /// 验证手机验证码
    /// </summary>
    public static class Verification {
        public static bool VerificationCode(string code, string sessionId)
        {
            bool IsVerify = true;
            if (HttpContext.Current.Session[sessionId] == null)
            {
                IsVerify = false;
                throw new CustomException("验证码已经过期或验证码不存在");
            }
            if (HttpContext.Current.Session[sessionId].ToString().ToUpper() != code.ToUpper())
            {
                IsVerify = false;
                throw new IndexOutOfRangeException("验证码错误");
            }
            return IsVerify;
        }
    }
    /// <summary>
    /// 验证IP地址是否可以获取验证码
    /// </summary>
    public static class VerificationIPAddress
    {
        public static bool VerificationIP()
        {
            bool IsVerfication = true;
            IPAddress ip = IPAddressLocator.GetRequestIP(HttpContext.Current.Request);
            if (!AccountCombineService.ValidateIP(ip.ToString()))
            {
                IsVerfication = false;
                throw new IndexOutOfRangeException("同一个IP一天只能申请3个账号");
            }
            return IsVerfication;
        }
    }
}