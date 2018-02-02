using System;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using System.Collections.Generic;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage
{
    public partial class ExternalInterfaceSetting : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                string companyId = Request.QueryString["CompanyId"];
                lblSecurityCode.Text = companyId.Replace("-", "");
                if (!string.IsNullOrWhiteSpace(companyId))
                {
                    var setting = ExternalInterfaceService.Query(Guid.Parse(companyId));
                    var companyInfo = CompanyService.GetCompanyInfo(Guid.Parse(companyId));
                    if (setting != null)
                    {
                        if (companyInfo.IsOpenExternalInterface)
                        {
                            rbnOpen.Checked = true;
                            rbnClose.Checked = false;
                        }
                        BindExternalInterfaceInfo(setting);
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (valiate())
            {
                try
                {
                    var setting = SaveInfo();
                    Service.Organization.ExternalInterfaceService.Save(setting);
                    RegisterScript("alert('保存成功');window.location.href='ExternalInterfaceList.aspx';", false);
                }
                catch (Exception ex)
                {
                    ShowExceptionMessage(ex, "保存");
                }
            }
        }

        private void BindExternalInterfaceInfo(Service.Organization.Domain.ExternalInterfaceSetting setting)
        {
            lblSecurityCode.Text = setting.SecurityCode;
            if (setting.InterfaceInvokeMethod != null)
            {
                foreach (var item in setting.InterfaceInvokeMethod)
                {
                    if (item == "PNRImport")
                    {
                        chkPNRImport.Checked = true;
                        continue;
                    }
                    if (item == "PNRImportWithoutPat")
                    {
                        chkPNRImportWithoutPat.Checked = true;
                        continue;
                    }
                    if (item == "ProduceOrder")
                    {
                        chkProduceOrder.Checked = true;
                        continue;
                    }
                    if (item == "ApplyRefund")
                    {
                        chkApplyRefund.Checked = true;
                        continue;
                    }
                    if (item == "ApplyPostpone")
                    {
                        chkApplyPostpone.Checked = true;
                        continue;
                    }
                    if (item == "OrderPay")
                    {
                        chkOrderPay.Checked = true;
                        continue;
                    }
                    if (item == "PayOrderByPayType")
                    {
                        chkPayOrderByPayType.Checked = true;
                        continue;
                    }
                    if (item == "PayApplyform")
                    {
                        chkPayApplyform.Checked = true;
                        continue;
                    }
                    if (item == "PayApplyformByPayType")
                    {
                        chkPayApplyformByPayType.Checked = true;
                        continue;
                    }
                    if (item == "QueryOrder")
                    {
                        chkQueryOrder.Checked = true;
                        continue;
                    }
                    if (item == "QueryApplyform")
                    {
                        chkQueryApplyform.Checked = true;
                        continue;
                    }
                    if (item == "AutoPay")
                    {
                        chkAutoPay.Checked = true;
                        continue;
                    }
                    if (item == "QueryFlights")
                    {
                        chkQueryFlights.Checked = true;
                        continue;
                    }
                    if (item == "QueryFlight")
                    {
                        chkQueryFlight.Checked = true;
                        continue;
                    }
                    if (item == "QueryFlightStop")
                    {
                        chkQueryFlightStop.Checked = true;
                        continue;
                    }
                    if (item == "ProduceOrder2")
                    {
                        chkProduceOrder2.Checked = true;
                        continue;
                    }
                    if (item == "ManualPay")
                    {
                        chkManualPay.Checked = true;
                        continue;
                    }
                }
            }
            if ((setting.PolicyTypes & Common.Enums.PolicyType.Normal) == Common.Enums.PolicyType.Normal)
                chkNormal.Checked = true;
            if ((setting.PolicyTypes & Common.Enums.PolicyType.Bargain) == Common.Enums.PolicyType.Bargain)
                chkBargain.Checked = true;
            if ((setting.PolicyTypes & Common.Enums.PolicyType.Team) == Common.Enums.PolicyType.Team)
                chkTeam.Checked = true;
            if ((setting.PolicyTypes & Common.Enums.PolicyType.Special) == Common.Enums.PolicyType.Special)
                chkSpecial.Checked = true;

            if (!string.IsNullOrWhiteSpace(setting.ConfirmSuccessAddress))
                txtConfirmSuccessAddress.Text = setting.ConfirmSuccessAddress;
            if (!string.IsNullOrWhiteSpace(setting.ConfirmFailAddress))
                txtConfirmFailAddress.Text = setting.ConfirmFailAddress;
            if (!string.IsNullOrWhiteSpace(setting.PaySuccessAddress))
                txtPaySuccessAddress.Text = setting.PaySuccessAddress;
            if (!string.IsNullOrWhiteSpace(setting.DrawSuccessAddress))
                txtDrawSuccessAddress.Text = setting.DrawSuccessAddress;
            if (!string.IsNullOrWhiteSpace(setting.RefuseAddress))
                txtRefuseAddress.Text = setting.RefuseAddress;
            if (!string.IsNullOrWhiteSpace(setting.AgreedAddress))
                txtAgreedAddress.Text = setting.AgreedAddress;
            if (!string.IsNullOrWhiteSpace(setting.RefundSuccessAddress))
                txtRefundSuccessAddress.Text = setting.RefundSuccessAddress;
            if (!string.IsNullOrWhiteSpace(setting.ReturnTicketSuccessAddress))
                txtReturnTicketSuccessAddress.Text = setting.ReturnTicketSuccessAddress;
            if (!string.IsNullOrWhiteSpace(setting.ReschedulingAddress))
                txtReschedulingAddress.Text = setting.ReschedulingAddress;
            if (!string.IsNullOrWhiteSpace(setting.RefuseTicketAddress))
                txtRefuseTicketAddress.Text = setting.RefuseTicketAddress;
            if (!string.IsNullOrWhiteSpace(setting.ReschPaymentAddress))
                txtReschPaymentAddress.Text = setting.ReschPaymentAddress;
            if (!string.IsNullOrWhiteSpace(setting.RefuseChangeAddress))
                txtRefuseChangeAddress.Text = setting.RefuseChangeAddress;
            if (!string.IsNullOrWhiteSpace(setting.CanceldulingAddress))
                txtCanceldulingAddress.Text = setting.CanceldulingAddress;
            if (!string.IsNullOrWhiteSpace(setting.RefundApplySuccessAddress))
                txtRefundApplySuccessAddress.Text = setting.RefundApplySuccessAddress;
            txtIP.Text = setting.BindIP;
        }

        private ExternalInterfaceView SaveInfo()
        {
            var setting = new ExternalInterfaceView();
            setting.CompanyId = Guid.Parse(Request.QueryString["CompanyId"]);
            if (!string.IsNullOrWhiteSpace(txtConfirmFailAddress.Text))
                setting.ConfirmFailAddress = txtConfirmFailAddress.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtConfirmSuccessAddress.Text))
                setting.ConfirmSuccessAddress = txtConfirmSuccessAddress.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtDrawSuccessAddress.Text))
                setting.DrawSuccessAddress = txtDrawSuccessAddress.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtPaySuccessAddress.Text))
                setting.PaySuccessAddress = txtPaySuccessAddress.Text.Trim();
            if (!string.IsNullOrWhiteSpace(txtRefuseAddress.Text))
                setting.RefuseAddress = txtRefuseAddress.Text.Trim();

            if (!string.IsNullOrWhiteSpace(txtAgreedAddress.Text))
                setting.AgreedAddress = txtAgreedAddress.Text;
            if (!string.IsNullOrWhiteSpace(txtRefundSuccessAddress.Text))
                setting.RefundSuccessAddress = txtRefundSuccessAddress.Text;
            if (!string.IsNullOrWhiteSpace(txtReturnTicketSuccessAddress.Text))
                setting.ReturnTicketSuccessAddress = txtReturnTicketSuccessAddress.Text;
            if (!string.IsNullOrWhiteSpace(txtReschedulingAddress.Text))
                setting.ReschedulingAddress = txtReschedulingAddress.Text;
            if (!string.IsNullOrWhiteSpace(txtRefuseTicketAddress.Text))
                setting.RefuseTicketAddress = txtRefuseTicketAddress.Text;
            if (!string.IsNullOrWhiteSpace(txtReschPaymentAddress.Text))
                setting.ReschPaymentAddress = txtReschPaymentAddress.Text;
            if (!string.IsNullOrWhiteSpace(txtRefuseChangeAddress.Text))
                setting.RefuseChangeAddress = txtRefuseChangeAddress.Text;
            if (!string.IsNullOrWhiteSpace(txtCanceldulingAddress.Text))
                setting.CanceldulingAddress = txtCanceldulingAddress.Text;
            if (!string.IsNullOrWhiteSpace(txtRefundApplySuccessAddress.Text))
                setting.RefundApplySuccessAddress = txtRefundApplySuccessAddress.Text;
            var interfaceInvokeMethod = new List<string>();
            if (chkPNRImport.Checked)
                interfaceInvokeMethod.Add("PNRImport");
            if (chkApplyPostpone.Checked)
                interfaceInvokeMethod.Add("ApplyPostpone");
            if (chkApplyRefund.Checked)
                interfaceInvokeMethod.Add("ApplyRefund");
            if (chkOrderPay.Checked)
                interfaceInvokeMethod.Add("OrderPay");
            if (chkPayApplyform.Checked)
                interfaceInvokeMethod.Add("PayApplyform");
            if (chkProduceOrder.Checked)
                interfaceInvokeMethod.Add("ProduceOrder");
            if (chkQueryApplyform.Checked)
                interfaceInvokeMethod.Add("QueryApplyform");
            if (chkQueryOrder.Checked)
                interfaceInvokeMethod.Add("QueryOrder");
            if (chkPayOrderByPayType.Checked)
                interfaceInvokeMethod.Add("PayOrderByPayType");
            if (chkPayApplyformByPayType.Checked)
                interfaceInvokeMethod.Add("PayApplyformByPayType");
            if (chkPNRImportWithoutPat.Checked)
                interfaceInvokeMethod.Add("PNRImportWithoutPat");
            if (chkAutoPay.Checked)
                interfaceInvokeMethod.Add("AutoPay");
            if (chkProduceOrder2.Checked)
                interfaceInvokeMethod.Add("ProduceOrder2");
            if (chkQueryFlightStop.Checked)
                interfaceInvokeMethod.Add("QueryFlightStop");
            if (chkQueryFlights.Checked)
                interfaceInvokeMethod.Add("QueryFlights");
            if (chkQueryFlight.Checked)
                interfaceInvokeMethod.Add("QueryFlight");
            if (chkManualPay.Checked)
                interfaceInvokeMethod.Add("ManualPay");
            setting.InterfaceInvokeMethod = interfaceInvokeMethod;
            setting.SecurityCode = lblSecurityCode.Text;
            setting.IsOpenExternalInterface = rbnOpen.Checked;
            if (chkNormal.Checked)
                setting.PolicyTypes |= Common.Enums.PolicyType.Normal;
            if (chkBargain.Checked)
                setting.PolicyTypes |= Common.Enums.PolicyType.Bargain;
            if (chkTeam.Checked)
                setting.PolicyTypes |= Common.Enums.PolicyType.Team;
            if (chkSpecial.Checked)
                setting.PolicyTypes |= Common.Enums.PolicyType.Special;

            setting.BindIP = txtIP.Text;
            return setting;
        }

        private bool valiate()
        {
            if (rbnOpen.Checked)
            {
                if (!string.IsNullOrWhiteSpace(txtConfirmSuccessAddress.Text) && txtConfirmSuccessAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("确认成功通知地址长度不能超过150！");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(txtConfirmFailAddress.Text) && txtConfirmFailAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("确认失败通知地址长度不能超过150！");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(txtPaySuccessAddress.Text) && txtPaySuccessAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("支付成功通知地址长度不能超过150！");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(txtDrawSuccessAddress.Text) && txtDrawSuccessAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("出票成功通知地址长度不能超过150！");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(txtRefuseAddress.Text) && txtRefuseAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("拒绝出票通知地址长度不能超过150！");
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(txtAgreedAddress.Text) && txtAgreedAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("同意改期通知地址不能超过150！");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(txtRefundSuccessAddress.Text) && txtRefundSuccessAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("退废票退款成功通知地址不能超过150！");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(txtReturnTicketSuccessAddress.Text) && txtReturnTicketSuccessAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("退废票处理成功通知地址不能超过150！");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(txtReschedulingAddress.Text) && txtReschedulingAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("改期成功通知地址不能超过150！");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(txtRefuseTicketAddress.Text) && txtRefuseTicketAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("拒绝退废票通知地址不能超过150！");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(txtReschPaymentAddress.Text) && txtReschPaymentAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("改期支付成功通知地址不能超过150！");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(txtCanceldulingAddress.Text) && txtCanceldulingAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("取消出票退款成功通知地址不能超过150！");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(txtRefundApplySuccessAddress.Text) && txtRefundApplySuccessAddress.Text.Trim().Length > 150)
                {
                    ShowMessage("拒绝改期退款成功通知地址不能超过150！");
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(txtIP.Text) && txtIP.Text.Trim().Length > 150)
                {
                    ShowMessage("使用者IP不能为空,不能超过150！");
                    return false;
                }
            }
            return true;
        }
    }
}