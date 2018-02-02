using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.SMS.Service;
using ChinaPay.SMS.DataTransferObject;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.SmsModule
{
    public partial class SMSSendSet : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                var obj = SMSCompanySmsParamService.Query(AccountType.Payment,CurrentCompany.CompanyId);
                if (obj != null)
                {
                    //if ((obj.B3BReceiveSms & CompanyB3BReceiveSms.AccountBinding) == CompanyB3BReceiveSms.AccountBinding)
                    //{
                    //    chkBang.Checked = true;
                    //}
                    if ((obj.B3BReceiveSms & CompanyB3BReceiveSms.FlightChanges) == CompanyB3BReceiveSms.FlightChanges)
                    {
                        chkBina.Checked = true;
                    }
                    if ((obj.B3BReceiveSms & CompanyB3BReceiveSms.Ticket) == CompanyB3BReceiveSms.Ticket)
                    {
                        chkChupiao.Checked = true;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            CompanyB3BReceiveSms param = CompanyB3BReceiveSms.None;
            //if (chkBang.Checked)
            //{
            //    param |= CompanyB3BReceiveSms.AccountBinding;
            //}
            if (chkChupiao.Checked)
            {
                param |= CompanyB3BReceiveSms.Ticket;
            }
            if (chkBina.Checked)
            {
                param |= CompanyB3BReceiveSms.FlightChanges;
            }
            //var company = new ChinaPay.SMS.Service.Domain.CompanySmsParam(CurrentCompany.CompanyId);
            //company.B3BReceiveSms = param;
            //company.PoolpayReceiveSms = CompanyPoolpayReceiveSms.None;
            //company.AccountNo = CurrentUser.UserName;
            try
            {
                var acc = from item in AccountService.Query(CurrentCompany.CompanyId)
                          where item.Type == Common.Enums.AccountType.Payment
                          select new { No = item.No };
                SMSCompanySmsParamService.SetCompanyB3BReceiveSmsByCompanyId(acc.First().No, param);
                RegisterScript("alert('设置成功');window.location.href='/SmsModule/SMSSendRecord.aspx';", true);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex,"设置短信");
            }

        }
    }
}