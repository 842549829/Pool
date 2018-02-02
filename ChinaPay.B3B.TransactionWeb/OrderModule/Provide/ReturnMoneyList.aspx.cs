using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Common;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.Service;
using ChinaPay.B3B.Service.Locker;
using ChinaPay.B3B.Service.Organization;
using ChinaPay.B3B.TransactionWeb.UserControl;
using ChinaPay.Core;
using ChinaPay.Core.Extension;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Provide {
    public partial class ReturnMoneyList : BasePage {

        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                txtAppliedDateStart.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                txtAppliedDateEnd.Text = DateTime.Today.ToString("yyyy-MM-dd");
                initData();
             btnQuery_Click(this, e);
           }
            pager.CurrentPageChanged += pager_CurrentPageChanged;
        }

        private void pager_CurrentPageChanged(Pager sender, int newPage) {
            var pagination = new Pagination {
                PageSize = pager.PageSize,
                PageIndex = newPage,
                GetRowCount = true
            };
            QueryApplyForm(pagination);
        }


        private void QueryApplyForm(Pagination pagination) {
            try {
                List<ApplyformListView> forms = ApplyformQueryService.ProviderQueryApplyformsForReturnMoney(getCondition(), pagination).ToList();
                var lockInfos = LockService.Query(forms.Select(form => form.ApplyformId.ToString())).ToList();
                dataList.DataSource = forms.Select(form => {
                    LockInfo lockInfo = lockInfos.FirstOrDefault(l=>l.Key==form.ApplyformId.ToString());
                                                           return new
                                                               {
                                                                   form.ApplyformId,
                                                                   PNR = form.OriginalPNR.ToListString(),
                                                                   AirportPair = form.Flights.Join("<br />",
                                                                       f =>
                                                                       string.Format(
                                                                           "{0}-{1}",
                                                                           f.DepartureCity,
                                                                           f.ArrivalCity)),
                                                                   FlightInfo = form.Flights.Join("<br />",
                                                                       f => string.Format(
                                                                           "{0}{1}<br />{2} / {3}",
                                                                           f.Carrier,
                                                                           f.FlightNo,
                                                                           string.IsNullOrEmpty(f.Bunk) ? "-" : f.Bunk,
                                                                           f.Discount.HasValue ? (f.Discount.Value * 100).TrimInvaidZero() : string.Empty)),
                                                                   TakeoffTime = form.Flights.Join("<br />",
                                                                       f =>
                                                                       f.TakeoffTime.ToString("yyyy-MM-dd<br />HH:mm")),
                                                                   Passengers = string.Join("<br />", form.Passengers),
                                                                   ApplyType = form.ApplyformType.GetDescription(),
                                                                   ProcessStatus = form.ApplyformType == ApplyformType.Postpone ?
                                                         Service.Order.StatusService.GetPostponeApplyformStatus((PostponeApplyformStatus)form.ApplyDetailStatus,
                                                                this.CurrentCompany.CompanyType == CompanyType.Provider ? OrderRole.Provider : OrderRole.Supplier) :
                                                         Service.Order.StatusService.GetRefundApplyformStatus((RefundApplyformStatus)form.ApplyDetailStatus,
                                                                 this.CurrentCompany.CompanyType == CompanyType.Provider ? OrderRole.Provider : OrderRole.Supplier),
                                                                   AppliedTime =
                                                                       form.AppliedTime.ToString("yyyy-MM-dd<br />HH:mm"),
                                                                   form.ApplierAccount,
                                                                   ProductType = form.ProductType.GetDescription(),
                                                                   form.ApplyformType,
                                                                   LockInfo = lockInfo == null
                                                                                  ? string.Empty
                                                                                  : lockInfo.Company ==
                                                                                    CurrentCompany.CompanyId
                                                                                        ? string.Format("{0}<br />{1}", lockInfo.Account, lockInfo.Name)
                                                                                        : string.Format("{0}<br />({1})", lockInfo.LockRole.GetDescription(), lockInfo.Account)
                                                               };
                                                       });
                dataList.DataBind();
                if(forms.Any()) {
                    pager.Visible = true;
                    if(pagination.GetRowCount) {
                        pager.RowCount = pagination.RowCount;
                    }
                } else {
                    pager.Visible = false;
                }
            } catch(Exception ex) {
                ShowExceptionMessage(ex, "查询");
            }
        }

        private ApplyformQueryCondition getCondition() {
            var parameter = new ApplyformQueryCondition {
                PNR = txtPNR.Text.Trim(),
                Passenger = txtPassenger.Text.Trim(),
                AppliedDateRange =
                    new Range<DateTime>(DateTime.Parse(txtAppliedDateStart.Text.Trim()),
                        DateTime.Parse(txtAppliedDateEnd.Text.Trim()).AddDays(1).AddTicks(-1)),
            };
            if(!String.IsNullOrEmpty(txtApplyformId.Text.Trim()) && Regex.IsMatch(txtApplyformId.Text.Trim(), "\\d+")) {
                parameter.ApplyformId = decimal.Parse(txtApplyformId.Text.Trim());
            }
            if(!string.IsNullOrEmpty(ddlProductType.SelectedValue)) {
                parameter.ProductType = (ProductType)int.Parse(ddlProductType.SelectedValue);
            }
            parameter.Provider = CurrentCompany.CompanyId;
            return parameter;
        }

        private void initData() {
            var productTypes = Enum.GetValues(typeof(ProductType));
            foreach(ProductType type in productTypes) {
                ddlProductType.Items.Add(new ListItem(type.GetDescription(), ((byte)type).ToString(CultureInfo.InvariantCulture)));
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e) {
            if(pager.CurrentPageIndex == 1) {
                var pagination = new Pagination {
                    PageSize = pager.PageSize,
                    PageIndex = 1,
                    GetRowCount = true
                };
                QueryApplyForm(pagination);
            } else {
                pager.CurrentPageIndex = 1;
            }
        }
    }
}