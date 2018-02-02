using System.Collections.Generic;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Policy;

namespace ChinaPay.B3B.Service.ExternalPlatform.Processor {
    interface IPolicyProcessor {
        /// <summary>
        /// 根据编码进行匹配
        /// </summary>
        RequestResult<IEnumerable<ExternalPolicyView>> Match(PNRPair pnrPair, ExternalPolicyFilter filter);
        /// <summary>
        /// 根据编码内容和pat内容进行匹配
        /// </summary>
        RequestResult<IEnumerable<ExternalPolicyView>> Match(PNRPair pnrPair, string pnrContent, string patContent, ExternalPolicyFilter filter);
    }
}