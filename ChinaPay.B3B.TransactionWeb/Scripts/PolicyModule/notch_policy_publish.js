
function vailNotchPolicy(notch) {
    if ($(".zidingyi").css("display") != "none" && $("#selOfficeTd").css("display") != "none") {
        if ($("#selZidingy option:selected").val() == "") {
            alert("自定义编号不能为空，请选择一个自定义编号！");
            return false;
        }
    }
    if ($("#selOfficeTd").css("display") != "none") {
        if ($("#selOffice option:selected").val() == "") {
            alert("OFFICE号不能为空，请选择一个OFFICE号！");
            return false;
        }
    }
    if (($("input[type='radio'][name='QuchengFilght']:checked").val() == "1" || $("input[type='radio'] [name='QuchengFilght']:checked").val() == "2") && $("#txtQuChengFilght").val() == "") {
        alert("航班限制不能为空，请填写航班限制!");
        return false;
    }

    if (notch.notch.DrawerCondition.length > 200) {
        alert("出票条件不能超过200个字！");
        $("#txtDrawerCondition").val($("#txtDrawerCondition").val().substring(0, 200));
        return false;
    }
    if (notch.notch.Remark.length > 200) {
        alert("备注信息不能超过200个字！");
        $("#txtRemark").val($("#txtRemark").val().substring(0, 200));
        return false;
    }


    var policyDepartureFilghtDataStart = $(".quchengkaishi").val();
    var policyDepartureFilghtDataEnd = $(".quchengjieshu").val();

    var policyStartPrintDate = $(".chupiao").val();

    var departureDateFilter = $(".paichuriqi").val();

    if (policyDepartureFilghtDataStart == "" || policyDepartureFilghtDataEnd == "" || policyStartPrintDate == "") {
        alert("政策的 航班日期，开始出票日期不能为空!");
        return false;
    }
    if (valiateDateTime(policyDepartureFilghtDataStart, policyDepartureFilghtDataEnd)) {
        alert("政策的航班日期范围有误！结束时间不能小于开始时间");
        return false;
    }
    if (valiateDateTime(policyStartPrintDate, policyDepartureFilghtDataEnd)) {
        alert("政策的出票时间不能大于去程的结束时间!");
        return false;
    }
    var filterFlag = true;
    var dCount = departureDateFilter.split(',');
    for (var l = 0; l < dCount.length; l++) {
        if (dCount[l].split('-').length == 2) {
            if (valiateDateTime(dCount[l].split('-')[0], dCount[l].split('-')[1])) {
                filterFlag = false;
                break;
            }
        }
    }
    if (!filterFlag) {
        alert("政策的排除日期范围有误！请确认");
        return false;
    }
    if ($(".shiyongbanqi input[type='checkbox']:checked").length == 0) {
        alert("政策的适用班期必须选择一个！");
        return false;
    }
    if (!($("#b2b").is(":checked")) && !($("#bsp").is(":checked"))) {
        alert("政策的返佣信息必须选择一个(B2B/BSP)！");
        return false;
    }
    var b2bInternalCommission = "";
    var b2bSubordinateCommission = "";
    var b2bProfessionCommission = "";
    var b2bBerths = "";
    var bspInternalCommission = "";
    var bspSubordinateCommission = "";
    var bspProfessionCommission = "";
    var bspBerths = "";
    var reg = /^[0-9]{1,10}(\.[0-9])?$/;
    if ($("#b2b").is(":checked")) {
        if ($(".canHaveSubordinate").css("display") == "none") {
            b2bInternalCommission = 0;
        } else {
            b2bInternalCommission = $(".b2bneibufanyong").val();
        }
        if (!reg.test(b2bInternalCommission) || parseInt(b2bInternalCommission) > 100) {
            alert("政策的返佣信息不能为空，且必须是100以内的数字（不包含100）!");
            return false;
        }
        b2bSubordinateCommission = $(".b2bxiajifanyong").val();

        if (!reg.test(b2bSubordinateCommission) || parseInt(b2bSubordinateCommission) > 100) {
            alert("政策的返佣信息不能为空，且必须是100以内的数字（不包含100）!");
            return false;
        }
        if ($(".allowBrotherPurchase").css("display") == "none") {
            b2bProfessionCommission = 0;
        } else {
            b2bProfessionCommission = $(".b2btonghangfanyong").val();
        }
        if (!reg.test(b2bProfessionCommission) || parseInt(b2bProfessionCommission) > 100) {
            alert("政策的返佣信息不能为空，且必须是100以内的数字（不包含100）!");
            return false;
        }

        for (g = 0; g < $(".b2bcangwei input[type='checkbox']:checked").length; g++) {
            if (g > 0) {
                b2bBerths += ",";
            }
            b2bBerths += $(".b2bcangwei input[type='checkbox']:checked").eq(g).val();
        }
        if (b2bBerths == "") {
            alert("政策的舱位不能为空，请选择一个舱位!");
            return false;
        }
    }
    if ($("#bsp").is(":checked")) {
        if ($(".canHaveSubordinate").css("display") == "none") {
            bspInternalCommission = 0;
        } else {
            bspInternalCommission = $(".bspneibufanyong").val();
        }
        if (!reg.test(bspInternalCommission) || parseInt(bspInternalCommission) > 100) {
            alert("政策的返佣信息不能为空，且必须是100以内的数字（不包含100）!");
            return false;
        }
        bspSubordinateCommission = $(".bspxiajifanyong").val();
        if (!reg.test(bspSubordinateCommission) || parseInt(bspSubordinateCommission) > 100) {
            alert("政策的返佣信息不能为空，且必须是100以内的数字（不包含100）!");
            return false;
        }
        if ($(".allowBrotherPurchase").css("display") == "none") {
            bspProfessionCommission = 0;
        } else {
            bspProfessionCommission = $(".bsptonghangfanyong").val();
        }

        if (!reg.test(bspProfessionCommission) || parseInt(bspProfessionCommission) > 100) {
            alert("政策的返佣信息不能为空，且必须是100以内的数字（不包含100）!");
            return false;
        }
        for (g = 0; g < $(".bspcangwei input[type='checkbox']:checked").length; g++) {
            if (g > 0) {
                bspBerths += ",";
            }
            bspBerths += $(".bspcangwei input[type='checkbox']:checked").eq(g).val();
        }
        if (bspBerths == "") {
            alert("政策的舱位不能为空，请选择一个舱位!");
            return false;
        }
    }
    if (notch.notch.DepartureArrival.length == 0) {
        return confirm("请注意,你还没有填写任何出发到达限制，本政策将适用所有缺口程。是否需要发布？");
    }
    return true;
}
function GetPolicy(parame) {
    var policyArrayBase;
    var policyArrayGroup = new Array();
    //出发达到
    var departureArrival = new Array();

    var airline = $("#selProvince").val();
    var office = $("#selOffice").val();
    var impowerOffice = $("#selOffice option:selected").attr("impower");
    var customCode = $.trim($("#selZidingy option:selected").val()) == "0" ? "" : $("#selZidingy option:selected").val();
    var isInternal = $(".canHaveSubordinate").css("display") != "none";
    var isPeer = $(".allowBrotherPurchase").css("display") != "none";
    var tripType = "Notch";

    var val = $("#inputTxtvalue").html();
    if ($.trim(val) != "") {
        var values = val.split(',');
        for (var i = 0; i < values.length; i++) {
            var ite = values[i].split('|');
            if (ite.length == 3) {
                departureArrival.push({ "IsAllowable": (ite[0] == "1"), "Departure": ite[1], "Arrival": ite[2] });
            }
        }
    }
    


    //航班限制类型
    var departureFilghtType = "";
    if ($("input[name='QuchengFilght']:checked").val() == "0") {
        departureFilghtType = "None";
    }
    if ($("input[name='QuchengFilght']:checked").val() == "1") {
        departureFilghtType = "Include";
    }
    if ($("input[name='QuchengFilght']:checked").val() == "2") {
        departureFilghtType = "Exclude";
    }
    //去程航班
    var departureFilght = $("#txtQuChengFilght").val();
    //备注
    var remark = $("#txtRemark").val();
    //出票条件
    var drawerCondition = $("#txtDrawerCondition").val();

    var policyDepartureFilghtDataStart = $(".quchengkaishi").val();
    var policyDepartureFilghtDataEnd = $(".quchengjieshu").val();

    var policyStartPrintDate = $(".chupiao").val();

    var departureDateFilter = $(".paichuriqi").val();
    var departureWeekFilter = "";
    for (var s = 0; s < $(".shiyongbanqi input[type='checkbox']:checked").length; s++) {
        if (departureWeekFilter != "") {
            departureWeekFilter += ",";
        }
        departureWeekFilter += $(".shiyongbanqi input[type='checkbox']:checked").eq(s).val();
    }
    var autoAudit = $("#zjsh").is(":checked");
    var changePnr = $("#hbmcp").is(":checked");
    var printBeforeTwoHours = false;
    //返佣信息
    var b2bInternalCommission = "";
    var b2bSubordinateCommission = "";
    var b2bProfessionCommission = "";
    var b2bBerths = "";
    var bspInternalCommission = "";
    var bspSubordinateCommission = "";
    var bspProfessionCommission = "";
    var bspBerths = "";

    if ($("#b2b").is(":checked")) {
        printBeforeTwoHours = $("#qfqcp").is(":checked");
        if ($(".canHaveSubordinate").css("display") == "none") {
            b2bInternalCommission = 0;
        } else {
            b2bInternalCommission = $(".b2bneibufanyong").val();
        }
        b2bSubordinateCommission = $(".b2bxiajifanyong").val();
        if ($(".allowBrotherPurchase").css("display") == "none") {
            b2bProfessionCommission = 0;
        } else {
            b2bProfessionCommission = $(".b2btonghangfanyong").val();
        }

        for (g = 0; g < $(".b2bcangwei input[type='checkbox']:checked").length; g++) {
            if (g > 0) {
                b2bBerths += ",";
            }
            b2bBerths += $(".b2bcangwei input[type='checkbox']:checked").eq(g).val();
        }
        policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "StartPrintDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "Berths": b2bBerths, "TicketType": "B2B", "InternalCommission": (parseFloat(b2bInternalCommission)), "SubordinateCommission": (parseFloat(b2bSubordinateCommission)), "ProfessionCommission": (parseFloat(b2bProfessionCommission)), "ChangePNR": changePnr, "AutoAudit": autoAudit, "PrintBeforeTwoHours": printBeforeTwoHours });

    }
    if ($("#bsp").is(":checked")) {
        printBeforeTwoHours = false;

        if ($(".canHaveSubordinate").css("display") == "none") {
            bspInternalCommission = 0;
        } else {
            bspInternalCommission = $(".bspneibufanyong").val();
        }
        bspSubordinateCommission = $(".bspxiajifanyong").val();
        if ($(".allowBrotherPurchase").css("display") == "none") {
            bspProfessionCommission = 0;
        } else {
            bspProfessionCommission = $(".bsptonghangfanyong").val();
        }

        for (g = 0; g < $(".bspcangwei input[type='checkbox']:checked").length; g++) {
            if (g > 0) {
                bspBerths += ",";
            }
            bspBerths += $(".bspcangwei input[type='checkbox']:checked").eq(g).val();
        }
        policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "StartPrintDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "Berths": bspBerths, "TicketType": "BSP", "InternalCommission": (parseFloat(bspInternalCommission)), "SubordinateCommission": (parseFloat(bspSubordinateCommission)), "ProfessionCommission": (parseFloat(bspProfessionCommission)), "ChangePNR": changePnr, "AutoAudit": autoAudit, "PrintBeforeTwoHours": printBeforeTwoHours });
    }
    policyArrayBase = { "notch": { "Airline": airline, "OfficeCode": office, "ImpowerOffice": impowerOffice, "IsInternal": isInternal, "IsPeer": isPeer, "CustomCode": customCode, "VoyageType": tripType, "DepartureFlightsFilterType": departureFilghtType, "DepartureFlightsFilter": departureFilght, "Remark": remark, "DrawerCondition": drawerCondition, "DepartureArrival": departureArrival, "RebateInfo": policyArrayGroup} };
    if (vailNotchPolicy(policyArrayBase)) {
        //发布政策
        var actionUrl = "/PolicyHandlers/RoleGeneralPolicy.ashx/RegisterNotchPolicy";

        sendPostRequest(actionUrl, JSON.stringify(policyArrayBase), function (e) {
            if (e == true) {
                alert("添加成功");
                if (parame != "jixu") {
                    window.location.href = "./notch_policy_manage.aspx";
                }
            } else {
                alert("添加失败");
            }
        }, function (e) {
            alert(e.responseText);
        });
    }
}