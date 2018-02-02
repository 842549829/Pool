using System;
using System.Linq;
using System.Net;
using System.Web;
using ChinaPay.AddressLocator;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.SMS.Service;

namespace ChinaPay.B3B.TransactionWeb.OrganizationHandlers
{
    /// <summary>
    /// 所在地
    /// </summary>
    public class Address : ChinaPay.Infrastructure.WebEx.AjaxHandler.WebAjaxHandler
    {
        /// <summary>
        /// 查询所在地
        /// </summary>
        /// <returns></returns>
        public object Addresses()
        {
            return from area in FoundationService.Areas
                   select new
                   {
                       Id = area.Code,
                       name = area.Name,
                       province = from province in area.Provinces
                                  select new
                                  {
                                      Id = province.Code,
                                      name = province.Name,
                                      city = from city in province.Cities
                                             select new
                                             {
                                                 Id = city.Code,
                                                 name = city.Name,
                                                 area = from county in city.Counties
                                                        select new
                                                        {
                                                            Id = county.Code,
                                                            name = county.Name
                                                        }
                                             }
                                  }
                   };
        }

        public object GteProvince(string code)
        {
            return FoundationService.Provinces.Select(item => new { item.Code, item.Name });
        }
       /// <summary>
        /// 获取指定省份的城市
        /// </summary>
        /// <param name="provinceName">省名称</param>
        /// <returns></returns>
        public object GetCities(string provinceCode)
        {
            var province = FoundationService.Provinces.FirstOrDefault(p => p.Code == provinceCode);
            if(province != null) return province.Cities.Select(p => new { p.Code, p.Name });
            return null;
        }
        public object GetCounty(string cityCode) {
            var county = FoundationService.Cities.FirstOrDefault(item => item.Code == cityCode);
            if(county != null)
            return county.Counties.Select(item => new { item.Code,item.Name });
            return null;
        }
        /// <summary>
        /// 发送短信
        /// </summary>
        public string SendSMS(string phone,string account) 
        {
            
            try
            {
                if (!SendSMSTime()) {
                    return "请120秒后再获取验证码"; 
                }
                IPAddress ip = IPAddressLocator.GetRequestIP(HttpContext.Current.Request);
                if (!AccountCombineService.ValidateIP(ip.ToString())) { 
                    return "同一个IP一天只有100次获取验证码的机会";
                }
                var verfiCode = new VerfiCode()
                {
                    CellPhone = phone,
                    Code = ChinaPay.Utility.VerifyCodeUtility.CreateVerifyCode(6,"1,2,3,4,5,6,7,8,9,0"),
                    IP = ip.ToString(),
                    Type= Common.Enums.VerfiCodeType.Register,
                    AccountNo = account
                };
                Session["phoneValidateCode"] = verfiCode.Code;
                Session["phoneTime"] = DateTime.Now;
                SMSSendService.SendB3bRegisterValidateCode(verfiCode.CellPhone, verfiCode.Code, 20,BasePage.CurrenContract.ServicePhone);
                AccountCombineService.SaveVerfiCode(verfiCode);
                return string.Empty;
            }
            catch (Exception)
            {
                return "发送验证码异常";
            }
        }
        private bool SendSMSTime() 
        {
            if (Session["phoneTime"] != null)
            {
                var phoneTime = (DateTime)Session["phoneTime"];
                int timeSeconds = (int)(DateTime.Now - phoneTime).TotalSeconds;
                if (timeSeconds <= 60) return false;
            }
            return true;
        }
    }
}