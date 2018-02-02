function ResquetQueryAction(actionUrl, actionName, param) {
    sendPostRequest(actionUrl, JSON.stringify(param), function (e) {
        var str = "";
        //特殊政策类型
        if (actionName == "QuerySpecialProductTypeList") {
            str = "";
            var strSpan = "";
            var itemValue;
            $.each(eval(e), function (i, item) {
                if (str == "") {
                    str = "<li><a href='javascript:;' class='navType2Selected' id='" + item.Value + "' >" + item.Name + "</a></li>";
                    $("#sel li a").removeClass("navType2Selected");
                    $("#" + item.Value).addClass("navType2Selected");
                    itemValue = item.Value;
                } else {
                    str += "<li><a href='javascript:;' id='" + item.Value + "' >" + item.Name + "</a></li>";
                }
                if (strSpan == "") {
                    strSpan = "<span id='span_" + item.Value + "'>" + item.Description + "</span>";
                } else {
                    strSpan += "<span id='span_" + item.Value + "' style='display: none;'>" + item.Description + "</span>";
                }
            });
            if (str == "") {
                alert("暂不能发布任何特殊产品，请联系平台！");
                window.location.href = "/Index.aspx";
            } else {
                $("#sel").html(str);
                $("#Description").html(strSpan);
                ShowOrHides(itemValue);
                //查询是否可以发布免票产品 
                ResquetQueryAction("/PolicyHandlers/RoleGeneralPolicy.ashx/IsPublishFree", "IsPublishFree", null);
            }
        }
        //出港城市的绑定
        if (actionName == "Area") {
            str = " ";
            $.each(eval(e), function (i, item) {
                str += "<option value='" + item.Code.Value + "'>" + item.Name + "</option>";
            });
            $("#lbSource").append(str);
            //            $("#lbSource1").append(str);
            //            $("#lbSource2").append(str);
            //$("#lbWangfanSource").append(str);
            $("#lbShifaSource").append(str);

        }
        //到达城市的绑定
        if (actionName == "Arrival") {
            str = " ";
            $.each(eval(e), function (i, item) {
                str += "<option value='" + item.Code.Value + "'>" + item.Name + "</option>";
            });
            // $("#lbSource").append(str);
            $("#lbSource1").append(str);
            $("#lbSource2").append(str);
            $("#lbWangfanSource").append(str);
            //$("#lbShifaSource").append(str);

        }
        //查询office号码
        if (actionName == "Office") {
            str = "";
            $.each(e, function (i, item) {
                if (str == "") {
                    str = "<option value='' impower='false'>--请选择--</option>";
                    $("#selOfficeTd span").hide();
                }
                str += "<option value='" + item.Number + "' impower='" + item.Impower + "'>" + item.Number + "</option>";
            });
            if (str == "") {
                str = "<option value=''>无OFFICE号</option>";
                $("#selOfficeTd span").hide();
                $("#officeTip").html("");
            }
            $("#selOffice").append(str);
        }
        //绑定航空公司
        if (actionName == "Airline") {
            str = "";
            $.each(eval(e), function (i, item) {
                str += "<option value='" + item.Code.Value + "'>" + item.Name + "</option>";
            });
            if (str == "") {
                str += "<option value=''>航空公司</option>";
            }
            $("#selProvince").append(str);
        }
        //查询改签规定
        if (actionName == "ChangeRegulation") {
            $("#selChangeRegulation").html("");
            str = "";
            $.each(eval(e), function (i, item) {
                str += "<option value='" + item.Name + "'>" + item.Name + "</option>";
            });
            if (str == "") {
                str += "<option value=''>改签规定</option>";
            }
            $("#selChangeRegulation").append(str);
        }
        //查询签转规定
        if (actionName == "EndorseRegulation") {
            $("#selEndorseRegulation").html("");
            str = "";
            $.each(eval(e), function (i, item) {
                str += "<option value='" + item.Name + "'>" + item.Name + "</option>";
            });
            if (str == "") {
                str += "<option value=''>签转规定</option>";
            }
            $("#selEndorseRegulation").append(str);
        }
        //查询退票规定
        if (actionName == "RefundRegulation") {
            $("#selRefundRegulation").html("");
            str = "";
            $.each(eval(e), function (i, item) {
                str += "<option value='" + item.Name + "'>" + item.Name + "</option>";
            });
            if (str == "") {
                str += "<option value=''>退票规定</option>";
            }
            $("#selRefundRegulation").append(str);
        }
        //作废规定
        if (actionName == "InvalidRegulation") {
            $("#selInvalidRegulation").html("");
            str = "";
            $.each(eval(e), function (i, item) {
                str += "<option value='" + item.Name + "'>" + item.Name + "</option>";
            });
            if (str == "") {
                str += "<option value=''>作废规定</option>";
            }
            $("#selInvalidRegulation").append(str);
        }
        //出票条件
        if (actionName == "DrawerCondition") {
            $("#selDrawerCondition").html("");
            str = "";
            $.each(eval(e), function (i, item) {
                str += "<option value='" + item.Name + "'>" + item.Name + "</option>";
            });
            if (str == "") {
                str += "<option value=''>出票条件</option>";
            }
            $("#selDrawerCondition").append(str);
        }

        //发布基础政策
        if (actionName == "Base_Publish") {
            if (e == true) {
                alert("添加成功");
                window.location.href = "./base_policy_manage.aspx";
            } else {
                alert("添加失败");
            }
        }
        //发布特价政策
        if (actionName == "Bargain_Publish") {
            if (e == true) {
                alert("添加成功");
                window.location.href = "./low_price_policy_manage.aspx";
            } else {
                alert("添加失败");
            }
        }
        //发布特殊政策
        if (actionName == "Special_Publish") {
            if (e == true) {
                alert("添加成功");
                window.location.href = "./special_policy_manage.aspx";
            } else {
                alert("添加失败");
            }
        }
        //发布往返政策
        if (actionName == "Rund_Publish") {
            if (e == true) {
                alert("添加成功");
                window.location.href = "./back_to_policy_manage.aspx";
            } else {
                alert("添加失败");
            }
        }
        //发布团队政策
        if (actionName == "Team_Publish") {
            if (e == true) {
                alert("添加成功");
                window.location.href = "./team_policy_manage.aspx";
            } else {
                alert("添加失败");
            }
        }
        //发布并继续
        if (actionName == "Ahead") {
            if (e == true) {
                alert("添加成功");
            } else {
                alert("添加失败");
            }
        }
        //判断公司是否可以发免票
        if (actionName == "IsPublishFree") {
            if (e == 1) {
                // $("#span_2").css("display", "");
                $("#2").css("display", "");
                $("#selOfficeTd").css("display", "");
                $(".neibu").css("display", "");
                $(".xiaji").css("display", "");
            } else {
                $("#span_2").css("display", "none");
                $("#2").css("display", "none");
                $("#zidingyiTd").css("display", "none");
                $("#selOfficeTd").css("display", "none");
                $(".neibu").css("display", "none");
                $(".xiaji").css("display", "none");
                //                $(".tonghang").css("display", "none");
            }
            $("#companytype").html(e);
            //查询公司参数 
            ResquetQueryAction("/PolicyHandlers/PolicyManager.ashx/GetCompanySetting", "GetCompanySetting", null);
        }
        //查询公司参数
        if (actionName == "GetCompanySetting") {
            if ($.trim($("#companytype").html()) == 4) {
                $(".tonghang").css("display", "");
                $(".neibu").css("display", "none");
                $(".xiaji").css("display", "none");
            }
            if ($.trim($("#companytype").html()) == 1 || $("#companytype").size() == 0) {
                if ($(".xiaji").css("display") != "none") {
                    $(".xiaji").css("display", "");
                }
                if ($(".canHaveSubordinate") != null) {
                    if (e.Parameter == null) {
                        $(".canHaveSubordinate").css("display", "none");
                        $(".neibu").css("display", "none");
                    } else {
                        if (e.Parameter.CanHaveSubordinate == true) {
                            $(".canHaveSubordinate").css("display", "");
                            if ($(".neibu").css("display") != "none") {
                                $(".neibu").css("display", "");
                            }
                        } else {
                            $(".canHaveSubordinate").css("display", "none");
                            $(".neibu").css("display", "none");
                        }
                    }
                }
                if ($(".allowBrotherPurchase") != null) {
                    if (e.Parameter == null) {
                        $(".allowBrotherPurchase").css("display", "none");
                        $(".tonghang").css("display", "none");
                    } else {
                        if (e.Parameter.AllowBrotherPurchase == true) {
                            $(".allowBrotherPurchase").css("display", "");
                            if ($(".tonghang").css("display") != "none") {
                                $(".tonghang").css("display", "");
                            }
                        } else {
                            $(".allowBrotherPurchase").css("display", "none");
                            $(".tonghang").css("display", "none");
                        }
                    }
                }
                if (e.WorkingSetting == null) {
                    $(".zidingyi").css("display", "none");
                } else {
                    if (e.WorkingSetting.IsImpower == true) {
                        $(".zidingyi").css("display", "");
                    } else {
                        $(".zidingyi").css("display", "none");
                    }
                }
                if ($(".navType2Selected").attr("id") == "3" || $(".navType2Selected").attr("id") == "4" || $(".navType2Selected").attr("id")=="6") {
                    $(".chanpinPrice").css("display", "none");
                }
            }

            if ($("#divLoad").html() != null) {
                $("#divLoad").hide();
            }
            //            if ($(".importantBox").html() != null) {
            //                if (e.Parameter.AutoPlatformAudit == true) {
            //                    $(".importantBox").css("display", "none");
            //                } else {
            //                    $(".importantBox").css("display", "");
            //                }
            //            }
        } //查询自定义编码
        if (actionName == "GetCustomeCode") {
            str = "";
            $.each(e, function (i, item) {
                if (str == "") {
                    str = "<option value='' >--请选择--</option>";
                }
                str += "<option value='" + item.Number + "'>" + item.Number + "</option>";
            });
            if (str == "") {
                str = "<option value='0'>无自定义编号</option>";
            }
            $("#selZidingy").append(str);
        }
    }, function (e) {
        alert(e.responseText);
    });
};

