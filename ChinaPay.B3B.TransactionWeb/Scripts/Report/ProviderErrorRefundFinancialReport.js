$(function () {
    $("#btnQuery").click(function () {
        if (!valiate()) {
            return false;
        }
    })
    SaveDefaultData(null, '.text-s1');
    $("#btnDownload").click(function () {
        if (valiate()) {
            var applyStartDate = $.trim($("#txtApplyStartDate").val());
            var applyEndDate = $.trim($("#txtApplyEndDate").val());
            var orderId = $.trim($("#txtOrderId").val());
            var departure = $.trim($("#txtDeparture_ddlAirports").val());
            var settleCode = $.trim($("#txtSettleCode").val());
            var ticketNo = $.trim($("#txtTicketNo").val());
            var applyformId = $.trim($("#txtApplyformId").val());
            var arrivals = $.trim($("#txtArrivals_ddlAirports").val());
            var passenger = $.trim($("#txtPassenger").val());
            var processorAccount = $.trim($("#ddlOperator").val());
            var provider = $.trim($("#hfdProvider").val());

            var amountOfErrorRefundCondition = applyStartDate + ',' + applyEndDate + ',' + orderId + ',' + departure + ',' +
						                                   settleCode + ',' + ticketNo + ',' + applyformId + ',' + arrivals + ',' +
						                                   passenger + ',' + processorAccount + ',' + provider;
            window.open('ReportDownload.aspx?ProviderErrorRefundFinancialCondition=' + amountOfErrorRefundCondition, 'newWindow');
        }
    })
})

function valiate() {
    if (!valiateOrderId($("#txtOrderId"))) {
        alert("订单号格式错误！");
        $("#txtOrderId").select();
        return false;
    }
    if (!valiatePassenger($("#txtPassenger"))) {
        alert("乘机人位数不能超过25位!");
        $("#txtPassenger").select();
        return false;
    }
    if (!valiateTicketNo($("#txtTicketNo"))) {
        alert("票号格式错误!");
        $("#txtTicketNo").select();
        return false;
    }
    if (!valiateApplyformId($("#txtApplyformId"))) {
        alert("申请单号格式错误!");
        $("#txtApplyformId").select();
        return false;
    }
    return true;
}