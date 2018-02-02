using System;
using System.Linq;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Common.Enums;
using System.Collections.Generic;

namespace ChinaPay.B3B.TransactionWeb.OrganizationModule.Account
{
    public partial class AccountInformation : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                BindAccount();
            }
        }
        private void BindAccount()
        {
            var account = Service.Organization.AccountService.Query(CurrentCompany.CompanyId);
            if (account != null)
            {
                var payAccount = account.FirstOrDefault(linq => linq.Type == Common.Enums.AccountType.Payment);
                if (payAccount != null)
                {
                    pay.InnerText = payAccount.No;
                    BindPayAccountInfo(payAccount.No);
                }
                if (CurrentCompany.CompanyType == CompanyType.Purchaser)
                {
                    if (CurrentCompany.IsOem)
                    {
                        BindReceiving(account);
                    }
                    else
                    {
                        HiddenReceiving();
                    }

                }
                else
                {
                    BindReceiving(account);
                }

            }
        }
        private void HiddenReceiving()
        {
            divCollection.Visible = collection.Visible
                                          = liShowCollection.Visible
                                          = liHideCollection.Visible
                                          = replacement_account.Visible
                                          = registerAccountNo.Visible
                                          = false;
        }
        private void BindReceiving(IEnumerable<Service.Organization.Domain.Account> account)
        {
            var collectionAccount = account.FirstOrDefault(linq => linq.Type == Common.Enums.AccountType.Receiving);
            if (collectionAccount != null)
            {
                replacementOpen.Visible = !collectionAccount.Valid;
                collection.InnerText = collectionAccount.No;
                liHideCollection.Visible = false;
                BindCollectionAccountInfo(collectionAccount.No);
            }
            else
            {
                liShowCollection.Visible = false;
            }
        }
        private void BindCollectionAccountInfo(string accountNo)
        {
            var account = PoolPay.Service.AccountBaseService.GetMembershipUser(accountNo);
            if (account != null)
            {
                lblCollectionAccountNo.Text = accountNo;
                lblCollectionAccountName.Text = account.Account.AccountName;
                lblCollectionAddress.Text = account.Account.AccountAddress.State + account.Account.AccountAddress.City + account.Account.AccountAddress.Zone;
                lblCollectionPostCode.Text = account.Account.AccountAddress.PostalCode;
                lblCollectionAdminName.Text = account.UserBaseInfo.Name;
                lblCollectionAdminEmail.Text = account.Email;
                lblCollectionRegisterTime.Text = account.CreationDate.ToString("yyyy-MM-dd HH:mm:ss");
                lblCollectionStatus.Text = account.Account.Status.GetDescription();
            }
        }
        private void BindPayAccountInfo(string accountNo)
        {
            var account = PoolPay.Service.AccountBaseService.GetMembershipUser(accountNo);
            if (account != null)
            {
                lblPayAccountNo.Text = accountNo;
                lblPayAccountName.Text = account.Account.AccountName;
                lblPayAddress.Text = account.Account.AccountAddress.State + account.Account.AccountAddress.City + account.Account.AccountAddress.Zone;
                lblPayPostCode.Text = account.Account.AccountAddress.PostalCode;
                lblPayAdminName.Text = account.UserBaseInfo.Name;
                lblPayAdminEmail.Text = account.Email;
                lblPayRegisterTime.Text = account.CreationDate.ToString("yyyy-MM-dd HH:mm:ss");
                lblPayStatus.Text = account.Account.Status.GetDescription();
            }
        }
    }
}