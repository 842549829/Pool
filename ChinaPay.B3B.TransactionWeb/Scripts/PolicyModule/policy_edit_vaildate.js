
function vaildate_bunks_time() {
    var policyDepartureFilghtDataStart = $("#txtDepartrueStart").val();
    var policyDepartureFilghtDataEnd = $("#txtDepartrueEnd").val();
    var policyStartPrintDate = $("#txtProvideDate").val();
    if (policyDepartureFilghtDataStart == "" || policyDepartureFilghtDataEnd == "" || policyStartPrintDate == "") {
        alert("去程航班日期，开始出票日期不能为空!");
        return false;
    }
    if (valiateDateTime(policyDepartureFilghtDataStart, policyDepartureFilghtDataEnd)) {
        alert("去程日期范围有误！结束时间不能小于开始时间");
        return false;
    }
    if (valiateDateTime(policyStartPrintDate, policyDepartureFilghtDataStart)) {
        alert("出票时间不能大于去程的开始时间!");
        return false;
    }
    return true;
}
//验证日期
function valiateDateTime(lowerDate, upperDate) {
    if ((lowerDate == "" || lowerDate == null) || (upperDate == "" || upperDate == null)) {
        return false;
    }
    else {
        var startDate = parseInt(lowerDate.replace(/-/g, ''), 10);
        var endDate = parseInt(upperDate.replace(/-/g, ''), 10);
        if (startDate > endDate) {
            return true;
        } else {
            return false;
        }
    }
}
function vaildate() {
    var reg = /^[0-9]{1,10}?$/;
    if ($("input[type='radio'][name='DepartureFilght']:checked").val() == "radYiXia" && $("#txtDepartrueFilght").val() == "") {
        alert("航班不能为空，请至少填写一个航班!");
        return false;
    }
    if ($("input[type='radio'][name='DepartureFilght']:checked").val() == "radBuYiXia" && $("#txtDepartrueFilght").val() == "") {
        alert("航班不能为空，请至少填写一个航班!");
        return false;
    }
    if ($("#txtPrice").val() == "" && $("#price0").css("display") != "none") {
        if ($.trim($("#titlePolicy").html()) != "往返") {
            alert("价格不能为空，请至少填写一个价格!");
            return false;
        }
    }
    if ($("#txtPrice").size() != 0 && $("#price0").css("display") != "none") {
        if ($("#txtPrice").val() != "" && !reg.test($("#txtPrice").val())) {
            alert("价格格式错误，只能输入整数!");
            return false;
        }
    }

    if ($("#txtDiscount").size() != 0 && $("#discount0").css("display") != "none") {
        if ($("#txtDiscount").val() != "" && !reg.test($("#txtDiscount").val())) {
            alert("折扣格式错误，只能输入整数!");
            return false;
        }
    }

    if ($("#txtPrice0").size() != 0) {
        if ($("#txtPrice0").val() == "") {
            alert("价格不能为空，请至少填写一个价格!");
            return false;
        }
        if ($("#txtPrice0").val() != "" && !reg.test($("#txtPrice0").val())) {
            alert("价格格式错误，只能输入整数!");
            return false;
        }
        //        if (parseInt($("#txtPrice0").val()) < 10) {
        //            alert("价格不能够小于10");
        //            return false;
        //        }
    }
    if ($(".groupLine").find("input[type='rdaio']").html() != null) {
        var resource = $("#txtResourceAmount").val();
        //判断是否是免票黑屏同步
        if ($("#selBunksSpan").css("display") == "none") {
            if (resource == "" && resource != null) {
                resource = "-1";
            }
            if (resource != "-1" && resource != null) {
                if (resource != "-1" && !reg.test(resource)) {
                    alert("提供资源张数格式错误，只能输入整数!");
                    return false;
                }
            }
        } else {
            if ($("#ddlBunks option").length == 0) {
                alert("舱位不能为空，请选择一个舱位");
                return false;
            }
        }
    } else {
        var resource = $("#txtResourceAmount").val();
        if (resource == "" && resource != null) {
            resource = "-1";
        }
        if (resource != "-1" && resource != null) {
            if (resource != "-1" && !reg.test(resource)) {
                alert("提供资源张数格式错误，只能输入整数!");
                return false;
            }
        }
    }

    if ($(".class_display").css("display") != "none") {
        if ($("input[type='radio'][name='ReturnFilght']:checked").val() == "radReturnYiXia" && $("#txtReturnFilght").val() == "") {
            alert("航班不能为空，请至少填写一个航段!");
            return false;
        }
        if ($("input[type='radio'][name='ReturnFilght']:checked").val() == "radReturnBuYiXia" && $("#txtReturnFilght").val() == "") {
            alert("航班不能为空，请至少填写一个航段!");
            return false;
        }
    }
    var tiqian = $("#txtTiQianDays").val();
    var mostBeforehandDays = $("#txtMostTiQianDays").val();
    if (tiqian == "") {
        tiqian = "0";
    }
    if (mostBeforehandDays == "") {
        mostBeforehandDays = "-1";
    }
    if ($(".groupBox1 .pd_left input[type='checkbox']:checked").length == 0) {
        alert("当前政策的适用班期必须选择一个！");
        return false;
    }
    var dCount = $("#txtPaiChu").val().split(',');
    var filterFlag = true;
    for (var l = 0; l < dCount.length; l++) {
        if (dCount[l].split('-').length == 2) {
            if (valiateDateTime(dCount[l].split('-')[0], dCount[l].split('-')[1])) {
                filterFlag = false;
                break;
            }
        }
    }
    if (!filterFlag) {
        alert("当前政策的排除日期范围有误！请确认");
        return false;
    }
    if (tiqian != null) {
        if (tiqian != "0" && !reg.test(tiqian)) {
            alert("最少提前天数格式输入错误，必须输入是3位以内整数!可为空");
            return false;
        }
    }
    if (mostBeforehandDays != null) {
        if (mostBeforehandDays != "-1" && !reg.test(mostBeforehandDays)) {
            alert("最多提前天数格式输入错误，必须输入是3位以内整数!可为空");
            return false;
        }
        if (mostBeforehandDays != "-1" && parseInt(tiqian) > parseInt(mostBeforehandDays)) {
            alert("最少提前天数不能大于最多提前天数");
            return false;
        }
    }
    if ($("input[type='radio'][name='tongbu']:checked").val() != null) {
        if ($("#hidBunks").val() == "" && $("input[type='radio'][name='tongbu']:checked").val() == "0") {
            alert("舱位不能为空，必须选择至少一个舱位!");
            return false;
        }
    } else {
        if ($("#hidBunks").val() == "") {
            alert("舱位不能为空，必须选择至少一个舱位!");
            return false;
        }
    }

    var pricePattern = /^[0-9]{1,10}$/;
    if ($("#txtPriceNeibu").val() == "") {
        alert("内部结算价格式错误，只能是整数，请填写");
        return false;
    }
    if ($("#txtPriceNeibu").size() != 0 && !pricePattern.test($("#txtPriceNeibu").val())) {
        alert("内部结算价格式错误，只能是整数，请填写");
        return false;
    }
    if ($("#txtPriceXiaji").val() == "") {
        alert("下级结算价格式错误，只能是整数，请填写");
        return false;
    }
    if ($("#txtPriceXiaji").size() != 0 && !pricePattern.test($("#txtPriceXiaji").val())) {
        alert("下级结算价格式错误，只能是整数，请填写");
        return false;
    }
    if ($("#txtPriceTonghang").val() == "") {
        alert("同行结算价格式错误，只能是整数，请填写");
        return false;
    }
    if ($("#txtPriceTonghang").size() != 0 && !pricePattern.test($("#txtPriceTonghang").val())) {
        alert("同行结算价格式错误，只能是整数，请填写");
        return false;
    }
    reg = /^[0-9]{1,2}(\.[0-9])?$/;
    var reg2 = /^[0-9]{1,5}(\.[0-9])?$/;
    if ($("#txtInternalCommission").val() != null && $("#txtInternalCommission").val() == "") {
        alert("内部佣金不能为空，必须填写一个整数或一位小数!");
        return false;
    }
    if ($("#txtInternalCommission").val() != null) {
        if ($("#selPrice").val() != null) {
            if ($("#selPrice").val() == "1" && $("#selPrice").css("display") != "none") {
                if ($("#txtInternalCommission").val() != "" && !reg.test($("#txtInternalCommission").val())) {
                    alert("内部佣金格式错误，必须填写一个整数或一位小数,100以内的数!");
                    return false;
                }
                if ($("#txtInternalCommission").val() > 100) {
                    alert("内部佣金不能大于100，必须是100以内的数!");
                    return false;
                }
            } else {
                if ($("#txtInternalCommission").val() != "" && !reg2.test($("#txtInternalCommission").val())) {
                    alert("内部佣金格式错误，必须填写一个整数或小数!");
                    return false;
                }
            }
        } else {
            if ($("#txtInternalCommission").val() != "" && !reg.test($("#txtInternalCommission").val())) {
                alert("内部佣金格式错误，必须填写一个整数或一位小数,100以内的数!");
                return false;
            }
            if ($("#txtInternalCommission").val() > 100) {
                alert("内部佣金不能大于100，必须是100以内的数!");
                return false;
            }
        }
    }
    if ($("#txtSubordinateCommission").val() != null && $("#txtSubordinateCommission").val() == "") {
        alert("下级佣金不能为空，必须填写一个整数或一位小数!");
        return false;
    }
    if ($("#txtSubordinateCommission").val() != null) {
        if ($("#selPrice").val() != null) {
            if ($("#selPrice").val() == "1" && $("#selPrice").css("display") != "none") {
                if ($("#txtSubordinateCommission").val() != "" && !reg.test($("#txtSubordinateCommission").val())) {
                    alert("下级佣金格式错误，必须填写一个整数或一位小数,100以内的数!");
                    return false;
                }
                if ($("#txtSubordinateCommission").val() > 100) {
                    alert("下级佣金不能大于100，必须是100以内的数!");
                    return false;
                }
            } else {
                if ($("#txtSubordinateCommission").val() != "" && !reg2.test($("#txtSubordinateCommission").val())) {
                    alert("下级佣金格式错误，必须填写一个整数或小数!");
                    return false;
                }
            }
        }
        else {
            if ($("#txtSubordinateCommission").val() != "" && !reg.test($("#txtSubordinateCommission").val())) {
                alert("下级佣金格式错误，必须填写一个整数或一位小数,100以内的数!");
                return false;
            }
            if ($("#txtSubordinateCommission").val() > 100) {
                alert("下级佣金不能大于100，必须是100以内的数!");
                return false;
            }
        }
    }
    if ($("#txtProfessionCommission").val() != null && $("#txtProfessionCommission").val() == "") {
        alert("同行佣金不能为空，必须填写一个整数或一位小数!");
        return false;
    }
    if ($("#txtProfessionCommission").val() != null) {
        if ($("#selPrice").val() != null) {
            if ($("#selPrice").val() == "1" && $("#selPrice").css("display") != "none") {
                if ($("#txtProfessionCommission").val() != "" && !reg.test($("#txtProfessionCommission").val())) {
                    alert("同行佣金格式错误，必须填写一个整数或一位小数,100以内的数!");
                    return false;
                }
                if ($("#txtProfessionCommission").val() > 100) {
                    alert("同行佣金不能大于100，必须是100以内的数!");
                    return false;
                }
            } else {
                if ($("#txtProfessionCommission").val() != "" && !reg2.test($("#txtProfessionCommission").val())) {
                    alert("同行佣金格式错误，必须填写一个整数或小数!");
                    return false;
                }
            }
        }
        else {
            if ($("#txtProfessionCommission").val() != "" && !reg.test($("#txtProfessionCommission").val())) {
                alert("同行佣金格式错误，必须填写一个整数或一位小数,100以内的数!");
                return false;
            }
            if ($("#txtProfessionCommission").val() > 100) {
                alert("同行佣金不能大于100，必须是100以内的数!");
                return false;
            }
        }
    }
    reg = /^[0-9]{1,5}?$/;
    if ($("#txtPriceNeibu").val() != null && $("#txtPriceNeibu").val() == "") {
        alert("内部结算价不能为空，必须填写5位以内的整数!");
        return false;
    }
    if ($("#txtPriceXiaji").val() != null && $("#txtPriceXiaji").val() == "") {
        alert("下级结算价不能为空，必须填写5位以内的整数!");
        return false;
    }
    if ($("#txtPriceTonghang").val() != null && $("#txtPriceTonghang").val() == "") {
        alert("同行结算价不能为空，必须填写5位以内的整数!");
        return false;
    }
    return true;
}

