$(function () {
    $("#btnQuery").click(function () {
        if (!valiate()) {
            return false;
        }
    });
    $("#btnDownload").click(function () {
        if (valiate()) {
            var payStartDate = $.trim($("#txtPayStartDate").val());
            var payEndDate = $.trim($("#txtPayEndDate").val());
            var etdzed = $.trim($("#ddlPrintStatus").val());
            var airline = $.trim($("#ddlAirlines").val());
            var departure = $.trim($("#txtDeparture_ddlAirports").val());
            var payed = $.trim($("#ddlPayStatus").val());
            var pnr = $.trim($("#PNR").val());
            var arrival = $.trim($("#txtArrivals_ddlAirports").val());
            var orderSource = $.trim($("#ddlOrderSource").val());
            var externalOrderId = $.trim($("#txtExternalOrderId").val());
            var internalOrderId = $.trim($("#txtInternalOrderId").val());
            var platformExternalOrderCondition = payStartDate + ',' + payEndDate + ',' + etdzed + ',' + airline + ',' + departure + ',' + payed + ',' + pnr + ',' + arrival + ',' + orderSource + ',' + externalOrderId + ',' + internalOrderId;
            window.open('ReportDownload.aspx?platformExternalOrderCondition=' + platformExternalOrderCondition, 'newWindow');
        }
    });
    SaveDefaultData(null, '.text-s1');
})
function valiate() {
    if (!valiatePnr($("#txtPnr"))) {
        alert("PNR编码格式错误！");
        $("#txtPnr").select();
        return false;
    }
    if ($.trim($("#txtExternalOrderId").val()).length > 20) {
        alert("外部订单号格式错误！");
        $("#txtExternalOrderId").select();
        return false;
    }
    if (!valiateOrderId($("#txtOrderId"))) {
        alert("内部订单号格式错误！");
        $("#txtInternalOrderId").select();
        return false;
    }
    return true;
}