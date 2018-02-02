using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Data.DataMapping;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SystemSettingModule
{
    public partial class ConfigurationAddOrUpdate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                InitData();
                string configId = Request.QueryString["configId"];
                if (!string.IsNullOrWhiteSpace(configId))
                {
                    this.hfdAddOrUpdate.Value = "Update";
                    //获取单个配置信息
                    Configuration config = ConfigurationService.QueryConfiguration(Guid.Parse(configId));
                    Bind(config);
                }
            }
        }

        private void Bind(Configuration config)
        {
            this.txtConfigName.Text = config.Login;
            this.txtConfigPwd.Text = config.Password;
            this.txtSequence.Text = config.PrinterSN.ToString();
            this.txtServerAddress.Text = config.Host;
            this.txtServerDk.Text = config.Port.ToString();
            this.txtSI.Text = config.SI;
            this.dropConfigType.SelectedValue = config.Type.ToString();
            this.txtOfficeNo.Text = config.OfficeNumber;
        }

         private void InitData()
        {
            var info = Enum.GetValues(typeof(ConfigurationType)) as ConfigurationType[];
            foreach (var item in info)
            {
                this.dropConfigType.Items.Add(new ListItem(item.GetDescription(),((int)item).ToString()));
            }
            this.dropConfigType.Items.Insert(0, new ListItem("-请选择-",""));
        }

         private void SaveInfo(Configuration config)
         {
             if (!string.IsNullOrWhiteSpace(this.dropConfigType.SelectedValue))
             {
                 config.Type = int.Parse(this.dropConfigType.SelectedValue);
             }
             config.SI = this.txtSI.Text.Trim();
             if (!string.IsNullOrWhiteSpace(this.txtSequence.Text))
             {
                 config.PrinterSN = int.Parse(this.txtSequence.Text.Trim());
             }
             config.Port = int.Parse(this.txtServerDk.Text.Trim());
             config.Password = this.txtConfigPwd.Text.Trim();
             config.Login = this.txtConfigName.Text.Trim();
             config.Host = this.txtServerAddress.Text.Trim();
             config.Company = this.CurrentCompany.CompanyId;
             config.OfficeNumber = this.txtOfficeNo.Text.Trim();
         }

         protected void btnSave_Click(object sender, EventArgs e)
         {
             if (Valiate())
             {
                 if (this.hfdAddOrUpdate.Value == "Update")
                 {
                     try
                     {
                         Configuration configuration = new Configuration();
                         string configId = Request.QueryString["configId"];
                         Guid config = Guid.Parse(configId);
                         SaveInfo(configuration);
                         configuration.Id = config;
                         ConfigurationService.UpdateConfiguration(configuration, this.CurrentUser.UserName);
                         RegisterScript("alert('保存成功');window.location.href='ConfigurationList.aspx'", false);
                     }
                     catch (Exception ex)
                     {
                         ShowExceptionMessage(ex,"保存");
                     }
                 }
                 else
                 {
                     try
                     {
                         Configuration configuration = new Configuration();
                         configuration.Id = Guid.NewGuid();
                         SaveInfo(configuration);
                         ConfigurationService.AddConfiguration(configuration,this.CurrentUser.UserName);
                         RegisterScript("alert('保存成功');window.location.href='ConfigurationList.aspx'", false);
                     }
                     catch (Exception ex)
                     {
                         ShowExceptionMessage(ex,"保存");
                     }
                 }
             }
         }

         private bool Valiate()
         {
             if (string.IsNullOrWhiteSpace(this.dropConfigType.SelectedValue))
             {
                 ShowMessage("请选择配置类型！");
                 return false;
             }
             if (string.IsNullOrWhiteSpace(this.txtConfigName.Text.Trim()))
             {
                ShowMessage("请输入配置用户名格式！");
                return false;
             }
             else{
                 if (this.txtConfigName.Text.Trim().Length > 20 ||!Regex.IsMatch(this.txtConfigName.Text.Trim(),"^[a-zA-Z\\d]{0,20}$"))
                 {
                     ShowMessage("配置用户名格式错误！");
                     return false;
                 }
             }
             if (string.IsNullOrWhiteSpace(this.txtConfigPwd.Text.Trim()))
             {
                 ShowMessage("请填写配置密码！");
                 return false;
             }
             else
             {
                 if (this.txtConfigPwd.Text.Trim().Length > 20)
                 {
                     ShowMessage("配置密码格式错误！");
                     return false;
                 }
             }
             if (string.IsNullOrWhiteSpace(this.txtServerAddress.Text.Trim()))
             {
                 ShowMessage("请输入服务器地址！");
                 return false;
             }
             else
             {
                 if (!Regex.IsMatch(this.txtServerAddress.Text.Trim(), "^(([1-9]|([1-9]\\d)|(1\\d\\d)|(2([0-4]\\d|5[0-5])))\\.)(([1-9]|([1-9]\\d)|(1\\d\\d)|(2([0-4]\\d|5[0-5])))\\.){2}([1-9]|([1-9]\\d)|(1\\d\\d)|(2([0-4]\\d|5[0-5])))$"))
                 {
                     ShowMessage("服务器地址格式错误！");
                     return false;
                 }
             }
             if (string.IsNullOrWhiteSpace(this.txtServerDk.Text.Trim()))
             {
                 ShowMessage("请输入服务器端口！");
                 return false;
             }
             else
             {
                 if (!Regex.IsMatch(this.txtServerDk.Text.Trim(), "^(\\d)+$"))
                 {
                     ShowMessage("服务器端口格式错误！");
                     return false;
                 }
             }
             if (string.IsNullOrWhiteSpace(this.txtOfficeNo.Text.Trim()))
             {
                 ShowMessage("请输入Office号！");
             }
             else
             {
                 
                 if (this.txtOfficeNo.Text.Trim().Length > 10 || !Regex.IsMatch(this.txtOfficeNo.Text.Trim(),"^[a-zA-Z]{3}[0-9]{3}$"))
                 {
                     ShowMessage("Office号格式错误！");
                     return false;
                 }
             }
             if (string.IsNullOrWhiteSpace(this.txtSI.Text.Trim()))
             {
                 ShowMessage("请输入SI: 工作号/密码！");
                 return false;
             }
             else
             {
                 if (this.txtSI.Text.Trim().Length > 50)
                 {
                     ShowMessage("工作号/密码格式错误！");
                     return false;
                 }
             }
             return true;
         }
    }
}