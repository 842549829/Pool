using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaPay.B3B.Service.Command.Domain.Utility
{
    public class ContentBulider
    {
        /// <summary>
        /// 根据给出的舱位等级、票面价、机场建设费及燃油附加费，反向模拟PAT的内容信息
        /// </summary>
        /// <param name="classOfService">舱位等级</param>
        /// <param name="fare">票面价</param>
        /// <param name="airportTax">机场建设费</param>
        /// <param name="bunkerAdjustmentFactor">燃油附加费</param>
        /// <returns>价格信息</returns>
        public static string GetPatString(string classOfService, decimal fare, decimal airportTax, decimal bunkerAdjustmentFactor)
        {
            var total = fare + airportTax + bunkerAdjustmentFactor;
            return string.Format("01 {0}+{0} FARE:CNY{1:0.00} TAX:CNY{2:0.00} YQ:CNY{3:0.00}  TOTAL:{4:0.00}\n>SFC:01\n", classOfService, fare, airportTax, bunkerAdjustmentFactor, total);
        }
    }
}
