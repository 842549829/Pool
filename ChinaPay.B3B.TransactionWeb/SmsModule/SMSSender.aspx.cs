using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.Core;
using ChinaPay.SMS.Service;
using System.Text.RegularExpressions;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.Service.SystemManagement.Domain;

namespace ChinaPay.B3B.TransactionWeb.SmsModule
{
    public partial class SMSSender : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                InitDataGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (valiate())
            {
                string strMsg = SMSSendService.CheckSensitiveWords(txtContext.Text);
                if (!string.IsNullOrEmpty(strMsg))
                {
                    ShowMessage("对不起，您所发送的短信中包含敏感词：\"" + strMsg + "\" 请修改后重试或联系平台！");
                    return;
                }
                try
                {
                    var acc = from item in AccountService.Query(CurrentCompany.CompanyId)
                              where item.Type == Common.Enums.AccountType.Payment
                              select new { No = item.No };
                    SMSSendService.SendCustomMessage(new ChinaPay.SMS.Service.Domain.Account(CurrentCompany.CompanyId, acc.First().No), this.txtPhone.Text.Trim().Split(new char[] { ',', '，' }), this.txtContext.Text);
                    RegisterScript("alert('提交成功');window.location.href='/SmsModule/SMSSendRecord.aspx';", true);
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "发送");
                }
            }
        }
        void InitDataGrid()
        {
            var query = SMSAccountService.QueryAccount(CurrentCompany.CompanyId);
            if (query != null)
            {
                if (query.Balance <= 50)
                {
                    LastDiv.Visible = true;
                    MoreDiv.Visible = false;
                    this.lblLastNum.Text = query.Balance.ToString();
                }
                else
                {
                    MoreDiv.Visible = true;
                    LastDiv.Visible = false;
                    this.lblMoreNum.Text = query.Balance.ToString();
                }
                this.lblLastNum.Text = query.Balance.ToString();
            }
            else
            {
                this.lblLastNum.Text = "0";
                LastDiv.Visible = true;
                MoreDiv.Visible = false;
            }
        }
        private bool valiate()
        {
            if (this.txtPhone.Text.Trim().Length == 0)
            {
                ShowMessage("请输入手机号码");
                return false;
            }
            if (this.txtPhone.Text.Trim().Length > 2000)
            {
                ShowMessage("手机号码长度不能超过2000位");
                return false;
            }
            //if(!Regex.IsMatch(this.txtPhone.Text.Trim(),@"^\d[,]{1,2000}$"))
            //{
            //    ShowMessage("手机号码格式错误");
            //    return false;
            //}
            if (this.txtContext.Text.Trim().Length > 300)
            {
                ShowMessage("短信内容不能超过300位");
                return false;
            }
            var phoneContent = this.txtPhone.Text.Trim().Split(new char[] { ',', '，' });
            if (int.Parse(lblLastNum.Text) < phoneContent.Count())
            {
                ShowMessage("短信可用条数不够，请先购买");
                return false;
            }
            var count = phoneContent.Distinct().Count();
            var orgCount = phoneContent.Count();
            if (count != orgCount)
            {
                ShowMessage("当前存在" + (orgCount - count + 1) + "个手机号码重复,请确认");
                return false;
            }
            for (var i = 0; i < phoneContent.Length; i++)
            {
                if (!Regex.IsMatch(phoneContent[i], "^1[3458]\\d{9}$"))
                {
                    ShowMessage("第 [" + (i + 1) + "] 个手机号码格式错误");
                    return false;
                }
            }
            return true;
        }
    }
}