function AddPolicyGroup(str, i) {
    if (i > 10) {
        return;
    }
    $(".policy_group").append(str);
    //查询公司参数 
    var url = "/PolicyHandlers/PolicyManager.ashx/GetCompanySetting";
    var actionName = "GetCompanySetting";
    ResquetQueryAction(url, actionName, null);

    setDefaultTime(parseInt(i) - 1);
};
function DelPolicyGroup(group) {
    if ($(".parent_div").length <= 1) {
        return;
    }
    $(group).parent().parent().parent().parent().parent().remove();
    for (var j = 0; j < $(".parent_div").length; j++) {
        $(".parent_div h4").eq(j).html("第&nbsp;" + (parseInt(j) + 1) + "&nbsp;组政策");
    }
    reinitIframe();
}
function reinitIframe() {
    var rightFrame = parent.document.getElementById("rightFrame");
    if (rightFrame) {
        rightFrame.height = document.body.scrollHeight;
    } 
}
function setDefaultTime(i) {
    //设置默认时间
    var d = Date.format(new Date(), 'yyyy-MM-dd');
    var date = new Date();
    var day = new Date(date.getFullYear(), (date.getMonth() + 1), 0);
    var lastMonth = day.getFullYear() + '-' + (day.getMonth() + 1) + '-' + day.getDate();
    var today = parseInt(d.replace(/-/g, ''), 10);
    var lastDay = parseInt(lastMonth.replace(/-/g, ''), 10);
    if (lastDay - today <= 5) {
//        if (parseInt(day.getMonth() + 2) > 12) {
//            //lastMonth = (parseInt(day.getFullYear()) + 1) + '-01-' + day.getDate();
//            
//        } else {
//            lastMonth = day.getFullYear() + '-' + (day.getMonth() + 2) + '-' + day.getDate();          
        //           }
        var currentMonth = new Date(date.getFullYear(), (date.getMonth()), 1);
        currentMonth = Date.addPart(currentMonth, 'M', 2);
        currentMonth = Date.addPart(currentMonth,'d',-1);
    }
    $(".chupiao").eq(i).val(d);
    $(".quchengkaishi").eq(i).val(d);

    $(".quchengjieshu").eq(i).val(Date.format(currentMonth, 'yyyy-MM-dd'));
    //    if ($(".huichengkaishi").eq(i).val() != null) {
    //        $(".huichengkaishi").eq(i).val(d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + day.getDate());
    //        $(".huichengjieshu").eq(i).val(d.getYear() + '-' + (d.getMonth() + 2) + '-' + day.getDate());
    //    } 
}

