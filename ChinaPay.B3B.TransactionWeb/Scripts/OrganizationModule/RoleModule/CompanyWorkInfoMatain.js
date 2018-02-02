$(function () {
    $("#companyDrawerCondition").click(function () {
        BindDrawdition();
        $("#hfdDreawId").val("");
        $("#txtDreaw").val("");
        $("#txtDreawContext").val("");
    });
    var m_StartTime = ":00";
    var m_StopTime = ":59";
    $("#btnSaveManager").click(function () {
        var isflag = true;
        var reg = /^[a-zA-z\u4e00-\uf95a]{1,8}$/;
        var phone = /^1[3458]\d{9}$/;
        var qq = /^\d{5,12}$/;
        var list = new Array();
        if ($("#divProviderPerson").length > 0) {
            $("#divProviderPerson table tr").each(function (index) {
                if ((index != 0) && !(reg.test($("td:nth-child(2) input:text", this).val()))) {
                    $("td:nth-child(2) input:text", this).focus().select();
                    alert("负责人格式错误");
                    isflag = false;
                    return false;
                }
                if ((index != 0) && !(phone.test($("td:nth-child(3) input:text", this).val()))) {
                    $("td:nth-child(3) input:text", this).focus().select();
                    alert("负责人手机格式错误");
                    isflag = false;
                    return false;
                }
                if ((index != 0) && !(qq.test($("td:nth-child(4) input:text", this).val()))) {
                    $("td:nth-child(4) input:text", this).focus().select();
                    alert("负责人QQ格式错误");
                    isflag = false;
                    return false;
                }
            });
            if (isflag) {
                list.push({ "BusinessName": "出票", "Mamanger": $("#txtDrawerPerson").val(), "Cellphone": $("#txtDrawerCellPhone").val(), "QQ": $("#txtDrawerQQ").val() });
                list.push({ "BusinessName": "退票", "Mamanger": $("#txtRetreatPerson").val(), "Cellphone": $("#txtRetreatCellPhone").val(), "QQ": $("#txtRetreatQQ").val() });
                list.push({ "BusinessName": "废票", "Mamanger": $("#txtWastePerson").val(), "Cellphone": $("#txtWasteCellPhone").val(), "QQ": $("#txtWasteQQ").val() });
                list.push({ "BusinessName": "改期", "Mamanger": $("#txtReschedulingPerson").val(), "Cellphone": $("#txtReschedulingCellPhoen").val(), "QQ": $("#txtReschedulingQQ").val() });
                var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/UpdatePerson";
                var parameters = JSON.stringify({ businesslist: list });
                sendPostRequest(targetUrl, parameters, function (e) {
                    if (e == true) {
                        alert("保存成功");
                        BindPersons();
                    } else {
                        alert("保存失败，请重试");
                    }
                }, function (e) { alert(JSON.parse(e.responseText)); });
            }
        }
    });
    //工作时间
    $("#btnSaveRefundTime").click(function () {
        var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/UpdateRefundTime";
        var parameters = JSON.stringify({
            workdayWorkStart: $("#txtWorkdayWorkStart").val() + m_StartTime,
            workdayWorkEnd: $("#txtWorkdayWorkEnd").val() + m_StopTime,
            restdayWorkStart: $("#txtRestdayWorkStart").val() + m_StartTime,
            restdayWorkEnd: $("#txtRestdayWorkEnd").val() + m_StopTime,
            workdayRefundStart: $("#txtWorkdayRefundStart").val() + m_StartTime,
            workdayRefundEnd: $("#txtWorkdayRefundEnd").val() + m_StopTime,
            restdayRefundStart: $("#txtRestdayRefundStart").val() + m_StartTime,
            restdayRefundEnd: $("#txtRestdayRefundEnd").val() + m_StopTime
        });
        sendPostRequest(targetUrl, parameters, function (e) {
            if (e == true) {
                alert("保存成功");
                BindPersons();
            } else {
                alert("保存失败，请重试");
            }
        }, function (e) {
            alert(JSON.parse(e.responseText));
        });
    });
    $("#chkChildern").click(function () {
        if ($("#chkChildern").is(":checked")) {
            $("#lblChildern").show();
        } else {
            $("#lblChildern").hide();
        }
    });
    if ($("#chkChildern").is(":checked")) {
        $("#lblChildern").show();
    } else {
        $("#lblChildern").hide();
    }

    //默认返佣
    defaultAirlinesShowOrHide();
    $("#chkDefaultCommission").click(function () {
        defaultAirlinesShowOrHide();
    });

    //公司信息
    $("#btnProvider").click(function () {
        var arrival = $("#Departure_txtAirport").val().toLocaleUpperCase(), departure = $("#Arrival_txtAirport").val().toLocaleUpperCase();
        var officeno = "";
        if (arrival.length > 0 && departure.length > 0) {
            if (arrival == departure || $("#Departure_ddlAirports").val() == $("#Arrival_ddlAirports").val()) {
                alert("到达城市不能与出发城市一样");
                return;
            }
        }
        if ($("#ddlOffice").val() == "0") {
            alert("请选择一个Office号");
            return;
        }
        if ($("#ddlOffice").val() != null) {
            officeno = $("#ddlOffice").val();
        }
        var reg = /^\d{1,2}(\.\d{1})?$/;
        if ($("#chkChildern").is(":checked") && !reg.test($("#txtCholdrenDeduction").val())) {
            alert("儿童返点格式错误,只支持保留一位小数");
            return;
        }
        if ($("#chkChildern").is(":checked") && parseFloat($("#txtCholdrenDeduction").val()) <= 0) {
            alert("儿童返点不能为0%");
            return;
        }
        if ($("#chkChildern").is(":checked") && $("#chklCholdrenDeduction :checkbox:checked").length < 1) {
            alert("请选择可发布儿童政策的航空公司");
            return;
        }


//        if ($("#chkDefaultCommission").is(":checked") && !reg.test($("#txtDefaultCommission").val())) {
//            alert("默认返点格式错误,只支持保留一位小数");
//            return;
//        }
//        if ($("#chkDefaultCommission").is(":checked") && parseFloat($("#txtDefaultCommission").val()) <= 0) {
//            alert("儿童返点不能为0%");
//            return;
//        }
//        if ($("#chkDefaultCommission").is(":checked") && $("#divDefaultAirlines :checkbox:checked").length < 1) {
//            alert("请选择允许内部及下级采购的航空");
//            return;
//        }
        var airlineForChild = "";
        var rebateForChild = null;
        if ($("#chkChildern").is(":checked")) {
            rebateForChild = parseFloat($("#txtCholdrenDeduction").val()) / 100;
        }
        if ($("#chkChildern").is(":checked")) {
            for (var i = 0; i < $("#chklCholdrenDeduction :checkbox:checked").length; i++) {
                if (airlineForChild != "") {
                    airlineForChild += "/";
                }
                airlineForChild += $("#chklCholdrenDeduction :checkbox:checked").eq(i).val();
            }
        }

//        var airlineForDefault = "";
//        var rebateForDefault = null;
//        if ($("#chkDefaultCommission").is(":checked")) {
//            rebateForDefault = parseFloat($("#txtDefaultCommission").val()) / 100;
//        }
//        if ($("#chkDefaultCommission").is(":checked")) {
//            for (var i = 0; i < $("#divDefaultAirlines :checkbox:checked").length; i++) {
//                if (airlineForDefault != "") {
//                    airlineForDefault += "/";
//                }
//                airlineForDefault += $("#divDefaultAirlines :checkbox:checked").eq(i).val();
//            }
//        }

        var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/UpdateChilder";
        var parameters = JSON.stringify({
            workingSetting: {
                DefaultOfficeNumber: officeno,
                DefaultDeparture: arrival,
                DefaultArrival: departure,
                RefundNeedAudit: $("#chkRefundFinancialAudit").is(":checked"),
                RebateForChild: rebateForChild,
                AirlineForChild: airlineForChild,
                IsImpower: $("#chkEmpowermentOffice").is(":checked")
            }
        });
        sendPostRequest(targetUrl, parameters, function (e) {
            if (e == true) {
                BindPersons();
                var enabled = $("#chkEmpowermentOffice").is(":checked");
                $("#hfdEmpowermentOffice").val(String(enabled).toLocaleUpperCase());
                QueryCustomNumber();
                alert("保存成功");
            } else {
                alert("保存失败，请重试");
            }
        }, function (e) { alert(JSON.parse(e.responseText)); });
    });
});

