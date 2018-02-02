using System;
using System.Collections.Generic;
using ChinaPay.B3B.Service.Log.Domain;
using ChinaPay.Core.Extension;
using System.Xml;

namespace ChinaPay.B3B.Service.ExternalPlatform._517Na {
    abstract class ProcessorBase {
        public Platform Platform { get { return _517Na.Platform.Instance; } }

        protected string GetRequest(string service, Dictionary<string, string> parameters) {
            var sign = Sign(parameters);
            return ConstructRequest(service, parameters, sign);
        }
        private string Sign(Dictionary<string, string> parameters) {
            var parameterContent = ConstructBusinessParameters(parameters);
            var signContent = parameterContent + Platform.UserName + Platform.EncriedPassword + DateTime.Today.ToString("yyyy-MM-dd") +
                              Platform.SecurityCode + Platform.PatternId;
            return Utility.MD5EncryptorService.MD5(signContent).ToUpper();
        }
        private string ConstructRequest(string service, Dictionary<string, string> parameters, string sign) {
            var xmlFormat = "<?xml version=\"1.0\" encoding=\"{0}\"?><request>" +
                            "<service>{1}</service>" +
                            "<pid>{2}</pid>" +
                            "<username>{3}</username>" +
                            "<sign>{4}</sign>" +
                            "<params>{5}</params></request>";
            var parameterContent = ConstructBusinessParameters(parameters);
            return string.Format(xmlFormat, Platform.Encoding.BodyName, service, Platform.PatternId, Platform.UserName, sign, parameterContent);
        }
        private string ConstructBusinessParameters(Dictionary<string, string> parameters) {
            return parameters.Join("", p => string.Format("<{0}>{1}</{0}>", p.Key, p.Value));
        }
        protected string GetPnrParameter(string textContent) {
            var replaced = textContent.ReplaceETermFlg('>')
                .RemoveSpecial()
                .Replace(" \r\n", " ").Replace("\r\n ", " ").Replace("\r\n", " ")
                .Replace(" \r", " ").Replace("\r ", " ").Replace("\r", " ")
                .Replace(" \n", " ").Replace("\n ", " ").Replace("\n", " ");
            return "<![CDATA[" + replaced + "]]>";
        }
        protected bool ResponseSuccess(XmlDocument doc, out string message) {
            var errorNode = doc.SelectSingleNode("/error");
            if(errorNode != null) {
                var errorMessageNode = errorNode.SelectSingleNode("//meg2");
                if(errorMessageNode == null || string.IsNullOrWhiteSpace(errorMessageNode.InnerText)) {
                    var errorCode = errorNode.SelectSingleNode("//code").InnerText.Trim();
                    if(!_errorMessages.TryGetValue(errorCode, out message)) {
                        message = "未知错误";
                    }
                } else {
                    var errorInfo = errorMessageNode.InnerText.Split('|');
                    message = errorInfo.Length == 2 ? errorInfo[1] : errorMessageNode.InnerText;
                }
                return false;
            }
            message = string.Empty;
            return true;
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
        protected string GetAttributeValue(XmlNode node, string attrName) {
            var attr = node.Attributes[attrName];
            return attr == null ? string.Empty : attr.Value.Trim();
        }

        private static Dictionary<string, string> _errorMessages = new Dictionary<string, string>
                                                                       {
                                                                           {"00000", "未知错误"},
                                                                           {"00001", "不允许此IP调用方法"},
                                                                           {"00002", "1分钟内多次取数据"},
                                                                           {"00003", "用户名为空"},
                                                                           {"00004", "参数不全"},
                                                                           {"00005", "校验码为空"},
                                                                           {"00006", "用户不存在"},
                                                                           {"00007", "安全码不存在"},
                                                                           {"00008", "校验码不正确"},
                                                                           {"00009", "无匹配数据"},
                                                                           {"00010", "部门无效"},
                                                                           {"00011", "操作员账号无效"},
                                                                           {"00012", "正在操作的订单的当前状况"},
                                                                           {"00014", "支付金额有误"},
                                                                           {"00015", "支付被禁止"},
                                                                           {"00016", "方法未授权"},
                                                                           {"00017", "账号或者IP未授权"},
                                                                           {"00018", "订单状态有误"},
                                                                           {"00019", "订单已被锁定"},
                                                                           {"00020", "用户无效"},
                                                                           {"00021", "无效的PNR"},
                                                                           {"00022", "无效的子政策"},
                                                                           {"00023", "不支持通过大编码创建订单"},
                                                                           {"00024", "无效的出票部门"},
                                                                           {"00025", "空的订单信息"},
                                                                           {"00026", "不属于您的订单"},
                                                                           {"00027", "参数错误"},
                                                                           {"00028", "PNR编码解析错误"},
                                                                           {"00029", "PNR编码不可用"},
                                                                           {"00030", "支付金额比计算值高，不生成订单"},
                                                                           {"00031", "未找到对应政策"},
                                                                           {"00032", "只能查看昨日和今日的订单"},
                                                                           {"00033", "禁止访问"},
                                                                           {"00034", "不能重复生成订单"},
                                                                           {"00035", "暂不出票理由为空"},
                                                                           {"00036", "暂不出票理由超长"},
                                                                           {"00037", "超过支付时间，不能支付"},
                                                                           {"00038", "传人的政策点不能高于平台的政策点"},
                                                                           {"00039", "成人PRN中有非成人类型"},
                                                                           {"00040", "儿童PRN中有非儿童类型"},
                                                                           {"00041", "创建儿童票儿童个数不能超过成人个数"},
                                                                           {"00042", "不匹配政策的有效时间限制"},
                                                                           {"00043", "不匹配政策的航班号"},
                                                                           {"00044", "不匹配政策的舱位"},
                                                                           {"00045", "不匹配政策的承运人"},
                                                                           {"00046", "不满足政策的班期限制（包含）"},
                                                                           {"00047", "不满足政策的班期限制"},
                                                                           {"00048", "PNR和政策的到达城市不匹配"},
                                                                           {"00049", "PNR和政策的出发城市不匹配"},
                                                                           {"00050", "政策的乘客类型不支持"},
                                                                           {"00051", "不支持婴儿类型"},
                                                                           {"00052", "PNR的乘客类型和政策不匹配"},
                                                                           {"00053", "不支持的航程类型"},
                                                                           {"00054", "不支持的航程类型"},
                                                                           {"00055", "没有符合条件的运价"},
                                                                           {"00056", "PRN上的起飞时间已过"},
                                                                           {"00057", "特价PNR不能创建订单"},
                                                                           {"00058", "共享航班"},
                                                                           {"00059", "PNR解析有误"},
                                                                           {"00060", "供应商已经下班，创建订单失败"},
                                                                           {"00061", "没有指定的票号"},
                                                                           {"00062", "机票不是预定状态"},
                                                                           {"00063", "超过废票时间"},
                                                                           {"00064", "不能重复提交退（废）票申请"},
                                                                           {"00065", "没有无密代付权限，需要联系平台开通"},
                                                                           {"00066", "没有无密代付账号，请自行添加可用账号"},
                                                                           {"00067", "参数与客户计算公式得到的支付价不相等"},
                                                                           {"00068", "参数与平台计算公式得到的支付价不相等"},
                                                                           {"00069", "参数与真实成人票面价不相等"},
                                                                           {"00070", "参数与真实儿童票面价不相等"},
                                                                           {"00071", "供应商不是在出票后分润"},
                                                                           {"00072", "创建儿童订单失败，取消成人订单"},
                                                                           {"00073", "创建儿童订单失败，取消成人订单失败"},
                                                                           {"00074", "创建订单失败，取消儿童订单失败"},
                                                                           {"00075", "创建订单失败，取消成人+儿童订单成功"},
                                                                           {"00076", "无效订单"},
                                                                           {"00077", "该订单已做过退改签操作"},
                                                                           {"00078", "开通废票不限时权限的订单记录订单操作日志失败"},
                                                                           {"00079", "机票已使用"},
                                                                           {"00080", "非当日机票"},
                                                                           {"00081", "供应商不在工作时间"},
                                                                           {"00082", "航班已起飞"},
                                                                           {"00083", "乘机人数量不匹配"},
                                                                           {"00084", "行程单座位未取消"},
                                                                           {"00085", "座位未取消"},
                                                                           {"00086", "行程单未取消"},
                                                                           {"00087", "当前系统忙碌"},
                                                                           {"00088", "传入理由有误"},
                                                                           {"00089", "支付方式不匹配，通常发生在客户绑定财付通支付时，供应商没有绑定财付通收款账号"},
                                                                           {"00090", "未找到相应乘机人"},
                                                                           {"00091", "退票验证失败"},
                                                                           {"00092", "订单中不包含对应票号"},
                                                                           {"00093", "退票理由内容和理由ID不匹配"},
                                                                           {"00094", "订单状态是不可做退票操作状态"},
                                                                           {"00095", "理由不存在"},
                                                                           {"00096", "上传文件超过指定大小"},
                                                                           {"00097", "上传文件格式错误"},
                                                                           {"00098", "超过上传文件数量限定"},
                                                                           {"00099", "提交退票失败"},
                                                                           {"00100", "理由对应附件不存在"},
                                                                           {"00101", "机票状态是不可做退票操作状态"},
                                                                           {"99999", "该方法使用次数已经用完"},
                                                                           {"-1", "未知错误"},
                                                                           {"00107", "错误PID"}
                                                                       };
    }
}