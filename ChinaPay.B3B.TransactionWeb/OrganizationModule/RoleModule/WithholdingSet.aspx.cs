using System;
using System.Text.RegularExpressions;
using PoolPay.DataTransferObject;
using ChinaPay.PoolPay.Service;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule
{
    public partial class WithholdingSet : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                lblPlatformName.Text = PlatformName;
                BindAliPayAccountInfo();
                BindPoolpayAccountInfo();
            }
        }

        private void BindAliPayAccountInfo()
        {
            WithholdingView alipayWithholding = AccountService.GetWithholding(WithholdingAccountType.Alipay, CurrentCompany.CompanyId);
            if (alipayWithholding != null)
            {
                txtAliPayNo.Text = alipayWithholding.AccountNo;
                if (alipayWithholding.Status != WithholdingProtocolStatus.Success)
                {
                    ModifyAlipayStatus(alipayWithholding);
                }
                else
                {
                    hfdIsProtocol.Value = "false";
                }
            }
        }
        private void BindPoolpayAccountInfo()
        {
            WithholdingView poolpayWithholding = AccountService.GetWithholding(WithholdingAccountType.Poolpay, CurrentCompany.CompanyId);
            if (poolpayWithholding != null && poolpayWithholding.Status == WithholdingProtocolStatus.Success)
            {
                lblPoolpayAccountNo.Text = lblCanclePoolpayAccountNo.Text = poolpayWithholding.AccountNo;
                lblAmount.Text = poolpayWithholding.Amount.TrimInvaidZero();
                lblStatus.Text = poolpayWithholding.Status == WithholdingProtocolStatus.Success ? "有效" : "无效";
                lblTime.Text = poolpayWithholding.Time.ToString("yyyy-MM-dd HH:mm:ss");
                divPoolpaySignedInfo.Visible = true;
                divAgreement.Visible = tabledivSignedOperating.Visible = false;
            }
            else 
            {
                divPoolpaySignedInfo.Visible = false;
                divAgreement.Visible = tabledivSignedOperating.Visible = true;
            }
        }

        private void ModifyAlipayStatus(WithholdingView alipayWithholding)
        {
            ProtocolDTO protocolDTO = new ProtocolDTO() { ChannelId = 1, AccountNo = alipayWithholding.AccountNo };
            bool isProtocol = AccountFillService.QueryProtocolStaus(protocolDTO);
            if (isProtocol)
            {
                setWithholding(alipayWithholding.AccountNo, WithholdingProtocolStatus.Success);
                hfdIsProtocol.Value = "false";
            }
        }

        protected void btnPoolpayNo_Click(object sender, EventArgs e)
        {
            decimal dcAmount;
            string amount = txtAmount.Text.Trim();
            string poolpayAccountNo = txtPoolpayAccountNo.Text.Trim();
            string payPassword = txtPayPassword.Text.Trim();
            if (string.IsNullOrEmpty(amount))
            {
                ShowMessage("自动支付最大金额不能为空");
                return;
            }
            if (!Regex.IsMatch(amount, @"^\d{1,7}(\.\d{1,2})?$"))
            {
                ShowMessage("自动支付最大金额格式错误");
                return;
            }
            if (!decimal.TryParse(amount, out dcAmount))
            {
                ShowMessage("自动支付最大金额格式错误");
                return;
            }
            if (string.IsNullOrEmpty(poolpayAccountNo))
            {
                ShowMessage("国付通账号不能为空");
                return;
            }
            if (string.IsNullOrEmpty(payPassword))
            {
                ShowMessage("支付密码不能为空");
                return;
            }
            PoolPayCAEDTO poolPayCAEDTO = new PoolPayCAEDTO
            {
                Amount = dcAmount,
                PayPassword = payPassword,
                TransAccountOut = poolpayAccountNo,
                TransAccountIn = ChinaPay.B3B.Service.SystemManagement.SystemParamService.PlatformSettleAccount
            };
            try
            {
                AccountTradeService.BuildPoolPayCAE(poolPayCAEDTO);
                WithholdingView withholdingView = createWithholding(dcAmount, poolpayAccountNo);
                AccountService.SetWithholdingInfo(withholdingView);
                BindPoolpayAccountInfo();
                ShowMessage("设置成功");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "设置");
            }
        }

        private WithholdingView createWithholding(decimal dcAmount, string poolpayAccountNo)
        {
            WithholdingView withholdingView = new WithholdingView
            {
                AccountNo = poolpayAccountNo,
                AccountType = WithholdingAccountType.Poolpay,
                Status = WithholdingProtocolStatus.Success,
                Amount = dcAmount,
                Time = DateTime.Now,
                Company = CurrentCompany.CompanyId
            };
            return withholdingView;
        }

        protected void btnAliPaySubmit_Click(object sender, EventArgs e)
        {
            string alipayAccountNo = txtAliPayNo.Text.Trim();
            if (!validationAlipay(alipayAccountNo)) return;
            ProtocolDTO protocolDTO = new ProtocolDTO() { ChannelId = 1, AccountNo = alipayAccountNo };
            string alipayProtocolUrl;
            try
            {
                alipayProtocolUrl = AccountFillService.GetProtocolUrl(protocolDTO);
                setWithholding(alipayAccountNo, WithholdingProtocolStatus.Submitted);
            }
            catch (Exception ex)
            {
                alipayProtocolUrl = string.Empty;
                ShowExceptionMessage(ex, "设置");
            }
            if (!string.IsNullOrEmpty(alipayProtocolUrl))
            {
                Response.Redirect(alipayProtocolUrl, true);
            }
        }

        protected void btnGetWithholdingStatus_Click(object sender, EventArgs e)
        {
            string alipayAccountNo = txtAliPayNo.Text.Trim();
            if (!validationAlipay(alipayAccountNo)) return;
            ProtocolDTO protocolDTO = new ProtocolDTO() { ChannelId = 1, AccountNo = alipayAccountNo };
            if (AccountFillService.QueryProtocolStaus(protocolDTO))
            {
                setWithholding(alipayAccountNo, WithholdingProtocolStatus.Success);
                ShowMessage("签约成功");
            }
            else
            {
                ShowMessage("签约失败");
            }
        }

        private bool validationAlipay(string alipayAccountNo)
        {
            if (string.IsNullOrEmpty(alipayAccountNo))
            {
                ShowMessage("支付宝账号不能为空");
                return false;
            }
            return true;
        }

        private void setWithholding(string alipayAccountNo, WithholdingProtocolStatus status)
        {
            WithholdingView withholdingView = new WithholdingView
            {
                AccountNo = alipayAccountNo,
                AccountType = WithholdingAccountType.Alipay,
                Status = status,
                Amount = 0,
                Time = DateTime.Now,
                Company = CurrentCompany.CompanyId
            };
            AccountService.SetWithholdingInfo(withholdingView);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            WithholdingView poolpayWithholding = AccountService.GetWithholding(WithholdingAccountType.Poolpay, CurrentCompany.CompanyId);
            if (poolpayWithholding != null && poolpayWithholding.Status == WithholdingProtocolStatus.Success)
            {
                try
                {
                    if (string.IsNullOrEmpty(txtCancelPayPassword.Text.Trim())) throw new ChinaPay.Core.Exception.InvalidValueException("支付密码不能为空");
                    AccountTradeService.CancelPoolPayCAE(poolpayWithholding.AccountNo, txtCancelPayPassword.Text.Trim());
                    poolpayWithholding.Status = WithholdingProtocolStatus.Submitted;
                    AccountService.CancelWithholdingInfo(poolpayWithholding);
                    BindPoolpayAccountInfo();
                    ShowMessage("关闭代扣成功");
                }
                catch (Exception ex)
                {

                    ShowExceptionMessage(ex, "关闭代扣");
                }

            }
            else
            {
                ShowMessage("该账号未设置代扣");
            }
        }
    }
}