function BindPersons() {
    var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/BindPerson";
    sendPostRequest(targetUrl, null, function (e) {
        $.each(eval(e), function (i, item) {
            if (item.BusinessName == "出票") { $("#txtDrawerPerson").val(item.Mamanger); $("#txtDrawerCellPhone").val(item.Cellphone); $("#txtDrawerQQ").val(item.QQ); }
            if (item.BusinessName == "退票") { $("#txtRetreatPerson").val(item.Mamanger); $("#txtRetreatCellPhone").val(item.Cellphone); $("#txtRetreatQQ").val(item.QQ); }
            if (item.BusinessName == "废票") { $("#txtWastePerson").val(item.Mamanger); $("#txtWasteCellPhone").val(item.Cellphone); $("#txtWasteQQ").val(item.QQ); }
            if (item.BusinessName == "改期") { $("#txtReschedulingPerson").val(item.Mamanger); $("#txtReschedulingCellPhoen").val(item.Cellphone); $("#txtReschedulingQQ").val(item.QQ); }
        });
    }, function () {

    });
}
//获取工作时间
function BindWorkingHours() {
    var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/BindWorkingHours";
    sendPostRequest(targetUrl, null, function (result) {
        $("#txtWorkdayWorkStart").val(result.workstarttime);
        $("#txtWorkdayWorkEnd").val(result.workendtime);
        $("#txtRestdayWorkStart").val(result.workstartweektime);
        $("#txtRestdayWorkEnd").val(result.workendweektime);
        $("#txtWorkdayRefundStart").val(result.refundstarttime);
        $("#txtWorkdayRefundEnd").val(result.refundendtime);
        $("#txtRestdayRefundStart").val(result.refundstartweektime);
        $("#txtRestdayRefundEnd").val(result.refundendweektime);
    }, function () {

    });
}
function BindselOffice() {
    var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/BindOffice";
    sendPostRequest(targetUrl, null, function (result) {
        $("#ddlOffice option").remove();
        var str = "<option value='0'>-请选择-</option>";
        $.each(eval(result), function (i, item) {
            if (item.office == item.defaultOffice) {
                str += "<option value='" + item.office + "' selected='selected'>" + item.office + "</option>";
            } else {
                str += "<option value='" + item.office + "'>" + item.office + "</option>";
            }
        });
        $("#ddlOffice").append(str);
    }, function () {
    });
}
function BindDrawdition() {
    var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/QueryDrawditions";
    sendPostRequest(targetUrl, null, function (result) {
        $("#tbDrawerCondition table").remove();
        var str = "<table>";
        str += "<tr><th>序号</th><th>类型</th><th>内容</th><th>操作</th></tr>";
        $.each(eval(result), function (i, item) {
            str += "<tr><td>" + (i + 1) + "</td><td style='width:100px;'>" + (item.Type == 0 ? "出票条件" : "政策备注") + "</td><td> " + item.Context + " </td><td style='width:100px;'> <a href='javascript:UpdateDition(\"" + item.Id + "\",\"" + item.Type + "\",\"" + item.Title + "\",\"" + item.Context + "\");' class='update'>修改</a>&nbsp;&nbsp;&nbsp;<a href='javascript:DelDition(\"" + item.Id + "\")'>删除</a> </td></tr>";
        }); str += "</table>";
        $("#tbDrawerCondition").append(str); 
    }, function () {
    });
}
function defaultAirlinesShowOrHide() {
    if ($("#chkDefaultCommission").is(":checked")) {
        $("#spanDefaultCommission").show();
    } else {
        $("#spanDefaultCommission").hide();
    }
}