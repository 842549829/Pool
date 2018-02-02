using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.XAPI.Service.Pid.Domain
{
    public static class DomainService
    { 
        /// <summary>
        /// 将数组编号转换成以逗号分隔的字符串
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string GetIds(int[] ids)
        {
            string str_ids = "";
            foreach (var item in ids)
            {
                if (str_ids != "")
                {
                    str_ids += ",";
                }
                str_ids += item;
            }
            return str_ids;
        }
    }
}
