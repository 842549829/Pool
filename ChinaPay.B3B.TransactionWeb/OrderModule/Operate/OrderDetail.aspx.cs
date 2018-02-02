using System;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using ChinaPay.B3B.Common.Enums;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.B3B.DataTransferObject.Organization;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate {
    public partial class OrderDetail : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            if (!IsPostBack)
            {
                setBackButton();
                var id = Request.QueryString["id"];
                decimal orderId;
                if(decimal.TryParse(id, out orderId)) {
                    var order = Service.OrderQueryService.QueryOrder(orderId);
                    if(order == null) {
                        showErrorMessage("订单不存在");
                    } else {
                        bindOrder(order);
                        setButtons(order);
                    }
                } else {
                    showErrorMessage("参数错误");
                }
            }
        }
        private void bindOrder(Service.Order.Domain.Order order) {
            bindOrderHeader(order);
            bindPNRGroups(order);
            bindOrderBills(order);
            if (order.IsOEMOrder)
            {
                bindOemInfo(order.OemInfo);
            }
            else
            {
                divOemInfo.Visible = false;    
            }
        }
        private void bindOemInfo(Service.Organization.Domain.OEMInfo oemInfo)
        {
            const string _timeRegex = "yyyy-MM-dd HH:mm:ss";

            lblOemCompanyUserName.Text = oemInfo.Company.UserName;
            lblOemCompanyTypeValue.Text = string.Format("{0}({1})",
                oemInfo.Company.CompanyType.GetDescription(),
                oemInfo.Company.AccountType.GetDescription());
            lblOemAuthorizationTime.Text = oemInfo.RegisterTime.ToString(_timeRegex);

            lblOemCompayName.Text = oemInfo.Company.CompanyName;
            lblOemCompanyAbbreviation.Text = oemInfo.Company.AbbreviateName;
            lblOemName.Text = oemInfo.SiteName;

            lblOemCompayPhone.Text = oemInfo.Company.OfficePhones;
            lblOemCompanyOrginationCode.Text = oemInfo.Company.OrginationCode;
            lblOemDomainName.Text = oemInfo.DomainName;

            lblOemCompanyManagerName.Text = oemInfo.Company.ManagerName;
            lblOemCompanyManagerCellPhone.Text = oemInfo.Company.ManagerCellphone;
            lblOemValid.Text = oemInfo.Valid ? "正常" : "失效";

            lblOemCompanyLocation.Text = AddressShow.GetAddressText(oemInfo.Company.Area, oemInfo.Company.Province, oemInfo.Company.City, oemInfo.Company.District);
            lblOemCompanyAddress.Text = oemInfo.Company.Address;
            lblOemEffectTime.Text = oemInfo.EffectTime.Value.ToString(_timeRegex);

            string postBackUrl = "/OrganizationModule/TerraceModule/CompanyInfoManage/LicenseQuery.aspx?type={0}&companyId={1}";
            lbtnBussinessLicense.HRef = string.Format(postBackUrl, "bussiness", oemInfo.CompanyId);
            lbtnIATA.HRef = string.Format(postBackUrl, "iata", oemInfo.CompanyId);
            lbtncertNo.HRef = string.Format(postBackUrl, "certNo", oemInfo.CompanyId);
            switch (oemInfo.Company.CompanyType)
            {
                case ChinaPay.B3B.Common.Enums.CompanyType.Provider:
                    break;
                case ChinaPay.B3B.Common.Enums.CompanyType.Purchaser:
                    lbtnBussinessLicense.Visible = lbtnIATA.Visible = false;
                    lbtncertNo.Visible = true;
                    break;
                case ChinaPay.B3B.Common.Enums.CompanyType.Supplier:
                    lbtnBussinessLicense.Visible = oemInfo.Company.AccountType == Common.Enums.AccountBaseType.Enterprise;
                    lbtncertNo.Visible = oemInfo.Company.AccountType == Common.Enums.AccountBaseType.Individual;
                    break;
            }

            lblOemEffectTimeStrat.Text = oemInfo.RegisterTime.ToString(_timeRegex);
            lblOemEffectTimeStrat.Text = oemInfo.EffectTime.Value.ToString(_timeRegex);

            CompanyDetailInfo companyDetailInfo = oemInfo.Company as CompanyDetailInfo;
            lblOemCompanyRegisterTime.Text = companyDetailInfo.RegisterTime.ToString(_timeRegex);
            lblOemCompanyAuditTime.Text = companyDetailInfo.AuditTime.HasValue ? companyDetailInfo.AuditTime.Value.ToString(_timeRegex) : string.Empty;
        }
        private void bindOrderHeader(Service.Order.Domain.Order order) {
            this.hfdPnrImport.Value = (order.Source == OrderSource.CodeImport || order.Source == OrderSource.ContentImport || order.Source == OrderSource.InterfaceOrder)?"true":"false";
            this.hfdAgreeAuth.Value = order.Choise != AuthenticationChoise.NoAUTHandArgee ? "true" : "false";
            this.lblOrderId.Text = order.Id.ToString();
            this.lblStatus.Text = Service.Order.StatusService.GetOrderStatus(order.Status, DataTransferObject.Order.OrderRole.Platform);
            this.lblAmount.Text = order.Purchaser.Amount.ToString("F2");
            this.lbRemark.Visible = order.ForbidChangPNR;
            this.cbForbidChangePNR.Checked = order.ForbidChangPNR;
            var product = order.IsThirdRelation?order.Supplier.Product:order.Provider.Product;
            if (product is SpeicalProductInfo) {
                var specialProductInfo = product as SpeicalProductInfo;
                this.lblProductType.Text = order.Product.ProductType.GetDescription()+"（"+specialProductInfo.SpeicalProductType.GetDescription()+"）";
                OriginalPolicyIsSpecial.Value = "1";
            }  else  {
                this.lblProductType.Text = order.Product.ProductType.GetDescription();
            }
            if(order.Provider != null && order.Provider.Product is Service.Order.Domain.CommonProductInfo) {
                this.lblTicketType.Text = (order.Provider.Product as Service.Order.Domain.CommonProductInfo).TicketType.ToString();
            } else {
                this.lblTicketType.Text = "-";
            }
            if (order.Provider != null)
            {
                hidOriginalPolicyOwner.Value = order.Provider.CompanyId.ToString();
            }
            this.lblOriginalOrderId.Text = order.AssociateOrderId.HasValue ? order.AssociateOrderId.Value.ToString() : "-";
            this.lblProducedTime.Text = order.Purchaser.ProducedTime.ToString("yyyy-MM-dd HH:mm:ss");
            this.lblPayTime.Text = order.Purchaser.PayTime.HasValue ? order.Purchaser.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";
            this.lblETDZTime.Text = order.ETDZTime.HasValue ? order.ETDZTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "-";
            switch(order.Status) {
                case OrderStatus.ConfirmFailed:
                case OrderStatus.DeniedWithSupply:
                case OrderStatus.DeniedWithETDZ:
                case OrderStatus.Canceled:
                    this.divFailed.Visible = true;
                    this.lblFailedReason.Text = order.Remark;
                    break;
            }
            this.linkPurchaser.InnerText = order.Purchaser.Name;
            this.linkPurchaser.HRef = "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + order.Purchaser.CompanyId.ToString() ;
            this.linkProvider.InnerText = order.Provider == null ? "-" : order.Provider.Name;
            this.linkProvider.HRef = order.Provider == null ? "#this" : "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + order.Provider.CompanyId.ToString() ;
            this.linkSupplier.InnerText = order.Supplier == null ? "-" : order.Supplier.Name;
            this.linkSupplier.HRef = order.Supplier == null ? "#this" : "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + order.Supplier.CompanyId.ToString() ;
            if (order.Provider != null && order.IsB3BOrder&& order.Provider.Product != null&&!order.Provider.Product.IsDefaultPolicy)
            {
                switch (order.Provider.Product.ProductType)
                {
                    case ProductType.General:
                        this.linkPrividerPolicy.HRef = "/Index.aspx?redirectUrl=/PolicyModule/MaintenancePolicy/base_policy_info.aspx?id=" + order.Provider.Product.Id;
                        break;
                    case ProductType.Promotion:
                        this.linkPrividerPolicy.HRef = "/Index.aspx?redirectUrl=/PolicyModule/MaintenancePolicy/low_price_policy_info.aspx?id=" + order.Provider.Product.Id;
                        break;
                    case ProductType.Special:
                        this.linkPrividerPolicy.HRef = "/Index.aspx?redirectUrl=/PolicyModule/MaintenancePolicy/special_policy_info.aspx?id=" + order.Provider.Product.Id;
                        break;
                    case ProductType.Team:
                        this.linkPrividerPolicy.HRef = "/Index.aspx?redirectUrl=/PolicyModule/MaintenancePolicy/team_policy_info.aspx?id=" + order.Provider.Product.Id;
                        break;
                }
            }
            else
            {
                this.linkPrividerPolicy.Visible = false;
            }
            if (order.IsThirdRelation)
            {
                this.linkSupplierPolicy.Visible = true;
                this.linkSupplierPolicy.HRef = "/Index.aspx?redirectUrl=/PolicyModule/MaintenancePolicy/special_policy_info.aspx?id=" + order.Supplier.Product.Id;
            }
        }
        private void bindPNRGroups(Service.Order.Domain.Order order) {
            Session["OriginalOrder"] = order;
            Session["ReservedFlights"] = order.PNRInfos.First().Flights;
            this.divPNRGroups.Visible = true;
            foreach(var item in order.PNRInfos) {
                var pnrInfo = LoadControl(ResolveUrl("~/OrderModule/UserControls/PNRInfo.ascx")) as OrderModule.UserControls.PNRInfo;
                pnrInfo.InitData(order, item);
                this.divPNRGroups.Controls.Add(pnrInfo);
            }
            var firstPNR = order.PNRInfos.First();
            if (firstPNR != null)
            {
                lblFlightInfo.Text = string.Join("<br />", firstPNR.Flights.Select(
                    f => string.Format("{0}{1} {2:yyyy-MM-dd HH:mm} {3}-{4} {5}/{6}", f.Carrier.Code, f.FlightNo, f.TakeoffTime, f.Departure.Code, f.Arrival.Code, f.Bunk.Code, f.Bunk.Discount==0?string.Empty:(f.Bunk.Discount * 100).TrimInvaidZero())));
                lblPassenger.Text = string.Join("<br />",firstPNR.Passengers.Select(
                    p => string.Format("{0}/{1}/{2}/{3}/{4}/{5}/<span class='price'>{6}</span>", 
                        p.Name,p.PassengerType.GetDescription(),p.Credentials,
                        p.Price.Fare.TrimInvaidZero(),p.Price.AirportFee.TrimInvaidZero(),
                        p.Price.BAF.TrimInvaidZero(),p.Price.Total.TrimInvaidZero()
                        )));
            }
            var productInfoType = (order.IsThirdRelation ? order.Supplier.Product.ProductType : order.Provider.Product.ProductType);
            PolicyType policyType = PolicyType.Normal;
            switch (productInfoType)
            {
                case ProductType.General:
                case ProductType.Special:
                    break;
                    case ProductType.Promotion:
                        policyType = PolicyType.Bargain;
                        break;
                    case ProductType.Team:
                    policyType = PolicyType.Team;
                    break;
            }
            hidPolicyType.Value = ((int)policyType).ToString();

            lblPurchase.Text = string.Format("{0}({1})", order.IsThirdRelation?order.Supplier.Company.UserName:order.Purchaser.Company.UserName,
                order.IsThirdRelation ? order.Supplier.Name : order.Purchaser.Name);
            lblPurchaseCommition.Text = string.Format("返点<span id='rate'>{0}</span>%（{1}元）", Math.Round((order.IsThirdRelation?
                order.Supplier.Rebate:order.Purchaser.Rebate) * 100, 2).TrimInvaidZero(),
                (order.IsThirdRelation?order.Supplier.Commission:order.Purchaser.Commission).TrimInvaidZero());
            PurshareID = order.IsThirdRelation ? order.Supplier.CompanyId.ToString() : order.Purchaser.CompanyId.ToString();
            if (order.Provider != null)
            {
                lblProvider.Text = string.Format("{0}({1})", order.Provider.Company.UserName, order.Provider.Name);
                lblCommition.Text = string.Format("返点<span id='rate'>{0}</span>%（{1}元）", Math.Round(order.Provider.Rebate * 100, 2).TrimInvaidZero(), order.Provider.Commission.TrimInvaidZero());
                ProviderID = order.Provider.CompanyId.ToString();
            }
        }
        private void bindOrderBills(Service.Order.Domain.Order order) {
            switch(order.Status) {
                case OrderStatus.PaidForSupply:
                case OrderStatus.DeniedWithSupply:
                case OrderStatus.PaidForETDZ:
                case OrderStatus.DeniedWithETDZ:
                case OrderStatus.Canceled:
                case OrderStatus.Finished:
                    this.bill.InitData(order.Bill);
                    this.bill.Visible = true;
                    break;
                default:
                    this.bill.Visible = false;
                    break;
            }
        }
        private void setButtons(Service.Order.Domain.Order order) {
            if(order.Status == OrderStatus.DeniedWithETDZ) {
                // 取消出票
                this.btnCancelOrder.Visible = true;
                // 重新出票
                this.btnReETDZ.Visible = true;
                // 重新提供资源
                this.btnReSupply.Visible = order.IsSpecial && order.IsThirdRelation;
                // 重新选择出票方
                if((order.Product.ProductType == ProductType.General || (order.IsSpecial && order.IsThirdRelation))
                    && !order.IsChildrenOrder && order.AssociateOrderId == null) {
                    this.btnChangeProvider.Visible = true;
                    //this.btnChangeProvider.Attributes.Add("onclick", "window.top.location.href='/FlightReserveModule/ChoosePolicy.aspx?orderId=" + order.Id + "&source=5';return false;");
                }
            } else if(order.Status == OrderStatus.Finished) {
                // 订单历史记录
                setRequestUrl("../OrderHistoryRecord.aspx", order.Id, this.btnOrderHistory);
                // 进行中的申请
                setRequestUrl("../ProcessingApplyform.aspx", order.Id, this.btnProcessingApplyforms);
                // 修改票号
                setRequestUrl("UpdateTicketNo.aspx", order.Id, this.btnUpdateTicketNo);
                // 修改证件号
                setRequestUrl("../UpdateCredentials.aspx", order.Id, this.btnUpdateCredentials);
                var now = DateTime.Now;
                var allTakeOff = order.PNRInfos.All(pnr=>pnr.Flights.All(item=>item.TakeoffTime<now));
                if (allTakeOff)
                    this.btnUpdateCredentials.Visible = false;
            }
        }
        private void setBackButton() {
            // 返回
            var returnUrl = Request.QueryString["returnUrl"];
            if(string.IsNullOrWhiteSpace(returnUrl)) {
                returnUrl = "OrderList.aspx";
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.hidReturnUrl.Value = returnUrl;
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private void setRequestUrl(string targetPage, decimal orderId, HtmlButton sender) {
            var requestUrl = string.Format("{0}?id={1}&returnUrl={2}&role=Operate", targetPage, orderId.ToString(), HttpUtility.UrlEncode(Request.Url.PathAndQuery));
            sender.Attributes.Add("onclick", "window.location.href='" + requestUrl + "';return false;");
            sender.Visible = true;
        }
        private void showErrorMessage(string message) {
            this.divError.Visible = true;
            this.divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }

        protected string ProviderID { get; set; }

        protected string PurshareID{get;set;}
    }
}