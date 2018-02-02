using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ChinaPay.B3B.Service.Order.Domain;
using ChinaPay.Core.Extension;
using ChinaPay.B3B.Service.SystemManagement;
using ChinaPay.B3B.DataTransferObject.Order;
using ChinaPay.B3B.TransactionWeb.PublicClass;
using ChinaPay.B3B.DataTransferObject.Organization;
using Izual.Data.Common;

namespace ChinaPay.B3B.TransactionWeb.OrderModule.Operate {
    public partial class Coordination : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
            RegisterOEMSkins("form.css");
            RegisterOEMSkins("page.css");
            if (!IsPostBack)
            {
                setBackButton();
                getCoordinationContent();
                getCoordinationResult();
                GetIdentificationOrder();
                var id = Request.QueryString["id"];
                var type = Request.QueryString["type"];
                decimal recordId;
                if(decimal.TryParse(id, out recordId)) {
                    if(type == "1") {
                        var applyform = Service.ApplyformQueryService.QueryApplyform(recordId);
                        if(applyform == null) {
                            showErrorMessage("申请单不存在");
                        } else {
                            bindDatas(applyform);
                            setButtons(recordId, type);
                            bindCoordinations(recordId, type);
                        }
                    } else {
                        var order = Service.OrderQueryService.QueryOrder(recordId);
                        if(order == null) {
                            showErrorMessage("订单不存在");
                        } else {
                            bindDatas(order);
                            setButtons(recordId, type);
                            bindCoordinations(recordId, type);
                            if (order.Status == OrderStatus.PaidForSupply || order.Status == OrderStatus.PaidForETDZ) {
                                //divIdentificationOrder.Visible = true;
                                dropIdentificationOrder.Enabled = true;
                                BindIdentificationOrderContent(order.Id, order.Status);
                            }
                        }
                    }
                } else {
                    showErrorMessage("参数错误");
                }
            }
        }
        private void BindIdentificationOrderContent(decimal id, OrderStatus status) {
            EmergentOrder emergentOrder = ChinaPay.B3B.Service.Order.CoordinationService.GetEmergentOrder(id,status);
            if (emergentOrder!= null && !string.IsNullOrEmpty(emergentOrder.Content))
            {
                dropIdentificationOrder.Items[1].Selected = true;
                //txtIdentificationOrder.Text = emergentOrder.Content;
                lblIdentificationTime.Text = emergentOrder.Time.ToString("yyyy-MM-dd HH:mm:ss");
                lblIdentificationContent.Text = emergentOrder.Content;
                lblIdentificationAccount.Text = emergentOrder.Account;
            }
        }
        private void bindDatas(Service.Order.Domain.Order order) {
            this.hidBusinessType.Value = ((int)BusinessType.出票).ToString();
            bindHeader(order);
            var passengers = from pnr in order.PNRInfos
                             from passenger in pnr.Passengers
                             select passenger;
            bindPassengers(passengers);
            bindOrderContact(order.Contact);
            bindPurchaser(order.Purchaser.Company);
            bindSupplier(order.Supplier == null ? null : order.Supplier.Company);
            bindProvider(order.Provider == null ? null : order.Provider.Company);
            if (order.IsOEMOrder)
            {
                bindOemInfo(order.OemInfo);
            }
            else 
            {
                divOemInfo.Visible = false;
            }
        }
        private void bindDatas(Service.Order.Domain.Applyform.BaseApplyform applyform) {
            this.hidBusinessType.Value = ((int)getBusinessType(applyform)).ToString();
            bindHeader(applyform);
            if (applyform is Service.Order.Domain.Applyform.BalanceRefundApplyform)
            {
                var form = applyform as Service.Order.Domain.Applyform.BalanceRefundApplyform;
                bindPassengers(form.Applyform.Passengers);
            }
            else
            {
                bindPassengers(applyform.Passengers);
            }
            bindOrderContact(applyform.Order.Contact);
            bindPurchaser(applyform.Purchaser);
            bindProvider(applyform.Provider);
            divOemInfo.Visible = false;
        }
        private BusinessType getBusinessType(Service.Order.Domain.Applyform.BaseApplyform applyform) {
            if(applyform is Service.Order.Domain.Applyform.RefundApplyform) {
                return BusinessType.退票;
            } else if(applyform is Service.Order.Domain.Applyform.ScrapApplyform) {
                return BusinessType.废票;
            } else if(applyform is Service.Order.Domain.Applyform.PostponeApplyform) {
                return BusinessType.改期;
            }
            else if (applyform is Service.Order.Domain.Applyform.BalanceRefundApplyform)
            {
                return BusinessType.差额退款;
            }
            return BusinessType.出票;
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
        private void bindHeader(Service.Order.Domain.Order order) {
            this.applyform.Visible = false;
            bindHeaderItem(this.lblOrderId, order.Id.ToString());
            bindHeaderItem( this.lblOrderStatus, Service.Order.StatusService.GetOrderStatus(order.Status, DataTransferObject.Order.OrderRole.Platform));
            bindHeaderItem(this.lblProductType, order.Product.ProductType.GetDescription());
            bindHeaderItem(this.lblProducedTime, order.Purchaser.ProducedTime.ToString("yyyy-MM-dd HH:mm:ss"));
            bindHeaderItem(this.lblPayTime, order.Purchaser.PayTime.HasValue ? order.Purchaser.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty);
            this.hidOrderId.Value = order.Id.ToString();
            hidOrderStatus.Value = order.Status.GetHashCode().ToString();
        }
        private void bindHeader(Service.Order.Domain.Applyform.BaseApplyform applyform) {
            this.order.Visible = false;
            bindHeaderItem(this.lblApplyformId, applyform.Id.ToString());
            bindHeaderItem(this.lblApplyformStatus, getApplyformStatus(applyform));
            bindHeaderItem(this.lblAppliedTime, applyform.AppliedTime.ToString("yyyy-MM-dd HH:mm:ss"));
            bindHeaderItem(this.lbllProductType, applyform.ProductType.GetDescription());
            this.hidOrderId.Value = applyform.OrderId.ToString();
            this.hidApplyformId.Value = applyform.Id.ToString();
        }
        private string getApplyformStatus(Service.Order.Domain.Applyform.BaseApplyform applyform) {
            if(applyform is Service.Order.Domain.Applyform.RefundOrScrapApplyform) {
                return Service.Order.StatusService.GetRefundApplyformStatus((applyform as Service.Order.Domain.Applyform.RefundOrScrapApplyform).Status, DataTransferObject.Order.OrderRole.Platform);
            } else if(applyform is Service.Order.Domain.Applyform.PostponeApplyform) {
                return Service.Order.StatusService.GetPostponeApplyformStatus((applyform as Service.Order.Domain.Applyform.PostponeApplyform).Status, DataTransferObject.Order.OrderRole.Platform);
            }
            else if (applyform is Service.Order.Domain.Applyform.BalanceRefundApplyform)
            {
                return Service.Order.StatusService.GetBalanceRefundStatus(((Service.Order.Domain.Applyform.BalanceRefundApplyform)applyform).BalanceRefundStatus, OrderRole.Platform);
            }
            return string.Empty;
        }
        private void bindHeaderItem(Label label, string value) {
            if(!string.IsNullOrWhiteSpace(value)) {
                label.Text = value;
            } 
        }
        private void bindPassengers(IEnumerable<Service.Order.Domain.Passenger> passengers) {
            this.passengers.DataSource = passengers;
            this.passengers.DataBind();
        }
        private void bindOrderContact(DataTransferObject.Order.Contact contact) {
            this.lblOrderContact.Text = contact.Name;
            this.lblOrderContactMobile.Text = contact.Mobile;
            this.lblOrderContactEmail.Text = contact.Email;
        }
        private void bindPurchaser(DataTransferObject.Organization.CompanyInfo purchaser) {
            if(purchaser != null) {
                this.lnkPurchaserCompanyName.InnerText = purchaser.AbbreviateName;
                this.lnkPurchaserCompanyName.HRef = "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + purchaser.CompanyId.ToString();
                this.lblPurchaserContact.Text = purchaser.Contact;
                this.lblPurchaserCompanyType.Text = purchaser.CompanyType.GetDescription();
                this.lblPurchaserCompanyTel.Text = purchaser.OfficePhones;
                this.lblPurchaserContactPhone.Text = purchaser.ContactPhone;
                this.lblPurchaserEmergentContactPhone.Text = purchaser.EmergencyCall;
                this.lblPurchaserLocation.Text = getLocation(purchaser.City);
                this.lblPurchaserAddress.Text = purchaser.Address;
                this.lblPurchaserPostcode.Text = purchaser.ZipCode;
            }
        }
        private void bindSupplier(DataTransferObject.Organization.CompanyInfo supplier) {
            if(supplier == null) {
                this.divSupplier.Visible = false;
            } else {
                this.divSupplier.Visible = true;
                this.lnkSupplierCompanyName.InnerText = supplier.AbbreviateName;
                this.lnkSupplierCompanyName.HRef = "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + supplier.CompanyId.ToString();
                this.lblSupplierContact.Text = supplier.Contact;
                this.lblSupplierContactPhone.Text = supplier.ContactPhone;
            }
        }
        private void bindProvider(DataTransferObject.Organization.CompanyInfo provider) {
            if(provider == null) {
                this.divProvider.Visible = false;
                this.divProviderBusinessInfo.Visible = false;
            } else {
                this.lnkProviderCompanyName.InnerText = provider.AbbreviateName;
                this.lnkProviderCompanyName.HRef = "/OrganizationModule/TerraceModule/CompanyInfoManage/LookUpCompanyInfo.aspx?CompanyId=" + provider.CompanyId.ToString();
                this.lblProviderContact.Text = provider.Contact;
                this.lblProviderCompanyTel.Text = provider.OfficePhones;
                this.lblProviderContactPhone.Text = provider.ContactPhone;
                this.lblProviderFax.Text = provider.Faxes;
                this.lblProviderLocation.Text = getLocation(provider.City);
                this.lblProviderAddress.Text = provider.Address;
                this.lblProviderPostcode.Text = provider.ZipCode;
                var workingHours = Service.Organization.CompanyService.GetWorkinghours(provider.CompanyId);
                if(DateTime.Today.IsWeekend()) {
                    this.lblProviderWorkingTime.Text = workingHours.RestdayWorkStart.ToString("HH:mm") + " - " + workingHours.RestdayWorkEnd.ToString("HH:mm");
                    this.lblProviderScrapTime.Text = workingHours.RestdayRefundStart.ToString("HH:mm") + " - " + workingHours.RestdayRefundEnd.ToString("HH:mm");
                } else {
                    this.lblProviderWorkingTime.Text = workingHours.WorkdayWorkStart.ToString("HH:mm") + " - " + workingHours.WorkdayWorkEnd.ToString("HH:mm");
                    this.lblProviderScrapTime.Text = workingHours.WorkdayRefundStart.ToString("HH:mm") + " - " + workingHours.WorkdayRefundEnd.ToString("HH:mm");
                }
                bindProviderBusinessContact(provider);
            }
        }
        private void bindProviderBusinessContact(DataTransferObject.Organization.CompanyInfo provider) {
            this.businessManagers.DataSource = Service.Organization.CompanyService.GetBusinessManagers(provider.CompanyId);
            this.businessManagers.DataBind();
        }
        private void bindCoordinations(decimal id, string type) {
            initCoordinationDatas();
            this.txtCoordinationContent.Visible = true;
            this.txtCoordinationResult.Visible = true;
            this.btnSave.Visible = true;
            this.btnSaveAndBack.Visible = true;
            IEnumerable<Service.Order.Domain.Coordination> coordinations = null;
            if(type == "1") {
                coordinations = Service.Order.CoordinationService.QueryApplyformCoordinations(id);
            } else {
                coordinations = Service.Order.CoordinationService.QueryOrderCoordinations(id);
            }
            drawCoordinations(coordinations);
        }
        private void drawCoordinations(IEnumerable<Service.Order.Domain.Coordination> coordinations) {
            var providerTemplate = "<div class=\"c_me\">   <div class=\"c_txt\">   <span class=\"c_ico\"></span>{0}  </div>   <div class=\"c_info\">   <span class=\"c_time\">{1}</span>   <span class=\"c_result\">{2}</span>   <a class=\"c_author\" href=\"javascript:void(0);\">{3}</a>   </div>   </div>";
            var platformTemplate = "<div class=\"c_others\">     <div class=\"c_txt\">         <span class=\"c_ico\"></span> {0}</div>     <div class=\"c_info\">         <span class=\"c_time\">{1}</span>         <span class=\"c_player\">{2}</span>         <span class=\"c_ways\">{3}</span>         <span class=\"c_result\">{4}</span>         <a class=\"c_author\" href=\"javascript:void(0);\">{5}</a>     </div> </div>";
            var contentHtml = new StringBuilder();
            foreach (Service.Order.Domain.Coordination coor in coordinations)
            {
                if (coor.OrderRole == OrderRole.Platform)
                {
                    contentHtml.AppendFormat(platformTemplate, coor.Content, coor.Time.ToString("yyyy-MM-dd HH:mm")
                        , coor.OrderRole.GetDescription(), coor.Mode.GetDescription(), coor.Result,
                        coor.Account);
                }
                else
                {
                    contentHtml.AppendFormat(providerTemplate, coor.Content, coor.Time.ToString("yyyy-MM-dd HH:mm"),
                        coor.Result, coor.Account);
                }
            }
            this.divCoordinationContent.Text = contentHtml.ToString();
        }

        private void initCoordinationDatas() {
            var contactModes = Enum.GetValues(typeof(ContactMode));
            this.ddlContactMode.Items.Clear();
            foreach(var item in contactModes) {
                var mode = (ContactMode)((int)item);
                this.ddlContactMode.Items.Add(new ListItem(mode.GetDescription(), ((int)mode).ToString()));
            }
            this.ddlContactMode.Items.Insert(0, new ListItem("请选择", ""));
        }
        private void setButtons(decimal id, string type) {
            var target = type == "1" ? "ApplyformDetail.aspx" : "OrderDetail.aspx";
            var returnUrl = Request.Url.PathAndQuery;
            var requestUrl = string.Format("{0}?id={1}&returnUrl={2}", target, id, HttpUtility.UrlEncode(returnUrl));
            this.btnDetail.Attributes.Add("onclick", "window.location.href='"+ requestUrl + "';return false;");
        }
        private void setBackButton() {
            var returnUrl = Request.QueryString["returnUrl"];
            if(string.IsNullOrWhiteSpace(returnUrl)) {
                returnUrl = (Request.UrlReferrer ?? Request.Url).PathAndQuery;
            }
            if (returnUrl.IndexOf("Search=Back") == -1) returnUrl += (returnUrl.IndexOf("?") > -1 ? "&" : "?") + "Search=Back";
            this.hidReturnUrl.Value = returnUrl;
            this.btnBack.Attributes.Add("onclick", "window.location.href='" + returnUrl + "';return false;");
        }
        private void showErrorMessage(string message) {
            this.divError.Visible = true;
            this.divError.InnerHtml = "<h2>" + message + "</h2>";
            form1.Visible = false;
        }
        private string getLocation(string cityCode) {
            if(!string.IsNullOrWhiteSpace(cityCode)) {
                var city = Service.FoundationService.QueryCity(cityCode);
                if(city != null) {
                    return (city.Province == null ? "" : city.Province.Name) + city.Name;
                }
            }
            return string.Empty;
        }
        protected void btnSave_Click(object sender, EventArgs e) {
            try {
                saveCoordination();
                var businessType = hidBusinessType.Value;
                bindCoordinations(businessType=="1"?hidApplyformId.Value.ToDecimal():hidOrderId.Value.ToDecimal(), hidBusinessType.Value);
                ClientScript.RegisterClientScriptBlock(this.GetType(), this.UniqueID, "alert('操作成功');", true);
            } catch(Exception ex) {
                ShowExceptionMessage(ex,"操作");
            }
        }
        protected void btnSaveAndBack_Click(object sender, EventArgs e) {
            try {
                saveCoordination();
                ClientScript.RegisterClientScriptBlock(this.GetType(), this.UniqueID, "alert('保存成功');window.location.href='"+ this.hidReturnUrl.Value +"';", true);
            } catch {
                ClientScript.RegisterClientScriptBlock(this.GetType(), this.UniqueID, "alert('操作失败');", true);
            }
        }
        private void saveCoordination() {
            var businessType = (BusinessType)int.Parse(this.hidBusinessType.Value);
            decimal orderId = decimal.Parse(this.hidOrderId.Value);
            Service.Order.Domain.Coordination coordination = null;
            bool isManipulative = false;
            if(businessType == BusinessType.出票) {
                OrderStatus orderStatus = (OrderStatus)int.Parse(hidOrderStatus.Value);
                if (!string.IsNullOrWhiteSpace(dropIdentificationOrder.SelectedValue))
                {
                    EmergentOrder emergentOrder = emergentOrder = new EmergentOrder();   
                    emergentOrder.Id = orderId;
                    emergentOrder.Content = dropIdentificationOrder.SelectedValue.Trim();
                    emergentOrder.Time = DateTime.Now;
                    emergentOrder.Type = orderStatus;
                    emergentOrder.Account = CurrentUser.UserName;
                    emergentOrder.OrderIdTypeValue = getOrderIdType();
                    if (!string.IsNullOrEmpty(emergentOrder.Content))
                    {
                        if (emergentOrder.Content.Length > 200) throw new ChinaPay.Core.CustomException("订单标识内容最多200字");
                        Service.Order.CoordinationService.SvaeEmergentOrder(emergentOrder, CurrentCompany.CompanyId);
                        lblIdentificationTime.Text = emergentOrder.Time.ToString("yyyy-MM-dd HH:mm:ss");
                        lblIdentificationContent.Text = emergentOrder.Content;
                        lblIdentificationAccount.Text = emergentOrder.Account;
                        isManipulative = true;
                    }
                }
                if (!string.IsNullOrEmpty(txtCoordinationContent.Text) && !string.IsNullOrEmpty(txtCoordinationResult.Text))
                {
                    if (string.IsNullOrEmpty(ddlContactMode.SelectedValue)) throw new ChinaPay.Core.CustomException("请选择联系方式");
                    if (txtCoordinationContent.Text.Length > 200) throw new ChinaPay.Core.CustomException("协调内容最多200字");
                    if (txtCoordinationResult.Text.Length > 200) throw new ChinaPay.Core.CustomException("协调结果最多200字");
                    var contactMode = (ContactMode)int.Parse(this.ddlContactMode.SelectedValue);
                    coordination = new Service.Order.Domain.Coordination(CurrentUser.UserName, this.txtCoordinationContent.Text.Trim(), this.txtCoordinationResult.Text.Trim(), businessType, contactMode);
                    Service.Order.CoordinationService.SaveOrderCoordination(orderId, coordination);
                    Service.OrderProcessService.UpdateRemindStatus(orderId);
                    isManipulative = true;
                }
                if (!isManipulative) throw new ChinaPay.Core.CustomException("由于您未进行任何协调信息变更，本次操作无效");
            } else {
                var contactMode = (ContactMode)int.Parse(this.ddlContactMode.SelectedValue);
                coordination = new Service.Order.Domain.Coordination(CurrentUser.UserName, this.txtCoordinationContent.Text.Trim(), this.txtCoordinationResult.Text.Trim(), businessType, contactMode);
                var applyformId = decimal.Parse(this.hidApplyformId.Value);
                Service.Order.CoordinationService.SaveApplyformCoordination(orderId, applyformId, coordination);
            }
            //if (coordination != null)
            //{
            //    this.divCoordinationContent.InnerHtml = this.divCoordinationContent.InnerHtml.Insert(this.divCoordinationContent.InnerHtml.Length - 8, drawCoordination(coordination));
            //}
            this.ddlContactMode.ClearSelection();
            this.txtCoordinationContent.Text = string.Empty;
            this.txtCoordinationResult.Text = string.Empty;
        }
        private ChinaPay.B3B.Service.Order.Domain.OrderIdType getOrderIdType() 
        {
            string type = Request.QueryString["type"];
            return string.IsNullOrEmpty(type) ? ChinaPay.B3B.Service.Order.Domain.OrderIdType.Order :type == "1" ?ChinaPay.B3B.Service.Order.Domain.OrderIdType.Apply :ChinaPay.B3B.Service.Order.Domain.OrderIdType.Order;
        }
        private void getCoordinationContent()
        {
            var coordinationContent = SystemDictionaryService.Query(Service.SystemManagement.Domain.SystemDictionaryType.CoordinationContent);
            this.ddlCoordinationContent.DataSource = coordinationContent;
            this.ddlCoordinationContent.DataTextField = "Name";
            this.ddlCoordinationContent.DataValueField = "Name";
            this.ddlCoordinationContent.DataBind();
            this.ddlCoordinationContent.Items.Insert(0,new ListItem("-请选择-", ""));
        }
        private void getCoordinationResult()
        {
            var coordinationResult = SystemDictionaryService.Query(Service.SystemManagement.Domain.SystemDictionaryType.CoordinationResult);
            this.ddlCoordinationResult.DataSource = coordinationResult;
            this.ddlCoordinationResult.DataTextField = "Name";
            this.ddlCoordinationResult.DataValueField = "Name";
            this.ddlCoordinationResult.DataBind();
            this.ddlCoordinationResult.Items.Insert(0, new ListItem("-请选择-",""));
        }
        private void GetIdentificationOrder() {
            IEnumerable<ChinaPay.B3B.Service.SystemManagement.Domain.SystemDictionaryItem> identificationOrder = SystemDictionaryService.Query(Service.SystemManagement.Domain.SystemDictionaryType.IdentificationOrder);
            dropIdentificationOrder.DataSource = identificationOrder;
            dropIdentificationOrder.DataValueField = dropIdentificationOrder.DataTextField = "Name";
            dropIdentificationOrder.DataBind();
            dropIdentificationOrder.Items.Insert(0, new ListItem("-请选择-", ""));
        }
    }
}