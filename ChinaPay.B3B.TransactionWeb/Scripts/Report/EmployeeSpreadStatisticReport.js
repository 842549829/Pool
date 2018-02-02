$(function () {
//    $("#btnQuery").click(function () {
//        if (!valiate()) {
//            return false;
//        }
//    });
    $("#btnDownload").click(function () {
        if (valiate()) {
            var finishStartDate = $.trim($("#txtStartDate").val());
            var finishEndDate = $.trim($("#txtEndDate").val());
            var type = $.trim($("#selCompanyType").val());
            var no = $.trim($("#txtEmpolyeeNo").val());
            var supplyTicketCondition = finishStartDate + ',' + finishEndDate + ',' + type + ',' + no;
            window.open('ReportDownload.aspx?platformSpreadStatisticCondition=' + supplyTicketCondition, 'newwindow');
        }
    });
    SaveDefaultData(null, '.text-s1');
})
function valiate() {
//    if (!valiateOrderId($("#txtOrderId"))) {
//        alert("订单号格式错误！");
//        $("#txtOrderId").select();
//        return false;
//    }
//    if (!valiatePnr($("#txtPNR"))) {
//        alert("PNR编码格式错误！");
//        $("#txtPNR").select();
//        return false;
//    }
    return true;
}