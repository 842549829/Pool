function Vaildate_Base() {
    if ($("#txtAirports").val() == "") {
        alert("出发地不能为空，请选择至少一个城市作为出发地!");
        return false;
    }
    if ($("input[name='radDepartureRange']:checked").val() == 1 && $("#lbSource option").length == 0) {
        alert("出发地不能为空，请选择至少一个城市作为出发地!");
        return false;
    }
    if ($("#txtAirports1").val() == "") {
        if ($(".classDisplay").css("display") == "none") {
            alert("目的地不能为空，请选择至少一个城市作为目的地!");
        } else {
            alert("中转地不能为空，请选择至少一个城市作为中转地!");
        }
        return false;
    }
    if ($("input[name='radInculd1']:checked").val() == 1 && $("#lbSource1 option").length == 0) {
        if ($(".classDisplay").css("display") == "none") {
            alert("目的地不能为空，请选择至少一个城市作为目的地!");
        } else {
            alert("中转地不能为空，请选择至少一个城市作为中转地!");
        }
        return false;
    }
    if ($(".classDisplay").css("display") != "none") {
        if ($("#txtAirports2").val() == "") {
            alert("目的地不能为空，请选择至少一个城市作为目的地!");
            return false;
        }
        if ($("input[name='radInculd2']:checked").val() == 1 && $("#lbSource2 option").length == 0) {
            alert("目的地不能为空，请选择至少一个城市作为目的地!");
            return false;
        }
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
    var flag = true;
    var bunksFlag = true;
    var filghtFlag = true;
    var roundFlag = true;
    var quchengFlag = true;
    var maxMission = true;
    var he = true;
    var groupIndex = 0;
    var banqishiyong = true;
    var filterFlag = true;
    var reg = /^[0-9]{1,10}(\.[0-9])?$/;
    for (var k = 0; k < $(".parent_div").length; k++) {
        groupIndex = parseInt(k) + 1;
        var policyDepartureFilghtDataStart = $(".parent_div").eq(k).find("input[type='text']").eq(0).val();
        var policyDepartureFilghtDataEnd = $(".parent_div").eq(k).find("input[type='text']").eq(1).val();
        var policyStartPrintDate = $(".parent_div").eq(k).find("input[type='text']").eq(2).val();
        var departureDateFilter = $(".parent_div").eq(k).find("input[type='text']").eq(3).val();
        var zhidingValue = $(".parent_div").eq(k).find(".zhidingcangwei input[type='radio']:checked").val();
        if (policyDepartureFilghtDataStart == "" || policyDepartureFilghtDataEnd == "" || policyStartPrintDate == "") {
            filghtFlag = false;
            break;
        }
        if (valiateDateTime(policyDepartureFilghtDataStart, policyDepartureFilghtDataEnd)) {
            quchengFlag = false;
            break;
        }
        if (valiateDateTime(policyStartPrintDate, policyDepartureFilghtDataEnd)) {
            roundFlag = false;
            break;
        }

        var dCount = departureDateFilter.split(',');
        for (var l = 0; l < dCount.length; l++) {
            if (dCount[l].split('-').length == 2) {
                if (valiateDateTime(dCount[l].split('-')[0], dCount[l].split('-')[1])) {
                    filterFlag = false;
                    break;
                }
            }
        }
        if ($(".parent_div").eq(k).find(".pd_left input[type='checkbox']:checked").length == 0) {
            banqishiyong = false;
            break;
        }

        if ($(".parent_div").eq(k).find(".groupBox2 table tr td[class='B2B']>input[type='checkbox']").is(":checked")) {
            if ($(".canHaveSubordinate").css("display") != "none") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val() == "") {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val() != "") {
                    if (!reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val())) {
                        flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val() > 100) {
                        maxMission = false;
                        break;
                    }
                }
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val() == "") {
                flag = false;
                break;
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val() != "") {
                if (!reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val())) {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val() > 100) {
                    maxMission = false;
                    break;
                }
            }
            if ($(".allowBrotherPurchase").css("display") != "none") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val() == "") {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val() != "") {
                    if (!reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val())) {
                        flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val() > 100) {
                        maxMission = false;
                        break;
                    }
                }
            }
            if (zhidingValue == "1") {
                if ($(".parent_div").eq(k).find(".groupBox2 .BunksRad").eq(1).find("input[type='checkbox']:checked").length == 0) {
                    bunksFlag = false;
                    break;
                }
            } else {
                if ($(".parent_div").eq(k).find(".groupBox2 .ZhidingBunks").eq(1).find("select option").length == 0) {
                    bunksFlag = false;
                    break;
                }
            }

        }
        if ($(".parent_div").eq(k).find(".groupBox2 table tr td[class='BSP']>input[type='checkbox']").is(":checked")) {
            if ($(".canHaveSubordinate").css("display") != "none") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val() == "") {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val() != "") {
                    if (!reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val())) {
                        flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val() > 100) {
                        maxMission = false;
                        break;
                    }
                }
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val() == "") {
                flag = false;
                break;
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val() != "") {
                if (!reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val())) {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val() > 100) {
                    maxMission = false;
                    break;
                }
            }
            if ($(".allowBrotherPurchase").css("display") != "none") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val() == "") {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val() != "") {
                    if (!reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val())) {
                        flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val() > 100) {
                        maxMission = false;
                        break;
                    }
                }
            }

            if (zhidingValue == "1") {
                if ($(".parent_div").eq(k).find(".groupBox2   .BunksRad").eq(0).find("input[type='checkbox']:checked").length == 0) {
                    bunksFlag = false;
                    break;
                }
            } else {
                if ($(".parent_div").eq(k).find(".groupBox2  .ZhidingBunks").eq(0).find("select option").length == 0) {
                    bunksFlag = false;
                    break;
                }
            }
        }
    }
    if (!filghtFlag) {
        alert("第 [ " + groupIndex + " ] 组政策的 航班日期，开始出票日期不能为空!");
        return false;
    }
    if (!quchengFlag) {
        alert("第 [ " + groupIndex + " ] 组政策的去程日期范围有误！结束时间不能小于开始时间");
        return false;
    }
    if (!banqishiyong) {
        alert("第 [ " + groupIndex + " ] 组政策的适用班期必须选择一个！");
        return false;
    }
    if (!roundFlag) {
        alert("第 [ " + groupIndex + " ] 组政策的出票时间不能大于去程的结束时间!");
        return false;
    }
    if (!flag) {
        alert("第 [ " + groupIndex + " ] 组政策的返佣信息不能为空，且必须是100以内的数字!");
        return false;
    }
    if (!bunksFlag) {
        alert("第 [ " + groupIndex + " ] 组政策的舱位必须选择一个!");
        return false;
    }
    if (!maxMission) {
        alert("第 [ " + groupIndex + " ] 组政策的返佣信息必须是100以内的数字!");
        return false;
    }
    if (!filterFlag) {
        alert("第 [ " + groupIndex + " ] 组政策的排除日期范围有误！请确认");
        return false;
    }

    return true;
}
