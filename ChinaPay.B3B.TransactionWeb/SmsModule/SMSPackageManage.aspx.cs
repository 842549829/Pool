using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.SMS.DataTransferObject;
using ChinaPay.SMS.Service;
using ChinaPay.SMS.Service.Domain;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.SmsModule
{
    public partial class SMSPackageManage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                InitDataGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            hidFlag.Value = "0";
            if (string.IsNullOrEmpty(hidId.Value.Trim()))
            {
                ProductDetail detail = new ProductDetail();
                detail.Amount = decimal.Parse(txtAounmt.Text.Trim());
                detail.Count = int.Parse(txtNum.Text.Trim());
                detail.EffectiveDate = DateTime.Parse(txtStartTime.Text);
                detail.ExpiredDate = DateTime.Parse(txtEndTime.Text);
                detail.Enable = true;
                detail.SortLevel = int.Parse(txtSort.Text);
                detail.ExChangeIntegral = txtExChangeIntegral.Text == "" ? 0 : int.Parse(txtExChangeIntegral.Text);
                try
                {
                    SMSProductService.Publish(detail, CurrentUser.UserName);
                }
                catch (Exception ex)
                {
                    hidFlag.Value = "1";
                    ShowExceptionMessage(ex, "添加套餐设置");
                }
            }
            else
            {
                try
                {
                    SMSProductService.Update(Guid.Parse(hidId.Value), DateTime.Parse(txtStartTime.Text), DateTime.Parse(txtEndTime.Text), int.Parse(txtSort.Text), txtExChangeIntegral.Text == "" ? 0 : int.Parse(txtExChangeIntegral.Text), CurrentUser.UserName);
                }
                catch (Exception ex)
                {
                    hidFlag.Value = "1";
                    ShowExceptionMessage(ex, "修改套餐设置");
                }
            }
            InitDataGrid();
        }
        void InitDataGrid()
        {
            var query = SMSProductService.Query();
            grv_Packge.DataSource = from item in query
                                    select new
                                    {
                                        Amount = item.Amount.TrimInvaidZero(),
                                        item.Count,
                                        EffectiveDate = item.EffectiveDate.ToString("yyyy-MM-dd"),
                                        ExpiredDate = item.ExpiredDate.ToString("yyyy-MM-dd"),
                                        item.Id,
                                        item.SortLevel,
                                        item.Enable,
                                        item.UnitPrice,
                                        item.ExChangeIntegral
                                    };
            grv_Packge.DataBind();
        }

        protected void grv_Packge_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DisEndable")
            {
                var str = e.CommandArgument.ToString();
                if (str.Split('|')[1] == "True")
                {
                    try
                    {
                        SMSProductService.Disable(Guid.Parse(str.Split('|')[0]), CurrentUser.UserName);
                        InitDataGrid();
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex, "禁用短信套餐");
                    }
                }
                if (str.Split('|')[1] == "False")
                {
                    try
                    {
                        SMSProductService.Enable(Guid.Parse(str.Split('|')[0]), CurrentUser.UserName);
                        InitDataGrid();
                    }
                    catch (Exception ex)
                    {
                        ShowExceptionMessage(ex, "禁用短信套餐");
                    }
                }
            }
        }

    }
}