$(function () {
    if ($("#dropOffice option:selected").val() == "True") {
        $("#selOfficeTd span").html("需授权").removeClass("obvious3").addClass("obvious2");
    } else {
        $("#selOfficeTd span").html("无需授权").removeClass("obvious2").addClass("obvious3");
    }
    if ($("#hidOfficeNo").size() != 0) {
        $("#hidOfficeNo").val($("#dropOffice option:selected").text());
    }
    $("#dropOffice").change(function () {
        if ($("#hidOfficeNo").size() != 0) {
            $("#hidOfficeNo").val($("#dropOffice option:selected").text());
        }
        if ($("#dropOffice option:selected").val() == "True") {
            $("#selOfficeTd span").html("需授权").removeClass("obvious3").addClass("obvious2");
        } else {
            $("#selOfficeTd span").html("无需授权").removeClass("obvious2").addClass("obvious3");
        }
    });

    $("#duihuan").live("click", function () {
        var lbSourceText = $("#txtDepartureAirports_txtAirports").val();
        //        var lbSource = $("#txtDepartureAirports_lbSource").html();
        //        var lbSelected = $("#txtDepartureAirports_lbSelected").html();

        $("#txtDepartureAirports_txtAirports").val($("#txtArrivalAirports_txtAirports").val());
        //        $("#txtDepartureAirports_lbSource").html($("#txtArrivalAirports_lbSource").html());
        //        $("#txtDepartureAirports_lbSelected").html($("#txtArrivalAirports_lbSelected").html());

        $("#txtArrivalAirports_txtAirports").val(lbSourceText);
        //        $("#txtArrivalAirports_lbSource").html(lbSource);
        //        $("#txtArrivalAirports_lbSelected").html(lbSelected);
        $("#txtArrivalAirports_txtAirports").blur();
        $("#txtDepartureAirports_txtAirports").blur();

    });
    $("#txtDepartrueFilght").keydown(function () {
        if ((48 <= event.keyCode && event.keyCode <= 57)
                || event.keyCode == 8 || event.keyCode == 46
                || (96 <= event.keyCode && event.keyCode <= 105)
                || (event.keyCode == 106 && !event.shiftKey)
                || (event.keyCode == 111 && !event.shiftKey)
                || (event.keyCode == 191 && !event.shiftKey)
                || (event.keyCode == 56 && event.shiftKey)) {
            return true;
        } else {
            return false;
        }
    });
});