$(function () {

    //查询Office号 
    ResquetQueryAction("/PolicyHandlers/RoleGeneralPolicy.ashx/GetOfficeNumbers", "Office", null);
    //查询公司参数
    ResquetQueryAction("/PolicyHandlers/PolicyManager.ashx/GetCompanySetting", "GetCompanySetting", null);
    //查询作废规定 
    ResquetQueryAction("/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations", "InvalidRegulation", { type: "BargainOneWayInvalidRegulation" });
    //查询改签规定 
    ResquetQueryAction("/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations", "ChangeRegulation", { type: "BargainOneWayChangeRegulation" });
    //查询签转规定 
    ResquetQueryAction("/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations", "EndorseRegulation", { type: "BargainOneWayEndorseRegulation" });
    //查询退票规定 
    ResquetQueryAction("/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations", "RefundRegulation", { type: "BargainOneWayRefundRegulation" });

    $("#txtWangfanAirports").live("blur", function () {
        var codePattern = $.trim($("#txtWangfanAirports").val()).toUpperCase();
        for (var i = 0; i < $("#lbShifaSource option").length; i++) {
            if ($("#lbShifaSource option").eq(i).val() == codePattern) {
                $("#txtWangfanAirports").val(codePattern);
                $("#lbShifaSource").val(codePattern);
                break;
            } else {
                $("#txtWangfanAirports").val("");
                $("#lbShifaSource").val("");
            }
        }
    });
    $("#txtWangfanAirports").live("keyup", function () {
        var codePattern = $.trim($("#txtWangfanAirports").val()).toUpperCase();
        $("#txtWangfanAirports").val(codePattern);
        $("#lbShifaSource").val(codePattern);
    });

    AirportsArrivals();
    AirportsDepartures();
    AirportsZhongZhuanDepartures();
    $(".btnpolicyaddgroup").live("click", function () {
        var i = $(".parent_div").length;
        i++;
        var str = " <div class='parent_div'><br /><h4 class='groupHd'>第&nbsp;" + i + "&nbsp;组政策</h4><div class='groupCt'><div class='groupBox1'><div class='clearfix'><div class='fl pd_right groupLine'>去程航班日期：<input type='text' onchange='ClearBunksByTime(this);' class='text datepicker datefrom  class3 quchengkaishi' />至<input type='text' onchange='ClearBunksByTime(this);' class='text datepicker datefrom  class3 quchengjieshu' /></div><div class='fl pd_right groupLine'>开始出票日期：<input type='text' onchange='ClearBunksByTime(this);' class='text datepicker datefrom  class3 chupiao' /></div> <div class='fl groupLine'> <input type='button' class='btn class2 btnRefresh' value='刷新舱位' /> </div></div><div class='groupLine'>航班排除日期：<input type='text' class='text text_width' /><p class='obvious1 pd'>请填写政策航班排除日期，单天如：20121121 连续多天如:20121125-20121127 多个天数用“,”隔开。</p></div><div class='pd_left groupLine'>适用班期：<input type='checkbox' id='mon_" + i + "' value='1'   checked='checked'  /><label for='mon_" + i + "'>周一</label><input type='checkbox' id='tue_" + i + "'   checked='checked'  value='2' /><label for='tue_" + i + "'>周二</label><input type='checkbox' id='wed_" + i + "'  value='3'  checked='checked'  /><label for='wed_" + i + "'>周三</label><input type='checkbox' id='thur_" + i + "' value='4'   checked='checked'  /><label for='thur_" + i + "'>周四</label><input type='checkbox' id='fri_" + i + "' value='5'  checked='checked'   /><label for='fri_" + i + "'>周五</label><input type='checkbox' id='sat_" + i + "' value='6'  checked='checked'  /><label for='sat_" + i + "' >周六</label><input type='checkbox' id='sun_" + i + "' value='7'   checked='checked'  /><label for='sun_" + i + "'>周日</label><span class='obvious1'>提示：若某个班期不适用，请将周期前的勾去掉</span></div><div class='pd_left groupLine tiqian'>最少提前天数： <input type='text' class='text text-s' /> 天 <span class='obvious1'>提示：请在黑屏输入相应航段NFD后NFN内最晚天数（即最晚提前预定天数）；若无限制则留空 </span></div><div class='pd_left groupLine tiqian'> 最多提前天数： <input type='text' class='text text-s' /> 天 <span class='obvious1'>提示：请在黑屏输入相应航段NFD后NFN内最早天数（即最早提前预定天数）；若无限制则留空 </span> </div><div class='pd_left groupLine chuxing' style='display: none;'> 出行天数： <input type='text' class='text text-s' />  天 <span class='obvious1'>提示：在输入框输入出行天数，没有可以不填 </span> </div> </div><div class='groupBox2'><table><tr><th>舱位</th><th class='discount'>折扣</th><th>客票类型</th><th class='canHaveSubordinate' style='display: none;'>内部佣金</th><th>下级佣金</th><th class='allowBrotherPurchase' style='display: none;'>同行佣金</th></tr><tr><td class='BunksRad'><label class='refBtnBunks btn class3'>点击获取舱位</label></td><td class='discount'> <select onchange='selPriceOrDiscount(this)' class='selectPrice'><option value='0'>按价格发布</option><option value='1'>按折扣发布</option><option value='3'>按返佣发布</option></select> <span class='price0'><input type='text' class='text text-s' maxlength='4' />元</span> <span class='discount0' style='display: none;'> <input type='text' class='text text-s' maxlength='3' />折 </span><span class='fanyong' style='display: none; width:85px;'> &nbsp;</span></td><td class='BSP txt-l'><input type='checkbox' checked='checked' id='bsp_" + i + "' /><label for='bsp_" + i + "'>BSP</label></td><td class='canHaveSubordinate' style='display: none;'><input type='text' class='text text-s' maxlength='4' />%</td><td><input type='text' class='text text-s' maxlength='4' />%</td><td class='allowBrotherPurchase' style='display: none;'><input type='text' class='text text-s' maxlength='4' />%</td></tr><tr><td class='BunksRad'><label class='refBtnBunks btn class3'>点击获取舱位</label></td><td class='discount'> <select onchange='selPriceOrDiscount(this)' class='selectPrice'><option value='0'>按价格发布</option><option value='1'>按折扣发布</option><option value='3'>按返佣发布</option></select> <span class='price0'><input type='text' class='text text-s' maxlength='4' />元</span> <span class='discount0' style='display: none;'> <input type='text' class='text text-s' maxlength='3' />折</span><span class='fanyong' style='display: none; width:85px;'> &nbsp;</span></td><td class='B2B'><input type='checkbox' checked='checked'  id='b2b_" + i + "'  class='chkb2b'  /><label for='b2b_" + i + "'>B2B</label><br /><span style='margin-left:125px' class='qfqcp'><input type='checkbox' id='qfqcp_" + i + "' class='qfqcp' style='margin-left:20px;' /> <label for='qfqcp_" + i + "' class='qfqcp'>起飞前2小时内可出票</label></span></td><td class='canHaveSubordinate' style='display: none;'><input type='text' class='text text-s' maxlength='4' />%</td><td><input type='text' class='text text-s' maxlength='4' />%</td><td class='allowBrotherPurchase' style='display: none;'><input type='text' class='text text-s' maxlength='4' />%</td></tr></table><div class='clearfix'><div class='fl policy_check'><input type='checkbox' id='zjsh_" + i + "' /><label for='zjsh_" + i + "'>直接审核</label><input type='checkbox' id='hbmcp_" + i + "' /><label for='hbmcp_" + i + "'>需换编码出票</label><input type='checkbox' id='syddlc_" + i + "' class='syddlc'  style='display:none;'/> <label for='syddlc_" + i + "' class='syddlc' style='display:none;' >适用多段联程</label><span class='obvious1 wangfantishi' style='display: none;'>该价格为往返总价格（不含税费），价格不确定时可以为空</span><span class='obvious1 zejiagetishi' style='display: none;'>折扣以100为换算单位，如全价Y舱为100折</span></div><div class='fr'><input type='button' class='btn class2 btnpolicyaddgroup' value='添加新组政策&nbsp;&nbsp;+' /> <input type='button' class='btn class2' onclick='DelPolicyGroup(this);' value='删除本组政策&nbsp;&nbsp;×' /></div></div></div></div></div>";
        AddPolicyGroup(str, i);
        $(".quchengkaishi").eq(parseInt(i) - 1).val($(".quchengkaishi").eq(parseInt(i) - 2).val());
        $(".quchengjieshu").eq(parseInt(i) - 1).val($(".quchengjieshu").eq(parseInt(i) - 2).val());

        //        if ($(".quchengkaishi").eq(parseInt(i) - 2).val() != "" && $(".quchengjieshu").eq(parseInt(i) - 2).val() != "") {
        //            $(".parent_div").eq(parseInt(i) - 1).find(".btnRefresh").click();
        //        }
        ShowOrHides($(".navType2Selected").attr("id"));
    });

    $("#btnPublish").click(function () {
        var publicFlag = Vaildate();
        var baseFlag = false;
        if (publicFlag) {
            baseFlag = Vaildate_Base();
        }

        if (publicFlag && baseFlag) {
            //            if ($("#txtRemark").val() == "") {
            //                alert("备注信息不能为空！");
            //                return false;
            //            }
            if ($("#txtRemark").val().length > 200) {
                alert("备注信息不能超过200个字！");
                $("#txtRemark").val($("#txtRemark").val().substring(0, 200));
                return false;
            }
            GetPolicyValue("Bargain_Publish");
        }
    });
    $("#btnPolicyAdd").click(function () {
        var publicFlag = Vaildate();
        var baseFlag = false;
        if (publicFlag) {
            baseFlag = Vaildate_Base();
        }
        if (publicFlag && baseFlag) {
            //            if ($("#txtRemark").val() == "") {
            //                alert("备注信息不能为空！");
            //                return false;
            //            }
            if ($("#txtRemark").val().length > 200) {
                alert("备注信息不能超过200个字！");
                $("#txtRemark").val($("#txtRemark").val().substring(0, 200));
                return false;
            }
            GetPolicyValue("Ahead");
        }
    });
    $("#sel li a").click(function () {
        $("#sel li a").removeClass("navType2Selected");
        $(this).addClass("navType2Selected");
        ShowOrHides($(this).attr("id"));
        ClearBunksByAirline();
    });
});
function changGe() {
    $("#txtWangfanAirports").val($("#lbShifaSource").val());
};

