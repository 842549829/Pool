using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Policy;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.Service.Order.Domain.Bunk;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.Gateway.Tradement;
using ChinaPay.PoolPay.Service;
using PoolPay.DataTransferObject;
using PoolPay.DomainModel.Membership;
using AccountType = ChinaPay.B3B.Common.Enums.AccountType;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Purchase
{
    public partial class OrderPay : BasePage
    {
        private List<Flight> flights;

        private decimal orderId
        {
            get { return Request.QueryString["id"].ToDecimal(); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                lblPlatformName.Text = PlatformName;
                string id = Request.QueryString["id"];
                decimal businessId;
                if (decimal.TryParse(id, out businessId))
                {
                    if (Request.QueryString["type"] == "2")
                    {
                        payPostponeFee(businessId);
                    }
                    else
                    {
                        payOrder(businessId);
                    }
                    //hidPayHost.Value = Service.SystemManagement.SystemParamService.PayHost;
                    hidUserName.Value = CurrentUser.UserName;
                }
                else
                {
                    showErrorMessage("参数错误");
                }
            }
        }

        private void payOrder(decimal orderId)
        {
            string source = Request.QueryString["source"];
            Order order = OrderQueryService.QueryOrder(orderId);
            if (order == null)
            {
                showErrorMessage("订单 [" + orderId + "] 不存在");
            }
            else
            {
                if (order.Source != OrderSource.PlatformOrder) orderIsImport.Value = "1";
                flights = order.PNRInfos.FirstOrDefault().Flights.ToList();
                PNRInfo pnr = order.PNRInfos.First();
                ShowTicketPrice.Value = pnr.Flights.First().Bunk is FreeBunk ||
                                        pnr.Passengers.First().Price.Fare != 0
                                            ? "1"
                                            : "0";

                // 状态是待申请的，则仅显示订单信息和提示信息
                if (order.Status == OrderStatus.Applied)
                {
                    bindOrder(order, source);
                    divOperations.Visible = false;
                }
                else
                {
                    // 内部机构订单不需要支付
                    if (order.IsInterior)
                    {
                        Response.Redirect("OrderDetail.aspx?id=" + orderId + "&returnUrl=OrderList.aspx");
                    }
                    // 其他情况，均要检查是否能支付和状态
                    string errorMessage;

                    if (OrderProcessService.Payable(orderId, out errorMessage))
                    {
                        string lockErrorMsg;
                        if (Lock(orderId, LockRole.Purchaser, "订单支付", out lockErrorMsg))
                        {
                            bindOrder(order, source);
                            bindPayTypes();
                        }
                        else
                        {
                            showErrorMessage("锁定订单失败。原因:" + lockErrorMsg);
                        }
                        if (!string.IsNullOrEmpty(errorMessage)) showErrorMessage(errorMessage);
                    }
                    else
                    {
                        showErrorMessage(errorMessage);
                    }
                }
            }
        }

        private void payPostponeFee(decimal applyformId)
        {
            PostponeApplyform postponeApplyform = ApplyformQueryService.QueryPostponeApplyform(applyformId);
            if (postponeApplyform == null)
            {
                showErrorMessage("改期申请单 [" + applyformId + "] 不存在");
            }
            else
            {
                flights = postponeApplyform.Flights.Select(f => f.NewFlight).ToList();
                string errorMessage;
                if (ApplyformProcessService.Payable(applyformId, out errorMessage))
                {
                    string lockErrorMsg;
                    if (Lock(applyformId, LockRole.Purchaser, "支付改期费", out lockErrorMsg))
                    {
                        bindApplyform(postponeApplyform);
                        bindPayTypes();
                    }
                    else
                    {
                        showErrorMessage("锁定改期申请单失败。原因:" + lockErrorMsg);
                    }
                }
                else
                {
                    showErrorMessage(errorMessage);
                }
            }
        }

        private void bindOrder(Order order, string source)
        {
            bindTips(order, source);
            bindVoyages(order);
            bindPassengers(order);
            bindParameters(order.Id, "1", order.Product.ProductType, order.Purchaser.Amount);
            bindRefundRule(order, source);
        }

        private void bindApplyform(PostponeApplyform applyform)
        {
            bindTips(null);
            bindVoyages(applyform);
            bindPassengers(applyform);
            bindParameters(applyform.Id, "2", applyform.ProductType, Math.Abs(applyform.PayBill.Applier.Amount));
        }

        private void bindTips(Order order, string source = "")
        {
            divTipInfo.Visible = divProgress.Visible = (order != null && OrderSource.PlatformOrder == order.Source);
            if (divTipInfo.Visible)
            {
                pTipsInfo.InnerHtml = string.Format("您的订单号：<a class='obvious' href='OrderDetail.aspx?id={0}'>{0}</a>已经预订成功 <strong>总价：{1}</strong>",
                    order.Id, order.Purchaser.Amount.ToString("N2"));
            }
        }

        private void bindPayTypes()
        {
            var payTypesHTML = new StringBuilder();
            var payTypeDetailsHTML = new StringBuilder();

            string payAccount = GetCurrentCompanyPayAccount();
            liPayAccount.Text = payAccount;
            PayPoolBindAccount.Value = payAccount;

            if (!string.IsNullOrWhiteSpace(payAccount))
            {
#if(DEBUG)
                FillSourceDTO onlinePayTypes = AccountFillService.GetPaySources();

                // 第三方支付
                payTypesHTML.Append("<li>    <a href='#payType_Online' class='payType_Online'>账户支付</a></li>");
                payTypeDetailsHTML.Append("<div id='payType_Online'><ul class=\"form paysList\">");

                //国付通支付通道
                payTypeDetailsHTML.AppendFormat(
                    "<li title='{1}' style=\"polition:relative\">   <span class=\"icon1 poolpay\">{1}</span>  <input type='hidden' value='{2}' /> <span class=\"payRec\">推荐</span></li>",
                    10000, "国付通", "10000|2" + "|" + "国付通");


                foreach (FillSourceDTO.FillBankDTO item in onlinePayTypes.Channels)
                {
                    payTypeDetailsHTML.AppendFormat("<li title='{1}'>   <span class=\"icon1 Pay{0}\">{1}</span>  <input type='hidden' value='{2}' /> </li>", item.ChannelId,
                        item.BankName, item.Bank + "|" + item.BankName);
                }
                payTypeDetailsHTML.Append("</ul></div>");

                // 网银
                payTypesHTML.Append("<li>    <a href='#payType_Bank' class='payType_Bank'>网上银行</a></li>");
                payTypeDetailsHTML.Append("<div id='payType_Bank' class='tab-item'><ul class='form paysList'>");
                foreach (FillSourceDTO.FillBankDTO item in onlinePayTypes.Banks)
                {
                    //payTypeDetailsHTML.AppendFormat("<li title='{1}'> <span class=\"icon {0}\">{1}</span> <input type='hidden' value='{2}' /></li>", item.BankCode, item.BankName, item.Bank + "|" + item.BankName);

                    if (item.BankType == "0")
                        payTypeDetailsHTML.AppendFormat("<li title='{1}'> <span class=\"icon {0}\">{1}</span> <input type='hidden' value='{2}' />", item.BankCode, item.BankName,
                            item.Bank + "|" + item.BankName);
                    if (item.BankType == "1")
                        payTypeDetailsHTML.AppendFormat(
                            "<li title='{1}'> <span class=\"icon {0}\">{1}</span><span class=\"icon_cop2\">企业</span> <input type='hidden' value='{2}' />", item.BankCode,
                            item.BankName, item.Bank + "|" + item.BankName);
                }
                payTypeDetailsHTML.Append("</ul></div>");
#else
                PayChannelQueryProcessor channelsRequest = new PayChannelQueryProcessor();

                if (channelsRequest.Execute())
                {
                    // 第三方支付
                    payTypesHTML.Append("<li>    <a href='#payType_Online' class='payType_Online'>账户支付</a></li>");
                    payTypeDetailsHTML.Append("<div id='payType_Online'><ul class=\"form paysList\">");

                    int index = 1;//在第一个支付方式上添加推荐标记
                    foreach (PayChannel item in channelsRequest.Channels)
                    {
                        payTypeDetailsHTML.AppendFormat("<li title='{1}'>   <span class=\"icon1 Pay{0}\">{1}</span>  <input type='hidden' value='{2}' /> {3}</li>", item.Code,
                            item.Name, item.Code + "||" + item.Name, index == 1 ? "<span class=\"payRec\">推荐</span>" : "");
                        index++;
                    }
                    payTypeDetailsHTML.Append("</ul></div>");

                    // 网银
                    payTypesHTML.Append("<li>    <a href='#payType_Bank' class='payType_Bank'>网上银行</a></li>");
                    payTypeDetailsHTML.Append("<div id='payType_Bank' class='tab-item'><ul class='form paysList'>");
                    foreach (Bank item in channelsRequest.Banks)
                    {
                        if (item.BankType == "0")
                            payTypeDetailsHTML.AppendFormat("<li title='{1}'> <span class=\"icon {0}\">{1}</span> <input type='hidden' value='{2}' />", item.Code, item.Name,
                                item.Channel+"|"+item.BankChannel + "|" + item.Name);
                        if (item.BankType == "1")
                            payTypeDetailsHTML.AppendFormat(
                                "<li title='{1}'> <span class=\"icon {0}\">{1}</span><span class=\"icon_cop2\">企业</span> <input type='hidden' value='{2}' />", item.Code,
                                item.Name, item.Channel + "|" + item.BankChannel + "|" + item.Name);
                    }
                    payTypeDetailsHTML.Append("</ul></div>");
                }
#endif
            }
            divPayTypes.InnerHtml = payTypesHTML.ToString();
            divPayTypeDetails.InnerHtml = payTypeDetailsHTML.ToString();
        }

        private string GetCurrentCompanyPayAccount()
        {
            Account payAccountModel = AccountService.Query(CurrentCompany.CompanyId, AccountType.Payment);
            string payAccount = payAccountModel != null && payAccountModel.Valid ? payAccountModel.No : string.Empty;
            return payAccount;
        }

        private void bindVoyages(Order order)
        {
            flights = order.PNRInfos.First().Flights.ToList();
            ucVoyages.InitData(order, flights);
            voyagesInfo1.Text =
                voyagesInfo.Text =
                string.Join("<br/>",
                    flights.Select(
                        f => string.Format("{0:yyyy-MM-dd} {1}-{2} {3} {4}{5}机票", f.TakeoffTime, f.Departure.City, f.Arrival.City, f.Carrier.Name, f.Carrier.Code, f.FlightNo)));
        }

        private void bindVoyages(PostponeApplyform applyform) { ucVoyages.InitData(applyform.Order, applyform.Flights.Select(item => item.OriginalFlight)); }
        private void bindPassengers(Order order) { ucPassengers.InitData(order, order.PNRInfos.First().Passengers, order.PNRInfos.First().Flights); }
        private void bindPassengers(PostponeApplyform applyform) { ucPassengers.InitData(applyform.Order, applyform.Passengers, applyform.Flights.Select(f => f.OriginalFlight)); }

        private void bindParameters(decimal businessId, string businessType, ProductType productType, decimal amount)
        {
            hidBusinessId.Value = businessId.ToString();
            hidBusinessType.Value = businessType;
            hidProductType.Value = ((int) productType).ToString();
            liShouldPay1.Text = liShouldPay.Text = lblPayAmount.Text = amount.ToString("F2");
            string payAccount = GetCurrentCompanyPayAccount();
            MembershipUser paypoolUser = AccountBaseService.GetMembershipUser(payAccount);
            decimal restMoney = (string.IsNullOrEmpty(payAccount) || paypoolUser == null) ? 0m : paypoolUser.Account.AvailableBalance;
            liRestMoney.Text = restMoney.ToString("F2");
            liNeedMore.Text = (amount - restMoney).ToString("F2");
        }

        private void bindRefundRule(Order order, string source)
        {
            if (order.IsSpecial)
            {
                RefundAndReschedulingProvision rule = order.IsThirdRelation
                                                          ? order.Supplier.Product.RefundAndReschedulingProvision
                                                          : order.Provider.Product.RefundAndReschedulingProvision;
                hidPublishRefundRule.Value =
                    string.Format(
                        "<div class='hd'><h2>更改规定：</h2><span>{0}</span></div><div class='hd'><h2>作废规定：</h2><span>{1}</span></div><div class='hd'><h2>退票规定：</h2><span>{2}</span></div><div class='hd'><h2>签转规定：</h2><span>{3}</span><div>",
                        rule == null ? string.Empty : rule.Alteration,
                        rule == null ? string.Empty : rule.Scrap,
                        rule == null ? string.Empty : rule.Refund,
                        rule == null ? string.Empty : rule.Transfer
                        );
                if (order.Status == OrderStatus.Applied)
                {
                    hidShowProductAttention.Value = "2";
                }
                else if (order.Status == OrderStatus.Ordered && order.RevisedPrice)
                {
                    hidShowProductAttention.Value = "3";
                }
                else
                {
                    hidShowProductAttention.Value = "1";
                }
            }
            else
            {
                hidShowProductAttention.Value = "";
            }
        }

        private void showErrorMessage(string message)
        {
            if ("用户所定航班已销售完毕" == message)
            {
                message = "您所预定的编码已被NO位或取消，建议您重新查询预定";
            }
            liErrorTip.Text = message;
            if (message.StartsWith("暂时无法验证您导入"))
            {
                RegisterJavaScript(this, "setTimeout(function(){$('#notValidateTip').click();},1000)");
                return;
            }

            if (message.IndexOf("供应商已下班", StringComparison.Ordinal) > -1)
            {
                ErrorOption.Visible = false;
            }
            if (flights == null || !flights.Any())
            {
                ErrorOption.Visible = false; 
                RegisterJavaScript(this, "setTimeout(function(){$('#payDelayed').click();},1000)");
                return;
            }
            Flight fistFlight = flights.FirstOrDefault();
            bool isMutiFlight = flights.Count > 1;
            voyagesInfo1.Text = string.Join("<br/>",
                flights.Select(
                    f => string.Format("{0:yyyy-MM-dd} {1}-{2} {3} {4}{5}机票", f.TakeoffTime, f.Departure.City, f.Arrival.City, f.Carrier.Name, f.Carrier.Code, f.FlightNo)));
            ReSearchFlight.HRef = (isMutiFlight ? "/FlightReserveModule/FlightQueryGoResult.aspx" : "/FlightReserveModule/FlightQueryResult.aspx")
                                  +
                                  string.Format("?source=1&airline=&departure={0}&arrival={1}&goDate={2:yyyy-MM-dd}&backDate={3}", fistFlight.Departure.Code,
                                      fistFlight.Arrival.Code, fistFlight.TakeoffTime, isMutiFlight ? flights.ElementAt(1).TakeoffTime.ToString("yyyy-MM-dd") : string.Empty);
            RegisterJavaScript(this, "setTimeout(function(){$('#payDelayed').click();},1000)");
            payContent.Visible = false;
            //Response.Write(message);
            //Response.End();
        }

        protected void CancleThisOrder(object sender, EventArgs e)
        {
            try
            {
                if (orderId == 0) return;
                OrderProcessService.CancelOrder(orderId, CurrentUser.UserName);
                RegisterJavaScript(this, "alert('取消成功！');location.href='/PurchaseDefault.aspx'");
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex, "取消订单");
            }
        }
    }
}