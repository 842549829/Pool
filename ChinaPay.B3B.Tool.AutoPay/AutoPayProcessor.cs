using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoolPay.DataTransferObject;
using ChinaPay.B3B.Service.Organization.Domain;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.Core;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.Service;
using System.Timers;
using ChinaPay.B3B.Service.Order;
using ChinaPay.B3B.Service.Order.Domain.Applyform;
using ChinaPay.B3B.Service.Tradement;

namespace ChinaPay.B3B.Tool.AutoPay
{
    class AutoPayProcessor
    {
        internal const string TicketOrderPayType = "Order";
        internal const string PostponeApplyformPayType = "Postpone";


        private static AutoPayProcessor _instance = null;
        private static object _locker = new object();
        public static AutoPayProcessor Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new AutoPayProcessor();
                        }
                    }
                }
                return _instance;
            }
        }

        private Timer _timer = null;
        private volatile bool _run = false;

        private AutoPayProcessor()
        {
        }

        public void Start()
        {
            if (_timer == null)
            {
                _run = true;
                _timer = new Timer()
                {
                    Interval = 60 * 1000
                };
                _timer.Elapsed += (sender, e) => process();
                _timer.Start();
                showMessage("开始处理");
                process();
            }
        }
        private void process()
        {
            showMessage("开始下一批处理");
            var noPorcess = AutoPayService.QueryNoPorcess();
            //var str = Environment.CurrentDirectory;
            string msg = "";
            foreach (var item in noPorcess)
            {
                if (!_run) break;
                AccountTradeDTO tradeView = null;
                //处理订单
                if (item.OrderType == OrderType.Order)
                {
                    OrderProcessService.Payable(item.OrderId, out msg);
                    if (string.IsNullOrEmpty(msg))
                    {
                        Order order = OrderQueryService.QueryOrder(item.OrderId);
                        tradeView = getPayTradeView(order, getPayAccountNo(item.PayType, order.Purchaser.Amount, order.Purchaser.CompanyId), "");
                    }
                }
                //处理申请单
                else if (item.OrderType == OrderType.Postpone)
                {
                    ApplyformProcessService.Payable(item.OrderId, out msg);
                    if (string.IsNullOrEmpty(msg))
                    {
                        PostponeApplyform applyform = ApplyformQueryService.QueryPostponeApplyform(item.OrderId);
                        if (applyform.PayBill.Tradement == null) msg = "申请单：" + item.OrderId + " 不能进行代扣,无支付信息!";
                        tradeView = getPayTradeView(applyform, getPayAccountNo(item.PayType, Math.Abs(applyform.PayBill.Applier.Amount), applyform.PurchaserId), "");
                    }
                }
                if (item.PayType == WithholdingAccountType.Alipay)
                {
                    tradeView.BuyerEmail = item.PayAccountNo;
                }
                try
                {
                    if (string.IsNullOrEmpty(msg))
                    {
                        global::PoolPay.DomainModel.Trade.PayTrade pay = AutoPayService.AutoPay(tradeView, item.PayType);
                        if (item.PayType == WithholdingAccountType.Poolpay)
                        {
                            //如果是国付通代扣就直接修改订单状态
                            if (pay != null && pay.Status == global::PoolPay.DataTransferObject.PayStatus.PaySuccess)
                            {
                                NotifyService.PaySuccess(item.OrderId, pay.CustomParameter, pay.Id.ToString(), pay.FillChargeId.ToString(), pay.PayDate.Value, "", (pay.BuyerAccount.Character as global::PoolPay.DomainModel.Accounts.CreditAccount) == null ? "0" : "1", pay.BuyerAccount.AccountNo);
                            }
                        }
                        showMessage("处理成功");
                    }
                    else
                    {
                        showMessage("处理失败" + Environment.NewLine + "原因:" + msg);
                    }
                }
                catch (Exception ex)
                {
                    showMessage("处理失败" + Environment.NewLine + "原因:" + ex.Message);
                }
                AutoPayService.UpdateProcess(item.OrderId);
                System.Threading.Thread.Sleep(50);
            }
            showMessage("当前批次处理结束，共处理 " + noPorcess.Count + " 条");
        }
        public void Stop()
        {
            _run = false;
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Close();
                _timer = null;
                showMessage("停止处理");
            }
        }
        private void showMessage(string message)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 代扣处理\n\t" + message);
        }
        private static string getPayAccountNo(WithholdingAccountType type, decimal amount, Guid purchaser)
        {
            var auto = AccountService.GetWithholding(type, purchaser);
            if (auto == null || auto.Status == WithholdingProtocolStatus.Submitted)
            {
                throw new CustomException("缺少代扣账号，不能支付！");
            }
            else if (auto.Amount < amount)
            {
                throw new CustomException("代扣金额超过上限，代扣失败！");
            }
            return auto.AccountNo;
        }

        private static AccountTradeDTO getPayTradeView(Order order, string payAccount, string operatorAccount)
        {
            return new AccountTradeDTO
            {
                OutOrderId = order.Id.ToString(),
                TradeAmount = order.Purchaser.Amount,
                IncomeAccountNo = order.Bill.PayBill.Tradement.PayeeAccount,
                PaymentAccountNo = payAccount,
                Subject = "代扣支付机票款",
                Note = order.ReservationPNR == null ? string.Empty : (order.ReservationPNR.PNR ?? order.ReservationPNR.BPNR),
                IsProtocol = true,
                CustomParameter = TicketOrderPayType + "|" + payAccount + "|" + operatorAccount
            };
        }
        private static AccountTradeDTO getPayTradeView(PostponeApplyform applyform, string payAccount, string operatorAccount)
        {
            return new AccountTradeDTO
            {
                OutOrderId = applyform.Id.ToString(),
                TradeAmount = Math.Abs(applyform.PayBill.Applier.Amount),
                IncomeAccountNo = applyform.PayBill.Tradement.PayeeAccount,
                PaymentAccountNo = payAccount,
                Subject = "代扣支付改期费",
                IsProtocol = true,
                Note = applyform.OriginalPNR == null ? string.Empty : (applyform.OriginalPNR.PNR ?? applyform.OriginalPNR.BPNR),
                CustomParameter = PostponeApplyformPayType + "|" + payAccount + "|" + operatorAccount + "|"
            };
        }

    }
}
