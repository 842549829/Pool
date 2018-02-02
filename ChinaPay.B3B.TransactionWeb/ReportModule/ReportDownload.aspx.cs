using System;
using System.Data;
using System.Text;
using System.Web;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.DataTransferObject.Report;
using ChinaPay.B3B.Service.Report;
using ChinaPay.ExportExcel;
using System.Web.UI.WebControls;
using ChinaPay.B3B.DataTransferObject.FlightTransfer;
using ChinaPay.B3B.Service.FlightTransfer;

namespace ChinaPay.B3B.TransactionWeb.ReportModule
{
    public partial class ReportDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 采购方差额退款表
            string purchaseErrorRefundCondition = Request.QueryString["PurchaseErrorRefundCondition"];
            if (!string.IsNullOrWhiteSpace(purchaseErrorRefundCondition))
            {
                string[] condition = purchaseErrorRefundCondition.Split(',');
                ErrorRefundQueryCondition view = new ErrorRefundQueryCondition();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.ApplyStartTime = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.ApplyEndTime = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.OrderId = decimal.Parse(condition[2]);
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    view.Departure = condition[3];
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.SettleCode = condition[4];
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    view.TicketNo = condition[5];
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.ApplyformId = decimal.Parse(condition[6]);
                }
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    view.Arrival = condition[7];
                }

                if (!string.IsNullOrWhiteSpace(condition[8]))
                    view.Passenger = condition[8];
                if (!string.IsNullOrWhiteSpace(condition[9]))
                {
                    view.ApplierAccount = condition[9];
                }
                if (!string.IsNullOrWhiteSpace(condition[10]))

                    view.Purchase = Guid.Parse(condition[10]);
                DataTable dt = ReportService.DownloadPurchaseErrorRefund(view);
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                dt.Rows.Add(dr);
                dt.Columns["OrderId"].ColumnName = "订单号";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["TicketNos"].ColumnName = "票号";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["FlightNos"].ColumnName = "航班号";
                dt.Columns["Bunks"].ColumnName = "舱位";
                dt.Columns["VoyageNames"].ColumnName = "航程";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["ApplyformId"].ColumnName = "申请单号";
                dt.Columns["AppliedTime"].ColumnName = "申请时间";
                dt.Columns["ProcessedTime"].ColumnName = "处理时间";
                dt.Columns["PurchaserAmount"].ColumnName = "差错金额";
                dt.Columns["ApplierAccountName"].ColumnName = "操作员";
                dt.Columns["PurchaserBillSuccess"].ColumnName = "账单状态";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("差额退款统计"));
            }
            #endregion

            #region 出票方差额退款表
            string providerErrorRefundCondition = Request.QueryString["ProviderErrorRefundCondition"];
            if (!string.IsNullOrWhiteSpace(providerErrorRefundCondition))
            {
                string[] condition = providerErrorRefundCondition.Split(',');
                ErrorRefundQueryCondition view = new ErrorRefundQueryCondition();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.ApplyStartTime = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.ApplyEndTime = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.OrderId = decimal.Parse(condition[2]);
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    view.Departure = condition[3];
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.SettleCode = condition[4];
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    view.TicketNo = condition[5];
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.ApplyformId = decimal.Parse(condition[6]);
                }
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    view.Arrival = condition[7];
                }

                if (!string.IsNullOrWhiteSpace(condition[8]))
                    view.Passenger = condition[8];
                if (!string.IsNullOrWhiteSpace(condition[9]))
                {
                    view.ProcessAccount = condition[9];
                }
                if (!string.IsNullOrWhiteSpace(condition[10]))

                    view.Provider = Guid.Parse(condition[10]);
                DataTable dt = ReportService.DownloadProviderErrorRefund(view);
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                dt.Rows.Add(dr);
                dt.Columns["OrderId"].ColumnName = "订单号";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["TicketNos"].ColumnName = "票号";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["FlightNos"].ColumnName = "航班号";
                dt.Columns["Bunks"].ColumnName = "舱位";
                dt.Columns["VoyageNames"].ColumnName = "航程";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["ApplyformId"].ColumnName = "申请单号";
                dt.Columns["AppliedTime"].ColumnName = "申请时间";
                dt.Columns["ProcessedTime"].ColumnName = "处理时间";
                dt.Columns["ProviderAnticipation"].ColumnName = "应退金额";
                dt.Columns["ProviderTradeFee"].ColumnName = "手续费金额";
                dt.Columns["ProviderAmount"].ColumnName = "实退金额";
                dt.Columns["ProcessorAccountName"].ColumnName = "操作员";
                dt.Columns["ProviderBillSuccess"].ColumnName = "账单状态";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("差额退款统计"));
            }
            #endregion

            #region 平台差额退款表
            string platformErrorRefundCondition = Request.QueryString["PlatformErrorRefundCondition"];
            if (!string.IsNullOrWhiteSpace(platformErrorRefundCondition))
            {
                string[] condition = platformErrorRefundCondition.Split(',');
                ErrorRefundQueryCondition view = new ErrorRefundQueryCondition();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.ApplyStartTime = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.ApplyEndTime = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.OrderId = decimal.Parse(condition[2]);
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    view.Departure = condition[3];
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.SettleCode = condition[4];
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    view.TicketNo = condition[5];
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.ApplyformId = decimal.Parse(condition[6]);
                }
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    view.Arrival = condition[7];
                }

                if (!string.IsNullOrWhiteSpace(condition[8]))
                {
                    view.Purchase = Guid.Parse(condition[8]);
                }
                if (!string.IsNullOrWhiteSpace(condition[9]))

                    view.Provider = Guid.Parse(condition[9]);
                DataTable dt = ReportService.DownloadPlatformErrorRefund(view);
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                dt.Rows.Add(dr);
                dt.Columns["OrderId"].ColumnName = "订单号";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["TicketNos"].ColumnName = "票号";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["FlightNos"].ColumnName = "航班号";
                dt.Columns["Bunks"].ColumnName = "舱位";
                dt.Columns["VoyageNames"].ColumnName = "航程";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["ApplyformId"].ColumnName = "申请单号";
                dt.Columns["AppliedTime"].ColumnName = "申请时间";
                dt.Columns["ProcessedTime"].ColumnName = "处理时间";

                dt.Columns["PurchaserAmount"].ColumnName = "采购所得金额";
                dt.Columns["ProviderAnticipation"].ColumnName = "出票应退金额";
                dt.Columns["ProviderTradeFee"].ColumnName = "出票手续费金额";
                dt.Columns["ProviderAmount"].ColumnName = "出票实退金额";
                dt.Columns["PlatformTradeFee"].ColumnName = "平台手续费金额";
                dt.Columns["PlatformAmount"].ColumnName = "平台应退金额";
                dt.Columns["PurchaserName"].ColumnName = "采购方";
                dt.Columns["ProviderName"].ColumnName = "出票方";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("差额退款统计"));
            }
            #endregion

            #region 采购方差额退款资金报表
            string purchaseErrorRefundFinancialCondition = Request.QueryString["PurchaseErrorRefundFinancialCondition"];
            if (!string.IsNullOrWhiteSpace(purchaseErrorRefundFinancialCondition))
            {
                string[] condition = purchaseErrorRefundFinancialCondition.Split(',');
                ErrorRefundQueryCondition view = new ErrorRefundQueryCondition();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.ApplyStartTime = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.ApplyEndTime = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.OrderId = decimal.Parse(condition[2]);
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    view.Departure = condition[3];
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.SettleCode = condition[4];
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    view.TicketNo = condition[5];
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.ApplyformId = decimal.Parse(condition[6]);
                }
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    view.Arrival = condition[7];
                }

                if (!string.IsNullOrWhiteSpace(condition[8]))
                    view.Passenger = condition[8];
                if (!string.IsNullOrWhiteSpace(condition[9]))
                {
                    view.ApplierAccount = condition[9];
                }
                if (!string.IsNullOrWhiteSpace(condition[10]))

                    view.Purchase = Guid.Parse(condition[10]);
                DataTable dt = ReportService.DownloadPurchaseErrorRefundFinancial(view);
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                dt.Rows.Add(dr);
                dt.Columns["OrderId"].ColumnName = "订单号";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["TicketNos"].ColumnName = "票号";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["FlightNos"].ColumnName = "航班号";
                dt.Columns["Bunks"].ColumnName = "舱位";
                dt.Columns["VoyageNames"].ColumnName = "航程";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["ApplyformId"].ColumnName = "申请单号";
                dt.Columns["AppliedTime"].ColumnName = "申请时间";
                dt.Columns["ProcessedTime"].ColumnName = "处理时间";
                dt.Columns["Amount"].ColumnName = "差错金额";
                dt.Columns["ApplierAccountName"].ColumnName = "操作员";
                dt.Columns["PurchaserBillSuccess"].ColumnName = "账单状态";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("差额退款资金报表"));
            }
            #endregion

            #region 出票方差额退款资金报表
            string providerErrorRefundFinancialCondition = Request.QueryString["ProviderErrorRefundFinancialCondition"];
            if (!string.IsNullOrWhiteSpace(providerErrorRefundFinancialCondition))
            {
                string[] condition = providerErrorRefundFinancialCondition.Split(',');
                ErrorRefundQueryCondition view = new ErrorRefundQueryCondition();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.ApplyStartTime = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.ApplyEndTime = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.OrderId = decimal.Parse(condition[2]);
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    view.Departure = condition[3];
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.SettleCode = condition[4];
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    view.TicketNo = condition[5];
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.ApplyformId = decimal.Parse(condition[6]);
                }
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    view.Arrival = condition[7];
                }

                if (!string.IsNullOrWhiteSpace(condition[8]))
                    view.Passenger = condition[8];
                if (!string.IsNullOrWhiteSpace(condition[9]))
                {
                    view.ProcessAccount = condition[9];
                }
                if (!string.IsNullOrWhiteSpace(condition[10]))

                    view.Provider = Guid.Parse(condition[10]);
                DataTable dt = ReportService.DownloadProviderErrorRefundFinancial(view);
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                dt.Rows.Add(dr);
                dt.Columns["OrderId"].ColumnName = "订单号";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["TicketNos"].ColumnName = "票号";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["FlightNos"].ColumnName = "航班号";
                dt.Columns["Bunks"].ColumnName = "舱位";
                dt.Columns["VoyageNames"].ColumnName = "航程";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["ApplyformId"].ColumnName = "申请单号";
                dt.Columns["AppliedTime"].ColumnName = "申请时间";
                dt.Columns["ProcessedTime"].ColumnName = "处理时间";
                dt.Columns["Anticipation"].ColumnName = "应退金额";
                dt.Columns["TradeFee"].ColumnName = "手续费金额";
                dt.Columns["Amount"].ColumnName = "实退金额";
                dt.Columns["ProcessorAccountName"].ColumnName = "操作员";
                dt.Columns["ProviderBillSuccess"].ColumnName = "账单状态";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("差额退款资金报表"));
            }
            #endregion

            #region 下级提成明细表
            string royaltyProfitCondition = Request.QueryString["royaltyProfitCondition"];
            if (!string.IsNullOrWhiteSpace(royaltyProfitCondition))
            {
                string[] condition = royaltyProfitCondition.Split(',');
                RoyaltyProfitCondition view = new RoyaltyProfitCondition();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.ETDZStartDate = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.ETDZEndDate = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.PNR = condition[2];
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    view.OrderId = decimal.Parse(condition[3]);
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.TicketNo = condition[4];
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    view.PaymentType = (RoyaltyReportType)int.Parse(condition[5]);
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.IsSuccess = condition[6] == "1";
                }
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    view.IsPoolPay = condition[7] == "1";
                }
                if (condition[10] == "Platform")
                {
                    if (!string.IsNullOrWhiteSpace(condition[8]))
                        view.Royalty = Guid.Parse(condition[8]);
                    if (!string.IsNullOrWhiteSpace(condition[12]))
                    {
                        view.PurchaseId = Guid.Parse(condition[12]);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(condition[11]))
                    {
                        view.IncomeGroupId = Guid.Parse(condition[11]);
                    }
                    view.Royalty = Guid.Parse(condition[9]);
                    if (!string.IsNullOrWhiteSpace(condition[13]))
                    {
                        view.PurchaseId = Guid.Parse(condition[13]);
                    }
                }

                DataTable dt = ReportService.DownloadRoyaltyProfit(view);
                DataRow dr = dt.NewRow();
                var tradeFee = dt.Compute("Sum(TradeFee)", "");
                if (tradeFee != DBNull.Value)
                {
                    dr["TradeFee"] = tradeFee;
                }
                var tradeRoyalty = dt.Compute("Sum(Commission)", "");
                if (tradeRoyalty != DBNull.Value)
                {
                    dr["Commission"] = tradeRoyalty;
                }
                var tradeAmount = dt.Compute("Sum(Anticipation)", "");
                if (tradeAmount != DBNull.Value)
                {
                    dr["Anticipation"] = tradeAmount;
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                dt.Rows.Add(dr);
                dt.Columns["RoyaltyName"].ColumnName = "分润方";
                dt.Columns["IncomeGroupName"].ColumnName = "用户组";
                dt.Columns["BillType"].ColumnName = "类型";
                dt.Columns["Success"].ColumnName = "支付状态";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["TicketNos"].ColumnName = "票号";
                dt.Columns["Type"].ColumnName = "机票状态";
                dt.Columns["PNR"].ColumnName = "订座PNR";
                dt.Columns["NewPNR"].ColumnName = "出票PNR";
                dt.Columns["CarrierName"].ColumnName = "承运人";
                dt.Columns["FlightNos"].ColumnName = "航班号";
                dt.Columns["Bunks"].ColumnName = "舱位";
                dt.Columns["VoyageNames"].ColumnName = "航程";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["Fare"].ColumnName = "票面价";
                dt.Columns["Tax"].ColumnName = "税费";
                dt.Columns["Rebate"].ColumnName = "扣点";
                dt.Columns["Increasing"].ColumnName = "加价";
                dt.Columns["TradeFee"].ColumnName = "手续费";
                dt.Columns["Commission"].ColumnName = "提成";
                dt.Columns["Anticipation"].ColumnName = "实收款";
                dt.Columns["PurchaserName"].ColumnName = "采购方";
                dt.Columns["Id"].ColumnName = "订单号";
                dt.Columns["ETDZTime"].ColumnName = "出票时间";
                dt.Columns["PayTime"].ColumnName = "支付时间";
                dt.Columns["RefundTime"].ColumnName = "退票时间";
                dt.Columns["IsPoolpay"].ColumnName = "支付方式";
                if (condition[10] == "Platform")
                {
                    dt.Columns.Remove(dt.Columns["用户组"]);
                }
                else
                {
                    dt.Columns.Remove(dt.Columns["分润方"]);
                }
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("下级提成明细"));
            }
            #endregion

            #region 平台外部订单明细表
            string platformExternalOrderCondition = Request.QueryString["platformExternalOrderCondition"];
            if (!string.IsNullOrWhiteSpace(platformExternalOrderCondition))
            {
                string[] condition = platformExternalOrderCondition.Split(',');
                PlatformExternalOrderView view = new PlatformExternalOrderView();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.BeginPayTime = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.EndPayTime = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.ETDZStatus = short.Parse(condition[2]);
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    view.Airline = condition[3];
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.Departure = condition[4];
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    view.Payed = condition[5] == "1";
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.PNR = condition[6];
                }
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    view.Arrival = condition[7];
                }
                if (!string.IsNullOrWhiteSpace(condition[8]))
                {
                    view.OrderSource = (PlatformType)int.Parse(condition[8]);
                }
                if (!string.IsNullOrWhiteSpace(condition[9]))
                {
                    view.ExternalOrderId = condition[9];
                }
                if (!string.IsNullOrWhiteSpace(condition[10]))
                {
                    view.OrderId = decimal.Parse(condition[10]);
                }
                DataTable dt = ReportService.DownloadPlatformExternalOrder(view);
                DataRow dr = dt.NewRow();
                var receivingAmount = dt.Compute("Sum(ReceivingAmount)", "");
                if (receivingAmount != DBNull.Value)
                {
                    dr["ReceivingAmount"] = receivingAmount;
                }
                var payAmount = dt.Compute("Sum(PayAmount)", "");
                if (payAmount != DBNull.Value)
                {
                    dr["PayAmount"] = payAmount;
                }
                var profit = dt.Compute("Sum(Profit)", "");
                if (profit != DBNull.Value)
                {
                    dr["Profit"] = profit;
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                dt.Rows.Add(dr);
                dt.Columns["PayTime"].ColumnName = "支付时间";
                dt.Columns["ExternalOrderId"].ColumnName = "外部订单号";
                dt.Columns["OrderId"].ColumnName = "内部订单号";
                dt.Columns["Platform"].ColumnName = "订单来源";
                dt.Columns["VoyageNames"].ColumnName = "起抵城市";
                dt.Columns["Airline"].ColumnName = "航空公司";
                dt.Columns["FlightNos"].ColumnName = "航班号";
                dt.Columns["Bunks"].ColumnName = "舱位";
                dt.Columns["Fare"].ColumnName = "票面价";
                dt.Columns["OriginalRebate"].ColumnName = "外部返点";
                dt.Columns["Airport"].ColumnName = "机建/燃油";
                dt.Columns["Paid"].ColumnName = "支付状态";
                dt.Columns["ETDZed"].ColumnName = "出票状态";
                dt.Columns["ReceivingAmount"].ColumnName = "收款金额";
                dt.Columns["PayAmount"].ColumnName = "付款金额";
                dt.Columns["Deduct"].ColumnName = "留点";
                dt.Columns["Profit"].ColumnName = "订单利润";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("平台外部订单明细表"));
            }
            #endregion

            #region 航班变动通知
            string flightChangeNoticeCondition = Request.QueryString["flightChangeNoticeCondition"];
            if (!string.IsNullOrWhiteSpace(flightChangeNoticeCondition))
            {
                string[] condition = flightChangeNoticeCondition.Split(',');
                var recordConditoin = new InfomrRecordSearchConditoin();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    recordConditoin.Carrier = condition[0];
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    recordConditoin.FlightNo = condition[1];
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    recordConditoin.TransferType = (TransferType)int.Parse(condition[2]);
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    recordConditoin.Departure = condition[3];
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    recordConditoin.Arrival = condition[4];
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    recordConditoin.PurchaserId = Guid.Parse(condition[5]);
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    recordConditoin.InformType = (InformType)int.Parse(condition[6]);
                }
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    recordConditoin.InformResult = (InformResult)int.Parse(condition[7]);
                }
                if (!string.IsNullOrWhiteSpace(condition[8]))
                {
                    recordConditoin.InformTimeFrom = DateTime.Parse(condition[8]);
                }
                if (!string.IsNullOrWhiteSpace(condition[9]))
                {
                    recordConditoin.InformTimeTo = DateTime.Parse(condition[9]).AddDays(1).AddMilliseconds(-3);
                }
                DataTable dt = QSService.QueryInformRecords(recordConditoin);
                dt.Columns["Login"].ColumnName = "采购账号";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["Airlnes"].ColumnName = "航线";
                dt.Columns["FlightNo"].ColumnName = "航班号";
                dt.Columns["TransferType"].ColumnName = "变更类型";
                dt.Columns["InformTime"].ColumnName = "通知时间";
                dt.Columns["InformMethod"].ColumnName = "通知方式";
                dt.Columns["InformResult"].ColumnName = "通知结果";
                dt.Columns["InformerName"].ColumnName = "操作人";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("查看通知记录"));
            }
            #endregion

            #region 平台机票销售表
            string platformTicketCondition = Request.QueryString["platformTicketCondition"];
            if (!string.IsNullOrWhiteSpace(platformTicketCondition))
            {
                string[] condition = platformTicketCondition.Split(',');
                PlatformTicketView view = new PlatformTicketView();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.FinishBeginTime = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.FinishEndTime = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.Airline = condition[2];
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    view.TicketState = (TicketState)int.Parse(condition[3]);
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.Provider = Guid.Parse(condition[4]);
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    view.Supplier = Guid.Parse(condition[5]);
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.Purchaser = Guid.Parse(condition[6]);
                }
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    view.TicketNo = condition[7];
                }
                if (!string.IsNullOrWhiteSpace(condition[8]))
                {
                    view.Passenger = condition[8];
                }
                if (!string.IsNullOrWhiteSpace(condition[9]))
                {
                    view.PNR = condition[9];
                }
                if (!string.IsNullOrWhiteSpace(condition[10]))
                {
                    view.RelationType = (RelationType)int.Parse(condition[10]);
                }
                if (!string.IsNullOrWhiteSpace(condition[11]))
                {
                    view.OrderId = decimal.Parse(condition[11]);
                }
                if (!string.IsNullOrWhiteSpace(condition[12]))
                {
                    view.TakeoffBeginDate = DateTime.Parse(condition[12]);
                }
                if (!string.IsNullOrWhiteSpace(condition[13]))
                {
                    view.TakeoffEndDate = DateTime.Parse(condition[13]);
                }
                if (!string.IsNullOrWhiteSpace(condition[14]))
                {
                    view.PayType = condition[14] == "1";
                }
                DataTable dt = ReportService.DownloadPlatformTicket(view);
                DataRow dr = dt.NewRow();
                var purchaserAmount = dt.Compute("Sum(PurchaserAmount)", "");
                if (purchaserAmount != DBNull.Value)
                {
                    dr["PurchaserAmount"] = purchaserAmount;
                }
                var providerAmount = dt.Compute("Sum(ProviderAmount)", "");
                if (providerAmount != DBNull.Value)
                {
                    dr["ProviderAmount"] = providerAmount;
                }
                var supplierAmount = dt.Compute("Sum(SupplierAmount)", "");
                if (supplierAmount != DBNull.Value)
                {
                    dr["SupplierAmount"] = supplierAmount;
                }
                var royalAmount = dt.Compute("Sum(RoyaltyAmount)", "");
                if (royalAmount != DBNull.Value)
                {
                    dr["RoyaltyAmount"] = royalAmount;
                }
                var postponeFee = dt.Compute("Sum(PostponeFee)", "");
                if (postponeFee != DBNull.Value)
                {
                    dr["PostponeFee"] = postponeFee;
                }

                var platformCommission = dt.Compute("Sum(PlatformCommission)", "");
                if (platformCommission != DBNull.Value)
                {
                    dr["PlatformCommission"] = platformCommission;
                }
                var platformProfit = dt.Compute("Sum(PlatformProfit)", "");
                if (platformProfit != DBNull.Value)
                {
                    dr["PlatformProfit"] = platformProfit;
                }
                var platformPremium = dt.Compute("Sum(Premium)", "");
                if (platformPremium != DBNull.Value)
                {
                    dr["Premium"] = platformPremium;
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                dt.Rows.Add(dr);
                dt.Columns["FinishTime"].ColumnName = "时间";
                dt.Columns["ProviderName"].ColumnName = "出票方";
                dt.Columns["SupplierName"].ColumnName = "产品方";
                dt.Columns["PurchaserName"].ColumnName = "采购方";
                dt.Columns["SellReleation"].ColumnName = "销售关系";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["TicketNo"].ColumnName = "票号";
                dt.Columns["TripType"].ColumnName = "行程";
                dt.Columns["Type"].ColumnName = "机票状态";
                dt.Columns["Id"].ColumnName = "订单号";
                dt.Columns["FlightNo"].ColumnName = "航班号";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["Voyage"].ColumnName = "航程(中文)";
                dt.Columns["EnglishVoyage"].ColumnName = "航程(代码)";
                dt.Columns["TakeoffTime"].ColumnName = "航班时间";
                dt.Columns["Bunk"].ColumnName = "舱位";
                dt.Columns["Fare"].ColumnName = "票面价";
                dt.Columns["PNR"].ColumnName = "订座PNR";
                dt.Columns["NewPNR"].ColumnName = "出票PNR";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["ServiceCharge"].ColumnName = "服务费";
                dt.Columns["AirportFee"].ColumnName = "民航基金";
                dt.Columns["BAF"].ColumnName = "燃油附加费";
                dt.Columns["ProviderRebate"].ColumnName = "卖出返点";
                dt.Columns["PurchaserRebate"].ColumnName = "买入返点";
                dt.Columns["PlatformRebate"].ColumnName = "平台扣点/贴点";
                dt.Columns["SupplierRebate"].ColumnName = "产品方返点";
                dt.Columns["PurchaserAmount"].ColumnName = "采购方支付金额";
                dt.Columns["ProviderAnticipation"].ColumnName = "出票方应收款";
                dt.Columns["ProviderTradeRate"].ColumnName = "出票方手续费率";
                dt.Columns["ProviderTradeFee"].ColumnName = "出票方手续费";
                dt.Columns["ProviderAmount"].ColumnName = "出票方实收款";
                dt.Columns["SupplierCommission"].ColumnName = " 产品方佣金";
                dt.Columns["SupplierAnticipation"].ColumnName = "产品方应收款";
                dt.Columns["SupplierTradeRate"].ColumnName = "产品方手续费率";
                dt.Columns["SupplierTradeFee"].ColumnName = "产品方手续费";
                dt.Columns["SupplierAmount"].ColumnName = "产品方实收款";
                dt.Columns["PostponeFee"].ColumnName = "平台改签收入";
                dt.Columns["PlatformCommission"].ColumnName = "政策扣点/贴点收益";
                dt.Columns["Premium"].ColumnName = "溢价收入";
                dt.Columns["PlatformProfit"].ColumnName = "平台利润";
                dt.Columns["PayType"].ColumnName = "支付方式";
                dt.Columns["Speed"].ColumnName = "出票效率(分)";
                dt.Columns["RoyaltyRebate"].ColumnName = "分润方返点";
                dt.Columns["RoyaltyCommission"].ColumnName = "分润方佣金";
                dt.Columns["RoyaltyIncreasing"].ColumnName = "分润方加价金额";
                dt.Columns["RoyaltyAnticipation"].ColumnName = "分润方应收款";
                dt.Columns["RoyaltyTradeRate"].ColumnName = "分润方手续费率";
                dt.Columns["RoyaltyTradeFee"].ColumnName = "分润方手续费";
                dt.Columns["RoyaltyAmount"].ColumnName = "分润方实收款";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("平台机票销售表"));
            }
            #endregion

            #region 平台采购量统计
            string purchaseStatisticsCondtion = Request.QueryString["purchaseStatisticsCondition"];
            if (!string.IsNullOrWhiteSpace(purchaseStatisticsCondtion))
            {
                string[] condition = purchaseStatisticsCondtion.Split(',');
                var view = new PurchaseStatisticView();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                    view.ReportStartDate = DateTime.Parse(condition[0]);
                if (!string.IsNullOrWhiteSpace(condition[1]))
                    view.ReportEndDate = DateTime.Parse(condition[1]);
                if (!string.IsNullOrWhiteSpace(condition[2]))
                    view.Departure = condition[2];
                if (!string.IsNullOrWhiteSpace(condition[3]))
                    view.Carrier = condition[3];
                if (!string.IsNullOrWhiteSpace(condition[4]))
                    view.Purchase = Guid.Parse(condition[4]);
                if (!string.IsNullOrWhiteSpace(condition[5]))
                    view.IsHasTrade = bool.Parse(condition[5]);
                DataTable dt = ReportService.DownloadPurchaseStatistics(view);
                DataRow dr = dt.NewRow();
                var orderCount = dt.Compute("Sum(OrderCount)", "");
                if (orderCount != DBNull.Value)
                {
                    dr["OrderCount"] = orderCount;
                }
                var ticketCount = dt.Compute("Sum(TicketCount)", "");
                if (ticketCount != DBNull.Value)
                {
                    dr["TicketCount"] = ticketCount;
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                dt.Rows.Add(dr);
                dt.Columns["Name"].ColumnName = "公司名称";
                dt.Columns["UserName"].ColumnName = "用户名";
                dt.Columns["Contact"].ColumnName = "联系人";
                dt.Columns["Mobile"].ColumnName = "联系电话";
                dt.Columns["RegisterDate"].ColumnName = "注册日期";
                dt.Columns["ReportDate"].ColumnName = "统计日期";
                dt.Columns.Remove("Carrier");
                dt.Columns.Remove("Departure");
                dt.Columns["OrderCount"].ColumnName = "订单数";
                dt.Columns["TicketCount"].ColumnName = "票号数";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("平台采购量统计"));
            }
            #endregion

            #region 平台出票量统计
            string ProviderStatisticsCondtion = Request.QueryString["ProviderStatisticsCondition"];
            if (!string.IsNullOrWhiteSpace(ProviderStatisticsCondtion))
            {
                string[] condition = ProviderStatisticsCondtion.Split(',');
                var searchCondition = new ProviderStatisticSearchCondition();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                    searchCondition.ReportStartDate = DateTime.Parse(condition[0]);
                if (!string.IsNullOrWhiteSpace(condition[1]))
                    searchCondition.ReportEndDate = DateTime.Parse(condition[1]);
                if (!string.IsNullOrWhiteSpace(condition[2]))
                    searchCondition.Departure = condition[2];
                if (!string.IsNullOrWhiteSpace(condition[3]))
                    searchCondition.Carrier = condition[3];
                if (!string.IsNullOrWhiteSpace(condition[4]))
                    searchCondition.Provider = Guid.Parse(condition[4]);
                if (!string.IsNullOrWhiteSpace(condition[5]))
                    searchCondition.IsHasTrade = bool.Parse(condition[5]);
                if (!string.IsNullOrEmpty(condition[6]))
                {
                    searchCondition.Arrival = condition[6];
                }
                if (!string.IsNullOrEmpty(condition[7]))
                {
                    searchCondition.SaleRelation = (RelationType)int.Parse(condition[7]);
                }
                if (!string.IsNullOrEmpty(condition[8]))
                {
                    searchCondition.ProductType = (ProductType)int.Parse(condition[8]);
                    if (searchCondition.ProductType == ProductType.Special && !string.IsNullOrEmpty(condition[9]))
                    {
                        searchCondition.SpecialProductType = (SpecialProductType)int.Parse(condition[9]);
                    }
                }
                DataTable dt = ReportService.DownloadProviderStatistics(searchCondition);
                DataRow dr = dt.NewRow();
                var orderCount = dt.Compute("Sum(OrderCount)", "");
                if (orderCount != DBNull.Value)
                {
                    dr["OrderCount"] = orderCount;
                }
                var ticketCount = dt.Compute("Sum(TicketCount)", "");
                if (ticketCount != DBNull.Value)
                {
                    dr["TicketCount"] = ticketCount;
                }
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                dt.Rows.Add(dr);
                dt.Columns["Name"].ColumnName = "公司名称";
                dt.Columns["UserName"].ColumnName = "用户名";
                dt.Columns["Contact"].ColumnName = "联系人";
                dt.Columns["Mobile"].ColumnName = "联系电话";
                dt.Columns["RegisterDate"].ColumnName = "注册日期";
                dt.Columns["ReportDate"].ColumnName = "统计日期";
                dt.Columns["OrderCount"].ColumnName = "订单数";
                dt.Columns["TicketCount"].ColumnName = "票号数";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("平台出票量统计"));
            }
            #endregion

            #region 产品方
            string supplyReportCondition = Request.QueryString["supplyTicketCondition"];
            if (!string.IsNullOrWhiteSpace(supplyReportCondition))
            {
                string[] condition = supplyReportCondition.Split(',');
                SupplyTicketView view = new SupplyTicketView();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.FinishBeginTime = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.FinishEndTime = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.OrderId = decimal.Parse(condition[2]);
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    view.PNR = condition[3];
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.TicketState = (TicketState)int.Parse(condition[4]);
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    view.Ariline = condition[5];
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.CompanyId = Guid.Parse(condition[6]);
                }
                if (!string.IsNullOrWhiteSpace(condition[7]) && condition[7] != "undefined")
                {
                    view.SpecialProductType = (SpecialProductType)int.Parse(condition[7]);
                }
                DataTable dt = ReportService.DownloadSupplyTicket(view);
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                var tradeAmount = dt.Compute("Sum(TradeAmount)", "");
                DataRow dr = dt.NewRow();
                dr["TradeAmount"] = tradeAmount;
                dt.Rows.Add(dr);
                dt.Columns["Type"].ColumnName = "机票状态";
                dt.Columns["Id"].ColumnName = "订单号";
                dt.Columns["TripType"].ColumnName = "行程";
                dt.Columns["TicketType"].ColumnName = "客票类型";
                dt.Columns["SpecialProductType"].ColumnName = "特殊票类型";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["TicketNo"].ColumnName = "票号";
                dt.Columns["FlightNo"].ColumnName = "航班号";
                dt.Columns["ChineseVoyage"].ColumnName = "航程(中文)";
                dt.Columns["EnglishVoyage"].ColumnName = "航程(代码)";
                dt.Columns["PayTime"].ColumnName = "支付时间";
                dt.Columns["ETDZTime"].ColumnName = "出票时间";
                dt.Columns["TakeoffTime"].ColumnName = "乘机时间";
                dt.Columns["RefundTime"].ColumnName = "退废时间";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["Bunk"].ColumnName = "舱位";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["Fare"].ColumnName = "票面价";
                dt.Columns["ServiceCharge"].ColumnName = "服务费";
                dt.Columns["AirportFee"].ColumnName = "民航基金";
                dt.Columns["BAF"].ColumnName = "燃油附加费";
                dt.Columns["PNR"].ColumnName = "订座PNR";
                dt.Columns["NewPNR"].ColumnName = "出票PNR";
                dt.Columns["Rebate"].ColumnName = "返点";
                dt.Columns["Commission"].ColumnName = "提成";
                dt.Columns["Anticipation"].ColumnName = "应收款";
                dt.Columns["TradeFee"].ColumnName = "交易手续费";
                dt.Columns["TradeAmount"].ColumnName = "实收款";
                dt.Columns["Success"].ColumnName = "账单状态";
                dt.Columns["TradeAccount"].ColumnName = "收款账号";
                dt.Columns["Remark"].ColumnName = "备注";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("提成明细"));
            }
            #endregion

            #region 采购方
            string purchaseTicketCondition = Request.QueryString["purchaseTicketCondition"];
            if (!string.IsNullOrWhiteSpace(purchaseTicketCondition))
            {
                string[] condition = purchaseTicketCondition.Split(',');
                PurchaseTicketView view = new PurchaseTicketView();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.FinishBeginDate = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.FinishEndDate = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.PayBeginDate = DateTime.Parse(condition[2]);
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    view.PayEndDate = DateTime.Parse(condition[3]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.TakeoffBeginDate = DateTime.Parse(condition[4]);
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    view.TakeoffEndDate = DateTime.Parse(condition[5]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.TicketNo = condition[6];
                }
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    view.Passenger = condition[7];
                }
                if (!string.IsNullOrWhiteSpace(condition[8]))
                {
                    view.PNR = condition[8];
                }
                if (!string.IsNullOrWhiteSpace(condition[9]))
                {
                    view.Departure = condition[9];
                }
                if (!string.IsNullOrWhiteSpace(condition[10]))
                {
                    view.Arrival = condition[10];
                }
                if (!string.IsNullOrWhiteSpace(condition[11]))
                {
                    view.TicketState = (TicketState)int.Parse(condition[11]);
                }
                if (!string.IsNullOrWhiteSpace(condition[12]))
                {
                    view.PolicyType = Convert.ToByte(condition[12]);
                }
                if (!string.IsNullOrWhiteSpace(condition[13]))
                {
                    view.Airline = condition[13];
                }
                if (!string.IsNullOrWhiteSpace(condition[14]))
                {
                    view.TicketType = (TicketType)int.Parse(condition[14]);
                }
                if (!string.IsNullOrWhiteSpace(condition[15]))
                {
                    view.CompanyId = Guid.Parse(condition[15]);
                }
                if (!string.IsNullOrWhiteSpace(condition[16]))
                {
                    view.OrderId = decimal.Parse(condition[16]);
                }
                if (!string.IsNullOrWhiteSpace(condition[17]))
                {
                    view.PayType = condition[17] == "1";
                }
                DataTable dt = ReportService.DownloadPurchaseTicket(view);
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                DataRow dr = dt.NewRow();
                dr["TradeAmount"] = dt.Compute("Sum(TradeAmount)", "");
                dt.Rows.Add(dr);
                dt.Columns["Type"].ColumnName = "机票状态";
                dt.Columns["Id"].ColumnName = "订单号";
                dt.Columns["TripType"].ColumnName = "行程";
                dt.Columns["TicketType"].ColumnName = "客票类型";
                dt.Columns["Product"].ColumnName = "政策类型";
                dt.Columns["TicketNo"].ColumnName = "票号";
                dt.Columns["FlightNo"].ColumnName = "航班号";
                dt.Columns["ChineseVoyage"].ColumnName = "航程(中文)";
                dt.Columns["EnglishVoyage"].ColumnName = "航程(代码)";
                dt.Columns["PayTime"].ColumnName = "支付时间";
                dt.Columns["ETDZTime"].ColumnName = "出票时间";
                dt.Columns["TakeoffTime"].ColumnName = "乘机时间";
                dt.Columns["RefundOrPostponeTime"].ColumnName = "退废时间";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["Bunk"].ColumnName = "舱位";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["Fare"].ColumnName = "票面价";
                dt.Columns["ServiceCharge"].ColumnName = "服务费";
                dt.Columns["AirportFee"].ColumnName = "民航基金";
                dt.Columns["BAF"].ColumnName = "燃油附加费";
                dt.Columns["Rebate"].ColumnName = "返点";
                dt.Columns["PNR"].ColumnName = "订座PNR";
                dt.Columns["NewPNR"].ColumnName = "出票PNR";
                dt.Columns["Commission"].ColumnName = "佣金";
                dt.Columns["RefundOrPostponeFee"].ColumnName = "退改签手续费";
                dt.Columns["TradeAmount"].ColumnName = "实付款";
                dt.Columns["Success"].ColumnName = "账单状态";
                dt.Columns["OperatorAccount"].ColumnName = "操作员";
                dt.Columns["TradeAccount"].ColumnName = "付款账号";
                dt.Columns["Remark"].ColumnName = "备注";
                dt.Columns["PayType"].ColumnName = "支付方式";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("买入机票明细"));
            }
            #endregion

            #region 采购方(新)
            string newPurchaseTicketCondition = Request.QueryString["NewPurchaseTicketCondition"];
            if (!string.IsNullOrWhiteSpace(newPurchaseTicketCondition))
            {
                string[] condition = newPurchaseTicketCondition.Split(',');
                PurchaseTicketView view = new PurchaseTicketView();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.FinishBeginDate = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.FinishEndDate = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.PayBeginDate = DateTime.Parse(condition[2]);
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    view.PayEndDate = DateTime.Parse(condition[3]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.TakeoffBeginDate = DateTime.Parse(condition[4]);
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    view.TakeoffEndDate = DateTime.Parse(condition[5]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.TicketNo = condition[6];
                }
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    view.Passenger = condition[7];
                }
                if (!string.IsNullOrWhiteSpace(condition[8]))
                {
                    view.PNR = condition[8];
                }
                if (!string.IsNullOrWhiteSpace(condition[9]))
                {
                    view.Departure = condition[9];
                }
                if (!string.IsNullOrWhiteSpace(condition[10]))
                {
                    view.Arrival = condition[10];
                }
                if (!string.IsNullOrWhiteSpace(condition[11]))
                {
                    view.TicketState = (TicketState)int.Parse(condition[11]);
                }
                if (!string.IsNullOrWhiteSpace(condition[12]))
                {
                    view.PolicyType = Convert.ToByte(condition[12]);
                }
                if (!string.IsNullOrWhiteSpace(condition[13]))
                {
                    view.Airline = condition[13];
                }
                if (!string.IsNullOrWhiteSpace(condition[14]))
                {
                    view.TicketType = (TicketType)int.Parse(condition[14]);
                }
                if (!string.IsNullOrWhiteSpace(condition[15]))
                {
                    view.CompanyId = Guid.Parse(condition[15]);
                }
                if (!string.IsNullOrWhiteSpace(condition[16]))
                {
                    view.OrderId = decimal.Parse(condition[16]);
                }
                if (!string.IsNullOrWhiteSpace(condition[17]))
                {
                    view.PayType = condition[17] == "1";
                }
                DataTable dt = ReportService.DownloadNewPurchaseTicket(view);
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                DataRow dr = dt.NewRow();
                dr["TradeAmount"] = dt.Compute("Sum(TradeAmount)", "");
                dt.Rows.Add(dr);
                dt.Columns["Type"].ColumnName = "机票状态";
                dt.Columns["Id"].ColumnName = "订单号";
                dt.Columns["TripType"].ColumnName = "行程";
                dt.Columns["TicketType"].ColumnName = "客票类型";
                dt.Columns["Product"].ColumnName = "政策类型";
                dt.Columns["TicketNos"].ColumnName = "票号";
                dt.Columns["FlightNos"].ColumnName = "航班号";
                dt.Columns["VoyageNames"].ColumnName = "航程(中文)";
                dt.Columns["Voyages"].ColumnName = "航程(代码)";
                dt.Columns["PayTime"].ColumnName = "支付时间";
                dt.Columns["ETDZTime"].ColumnName = "出票时间";
                dt.Columns["TakeoffTimes"].ColumnName = "乘机时间";
                dt.Columns["RefundOrPostponeTime"].ColumnName = "退改签时间";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["Bunks"].ColumnName = "舱位";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["Fare"].ColumnName = "票面价";
                dt.Columns["ServiceCharge"].ColumnName = "服务费";
                dt.Columns["AirportFee"].ColumnName = "民航基金";
                dt.Columns["BAF"].ColumnName = "燃油附加费";
                dt.Columns["Rebate"].ColumnName = "返点";
                dt.Columns["PNR"].ColumnName = "订座PNR";
                dt.Columns["NewPNR"].ColumnName = "出票PNR";
                dt.Columns["Commission"].ColumnName = "佣金";
                dt.Columns["RefundOrPostponeFee"].ColumnName = "退改签手续费";
                dt.Columns["TradeAmount"].ColumnName = "实付款";
                dt.Columns["Success"].ColumnName = "账单状态";
                dt.Columns["OperatorAccount"].ColumnName = "操作员";
                dt.Columns["TradeAccount"].ColumnName = "付款账号";
                dt.Columns["Remark"].ColumnName = "备注";
                dt.Columns["PayType"].ColumnName = "支付方式";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("买入机票明细"));
            }
            #endregion

            #region 采购方资金报表
            string purchaseFinancialCondition = Request.QueryString["purchaseFinancialCondition"];
            if (!string.IsNullOrWhiteSpace(purchaseFinancialCondition))
            {
                string[] condition = purchaseFinancialCondition.Split(',');
                PurchaseTicketView view = new PurchaseTicketView();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.FinishBeginDate = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.FinishEndDate = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.PayBeginDate = DateTime.Parse(condition[2]);
                }
                if (!string.IsNullOrWhiteSpace(condition[3]))
                {
                    view.PayEndDate = DateTime.Parse(condition[3]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.TakeoffBeginDate = DateTime.Parse(condition[4]);
                }
                if (!string.IsNullOrWhiteSpace(condition[5]))
                {
                    view.TakeoffEndDate = DateTime.Parse(condition[5]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.TicketNo = condition[6];
                }
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    view.Passenger = condition[7];
                }
                if (!string.IsNullOrWhiteSpace(condition[8]))
                {
                    view.PNR = condition[8];
                }
                if (!string.IsNullOrWhiteSpace(condition[9]))
                {
                    view.Departure = condition[9];
                }
                if (!string.IsNullOrWhiteSpace(condition[10]))
                {
                    view.Arrival = condition[10];
                }
                if (!string.IsNullOrWhiteSpace(condition[11]))
                {
                    view.TicketState = (TicketState)int.Parse(condition[11]);
                }
                if (!string.IsNullOrWhiteSpace(condition[12]))
                {
                    view.PolicyType = Convert.ToByte(condition[12]);
                }
                if (!string.IsNullOrWhiteSpace(condition[13]))
                {
                    view.Airline = condition[13];
                }
                if (!string.IsNullOrWhiteSpace(condition[14]))
                {
                    view.TicketType = (TicketType)int.Parse(condition[14]);
                }
                if (!string.IsNullOrWhiteSpace(condition[15]))
                {
                    view.CompanyId = Guid.Parse(condition[15]);
                }
                if (!string.IsNullOrWhiteSpace(condition[16]))
                {
                    view.OrderId = decimal.Parse(condition[16]);
                }
                if (!string.IsNullOrWhiteSpace(condition[17]))
                {
                    view.PayType = condition[17] == "1";
                }
                DataTable dt = ReportService.DownloadPurchaseFinancial(view);
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                DataRow dr = dt.NewRow();
                dr["TradeAmount"] = dt.Compute("Sum(TradeAmount)", "");
                dt.Rows.Add(dr);
                dt.Columns["Type"].ColumnName = "机票状态";
                dt.Columns["Id"].ColumnName = "订单号";
                dt.Columns["TripType"].ColumnName = "行程";
                dt.Columns["TicketType"].ColumnName = "客票类型";
                dt.Columns["Product"].ColumnName = "政策类型";
                dt.Columns["TicketNos"].ColumnName = "票号";
                dt.Columns["FlightNos"].ColumnName = "航班号";
                dt.Columns["VoyageNames"].ColumnName = "航程(中文)";
                dt.Columns["Voyages"].ColumnName = "航程(代码)";
                dt.Columns["PayTime"].ColumnName = "支付时间";
                dt.Columns["ETDZTime"].ColumnName = "出票时间";
                dt.Columns["TakeoffTimes"].ColumnName = "乘机时间";
                dt.Columns["RefundOrPostponeTime"].ColumnName = "退改签时间";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["Bunks"].ColumnName = "舱位";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["Fare"].ColumnName = "票面价";
                dt.Columns["ServiceCharge"].ColumnName = "服务费";
                dt.Columns["AirportFee"].ColumnName = "民航基金";
                dt.Columns["BAF"].ColumnName = "燃油附加费";
                dt.Columns["Rebate"].ColumnName = "返点";
                dt.Columns["PNR"].ColumnName = "订座PNR";
                dt.Columns["NewPNR"].ColumnName = "出票PNR";
                dt.Columns["Commission"].ColumnName = "佣金";
                dt.Columns["RefundOrPostponeFee"].ColumnName = "退改签手续费";
                dt.Columns["TradeAmount"].ColumnName = "实付款";
                dt.Columns["Success"].ColumnName = "账单状态";
                dt.Columns["OperatorAccount"].ColumnName = "操作员";
                dt.Columns["TradeAccount"].ColumnName = "付款账号";
                dt.Columns["Remark"].ColumnName = "备注";
                dt.Columns["PayType"].ColumnName = "支付方式";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("买入资金报表"));
            }
            #endregion

            #region 出票方卖出报表
            string providerTicketCondition = Request.QueryString["providerTicketCondition"];
            if (!string.IsNullOrWhiteSpace(providerTicketCondition))
            {
                string[] condtion = providerTicketCondition.Split(',');
                ProvideTicketView view = new ProvideTicketView();
                if (!string.IsNullOrWhiteSpace(condtion[0]))
                {
                    view.FinishBeginDate = DateTime.Parse(condtion[0]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[1]))
                {
                    view.FinishEndDate = DateTime.Parse(condtion[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condtion[2]))
                {
                    view.PayBeginDate = DateTime.Parse(condtion[2]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[3]))
                {
                    view.PayEndDate = DateTime.Parse(condtion[3]).AddDays(1).AddMilliseconds(-3);
                }
                //if (!string.IsNullOrWhiteSpace(condtion[4]))
                //{
                //    view.TakeoffBeginDate = DateTime.Parse(condtion[4]);
                //}
                //if (!string.IsNullOrWhiteSpace(condtion[5]))
                //{
                //    view.TakeoffEndDate = DateTime.Parse(condtion[5]).AddDays(1).AddMilliseconds(-3);
                //}
                if (!string.IsNullOrWhiteSpace(condtion[4]))
                {
                    view.TicketNo = condtion[4];
                }
                if (!string.IsNullOrWhiteSpace(condtion[5]))
                {
                    view.Passenger = condtion[5];
                }
                //if (!string.IsNullOrWhiteSpace(condtion[8]))
                //{
                //    view.PNR = condtion[8];
                //}
                if (!string.IsNullOrWhiteSpace(condtion[6]))
                {
                    view.Departure = condtion[6];
                }
                if (!string.IsNullOrWhiteSpace(condtion[7]))
                {
                    view.Arrival = condtion[7];
                }
                if (!string.IsNullOrWhiteSpace(condtion[8]))
                {
                    view.TicketState = (TicketState)int.Parse(condtion[8]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[9]))
                {
                    view.PolicyType = Convert.ToByte(condtion[9]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[10]))
                {
                    view.Airline = condtion[10];
                }
                if (!string.IsNullOrWhiteSpace(condtion[11]))
                {
                    view.TicketType = (TicketType)int.Parse(condtion[11]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[12]))
                {
                    view.OrderId = decimal.Parse(condtion[12]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[13]))
                {
                    view.CompanyId = Guid.Parse(condtion[13]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[14]))
                {
                    view.OfficeNo = condtion[14];
                }
                if (!string.IsNullOrWhiteSpace(condtion[15]))
                {
                    view.RelationType = (RelationType)int.Parse(condtion[15]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[16]))
                {
                    view.Purchase = Guid.Parse(condtion[16]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[17]))
                {
                    view.ProcessorAccount = condtion[17];
                }
                if (!string.IsNullOrWhiteSpace(condtion[18]) && condtion[18] != "undefined")
                {
                    view.SpecialProductType = (SpecialProductType)int.Parse(condtion[18]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[19]))
                {
                    view.TakeoffBeginDate = DateTime.Parse(condtion[19]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[20]))
                {
                    view.TakeoffEndDate = DateTime.Parse(condtion[20]).AddDays(1).AddMilliseconds(-3);
                }
                DataTable dt = ReportService.DownloadProvideTicket(view);
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                DataRow dr = dt.NewRow();
                dr["TradeAmount"] = dt.Compute("Sum(TradeAmount)", "");
                dt.Rows.Add(dr);
                dt.Columns["Type"].ColumnName = "机票状态";
                dt.Columns["Id"].ColumnName = "订单号";
                dt.Columns["TripType"].ColumnName = "行程";
                dt.Columns["TicketType"].ColumnName = "客票类型";
                dt.Columns["Product"].ColumnName = "政策类型";
                dt.Columns["SpecialProductType"].ColumnName = "特殊票类型";
                dt.Columns["Releation"].ColumnName = "销售关系";
                dt.Columns["PurchaserName"].ColumnName = "采购方";
                dt.Columns["TicketNos"].ColumnName = "票号";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["FlightNos"].ColumnName = "航班号";
                dt.Columns["VoyageNames"].ColumnName = "航程(中文)";
                dt.Columns["Voyages"].ColumnName = "航程(代码)";
                dt.Columns["PayTime"].ColumnName = "支付时间";
                dt.Columns["ETDZTime"].ColumnName = "出票时间";
                dt.Columns["TakeoffTimes"].ColumnName = "乘机时间";
                dt.Columns["RefundTime"].ColumnName = "退废时间";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["Bunks"].ColumnName = "舱位";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["Fare"].ColumnName = "票面价";
                dt.Columns["ServiceCharge"].ColumnName = "服务费";
                dt.Columns["PNR"].ColumnName = "订座PNR";
                dt.Columns["NewPNR"].ColumnName = "出票PNR";
                dt.Columns["AirportFee"].ColumnName = "民航基金";
                dt.Columns["BAF"].ColumnName = "燃油附加费";
                dt.Columns["Rebate"].ColumnName = "返点";
                dt.Columns["Commission"].ColumnName = "佣金";
                dt.Columns["RefundFee"].ColumnName = "退改签手续费";
                dt.Columns["Anticipation"].ColumnName = "应收款";
                dt.Columns["TradeFee"].ColumnName = "支付手续费";
                dt.Columns["TradeAmount"].ColumnName = "实收款";
                dt.Columns["Success"].ColumnName = "账单状态";
                dt.Columns["Processor"].ColumnName = "操作员";
                dt.Columns["OfficeNo"].ColumnName = "OFFICE号";
                dt.Columns["TradeAccount"].ColumnName = "收款账号";
                dt.Columns["Remark"].ColumnName = "备注";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("卖出机票明细"));
            }
            #endregion

            #region 出票方资金报表
            string providerFinancialCondition = Request.QueryString["providerFinancialCondition"];
            if (!string.IsNullOrWhiteSpace(providerFinancialCondition))
            {
                string[] condtion = providerFinancialCondition.Split(',');
                ProvideTicketView view = new ProvideTicketView();
                if (!string.IsNullOrWhiteSpace(condtion[0]))
                {
                    view.FinishBeginDate = DateTime.Parse(condtion[0]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[1]))
                {
                    view.FinishEndDate = DateTime.Parse(condtion[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condtion[2]))
                {
                    view.PayBeginDate = DateTime.Parse(condtion[2]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[3]))
                {
                    view.PayEndDate = DateTime.Parse(condtion[3]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condtion[4]))
                {
                    view.TicketNo = condtion[4];
                }
                if (!string.IsNullOrWhiteSpace(condtion[5]))
                {
                    view.Passenger = condtion[5];
                }
                if (!string.IsNullOrWhiteSpace(condtion[6]))
                {
                    view.Departure = condtion[6];
                }
                if (!string.IsNullOrWhiteSpace(condtion[7]))
                {
                    view.Arrival = condtion[7];
                }
                if (!string.IsNullOrWhiteSpace(condtion[8]))
                {
                    view.TicketState = (TicketState)int.Parse(condtion[8]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[9]))
                {
                    view.PolicyType = Convert.ToByte(condtion[9]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[10]))
                {
                    view.Airline = condtion[10];
                }
                if (!string.IsNullOrWhiteSpace(condtion[11]))
                {
                    view.TicketType = (TicketType)int.Parse(condtion[11]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[12]))
                {
                    view.OrderId = decimal.Parse(condtion[12]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[13]))
                {
                    view.CompanyId = Guid.Parse(condtion[13]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[14]))
                {
                    view.OfficeNo = condtion[14];
                }
                if (!string.IsNullOrWhiteSpace(condtion[15]))
                {
                    view.RelationType = (RelationType)int.Parse(condtion[15]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[16]))
                {
                    view.Purchase = Guid.Parse(condtion[16]);
                }
                if (!string.IsNullOrWhiteSpace(condtion[17]))
                {
                    view.ProcessorAccount = condtion[17];
                }
                if (!string.IsNullOrWhiteSpace(condtion[18]) && condtion[18] != "undefined")
                {
                    view.SpecialProductType = (SpecialProductType)int.Parse(condtion[18]);
                }
                DataTable dt = ReportService.DownloadProvideFinancial(view);
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                DataRow dr = dt.NewRow();
                dr["TradeAmount"] = dt.Compute("Sum(TradeAmount)", "");
                dt.Rows.Add(dr);
                dt.Columns["Type"].ColumnName = "机票状态";
                dt.Columns["Id"].ColumnName = "订单号";
                dt.Columns["TripType"].ColumnName = "行程";
                dt.Columns["TicketType"].ColumnName = "客票类型";
                dt.Columns["Product"].ColumnName = "政策类型";
                dt.Columns["SpecialProductType"].ColumnName = "特殊票类型";
                dt.Columns["Releation"].ColumnName = "销售关系";
                dt.Columns["PurchaserName"].ColumnName = "采购方";
                dt.Columns["TicketNos"].ColumnName = "票号";
                dt.Columns["SettleCode"].ColumnName = "结算码";
                dt.Columns["FlightNos"].ColumnName = "航班号";
                dt.Columns["VoyageNames"].ColumnName = "航程(中文)";
                dt.Columns["Voyages"].ColumnName = "航程(代码)";
                dt.Columns["PayTime"].ColumnName = "支付时间";
                dt.Columns["ETDZTime"].ColumnName = "出票时间";
                dt.Columns["TakeoffTimes"].ColumnName = "乘机时间";
                dt.Columns["RefundTime"].ColumnName = "退废时间";
                dt.Columns["CarrierName"].ColumnName = "航空公司";
                dt.Columns["Bunks"].ColumnName = "舱位";
                dt.Columns["Passenger"].ColumnName = "乘机人";
                dt.Columns["Fare"].ColumnName = "票面价";
                dt.Columns["ServiceCharge"].ColumnName = "服务费";
                dt.Columns["PNR"].ColumnName = "订座PNR";
                dt.Columns["NewPNR"].ColumnName = "出票PNR";
                dt.Columns["AirportFee"].ColumnName = "民航基金";
                dt.Columns["BAF"].ColumnName = "燃油附加费";
                dt.Columns["Rebate"].ColumnName = "返点";
                dt.Columns["Commission"].ColumnName = "佣金";
                dt.Columns["RefundFee"].ColumnName = "退改签手续费";
                dt.Columns["Anticipation"].ColumnName = "应收款";
                dt.Columns["TradeFee"].ColumnName = "支付手续费";
                dt.Columns["TradeAmount"].ColumnName = "实收款";
                dt.Columns["Success"].ColumnName = "账单状态";
                dt.Columns["Processor"].ColumnName = "操作员";
                dt.Columns["OfficeNo"].ColumnName = "OFFICE号";
                dt.Columns["TradeAccount"].ColumnName = "收款账号";
                dt.Columns["Remark"].ColumnName = "备注";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("卖出资金报表"));
            }
            #endregion

            #region 经纪人报表
            string spreaderCondition = Request.QueryString["spreaderCondition"];
            if (!string.IsNullOrWhiteSpace(spreaderCondition))
            {
                string[] condition = spreaderCondition.Split(',');
                SpreadTicketView view = new SpreadTicketView();
                if (!string.IsNullOrWhiteSpace(condition[0]))
                {
                    view.BeginFinishTime = DateTime.Parse(condition[0]);
                }
                if (!string.IsNullOrWhiteSpace(condition[1]))
                {
                    view.EndFinishTime = DateTime.Parse(condition[1]).AddDays(1).AddMilliseconds(-3);
                }
                if (!string.IsNullOrWhiteSpace(condition[2]))
                {
                    view.BargainType = (CompanyType)int.Parse(condition[2]);
                }
                if (!string.IsNullOrWhiteSpace(condition[3]) && condition[3] != "undefined")
                {
                    view.Bargainer = Guid.Parse(condition[3]);
                }
                if (!string.IsNullOrWhiteSpace(condition[4]))
                {
                    view.TicketState = (TicketState)int.Parse(condition[4]);
                }
                if (!string.IsNullOrWhiteSpace(condition[6]))
                {
                    view.Spreader = Guid.Parse(condition[6]);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(condition[5]) && condition[5] != "undefined")
                    {
                        view.Spreader = Guid.Parse(condition[5]);
                    }
                }
                DataTable dt = ReportService.DownloadSpreadTicket(view);
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }
                DataRow dr = dt.NewRow();
                dr["TradeAmount"] = dt.Compute("Sum(TradeAmount)", "");
                dr["Amount"] = dt.Compute("Sum(Amount)", "");
                dt.Rows.Add(dr);
                if (!string.IsNullOrWhiteSpace(condition[7]))
                {
                    dt.Columns.Remove(dt.Columns["SpreaderName"]);
                    dt.Columns.Remove(dt.Columns["SpreaderUserName"]);
                }
                else
                {
                    dt.Columns["SpreaderName"].ColumnName = "推广方";
                    dt.Columns["SpreaderUserName"].ColumnName = "推广方用户名";
                }
                dt.Columns["BargainerType"].ColumnName = "交易角色";
                dt.Columns["BargainerName"].ColumnName = "交易方";
                dt.Columns["Id"].ColumnName = "订单号";
                dt.Columns["FlightNo"].ColumnName = "航班号";
                dt.Columns["Voyage"].ColumnName = "航程(中文)";
                dt.Columns["Bunk"].ColumnName = "舱位";
                dt.Columns["PassengerCount"].ColumnName = "人数";
                dt.Columns["Type"].ColumnName = "机票状态";
                dt.Columns["FinishTime"].ColumnName = "时间";
                dt.Columns["TradeAmount"].ColumnName = " 交易金额";
                dt.Columns["Amount"].ColumnName = "后返金额";
                dt.Columns["PayType"].ColumnName = "支付方式";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("经纪人报表"));
            }
            #endregion

            #region 用户推广明细报表
            string platformSpreadStatisticCondition = Request.QueryString["platformSpreadStatisticCondition"];
            if (!string.IsNullOrWhiteSpace(platformSpreadStatisticCondition))
            {
                string[] condition = platformSpreadStatisticCondition.Split(',');
                DateTime startTime = DateTime.Parse(condition[0]);
                DateTime endTime = DateTime.Parse(condition[1]);
                CompanyType? type = condition[2] == "0" ? null : (CompanyType?)int.Parse(condition[2]);
                string employeeNo = condition[3];
                decimal totalPurchaseAmount, totalSupplyAmount, totalProvideAmount;
                int totalPurchaseCount, totalSupplyCount, totalProvideCount;

                DataTable dt = ReportService.DownloadEmployeeSpreadStatisticReport(startTime, endTime, type, employeeNo, out  totalPurchaseAmount, out  totalSupplyAmount, out  totalProvideAmount, out  totalPurchaseCount, out  totalSupplyCount, out  totalProvideCount);
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    dt.Columns[i].AllowDBNull = true;
                }

                DataRow dr = dt.NewRow();


                dr["UserName"] = "采购方:" + totalPurchaseCount + "个(交易总额" + totalPurchaseAmount + "元);" + "产品方:" + totalSupplyCount + "个(交易总额" + totalSupplyAmount + "元);" + "出票方:" + totalProvideCount + "个(交易总额" + totalProvideAmount + "元);";
                dt.Rows.Add(dr);
                if (!string.IsNullOrWhiteSpace(employeeNo))
                {
                    dt.Columns.Remove(dt.Columns["EmployeeUserName"]);
                }
                else
                {
                    dt.Columns["EmployeeUserName"].ColumnName = "员工账号";
                }
                dt.Columns["UserName"].ColumnName = "账号";
                dt.Columns["Name"].ColumnName = "简称";
                dt.Columns["CompanyType"].ColumnName = "公司类型";
                dt.Columns["RegisterTime"].ColumnName = "注册时间";
                dt.Columns["PurchaseAmount"].ColumnName = "采购总额";
                dt.Columns["SupplyAmount"].ColumnName = "出票总额";
                dt.Columns["ProvideAmount"].ColumnName = "产品总额";
                NPOIExcelHelper.ExportDataTableToExcelAutoSheetName(dt, downloadFileName("用户推广明细报表"));
            #endregion

            }
        }

        private string downloadFileName(string fileName)
        {
            string downloadFileName = fileName;
            string UserAgent = Request.ServerVariables["http_user_agent"].ToLower();
            if (UserAgent.IndexOf("firefox") == -1)
            {//非火狐浏览器
                downloadFileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
            }
            return downloadFileName;
        }
    }
}