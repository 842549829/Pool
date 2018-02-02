using System.Collections.Generic;

namespace ChinaPay.B3B.Interface {
    internal static class Message {
        private static Dictionary<string, string> _formats;

        static Message() {
            _formats = new Dictionary<string, string>
                           {
                               {"0", "成功"},
                               {"1", "参数[{0}]格式错误"},
                               {"2", "用户名不存在"},
                               {"3", "签名错误"},
                               {"4", "用户被禁用"},
                               {"5", "单位被禁用"},
                               {"6", "未开通接口"},
                               {"7", "缺少接口设置信息"},
                               {"8", "参数[{0}]不能为空" },
                               {"9", "{0}" },
                               {"10", "无权限" },
                               {"11", "没有获取到相关政策" },
                               {"79", "无效的接口名称"},
                               {"80", "访问过于频繁"},
                               {"99", "系统错误"}
                           };
        }

        public static string Get(string code, params string[] args) {
            if(_formats.ContainsKey(code)) {
                var format = _formats[code];
                return string.Format(format, args);
            }
            return "未知";
        }
    }
}