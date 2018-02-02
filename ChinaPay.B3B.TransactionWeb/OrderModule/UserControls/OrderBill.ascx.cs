using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaPay.B3B.Service.Distribution.Domain.Bill;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Pay;
using ChinaPay.B3B.Service.Distribution.Domain.Bill.Refund;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.UserControls {
    public partial class OrderBill : System.Web.UI.UserControl {
        private readonly List<BillBase> _bills = new List<BillBase>();

        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                bindingBills();
            }
        }

        public void InitData(Service.Distribution.Domain.OrderBill bill) {
            _bills.Add(bill.PayBill);
            _bills.AddRange(bill.NormalRefundBills);
            _bills.AddRange(bill.PostponePayBills);
            _bills.AddRange(bill.PostponeRefundBills);
        }
        public void InitData(NormalRefundBill refundBill) {
            if(refundBill != null) {
                _bills.Add(refundBill);
            }
        }
        public void InitData(PostponePayBill payBill) {
            if(payBill != null) {
                _bills.Add(payBill);
                if(payBill.RefundBill != null) {
                    _bills.Add(payBill.RefundBill);
                }
            }
        }
        private void bindingBills() {
            if(_bills.Any()) {
                var billsInfo = "<table cellspacing=\"0\">" + drawTitle();
                billsInfo += isPlatform() ? drawContentsForPlatform() : drawContentsForUser();
                billsInfo += "</table>";
                this.divBills.InnerHtml = billsInfo;
            }
        }
        private string drawTitle() {
            var title = new StringBuilder("<tr>");
            title.Append("<th>类型</th>");
            title.Append("<th>订单/申请单号</th>");
            title.Append("<th>金额</th>");
            title.Append("<th>交易账号</th>");
            if(isPlatform()) {
                title.Append("<th>单位信息</th>");
                title.Append("<th>状态</th>");
            }
            title.Append("<th>交易流水号</th>");
            title.Append("<th>时间</th>");
            title.Append("</tr>");
            return title.ToString();
        }
        private string drawContentsForUser() {
            var contents = new StringBuilder();
            var userBills = from bill in _bills
                            let roleBills = getUserBills(bill)
                            where roleBills.Any()
                            from roleBill in roleBills
                            orderby roleBill.Role.Time
                            select roleBill;
            foreach(var bill in userBills) {
                if(bill != null) {
                    contents.Append("<tr>");
                    contents.AppendFormat("<td>{0}({1})</td>", bill.BillType, bill.Remark);
                    contents.AppendFormat("<td>{0}</td>", bill.Id);
                    contents.AppendFormat("<td><span class=\"price\">{0}</span></td>", bill.Role.Amount.TrimInvaidZero());
                    contents.AppendFormat("<td>{0}</td>", bill.Role.Owner.Account);
                    contents.AppendFormat("<td>{0}</td>", bill.TradeNo);
                    contents.AppendFormat("<td>{0}</td>", bill.Role.Time.HasValue ? bill.Role.Time.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
                    contents.Append("</tr>");
                }
            }
            return contents.ToString();
        }
        private string drawContentsForPlatform() {
            var contents = new StringBuilder();
            var bills = _bills.Select(getPlatformBill).OrderBy(b => b.TradeTime);
            foreach(var bill in bills) {
                var index = 0;
                foreach(var role in bill.Roles) {
                    contents.Append("<tr>");
                    if(index == 0) {
                        contents.AppendFormat("<td rowspan='{0}'>{1}({2})</td>", bill.Roles.Count, bill.BillType, bill.Remark);
                        contents.AppendFormat("<td rowspan='{0}'>{1}</td>", bill.Roles.Count, bill.Id);
                    }
                    contents.AppendFormat("<td><span class=\"price\">{0}</span></td>", role.Amount.TrimInvaidZero());
                    contents.AppendFormat("<td>{0}</td>", role.Account);
                    if(role.Owner == Guid.Empty) {
                        contents.AppendFormat("<td>{0}</td>", role.RoleName);
                    } else {
                        var companyUrl = "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + role.Owner.ToString();
                        contents.AppendFormat("<td><a href='{0}' class='obvious-a'>{1}</a></td>", companyUrl, role.RoleName);
                    }
                    contents.AppendFormat("<td>{0}</td>", role.Status.HasValue ? role.Status.Value ? "成功" : "失败" : "");
                    if(index == 0) {
                        contents.AppendFormat("<td rowspan='{0}'>{1}</td>", bill.Roles.Count, bill.TradeNo);
                        contents.AppendFormat("<td rowspan='{0}'>{1}</td>", bill.Roles.Count, bill.TradeTime.HasValue ? bill.TradeTime.Value.ToString("yyyy-MM-dd<br />HH:mm:ss") : string.Empty);
                    }
                    contents.Append("</tr>");
                    index++;
                }
            }
            return contents.ToString();
        }
        private bool isPlatform() {
            return BasePage.LogonCompany.CompanyType == Common.Enums.CompanyType.Platform;
        }
        private PlatformBill getPlatformBill(BillBase bill) {
            var result = new PlatformBill {
                BillType = getTradementType(bill.TradementBase),
                Remark = bill.Remark,
                Id = bill.Id,
                TradeNo = bill.TradementBase.TradeNo,
                TradeTime = bill.TradeTime,
                Roles = new List<BillRole>()
            };
            foreach(var roleBill in bill.RoleBills) {
                result.Roles.Add(getBillRole(roleBill));
            }
            if(bill.PlatformBasicProfit != null) {
                result.Roles.Add(new BillRole {
                    Amount = bill.PlatformBasicProfit.TradeFee,
                    RoleName = "平台手续费",
                    Owner = Guid.Empty,
                    Status = bill.PlatformBasicProfit.Success,
                    Account = bill.PlatformBasicProfit.Account
                });
                if(bill.PlatformBasicProfit.Premium != 0) {
                    result.Roles.Add(new BillRole {
                        Amount = bill.PlatformBasicProfit.Premium,
                        RoleName = "溢价",
                        Owner = Guid.Empty,
                        Status = bill.PlatformBasicProfit.Success,
                        Account = bill.PlatformBasicProfit.Account
                    });
                }
            }
            result.Roles.Add(new BillRole {
                Amount = bill.TradementBase.TradeFee,
                RoleName = "通道手续费",
                Owner = Guid.Empty,
                Status = null,
                Account = bill.TradementBase.PayeeAccount
            });
            return result;
        }
        private IEnumerable<UserBill> getUserBills(BillBase bill) {
            var currentCompany = BasePage.LogonCompany.CompanyId;
            return from roleBill in bill.RoleBills
                   where roleBill.Owner.Id == currentCompany
                   select new UserBill {
                       BillType = getTradementType(bill.TradementBase),
                       Remark = bill.Remark,
                       Id = bill.Id,
                       Role = roleBill,
                       TradeNo = bill.TradementBase.TradeNo
                   };
        }
        private string getTradementType(Service.Distribution.Domain.Tradement.Tradement tradement) {
            return tradement is Service.Distribution.Domain.Tradement.Payment ? "支付" : "退款";
        }
        private BillRole getBillRole(RoleBill roleBill) {
            return new BillRole {
                Amount = roleBill.Amount,
                Account = roleBill.Owner.Account,
                Owner = roleBill.Owner.Id,
                RoleName = roleBill.Owner.RoleType.GetDescription(),
                Status = roleBill.Success
            };
        }
        class UserBill {
            public string BillType { get; set; }
            public string Remark { get; set; }
            public decimal Id { get; set; }
            public RoleBill Role { get; set; }
            public string TradeNo { get; set; }
        }
        class PlatformBill {
            public string BillType { get; set; }
            public string Remark { get; set; }
            public decimal Id { get; set; }
            public string TradeNo { get; set; }
            public DateTime? TradeTime { get; set; }
            public List<BillRole> Roles { get; set; }
        }
        class BillRole {
            public decimal Amount { get; set; }
            public string Account { get; set; }
            public Guid Owner { get; set; }
            public string RoleName { get; set; }
            public bool? Status { get; set; }
        }
    }
}