function ShowOrHides(id) {
    if (id == "OneWay") {
        $(".wangfantishi").css("display", "none");
        $(".price0 input[type='text']").removeAttr("title");
        $(".discount").eq(0).html("折扣");
        $("#shifadi").css("display", "none");
        $("#paichu").css("display", "");
        $(".selectPrice").css("display", "");
        $(".discount").css("display", "");
        $("#huichengTh").css("display", "none");
        $(".classDisplay").css("display", "none");
        $(".chuxing").css("display", "none");
        $("#mudi").css("display", "none");
        $(".mudi").css("display", "none");
        $(".tiqian").css("display", "");
        $("#duihuan").css("display", "");
        $("#mudidi").css("display", "");
        $(".mudidi").css("display", "");
        $(".syddlc").css("display", "none");
        $("#qucheng").html("航班限制");
        $("#zhongzhuandi").html("目的地");
        for (var i = 0; i < $(".selectPrice").length; i++) {
            if ($(".selectPrice option:selected").eq(i).val() == "0") {
                $(".price0").eq(i).show();
                $(".discount0").eq(i).hide();
                $(".selectPrice").eq(i).html("<option value='0' selected='selected'>按价格发布</option><option value='1'>按折扣发布</option><option value='3'>按返佣发布</option>");
            }
            if ($(".selectPrice option:selected").eq(i).val() == "1") {
                $(".price0").eq(i).hide();
                $(".discount0").eq(i).show();
                $(".selectPrice").eq(i).html("<option value='0'>按价格发布</option><option value='1' selected='selected'>按折扣发布</option><option value='3'>按返佣发布</option>");
            }
            if ($(".selectPrice option:selected").eq(i).val() == "3") {
                $(".price0").eq(i).hide();
                $(".discount0").eq(i).hide();
                $(".selectPrice").eq(i).html("<option value='0'>按价格发布</option><option value='1'>按折扣发布</option><option value='3' selected='selected'>按返佣发布</option>");
            }
        }
        $(".zejiagetishi").css("display", "");
        $(".paichutishi").html("提示： 输入不适用本政策的始发和目的地，如：北京--济南行程不适用本政策，则输入PEKTNA，多个不适用航段用“ / ”隔开。");
        //查询作废规定
        url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
        actionName = "InvalidRegulation";
        ResquetQueryAction(url, actionName, { type: "BargainOneWayInvalidRegulation" });
        //查询改签规定
        url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
        actionName = "ChangeRegulation";
        ResquetQueryAction(url, actionName, { type: "BargainOneWayChangeRegulation" });
        //查询签转规定
        url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
        actionName = "EndorseRegulation";
        ResquetQueryAction(url, actionName, { type: "BargainOneWayEndorseRegulation" });
        //查询退票规定
        url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
        actionName = "RefundRegulation";
        ResquetQueryAction(url, actionName, { type: "BargainOneWayRefundRegulation" });

        //ClearBunksByAirline();
    }
    else if (id == "RoundTrip") {
        $("#txtOutWithFilght").val("");
        $(".price0 input[type='text']").attr("title", "该价格为往返总价格（不含税费），价格不确定时可以为空");
        $(".wangfantishi").css("display", "");
        $(".discount").eq(0).html("价格");
        $("#paichu").css("display", "none");
        $(".selectPrice").css("display", "");
        $(".discount").css("display", "");
        $("#shifadi").css("display", "");
        $("#huichengTh").css("display", "");
        $(".classDisplay").css("display", "");
        $("#duihuan").css("display", "");
        $(".chuxing").css("display", "");
        $(".tiqian").css("display", "");
        $("#mudidi").css("display", "none");
        $(".mudidi").css("display", "none");
        $(".syddlc").css("display", "none");
        $("#mudi").css("display", "");
        $(".mudi").css("display", "");
        $("#qucheng").html("去程航班");
        $("#huicheng").html("回程航班");
        $("#zhongzhuandi").html("目的地");
        //        $(".price0").show();
        //        for (var i = 0; i < $(".discount0").length; i++) {
        //            if ($(".discount0").eq(i).css("display") == "none") {
        //                $(".discount0").eq(i).hide();
        //                $(".price0").eq(i).show();
        //                $(".selectPrice").html("<option value='0'>按价格发布</option><option value='3'>按返佣发布</option>");
        //            } else {
        //                $(".selectPrice").html("<option value='0'>按价格发布</option><option value='3' selected='selected'>按返佣发布</option>");
        //                $(".price0").eq(i).hide();
        //                $(".discount0").eq(i).hide();
        //            }
        //        }
        for (var i = 0; i < $(".selectPrice").length; i++) {
            if ($(".selectPrice option:selected").eq(i).val() == "0") {
                $(".price0").eq(i).show();
                $(".discount0").eq(i).hide();
                $(".selectPrice").eq(i).html("<option value='0' selected='selected'>按价格发布</option><option value='3'>按返佣发布</option>");
            }
            else if ($(".selectPrice option:selected").eq(i).val() == "3") {
                $(".price0").eq(i).hide();
                $(".discount0").eq(i).hide();
                $(".selectPrice").eq(i).html("<option value='0'>按价格发布</option><option value='3' selected='selected'>按返佣发布</option>");
            } else {
                $(".price0").eq(i).show();
                $(".discount0").eq(i).hide();
                $(".selectPrice").eq(i).html("<option value='0' selected='selected'>按价格发布</option><option value='3'>按返佣发布</option>");
            }
        }
        $(".zejiagetishi").css("display", "none");

        //查询作废规定
        url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
        actionName = "InvalidRegulation";
        ResquetQueryAction(url, actionName, { type: "BargainRoundTripInvalidRegulation" });
        //查询改签规定
        url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
        actionName = "ChangeRegulation";
        ResquetQueryAction(url, actionName, { type: "BargainRoundTripChangeRegulation" });
        //查询签转规定
        url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
        actionName = "EndorseRegulation";
        ResquetQueryAction(url, actionName, { type: "BargainRoundTripEndorseRegulation" });
        //查询退票规定
        url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
        actionName = "RefundRegulation";
        ResquetQueryAction(url, actionName, { type: "BargainRoundTripRefundRegulation" });

        //ClearBunksByAirline();
    }
    else if (id == "TransitWay") {
        $(".wangfantishi").css("display", "none");
        $(".price0 input[type='text']").removeAttr("title");
        $("#paichu").css("display", "");
        $("#shifadi").css("display", "none");
        $(".discount").css("display", "none");
        $("#duihuan").css("display", "none");
        $("#mudidi").css("display", "");
        $(".mudidi").css("display", "");
        $("#mudi").css("display", "");
        $(".mudi").css("display", "");
        $("#huichengTh").css("display", "");
        $(".classDisplay").css("display", "");
        $(".chuxing").css("display", "none");
        $(".tiqian").css("display", "none");
        $(".syddlc").css("display", "");
        $("#qucheng").html("第一程航班");
        $("#huicheng").html("第二程航班");
        $("#zhongzhuandi").html("中转地");
        $(".paichutishi").html("提示： 输入排除航线，多条航线之间用“ / ”隔开，（如：昆明到广州到杭州不适用，填写KMGCANHGH）");
        $(".zejiagetishi").css("display", "none");
        //查询作废规定
        url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
        actionName = "InvalidRegulation";
        ResquetQueryAction(url, actionName, { type: "BargainTransitWayInvalidRegulation" });
        //查询改签规定
        url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
        actionName = "ChangeRegulation";
        ResquetQueryAction(url, actionName, { type: "BargainTransitWayChangeRegulation" });
        //查询签转规定
        url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
        actionName = "EndorseRegulation";
        ResquetQueryAction(url, actionName, { type: "BargainTransitWayEndorseRegulation" });
        //查询退票规定
        url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
        actionName = "RefundRegulation";
        ResquetQueryAction(url, actionName, { type: "BargainTransitWayRefundRegulation" });

        //ClearBunksByAirline();
    }
}

