using System;
using System.Linq;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service.Policy;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.SystemManagement.Domain;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.Policy.Domain;

namespace ChinaPay.B3B.TransactionWeb.PolicyHandlers
{
    /// <summary>
    /// 政策信息查看三种角色管理 政策
    /// </summary>
    public class RoleGeneralPolicy : BaseHandler
    {
        /// <summary>
        /// 发布基础政策
        /// </summary> 
        /// <param name="Normal"> </param>
        public bool RegisterBasicPolicy(NormalPolicyReleaseInfo Normal)
        {
            try
            {
                Normal.BasicInfo.Creator = this.CurrentUser.UserName;
                Normal.BasicInfo.Owner = this.CurrentUser.Owner;
                return PolicyManageService.ReleaseNormalPolicies(Normal, this.CurrentUser.UserName);
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("添加信息发生未知错误，请稍后再试");
            }
        }

        /// <summary>
        /// 发布特价政策
        /// </summary>
        /// <param name="Policy"></param>
        /// <param name="operatorAccount"></param>
        public bool RegisterSpecialOfferPolicy(BargainPolicyReleaseInfo Bargain)
        {
            try
            {
                Bargain.BasicInfo.Creator = this.CurrentUser.UserName;
                Bargain.BasicInfo.Owner = this.CurrentUser.Owner;
                return PolicyManageService.ReleaseBargainPolicies(Bargain, this.CurrentUser.UserName);
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("添加信息发生未知错误，请稍后再试");
            }
        }

        /// <summary>
        /// 发布特殊政策
        /// </summary>
        /// <param name="Policy"></param>
        /// <param name="operatorAccount"></param>
        public bool RegisterSpecialPolicy(SpecialPolicyReleaseInfo Special)
        {
            try
            {
                Special.BasicInfo.Owner = this.CurrentUser.Owner;
                return PolicyManageService.ReleaseSpecialPolicies(Special, this.CurrentUser.UserName);
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("添加信息发生未知错误，请稍后再试");
            }
        }

        ///// <summary>
        ///// 发布往返政策
        ///// </summary>
        ///// <param name="Policy"></param>
        ///// <param name="operatorAccount"></param>
        //public bool RegisterProductClassPolicy(RoundTripPolicyReleaseInfo Round)
        //{
        //    try
        //    {
        //        Round.BasicInfo.Owner = this.CurrentUser.Owner;
        //        return PolicyManageService.ReleaseRoundTripPolicies(Round, this.CurrentUser.UserName);
        //    }
        //    catch (System.Data.Common.DbException)
        //    {
        //        throw new Exception("添加信息发生未知错误，请稍后再试");
        //    }

        //}

        /// <summary>
        /// 发布团队政策
        /// </summary> 
        /// <param name="Normal"> </param>
        public bool RegisterTeamPolicy(TeamPolicyReleaseInfo team)
        {
            try
            {
                team.BasicInfo.Creator = this.CurrentUser.UserName;
                team.BasicInfo.Owner = this.CurrentUser.Owner;
                return PolicyManageService.ReleaseTeamPolicies(team, this.CurrentUser.UserName);
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("添加信息发生未知错误，请稍后再试");
            }
        }


        /// <summary>
        /// 发布缺口政策
        /// </summary> 
        /// <param name="Normal"> </param>
        public bool RegisterNotchPolicy(NotchPolicyReleaseInfo notch)
        {
            try
            {
                notch.Creator = this.CurrentUser.UserName;
                notch.Owner = this.CurrentUser.Owner;
                return PolicyManageService.ReleaseNotchPolicies(notch, this.CurrentUser.UserName);
            }
            catch (System.Data.Common.DbException ex)
            {
                Service.LogService.SaveExceptionLog(ex);
                throw new Exception("添加信息发生未知错误，请稍后再试");
            }
        }



