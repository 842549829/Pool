
function GetPolicyValue(actionName) {
    var policyArrayBase;
    var policyArrayGroup = new Array();

    var airline = $("#selProvince").val();
    var office = $("#selOffice").val();
    var impowerOffice = $("#selOffice option:selected").attr("impower");
    var customCode = $.trim($("#selZidingy option:selected").val()) == "0" ? "" : $("#selZidingy option:selected").val();
    var isInternal = $(".canHaveSubordinate").css("display") != "none";
    var isPeer = $(".allowBrotherPurchase").css("display") != "none";
    var tripType = "";
    //中转城市
    var transit = "";
    //出发地
    var departureAirports = "";
    if ($("input[name='radDepartureRange']:checked").val() == "0") {
        departureAirports = $("#txtAirports").val();
    }
    if ($("input[name='radDepartureRange']:checked").val() == "1") {
        $("option", $("#lbSource")).each(function (index) {
            if (index > 0) {
                departureAirports += "/";
            }
            departureAirports += $(this).attr("value");
        });
    }
    //目的地
    var arrivalAirports = "";
    //行程类型
    if ($(".navType2Selected").attr("id") == "dancheng") {
        tripType = "OneWay";
        if ($("input[name='radInculd1']:checked").val() == "0") {
            arrivalAirports = $("#txtAirports1").val();
        }
        if ($("input[name='radInculd1']:checked").val() == "1") {
            $("option", $("#lbSource1")).each(function (index) {
                if (index > 0) {
                    arrivalAirports += "/";
                }
                arrivalAirports += $(this).attr("value");
            });
        }

    }
    else if ($(".navType2Selected").attr("id") == "wanfang") {
        tripType = "RoundTrip";
        if ($("input[name='radInculd1']:checked").val() == "0") {
            arrivalAirports = $("#txtAirports1").val();
        }
        if ($("input[name='radInculd1']:checked").val() == "1") {
            $("option", $("#lbSource1")).each(function (index) {
                if (index > 0) {
                    arrivalAirports += "/";
                }
                arrivalAirports += $(this).attr("value");
            });
        }

    }
    else if ($(".navType2Selected").attr("id") == "danchengwanfang") {
        tripType = "OneWayOrRound";
        if ($("input[name='radInculd1']:checked").val() == "0") {
            arrivalAirports = $("#txtAirports1").val();
        }
        if ($("input[name='radInculd1']:checked").val() == "1") {
            $("option", $("#lbSource1")).each(function (index) {
                if (index > 0) {
                    arrivalAirports += "/";
                }
                arrivalAirports += $(this).attr("value");
            });
        }

    }
    else if ($(".navType2Selected").attr("id") == "zhongzhuan") {
        tripType = "TransitWay";
        if ($("input[name='radInculd1']:checked").val() == "0") {
            transit = $("#txtAirports1").val();
        }
        if ($("input[name='radInculd1']:checked").val() == "1") {
            $("option", $("#lbSource1")).each(function (index) {
                if (index > 0) {
                    transit += "/";
                }
                transit += $(this).attr("value");
            });
        }
        if ($("input[name='radInculd2']:checked").val() == "0") {
            arrivalAirports = $("#txtAirports2").val();
        }
        if ($("input[name='radInculd2']:checked").val() == "1") {
            $("option", $("#lbSource2")).each(function (index) {
                if (index > 0) {
                    arrivalAirports += "/";
                }
                arrivalAirports += $(this).attr("value");
            });
        }

    }
    //去程班期类型限制
    var departureDateType = "Date";
    var departureDate = "";

    //去程航班限制类型
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
    //回程航班类型限制
    var arrivalFilghtType = "";
    if ($("input[name='ReturnFilght']:checked").val() == "0") {
        arrivalFilghtType = "None";
    }
    if ($("input[name='ReturnFilght']:checked").val() == "1") {
        arrivalFilghtType = "Include";
    }
    if ($("input[name='ReturnFilght']:checked").val() == "2") {
        arrivalFilghtType = "Exclude";
    }
    //回程航班
    var arrivalFilght = $("#txtReturnFilght").val();
    //排除航线
    var exceptAirways = $("#txtOutWithFilght").val();
    //备注
    var remark = $("#txtRemark").val();
    //出票条件
    var drawerCondition = $("#txtDrawerCondition").val();


    for (var k = 0; k < $(".parent_div").length; k++) {
        var policyDepartureFilghtDataStart = $(".parent_div").eq(k).find("input[type='text']").eq(0).val();
        var policyDepartureFilghtDataEnd = $(".parent_div").eq(k).find("input[type='text']").eq(1).val();
        var policyStartPrintDate = $(".parent_div").eq(k).find("input[type='text']").eq(2).val();
        var policyReturnDateStart = null;
        var policyReturnDateEnd = null;

        var departureDateFilter = $(".parent_div").eq(k).find("input[type='text']").eq(3).val();
        var departureWeekFilter = "";
        for (var s = 0; s < $(".parent_div").eq(k).find(".pd_left input[type='checkbox']:checked").length; s++) {
            if (departureWeekFilter != "") {
                departureWeekFilter += ",";
            }
            departureWeekFilter += $(".parent_div").eq(k).find(".pd_left input[type='checkbox']:checked").eq(s).val();
        }
        //是否指定团队舱位
        var appointBerths = $(".parent_div").eq(k).find(".zhidingcangwei input[type='radio']").eq(0).is(":checked");

        var autoAudit = $(".parent_div").eq(k).find(".policy_check input[type='checkbox']").eq(0).is(":checked");
        var changePnr = $(".parent_div").eq(k).find(".policy_check input[type='checkbox']").eq(1).is(":checked");
        var suitReduce = $(".parent_div").eq(k).find(".policy_check input[type='checkbox']").eq(2).is(":checked");
        var multiSuitReduce = $(".parent_div").eq(k).find(".policy_check input[type='checkbox']").eq(3).is(":checked");
        var printBeforeTwoHours = false;
        //返佣信息
        var b2BInternalCommission = "";
        var b2BSubordinateCommission = "";
        var b2BProfessionCommission = "";
        var b2BBerths = "";
        var bspInternalCommission = "";
        var bspSubordinateCommission = "";
        var bspProfessionCommission = "";
        var bspBerths = "";

        var g;
        if ($(".parent_div").eq(k).find(".groupBox2 table tr td[class='BSP']>input[type='checkbox']").is(":checked")) {
            if ($(".canHaveSubordinate").css("display") == "none") {
                bspInternalCommission = -1;
            } else {
                bspInternalCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val();
            }
            bspSubordinateCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val();
            if ($(".allowBrotherPurchase").css("display") == "none") {
                bspProfessionCommission = -1;
            } else {
                bspProfessionCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val();
            }
            if (appointBerths) {
                bspBerths += $(".parent_div").eq(k).find(".groupBox2 .ZhidingBunks").eq(0).find("select option:selected").val();
            } else {
                for (g = 0; g < $(".parent_div").eq(k).find(".groupBox2 table tr td .BunksRad").eq(0).find("input[type='checkbox']:checked").length; g++) {
                    if (g > 0) {
                        bspBerths += ",";
                    }
                    bspBerths += $(".parent_div").eq(k).find(".groupBox2 table tr td .BunksRad").eq(0).find("input[type='checkbox']:checked").eq(g).val();
                }
            }

            if ($("input[name='radVoyageType']:checked").val() != "1") {
                policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "StartPrintDate": policyStartPrintDate, "ReturnDateStart": policyReturnDateStart, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "ReturnDateEnd": policyReturnDateEnd, "Berths": bspBerths, "AppointBerths": appointBerths, "TicketType": "BSP", "InternalCommission": (parseFloat(bspInternalCommission)), "SubordinateCommission": (parseFloat(bspSubordinateCommission)), "ProfessionCommission": (parseFloat(bspProfessionCommission)), "Vip": 0, "ChangePNR": changePnr, "AutoAudit": autoAudit, "SuitReduce": suitReduce, "MultiSuitReduce": multiSuitReduce, "PrintBeforeTwoHours": false });
            } else {
                policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "DepartureDateEnd": policyDepartureFilghtDataEnd, "StartPrintDate": policyStartPrintDate, "Berths": bspBerths, "AppointBerths": appointBerths, "TicketType": "BSP", "InternalCommission": (parseFloat(bspInternalCommission)), "SubordinateCommission": (parseFloat(bspSubordinateCommission)), "ProfessionCommission": (parseFloat(bspProfessionCommission)), "ChangePNR": changePnr, "AutoAudit": autoAudit, "SuitReduce": suitReduce, "MultiSuitReduce": multiSuitReduce, "PrintBeforeTwoHours": false });
            }
        }

        if ($(".parent_div").eq(k).find(".groupBox2 table tr td .chkb2b").is(":checked")) {
            printBeforeTwoHours = $(".parent_div").eq(k).find(".groupBox2 table tr td .qfqcp").is(":checked");
            if ($(".canHaveSubordinate").css("display") == "none") {
                b2BInternalCommission = -1;
            } else {
                b2BInternalCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val();
            }
            b2BSubordinateCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val();
            if ($(".allowBrotherPurchase").css("display") == "none") {
                b2BProfessionCommission = -1;
            } else {
                b2BProfessionCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val();
            } if (appointBerths) {
                b2BBerths += $(".parent_div").eq(k).find(".groupBox2 .ZhidingBunks").eq(1).find("select option:selected").val();
            } else {
                for (g = 0; g < $(".parent_div").eq(k).find(".groupBox2 table tr td .BunksRad").eq(1).find("input[type='checkbox']:checked").length; g++) {
                    if (g > 0) {
                        b2BBerths += ",";
                    }
                    b2BBerths += $(".parent_div").eq(k).find(".groupBox2 table tr td .BunksRad").eq(1).find("input[type='checkbox']:checked").eq(g).val();
                }
            }

            if ($("input[name='radVoyageType']:checked").val() == "1") {
                policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "StartPrintDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "Berths": b2BBerths, "AppointBerths": appointBerths, "TicketType": "B2B", "InternalCommission": parseFloat(b2BInternalCommission), "SubordinateCommission": parseFloat(b2BSubordinateCommission), "ProfessionCommission": parseFloat(b2BProfessionCommission), "Vip": 0, "ChangePNR": changePnr, "AutoAudit": autoAudit, "SuitReduce": suitReduce, "MultiSuitReduce": multiSuitReduce, "PrintBeforeTwoHours": printBeforeTwoHours });
            } else {
                policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "StartPrintDate": policyStartPrintDate, "ReturnDateStart": policyReturnDateStart, "ReturnDateEnd": policyReturnDateEnd, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "Berths": b2BBerths, "AppointBerths": appointBerths, "TicketType": "B2B", "InternalCommission": parseFloat(b2BInternalCommission), "SubordinateCommission": parseFloat(b2BSubordinateCommission), "ProfessionCommission": parseFloat(b2BProfessionCommission), "Vip": 0, "ChangePNR": changePnr, "AutoAudit": autoAudit, "SuitReduce": suitReduce, "MultiSuitReduce": multiSuitReduce, "PrintBeforeTwoHours": printBeforeTwoHours });
            }
        }
    }

    policyArrayBase = { "team": { "BasicInfo": { "Airline": airline, "OfficeCode": office, "ImpowerOffice": impowerOffice, "IsInternal": isInternal, "IsPeer": isPeer, "CustomCode": customCode, "VoyageType": tripType, "Departure": departureAirports, "Arrival": arrivalAirports, "Transit": transit, "DepartureFlightsFilterType": departureFilghtType, "DepartureFlightsFilter": departureFilght, "ReturnFlightsFilterType": arrivalFilghtType, "ReturnFlightsFilter": arrivalFilght, "ExceptAirways": exceptAirways, "Remark": remark, "DrawerCondition": drawerCondition }, "Rebates": policyArrayGroup} };
    //发布团队政策
    var url = "/PolicyHandlers/RoleGeneralPolicy.ashx/RegisterTeamPolicy";

    ResquetQueryAction(url, actionName, policyArrayBase);
};

