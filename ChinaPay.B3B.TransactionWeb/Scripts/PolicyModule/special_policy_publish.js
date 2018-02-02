$(function () {
    var url;
    var actionName;
    //查询特殊政策类型
    url = "/PolicyHandlers/PolicyManager.ashx/QuerySpecialProductTypeList";
    actionName = "QuerySpecialProductTypeList";
    ResquetQueryAction(url, actionName, null);

    //查询Office号
    url = "/PolicyHandlers/RoleGeneralPolicy.ashx/GetOfficeNumbers";
    actionName = "Office";
    ResquetQueryAction(url, actionName, null);
    //    //查询作废规定
    //    url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
    //    actionName = "InvalidRegulation";
    //    ResquetQueryAction(url, actionName, { type: "SpecialSinglenessInvalidRegulation" });
    //    //查询改签规定
    //    url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
    //    actionName = "ChangeRegulation";
    //    ResquetQueryAction(url, actionName, { type: "SpecialSinglenessChangeRegulation" });
    //    //查询签转规定
    //    url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
    //    actionName = "EndorseRegulation";
    //    ResquetQueryAction(url, actionName, { type: "SpecialSinglenessEndorseRegulation" });
    //    //查询退票规定
    //    url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
    //    actionName = "RefundRegulation";
    //    ResquetQueryAction(url, actionName, { type: "SpecialSinglenessRefundRegulation" });
    //    //查询出票条件
    //    url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryRegulations";
    //    actionName = "DrawerCondition";
    //    ResquetQueryAction(url, actionName, { type: "SpecialSinglenessDrawerCondition" });

    AirportsArrivals();
    AirportsDepartures();
    AirportsZhongZhuanDepartures();

    $("#txtShifaAirports").live("blur", function () {
        var codePattern = $.trim($("#txtShifaAirports").val()).toUpperCase();
        for (var i = 0; i < $("#lbShifaSource option").length; i++) {
            if ($("#lbShifaSource option").eq(i).val() == codePattern) {
                $("#lbShifaSource").val(codePattern);
                $("#txtShifaAirports").val(codePattern);
                break;
            } else {
                $("#txtShifaAirports").val("");
                $("#lbShifaSource").val("");
            }
        }
    });
    $("#txtShifaAirports").live("keyup", function () {
        var codePattern = $.trim($("#txtShifaAirports").val()).toUpperCase();
        $("#txtShifaAirports").val(codePattern);
        $("#lbShifaSource").val(codePattern);
    });
    $("#txtWangfanAirports").live("blur", function () {
        var codePattern = $.trim($("#txtWangfanAirports").val()).toUpperCase();
        for (var i = 0; i < $("#lbWangfanSource option").length; i++) {
            if ($("#lbWangfanSource option").eq(i).val() == codePattern) {
                $("#txtWangfanAirports").val(codePattern);
                $("#lbWangfanSource").val(codePattern);
                break;
            } else {
                $("#txtWangfanAirports").val("");
                $("#lbWangfanSource").val("");
            }
        }
    });
    $("#lbWangfanSource").live("change", function () {
        $("#txtWangfanAirports").val($("#lbWangfanSource").val());
    });

    $(".btnpolicyaddgroup").live("click", function () {
        var i = $(".parent_div").length;
        i++;
        var str = "<div class='parent_div'>  <br /><h4 class='groupHd'> 第&nbsp;" + i + "&nbsp;组政策 </h4><div class='groupCt' style='min-height:370px;'><div class='groupBox1'><div class='clearfix'><div class='fl pd_right groupLine'> 适用航班日期： <input type='text' onchange='ClearBunksByTime(this);' class='text datepicker datefrom  class3  quchengkaishi' /> 至 <input type='text' onchange='ClearBunksByTime(this);' class='text datepicker datefrom  class3  quchengjieshu' /> </div> <div class='fl pd_right groupLine'> 开始出票日期： <input type='text' onchange='ClearBunksByTime(this);' class='text datepicker datefrom  class3  chupiao' />  </div> <div class='fl groupLine btnRefreshWangfan' style='display: none;'><input type='button' class='btn class2 btnRefresh' value='刷新舱位' /> </div> </div> <div class='groupLine'> 航班排除日期：  <input type='text' class='text text_width' />  <p class='obvious1 pd'>请填写政策航班排除日期，单天如：20121121 连续多天如:20121125-20121127 多个天数用“,”隔开</p> </div><div class='pd_left groupLine'> 适用班期： <input type='checkbox' id='mon_" + i + "' value='1' checked='checked'  /> <label for='mon_" + i + "'> 周一</label> <input type='checkbox' id='tue_" + i + "' value='2' checked='checked' /> <label for='tue_" + i + "'> 周二</label> <input type='checkbox' id='wed_" + i + "' value='3'  checked='checked' />  <label for='wed_" + i + "'> 周三</label> <input type='checkbox' id='thur_" + i + "' value='4' checked='checked'  /> <label for='thur_" + i + "'> 周四</label> <input  type='checkbox' id='fri_" + i + "' value='5' checked='checked'  /> <label for='fri_" + i + "'> 周五</label> <input type='checkbox' id='sat_" + i + "' value='6' checked='checked'  /> <label for='sat_" + i + "'> 周六</label> <input type='checkbox'  id='sun_" + i + "' value='7' checked='checked'  /> <label for='sun_" + i + "'> 周日</label> <span class='obvious1'>提示：若某个班期不适用，请将周期前的勾去掉</span> </div>  <div class='pd_left groupLine'> 提前天数：  <input type='text' class='text text-s' /> 天 <span  class='obvious1'>提示：如果有提前订票限制，请输入限制天数 </span>  </div><div class='pd_left groupLine jituan'> 舱位选择：<input type='radio' id= 'normal_" + i + "' checked='checked' name='chooiceBunks_" + i + "' value='0' /><label for='normal_" + i + "'>普通舱位</label> <input type='radio' id='bargain_" + i + "' name='chooiceBunks_" + i + "' value='1' /><label for='bargain_" + i + "'>特价舱位</label> </div> <div class='pd_left groupLine heipingtongbu' style='display: none;'> 黑屏同步： <div  class='pd_left groupLine'> <input id='hptb_" + i + "' type='radio' name='tongbu_" + i + "' class='hptb1' value='0'    /><label for='hptb_" + i + "'>同&nbsp;&nbsp; 步</label><span  class='BunksRad hptb_" + i + " shuliangBunks' style='display: none;'><label class='refBtnBunks btn class3'>点击获取舱位</label></span> <span  class='obvious1'>提示：选择同步后系统将在用户导入编码或查询时自动查询剩余机票张数 </span> </div> <div class='pd_left groupLine'> <input id='bhptb_" + i + "' type='radio'  name='tongbu_" + i + "' class='hptb1' value='1'    /><label for='bhptb_" + i + "'>不同步</label> <span class='obvious1'>提示：如果您能确保为购买该政策机票的客户出票，则选择不同步 </span><div class='pd_left groupLine shuliangBunks bhptb_" + i + "' style='display: none;'> <input id='youwei" + i + "' type='radio'  value='0' name='youwei" + i + "' /><label for='youwei" + i + "'>有位出票</label> <span class='obvious1'>提示：须保证编码状态为OK </span> <br /> <input id='wuwei" + i + "' type='radio'  value='1' name='youwei" + i + "' /><label for='wuwei" + i + "'>无位出票</label> <span class='obvious1'>提示：即候补状态可以保证出票 </span></div> </div> </div><div class='pd_left groupLine chanpinBunks'> 产品舱位： <input type='text' class='text text-s' maxlength='2' /><span class='obvious1 chanpinPrice'>&nbsp;&nbsp;&nbsp;&nbsp;提示：请输入产品舱位 </span> </div> <div class='pd_left groupLine shuliang shuliangBunks bhptb_" + i + "'> 提供产品数量： <input  type='text' class='text text-s' /> 张&nbsp;&nbsp;&nbsp;&nbsp; <span class='obvious1 tishi'>提示：请输入1-9的纯数字 </span> </div><div class='clearfix'><div class='fl pd_left groupLine chanpinPrice neibu' style='display: none;'> 内部结 算 价 ： <input type='text' class='text text-s' maxlength='5'  /> 元  </div> <div class='fl pd_left groupLine chanpinPrice xiaji' style='display: none;'> 下级结 算 价 ： <input type='text' class='text text-s' maxlength='5'  /> 元 </div> <div class='fl pd_left groupLine chanpinPrice tonghang' style='display: none;'>  同行结 算 价 ： <input type='text' class='text text-s'  maxlength='5' /> 元 <span class='obvious1 chanpinPrice'>&nbsp;&nbsp;&nbsp;&nbsp;提示：请输入整数的价格 </span> </div></div></div> <div  class='groupBox2'> <table class='wanfangTable' style='display: none;'>  <tr> <th> 舱位 </th> <th class='discount'> 票面价格 </th> <th class='shanglvka'> 客票类型 </th>  <th class='canHaveSubordinate' style='display: none;'> 内部结算价 </th>  <th> 下级结算价 </th>  <th class='allowBrotherPurchase' style='display: none;'> 同行结算价 </th> </tr> <tr> <td><div class='BunksRad'><label class='refBtnBunks btn class3'> 点击获取舱位</label></div> <div class='BunksBargain' style='display:none;'><label class='refBtnBunks btn class3'>点击获取舱位</label></div></td>  <td class='discount'>  <select onchange='selPriceOrDiscount(this)' class='selectPrice'> <option value='0'>按价格发布</option> <option value='1'>按直减发布</option> </select> <span class='price0'> <input type='text' class='text text-s' maxlength='5' />元</span> <span class='discount0' style='display: none;'> <input type='text' class='text text-s' maxlength='5' />%</span> </td> <td class='BSP txt-l' style='text-align:center;'> <input type='checkbox' checked='checked' id='bsp_" + i + "' /> <label for='bsp_" + i + "'> BSP</label> </td>  <td class='canHaveSubordinate' style='display: none;'><span class='zhijianSpan' style='visibility:hidden;'>直减</span> <input type='text' class='text text-s' maxlength='5' /><span class='yaunOrzhijian'>元</span> </td> <td><span class='zhijianSpan' style='visibility:hidden;'>直减</span> <input type='text' class='text text-s' maxlength='5' /><span class='yaunOrzhijian'>元</span> </td> <td class='allowBrotherPurchase' style='display: none;'><span class='zhijianSpan' style='visibility:hidden;'>直减</span> <input type='text' class='text text-s' maxlength='5' /><span class='yaunOrzhijian'>元</span> </td> </tr> <tr class='shanglvka'> <td><div class='BunksRad'><label class='refBtnBunks btn class3'> 点击获取舱位</label></div> <div class='BunksBargain' style='display:none;'><label class='refBtnBunks btn class3'>点击获取舱位</label></div></td> <td class='discount'> <select onchange='selPriceOrDiscount(this)' class='selectPrice'> <option value='0'>按价格发布</option> <option value='1'>按直减发布</option>  </select> <span class='price0'> <input type='text' class='text text-s' maxlength='5' />元</span> <span class='discount0' style='display: none;'> <input type='text' class='text text-s' maxlength='5' />%</span> </td> <td class='B2B'> <input type='checkbox' checked='checked' id='b2b_" + i + "' class='chkb2b' /> <label for='b2b_" + i + "'> B2B</label><br /><span style='margin-left:125px' class='qfqcp'><input type='checkbox' id='qfqcp_" + i + "' class='qfqcp' style='margin-left:20px;' /> <label for='qfqcp_" + i + "' class='qfqcp'>起飞前2小时内可出票</label></span> </td> <td class='canHaveSubordinate' style='display: none;'><span class='zhijianSpan' style='visibility:hidden;'>直减</span><input type='text' class='text text-s' maxlength='5' /><span class='yaunOrzhijian'>元</span>  </td> <td><span class='zhijianSpan' style='visibility:hidden;'>直减</span> <input type='text' class='text text-s' maxlength='5' /><span class='yaunOrzhijian'>元</span> </td> <td class='allowBrotherPurchase' style='display: none;'> <span class='zhijianSpan' style='visibility:hidden;'>直减</span><input type='text' class='text text-s' maxlength='5' /><span class='yaunOrzhijian'>元</span> </td> </tr></table><div class='clearfix'> <div class='fl policy_check'> <input type='checkbox' id='zjsh_" + i + "' /> <label for='zjsh_" + i + "'> 直接审核</label><input type='checkbox' class='xqrzy' id='xqrzy_" + i + "' /> <label for='xqrzy_" + i + "' class='xqrzy' > 采购时需要确认座位</label><span class='obvious1 querenzuowei'>即采购选择政策后暂不支付，待您确认有座时再进行支付，再进行出票</span><br/><input type='checkbox' onclick='chkDjbc(this)' id='chkdjdf' class='djdf' /><label for='chkdjdf' class='djdf'> 价格限制</label> <span class='djdftxt djdf' style='display: none;'>直减后的票面价限制区间，<input type='text' class='text text-s' maxlength='5'  /><b style='color:Red;'> * </b>元(包含) 至<input type='text' class='text text-s' maxlength='5'  />元(包含)</span><br /><span class='obvious1 djdf chkdjdftip'>您可以设置您所愿意出票的票面价区间；后面一个输入框留空则表示不限票面价；如1000-留空表示仅出票面价1000以上的航线</span></div> <div class='fr'> <input type='button' class='btn class2 btnpolicyaddgroup' value='添加新组政策&nbsp;&nbsp;+' />  <input type='button' class='btn class2' onclick='DelPolicyGroup(this);' value='删除本组政策&nbsp;&nbsp;×' />  </div>  </div></div> </div> </div>";
        AddPolicyGroup(str, i);
        $(".quchengkaishi").eq(parseInt(i) - 1).val($(".quchengkaishi").eq(parseInt(i) - 2).val());
        $(".quchengjieshu").eq(parseInt(i) - 1).val($(".quchengjieshu").eq(parseInt(i) - 2).val());
        //        if ($(".quchengkaishi").eq(parseInt(i) - 2).val() != "" && $(".quchengjieshu").eq(parseInt(i) - 2).val() != "") {
        //            $(".parent_div").eq(parseInt(i) - 1).find(".btnRefresh").click();
        //        }
        var id = $(".navType2Selected").attr("id");
        ShowOrHides1(id);
    });
});

