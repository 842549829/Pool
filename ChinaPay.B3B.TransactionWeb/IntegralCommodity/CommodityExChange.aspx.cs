using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.Integral;
using ChinaPay.B3B.Service.Integral;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.IntegralCommodity
{
    public partial class CommodityExChange : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    IntegralConsumptionView view = IntegralServer.GetIntegralConsumption(Guid.Parse(Request.QueryString["id"].ToString()));
                    if (view != null)
                    {
                        lblId.Text = view.Id + "";
                        lblAccountName.Text = view.AccountName;
                        lblAccountNo.Text = view.AccountNo;
                        lblCommodityName.Text = view.CommodityName;
                        lblCount.Text = view.CommodityCount + "";
                        lblIntegral.Text = view.ConsumptionIntegral + "";
                        lblCompanyName.Text = view.CompanyShortName;
                        lblPhone.Text = view.AccountPhone;
                        txtNo.Text = view.ExpressDelivery;
                        txtAddress.Text = view.DeliveryAddress;
                        txtCompany.Text = view.ExpressCompany;
                        txtRemark.Text = view.Reason;
                    }
                    if (Request.QueryString["type"] == "chuli")
                    {
                        txtNo.ReadOnly = false;
                        txtAddress.ReadOnly = false;
                        txtCompany.ReadOnly = false;
                        txtRemark.ReadOnly = false;
                        btnSave.Enabled = true;
                        btnSave.Visible = true;
                        btnRefuse.Enabled = true;
                        btnRefuse.Visible = true;
                    }
                    else if (Request.QueryString["type"] == "look")
                    {
                        txtNo.ReadOnly = true;
                        txtAddress.ReadOnly = true;
                        txtCompany.ReadOnly = true;
                        txtRemark.ReadOnly = true;
                        btnSave.Enabled = false;
                        btnSave.Visible = false;
                        btnRefuse.Enabled = false;
                        btnRefuse.Visible = false;
                    }
                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool falg = false;
            try
            {
                IntegralServer.UpdateIntegralConsumption(Guid.Parse(lblId.Text), ExchangeState.Success, this.CurrentUser.Name, txtNo.Text, txtCompany.Text, txtAddress.Text, txtRemark.Text);
                falg = true;
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "处理");
            }
            if (falg)
                RegisterScript("alert('处理成功！');window.location.href='./CommodityExChangeList.aspx';", true);
        }

        protected void btnRefuse_Click(object sender, EventArgs e)
        {
            bool falg = false;
            if (txtRemark.Text.Trim()=="")
            {
                ShowMessage("备注不能为空，请填写！");
                return;
            }
            try
            {
                IntegralServer.UpdateIntegralConsumption(Guid.Parse(lblId.Text), ExchangeState.Refuse, this.CurrentUser.Name, txtNo.Text, txtCompany.Text, txtAddress.Text, txtRemark.Text);
                falg = true;
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "处理");
            }
            if (falg)
                RegisterScript("alert('处理成功！');window.location.href='./CommodityExChangeList.aspx';", true);
        }
    }
}