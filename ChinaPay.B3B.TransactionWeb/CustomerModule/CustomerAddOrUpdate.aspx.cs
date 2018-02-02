using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Organization;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.CustomerModule
{
    public partial class CustomerAddOrUpdate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("core.css");
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");

            if (!IsPostBack)
            {
                InitData();
                string customerId = Request.QueryString["customerId"];
                if (!string.IsNullOrWhiteSpace(customerId))
                {
                    this.hfdAddOrUpdate.Value = "Update";
                    var customer = CustomerService.Query(Guid.Parse(customerId));
                    Bind(customer);
                }
            }
        }

        private void InitData()
        {
            //乘客类型
            var info = Enum.GetValues(typeof(PassengerType)) as PassengerType[];
            foreach (var item in info)
            {
                this.dropCustomerType.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.dropCustomerType.Items.Insert(0, new ListItem("-请选择-", ""));
            //证件类型
            var credentialsType = Enum.GetValues(typeof(CredentialsType)) as CredentialsType[];
            foreach (var item in credentialsType)
            {
                this.dropCertType.Items.Add(new ListItem(item.GetDescription(), ((int)item).ToString()));
            }
            this.dropCertType.Items.Insert(0, new ListItem("-请选择-", ""));
        }

        private void Bind(Customer customer)
        {
            this.txtCertId.Text = customer.Credentials;
            this.txtContactPhone.Text = customer.Mobile;
            this.txtName.Text = customer.Name;
            this.txtRemark.InnerText = customer.Remark;
            switch (customer.PassengerType)
            {
                case PassengerType.Child:
                    this.dropCustomerType.SelectedValue = ((int)PassengerType.Child).ToString();
                    break;
                case PassengerType.Adult:
                    this.dropCustomerType.SelectedValue = ((int)PassengerType.Adult).ToString();
                    break;
            }
            switch (customer.CredentialsType)
            {
                case CredentialsType.出生日期:
                    this.dropCertType.SelectedValue = ((int)CredentialsType.出生日期).ToString();
                    break;
                case CredentialsType.护照:
                    this.dropCertType.SelectedValue = ((int)CredentialsType.护照).ToString();
                    break;
                case CredentialsType.军官证:
                    this.dropCertType.SelectedValue = ((int)CredentialsType.军官证).ToString();
                    break;
                case CredentialsType.其他:
                    this.dropCertType.SelectedValue = ((int)CredentialsType.其他).ToString();
                    break;
                case CredentialsType.身份证:
                    this.dropCertType.SelectedValue = ((int)CredentialsType.身份证).ToString();
                    break;
                case CredentialsType.台胞证:
                    this.dropCertType.SelectedValue = ((int)CredentialsType.台胞证).ToString();
                    break;
                case CredentialsType.学生证:
                    this.dropCertType.SelectedValue = ((int)CredentialsType.学生证).ToString();
                    break;
            }
            if(customer.Sex.HasValue)
            {
                switch (customer.Sex.Value)
                {
                    case Core.Sex.Male:
                        this.rbnMale.Checked = true;
                        break;
                    case Core.Sex.Female:
                        this.rbnFemale.Checked = true;
                        break;
                }
            }
        }

        private void Save(Customer customer)
        {
            customer.Credentials = this.txtCertId.Text;
            if (!string.IsNullOrWhiteSpace(this.dropCertType.SelectedValue))
            {
                customer.CredentialsType = (CredentialsType)int.Parse(this.dropCertType.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(this.dropCustomerType.SelectedValue))
            {
                customer.PassengerType = (PassengerType)int.Parse(this.dropCustomerType.SelectedValue);
            }
            if (this.rbnFemale.Checked)
            {
                customer.Sex = Core.Sex.Female;
            }
            if (this.rbnMale.Checked)
            {
                customer.Sex = Core.Sex.Male;
            }
            customer.Mobile = this.txtContactPhone.Text;
            customer.Name = this.txtName.Text;
            customer.Remark = this.txtRemark.InnerText;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Valiate())
            {
                if (this.hfdAddOrUpdate.Value == "Update")
                {
                    string customerId = Request.QueryString["customerId"];
                    Guid id;
                    if (Guid.TryParse(customerId, out id))
                    {
                        Customer customer = new Customer(id);
                        Save(customer);
                        try
                        {
                            CustomerService.Update(this.CurrentCompany.CompanyId, customer, this.CurrentUser.UserName);
                            RegisterScript("alert('修改成功');window.location.href='CustomerList.aspx'", false);
                        }
                        catch (Exception ex)
                        {
                            ShowExceptionMessage(ex,"修改");
                        }
                    }
                }
                else
                {
                    Customer customer = new Customer();
                    Save(customer);
                    try
                    {
                        CustomerService.Register(this.CurrentCompany.CompanyId, customer, this.CurrentUser.UserName);
                        RegisterScript("alert('添加成功');window.location.href='CustomerList.aspx'", false);
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex,"添加");
                    }
                }
            }
        }

        private bool Valiate()
        {
            if (string.IsNullOrWhiteSpace(this.txtName.Text))
            {
                ShowMessage("请输入姓名！");
                return false;
            }
            else
            {
                if(this.txtName.Text.Trim().Length >25)
                {
                    ShowMessage("姓名格式错误！");
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(this.dropCustomerType.SelectedValue))
            {
                ShowMessage("请选择旅客类型！");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.txtContactPhone.Text.Trim()))
            {
                ShowMessage("请输入联系电话！");
                return false;
            }
            else
            {
                if (!Regex.IsMatch(this.txtContactPhone.Text, "^1[3458]\\d{9}$"))
                {
                    ShowMessage("联系电话格式错误！");
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(this.dropCertType.SelectedValue))
            {
                ShowMessage("请选择证件类型！");
                return false;
            }
            if (string.IsNullOrWhiteSpace(this.txtCertId.Text))
            {
                ShowMessage("请填写证件号码！");
                return false;
            }
            else
            {
                if (this.txtCertId.Text.Trim().Length > 50)
                {
                    ShowMessage("证件号码格式错误！");
                    return false;
                }
            }
            if (this.dropCertType.SelectedValue == "0")
            {
                if (!ValidateIdentifyCard(this.txtCertId.Text.Trim()))
                {
                    return false;
                }
            }
            return true;
        }

        private bool ValidateIdentifyCard(string identifyCardNo)
        {
            var validator = new ChinaPay.IdentityCard.Validator(identifyCardNo);
            validator.Execute();
            return validator.Success;
        }
    }
}