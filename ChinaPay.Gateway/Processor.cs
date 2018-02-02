using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ChinaPay.Core.Extension;
using ChinaPay.Utility;

namespace ChinaPay.Gateway
{
    public abstract class Processor
    {
        protected const string SignKey = "sign";
        protected const string DefaultEncodingName = "UTF-8";

        /// <summary>
        /// 商户号
        /// </summary>
        protected static readonly string PatternCode = ConfigurationManager.AppSettings["PoolPayPatternCode"];

        /// <summary>
        /// 商户密钥
        /// </summary>
        private static readonly string PatternKey = ConfigurationManager.AppSettings["PoolPayPatternKey"];

        /// <summary>
        /// 签名
        /// </summary>
        protected string Sign(Dictionary<string, string> parameters)
        {
            IEnumerable<string> sortedValues = from p in parameters
                                               where p.Key != SignKey
                                               orderby p.Key
                                               select p.Value;
            string signContent = sortedValues.Join("") + PatternKey;
            return MD5EncryptorService.MD5(signContent);
        }
    }
}