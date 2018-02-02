
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
function VaildateRefreshBunksTime(k) {
    var policyDepartureFilghtDataStart = $(".parent_div").eq(k).find("input[type='text']").eq(0).val();
    var policyDepartureFilghtDataEnd = $(".parent_div").eq(k).find("input[type='text']").eq(1).val();
    var policyStartPrintDate = $(".parent_div").eq(k).find("input[type='text']").eq(2).val();
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



function Vaildate() {
    if ($("#selOfficeTd").css("display") != "none") {
        if ($("#selOffice option:selected").val() == "") {
            alert("OFFICE号不能为空，请选择一个OFFICE号！");
            return false;
        }
    }
    if ($(".zidingyi").css("display") != "none" && $("#selOfficeTd").css("display") != "none") {
        if ($("#selZidingy option:selected").val() == "") {
            alert("自定义编号不能为空，请选择一个自定义编号！");
            return false;
        }
    }
    if (($("input[type='radio'][name='QuchengFilght']:checked").val() == "1" || $("input[type='radio'] [name='QuchengFilght']:checked").val() == "2") && $("#txtQuChengFilght").val() == "") {
        alert("航班限制不能为空，请填写航班限制!");
        return false;
    }
    return true;
}

$(function () {
    $("#txtQuChengFilght").keydown(function () {
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
    }); $("#txtOutWithFilght").keydown(function () {
        if ((65 <= event.keyCode && event.keyCode <= 90)
                || event.keyCode == 8 || event.keyCode == 46
                || (96 <= event.keyCode && event.keyCode <= 105)
                || (event.keyCode == 106 && !event.shiftKey)
                || (event.keyCode == 111 && !event.shiftKey)
                || (event.keyCode == 191 && !event.shiftKey) ) {
            return true;
        } else {
            return false;
        }
    });
});