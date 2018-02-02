function vailNotchPolicy() {
    if ($("#ddlCustomCode").size() != 0 && $("#ddlCustomCode option:selected").val() == "") {
        alert("自定义编号不能为空，请选择一个自定义编号！");
        return false;
    }
    if ($("#dropOffice option:selected").val() == "") {
        alert("OFFICE号不能为空，请选择一个OFFICE号！");
        return false;
    }
    if (($("input[type='radio'][name='DepartureFilght']:checked").val() == "1" || $("input[type='radio'] [name='DepartureFilght']:checked").val() == "2") && $("#txtDepartrueFilght").val() == "") {
        alert("航班限制不能为空，请填写航班限制!");
        return false;
    }

    if ($("#txtDrawerCondition").val().length > 200) {
        alert("出票条件不能超过200个字！");
        $("#txtDrawerCondition").val($("#txtDrawerCondition").val().substring(0, 200));
        return false;
    }
    if ($("#txtRemark").val().length > 200) {
        alert("备注信息不能超过200个字！");
        $("#txtRemark").val($("#txtRemark").val().substring(0, 200));
        return false;
    }


    var policyDepartureFilghtDataStart = $("#txtDepartrueStart").val();
    var policyDepartureFilghtDataEnd = $("#txtDepartrueEnd").val();

    var policyStartPrintDate = $("#txtProvideDate").val();

    var departureDateFilter = $("#txtPaiChu").val();

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
    var InternalCommission = "";
    var SubordinateCommission = "";
    var ProfessionCommission = "";
    var Berths = "";
    var reg = /^[0-9]{1,10}(\.[0-9])?$/;
    if ($("#neibufanyong").size() == 0) {
        InternalCommission = 0;
    } else {
        InternalCommission = $("#txtInternalCommission").val();
    }
    if (!reg.test(InternalCommission) || parseInt(InternalCommission) > 100) {
        alert("政策的返佣信息不能为空，且必须是100以内的数字（不包含100）!");
        return false;
    }
    SubordinateCommission = $("#txtSubordinateCommission").val();
    if (!reg.test(SubordinateCommission) || parseInt(SubordinateCommission) > 100) {
        alert("政策的返佣信息不能为空，且必须是100以内的数字（不包含100）!");
        return false;
    }
    if ($("#tonghang").size() == 0) {
        ProfessionCommission = 0;
    } else {
        ProfessionCommission = $("#txtProfessionCommission").val();
    }
    if (!reg.test(ProfessionCommission) || parseInt(ProfessionCommission) > 100) {
        alert("政策的返佣信息不能为空，且必须是100以内的数字（不包含100）!");
        return false;
    }

    for (g = 0; g < $("#Bunks input[type='checkbox']:checked").length; g++) {
        if (g > 0) {
            Berths += ",";
        }
        Berths += $("#Bunks input[type='checkbox']:checked").eq(g).val();
    }
    if (Berths == "") {
        alert("政策的舱位不能为空，请选择一个舱位!");
        return false;
    }
    if ($.trim($("#inputTxtvalue").val()) == "") {
        return confirm("请注意,你还没有填写任何出发到达限制，本政策将适用所有缺口程。是否需要继续？");
    }
    return true;
} //验证日期
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