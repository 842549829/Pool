using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Service;
using System.Data;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Service.Foundation.Domain;
using System.Web.UI.WebControls;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy
{
    public static class BasePolicy
    {
        public static NormalPolicy saveInfo(DataRow item, List<OfficeNumber> officeNumbers, WorkingSetting workingSetting, IEnumerable<CustomNumber> customerNumbers, IEnumerable<string> airlines, CompanyParameter companyParameter, IEnumerable<string> airports)
        {
            NormalPolicy normalPolicy = new NormalPolicy();
            normalPolicy.VoyageType = (Common.Enums.VoyageType)getVoyageType(item[0].ToString());
            normalPolicy.Airline = item[1].ToString().ToUpper();
            normalPolicy.OfficeCode = item[2].ToString().ToUpper();
            normalPolicy.ImpowerOffice = officeNumbers.FirstOrDefault(o => o.Number == item[2].ToString().ToUpper()).Impower;
            if (workingSetting.IsImpower)
            {
                normalPolicy.CustomCode = item[3].ToString();
            }
            else
            {
                normalPolicy.CustomCode = "";
            }
            normalPolicy.Departure = item[4].ToString().ToUpper();
            normalPolicy.Arrival = item[5].ToString().ToUpper();
            normalPolicy.ExceptAirways = item[6].ToString().ToUpper();
            if (!string.IsNullOrWhiteSpace(item[7].ToString()))
            {
                normalPolicy.DepartureFlightsFilterType = Common.Enums.LimitType.Exclude;
                normalPolicy.DepartureFlightsFilter = item[7].ToString();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(item[8].ToString()))
                {
                    normalPolicy.DepartureFlightsFilterType = Common.Enums.LimitType.Include;
                    if (item[8].ToString() == "*")
                    {
                        normalPolicy.DepartureFlightsFilterType = Common.Enums.LimitType.None;
                        normalPolicy.DepartureFlightsFilter = "";
                    }
                    else
                    {
                        normalPolicy.DepartureFlightsFilter = item[8].ToString();
                    }
                }
                else
                {
                    normalPolicy.DepartureFlightsFilterType = Common.Enums.LimitType.None;
                    normalPolicy.DepartureFlightsFilter = "";
                }
            }
            //normalPolicy.DepartureDatesFilterType = Common.Enums.DateMode.Date;
            //normalPolicy.DepartureDatesFilter = "";
            if (normalPolicy.VoyageType != Common.Enums.VoyageType.OneWay)
            {
                if (!string.IsNullOrWhiteSpace(item[9].ToString()))
                {
                    normalPolicy.ReturnFlightsFilterType = Common.Enums.LimitType.Exclude;
                    normalPolicy.ReturnFlightsFilter = item[9].ToString();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(item[10].ToString()))
                    {
                        normalPolicy.ReturnFlightsFilterType = Common.Enums.LimitType.Include;
                        if (item[10].ToString() == "*")
                        {
                            normalPolicy.ReturnFlightsFilterType = Common.Enums.LimitType.None;
                            normalPolicy.ReturnFlightsFilter = "";
                        }
                        else
                        {
                            normalPolicy.ReturnFlightsFilter = item[10].ToString();
                        }
                    }
                    else
                    {
                        normalPolicy.ReturnFlightsFilterType = Common.Enums.LimitType.None;
                        normalPolicy.DepartureFlightsFilter = "";
                    }
                }
                //normalPolicy.ReturnDatesFilterType = Common.Enums.DateMode.Date;
                //normalPolicy.ReturnDatesFilter = "";
            }
            normalPolicy.DrawerCondition = item[11].ToString();
            normalPolicy.Remark = item[12].ToString();
            normalPolicy.DepartureDateStart = DateTime.Parse(item[13].ToString());
            normalPolicy.DepartureDateEnd = DateTime.Parse(item[14].ToString());
            normalPolicy.StartPrintDate = DateTime.Parse(item[15].ToString());
            normalPolicy.DepartureDateFilter = item[16].ToString();
            normalPolicy.DepartureWeekFilter = item[17].ToString();
            normalPolicy.Berths = (item[18].ToString()).ToUpper();
            normalPolicy.TicketType = item[19].ToString() == "B2B" ? Common.Enums.TicketType.B2B : Common.Enums.TicketType.BSP;
            //normalPolicy.TravelDays = 0;
            if (companyParameter.CanHaveSubordinate)
            {
                normalPolicy.IsInternal = true;
                normalPolicy.InternalCommission = decimal.Parse(item[20].ToString()) / 100;
            }
            else
            {
                normalPolicy.IsInternal = false;
                normalPolicy.InternalCommission = 0;
            }
            normalPolicy.SubordinateCommission = decimal.Parse(item[21].ToString()) / 100;
            if (companyParameter.AllowBrotherPurchase)
            {
                normalPolicy.IsPeer = true;
                normalPolicy.ProfessionCommission = decimal.Parse(item[22].ToString()) / 100;
            }
            else
            {
                normalPolicy.IsPeer = false;
                normalPolicy.ProfessionCommission = 0;
            }
            if (normalPolicy.VoyageType == Common.Enums.VoyageType.OneWay || item[23].ToString() == "否")
            {
                normalPolicy.SuitReduce = false;
            }
            else
            {
                normalPolicy.SuitReduce = true;
            }
            normalPolicy.Audited = item[24].ToString() == "已审" ? true : false;
            normalPolicy.ChangePNR = item[25].ToString() == "是" ? true : false;
            normalPolicy.Transit = "";
            normalPolicy.PrintBeforeTwoHours = item[19].ToString().ToUpper() == "B2B" && item[26].ToString() == "是" ? true : false;
            return normalPolicy;
        }

        public static string valiate(DataRow row, List<OfficeNumber> officeNumbers, WorkingSetting workingSetting, IEnumerable<CustomNumber> customerNumbers, IEnumerable<string> airlines, CompanyParameter companyParameter, IEnumerable<string> airports)
        {
            string errorInfo = "";
            if (string.IsNullOrWhiteSpace(row[0].ToString()) || getVoyageType(row[0].ToString()) == -1)
                return errorInfo = "行程类型格式错误";
            var airline = row[1].ToString();
            if (string.IsNullOrWhiteSpace(airline) || !airlineIsLegal(airline.ToUpper(), airlines))
                return errorInfo = "航空公司格式错误或无法发布该航空公司下的政策";
            if (string.IsNullOrWhiteSpace(row[2].ToString()) || !officeNoIsLegal(row[2].ToString().ToUpper(), officeNumbers))
                return errorInfo = "OFFICE号格式错误或此公司不能用此OfficeNo号发布政策";
            if (workingSetting.IsImpower)
            {
                if (string.IsNullOrWhiteSpace(row[3].ToString()) || !CustomerCodeIsLegal(row[3].ToString(), customerNumbers))
                {
                    return errorInfo = "自定义编码格式错误";
                }
            }
            if (string.IsNullOrWhiteSpace(row[4].ToString()) || !cityIsLegal(row[4].ToString().ToUpper(), airports))
                return errorInfo = "出发城市格式错误";
            if (string.IsNullOrWhiteSpace(row[5].ToString()) || !arrivalCityIsLegal(row[5].ToString().ToUpper()))
                return errorInfo = "到达城市格式错误";
            if (!string.IsNullOrWhiteSpace(row[6].ToString()))
            {
                if (row[6].ToString().Length > 200 || !Regex.IsMatch(row[6].ToString(), @"^([a-zA-Z*]{6}[/]?)+$"))
                {
                    return errorInfo = "排除航段格式错误";
                }
            }
            if (!string.IsNullOrWhiteSpace(row[7].ToString()) && string.IsNullOrWhiteSpace(row[8].ToString()))
            {
                if (row[7].ToString().Length > 100 || !Regex.IsMatch(row[7].ToString(), @"^[a-z\dA-Z/*]+$"))
                {
                    return errorInfo = "去程不适用航班号格式错误";
                }
            }
            if (!string.IsNullOrWhiteSpace(row[8].ToString()) && string.IsNullOrWhiteSpace(row[7].ToString()))
            {
                if (row[8].ToString().Length > 100 || !Regex.IsMatch(row[8].ToString(), @"^[a-z\dA-Z/*]+$"))
                {
                    return errorInfo = "去程适用航班号格式错误";
                }
            }
            if (row[0].ToString().Trim() != "单程")
            {
                if (!string.IsNullOrWhiteSpace(row[9].ToString()) && string.IsNullOrWhiteSpace(row[10].ToString()))
                {
                    if (row[9].ToString().Length > 100 || !Regex.IsMatch(row[9].ToString(), @"^[a-z\dA-Z/*]+$"))
                    {
                        return errorInfo = "回程不适用航班号格式错误";
                    }
                }
                if (!string.IsNullOrWhiteSpace(row[10].ToString()) && string.IsNullOrWhiteSpace(row[9].ToString()))
                {
                    if (row[10].ToString().Length > 100 || !Regex.IsMatch(row[10].ToString(), @"^[a-z\dA-Z/*]+$"))
                    {
                        return errorInfo = "回程适用航班号格式错误";
                    }
                }
            }
            if (!string.IsNullOrWhiteSpace(row[11].ToString()))
            {
                if (row[11].ToString().Length > 100)
                {
                    return errorInfo = "出票条件格式错误";
                }
            }
            if (!string.IsNullOrWhiteSpace(row[12].ToString()))
            {
                if (row[12].ToString().Length > 100)
                {
                    return errorInfo = "政策备注格式错误";
                }
            }
            var datePattern = @"^[\d]{4}-[\d]{2}-[\d]{2}$";
            var exceptDatePattern = @"^[\d,-]+$";
            if (string.IsNullOrWhiteSpace(row[13].ToString()) || !Regex.IsMatch(row[13].ToString(), datePattern))
                return errorInfo = "去程航班有效日期格式错误，格式必须是2012-01-01";
            if (string.IsNullOrWhiteSpace(row[14].ToString()) || !Regex.IsMatch(row[14].ToString(), datePattern))
                return errorInfo = "去程航班失效日期格式错误，格式必须是2012-01-01";
            if (string.IsNullOrWhiteSpace(row[15].ToString()) || !Regex.IsMatch(row[15].ToString(), datePattern))
                return errorInfo = "出票日期格式错误";
            if (!string.IsNullOrWhiteSpace(row[16].ToString()) && !Regex.IsMatch(row[16].ToString(), exceptDatePattern))
                return errorInfo = "排除日期格式错误";

            if (string.IsNullOrWhiteSpace(row[17].ToString()))
                return errorInfo = "适用班期不能为空";
            if (row[17].ToString().Length > 100 || !Regex.IsMatch(row[17].ToString(), @"^[0-9,]+$"))
            {
                return errorInfo = "适用班期格式错误";
            }
            if (string.IsNullOrWhiteSpace(row[18].ToString()) || row[18].ToString().Length > 100 || !Regex.IsMatch(row[18].ToString(), @"^[a-zA-Z,]+$"))
                return errorInfo = "舱位格式错误";
            if (string.IsNullOrWhiteSpace(row[19].ToString()) || (policyType(row[19].ToString()) < 0))
                return errorInfo = "政策类型格式错误";
            var pattern = @"^[0-9]{1,2}(.[1-9]{1})?$";

            if (companyParameter.CanHaveSubordinate)
            {
                if (string.IsNullOrWhiteSpace(row[20].ToString()) || !Regex.IsMatch(row[20].ToString(), pattern) || decimal.Parse(row[20].ToString()) < 2.5M)
                {
                    return errorInfo = "内部返点格式错误";
                }
            }
            if (string.IsNullOrWhiteSpace(row[21].ToString()) || !Regex.IsMatch(row[21].ToString(), pattern) || decimal.Parse(row[21].ToString()) < 2.5M)
                return errorInfo = "下级返点格式错误";
            if (companyParameter.AllowBrotherPurchase)
            {
                if (string.IsNullOrWhiteSpace(row[22].ToString()) || !Regex.IsMatch(row[22].ToString(), pattern) || decimal.Parse(row[22].ToString()) < 2.5M)
                {
                    return errorInfo = "同行返点格式错误";
                }
            }
            if (row[0].ToString().Trim() != "单程")
            {
                if (!string.IsNullOrWhiteSpace(row[23].ToString()) && isYesOrNo(row[23].ToString()) < 0)
                    return errorInfo = "是否适用往返降舱格式错误";
            }
            if (string.IsNullOrWhiteSpace(row[24].ToString()) || !auditStatusIsLegal(row[24].ToString()))
                return errorInfo = "审核状态格式错误";
            if (string.IsNullOrWhiteSpace(row[25].ToString()) || isYesOrNo(row[25].ToString()) < 0)
                return errorInfo = "是否换编码出票格式错误";
            if (row[19].ToString().ToUpper() == "B2B" && (string.IsNullOrWhiteSpace(row[26].ToString()) || isYesOrNo(row[26].ToString()) < 0))
                return errorInfo = "起飞前2小时内可用B2B出票格式错误";
            return errorInfo;
        }

        public static bool airlineIsLegal(string airline, IEnumerable<string> airlines)
        {
            bool isLegal = false;
            if (airlines.Contains(airline))
                isLegal = true;
            return isLegal;
        }

        public static int getVoyageType(string voyageType)
        {
            int voyage = -1;
            switch (voyageType)
            {
                case "单程":
                    voyage = (int)Common.Enums.VoyageType.OneWay;
                    break;
                case "往返":
                    voyage = (int)Common.Enums.VoyageType.RoundTrip;
                    break;
                case "单程/往返":
                    voyage = (int)Common.Enums.VoyageType.OneWayOrRound;
                    break;
            }
            return voyage;
        }

        public static bool cityIsLegal(string city, IEnumerable<string> airports)
        {
            bool isLegal = true;
            Array.ForEach(city.Split('/'), s =>
            {
                if (!airports.Contains(s))
                    isLegal = false;
            });
            return isLegal;
        }

        public static bool arrivalCityIsLegal(string city)
        {
            bool isLegal = true;
            var airports = FoundationService.Airports;
            Array.ForEach(city.Split('/'), s =>
            {
                if (!airports.Select(item => item.Code.Value).Contains(s))
                {
                    isLegal = false;
                }
            });
            return isLegal;
        }

        public static int policyType(string policyType)
        {
            int policy = -1;
            if (policyType.Trim() == "B2B" || policyType.Trim() == "BSP")
                policy = 1;
            return policy;
        }

        public static int isYesOrNo(string content)
        {
            int isYesOrNo = -1;
            if (content.Trim() == "是" || content.Trim() == "否")
                isYesOrNo = 1;
            return isYesOrNo;
        }

        public static bool officeNoIsLegal(string officeNo, List<OfficeNumber> officeNumbers)
        {
            bool isLegal = true;
            var pattern = @"^[a-zA-Z]{3}[0-9]{3}$";
            if (!Regex.IsMatch(officeNo, pattern))
                isLegal = false;
            if (!officeNumbers.Select(o => o.Number).Contains(officeNo))
                isLegal = false;
            return isLegal;
        }

        public static bool auditStatusIsLegal(string auditStauts)
        {
            bool isLegal = auditStauts.Trim() == "已审" || auditStauts.Trim() == "未审";
            return isLegal;
        }

        /// <summary>
        /// 得到政策是公司自己挂起还是平台挂起的
        /// </summary>
        /// <param name="airline"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetHungInfo(string airline, Guid id)
        {
            return Service.Policy.PolicyManageService.GetSuspendInfo(id).SuspendByCompany.Contains(airline) ? "公司挂起" : "平台挂起";
        }
        /// <summary>
        /// 是否可以发内部返佣
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CanHaveSubordinate(Guid id)
        {
            var company = CompanyService.GetCompanySettingsInfo(id);
            return company != null && company.Parameter.CanHaveSubordinate;
        }

        /// <summary>
        /// 自定义编码
        /// </summary>
        /// <param name="customerCode"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        private static bool CustomerCodeIsLegal(string customerCode, IEnumerable<CustomNumber> customerNumber)
        {
            bool isValid = false;
            if (customerNumber.Select(item => item.Number).Contains(customerCode))
                isValid = true;
            return isValid;
            //return customerNumber.Any(item => item.Number == customerCode);
        }

    }
}