$(function () {
    setDefaultTime(0);
    //查询出港城市
    var url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryAirportsDepartureByCompany";
    var actionName = "Area";
    ResquetQueryAction(url, actionName, null);
    //查询到达城市
    url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryAirportsArrivalList";
    actionName = "Arrival";
    ResquetQueryAction(url, actionName, null);
    //查询航空公司
    url = "/PolicyHandlers/RoleGeneralPolicy.ashx/QueryAirlines";
    actionName = "Airline";
    ResquetQueryAction(url, actionName, null);
    //查询自定义编码
    url = "/PolicyHandlers/RoleGeneralPolicy.ashx/GetCustomeCode";
    actionName = "GetCustomeCode";
    ResquetQueryAction(url, actionName, null);
    //给文本框绑定日期控件
    $(".datefrom,.dateto").live("click", function () {
        WdatePicker({ isShowClear: "flase", skin: 'default', readOnly: "true", doubleCalendar: "true" });
    });
    $(".choice").live("click", function () {
        if ($(this).val() == "0") {
            $(this).parent().find("input[type='checkbox']").attr("checked", "checked");
        }
        if ($(this).val() == "1") {
            for (var k = 0; k < $(this).parent().find("input[type='checkbox']").length; k++) {
                if ($(this).parent().find("input[type='checkbox']").eq(k).is(":checked")) {
                    $(this).parent().find("input[type='checkbox']").eq(k).removeAttr("checked");
                } else {
                    $(this).parent().find("input[type='checkbox']").eq(k).attr("checked", "checked");
                }
            }
        }
    });
    $("#selOffice").change(function () {
        if ($.trim($("#selOffice option:selected").val()) == "") {
            $("#selOfficeTd span").hide();
        } else {
            $("#selOfficeTd span").show();
        }
        if ($("#selOffice option:selected").attr("impower") == "true") {
            $("#selOfficeTd span").html("需授权").removeClass("obvious3").addClass("obvious2");
        } else {
            $("#selOfficeTd span").html("无需授权").removeClass("obvious2").addClass("obvious3");
        }
    });
    $("#duihuan").live("click", function () {
        var lbSourceText = $("#txtAirports").val();
        //        var lbSource = $("#lbSource").html();
        //        var lbSelected = $("#lbSelected").html();

        $("#txtAirports").val($("#txtAirports1").val());
        //        $("#lbSource").html($("#lbSource1").html());
        //        $("#lbSelected").html($("#lbSelected1").html());

        $("#txtAirports1").val(lbSourceText);
        //        $("#lbSource1").html(lbSource);
        //        $("#lbSelected1").html(lbSelected);
        $("#txtAirports").blur();
        $("#txtAirports1").blur();
    });
    $(".refBtnBunks").live("click", function () {
        var bunksIndex = 0;
        if ($(this).parent().parent().parent().parent().find(".heipingtongbu").html != null) {
            bunksIndex = $(this).parent().parent().parent().parent().index();
        } else {
            bunksIndex = $(this).parent().parent().parent().parent().parent().parent().parent().parent().index();
        }
        $(".parent_div").eq(bunksIndex).find(".btnRefresh").click();
    });
});

function ClearBunksByAirline() {
    $(".BunksRad").html("<label class='refBtnBunks btn class3'>点击获取舱位</label>");
    if ($(".ZhidingBunks").html() != null) {
        $(".ZhidingBunks").html("<label class='refBtnBunks btn class3'>点击获取舱位</label>");
    }
    if ($(".BunksBargain").html() != null) {
        $(".BunksBargain").html("<label class='refBtnBunks btn class3'>点击获取舱位</label>");
    }
};

function ClearBunksByTime(text) {
    var bunksIndex = $(text).parent().parent().parent().parent().index();
    $(".parent_div").eq(bunksIndex).find(".BunksRad").html("<label class='refBtnBunks btn class3'>点击获取舱位</label>");
    if ($(".ZhidingBunks").html() != null) {
        $(".ZhidingBunks").html("<label class='refBtnBunks btn class3'>点击获取舱位</label>");
    }
    if ($(".parent_div").eq(bunksIndex).find(".BunksBargain").html() != null) {
        $(".parent_div").eq(bunksIndex).find(".BunksBargain").html("<label class='refBtnBunks btn class3'>点击获取舱位</label>");
    }
};