using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service;
using System.Text.RegularExpressions;

namespace ChinaPay.B3B.MaintenanceWeb.BasicData
{
    public partial class Check_In_New : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["action"] == "upate")
                {
                    BindInta(Request.QueryString["Id"]);
                }
            }
        }

        private void BindInta(string id)
        {
            var check_In = ChinaPay.B3B.Service.FoundationService.Check_Ins.FirstOrDefault(i => i.Id == Guid.Parse(id));
            if (check_In != null)
            {
                ucAriline.Code = check_In.AirlineCode;
                txtHref.Text = check_In.OperatingHref;
                txtRemark.Text = check_In.Remark;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                validation();
                if (Request.QueryString["action"] == "upate") 
                {
                    var check_In = FoundationService.Check_Ins.FirstOrDefault(i => i.Id == Guid.Parse(Request.QueryString["Id"]));
                    check_In.AirlineCode = ucAriline.Code;
                    check_In.AirlineName = FoundationService.Airlines.FirstOrDefault(c => c.Code.Value == ucAriline.Code).Name;
                    check_In.OperatingHref = txtHref.Text.Trim();
                    check_In.Opertor = CurrentUser.UserName;
                    check_In.Remark = txtRemark.Text.Trim();
                    FoundationService.UpdateCheck_In(check_In, CurrentUser.UserName);
                    RegisterScript("alert('修改成功！'); window.location.href='Check_In.aspx'");
                } 
                else 
                {
                    var check_in = new ChinaPay.B3B.Service.Foundation.Domain.Check_In
                    {
                        Id = Guid.NewGuid(),
                        Time = DateTime.Now,
                        AirlineCode = ucAriline.Code,
                        AirlineName = FoundationService.Airlines.FirstOrDefault(c => c.Code.Value == ucAriline.Code).Name,
                        OperatingHref = txtHref.Text.Trim(),
                        Opertor = CurrentUser.UserName,
                        Remark = txtRemark.Text.Trim(),
                    };
                    FoundationService.AddCheck_In(check_in, CurrentUser.UserName);
                    RegisterScript("alert('添加成功！'); window.location.href='Check_In.aspx'");
                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex,"保存");
            }
        }

        private void validation() {
            if (string.IsNullOrEmpty(ucAriline.Code))
            {
                throw new ChinaPay.Core.Exception.InvalidValueException("航空公司不能为空");
            }
            if (!string.IsNullOrEmpty(txtHref.Text.Trim()) && Regex.IsMatch(txtHref.Text.Trim(),@"^[\w\.\\:]{1,100}$"))
            {
                throw new ChinaPay.Core.Exception.InvalidValueException("链接地址格式错误");
            }
            if (!string.IsNullOrEmpty(txtRemark.Text.Trim()) &&  txtRemark.Text.Trim().Length > 1000)
            {
                 throw new ChinaPay.Core.Exception.InvalidValueException("备注信息过长");
            }
        }
    }
}