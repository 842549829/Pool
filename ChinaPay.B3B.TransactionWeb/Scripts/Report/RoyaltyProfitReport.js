$(function () {
    SaveDefaultData(null, '.text-s1');
    $("#btnQuery").click(function () {
        if (!valiate()) {
            return false;
        }
    });
    $("#btnDownload").click(function () {
        var startDate = $.trim($("#txtStartDate").val());
        var endDate = $.trim($("#txtEndDate").val());
        var pnr = $.trim($("#txtPnr").val());
        var orderId = $.trim($("#txtOrderId").val());
        var titcketNo = $.trim($("#txtTicketNo").val());
        var type = $.trim($("#ddlType").val());
        var payStatus = $.trim($("#ddlPayStatus").val());
        var payType = $.trim($("#ddlPayType").val());
        var royaltyCompanyId = $.trim($("#ddlRoyaltyCompany").val());
        var companyId = $.trim($("#hfdCompanyId").val());
        var companyType = $.trim($("#hfdCompanyType").val());
        var incomeGroupId = $.trim($("#ddlIncomeGroup").val());
        var purchaseCompany = $.trim($("#txtPurchaseCompany_hidCompanyId").val());
        var purchaseId = $.trim($("#ddlPurchaseCompany").val());
        var royaltyProfitCondition = startDate + ',' + endDate + ',' + pnr + ',' + orderId + ','
                                             + titcketNo + ',' + type + ',' + payStatus + ',' + payType + ','
                                             + royaltyCompanyId + ',' + companyId + ',' + companyType + ',' + incomeGroupId + ',' + purchaseCompany+','+purchaseId;
        window.open('ReportDownload.aspx?royaltyProfitCondition=' + royaltyProfitCondition, 'newwindow');
    });
})
function valiate() {
    if (!valiatePnr($("#txtPnr"))) {
        alert("PNR编码格式错误！");
        $("#txtPnr").select();
        return false;
    }
    if (!valiateOrderId($("#txtOrderId"))) {
        alert("订单号格式错误！");
        $("#txtOrderId").select();
        return false;
    }
    if (!valiateTicketNo($("#txtTicketNo"))) {
        alert("票号格式错误！");
        $("#txtTicketNo").select();
        return false;
    }
    return true;
}