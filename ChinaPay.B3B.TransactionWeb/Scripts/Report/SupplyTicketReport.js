$(function () {
    $("#btnQuery").click(function () {
        if (!valiate()) {
            return false;
        }
    });
    $("#btnDownload").click(function () {
        if (valiate()) {
            var finishStartDate = $.trim($("#txtFinishStartDate").val());
            var finishEndDate = $.trim($("#txtFinishEndDate").val());
            var orderId = $.trim($("#txtOrderId").val());
            var pnr = $.trim($("#txtPNR").val());
            var ticketStatus = $.trim($("#ddlTicketStatus").val());
            var airlines = $.trim($("#ddlAirlines").val());
            var companyId = $.trim($("#hfdSupplyCompanyId").val());
            var speacialType = $.trim($("#ddlSpecialType").val());
            var supplyTicketCondition = finishStartDate + ',' + finishEndDate + ',' + orderId + ',' + pnr + ',' + ticketStatus + ',' + airlines + ',' + companyId+','+speacialType;
            window.open('ReportDownload.aspx?supplyTicketCondition=' + supplyTicketCondition, 'newwindow');
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
    if (!valiatePnr($("#txtPNR"))) {
        alert("PNR编码格式错误！");
        $("#txtPNR").select();
        return false;
    }
    return true;
}