function changGe() {
    $("#txtShifaAirports").val($("#lbShifaSource").val());
}
function selPriceOrDiscount(parame) {
    if ($(parame).val() == "0") {
        $(parame).parent().parent().find(".yaunOrzhijian").html("元");
        $(parame).parent().parent().find(".zhijianSpan").css("visibility", "hidden"); 
        $(parame).parent().find(".price0").show();
        $(parame).parent().find(".discount0").hide();
    }
    if ($(parame).val() == "1") {
        $(parame).parent().parent().find(".yaunOrzhijian").html("%");
        $(parame).parent().parent().find(".zhijianSpan").css("visibility", "");
        $(parame).parent().find(".price0").hide();
        $(parame).parent().find(".discount0").show();
    }
    if ($(".navType2Selected").attr("id") == "4") {
        $(".djbc").css("display", "none");
        $(".djdf").css("display", "none");
    } else {
        if ($(parame).parent().parent().parent().find(".selectPrice").eq(0).val() == "0" && $(parame).parent().parent().parent().find(".selectPrice").eq(1).val() == "0") {
            $(parame).parent().parent().parent().parent().parent().find(".djbc").css("display", "none");
            $(parame).parent().parent().parent().parent().parent().find(".djdf").css("display", "none");
//            alert($(parame).parent().parent().parent().parent().html());
        } else {
            $(parame).parent().parent().parent().parent().parent().find(".djbc").css("display", "");
            $(parame).parent().parent().parent().parent().parent().find(".djdf").css("display", "");
            $(parame).parent().parent().parent().parent().parent().find(".djbctxt").css("display", "none");
            $(parame).parent().parent().parent().parent().parent().find(".djdftxt").css("display", "none");
        }
    }
    $(parame).parent().parent().parent().parent().parent().find(".djbc").removeAttr("checked");
    $(parame).parent().parent().parent().parent().parent().find(".djdf").removeAttr("checked");
}
function GetPolicyValue(actionName) {
    var policyArrayBase;
    var policyArrayGroup = new Array();
    //特殊产品类型
    var productType = $(".navType2Selected").attr("id");

    var airline = $("#selProvince").val();
    var tripType = "OneWay";
    var office = $.trim($("#selOffice option:selected").val()) == "0" ? "" : $("#selOffice option:selected").val();
    var impowerOffice = $("#selOffice option:selected").attr("impower");
    var customCode = $.trim($("#selZidingy option:selected").val()) == "0" ? "" : $("#selZidingy option:selected").val();
    //出发地
    var departureAirports = "";

    //目的地
    var arrivalAirports = "";
    //排除航线
    var exceptAirways = $("#txtOutWithFilght").val();
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
    //航班限制
    var departureFilght = $("#txtQuChengFilght").val();
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
    var drawerCondition = "";

    //单程控位政策
    if (productType == "0") {
        departureAirports = $("#txtShifaAirports").val();
        arrivalAirports = $("#txtWangfanAirports").val();
        drawerCondition = $("#selDrawerCondition option:selected").val();

    } //散冲团
    else if (productType == "1") {
        departureAirports = $("#txtShifaAirports").val();
        arrivalAirports = $("#txtWangfanAirports").val();
        drawerCondition = $("#selDrawerCondition option:selected").val();
    } //免票 
    else if (productType == "2") {
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
        drawerCondition = $("#selDrawerCondition option:selected").val();
    }
    //集团产品
    else if (productType == "3") {
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
        drawerCondition = $("#txtDrawerCondition").val();
    }
    //商旅卡
    else if (productType == "4" || productType == "6") {
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
        drawerCondition = $("#txtDrawerCondition").val();
    } //免票 
    else if (productType == "5") {
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
        drawerCondition = $("#selDrawerCondition option:selected").val();
    }


    //政策组
    for (var k = 0; k < $(".parent_div").length; k++) {
        var policyDepartureFilghtDataStart = "";
        var policyDepartureFilghtDataEnd = "";
        var policyStartPrintDate = "";
        var departureDateFilter = "";
        var before = "";
        var price = "";
        var resourceAmount = "";
        var synBlackScreen = false;
        //返佣信息
        var ticketType = "";
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
        var lowNoType = 0;
        var lowNoMinPrice = -1;
        var lowNoMaxPrice = -1;
        var departureWeekFilter = "";
        var printBeforeTwoHours = false;
        for (var s = 0; s < $(".parent_div").eq(k).find(".pd_left input[type='checkbox']:checked").length; s++) {
            if (departureWeekFilter != "") {
                departureWeekFilter += ",";
            }
            departureWeekFilter += $(".parent_div").eq(k).find(".pd_left input[type='checkbox']:checked").eq(s).val();
        }

        var autoAudit = $(".parent_div").eq(k).find(".policy_check input[type='checkbox']").eq(0).is(":checked");
        var confirmResource = $(".parent_div").eq(k).find(".policy_check input[type='checkbox']").eq(1).is(":checked");
        //是否是特价舱位
        var IsBargainBerths = $(".parent_div").eq(k).find(".jituan input[type='radio']:checked").val() == "1";
        //单程控位政策
        if (productType == "0") {
            policyDepartureFilghtDataStart = $(".parent_div").eq(k).find("input[type='text']").eq(0).val();
            policyDepartureFilghtDataEnd = $(".parent_div").eq(k).find("input[type='text']").eq(1).val();
            policyStartPrintDate = $(".parent_div").eq(k).find("input[type='text']").eq(2).val();
            departureDateFilter = $(".parent_div").eq(k).find("input[type='text']").eq(3).val();
            before = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val() == "" ? "0" : $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val();
            resourceAmount = $(".parent_div").eq(k).find("input[type='text']").eq(6).val();

            price = "-1";
            b2BInternalCommission = $(".parent_div").eq(k).find("input[type='text']").eq(7).val() == "" ? "-1" : $(".parent_div").eq(k).find("input[type='text']").eq(7).val();
            b2BSubordinateCommission = $(".parent_div").eq(k).find("input[type='text']").eq(8).val() == "" ? "-1" : $(".parent_div").eq(k).find("input[type='text']").eq(8).val();
            b2BProfessionCommission = $(".parent_div").eq(k).find("input[type='text']").eq(9).val() == "" ? "-1" : $(".parent_div").eq(k).find("input[type='text']").eq(9).val();
            b2BBerths = "";
            b2BPriceType = "Price";
            ticketType = "B2B";
            synBlackScreen = false;
            IsSeat = false;
            IsBargainBerths = false;
            policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "ProvideDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "BeforehandDays": before, "SynBlackScreen": synBlackScreen, "Berths": b2BBerths, "TicketType": ticketType, "Price": price, "InternalCommission": b2BInternalCommission, "SubordinateCommission": b2BSubordinateCommission, "ProfessionCommission": b2BProfessionCommission, "PriceType": b2BPriceType, "AutoAudit": autoAudit, "ResourceAmount": resourceAmount, "ConfirmResource": confirmResource, "IsBargainBerths": IsBargainBerths, "PrintBeforeTwoHours": "false", "LowNoType": lowNoType, "LowNoMaxPrice": lowNoMaxPrice, "LowNoMinPrice": lowNoMinPrice });
        } //散冲团
        else if (productType == "1") {
            policyDepartureFilghtDataStart = $(".parent_div").eq(k).find("input[type='text']").eq(0).val();
            policyDepartureFilghtDataEnd = $(".parent_div").eq(k).find("input[type='text']").eq(1).val();
            policyStartPrintDate = $(".parent_div").eq(k).find("input[type='text']").eq(2).val();
            departureDateFilter = $(".parent_div").eq(k).find("input[type='text']").eq(3).val();
            before = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val() == "" ? "0" : $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val();
            price = "-1";
            resourceAmount = $(".parent_div").eq(k).find("input[type='text']").eq(6).val();

            b2BInternalCommission = $(".parent_div").eq(k).find("input[type='text']").eq(7).val() == "" ? "-1" : $(".parent_div").eq(k).find("input[type='text']").eq(7).val();
            b2BSubordinateCommission = $(".parent_div").eq(k).find("input[type='text']").eq(8).val() == "" ? "-1" : $(".parent_div").eq(k).find("input[type='text']").eq(8).val();
            b2BProfessionCommission = $(".parent_div").eq(k).find("input[type='text']").eq(9).val() == "" ? "-1" : $(".parent_div").eq(k).find("input[type='text']").eq(9).val();
            b2BBerths = "";
            b2BPriceType = "Price";
            ticketType = "B2B";
            synBlackScreen = false;
            IsSeat = false;
            IsBargainBerths = false;
            policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "ProvideDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "BeforehandDays": before, "SynBlackScreen": synBlackScreen, "Berths": b2BBerths, "TicketType": ticketType, "Price": price, "InternalCommission": b2BInternalCommission, "SubordinateCommission": b2BSubordinateCommission, "ProfessionCommission": b2BProfessionCommission, "PriceType": b2BPriceType, "AutoAudit": autoAudit, "ResourceAmount": resourceAmount, "ConfirmResource": confirmResource, "IsBargainBerths": IsBargainBerths, "PrintBeforeTwoHours": "false", "LowNoType": lowNoType, "LowNoMaxPrice": lowNoMaxPrice, "LowNoMinPrice": lowNoMinPrice });
        } //免票 
        else if (productType == "2") {
            policyDepartureFilghtDataStart = $(".parent_div").eq(k).find("input[type='text']").eq(0).val();
            policyDepartureFilghtDataEnd = $(".parent_div").eq(k).find("input[type='text']").eq(1).val();
            policyStartPrintDate = $(".parent_div").eq(k).find("input[type='text']").eq(2).val();
            departureDateFilter = $(".parent_div").eq(k).find("input[type='text']").eq(3).val();
            before = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val() == "" ? "0" : $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val();
            price = -1;

            b2BInternalCommission = $(".parent_div").eq(k).find("input[type='text']").eq(7).val() == "" ? "-1" : $(".parent_div").eq(k).find("input[type='text']").eq(7).val();
            b2BSubordinateCommission = $(".parent_div").eq(k).find("input[type='text']").eq(8).val() == "" ? "-1" : $(".parent_div").eq(k).find("input[type='text']").eq(8).val();
            b2BProfessionCommission = $(".parent_div").eq(k).find("input[type='text']").eq(9).val() == "" ? "-1" : $(".parent_div").eq(k).find("input[type='text']").eq(9).val();
            b2BPriceType = "Price";
            ticketType = "B2B";
            IsBargainBerths = false;
            synBlackScreen = $(".parent_div .groupBox1").eq(k).find(".heipingtongbu input[type='radio']").eq(0).is(":checked");

            if (synBlackScreen) {
                resourceAmount = -1;
                b2BBerths = $(".parent_div").eq(k).find(".groupBox1 .BunksRad select option:selected").val();
                IsSeat = true;
                confirmResource = false;
            } else {
                resourceAmount = $(".parent_div").eq(k).find("input[type='text']").eq(6).val();
                b2BBerths = "";
                IsSeat = $(".parent_div .groupBox1").eq(k).find(".shuliangBunks input[type='radio']").eq(0).is(":checked");
            }
            policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "ProvideDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "BeforehandDays": before, "SynBlackScreen": synBlackScreen, "Berths": b2BBerths, "TicketType": ticketType, "Price": price, "InternalCommission": b2BInternalCommission, "SubordinateCommission": b2BSubordinateCommission, "ProfessionCommission": b2BProfessionCommission, "PriceType": b2BPriceType, "AutoAudit": autoAudit, "ResourceAmount": resourceAmount, "ConfirmResource": confirmResource, "IsBargainBerths": IsBargainBerths, "IsSeat": IsSeat, "PrintBeforeTwoHours": "false", "LowNoType": lowNoType,"LowNoMaxPrice": lowNoMaxPrice, "LowNoMinPrice": lowNoMinPrice });
        }
        //集团产品
        else if (productType == "3") {
            policyDepartureFilghtDataStart = $(".parent_div").eq(k).find("input[type='text']").eq(0).val();
            policyDepartureFilghtDataEnd = $(".parent_div").eq(k).find("input[type='text']").eq(1).val();
            policyStartPrintDate = $(".parent_div").eq(k).find("input[type='text']").eq(2).val();
            departureDateFilter = $(".parent_div").eq(k).find("input[type='text']").eq(3).val();
            before = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val() == "" ? "0" : $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val();
            if ($(".parent_div").eq(k).find(".policy_check input[type='checkbox']").eq(2).is(":checked")) {
                lowNoType = "1";
            }
//            lowNoType = $(".parent_div").eq(k).find(".policy_check input[type='checkbox']").eq(2).is(":checked") ? "1" : $(".parent_div").eq(k).find(".policy_check input[type='checkbox']").eq(3).is(":checked") ? "2" : "0";
            if (lowNoType == "1") {
                lowNoMinPrice = $(".parent_div").eq(k).find(".policy_check input[type='text']").eq(0).val();
                lowNoMaxPrice = $(".parent_div").eq(k).find(".policy_check input[type='text']").eq(1).val() == "" ? -1 : $(".parent_div").eq(k).find(".policy_check input[type='text']").eq(1).val();
            }
            resourceAmount = -1;
            if ($(".parent_div").eq(k).find(".groupBox2 table tr td[class='BSP txt-l']>input[type='checkbox']").is(":checked")) {
                if (IsBargainBerths) {
                    bspBerths = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".BunksBargain select option:selected").val();
                } else {
                    for (g = 0; g < $(".parent_div").eq(k).find(".groupBox2 table tr td .BunksRad").eq(0).find("input[type='checkbox']:checked").length; g++) {
                        if (g > 0) {
                            bspBerths += ",";
                        }
                        bspBerths += $(".parent_div").eq(k).find(".groupBox2 table tr td .BunksRad").eq(0).find("input[type='checkbox']:checked").eq(g).val();
                    }
                }
                if ($(".canHaveSubordinate").css("display") == "none") {
                    bspInternalCommission = -1;
                } else {
                    bspInternalCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val();
                }
                bspSubordinateCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val();
                if ($(".allowBrotherPurchase").css("display") == "none") {
                    bspProfessionCommission = -1;
                } else {
                    bspProfessionCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val();
                }
                if ($(".discount").css("display") == "none") {
                    bspPriceType = "Price";
                    bspPriceOrDiscount = -1;
                } else if ($(".selectPrice").css("display") == "none") {
                    bspPriceType = "Price";
                    bspPriceOrDiscount = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val();
                } else {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".selectPrice option:selected").val() == 0) {
                        bspPriceType = "Price";
                        bspPriceOrDiscount = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val());
                    } else if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".selectPrice option:selected").val() == "1") {
                        bspPriceType = "Subtracting";
                        bspPriceOrDiscount = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val());
                        bspInternalCommission = parseFloat(bspInternalCommission);
                        bspSubordinateCommission = parseFloat(bspSubordinateCommission);
                        bspProfessionCommission = parseFloat(bspProfessionCommission);
                    }
                }
                synBlackScreen = false;
                policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "ProvideDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "BeforehandDays": before, "SynBlackScreen": synBlackScreen, "Berths": bspBerths, "TicketType": "BSP", "Price": bspPriceOrDiscount, "InternalCommission": bspInternalCommission, "SubordinateCommission": bspSubordinateCommission, "ProfessionCommission": bspProfessionCommission, "PriceType": bspPriceType, "AutoAudit": autoAudit, "ResourceAmount": resourceAmount, "ConfirmResource": false, "IsBargainBerths": IsBargainBerths, "PrintBeforeTwoHours": printBeforeTwoHours, "LowNoType": lowNoType, "LowNoMaxPrice": lowNoMaxPrice, "LowNoMinPrice": lowNoMinPrice });
            }
            if ($(".parent_div").eq(k).find(".groupBox2 table tr td .chkb2b").is(":checked")) {
                printBeforeTwoHours = $(".parent_div").eq(k).find(".groupBox2 table tr td .qfqcp").is(":checked");
                if (IsBargainBerths) {
                    b2BBerths = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".BunksBargain select option:selected").val();
                } else {
                    for (g = 0; g < $(".parent_div").eq(k).find(".groupBox2 table tr td .BunksRad").eq(1).find("input[type='checkbox']:checked").length; g++) {
                        if (g > 0) {
                            b2BBerths += ",";
                        }
                        b2BBerths += $(".parent_div").eq(k).find(".groupBox2 table tr td .BunksRad").eq(1).find("input[type='checkbox']:checked").eq(g).val();
                    }
                }
                if ($(".canHaveSubordinate").css("display") == "none") {
                    b2BInternalCommission = -1;
                } else {
                    b2BInternalCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(2).val();
                }
                b2BSubordinateCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(3).val();
                if ($(".allowBrotherPurchase").css("display") == "none") {
                    b2BProfessionCommission = -1;
                } else {
                    b2BProfessionCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(4).val();
                }
                if ($(".discount").css("display") == "none") {
                    b2BPriceType = "Price";
                    b2BPriceOrDiscount = -1;
                } else if ($(".selectPrice").css("display") == "none") {
                    b2BPriceType = "Price";
                    b2BPriceOrDiscount = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val();
                } else {
                    if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".selectPrice option:selected").val() == 0) {
                        b2BPriceType = "Price";
                        b2BPriceOrDiscount = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(0).val());
                    } else if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find(".selectPrice option:selected").val() == "1") {
                        b2BPriceType = "Subtracting";
                        b2BPriceOrDiscount = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(2).find("input[type='text']").eq(1).val());
                        b2BInternalCommission = parseFloat(b2BInternalCommission);
                        b2BSubordinateCommission = parseFloat(b2BSubordinateCommission);
                        b2BProfessionCommission = parseFloat(b2BProfessionCommission);
                    }
                }
                synBlackScreen = false;
                IsSeat = false;
                policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "ProvideDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "BeforehandDays": before, "SynBlackScreen": synBlackScreen, "Berths": b2BBerths, "TicketType": "B2B", "Price": b2BPriceOrDiscount, "InternalCommission": b2BInternalCommission, "SubordinateCommission": b2BSubordinateCommission, "ProfessionCommission": b2BProfessionCommission, "PriceType": b2BPriceType, "AutoAudit": autoAudit, "ResourceAmount": resourceAmount, "ConfirmResource": false, "IsBargainBerths": IsBargainBerths, "PrintBeforeTwoHours": printBeforeTwoHours, "LowNoType": lowNoType, "LowNoMaxPrice": lowNoMaxPrice, "LowNoMinPrice": lowNoMinPrice });
            }
        }
        //商旅卡
        else if (productType == "4") {
            printBeforeTwoHours = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(3).find("input[type='checkbox']").is(":checked");
            policyDepartureFilghtDataStart = $(".parent_div").eq(k).find("input[type='text']").eq(0).val();
            policyDepartureFilghtDataEnd = $(".parent_div").eq(k).find("input[type='text']").eq(1).val();
            policyStartPrintDate = $(".parent_div").eq(k).find("input[type='text']").eq(2).val();
            departureDateFilter = $(".parent_div").eq(k).find("input[type='text']").eq(3).val();
            before = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val() == "" ? "0" : $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val();
            resourceAmount = -1;
            if (IsBargainBerths) {
                bspBerths = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".BunksBargain select option:selected").val();
            } else {
                for (g = 0; g < $(".parent_div").eq(k).find(".groupBox2 table tr td .BunksRad").eq(0).find("input[type='checkbox']:checked").length; g++) {
                    if (g > 0) {
                        bspBerths += ",";
                    }
                    bspBerths += $(".parent_div").eq(k).find(".groupBox2 table tr td .BunksRad").eq(0).find("input[type='checkbox']:checked").eq(g).val();
                }
            }
            if ($(".canHaveSubordinate").css("display") == "none") {
                bspInternalCommission = -1;
            } else {
                bspInternalCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val();
            }
            bspSubordinateCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val();
            if ($(".allowBrotherPurchase").css("display") == "none") {
                bspProfessionCommission = -1;
            } else {
                bspProfessionCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val();
            }
            if ($(".discount").css("display") == "none") {
                bspPriceType = "Price";
                bspPriceOrDiscount = -1;
            } else if ($(".selectPrice").css("display") == "none") {
                bspPriceType = "Price";
                bspPriceOrDiscount = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val();
            } else {
                if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".selectPrice option:selected").val() == 0) {
                    bspPriceType = "Price";
                    bspPriceOrDiscount = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(0).val());
                } else if ($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find(".selectPrice option:selected").val() == "1") {
                    bspPriceType = "Subtracting";
                    bspPriceOrDiscount = parseFloat($(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(1).val());
                    bspInternalCommission = parseFloat(bspInternalCommission);
                    bspSubordinateCommission = parseFloat(bspSubordinateCommission);
                    bspProfessionCommission = parseFloat(bspProfessionCommission);
                }
            }
            synBlackScreen = false;
            IsSeat = false;
            policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "ProvideDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "BeforehandDays": before, "SynBlackScreen": synBlackScreen, "Berths": bspBerths, "TicketType": "B2B", "Price": bspPriceOrDiscount, "InternalCommission": bspInternalCommission, "SubordinateCommission": bspSubordinateCommission, "ProfessionCommission": bspProfessionCommission, "PriceType": bspPriceType, "AutoAudit": autoAudit, "ResourceAmount": resourceAmount, "ConfirmResource": false, "IsBargainBerths": IsBargainBerths, "PrintBeforeTwoHours": printBeforeTwoHours, "LowNoType": lowNoType,"LowNoMaxPrice": lowNoMaxPrice, "LowNoMinPrice": lowNoMinPrice });
        } //其他 
        else if (productType == "5") {
            policyDepartureFilghtDataStart = $(".parent_div").eq(k).find("input[type='text']").eq(0).val();
            policyDepartureFilghtDataEnd = $(".parent_div").eq(k).find("input[type='text']").eq(1).val();
            policyStartPrintDate = $(".parent_div").eq(k).find("input[type='text']").eq(2).val();
            departureDateFilter = $(".parent_div").eq(k).find("input[type='text']").eq(3).val();
            before = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val() == "" ? "0" : $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val();
            price = -1;
            b2BBerths = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(5).val();
            b2BInternalCommission = $(".parent_div").eq(k).find("input[type='text']").eq(7).val() == "" ? "-1" : $(".parent_div").eq(k).find("input[type='text']").eq(7).val();
            b2BSubordinateCommission = $(".parent_div").eq(k).find("input[type='text']").eq(8).val() == "" ? "-1" : $(".parent_div").eq(k).find("input[type='text']").eq(8).val();
            b2BProfessionCommission = $(".parent_div").eq(k).find("input[type='text']").eq(9).val() == "" ? "-1" : $(".parent_div").eq(k).find("input[type='text']").eq(9).val();
            b2BPriceType = "Price";
            ticketType = "B2B";
            IsBargainBerths = false;
            resourceAmount = -1;
            synBlackScreen = true;
            IsSeat = true;

            policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "ProvideDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "BeforehandDays": before, "SynBlackScreen": synBlackScreen, "Berths": b2BBerths, "TicketType": ticketType, "Price": price, "InternalCommission": b2BInternalCommission, "SubordinateCommission": b2BSubordinateCommission, "ProfessionCommission": b2BProfessionCommission, "PriceType": b2BPriceType, "AutoAudit": autoAudit, "ResourceAmount": resourceAmount, "ConfirmResource": confirmResource, "IsBargainBerths": IsBargainBerths, "IsSeat": IsSeat, "PrintBeforeTwoHours": "false", "LowNoType": lowNoType, "LowNoMaxPrice": lowNoMaxPrice, "LowNoMinPrice": lowNoMinPrice });
        }
        //低打高返
        else if (productType == "6") {
            printBeforeTwoHours = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(3).find("input[type='checkbox']").is(":checked");
            policyDepartureFilghtDataStart = $(".parent_div").eq(k).find("input[type='text']").eq(0).val();
            policyDepartureFilghtDataEnd = $(".parent_div").eq(k).find("input[type='text']").eq(1).val();
            policyStartPrintDate = $(".parent_div").eq(k).find("input[type='text']").eq(2).val();
            departureDateFilter = $(".parent_div").eq(k).find("input[type='text']").eq(3).val();
            before = $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val() == "" ? "0" : $(".parent_div").eq(k).find(".groupBox1 input[type='text']").eq(4).val();
            resourceAmount = -1;
            for (g = 0; g < $(".parent_div").eq(k).find(".groupBox2 table tr td .BunksRad").eq(0).find("input[type='checkbox']:checked").length; g++) {
                if (g > 0) {
                    bspBerths += ",";
                }
                bspBerths += $(".parent_div").eq(k).find(".groupBox2 table tr td .BunksRad").eq(0).find("input[type='checkbox']:checked").eq(g).val();
            }
            if ($(".canHaveSubordinate").css("display") == "none") {
                bspInternalCommission = -1;
            } else {
                bspInternalCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(2).val();
            }
            bspSubordinateCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(3).val();
            if ($(".allowBrotherPurchase").css("display") == "none") {
                bspProfessionCommission = -1;
            } else {
                bspProfessionCommission = $(".parent_div").eq(k).find(".groupBox2 table tr").eq(1).find("input[type='text']").eq(4).val();
            }
            bspPriceOrDiscount = -1;
            bspPriceType = "Commission";
            bspInternalCommission = parseFloat(bspInternalCommission);
            bspSubordinateCommission = parseFloat(bspSubordinateCommission);
            bspProfessionCommission = parseFloat(bspProfessionCommission);
            synBlackScreen = true;
            IsSeat = false;
            policyArrayGroup.push({ "DepartureDateStart": policyDepartureFilghtDataStart, "DepartureDateEnd": policyDepartureFilghtDataEnd, "ProvideDate": policyStartPrintDate, "DepartureDateFilter": departureDateFilter, "DepartureWeekFilter": departureWeekFilter, "BeforehandDays": before, "SynBlackScreen": synBlackScreen, "Berths": bspBerths, "TicketType": "B2B", "Price": bspPriceOrDiscount, "InternalCommission": bspInternalCommission, "SubordinateCommission": bspSubordinateCommission, "ProfessionCommission": bspProfessionCommission, "PriceType": bspPriceType, "AutoAudit": autoAudit, "ResourceAmount": resourceAmount, "ConfirmResource": confirmResource, "IsBargainBerths": IsBargainBerths, "PrintBeforeTwoHours": printBeforeTwoHours, "LowNoType": lowNoType, "LowNoMaxPrice": lowNoMaxPrice, "LowNoMinPrice": lowNoMinPrice });
        }
    }
    policyArrayBase = { "Special": { "BasicInfo": { "Type": productType, "OfficeCode": office, "CustomCode": customCode, "ImpowerOffice": impowerOffice, "Airline": airline, "VoyageType": tripType, "Departure": departureAirports, "Arrival": arrivalAirports, "ExceptAirways": exceptAirways, "DepartureFlightsFilterType": departureFilghtType, "DepartureFlightsFilter": departureFilght, "InvalidRegulation": invalidRegulation, "EndorseRegulation": endorseRegulation, "RefundRegulation": refundRegulation, "ChangeRegulation": changeRegulation, "Remark": remark, "DrawerCondition": drawerCondition }, "Rebates": policyArrayGroup} };
    //发布特殊政策
    var url = "/PolicyHandlers/RoleGeneralPolicy.ashx/RegisterSpecialPolicy";

    ResquetQueryAction(url, actionName, policyArrayBase);
};