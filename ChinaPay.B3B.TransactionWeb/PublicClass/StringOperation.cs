using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChinaPay.B3B.TransactionWeb.PublicClass
{
    public static class StringOperation
    {
        /// <summary>   
        /// 每隔"interval"个字符插入一个字符   
        /// </summary>   
        /// <param name="input">源字符串</param>   
        /// <param name="interval">间隔字符数</param>   
        /// <param name="value">待插入值</param>   
        /// <returns>返回新生成字符串</returns>   
        public static string InsertFormat(string input, int interval, string value)
        {
            for (int i = interval; i < input.Length; i += interval + value.Length)
                input = input.Insert(i, value);
            return input;
        }

        /// <summary>
        /// 将数字转换成中文的星期一、星期二等
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string TransferToChinese(string input)
        {
           
            string result = "";
            if (input == null||input ==""||input =="1,2,3,4,5,6,7") return "不限";
            IEnumerable<string> chineseWeeks = input.Split(',').Distinct();
            foreach (var item in chineseWeeks)
            {
                switch (item)
                {
                    case "1":
                        result += "周一/";
                        break;
                    case "2":
                        result += "周二/";
                        break;
                    case "3":
                        result += "周三/";
                        break;
                    case "4":
                        result += "周四/";
                        break;
                    case "5":
                        result += "周五/";
                        break;
                    case "6":
                        result += "周六/";
                        break;
                    case "7":
                        result += "周日/";
                        break;
                }
            }
            if (result.Length >= 3)
            {
                result = result.Remove(result.Length - 1, 1);
            }
            return result;
        }
    }
}