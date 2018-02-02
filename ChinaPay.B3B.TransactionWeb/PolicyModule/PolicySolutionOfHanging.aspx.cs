using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Policy;
using System.Collections.Generic;
using System.Linq;
using ChinaPay.Core;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.PolicyModule
{
    public partial class PolicySolutionOfHanging : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                InitValue();
            }
        }
        private void ResponseRedirect()
        {
            if (this.CurrentCompany.CompanyType == CompanyType.Platform)
            {
                Response.Redirect("./PolicySolutionOfHangingManage.aspx");
            }
        }

        private void InitValue()
        {
            CompanyType type = CurrentCompany.CompanyType;
            string flag = Request.QueryString["flag"];

            this.SolutionReason.Text = string.Empty;
            this.txtRemark.Text = string.Empty;
            //挂起
            if (flag == "hang" && flag != null)
            {
                this.btnHang.Visible = true;
                this.btnSolution.Visible = false;
                tdCause.InnerText = "挂起原因";
                lblCause.InnerText = "请输入挂起原因";
                QueryAirLines(type, flag);
            }
            else
            {
                this.btnSolution.Visible = true;
                this.btnHang.Visible = false;
                QueryAirLines(type, flag);
            }
        }

        private void QueryAirLines(CompanyType type, string flag)
        {
            //运营方政策管理挂起解挂
            if (type == CompanyType.Platform)
            {
                var list = PolicySetService.QueryAirlines(Guid.Parse(Request.QueryString["id"]));
                //得到公司下所有的航空公司
                chklist.DataSource = list;
                chklist.DataBind();
                hung.Visible = false;
                msgTip.InnerHtml = "●灰色部份由出票方/产品方挂起";
                if (flag == "solution" && flag != null)
                    title.InnerHtml = "平台解挂政策：";
                if (flag == "hang" && flag != null)
                {
                    tip.Visible = false;
                    msg_box.Visible = false;
                    title.InnerHtml = "平台挂起政策：";
                }
            }
            //用户政策管理挂起解挂
            else
            {
                var list = PolicySetService.QueryAirlines(this.CurrentCompany.CompanyId);
                //得到公司下所有的航空公司
                chklist.DataSource = list;
                chklist.DataBind();
                hung.Visible = true;
                chkHung.DataSource = list;
                chkHung.DataBind();
                msgTip.InnerHtml = "●灰色部份由平台挂起";
            }
        }

        //解挂
        protected void btnSolution_Click(object sender, EventArgs e)
        {
            var airlines = (from ListItem item in chklist.Items where item.Selected && item.Enabled select item.Value).ToList();
            if (airlines.Count == 0)
            {
                ShowMessage("解挂航空公司必须选择一个！本次操作被取消！");
                return;
            }
            if (SolutionReason.Text.Trim() == "")
            {
                ShowMessage("解挂原因不能为空！请输入！");
                return;
            }
            if (SolutionReason.Text.Trim().Length >= 100)
            {
                SolutionReason.Text = SolutionReason.Text.Trim().Substring(0, 100);
                ShowMessage("输入解挂原因过长，只能在100字以内！");
                return;
            }
            string flag = Request.QueryString["flag"];
            CompanyType type = CurrentCompany.CompanyType;
            var ipAddress = ChinaPay.AddressLocator.IPAddressLocator.GetRequestIP(HttpContext.Current.Request);
            //var ipLocation = ChinaPay.AddressLocator.CityLocator.GetIPLocation(ipAddress);
            if (type == CompanyType.Platform && flag == "solution" && flag != null)
            {
                string Reason = SolutionReason.Text;
                bool falg = PolicyManageService.UnSuspendPolicies(Guid.Parse(Request.QueryString["id"]), this.CurrentUser.UserName, Reason, ipAddress.ToString(), CurrentCompany.CompanyType == CompanyType.Platform ? PublishRole.平台 : PublishRole.用户, airlines.ToArray());
                if (falg)
                {
                    ResponseRedirect();
                    InitValue();
                }
            }
            else
            {
                string Reason = SolutionReason.Text;
                bool falg = PolicyManageService.UnSuspendPolicies(this.CurrentCompany.CompanyId, this.CurrentUser.UserName, Reason, ipAddress.ToString(), CurrentCompany.CompanyType == CompanyType.Platform ? PublishRole.平台 : PublishRole.用户, airlines.ToArray());
                if (falg)
                {
                    InitValue();
                }
            }
        }
        //挂起
        protected void btnHang_Click(object sender, EventArgs e)
        {
            try
            {
                string Reason = txtRemark.Text;
                var airlines = (from ListItem item in chklist.Items
                                where item.Selected && item.Enabled
                                select item.Value).ToList();
                var companyId = CurrentCompany.CompanyType == CompanyType.Platform ? Guid.Parse(Request.QueryString["id"]) : CurrentCompany.CompanyId;
                var existsPolicyType = PolicyManageService.CheckIfHasDefaultPolicy(companyId, airlines);
                if (existsPolicyType != PolicyType.Unknown)
                {
                    throw new CustomException(CurrentCompany.CompanyType == CompanyType.Platform ? "该供应商在平台存在"+existsPolicyType.GetDescription()+"指向，请调整后再行操作" :
                        "该航线在平台存在"+existsPolicyType.GetDescription()+"指向，请联系平台调整后再行操作");
                }
                if (airlines.Count == 0)
                {
                    ShowMessage("挂起航空公司必须选择一个！本次操作被取消！");
                    return;
                }
                if (SolutionReason.Text.Trim() == "")
                {
                    ShowMessage("挂起原因不能为空！请输入！");
                    return;
                }
                if (SolutionReason.Text.Trim().Length >= 100)
                {
                    SolutionReason.Text = SolutionReason.Text.Trim().Substring(0, 100);
                    ShowMessage("输入挂起原因过长，只能在100字以内！");
                    return;
                }
                var flag = Request.QueryString["flag"].Trim();
                var type = CurrentCompany.CompanyType;

                var ipAddress = ChinaPay.AddressLocator.IPAddressLocator.GetRequestIP(HttpContext.Current.Request);
                //var ipLocation = ChinaPay.AddressLocator.CityLocator.GetIPLocation(ipAddress);
                if (type == CompanyType.Platform && flag == "hang")
                {
                    var falg = PolicyManageService.SuspendPolicies(Guid.Parse(Request.QueryString["id"]), this.CurrentUser.UserName, SolutionReason.Text.Trim(), ipAddress.ToString(), PublishRole.平台, airlines.ToArray());
                    if (falg)
                    {
                        ResponseRedirect();
                        InitValue();
                    }
                }
                else
                {
                    bool falg = PolicyManageService.SuspendPolicies(this.CurrentCompany.CompanyId, this.CurrentUser.UserName, Reason, ipAddress.ToString(), CurrentCompany.CompanyType == CompanyType.Platform ? PublishRole.平台 : PublishRole.用户, airlines.ToArray());
                    if (falg)
                    {
                        InitValue();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "挂起政策");
            }
        }

        protected void chklist_DataBound(object sender, EventArgs e)
        {
            string flag = Request.QueryString["flag"];
            CompanyType type = CurrentCompany.CompanyType;
            //string website = Request.QueryString["website"].ToString().Trim();
            if (type == CompanyType.Platform)
            {
                var suspendPolicies = Service.Policy.PolicyManageService.GetSuspendInfo(Guid.Parse(Request.QueryString["id"]));

                if (flag == "solution")
                {
                    chklist.Items.Clear();
                    foreach (string suspenditem in suspendPolicies.SuspendByCompany)
                    {
                        chklist.Items.Add(new ListItem { Text = suspenditem, Enabled = false, Selected = true });
                    }
                    foreach (string suspenditem in suspendPolicies.SuspendByPlatform)
                    {
                        chklist.Items.Add(new ListItem { Text = suspenditem });
                    }
                }
                if (flag == "hang")
                {
                    foreach (string suspenditem in suspendPolicies.SuspendByPlatform)
                    {
                        for (int i = 0; i < chklist.Items.Count; i++)
                        {
                            if (chklist.Items[i].Text.Equals(suspenditem))
                            {
                                //挂起的政策移除集合中
                                chklist.Items.Remove(chklist.Items[i]);
                            }
                        }
                    }

                    foreach (string suspenditem in suspendPolicies.SuspendByCompany)
                    {
                        for (int i = 0; i < chklist.Items.Count; i++)
                        {
                            if (chklist.Items[i].Text.Equals(suspenditem))
                            {
                                //挂起的政策移除集合中
                                chklist.Items.Remove(chklist.Items[i]);
                            }
                        }
                    }
                }
            }
            else
            {
                var suspendPolicies = Service.Policy.PolicyManageService.GetSuspendInfo(CurrentCompany.CompanyId);

                chklist.Items.Clear();
                foreach (string suspenditem in suspendPolicies.SuspendByCompany)
                {
                    chklist.Items.Add(new ListItem { Text = suspenditem });
                }
                foreach (string suspenditem in suspendPolicies.SuspendByPlatform)
                {
                    chklist.Items.Add(new ListItem { Text = suspenditem, Enabled = false, Selected = true });
                }


                foreach (string suspenditem in suspendPolicies.SuspendByCompany)
                {
                    for (int i = 0; i < chkHung.Items.Count; i++)
                    {
                        if (chkHung.Items[i].Text.Equals(suspenditem))
                        {
                            //挂起的政策移除集合中
                            chkHung.Items.Remove(chkHung.Items[i]);
                        }
                    }
                }
                foreach (string suspenditem in suspendPolicies.SuspendByPlatform)
                {
                    for (int i = 0; i < chkHung.Items.Count; i++)
                    {
                        if (chkHung.Items[i].Text.Equals(suspenditem))
                        {
                            //挂起的政策移除集合中
                            chkHung.Items.Remove(chkHung.Items[i]);
                        }
                    }
                }
            }
            if (chklist.Items.Count == 0)
            {
                btnHang.Enabled = false;
                btnSolution.Enabled = false;
            }
            if (chkHung.Items.Count == 0)
            {
                // btnHung.Enabled = false;
            }
        }

        protected void btnHung_Click(object sender, EventArgs e)
        {
            try
            {
                string Reason = txtRemark.Text;
                List<string> airlines = new List<string>();
                foreach (ListItem item in chkHung.Items)
                {
                    if (item.Selected && item.Enabled)
                        airlines.Add(item.Value);
                }
                if (airlines.Count == 0)
                {
                    ShowMessage("挂起航空公司必须选择一个！本次操作被取消！");
                    return;
                }
                var companyId = CurrentCompany.CompanyType == CompanyType.Platform ? Guid.Parse(Request.QueryString["id"]) : CurrentCompany.CompanyId;
                var existsPolicyType = PolicyManageService.CheckIfHasDefaultPolicy(companyId, airlines);
                if (existsPolicyType != PolicyType.Unknown)
                {
                    throw new CustomException(CurrentCompany.CompanyType == CompanyType.Platform ? "该供应商在平台存在" + existsPolicyType.GetDescription() + "指向，请调整后再行操作" :
                        "该航线在平台存在" + existsPolicyType.GetDescription() + "指向，请联系平台调整后再行操作");
                }

                if (txtRemark.Text.Trim() == "")
                {
                    ShowMessage("挂起原因不能为空！请输入！");
                    return;
                }
                if (txtRemark.Text.Trim().Length > 100)
                {
                    txtRemark.Text = txtRemark.Text.Trim().Substring(0, 100);
                    ShowMessage("输入挂起原因过长，只能在100字以内！");
                    return;
                }
                var ipAddress = ChinaPay.AddressLocator.IPAddressLocator.GetRequestIP(HttpContext.Current.Request);
                //var ipLocation = ChinaPay.AddressLocator.CityLocator.GetIPLocation(ipAddress);

                bool falg = PolicyManageService.SuspendPolicies(this.CurrentCompany.CompanyId, this.CurrentUser.UserName, Reason, ipAddress.ToString(), CurrentCompany.CompanyType == CompanyType.Platform ? PublishRole.平台 : PublishRole.用户, airlines.ToArray());
                if (falg)
                {
                    InitValue();
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "挂起政策");
            }
        }
    }
}