function ShowOrHides(id) {
    if (id == "dancheng") {
        $("#duihuan").css("display", "");
        $(".class_display").css("display", "none");
        $(".classDisplay").css("display", "none");
        $(".syddlc").css("display", "none");
        $(".wfjc").css("display", "none");
        $("#qucheng").html("航班限制");
        $("#huicheng").html("回程航班");
        $("#liancheng").html("目的地");
        $(".paichutishi").html("提示： 输入不适用本政策的始发和目的地，如：北京--济南行程不适用本政策，则输入PEKTNA，多个不适用航段用“ / ”隔开。");
        //ClearBunksByAirline();
    } else if (id == "wanfang") {
        $(".class_display").css("display", "");
        $("#duihuan").css("display", "");
        $(".classDisplay").css("display", "none");
        $(".syddlc").css("display", "none");
        $(".wfjc").css("display", "");
        $("#qucheng").html("去程航班");
        $("#huicheng").html("回程航班");
        $("#liancheng").html("目的地");
        $(".wfjc label").html("适用于往返降舱政策");
        $(".paichutishi").html("提示： 输入不适用本政策的始发和目的地，如：北京--济南行程不适用本政策，则输入PEKTNA，多个不适用航段用“ / ”隔开。");
        //ClearBunksByAirline();
    } else if (id == "zhongzhuan") {
        $(".class_display").css("display", "");
        $(".classDisplay").css("display", "");
        $(".wfjc").css("display", "");
        $(".syddlc").css("display", "");
        $(".wfjc label").html("适用于联程降舱政策");
        $("#duihuan").css("display", "none");
        $("#qucheng").html("第一程航班");
        $("#huicheng").html("第二程航班");
        $("#liancheng").html("中转地");
        $(".paichutishi").html("提示： 输入排除航线，多条航线之间用“ / ”隔开，（如：昆明到广州到杭州不适用，填写KMGCANHGH）");
        //ClearBunksByAirline();
    }
}
$(function () {

    //查询Office号
    var url;
    url = "/PolicyHandlers/RoleGeneralPolicy.ashx/GetOfficeNumbers";
    var actionName;
    actionName = "Office";
    ResquetQueryAction(url, actionName, null);

    //查询公司参数
    url = "/PolicyHandlers/PolicyManager.ashx/GetCompanySetting";
    actionName = "GetCompanySetting";
    ResquetQueryAction(url, actionName, null);

    AirportsArrivals();
    AirportsDepartures();
    AirportsZhongZhuanDepartures();
    $(".btnpolicyaddgroup").live("click", function () {
        var i = $(".parent_div").length;
        i++;
        var str = "<div class='parent_div'><br /><h4 class='groupHd'>第&nbsp;" + i + "&nbsp;组政策</h4><div class='groupCt'><div class='groupBox1'><div class='clearfix'><div class='fl pd_right groupLine'>适用航班日期：<input type='text' class='text datepicker datefrom  class3 quchengkaishi' onchange='ClearBunksByTime(this);' />至<input type='text' class='text datepicker datefrom  class3 quchengjieshu' onchange='ClearBunksByTime(this);' /></div><div class='fl pd_right groupLine'>开始出票日期：<input type='text' class='text datepicker datefrom  class3 chupiao' onchange='ClearBunksByTime(this);' ></div><div class='fl groupLine'><input type='button' class='btn class2 btnRefresh' value='刷新舱位' /></div></div><div class='groupLine'>航班排除日期：<input type='text' class='text text_width' /><p class='obvious1 pd'>请填写政策航班排除日期，单天如：20121121 连续多天如:20121125-20121127 多个天数用“,”隔开</p></div><div class='pd_left'>适用班期：<input type='checkbox' id='mon_" + i + "' value='1'   checked='checked'  /><label for='mon_" + i + "'>周一</label><input type='checkbox' id='tue_" + i + "'  checked='checked'  value='2'  /><label for='tue_" + i + "'>周二</label><input type='checkbox' id='wed_" + i + "'  value='3'   checked='checked'  /><label for='wed_" + i + "'>周三</label><input type='checkbox' id='thur_" + i + "'  checked='checked'  value='4'   /><label for='thur_" + i + "'>周四</label><input type='checkbox' id='fri_" + i + "'   value='5'  checked='checked'  /><label for='fri_" + i + "'>周五</label><input type='checkbox' id='sat_" + i + "'  checked='checked'   value='6'  /><label for='sat_" + i + "'>周六</label><input type='checkbox' id='sun_" + i + "'   value='7'  checked='checked'  /><label for='sun_" + i + "'>周日</label><span class='obvious1'>提示：若某个班期不适用，请将周期前的勾去掉</span></div><br /><div class='pd_left zhidingcangwei'> 舱位指定：<input type='radio' class='zhidingcang'  value='0' id='zhiding_" + i + "' name='zhiding_" + i + "' ' checked='checked'   /><label for='zhiding_" + i + "'>指定团队舱位</label><input class='zhidingcang'  type='radio' value='1' id='buzhiding_" + i + "' name='zhiding_" + i + "' checked='checked' /><label for='buzhiding_" + i + "'>普通舱位</label> </div></div><div class='groupBox2'><table><tr><th>舱位</th><th>客票类型</th><th class='canHaveSubordinate' style='display: none;'>内部佣金</th><th>下级佣金</th><th class='allowBrotherPurchase' style='display: none;'>同行佣金</th></tr><tr><td><div class='ZhidingBunks' style='display:none;'><label class='refBtnBunks btn class3'>点击获取舱位</label></div><div class='BunksRad'><label class='refBtnBunks btn class3'>点击获取舱位</label></div></td><td class='BSP'><input type='checkbox' checked='checked'  id='bsp_" + i + "' /><label for='bsp_" + i + "'>BSP</label></td><td class='canHaveSubordinate' style='display: none;'><input type='text' class='text text-s' />%</td><td><input type='text' class='text text-s' />%</td><td class='allowBrotherPurchase' style='display: none;'><input type='text' class='text text-s' />%</td></tr><tr><td><div class='ZhidingBunks' style='display:none;'><label class='refBtnBunks btn class3'>点击获取舱位</label></div><div class='BunksRad'><label class='refBtnBunks btn class3'>点击获取舱位</label></div></td><td class='B2B'><input type='checkbox' checked='checked'  id='b2b_" + i + "' class='chkb2b'  /><label for='b2b_" + i + "'>B2B</label><br /><span style='margin-left:125px' class='qfqcp'><input type='checkbox' id='qfqcp_" + i + "' class='qfqcp' style='margin-left:20px;' /> <label for='qfqcp_" + i + "' class='qfqcp'>起飞前2小时内可出票</label></span></td><td class='canHaveSubordinate'><input type='text' class='text text-s' />%</td><td><input type='text' class='text text-s' />%</td><td class='allowBrotherPurchase' style='display: none;'><input type='text' class='text text-s' />%</td></tr></table><div class='clearfix'><div class='fl policy_check'><input type='checkbox' id='zjsh_" + i + "' /><label for='zjsh_" + i + "'>直接审核</label><input type='checkbox' id='hbmcp_" + i + "' /><label for='hbmcp_" + i + "'>需换编码出票</label><span class='wfjc'><input type='checkbox' id='wfjc_" + i + "' /><label for='wfjc_" + i + "'>适用于往返降舱政策</label></span><input type='checkbox' id='syddlc_" + i + "' class='syddlc'  style='display:none;'/> <label for='syddlc_" + i + "' class='syddlc' style='display:none;' >适用多段联程</label></div><div class='fr'><input type='button' class='btn class2 btnpolicyaddgroup' value='添加新组政策&nbsp;&nbsp;+' /><input type='button' class='btn class2' onclick='DelPolicyGroup(this);' value='删除本组政策&nbsp;&nbsp;×' /></div></div></div></div></div>";
        AddPolicyGroup(str, i);
        $(".quchengkaishi").eq(parseInt(i) - 1).val($(".quchengkaishi").eq(parseInt(i) - 2).val());
        $(".quchengjieshu").eq(parseInt(i) - 1).val($(".quchengjieshu").eq(parseInt(i) - 2).val());
        //        if ($(".quchengkaishi").eq(parseInt(i) - 2).val() != "" && $(".quchengjieshu").eq(parseInt(i) - 2).val() != "") {
        //            $(".parent_div").eq(parseInt(i) - 1).find(".btnRefresh").click();
        //        }
        if ($("input[name='radVoyageType']:checked").val() == "1") {
            $(".class_display").css("display", "none");
        } else {
            $(".class_display").css("display", "");
        }
        ShowOrHides($(".navType2Selected").attr("id"));
    });

    $("input[name='radVoyageType']").click(function () {
        if ($(this).val() == "1") {
            $(".class_display").css("display", "none");

        } else {
            $(".class_display").css("display", "");
        }
    });
    $("#btnPublish").click(function () {
        var publicFlag = Vaildate();
        var baseFlag = false;
        if (publicFlag) {
            baseFlag = Vaildate_Base();
        }

        if (publicFlag && baseFlag) {
            GetPolicyValue("Team_Publish");
        }
    });
    $("#btnPolicyAdd").click(function () {
        var publicFlag = Vaildate();
        var baseFlag = false;
        if (publicFlag) {
            baseFlag = Vaildate_Base();
        }
        if (publicFlag && baseFlag) {
            GetPolicyValue("Ahead");
        }
    });
    $(".zhidingcang").live("click", function () {
        var index = $(this).parent().parent().parent().parent().index();
        if ($(this).val() == "0") {
            $(".parent_div").eq(index).find(".BunksRad").css("display", "none");
            $(".parent_div").eq(index).find(".ZhidingBunks").css("display", "");
        } else {
            $(".parent_div").eq(index).find(".BunksRad").css("display", "");
            $(".parent_div").eq(index).find(".ZhidingBunks").css("display", "none");
        }
    });
    $(".btnRefresh").live("click", function () {
        var bunksIndex = $(this).parent().parent().parent().parent().parent().index();
        var zhidingValue = $(".parent_div").eq(bunksIndex).find(".zhidingcangwei input[type='radio']:checked").val();
        if (VaildateRefreshBunksTime(bunksIndex)) {
            var voyage = $(".navType2Selected").attr("id");

            //查询舱位
            var url;
            var param;
            if (zhidingValue == "0") {
                if (voyage == "dancheng") {
                    voyage = "OneWay";
                } else if (voyage == "wanfang") {
                    voyage = "RoundTrip";
                } else if (voyage == "zhongzhuan") {
                    voyage = "TransitWay";
                }
                param = { "airline": $("#selProvince").val(), "startTime": $(".parent_div").eq(bunksIndex).find("input[type='text']").eq(0).val(), "endTime": $(".parent_div").eq(bunksIndex).find("input[type='text']").eq(1).val(), "startETDZDate": $(".parent_div").eq(bunksIndex).find("input[type='text']").eq(2).val(), "voyage": voyage };
                url = "/PolicyHandlers/PolicyManager.ashx/QueryTeamBunksPolicy";
            }
            else {
                if (voyage == "dancheng") {
                    voyage = "OneWay";
                } else if (voyage == "wanfang") {
                    voyage = "RoundTrip";
                } else if (voyage == "zhongzhuan") {
                    voyage = "OneWayOrRound";
                }
                param = { "airline": $("#selProvince").val(), "startTime": $(".parent_div").eq(bunksIndex).find("input[type='text']").eq(0).val(), "endTime": $(".parent_div").eq(bunksIndex).find("input[type='text']").eq(1).val(), "startETDZDate": $(".parent_div").eq(bunksIndex).find("input[type='text']").eq(2).val(), "voyage": voyage };
                url = "/PolicyHandlers/PolicyManager.ashx/QueryTeamNormalBunksPolicy";
            }
            sendPostRequest(url, JSON.stringify(param), function (e) {
                if (zhidingValue == "0") {
                    var str = "<select class='select' style='width: 50px;'>";
                    $.each(eval(e), function (i, item) {
                        str += "<option value='" + item + "'>" + item + "</option>";
                    });
                    str += "</select>";
                    $(".parent_div").eq(bunksIndex).find(".ZhidingBunks").eq(0).html(str);
                    $(".parent_div").eq(bunksIndex).find(".ZhidingBunks").eq(1).html(str);
                } else {
                    var str = "<input type='radio' value='0' name='1radio_" + bunksIndex + "' id='1all_" + bunksIndex + "'  class='choice' /><label for='1all_" + bunksIndex + "'> 全选</label> <input type='radio' value='1' name='1radio_" + bunksIndex + "' id='1not_" + bunksIndex + "' class='choice' /><label for='1not_" + bunksIndex + "'> 反选</label><br />";
                    var str1 = "<input type='radio' value='0' name='1radio_" + bunksIndex + "1' id='1all_" + bunksIndex + "1'  class='choice' /><label for='1all_" + bunksIndex + "1'> 全选</label> <input type='radio' value='1' name='1radio_" + bunksIndex + "1' id='1not_" + bunksIndex + "1' class='choice' /><label for='1not_" + bunksIndex + "1'> 反选</label><br />";
                    $.each(eval(e), function (i, item) {
                        str1 += "<input type='checkbox' value='" + item + "'/>" + item;
                        str += "<input type='checkbox' value='" + item + "'/>" + item;
                        if ((i + 1) % 4 == 0 && i > 0) {
                            str += "<br />";
                            str1 += "<br />";
                        }
                    });
                    $(".parent_div").eq(bunksIndex).find(".BunksRad").eq(0).html(str);
                    $(".parent_div").eq(bunksIndex).find(".BunksRad").eq(1).html(str1);
                }
            }, function (e) {
            });
        }
    });
    $("#btnReturn").click(function () {
        window.location.href = "./team_policy_manage.aspx";
    });
}); 