        /// <summary>
        /// 获取指定公司的 Office 号
        /// </summary>
        /// <param name="id">Office 号所属的公司 Id</param>
        /// <returns>返回该公司所有的 Office 号</returns>
        public object GetOfficeNumbers()
        {
            return CompanyService.QueryOfficeNumbers(this.CurrentCompany.CompanyId);
        }
        /// <summary>
        /// 获取指定公司授权的 自定义编码
        /// </summary> 
        /// <returns>返回该公司所有的 Office 号</returns>
        public object GetCustomeCode()
        {
            return CompanyService.GetCustomNumberByEmployee(CurrentUser.Id);
        }
        /// <summary>
        /// 得到公司下的始发地（根据公司编号查询）
        /// </summary>
        /// <returns></returns>
        public object QueryAirportsDepartureByCompany()
        {
            SetPolicy set = PolicySetService.QuerySetPolicy(this.CurrentCompany.CompanyId);
            if (set == null)
            {
                throw new Exception("还没设置出港城市，请先设置出港城市！");
            }
            var list = set.Departure;

            return from item in ChinaPay.B3B.Service.FoundationService.Airports
                   where list.Contains(item.Code.Value)
                   select new
                   {
                       Code = item.Code,
                       Name = item.Code + "-" + item.ShortName
                   };
        }
        /// <summary>
        /// 得到所有的到达地
        /// </summary>
        /// <returns></returns>
        public object QueryAirportsArrivalList()
        {
            return from item in ChinaPay.B3B.Service.FoundationService.Airports 
                   select new
                   {
                       Code = item.Code,
                       Name = item.Code + "-" + item.ShortName
                   };
        }
        /// <summary>
        /// 得到所有的航空公司
        /// </summary>
        /// <returns></returns>
        public object QueryAirlines()
        {
            SettingPolicy setting = CompanyService.GetPolicySetting(this.CurrentCompany.CompanyId);
            if (setting.Airlines == "")
            {
                throw new Exception("还没有设置航空公司，请先设置航空公司！");
            }
            string[] str_ids = setting.Airlines.Split('/');
            return from item in ChinaPay.B3B.Service.FoundationService.Airlines
                   where item.Valid && str_ids.Contains(item.Code.Value)
                   select new
                   {
                       Code = item.Code,
                       Name = item.Code + "-" + item.ShortName
                   };
        }
        /// <summary>
        /// 得到舱位
        /// </summary>
        /// <param name="AirLineCode">航空公司代码</param>
        /// <param name="StartTime">去程日期开始时间</param>
        /// <param name="EndTime">去程日期结束时间</param> 
        /// <returns>舱位</returns>
        public object QueryBunks(string AirLineCode, string StartTime, string EndTime)
        {
            return ChinaPay.B3B.Service.FoundationService.Bunks.Where(
                item => item.AirlineCode.Value == AirLineCode
                && (StartTime.Trim() == "" || item.FlightBeginDate >= DateTime.Parse(StartTime))
                && (EndTime.Trim() == "" || item.FlightEndDate <= DateTime.Parse(EndTime)));
        }

        /// <summary>
        /// 查询推退改签规定
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object QueryRegulations(SystemDictionaryType type)
        {
            return SystemDictionaryService.Query(type);
        }

        /// <summary>
        /// 查询公司是否可以发布vip的返点
        /// </summary>
        /// <returns></returns>
        public bool IsPublishVip()
        {
            return CompanyService.GetCompanyParameter(this.CurrentCompany.CompanyId).CanReleaseVip;
        }
        /// <summary>
        /// 查询公司是否可以平台审核特殊政策
        /// </summary>
        /// <returns></returns>
        public bool IsAutoPlatformAudit()
        {
            return CompanyService.GetCompanyParameter(this.CurrentCompany.CompanyId).AutoPlatformAudit;
        }
        /// <summary>
        /// 判断当前公司类型
        /// </summary>
        /// <returns></returns>
        public CompanyType IsPublishFree()
        {
            return CurrentCompany.CompanyType;
        }

    }
}