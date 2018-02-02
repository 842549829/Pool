using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role
{
    public partial class ShouyishezhiGlobal : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                string str = ""; int index = 0;
                var list = ChinaPay.B3B.Service.FoundationService.Airlines.OrderBy(item => item.ShortName.Length);
                foreach (var item in list)
                {
                    index++;
                    str += " <input type='checkbox' value='" + item.Code.Value + "' id='" + item.Code.Value + "' /> <label for='" + item.Code.Value + "'>" + item.ShortName + "</label>";
                    if (index % 6 == 0)
                    {
                        str += "<br />";
                    }
                }
                hangkonggongsi.InnerHtml = str;
                InitD();
            }
        }
        private void InitD()
        {
            var limit = IncomeGroupLimitService.QueryIncomeGroupLimitGroupByCompanyId(CurrentCompany.CompanyId);
            var type = CompanyService.QueryGlobalPurchaseIncome(CurrentCompany.CompanyId);
            radFenzu.Checked = type == Common.Enums.IncomeGroupLimitType.Each;
            radQuanju.Checked = type == Common.Enums.IncomeGroupLimitType.Global;
            radBuxianzi.Checked = type == Common.Enums.IncomeGroupLimitType.None;
            string str = "", strPeriod = "", strG = "", strT = "";
            remark.Value = limit.Remark;
            foreach (var item in limit.Limitation)
            {
                var currStr = (item.IsOwnerPolicy ? "1" : "0") + "," + item.Airlines + "," + (byte)item.Type;
                var str1 = "";
                foreach (var period in item.Period)
                {
                    if (strPeriod != "")
                    {
                        strPeriod += ";";
                    }
                    if (item.Type == Common.Enums.PeriodType.Interval)
                    {
                        str1 += "[" + item.Airlines + "]" + " 区域：" + "[" + (period.StartPeriod * 100).TrimInvaidZero() + "," + (period.EndPeriod * 100).TrimInvaidZero() + "];扣点:" + (period.Period * 100).TrimInvaidZero() + "% <br />";
                        strPeriod += (period.StartPeriod * 100).TrimInvaidZero() + "|" + (period.EndPeriod * 100).TrimInvaidZero() + "|" + (period.Period * 100).TrimInvaidZero();
                    }
                    else
                    {
                        strPeriod += (period.Period * 100).TrimInvaidZero();
                        str1 += "[" + item.Airlines + "]" + " 统一返点：" + (period.Period * 100).TrimInvaidZero()+"%";
                    }
                }
                currStr += "," + strPeriod + "," + item.Price;
                if (str != "")
                {
                    str += "^";
                }
                str += currStr;
                strPeriod = "";
                if (item.IsOwnerPolicy)
                {
                    strG += " <p curr=" + currStr + ">" + str1 + " 每张票加价：" + item.Price + " 元 <a href='#' class='remove' curr=" + currStr + ">删除</a> </p>";
                }
                else
                {
                    strT += " <p curr=" + currStr + ">" + str1 + " 每张票加价：" + item.Price + " 元 <a href='#' class='remove' curr=" + currStr + ">删除</a> </p>";
                }
            }
            hidShouyishezhi.Value = str;
            bengongsi.InnerHtml = strG;
            tonghang.InnerHtml = strT;
        }
    }
}