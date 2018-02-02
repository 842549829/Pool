using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Domain.Utility
{
    internal class ErrorMessageUtil
    {
        private static readonly Dictionary<string, string> ErrorMessages;

        static ErrorMessageUtil()
        {
            ErrorMessages = new Dictionary<string, string>
                                {
                                    {"INFANT", "缺少婴儿标识。"},
                                    {"INVALID CHAR", "姓名中存在非法字符，或终端参数设置有误。"},
                                    {"NAME LENGTH", "姓名超长或姓氏少于两个字符。"},
                                    {"PLS NM1XXXX/XXXXXX", "姓名中应加斜线(/)，或斜线数量不正确。"},
                                    {"SEATS", "座位数与姓名数不符，可RT 检查当前的PNR。"},
                                    {"NO NAME CHANGE FOR MU/Y", "某航空公司不允许修改姓名。"},
                                    {"ACTION", "行动代码不正确。"},
                                    {"SEGMENT", "城市对输入无效。"},
                                    {"TIME", "航班号不正确。"},
                                    {"SCH NBR", "航线序号不符。"},
                                    {"NO PNR", "编码不存在。"},
                                    {"**NO DIRECT ROUTING**", "没有直达航班。"},
                                    {"THIS PNR WAS ENTIRELY CANCELLED", "编码已被取消。"}
                                };
        }
        
        internal static string ReplaceErrorMessage(string str)
        {
            foreach(var errorMsg in ErrorMessages)
            {
                if (str.Contains(errorMsg.Key))
                {
                    return errorMsg.Value;
                }
            }
            
            return str;
        }


    }
}
