using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Policy;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.ExternalPlatform {
    public static class PolicyService {
        /// <summary>
        /// 通过编码获取政策
        /// </summary>
        /// <param name="pnrPair">编码</param>
        /// <param name="filter">过滤条件</param>
        public static RequestResult<IEnumerable<ExternalPolicyView>> Match(PNRPair pnrPair, ExternalPolicyFilter filter) {
            if(filter == null) throw new ArgumentNullException("filter");
            if(PNRPair.IsNullOrEmpty(pnrPair)) throw new ArgumentNullException("pnrPair");

            var matchedResult = new List<ExternalPolicyView>();
            var platforms = new List<Processor.PlatformBase> { Yeexing.Platform.Instance };
            var validPlatforms = platforms.Where(p => p.Setting != null && p.Setting.Enabled).ToList();
            if(validPlatforms.Count > 1) {
                Parallel.ForEach(validPlatforms, platform => {
                    var processor = platform.GetPolicyProcessor();
                    var matchResult = processor.Match(pnrPair, filter);
                    if(matchResult.Success) {
                        matchedResult.AddRange(matchResult.Result);
                    }
                });
            } else {
                foreach(var platform in validPlatforms) {
                    var processor = platform.GetPolicyProcessor();
                    var matchResult = processor.Match(pnrPair, filter);
                    if(matchResult.Success) {
                        matchedResult.AddRange(matchResult.Result);
                    }
                }
            }
            return new RequestResult<IEnumerable<ExternalPolicyView>> {
                Success = true,
                Result = matchedResult
            };
        }
        /// <summary>
        /// 通过编码内容和pat内容获取政策
        /// </summary>
        /// <param name="pnrPair">编码</param>
        /// <param name="pnrContent">编码内容</param>
        /// <param name="patContent">PAT内容</param>
        /// <param name="filter">过滤条件</param>
        public static RequestResult<IEnumerable<ExternalPolicyView>> Match(PNRPair pnrPair, string pnrContent, string patContent, ExternalPolicyFilter filter) {
            if(filter == null) throw new ArgumentNullException("filter");

            if(string.IsNullOrWhiteSpace(pnrContent)) {
                return new RequestResult<IEnumerable<ExternalPolicyView>> {
                    Success = false,
                    ErrMessage = "缺少rt内容"
                };
            } else if(string.IsNullOrWhiteSpace(patContent)) {
                return new RequestResult<IEnumerable<ExternalPolicyView>> {
                    Success = false,
                    ErrMessage = "缺少pat内容"
                };
            } else {
                pnrContent = pnrContent.RemovePrintedContent().RemoveETermSpecialContentOnWeb();
                var matchedResult = new List<ExternalPolicyView>();
                var platforms = new List<Processor.PlatformBase> { Yeexing.Platform.Instance, _517Na.Platform.Instance };
                var validPlatforms = platforms.Where(p => p.Setting != null && p.Setting.Enabled).ToList();
                if(validPlatforms.Count > 1) {
                    Parallel.ForEach(validPlatforms, platform => {
                        var processor = platform.GetPolicyProcessor();
                        var matchResult = processor.Match(pnrPair, pnrContent, patContent, filter);
                        if(matchResult.Success) {
                            matchedResult.AddRange(matchResult.Result);
                        }
                    });
                } else {
                    foreach(var platform in validPlatforms) {
                        var processor = platform.GetPolicyProcessor();
                        var matchResult = processor.Match(pnrPair, pnrContent, patContent, filter);
                        if(matchResult.Success) {
                            matchedResult.AddRange(matchResult.Result);
                        }
                    }
                }
                return new RequestResult<IEnumerable<ExternalPolicyView>> {
                    Success = true,
                    Result = matchedResult
                };
            }
        }
        /// <summary>
        /// 查询外平台政策
        /// </summary>
        public static IEnumerable<ExternalPolicyLog> QueryExternalPolicyLogs(decimal? orderId, DateTime stratDate, DateTime endDate, ChinaPay.Core.Pagination pagination) {
            return Repository.Factory.CreateSettingReposity().QueryExternalPolicys(orderId, stratDate, endDate, pagination);
        }
    }
}