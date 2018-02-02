
function Vaildate_Base() {
    if ($("#mudidi").css("display") != "none") {
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
    } else {
        var one = $(".diaohuan").eq(0);
        var two = $(".diaohuan").eq(1);
        if (one.find("#txtWangfanAirports") != null) {
            if (one.find("#txtWangfanAirports").val() == "") {
                alert("出发地不能为空，请选择至少一个城市作为出发地!");
                return false;
            }
            if (two.find("#txtAirports2").val() == "") {
                alert("目的地不能为空，请选择至少一个城市作为目的地!");
                return false;
            }
            if (two.find("input[name='radInculd2']:checked").val() == 1 && two.find("#lbSource2 option").length == 0) {
                alert("目的地不能为空，请选择至少一个城市作为目的地!");
                return false;
            }
        }
        else {
            if (one.find("#txtAirports2").val() == "") {
                alert("出发地不能为空，请选择至少一个城市作为出发地!");
                return false;
            }
            if (one.find("input[name='radInculd2']:checked").val() == 1 && one.find("#lbSource2 option").length == 0) {
                alert("出发地不能为空，请选择至少一个城市作为出发地!");
                return false;
            }
            if (two.find("#txtWangfanAirports").val() == "") {
                alert("目的地不能为空，请选择至少一个城市作为目的地!");
                return false;
            }
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

    var reg = /^[0-9]{1,3}?$/;

    var before = true;
    var flag = true;
    var chuxing_flag = true;
    var tiqian_flag = true;
    var bunks_flag = true;
    var filght_flag = true;
    var round_flag = true;
    var qucheng_flag = true;
    var huicheng_flag = true;
    var rungStart_flag = true;
    var rungEnd_flag = true;
    var group_index = 0;
    var price_flag = true;
    var maxMission = true;
    var banqishiyong = true;
    var mostTiqian = true;
    var rundTiqian = true;
    var filterFlag = true;
    reg = /^[0-9]{1,3}(\.[0-9])?$/;
    var reg1 = /^[a-zA-Z]{1,10}?$/;
    var reg2 = /^[0-9]{1,10}?$/;
    var id = $(".navType2Selected").attr("id");

    for (var k = 0; k < $(".parent_div").length; k++) {
        group_index = parseInt(k) + 1;
        var PolicyDepartureFilghtDataStart = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(0).val();
        var PolicyDepartureFilghtDataEnd = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(1).val();
        var PolicyStartPrintDate = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(2).val();
        var departureDateFilter = $(".parent_div").eq(k).find("input[type='text']").eq(3).val();

        if (PolicyDepartureFilghtDataStart == "" || PolicyDepartureFilghtDataEnd == "" || PolicyStartPrintDate == "") {
            filght_flag = false;
            break;
        }
        if (valiateDateTime(PolicyDepartureFilghtDataStart, PolicyDepartureFilghtDataEnd)) {
            qucheng_flag = false;
            break;
        }
        if (valiateDateTime(PolicyStartPrintDate, PolicyDepartureFilghtDataEnd)) {
            round_flag = false;
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

        var beforehandDays = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val() == "" ? "0" : $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val();
        var mostBeforehandDays = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(5).val() == "" ? "-1" : $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(5).val();
        var chuxingDays = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(6).val();
        if (id == "OneWay") {
            if (beforehandDays == "") {
                tiqian_flag = false;
                break;
            }
            if (beforehandDays != "0" && !reg2.test(beforehandDays)) {
                tiqian_flag = false;
                break;
            }
            if (mostBeforehandDays == "") {
                mostTiqian = false;
                break;
            }
            if (mostBeforehandDays != "-1" && !reg2.test(mostBeforehandDays)) {
                mostTiqian = false;
                break;
            }
            if (mostBeforehandDays != "-1" && parseInt(beforehandDays) > parseInt(mostBeforehandDays)) {
                rundTiqian = false;
            }
        } else if (id == "RoundTrip") {
            if (beforehandDays == "") {
                tiqian_flag = false;
                break;
            }
            if (beforehandDays != "0" && !reg2.test(beforehandDays)) {
                tiqian_flag = false;
                break;
            }
            if (mostBeforehandDays == "") {
                mostTiqian = false;
                break;
            }
            if (mostBeforehandDays != "-1" && !reg2.test(mostBeforehandDays)) {
                mostTiqian = false;
                break;
            }
            if (mostBeforehandDays != "-1" && parseInt(beforehandDays) > parseInt(mostBeforehandDays)) {
                rundTiqian = false;
            }
            if (chuxingDays != "" && !reg2.test(chuxingDays)) {
                chuxing_flag = false;
                break;
            }
        }

        if ($(".parent_div").eq(k).find(".groupBox2 table tr .BSP >input[type='checkbox']").is(":checked")) {
            if ($(".parent_div").eq(k).find(".groupBox2 table tr td[class='BunksRad']").eq(0).find(".select").html() == null) {
                bunks_flag = false;
                break;
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr td[class='BunksRad']").eq(0).find(".select option").length == 0) {
                bunks_flag = false;
                break;
            }
            if (id == "OneWay") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".selectPrice").val() == "0") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val() == "") {
                        price_flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val() != "" && !reg2.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val())) {
                        price_flag = false;
                        break;
                    }
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".selectPrice").val() == "1") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val() == "") {
                        flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val() != "" && !reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val())) {
                        flag = false;
                        break;
                    }
                }
            } else if (id == "RoundTrip") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".selectPrice").val() == "0") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val() == "") {
                        price_flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val() != "" && !reg2.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val())) {
                        price_flag = false;
                        break;
                    }
                }
            }
            if ($(".canHaveSubordinate").css("display") != "none") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val() == "") {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val() != "" && !reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val())) {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val() > 100) {
                    maxMission = false;
                    break;
                }
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val() == "") {
                flag = false;
                break;
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val() != "" && !reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val())) {
                flag = false;
                break;
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val() > 100) {
                maxMission = false;
                break;
            }
            if ($(".allowBrotherPurchase").css("display") != "none") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val() == "") {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val() != "" && !reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val())) {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val() > 100) {
                    maxMission = false;
                    break;
                }
            }
        }
        if ($(".parent_div").eq(k).find(".groupBox2 table tr .B2B>input[type='checkbox']").is(":checked")) {
            if ($(".parent_div").eq(k).find(".groupBox2 table tr td[class='BunksRad']").eq(1).find(".select").html() == null) {
                bunks_flag = false;
                break;
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr td[class='BunksRad']").eq(1).find(".select option").length == 0) {
                bunks_flag = false;
                break;
            }
            if (id == "OneWay") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".selectPrice").val() == "0") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val() == "") {
                        price_flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val() != "" && !reg2.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val())) {
                        price_flag = false;
                        break;
                    }
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".selectPrice").val() == "1") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val() == "") {
                        price_flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val() != "" && !reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val())) {
                        price_flag = false;
                        break;
                    }
                }
            } else if (id == "RoundTrip") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".selectPrice").val() == "0") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val() == "") {
                        price_flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val() != "" && !reg2.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val())) {
                        price_flag = false;
                        break;
                    }
                } 
            }

            if ($(".canHaveSubordinate").css("display") != "none") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val() == "") {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val() != "" && !reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val())) {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val() > 100) {
                    maxMission = false;
                    break;
                }
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(3).val() == "") {
                flag = false;
                break;
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(3).val() != "" && !reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(3).val())) {
                flag = false;
                break;
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(3).val() > 100) {
                maxMission = false;
                break;
            }
            if ($(".allowBrotherPurchase").css("display") != "none") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(4).val() == "") {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(4).val() != "" && !reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(4).val())) {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(4).val() > 100) {
                    maxMission = false;
                    break;
                }
            }
        }
    }
    if (!before) {
        alert("第 [ " + group_index + " ] 组政策的提前天数只能是整数并且在3位数以内，请重新填写!");
        return false;
    }
    if (!filght_flag) {
        alert("第 [ " + group_index + " ] 组政策的去程航班日期，开始出票日期不能为空!");
        return false;
    }
    if (!qucheng_flag) {
        alert("第 [ " + group_index + " ] 组政策的去程日期范围有误！结束时间不能小于开始时间");
        return false;
    }
    if (!rungStart_flag) {
        alert("第 [ " + group_index + " ] 组政策的回程日期范围有误！开始时间不能小于去程开始时间");
        return false;
    }
    if (!huicheng_flag) {
        alert("第 [ " + group_index + " ] 组政策的回程日期范围有误！结束时间不能小于开始时间");
        return false;
    }
    if (!rungEnd_flag) {
        alert("第 [ " + group_index + " ] 组政策的回程日期范围有误！结束时间不能小于去程结束时间");
        return false;
    }
    if (!round_flag) {
        alert("第 [ " + group_index + " ] 组政策的出票时间不能大于去程的结束时间!");
        return false;
    }
    if (!bunks_flag) {
        alert("第 [ " + group_index + " ] 组政策的舱位不能为空!");
        return false;
    }
    if (!tiqian_flag) {
        alert("第 [ " + group_index + " ] 组政策的最少提前天数不能为空，并且必须为整数!");
        return false;
    }
    if (!mostTiqian) {
        alert("第 [ " + group_index + " ] 组政策的最多提前天数不能为空，并且必须为整数!");
        return false;
    }
    if (!rundTiqian) {
        alert("第 [ " + group_index + " ] 组政策的最少提前天数不能大于最多提前天数!");
        return false;
    }
    if (!chuxing_flag) {
        alert("第 [ " + group_index + " ] 组政策的出行天数不能为空，并且必须为整数!");
        return false;
    }
    if (!price_flag) {
        alert("第 [ " + group_index + " ] 组政策的价格或折扣不能为空，并且必须为整数!");
        return false;
    }
    if (!flag) {
        alert("第 [ " + group_index + " ] 组政策的返佣信息不能为空，且必须是100以内的数字!");
        return false;
    }
    if (!maxMission) {
        alert("第 [ " + group_index + " ] 组政策的返佣信息必须是100以内的数字!");
        return false;
    }

    if (!banqishiyong) {
        alert("第 [ " + group_index + " ] 组政策的适用班期必须选择一个！");
        return false;
    }
    if (!filterFlag) {
        alert("第 [ " + groupIndex + " ] 组政策的排除日期范围有误！请确认");
        return false;
    }
    return true;
}