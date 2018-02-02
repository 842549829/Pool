using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Organization.Domain;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingHandlers
{
    /// <summary>
    /// IncomeLimitGroup 的摘要说明
    /// </summary>
    public class IncomeLimitGroup : BaseHandler
    {
        public bool InsertIncomeLimitGroupGobal(byte type, string str, string remark)
        {
            IncomeGroupLimitGroup g = new IncomeGroupLimitGroup()
            {
                Id = Guid.NewGuid(),
                CompanyId = CurrentCompany.CompanyId,
                IncomeGroupId = null,
                Remark = remark,
                Limitation = new List<IncomeGroupLimit>()
            };
            var limitStr = str.Split('^');
            if (type == 2)
            {

                for (int i = 0; i < limitStr.Length; i++)
                {
                    var currLimit = limitStr[i].Split(',');
                    var limit = new IncomeGroupLimit();
                    limit.Id = Guid.NewGuid();
                    limit.IncomeId = g.Id;
                    limit.IsOwnerPolicy = currLimit[0] == "1";
                    limit.Airlines = currLimit[1];
                    limit.Type = (PeriodType)byte.Parse(currLimit[2]);
                    limit.Price = int.Parse(currLimit[4]);
                    limit.Period = new List<IncomeGroupPeriod>();
                    if (limit.Type == PeriodType.Interval)
                    {
                        var currPeriod = currLimit[3].Split(';');
                        for (int j = 0; j < currPeriod.Length; j++)
                        {
                            var currValue = currPeriod[j].Split('|');
                            limit.Period.Add(new IncomeGroupPeriod { DeductId = limit.Id, StartPeriod = decimal.Parse(currValue[0]), EndPeriod = decimal.Parse(currValue[1]), Period = decimal.Parse(currValue[2]) });
                        }
                    }
                    else
                    {
                        limit.Period.Add(new IncomeGroupPeriod { DeductId = limit.Id, StartPeriod = 0, EndPeriod = 1, Period = decimal.Parse(currLimit[3]) });
                    }
                    g.Limitation.Add(limit);
                }
            }
            IncomeGroupLimitService.InsertIncomeGroupLimitGlobal((IncomeGroupLimitType)type, g);
            return true;
        }

        public bool InsertIncomeLimitGroup(string incomgroupId, string str, string remark)
        {
            IncomeGroupLimitGroup g = new IncomeGroupLimitGroup()
            {
                Id = Guid.NewGuid(),
                CompanyId = CurrentCompany.CompanyId,
                IncomeGroupId = Guid.Parse(incomgroupId),
                Remark = remark,
                Limitation = new List<IncomeGroupLimit>()
            };
            var limitStr = str.Split('^');
            for (int i = 0; i < limitStr.Length; i++)
            {
                var currLimit = limitStr[i].Split(',');
                var limit = new IncomeGroupLimit();
                limit.Id = Guid.NewGuid();
                limit.IncomeId = g.Id;
                limit.IsOwnerPolicy = currLimit[0] == "1";
                limit.Airlines = currLimit[1];
                limit.Type = (PeriodType)byte.Parse(currLimit[2]);
                limit.Price = int.Parse(currLimit[4]);
                limit.Period = new List<IncomeGroupPeriod>();
                if (limit.Type == PeriodType.Interval)
                {
                    var currPeriod = currLimit[3].Split(';');
                    for (int j = 0; j < currPeriod.Length; j++)
                    {
                        var currValue = currPeriod[j].Split('|');
                        limit.Period.Add(new IncomeGroupPeriod { DeductId = limit.Id, StartPeriod = decimal.Parse(currValue[0]), EndPeriod = decimal.Parse(currValue[1]), Period = decimal.Parse(currValue[2]) });
                    }
                }
                else
                {
                    limit.Period.Add(new IncomeGroupPeriod { DeductId = limit.Id, StartPeriod = 0, EndPeriod = 1, Period = decimal.Parse(currLimit[3]) });
                }
                g.Limitation.Add(limit);
            }
            IncomeGroupLimitService.InsertIncomeGroupLimit(g);
            return true;
        }

    }
}