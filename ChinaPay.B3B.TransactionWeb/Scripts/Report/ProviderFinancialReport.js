$(function () {
    $("#btnQuery").click(function () {
        if (!valiate()) {
            return false;
        }
    });
    $("#btnDownload").click(function () {
        var startDate = $.trim($("#txtStartDate").val());
        var endDate = $.trim($("#txtEndDate").val());
        var payStartDate = $.trim($("#txtPayStartDate").val());
        var payEndDate = $.trim($("#txtPayEndDate").val());
        var ticketNo = $.trim($("#txtTicketNo").val());
        var passenger = $.trim($("#txtPassenger").val());
        var departure = $.trim($("#txtDeparture_ddlAirports").val());
        var arrival = $.trim($("#txtArrivals_ddlAirports").val());
        var titcketStatus = $.trim($("#ddlTicketStatus").val());
        var policyType = $.trim($("#ddlPolicyType").val());
        var airlines = $.trim($("#ddlAirlines").val());
        var tiketType = $.trim($("#ddlTiketType").val());
        var orderId = $.trim($("#txtOrderId").val());
        var companyId = $.trim($("#hfdProviderCompanyId").val());
        var officeNo = $.trim($("#ddlOffice").val());
        var relationType = $.trim($("#ddlRelationType").val());
        var purchase = '';
        if (relationType == 2) {
            purchase = $.trim($("#LowerCompany").val());
        }
        if (relationType ==4) {
            purchase = $.trim($("#SubordinateCompany").val());
        }
        var processorAccount = $.trim($("#ddlEmployee").val());
        var speacialType = $.trim($("#ddlSpecialType").val());
        var providerFinancialCondition = startDate + ',' + endDate + ',' + payStartDate + ',' + payEndDate + ','
                                             + ticketNo + ',' + passenger + ',' + departure + ',' + arrival + ',' + titcketStatus + ',' + policyType + ','
                                              + airlines + ',' + tiketType + ',' + orderId + ',' + companyId + ',' + officeNo + ',' + relationType + ',' + purchase + ',' + processorAccount + ',' + speacialType;
        window.open('ReportDownload.aspx?providerFinancialCondition=' + providerFinancialCondition, 'newwindow');
    });
    SaveDefaultData(null, '.text-s1');

    showOrHide();
    specialTypeShowOrHide();
    seniorConditionShowOrHide();
    $("#ddlRelationType").change(function () {
        showOrHide();
    });
    $("#ddlPolicyType").change(function () {
        specialTypeShowOrHide();
    });
    $("#btnSeniorCondition").click(function () {
        if ($("#btnSeniorCondition").val() == "更多条件") {
            $("#seniorCondition").css("display", "");
            $(".reset").css("display", "");
            $(".resetName").css("display", "");
            $("#ddlPolicyType").css("width", "160px");
            $("#hfdSeniorCondition").val("show");
        }
        if ($("#btnSeniorCondition").val() == "简化条件") {
            $("#seniorCondition").css("display", "none");
            $(".reset").css("display", "none");
            $(".resetName").css("display", "none");
            $(".specialType").css("display", "none");
            $("#hfdSeniorCondition").val("hide");
        }
        seniorConditionShowOrHide();
    });
})
function valiate() {
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
    if (!valiatePassenger($("#txtPassenger"))) {
        alert("乘机人位数不能超过25位！");
        $("#txtPassenger").select();
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
        $("#SubordinateCompany").hide();
        $("#LowerCompany").hide();
    }
}
function specialTypeShowOrHide() {
    if ($("#ddlPolicyType").val() == "2") {
        $("#ddlSpecialType").show();
        $("#ddlPolicyType").css("width", "80px");
    } else {
        $("#ddlSpecialType").hide();
        $("#ddlPolicyType").css("width", "160px");
    }
}
function showOrHide() {
    if ($("#ddlRelationType").val() == 2) {
        $("#SubordinateCompany").hide();
        $("#LowerCompany").show();
        $("#LowerCompany").width("110px");
        $("#ddlRelationType").width("68px");
    } else {
        if ($("#ddlRelationType").val() == 4) {
            $("#LowerCompany").hide();
            $("#SubordinateCompany").show();
            $("#SubordinateCompany").width("110px");
            $("#ddlRelationType").width("68px");
        } else {
            $("#LowerCompany").hide();
            $("#SubordinateCompany").hide();
            $("#ddlRelationType").width("178px");
        }
    }
}
