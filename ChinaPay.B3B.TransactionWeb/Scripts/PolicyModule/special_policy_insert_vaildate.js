function Vaildate_Base() {
    //特殊产品类型
    var productType = $(".navType2Selected").attr("id");
    if (productType == "0" || productType == "1") {
        if ($("#txtShifaAirports").val() == "") {
            alert("出发地不能为空，请选择至少一个城市作为出发地!");
            return false;
        }
        if ($("#txtWangfanAirports").val() == "") {
            alert("目的地不能为空，请选择至少一个城市作为目的地!");
            return false;
        }
    }
    if (productType == "2" || productType == "3" || productType == "4" || productType == "5" || productType == "6") {
        if ($("#txtAirports").val() == "") {
            alert("出发地不能为空，请选择至少一个城市作为出发地!");
            return false;
        }
        if ($("input[name='radDepartureRange']:checked").val() == 1 && $("#lbSource option").length == 0) {
            alert("出发地不能为空，请选择至少一个城市作为出发地!");
            return false;
        }
        if ($("#txtAirports1").val() == "") {
            alert("目的地不能为空，请选择至少一个城市作为目的地!");
            return false;
        }
        if ($("input[name='radInculd1']:checked").val() == 1 && $("#lbSource1 option").length == 0) {
            alert("目的地不能为空，请选择至少一个城市作为目的地!");
            return false;
        }
    }
    if (productType == "3" || productType == "4" || productType == "6") {
        if ($("#txtDrawerCondition").val().length > 200) {
            alert("出票条件不能超过200个字！");
            $("#txtDrawerCondition").val($("#txtDrawerCondition").val().substring(0, 200));
            return false;
        }
    }
    if ($("#txtRemark").val().length > 200) {
        alert("备注信息不能超过200个字！");
        $("#txtRemark").val($("#txtRemark").val().substring(0, 200));
        return false;
    }
    var msg = "";
    var flag = true;
    var tiqian_flag = true;
    var bunks1_flag = true;
    var price_flag = true;
    var filght_flag = true;
    var round_flag = true;
    var qucheng_flag = true;
    var count_flag = true;
    var syn_flag = true;
    var youwei_flag = true;
    var bunks_flag = true;
    var rungStart_flag = true;
    var rungEnd_flag = true;
    var round_flag = true;
    var maxMission = true;
    var group_index = 0;
    var banqishiyong = true;
    var low = true;
    var filterFlag = true;

    var reg = /^[0-9]{1,10}(\.[0-9])?$/;
    for (var k = 0; k < $(".parent_div").length; k++) {
        group_index = parseInt(k) + 1;
        var PolicyDepartureFilghtDataStart = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(0).val();
        var PolicyDepartureFilghtDataEnd = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(1).val();
        var PolicyStartPrintDate = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(2).val();
        var departureDateFilter = $(".parent_div").eq(k).find("input[type='text']").eq(3).val();


        var Tiqian = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val() == "" ? "0" : $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val();
        //var Price = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(5).val();

        var b2BInternalCommission = $(".parent_div").eq(k).find("input[type='text']").eq(7).val();
        var b2BSubordinateCommission = $(".parent_div").eq(k).find("input[type='text']").eq(8).val();
        var b2BProfessionCommission = $(".parent_div").eq(k).find("input[type='text']").eq(9).val();

        var bunks = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(5).val();
        var Count = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(6).val();

        var lowNoType = $(".parent_div").eq(k).find(".policy_check input[type='checkbox']").eq(2).is(":checked") ? "1" : ($(".parent_div").eq(k).find(".policy_check input[type='checkbox']").eq(3).is(":checked") ? "2" : "0");
        var lowNoCommission = "-1";
        var lowNoPrice = "-1";
        if (lowNoType == "1") {
            lowNoPrice = $(".parent_div").eq(k).find(".policy_check input[type='text']").eq(0).val();
            lowNoCommission = $(".parent_div").eq(k).find(".policy_check input[type='text']").eq(1).val();
        }
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

        reg = /^[0-9]{1,10}?$/;
        if (productType == "0") {
            if (Tiqian == "") {
                tiqian_flag = false;
                break;
            }
            if (Tiqian != "0" && !reg.test(Tiqian)) {
                tiqian_flag = false;
                break;
            }
            if ($(".neibu").css("display") != "none") {
                if (b2BInternalCommission == "") {
                    flag = false;
                    break;
                }
                if (b2BInternalCommission != "-1" && !reg.test(b2BInternalCommission)) {
                    flag = false;
                    break;
                }
                if (b2BInternalCommission <= 0) {
                    flag = false;
                    break;
                }
            }
            if ($(".xiaji").css("display") != "none") {
                if (b2BSubordinateCommission == "") {
                    flag = false;
                    break;
                }
                if (b2BSubordinateCommission != "-1" && !reg.test(b2BSubordinateCommission)) {
                    flag = false;
                    break;
                }
                if (b2BSubordinateCommission <= 0) {
                    flag = false;
                    break;
                }
            }

            if ($(".tonghang").css("display") != "none") {
                if (b2BProfessionCommission == "") {
                    flag = false;
                    break;
                }
                if (b2BProfessionCommission != "-1" && !reg.test(b2BProfessionCommission)) {
                    flag = false;
                    break;
                }
                if (b2BProfessionCommission <= 0) {
                    flag = false;
                    break;
                }
            }
            if (Count == "") {
                count_flag = false;
                break;
            }
            var reg1 = /^[0-9]{1}?$/;
            if (Count != "" && !reg1.test(Count)) {
                msg = "只能输入1-9的整数！";
                count_flag = false;
                break;
            }
            if (Count <= 0) {
                count_flag = false;
                break;
            }
        } else if (productType == "1") {
            if (Tiqian == "") {
                tiqian_flag = false;
                break;
            }
            if (Tiqian != "0" && !reg.test(Tiqian)) {
                tiqian_flag = false;
                break;
            }
            if ($(".neibu").css("display") != "none") {
                if (b2BInternalCommission == "") {
                    flag = false;
                    break;
                }
                if (b2BInternalCommission != "-1" && !reg.test(b2BInternalCommission)) {
                    flag = false;
                    break;
                }
                if (b2BInternalCommission <= 0) {
                    flag = false;
                    break;
                }
            }
            if ($(".xiaji").css("display") != "none") {
                if (b2BSubordinateCommission == "") {
                    flag = false;
                    break;
                }
                if (b2BSubordinateCommission != "-1" && !reg.test(b2BSubordinateCommission)) {
                    flag = false;
                    break;
                }
                if (b2BSubordinateCommission <= 0) {
                    flag = false;
                    break;
                }
            }

            if ($(".tonghang").css("display") != "none") {
                if (b2BProfessionCommission == "") {
                    flag = false;
                    break;
                }
                if (b2BProfessionCommission != "-1" && !reg.test(b2BProfessionCommission)) {
                    flag = false;
                    break;
                }
                if (b2BProfessionCommission <= 0) {
                    flag = false;
                    break;
                }
            }
            if (Count == "") {
                count_flag = false;
                break;
            }
            var reg1 = /^[0-9]{1,2}?$/;
            if (Count != "" && !reg1.test(Count)) {
                msg = "只能输入1-99的整数！";
                count_flag = false;
                break;
            }
            if (Count <= 0) {
                count_flag = false;
                break;
            }
        } else if (productType == "2") {
            if (Tiqian == "") {
                tiqian_flag = false;
                break;
            }
            if (Tiqian != "0" && !reg.test(Tiqian)) {
                tiqian_flag = false;
                break;
            }
            if ($(".neibu").css("display") != "none") {
                if (b2BInternalCommission == "") {
                    flag = false;
                    break;
                }
                if (b2BInternalCommission != "-1" && !reg.test(b2BInternalCommission)) {
                    flag = false;
                    break;
                }
                if (b2BInternalCommission <= 0) {
                    flag = false;
                    break;
                }
            }
            if ($(".xiaji").css("display") != "none") {
                if (b2BSubordinateCommission == "") {
                    flag = false;
                    break;
                }
                if (b2BSubordinateCommission != "-1" && !reg.test(b2BSubordinateCommission)) {
                    flag = false;
                    break;
                }
                if (b2BSubordinateCommission <= 0) {
                    flag = false;
                    break;
                }
            }

            if ($(".tonghang").css("display") != "none") {
                if (b2BProfessionCommission == "") {
                    flag = false;
                    break;
                }
                if (b2BProfessionCommission != "-1" && !reg.test(b2BProfessionCommission)) {
                    flag = false;
                    break;
                }
                if (b2BProfessionCommission <= 0) {
                    flag = false;
                    break;
                }
            }
            if (!$(".parent_div .groupBox1").eq(k).find(".heipingtongbu input[type='radio']").eq(0).is(":checked") && !$(".parent_div .groupBox1").eq(k).find(".heipingtongbu input[type='radio']").eq(1).is(":checked")) {
                syn_flag = false;
                break;
            }
            if ($(".parent_div .groupBox1").eq(k).find(".heipingtongbu input[type='radio']").eq(1).is(":checked") && !$(".parent_div .groupBox1").eq(k).find(".shuliangBunks input[type='radio']").eq(0).is(":checked") && !$(".parent_div .groupBox1").eq(k).find(".shuliangBunks input[type='radio']").eq(1).is(":checked")) {
                youwei_flag = false;
                break;
            }
            if ($(".parent_div .groupBox1").eq(k).find(".heipingtongbu input[type='radio']").eq(0).is(":checked")) {
                if ($(".parent_div").eq(k).find(".groupBox1 .BunksRad select option:selected").val() == null || $(".parent_div").eq(k).find(".groupBox1 .BunksRad select option:selected").val() == "") {
                    bunks_flag = false;
                    break;
                }
            } else {
                if (Count == "") {
                    count_flag = false;
                    break;
                } var reg1 = /^[0-9]{1,2}?$/;
                if (Count != "" && !reg1.test(Count)) {
                    msg = "只能输入1-99的整数！";
                    count_flag = false;
                    break;
                }
                if (Count <= 0) {
                    count_flag = false;
                    break;
                }
            }
        }
        else if (productType == "3") {
            var reg3 = /^[0-9]{1,10}(\.[0-9])?$/;
            if (Tiqian == "") {
                tiqian_flag = false;
                break;
            }
            if (Tiqian != "0" && !reg.test(Tiqian)) {
                tiqian_flag = false;
                break;
            }
            var IsBargainBerths = $(".parent_div").eq(k).find(".jituan input[type='radio']:checked").val() == "1";
            if ($(".parent_div").eq(k).find(".groupBox2 table tr td[class='BSP txt-l']>input[type='checkbox']").is(":checked")) {
                if (IsBargainBerths) {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".BunksBargain select option:selected").val() == null || $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".BunksBargain select option:selected").val() == "") {
                        bunks_flag = false;
                        break;
                    }
                } else {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".BunksRad input[type='checkbox']").val() == null || $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".BunksRad input[type='checkbox']:checked").length == 0) {
                        bunks_flag = false;
                        break;
                    }
                }
                var selValue = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".selectPrice").val();
                if (selValue == "0") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val() == "") {
                        price_flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val() != "" && !reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val())) {
                        price_flag = false;
                        break;
                    }
                }
                if (selValue == "1") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val() == "") {
                        price_flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val() != "" && !reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val())) {
                        price_flag = false;
                        break;
                    }
                    if (parseInt($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val()) > 100) {
                        maxMission = false;
                        break;
                    }
                }

                if ($(".canHaveSubordinate").css("display") != "none") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val() == "") {
                        flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val() != "" && !reg3.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val())) {
                        flag = false;
                        break;
                    }
                    if (selValue == "1") {
                        if (parseInt($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val()) > 100) {
                            maxMission = false;
                            break;
                        }
                    }
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val() == "") {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val() != "" && !reg3.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val())) {
                    flag = false;
                    break;
                }
                if (selValue == "1") {
                    if (parseInt($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val()) > 100) {
                        maxMission = false;
                        break;
                    }
                }
                if ($(".allowBrotherPurchase").css("display") != "none") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val() == "") {
                        flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val() != "" && !reg3.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val())) {
                        flag = false;
                        break;
                    }
                    if (selValue == "1") {
                        if (parseInt($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val()) > 100) {
                            maxMission = false;
                            break;
                        }
                    }
                }
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr td[class='B2B']>input[type='checkbox']").is(":checked")) {
                if (IsBargainBerths) {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".BunksBargain select option:selected").val() == null || $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".BunksBargain select option:selected").val() == "") {
                        bunks_flag = false;
                        break;
                    }
                } else {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".BunksRad input[type='checkbox']").val() == null || $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".BunksRad input[type='checkbox']:checked").length == 0) {
                        bunks_flag = false;
                        break;
                    }
                }
                var selValue = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".selectPrice").val();
                if (selValue == "0") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val() == "") {
                        price_flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val() != "" && !reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val())) {
                        price_flag = false;
                        break;
                    }
                }
                if (selValue == "1") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val() == "") {
                        price_flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val() != "" && !reg3.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val())) {
                        price_flag = false;
                        break;
                    }
                    if (parseInt($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val()) > 100) {
                        maxMission = false;
                        break;
                    }
                }

                if ($(".canHaveSubordinate").css("display") != "none") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val() == "") {
                        flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val() != "" && !reg3.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val())) {
                        flag = false;
                        break;
                    }
                    if (selValue == "1") {
                        if (parseInt($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val()) > 100) {
                            maxMission = false;
                            break;
                        }
                    }
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(3).val() == "") {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(3).val() != "" && !reg3.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(3).val())) {
                    flag = false;
                    break;
                }
                if (selValue == "1") {
                    if (parseInt($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(3).val()) > 100) {
                        maxMission = false;
                        break;
                    }
                }
                if ($(".allowBrotherPurchase").css("display") != "none") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(4).val() == "") {
                        flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(4).val() != "" && !reg3.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(4).val())) {
                        flag = false;
                        break;
                    }
                    if (selValue == "1") {
                        if (parseInt($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(4).val()) > 100) {
                            maxMission = false;
                            break;
                        }
                    }
                }
            }
            if (lowNoType == "1") {
                if (lowNoPrice == "") {
                    price_flag = false;
                    break;
                }
                if (lowNoPrice != "" && !reg3.test(lowNoPrice)) {
                    price_flag = false;
                    break;
                }
                if (lowNoCommission != "" && !reg3.test(lowNoCommission)) {
                    price_flag = false;
                    break;
                }
                if (lowNoCommission != "" && parseInt(lowNoPrice) > parseInt(lowNoCommission)) {
                    low = false;
                    break;
                }
            }
        } else if (productType == "4" || productType == "6") {
            var reg3 = /^[0-9]{1,10}(\.[0-9])?$/;
            reg = /^[0-9]{1,2}?$/;
            if (Tiqian == "") {
                tiqian_flag = false;
                break;
            }
            if (Tiqian != "0" && !reg.test(Tiqian)) {
                tiqian_flag = false;
                break;
            }
            reg = /^[0-9]{1,10}?$/;
            var IsBargainBerths = $(".parent_div").eq(k).find(".jituan input[type='radio']:checked").val() == "1";
            if (IsBargainBerths) {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".BunksBargain select option:selected").val() == null || $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".BunksBargain select option:selected").val() == "") {
                    bunks_flag = false;
                    break;
                }
            } else {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".BunksRad input[type='checkbox']").val() == null || $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".BunksRad input[type='checkbox']:checked").length == 0) {
                    bunks_flag = false;
                    break;
                }
            }
            if (productType != "6") {

                var selValue = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".selectPrice option:selected").val();
                if (selValue == "0") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val() == "") {
                        price_flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val() != "" && !reg.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val())) {
                        price_flag = false;
                        break;
                    }
                }
                if (selValue == "1") {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val() == "") {
                        price_flag = false;
                        break;
                    }
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val() != "" && !reg3.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val())) {
                        price_flag = false;
                        break;
                    }
                    if (parseInt($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val()) > 100) {
                        maxMission = false;
                        break;
                    }
                }
            }
            if ($(".canHaveSubordinate").css("display") != "none") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val() == "") {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val() != "" && !reg3.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val())) {
                    flag = false;
                    break;
                }
                if (selValue == "1") {
                    if (parseInt($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val()) > 100) {
                        maxMission = false;
                        break;
                    }
                }
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val() == "") {
                flag = false;
                break;
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val() != "" && !reg3.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val())) {
                flag = false;
                break;
            }
            if (selValue == "1") {
                if (parseInt($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val()) > 100) {
                    maxMission = false;
                    break;
                }
            }
            if ($(".allowBrotherPurchase").css("display") != "none") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val() == "") {
                    flag = false;
                    break;
                }
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val() != "" && !reg3.test($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val())) {
                    flag = false;
                    break;
                }
                if (selValue == "1") {
                    if (parseInt($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val()) > 100) {
                        maxMission = false;
                        break;
                    }
                }
            }
        } else if (productType == "5") {
            if (Tiqian == "") {
                tiqian_flag = false;
                break;
            }
            if (Tiqian != "0" && !reg.test(Tiqian)) {
                tiqian_flag = false;
                break;
            }
            if (bunks == "") {
                bunks1_flag = false;
                break;
            }
            if ($(".neibu").css("display") != "none") {
                if (b2BInternalCommission == "") {
                    flag = false;
                    break;
                }
                if (b2BInternalCommission != "-1" && !reg.test(b2BInternalCommission)) {
                    flag = false;
                    break;
                }
                if (b2BInternalCommission <= 0) {
                    flag = false;
                    break;
                }
            }
            if ($(".xiaji").css("display") != "none") {
                if (b2BSubordinateCommission == "") {
                    flag = false;
                    break;
                }
                if (b2BSubordinateCommission != "-1" && !reg.test(b2BSubordinateCommission)) {
                    flag = false;
                    break;
                }
                if (b2BSubordinateCommission <= 0) {
                    flag = false;
                    break;
                }
            }
            if ($(".tonghang").css("display") != "none") {
                if (b2BProfessionCommission == "") {
                    flag = false;
                    break;
                }
                if (b2BProfessionCommission != "-1" && !reg.test(b2BProfessionCommission)) {
                    flag = false;
                    break;
                }
                if (b2BProfessionCommission <= 0) {
                    flag = false;
                    break;
                }
            }
        }

    }
    if (!filght_flag) {
        alert("第 [ " + group_index + " ] 组政策的去程航班日期，开始出票日期不能为空!");
        return false;
    }
    if (!qucheng_flag) {
        alert("第 [ " + group_index + " ] 组政策的去程日期范围有误！结束时间不能小于开始时间");
        return false;
    }
    if (!round_flag) {
        alert("第 [ " + group_index + " ] 组政策的出票时间不能大于去程的结束时间");
        return false;
    }
    if (!tiqian_flag) {
        alert("第 [ " + group_index + " ] 组政策的提前天数不能为空，且必须是大于零的整数");
        return false;
    }
    if (!bunks1_flag) {
        alert("第 [ " + group_index + " ] 组政策的舱位为空，必须填写一个舱位！");
        return false;
    }
    if (!flag) {
        alert("第 [ " + group_index + " ] 组政策的返佣信息/价格不能为空，且必须是大于等于零的整数或一位小数!");
        return false;
    }
    if (!price_flag) {
        alert("第 [ " + group_index + " ] 组政策的价格或直减不能为空，且必须是大于零的整数或一位小数");
        return false;
    }
    if (!syn_flag) {
        alert("第 [ " + group_index + " ] 组政策的黑屏同步两种必须选择一种！");
        return false;
    }
    if (!low) {
        alert("第 [ " + group_index + " ] 组政策的票面价区间前面的价格不能大于后面的价格！");
        return false;
    }
    if (!youwei_flag) {
        alert("第 [ " + group_index + " ] 组政策的黑屏不同步中有位或无位出票两种必须选择一种！");
        return false;
    }
    if (!bunks_flag) {
        alert("第 [ " + group_index + " ] 组政策的舱位为空，必须选择一个舱位！");
        return false;
    }
    if (!count_flag) {
        alert("第 [ " + group_index + " ] 组政策的提供资源张数不能为空，且必须是大于零的整数。" + msg);
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
