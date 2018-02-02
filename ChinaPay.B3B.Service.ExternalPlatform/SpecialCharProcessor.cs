using System.Text;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.Service.ExternalPlatform {
    static class SpecialCharProcessor {
        /// <summary>
        /// 替换黑屏中的指令标识符
        /// </summary>
        public static string ReplaceETermFlg(this string value, char newContent) {
            if(string.IsNullOrWhiteSpace(value)) return value;
            return value.Replace((char)16, '>');
        }
        /// <summary>
        /// 去掉已出票的编码内容标识
        /// </summary>
        public static string RemovePrintedContent(this string value) {
            if(string.IsNullOrWhiteSpace(value)) return value;
            return Regex.Replace(value, @"\s*\**ELECTRONIC TICKET PNR\**", "");
        }
        /// <summary>
        /// 去掉网页式黑屏的指令特殊内容
        /// </summary>
        public static string RemoveETermSpecialContentOnWeb(this string value) {
            if(string.IsNullOrWhiteSpace(value)) return value;
            return Regex.Replace(value, @"\s*服务器耗时[^>]*秒", "");
        }
        /// <summary>
        /// 去掉特殊字符
        /// </summary>
        public static string RemoveSpecial(this string value) {
            if(string.IsNullOrWhiteSpace(value)) return value;
            var result = new StringBuilder();
            foreach(var item in value) {
                if(!isSpecialChar(item)) {
                    result.Append(item);
                }
            }
            return result.ToString();
        }
        private static bool isSpecialChar(char value) {
            if(1 <= value && value <= 6) return true;
            if(14 <= value && value <= 31) return true;
            return false;
        }
    }
}