using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.SMS.Service;
using ChinaPay.SMS.Service.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Integral;
using ChinaPay.B3B.DataTransferObject.Integral;
using ChinaPay.B3B.Service.Integral.Domain;
using ChinaPay.B3B.Common.Enums;

namespace ChinaPay.B3B.TransactionWeb.SmsModule
{
    public partial class SMSBuy : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            RegisterOEMSkins("register.css");
            if (!IsPostBack)
            {
                var count = IntegralServer.GetIntegralByAccountIdZong(this.CurrentUser.Owner);
                if (count != null)
                {
                    lblKeYong.Text = count.IntegralAvailable + "";
                    lblZong.Text = count.IntegralCounts + "";
                }
                else
                {
                    lblKeYong.Text = "0";
                    lblZong.Text = "0";
                }
            }
        }

        protected void btnBuy_Click(object sender, EventArgs e)
        {
            //生成订单
            Order order = null;
            try
            {
                var acc = from item in AccountService.Query(CurrentCompany.CompanyId)
                          where item.Type == Common.Enums.AccountType.Payment
                          select new { No = item.No };
                order = SMSOrderService.Purchase(CurrentCompany.CompanyId, Guid.Parse(hidbackPageId.Value), int.Parse(hidsmsNumber.Value), acc.First().No);
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "购买短信套餐");
            }
            if (order != null)
            {
                Response.Redirect("./SMSPay.aspx?orderid=" + order.Id, true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                IntegralParameterView paraemter = IntegralServer.GetIntegralParameter();
                IntegralCount counts = IntegralServer.GetIntegralByAccountIdZong(CurrentCompany.CompanyId);
                if (counts == null) throw new Exception("积分不够，暂不能兑换该短信！");
                if (counts.IntegralAvailable < int.Parse(hidjf.Value)) throw new Exception("积分不够，暂不能兑换该短信！");
                var consumtion = new IntegralConsumption
                {
                    CompnayId = CurrentCompany.CompanyId,
                    CompanyShortName = CurrentCompany.AbbreviateName,
                    AccountName = CurrentUser.Name,
                    AccountNo = CurrentUser.UserName,
                    AccountPhone = "",
                    DeliveryAddress = "",
                    CommodityCount = int.Parse(hidsmsNumber.Value),
                    CommodityId = Guid.Empty,
                    CommodityName = hidName.Value,
                    Exchange = ExchangeState.Success,
                    ExchangeTiem = DateTime.Now,
                    ExpressCompany = "",
                    ExpressDelivery = "",
                    Reason = "",
                    Remark = "积分兑换商品",
                    Way = IntegralWay.ExchangeSms,
                    ConsumptionIntegral = int.Parse(hidjf.Value)
                };
                IntegralServer.InsertIntegralConsumption(consumtion);
                IntegralServer.UpdateIntegralCountByConsumption(0 - consumtion.ConsumptionIntegral, CurrentUser.Owner);
                var acc = from item in AccountService.Query(CurrentCompany.CompanyId)
                          where item.Type == Common.Enums.AccountType.Payment
                          select new { No = item.No };
                SMSOrderService.ExChangeSms(CurrentCompany.CompanyId, int.Parse(hidjf.Value), int.Parse(hidsmsNumber.Value), int.Parse(hidjfNum.Value), acc.First().No);
                hidShow.Value = "0";
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "兑换短信套餐");
                hidShow.Value = "1";
            }
            if (hidShow.Value == "0")
            {
                RegisterScript("alert('兑换短信成功。');window.location.href='./SMSBuy.aspx';", true);
            }
        }
    }
}