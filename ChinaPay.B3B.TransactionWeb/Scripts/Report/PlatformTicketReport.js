$(function () {
    $("#btnQuery").click(function () {
        if (!valiate()) {
            return false;
        }
    });
    $("#btnDownload").click(function () {
        if (valiate()) {
            var startDate = $.trim($("#txtStartDate").val());
            var endDate = $.trim($("#txtEndDate").val());
            var airlines = $.trim($("#ddlAirlines").val());
            var titcketStatus = $.trim($("#ddlTicketStatus").val());
            var providerCompany = $.trim($("#txtProviderCompany_hidCompanyId").val());
            var productCompany = $.trim($("#txtProductCompany_hidCompanyId").val());
            var purchaseCompany = $.trim($("#txtPurchaseCompany_hidCompanyId").val());
            var ticketNo = $.trim($("#txtTicketNo").val());
            var passenger = $.trim($("#txtPassenger").val());
            var pnr = $.trim($("#txtPNR").val());
            var relationType = $.trim($("#ddlRelationType").val());
            var orderId = $.trim($("#txtOrderId").val());
            var takeOffStartDate = $.trim($("#txtTakeOffLowerTime").val());
            var takeOffEndDate = $.trim($("#txtTakeOffUpperTime").val());
            var payType = $("#ddlPayType").val();
            var platformTicketCondition = startDate + ',' + endDate + ',' + airlines + ',' + titcketStatus + ',' + providerCompany + ',' + productCompany + ',' + purchaseCompany + ',' + ticketNo + ',' + passenger + ',' + pnr + ',' + relationType + ',' + orderId + ',' + takeOffStartDate + ',' + takeOffEndDate+','+payType;
            window.open('ReportDownload.aspx?platformTicketCondition=' + platformTicketCondition, 'newWindow');
        }
    });
    SaveDefaultData(null, '.text-s1');
})
function valiate() {
    if (!valiateOrderId($("#txtOrderId"))) {
        alert("订单号格式错误！");
        $("#txtOrderId").select();
        return false;
    }
    if (!valiatePassenger($("#txtPassenger"))) {
        alert("乘机人位数不能超过25位！");
        $("#txtPassenger").select();
        return false;
    }  
    if (!valiateTicketNo($("#txtTicketNo"))) {
        alert("票号格式错误！");
        $("#txtTicketNo").select();
        return false;
    }
    if (!valiatePnr($("#txtPNR"))) {
        alert("PNR编码格式错误！");
        $("#txtPNR").select();
        return false;
    }
    return true;
}