function selPriceOrDiscount(parame) {
    if ($(parame).val() == "0") {
        $(parame).parent().find(".price0").show();
        $(parame).parent().find(".fanyong").hide();
        $(parame).parent().find(".discount0").hide();

    }
    if ($(parame).val() == "1") {
        $(parame).parent().find(".price0").hide();
        $(parame).parent().find(".fanyong").hide();
        $(parame).parent().find(".discount0").show();
    }
    if ($(parame).val() == "3") {
        $(parame).parent().find(".price0").hide();
        $(parame).parent().find(".discount0").hide();
        $(parame).parent().find(".fanyong").css("display", "inline-block");
    }
}
function GetPolicyValue(actionName) {
    var policyArrayBase;
    var policyArrayGroup = new Array();

    var airline = $("#selProvince option:selected").val();
    var office = $("#selOffice option:selected").val();
    var impowerOffice = $("#selOffice option:selected").attr("impower");
    var customCode = $.trim($("#selZidingy option:selected").val()) == "0" ? "" : $("#selZidingy option:selected").val();
    var tripType = $(".navType2Selected").attr("id");
    //出发地
    var departureAirports = "";
    //中转城市
    var transit = "";
    //目的地
    var arrivalAirports = "";
    if (tripType == "OneWay") {
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
    } else if (tripType == "RoundTrip") {
        var obj = $(".diaohuan").eq(0);
        var obj1 = $(".diaohuan").eq(1);
        //如果不为空就出发地是单
        if (obj.find("#txtWangfanAirports").val() != null) {
            departureAirports = obj.find("#txtWangfanAirports").val();

            if (obj1.find("input[name='radInculd2']:checked").val() == "0") {
                arrivalAirports = obj1.find("#txtAirports2").val();
            } else if (obj1.find("input[name='radInculd2']:checked").val() == "1") {
                $("option", obj1.find("#lbSource2")).each(function (index) {
                    if (index > 0) {
                        arrivalAirports += "/";
                    }
                    arrivalAirports += $(this).attr("value");
                });
            }

        } else {
            if (obj.find("input[name='radInculd2']:checked").val() == "0") {
                departureAirports = obj.find("#txtAirports2").val();
            } else if (obj.find("input[name='radInculd2']:checked").val() == "1") {
                $("option", obj.find("#lbSource2")).each(function (index) {
                    if (index > 0) {
                        arrivalAirports += "/";
                    }
                    arrivalAirports += $(this).attr("value");
                });
            }
            arrivalAirports = obj1.find("#txtWangfanAirports").val();
        }
    } else if (tripType == "TransitWay") {
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

    //排除航线
    var exceptAirways = $("#txtOutWithFilght").val();


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

    //回程航班限制类型
    var returnFilghtType = "";
    if ($("input[name='ReturnFilght']:checked").val() == "0") {
        returnFilghtType = "None";
    }
    if ($("input[name='ReturnFilght']:checked").val() == "1") {
        returnFilghtType = "Include";
    }
    if ($("input[name='ReturnFilght']:checked").val() == "2") {
        returnFilghtType = "Exclude";
    }
    //回程航班
    var returnFilght = $("#txtReturnFilght").val();
    if (tripType == "OneWay") {
        returnFilghtType = "None";
        returnFilght = "";
    }

    //作废规定
    var invalidRegulation = $("#selInvalidRegulation option:selected").text();
    //改签规定
    var changeRegulation = $("#selChangeRegulation option:selected").text();
    //签转规定
    var endorseRegulation = $("#selEndorseRegulation option:selected").text();
    //退票规定
    var refundRegulation = $("#selRefundRegulation option:selected").text();
    //备注
    var remark = $("#txtRemark").val();
    //出票条件
    var drawerCondition = $("#txtDrawerCondition").val();

    //政策组
    for (var k = 0; k < $(".parent_div").length; k++) {
        var policyDepartureFilghtDataStart = $(".parent_div").eq(k).find("input[type='text']").eq(0).val();
        var policyDepartureFilghtDataEnd = $(".parent_div").eq(k).find("input[type='text']").eq(1).val();
        var policyStartPrintDate = $(".parent_div").eq(k).find("input[type='text']").eq(2).val();

        var departureDateFilter = $(".parent_div").eq(k).find("input[type='text']").eq(3).val();

        var beforehandDays = $(".parent_div").eq(k).find("input[type='text']").eq(4).val() == "" ? 0 : $(".parent_div").eq(k).find("input[type='text']").eq(4).val();
        var mostBeforehandDays = $(".parent_div").eq(k).find("input[type='text']").eq(5).val() == "" ? -1 : $(".parent_div").eq(k).find("input[type='text']").eq(5).val();
        var travelDays = $(".parent_div").eq(k).find("input[type='text']").eq(6).val() == "" ? 0 : $(".parent_div").eq(k).find("input[type='text']").eq(6).val();
        if (tripType == "OneWay") {
            travelDays = 0;
        }
        else if (tripType == "TransitWay") {
            beforehandDays = 0;
            mostBeforehandDays = -1;
            travelDays = 0;
        }
        var departureWeekFilter = "";
        for (var s = 0; s < $(".parent_div").eq(k).find(".pd_left input[type='checkbox']:checked").length; s++) {
            if (departureWeekFilter != "") {
                departureWeekFilter += ",";
            }
            departureWeekFilter += $(".parent_div").eq(k).find(".pd_left input[type='checkbox']:checked").eq(s).val();
        }
        var autoAudit = $(".parent_div").eq(k).find(".groupBox2 .policy_check  input[type='checkbox']").eq(0).is(":checked");
        var changePnr = $(".parent_div").eq(k).find(".groupBox2 .policy_check  input[type='checkbox']").eq(1).is(":checked");
        var multiSuitReduce = $(".parent_div").eq(k).find(".policy_check input[type='checkbox']").eq(2).is(":checked");
        var printBeforeTwoHours = false;
        //返佣信息 
        var b2BInternalCommission = "";
        var b2BSubordinateCommission = "";
        var b2BProfessionCommission = "";
        var b2BBerths = "";
        var b2BPriceType = "";
        var b2BPriceOrDiscount = "";
        var bspInternalCommission = "";
        var bspSubordinateCommission = "";
        var bspProfessionCommission = "";
        var bspBerths = "";
        var bspPriceType = "";
        var bspPriceOrDiscount = "";


        if ($(".parent_div").eq(k).find(".groupBox2 table tr .BSP >input[type='checkbox']").is(":checked")) {
            bspBerths = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".BunksRad select option:selected").val();
            if ($(".canHaveSubordinate").css("display") == "none") {
                bspInternalCommission = -1;
            } else {
                bspInternalCommission = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val()) ;
            }
            bspSubordinateCommission = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val()) ;
            if ($(".allowBrotherPurchase").css("display") == "none") {
                bspProfessionCommission = -1;
            } else {
                bspProfessionCommission = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val()) ;
            }
            if ($(".discount").css("display") == "none") {
                bspPriceType = "Price";
                bspPriceOrDiscount = -1;
            } else if ($(".selectPrice").css("display") == "none") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val() == "") {
                    bspPriceType = "Price";
                    bspPriceOrDiscount = -1;
                } else {
                    bspPriceType = "Price";
                    bspPriceOrDiscount = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val();
                }
            } else {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".selectPrice option:selected").val() == "0") {
                    bspPriceType = "Price";
                    bspPriceOrDiscount = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val());
                } else if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".selectPrice option:selected").val() == "1") {
                    bspPriceType = "Discount";
                    bspPriceOrDiscount = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val()) ;
                } else if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".selectPrice option:selected").val() == "3") {
                    bspPriceType = "Commission";
                    bspPriceOrDiscount = -1;
                }
            }
            if ($(".navType2Selected").attr("id") == "TransitWay") {
                bspPriceType = "Commission";
                bspPriceOrDiscount = -1;
            }
            policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "StartPrintDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "Berths": bspBerths, "TicketType": "BSP", "InternalCommission": bspInternalCommission, "SubordinateCommission": bspSubordinateCommission, "ProfessionCommission": bspProfessionCommission, "PriceType": bspPriceType, "BeforehandDays": beforehandDays, "MostBeforehandDays": mostBeforehandDays, "TravelDays": travelDays, "Price": bspPriceOrDiscount, "AutoAudit": autoAudit, "ChangePNR": changePnr, "MultiSuitReduce": multiSuitReduce, "PrintBeforeTwoHours": false });
        }
        if ($(".parent_div").eq(k).find(".groupBox2 table tr td .chkb2b").is(":checked")) {
            printBeforeTwoHours = $(".parent_div").eq(k).find(".groupBox2 table tr td .qfqcp").is(":checked");
            b2BBerths = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".BunksRad select option:selected").val();
            if ($(".canHaveSubordinate").css("display") == "none") {
                b2BInternalCommission = -1;
            } else {
                b2BInternalCommission = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val()) ;
            }
            b2BSubordinateCommission = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(3).val()) ;
            if ($(".allowBrotherPurchase").css("display") == "none") {
                b2BProfessionCommission = -1;
            } else {
                b2BProfessionCommission = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(4).val()) ;
            }
            if ($(".discount").css("display") == "none") {
                b2BPriceType = "Price";
                b2BPriceOrDiscount = -1;
            } else if ($(".selectPrice").css("display") == "none") {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val() == "") {
                    b2BPriceType = "Price";
                    b2BPriceOrDiscount = -1;
                } else {
                    b2BPriceType = "Price";
                    b2BPriceOrDiscount = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val();
                }
            } else {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".selectPrice option:selected").val() == "0") {
                    b2BPriceType = "Price";
                    b2BPriceOrDiscount = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val());
                }
                else if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".selectPrice option:selected").val() == "1") {
                    b2BPriceType = "Discount";
                    b2BPriceOrDiscount = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val()) ;
                }
                else if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".selectPrice option:selected").val() == "3") {
                    b2BPriceType = "Commission";
                    b2BPriceOrDiscount = -1;
                }
            } 
            if ($(".navType2Selected").attr("id") == "TransitWay") {
                b2BPriceType = "Commission";
                b2BPriceOrDiscount = -1;
            }
            policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "StartPrintDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "Berths": b2BBerths, "TicketType": "B2B", "InternalCommission": b2BInternalCommission, "SubordinateCommission": b2BSubordinateCommission, "ProfessionCommission": b2BProfessionCommission, "PriceType": b2BPriceType, "BeforehandDays": beforehandDays, "MostBeforehandDays": mostBeforehandDays, "TravelDays": travelDays, "Price": b2BPriceOrDiscount, "AutoAudit": autoAudit, "ChangePNR": changePnr, "MultiSuitReduce": multiSuitReduce, "PrintBeforeTwoHours": printBeforeTwoHours });
        }
    }

    policyArrayBase = { "Bargain": { "BasicInfo": { "Airline": airline, "OfficeCode": office, "CustomCode": customCode, "ExceptAirways": exceptAirways, "ImpowerOffice": impowerOffice, "VoyageType": tripType, "Departure": departureAirports, "Transit": transit, "Arrival": arrivalAirports, "DepartureDatesFilter": departureDate, "DepartureDatesFilterType": departureDateType, "ReturnFlightsFilterType": returnFilghtType, "ReturnFlightsFilter": returnFilght, "DepartureFlightsFilterType": departureFilghtType, "DepartureFlightsFilter": departureFilght, "InvalidRegulation": invalidRegulation, "EndorseRegulation": endorseRegulation, "RefundRegulation": refundRegulation, "ChangeRegulation": changeRegulation, "Remark": remark, "DrawerCondition": drawerCondition }, "Rebates": policyArrayGroup} };

    //发布特价政策
    var url = "/PolicyHandlers/RoleGeneralPolicy.ashx/RegisterSpecialOfferPolicy";

    ResquetQueryAction(url, actionName, policyArrayBase);
};