$(function () {
    $("#btnQuery").click(function () {
        if (!valiate()) {
            return false;
        }
    });
    SaveDefaultData(null, '.text-s1');
    $("#btnDownload").click(function () {
        if (valiate()) {
            var startDate = $.trim($("#txtStartDate").val());
            var endDate = $.trim($("#txtEndDate").val());
            var payStartDate = $.trim($("#txtPayStartDate").val());
            var payEndDate = $.trim($("#txtPayEndDate").val());
            var takeOffStartDate = $.trim($("#txtTakeOffStartDate").val());
            var takeOffEndDate = $.trim($("#txtTakeOffEndDate").val());
            var ticketNo = $.trim($("#txtTicketNo").val());
            var passenger = $.trim($("#txtPassenger").val());
            var pnr = $.trim($("#txtPNR").val());
            var departure = $.trim($("#txtDeparture_ddlAirports").val());
            var arrival = $.trim($("#txtArrivals_ddlAirports").val());
            var titcketStatus = $.trim($("#ddlTicketStatus").val());
            var policyType = $.trim($("#ddlPolicyType").val());
            var airlines = $.trim($("#ddlAirlines").val());
            var tiketType = $.trim($("#ddlTiketType").val());
            var companyId = $.trim($("#hfdPurchaseCompanyId").val());
            var orderId = $.trim($("#txtOrderId").val());
            var payType = $("#ddlPayType").val();
            var purchaseTicketCondition = startDate + ',' + endDate + ',' + payStartDate + ',' + payEndDate + ',' + takeOffStartDate + ',' + takeOffEndDate + ','
                                             + ticketNo + ',' + passenger + ',' + pnr + ',' + departure + ',' + arrival + ',' + titcketStatus + ',' + policyType + ','
                                              + airlines + ',' + tiketType + ',' + companyId + ',' + orderId+','+payType;
            window.open('ReportDownload.aspx?purchaseTicketCondition=' + purchaseTicketCondition, 'newWindow');
        }
    });

    seniorConditionShowOrHide();
    $("#btnSeniorCondition").click(function () {
        if ($("#btnSeniorCondition").val() == "更多条件") {
            $("#seniorCondition").css("display", "");
            $(".reset").css("display", "");
            $(".resetName").css("display", "");
            $("#hfdSeniorCondition").val("show");
        }
        if ($("#btnSeniorCondition").val() == "简化条件") {
            $("#seniorCondition").css("display", "none");
            $(".reset").css("display", "none");
            $(".resetName").css("display", "none");
            $("#hfdSeniorCondition").val("hide");
        }
        seniorConditionShowOrHide();
    });
})
function valiate() {
    if (!valiatePnr($("#txtPNR"))) {
        alert("PNR编码格式错误！");
        $("#txtPNR").select();
        return false;
    }
    if (!valiateTicketNo($("#txtTicketNo"))) {
        alert("票号格式错误！");
        $("#txtTicketNo").select();
        return false;
    }
    if (!valiatePassenger($("#txtPassenger"))) {
        alert("乘机人位数不能超过25位！");
        $("#txtPassenger").select();
        return false;
    }
    if (!valiateOrderId($("#txtOrderId"))) {
        alert("订单号格式错误！");
        $("#txtOrderId").select();
        return false;
    }
    return true;
}

function seniorConditionShowOrHide() {
    if ($("#hfdSeniorCondition").val() == "show") {
        $("#btnSeniorCondition").val("简化条件");
    }
    if ($("#hfdSeniorCondition").val() == "hide") {
        $("#btnSeniorCondition").val("更多条件");
        resetSeniorCondition();
    }
}

function resetSeniorCondition() {
    if ($("#hfdSeniorCondition").val() != "show") {
        $(".reset").val("");
    }
}