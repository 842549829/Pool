using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.Service.ExternalPlatform;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide
{
    public partial class Pay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var platformText = Request.QueryString["platformText"];
                var payInterfaceValue = Request.QueryString["payInterfaceValue"];
                var orderAmount = Request.QueryString["orderAmount"];
                var internalOrderId = Request.QueryString["internalOrderId"];
                var externalOrderId = Request.QueryString["externalOrderId"];
                if (!string.IsNullOrWhiteSpace(platformText) && !string.IsNullOrWhiteSpace(payInterfaceValue)
                    && !string.IsNullOrWhiteSpace(internalOrderId) && !string.IsNullOrWhiteSpace(externalOrderId)
                    && !string.IsNullOrWhiteSpace(orderAmount))
                {
                    this.hfdRequetUrl.Value = QueryManualPayUrl(platformText, payInterfaceValue, internalOrderId, externalOrderId, orderAmount);
                }
            }
        }

        private string QueryManualPayUrl(string platformText, string payInterfaceValue, string internalOrderId, string externalOrderId, string orderAmount)
        {
            var result = "";
            try
            {
                var platformType = Enum.GetValues(typeof(PlatformType)) as PlatformType[];
                var newPlatform = PlatformType.Yeexing;
                foreach (var item in platformType)
                {
                    if (item.GetDescription() == platformText)
                        newPlatform = item;
                }
                var url = OrderService.GetPayUrl(newPlatform,
                        decimal.Parse(internalOrderId),
                        externalOrderId,
                       (DataTransferObject.Common.PayInterface)int.Parse(payInterfaceValue),
                        decimal.Parse(orderAmount));
                if (url.Success)
                {
                    result = url.Result;
                }
                if (!string.IsNullOrWhiteSpace(url.ErrMessage))
                {
                    BasePage.ShowMessage(this.Page, url.ErrMessage);
                }
            }
            catch (Exception)
            {
                BasePage.ShowMessage(this.Page, "获取地址失败");
            }
            return result;

        }
    }
}