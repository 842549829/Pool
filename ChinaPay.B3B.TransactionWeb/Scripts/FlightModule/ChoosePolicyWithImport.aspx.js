var minPrice = 99999;
var recommanded = false;
var maxDiscount = 0;
//绑定推荐数据
function BindRecommand(recommand)
{
    var tabindex = -1;
    var hasData = false;
    if (recommand && recommand.Yestoday && recommand.Yestoday.length > 0)
    {
        $("#PolicyTmpl").tmpl(recommand.Yestoday).appendTo("#prevDayContrainer");
        $("#prevDate").text(/\d{4}-(\d{1,2}-\d{1,2}) 00:00:00/.exec(recommand.Yestoday[0].FlightDate)[1]);
        $("#prevDatePrice").text("￥" + recommand.Yestoday[0].LowerPrice);
        hasData = true;
        if (tabindex == -1) tabindex = 0;
    } else
    {
        $("#prev,#prevTable").hide();
    }
    if (recommand && recommand.Today && recommand.Today.length > 0)
    {
        $("#PolicyTmpl").tmpl(recommand.Today).appendTo("#currentDayContainer");
        $("#currentDate").text(/\d{4}-(\d{1,2}-\d{1,2}) 00:00:00/.exec(recommand.Today[0].FlightDate)[1]);
        $("#currentDatePrice").text("￥" + recommand.Today[0].LowerPrice);
        hasData = true;
        if (tabindex == -1) tabindex = 1;
    } else
    {
        $("#current,#currentTable").hide();
    }
    if (recommand && recommand.Tomorrow && recommand.Tomorrow.length > 0)
    {
        $("#PolicyTmpl").tmpl(recommand.Tomorrow).appendTo("#nextDayContainer");
        $("#nextDate").text(/\d{4}-(\d{1,2}-\d{1,2}) 00:00:00/.exec(recommand.Tomorrow[0].FlightDate)[1]);
        $("#nextDatePrice").text("￥" + recommand.Tomorrow[0].LowerPrice);
        hasData = true;
        if (tabindex == -1) tabindex = 2;
    } else
    {
        $("#next,#nextTable").hide();
    }
    if (hasData) showSuggest();
    $('#suggestDates').tabs({
        event: "click",
        selected: "curr",
        index: tabindex == -1 ? 1 : tabindex,
        callback: function (i) { }
    });
}

//隐藏推荐
function closeSuggest()
{
    $("#specialRecommend").hide();
    $("#policyConner").show();
}
//重新显示推荐
function showSuggest()
{
    $("#specialRecommend").show();
    $("#policyConner").hide();
}

///绑定政策列表
function BindPolicys(nomalPolicy, _specialPolicy)
{
    var hasData = false;

    if (nomalPolicy && nomalPolicy.length > 0)
    {
        //$("#nomalPolicyConatiner").empty();
        $("#nomalPolicyTmpl").tmpl(nomalPolicy, { TranslateRelationName: TranslateRelationName }).appendTo("#nomalPolicyConatiner");
        var lowerPrice = $.Min(nomalPolicy, function (ele) { return parseFloat(ele.SettleAmount); }); 
        maxDiscount = $.Max(nomalPolicy, function (ele) { return parseFloat(ele.dRebate); });
        $("#nomalPrice").text(lowerPrice.toString());
        UpdateMinPrice(lowerPrice);
        hasData = true;
        $("#nomal,#nomalPolicyConatiner").show();
    } else
    {
        $("#nomal,#nomalPolicyConatiner").hide();
    }
    if (_specialPolicy && _specialPolicy.length > 0)
    {
        //$("#specialPolicyContainer").empty();
        $("#specialPolicyTmpl").tmpl(_specialPolicy, { TranslateRelationName: TranslateRelationName }).appendTo("#specialPolicyContainer");
        var lowerPrice1 = $.Min(_specialPolicy, function (ele) { return parseFloat(ele.SettleAmount); });
        $("#specialPrice").text(lowerPrice1.toString());
        UpdateMinPrice(lowerPrice1);
        hasData = true;
        $("#spical,#specialPolicyContainer").show();
    } else
    {
        $("#spical,#specialPolicyContainer").hide();
    }
    if (!hasData)
    {
        $(".policyInfoBox").not("#ExternalPolicyBox").html("未能匹配到政策");
    }
    $(".provision").hide();
    $("#PolicyList input[type='radio']").click(function ()
    {
        $(".provision").hide();
        $(this).parent().parent().next().show();
    });
    $("#PolicyList input[type='radio']").first().trigger("click");
    hideMorePolicyButton();

    if ($("#nomal").is(":visible") && $("#spical").is(":visible"))
    {
        $('#policyInfo').tabs({
            event: "click",
            selected: "curr",
            callback: function (i)
            {
                if (i == 1)
                {
                    $("#specialPolicyContainer input[type='radio']").first().trigger("click");

                } else
                {
                    $("#nomalPolicyConatiner input[type='radio']").first().trigger("click");
                }
            }
        });
    }
    if (needExternalPolicy)
    {
        LoadExternalPolicy(nomalPolicy==null&&_specialPolicy==null);
        needExternalPolicy = false;
    }

}

function LoadRecommand()   //加载推荐
{
    if (isFreeTicket || IsChildTicket) return;   //对于免票不推荐
    if (minPrice == 99999) minPrice = 0;
    if (isImport && flightCount == "1" && !recommanded && $("#hidIsTeam").val() != "1")
    {
        var flightInfo = $("#hidFlishtInfos").val().split(",");
        var parameters = { departure: flightInfo[0], arrival: flightInfo[1], flightDate: flightInfo[2], currentPrice: minPrice };
        sendPostRequest("/FlightHandlers/ChoosePolicy.ashx/QueryRecommand", JSON.stringify(parameters), function (rsp)
        {
            BindRecommand(rsp);
            recommanded = true;
        }, $.noop);
    }
}

function LoadExternalPolicy(hideButton)  //加载外部政策
{
    var parameters = { "policyType": defalutPolicyType, "policyCount": maxPolicyCount, "source": source, "policyOwner": $("#hidOriginalPolicyOwner").val(), needSubsidize: source == "1" || source == "2", IsUsePatPrice: IsUsePatPrice, maxdRebate: maxDiscount };
    sendPostRequest("/FlightHandlers/ChoosePolicy.ashx/QueryExternalPolicy", JSON.stringify(parameters), function (data)
    {
        BindExternalPolicy(data);
        if(hideButton) $("#ExternalPolicyBox input[type='radio']").first().trigger("click");
    }, function (e)
    {
        $("#ExternalPolicyBox").html("<span class='obvious'>很可惜，平台未能匹配到更多政策！</span>");
        if ($("#btnProduce,#btnProduce1").hide()) $("#btnProduce,#btnProduce1").hide();
    });
    $("#ExternalPolicyContainer").show();
}

function BindExternalPolicy(policys)
{
    $("#ExternalPolicyBox").show();
    if (policys.length > 0)
    {
        $("#ExternalPolicyBox").empty();
        var exterPolicyLowerPrice = $.Min(policys, function (ele) { return parseFloat(ele.SettleAmount); });
        $("#nomalPolicyTmpl").tmpl(policys, { TranslateRelationName: TranslateRelationName }).appendTo("#ExternalPolicyBox");
        $("#ExternalPolicyBox .provision").hide();
        $("#exterPolicyLowerPrice").text(exterPolicyLowerPrice);
    } else
    {
        $("#ExternalPolicyBox").html("<span class='obvious'>很可惜，平台未能匹配到更多政策！</span>");
    }
}

function UpdateMinPrice(price)
{
    if (price < minPrice) minPrice = price;
}

function showMore()
{
    $(".more").toggle();
}

function ReserveFlight(departure, arrival, takeoffTime, landTime, flightDate, carrierCode, carrier, flightNo, airCarft, YBPrice, airportFee, BAF, discount, Bank, settleAmount, SeatCount, AdultBAF, ChildBAF, DepartureName, DepartureCity, DepartureTerminal, ArrivalName, ArrivalCity, ArrivalTerminal, policyId, policyType, publisher, officeNo, needAUTH, sender, Condition, spType, specialPolicy, PolicyDesc)
{
    if (!CheckSeatCount(SeatCount, departure, arrival, flightDate)) return;
    var parameter = { departure: departure, arrival: arrival, takeoffTime: takeoffTime,
        landTime: landTime, flightDate: flightDate, carrierCode: carrierCode, carrier: carrier
            , flightNo: flightNo, airCarft: airCarft, YBPrice: YBPrice, airportFee: airportFee, BAF: BAF,
        discount: discount, Bank: Bank, settleAmount: settleAmount, seatCount: SeatCount,
        AdultBAF: AdultBAF, ChildBAF: ChildBAF, DepartureName: DepartureName, DepartureCity: DepartureCity,
        DepartureTerminal: DepartureTerminal, ArrivalName: ArrivalName, ArrivalCity: ArrivalCity, ArrivalTerminal: ArrivalTerminal,
        policyId: policyId, policyType: policyType, publisher: publisher, officeNo: officeNo, source: source, choise: 0, needAUTH: needAUTH
    };
    $("#UserSelect,#UserSelect1").siblings(".i").remove();
    $("#UserSelect1").before($(sender).next().next().next().html());
    $("#UserSelect").before($(sender).next().next().next().html());
    if (policyType == SpecialPoliyType)
    {
        var totalAmount = Math.round((settleAmount + window.BAF + AirportFee) * passengerCount * 100) / 100;
        $("#PayPrice").text(fillZero(totalAmount));
        $("#spTotalAmount").text(fillZero(totalAmount) + priceUnit);
        $("#productName").text(spType);

        $("#productName1").text("什么是" + spType + "?").attr("href", "/About/help.aspx?flag=" + specialPolicy).attr("target", "_blank");
        $("#PolicyDesc").text(PolicyDesc);
        $("#Condition").text(Condition);
    }
    showTip(parameter, false, parameter.needAUTH ? 1 : parameter.policyType == SpecialPoliyType ? 2 : 3, function ()
    {
        sendPostRequest("/FlightHandlers/ChoosePolicy.ashx/SaveNewFlight", JSON.stringify(parameter), function (orderId)
        {
            var redirectUrl = '/OrderModule/Purchase/OrderPay.aspx?id=' + orderId + '&type=1&source=' + source;
            window.location.href = '/Index.aspx?redirectUrl=' + encodeURIComponent(redirectUrl);
        }, function (e)
        {
            alert(JSON.parse(e.responseText));
        });
    });

}


var callbackCache;
function showTip(parameter, directRun, tipType, callback)
{
    callbackCache = parameter || {};
    if (directRun)
    {
        callback(parameter);
        return;
    }
    if (tipType == 1)//授权提示
    {
        $("#showPop").click();
        $("#PNRCode").text(callbackCache.officeNo);
        $("#SureAUTH").off().one("click", function ()
        {
            $(".close").click();
            if ($("#NoAUTHAgree").is(":checked")) return;
            showTip(callbackCache, callbackCache.policyType != SpecialPoliyType && callbackCache.policyType != BargianPolicyType, callbackCache.policyType == SpecialPoliyType ? 2 : 3, callback);
        });
    } else if (tipType == 2)
    {//特殊票退改签提示
        $("#Confirm").click();
        $("#argee").off().one("click", function ()
        {
            $(this).hide();
            callback(callbackCache);
        });
    } else
    { //特价票退改签提示
        $("#Confirm1").click();
        $("#argee1").off().one("click", function ()
        {
            callback(callbackCache);
        });
    }
    $("#agree").click(function () { callbackCache.choise = 1; });
    $("#agreeAUTH").click(function () { callbackCache.choise = 2; });
}

function CheckSeatCount(seatCount, departure, arrival, flightDate)
{
    if (passengerCount > seatCount)
    {
        $("#showCantDoPop").click();
        $("#OtherFlight").attr("href",
                     "/FlightReserveModule/FlightQueryResult.aspx?source=1&departure=" + departure + "&arrival=" + arrival + "&goDate=" + flightDate);
        return false;
    }
    return true;
}
$(function () {
    BindDescriptionTip();
    setTimeout("CheckPNR()", 1000);
    $("#chbChangePNRChoise").click(function () {
        if ($(this).attr("checked") == "checked") {
            $("#agreeAUTH").parent().hide();
            $(".PolicyItem").has("input[value='false']").hide();
            if ($(".PolicyItem").has("input[type='radio']:checked").first().is(":hidden"))
            {
                $("#PolicyList input[type='radio']:visible").first().trigger("click");
            }
        } else {
            $(".PolicyItem").has("input[value='false']").show();
            $("#agreeAUTH").parent().show();
        }
    });
});

function CheckPNR()
{
    sendPostRequest("/FlightHandlers/ChoosePolicy.ashx/ExistsPNR", {}, function (e)
    {
        if (e.HasPNR)
        {
            $("#SamePNRTip a.orderId").text(e.OrderId).add("#viewOrder").attr("href", "/Index.aspx?redirectUrl=/OrderModule/Purchase/OrderDetail.aspx?id=" + e.OrderId);
            $("#SamePNRTipTrigger").click();
        }
    }, $.noop);
}

function BindDescriptionTip()
{
    $(".tips_btn").live("mouseover", function ()//绑定申请和候补的详细内容描述浮动显示事件
    {
        $(".tips_box").removeClass("hidden");
        $(".tips_box").css("left", $(this).offset().left - 80);
        $(".tips_box").css("top", $(this).offset().top + 10);
        var h = $(".tips_box").height();
        var top = $(".tips_box").offset().top;
        var sor = $(window).scrollTop();
        var wh = $(window).height();
        if ($(this).hasClass("standby_ticket"))
        {
            $(".tips_bd p").html($("#hidHBtip").val());
        } else
        {
            $(".tips_bd p").html($("#hidSQtip").val());
        }
        if (h + top - sor > wh)
        {
            $(".tips_box").css({ top: (top - h - 50) });
            $(".tips_box").addClass("tips_box1").removeClass("tips_box");
        };
    });
    $(".tips_btn").live("mouseout", function ()
    {
        $(".tips_box1").addClass("tips_box").removeClass("tips_box1");
        $(".tips_box").addClass("hidden");
    });
    $(".sup-p").live("mouseenter", function ()
    {
        $("#relationTip").text($.trim($(this).text().substring(0, 2)));
        $(".sup-p_box1").addClass("sup-p_box").removeClass("sup-p_box1");
        $(".sup-p_box").removeClass("hidden");
        $(".sup-p_box").css("left", $(this).offset().left - 107);
        $(".sup-p_box").css("top", $(this).offset().top + 20);
        var h = $(".sup-p_box").height();
        var top = $(".sup-p_box").offset().top;
        var sor = $(window).scrollTop();
        var wh = $(window).height();
        if (h + top - sor > wh)
        {
            $(".sup-p_box").css({ top: (top - h - 37) });
            $(".sup-p_box").addClass("sup-p_box1").removeClass("sup-p_box");
        };
    }).live("mouseleave", function ()
    {
        $(".sup-p_box").mouseenter(function ()
        {
            $(this).removeClass("hidden");
        }).mouseleave(function ()
        {
            $(this).addClass("hidden");
        });
        $(".sup-p_box1").mouseenter(function ()
        {
        }).mouseleave(function ()
        {
            $(this).addClass("hidden");
        });
        $(".sup-p_box1").addClass("hidden");
        $(".sup-p_box").addClass("hidden");
    });

}

function BindOrderOption()
{
    $("#btnProduce").off().click(function ()
    {
        var that = $(this);
        var policyId = $("#hidPolicyId").val();
        var policyOwner = $("#hidPolicyOwner").val();
        var policyType = $("#hidPolicyType").val();
        if (policyId == '')
        {
            alert("请选择政策");
        } else
        {
            // 编码导入和通过编码方式升舱的时候,将会提示给出票方的Office号授权
            if (source == "2" || source == "3")
            {
                if (NeedCheckOfficeNOAuth)
                {
                    $("#PNRCode").html(providerOfficeNo);
                    $("#showPop").click();
                    $("#SureAUTH").off().click(function ()
                    {
                        if ($("#NoAUTHAgree").is(":checked"))
                        {
                            $(".close").trigger("click");
                            return;
                        }
                        var customerChoise;
                        if ($("#agree").is(":checked"))
                        {
                            customerChoise = 1;
                        } else if ($("#agreeAUTH").is(":checked"))
                        {
                            customerChoise = 2;
                        }
                        else
                        {
                            customerChoise = 0;
                        }
                        $(".close").click();
                        that.hide();
                        $("#btnProcessing").show();
                        var submitMethod;
                        var submitParameters;
                        if (source == "3" || source == "4")
                        {
                            submitParameters = JSON.stringify({ "policyId": policyId, "policyType": policyType, "officeNo": providerOfficeNo, "orderId": $("#hidOriginalOrderId").val(), "source": source, "choise": customerChoise, needAUTH: NeedCheckOfficeNOAuth, HasSubsidized: HasSubsidized, IsUsePatPrice: IsUsePatPrice, forbidChnagePNR: forbidSet() });
                            submitMethod = "/FlightHandlers/ChoosePolicy.ashx/ProduceApplyform";
                        } else
                        {
                            submitParameters = JSON.stringify({ "policyId": policyId, "policyType": policyType, "publisher": policyOwner, "officeNo": providerOfficeNo, "source": source, "choise": customerChoise, needAUTH: NeedCheckOfficeNOAuth, HasSubsidized: HasSubsidized, IsUsePatPrice: IsUsePatPrice, forbidChnagePNR: forbidSet() });
                            submitMethod = "/FlightHandlers/ChoosePolicy.ashx/ProduceOrder";
                        }
                        sendPostRequest(submitMethod, submitParameters, function (data)
                        {
                            Redirect('/OrderModule/Purchase/OrderPay.aspx?id=' + data + '&type=1&source=' + source);
                        }, function (error)
                        {
                            if (error.status == '300')
                            {
                                alert(JSON.parse(error.responseText));
                            } else
                            {
                                alert('系统故障，请联系平台');
                            }
                            $("#btnProcessing").hide();
                            $("#btnProduce").show();
                        });
                    });

                } else
                {
                    that.hide();
                    $("#btnProcessing").show();
                    var submitMethod1;
                    var submitParameters1;
                    if (source == "3" || source == "4")
                    {
                        submitParameters1 = JSON.stringify({ "policyId": policyId, "policyType": policyType, "officeNo": providerOfficeNo, "orderId": $("#hidOriginalOrderId").val(), "source": source, "choise": 0, needAUTH: NeedCheckOfficeNOAuth, HasSubsidized: HasSubsidized, IsUsePatPrice: IsUsePatPrice, forbidChnagePNR: forbidSet() });
                        submitMethod1 = "/FlightHandlers/ChoosePolicy.ashx/ProduceApplyform";
                    } else
                    {
                        submitParameters1 = JSON.stringify({ "policyId": policyId, "policyType": policyType, "publisher": policyOwner, "officeNo": providerOfficeNo, "source": source, "choise": 0, needAUTH: NeedCheckOfficeNOAuth, HasSubsidized: HasSubsidized, IsUsePatPrice: IsUsePatPrice, forbidChnagePNR: forbidSet() });
                        submitMethod1 = "/FlightHandlers/ChoosePolicy.ashx/ProduceOrder";
                    }
                    sendPostRequest(submitMethod1, submitParameters1, function (data)
                    {
                        Redirect('/OrderModule/Purchase/OrderPay.aspx?id=' + data + '&type=1&source=' + source);
                    }, function (error)
                    {
                        if (error.status == '300')
                        {
                            alert(JSON.parse(error.responseText));
                        } else
                        {
                            alert('系统故障，请联系平台');
                        }
                        $("#btnProcessing").hide();
                        $("#btnProduce").show();
                    });

                }

            } else if (selectedPolicy == SpecialPoliyType)
            {

                $(this).add(that).hide();
                $("#btnProcessing,#btnProcessing1").show();
                $("#btnProduce,#btnProduce1").hide();
                $("#notAgree").attr("disabled", "disabled");
                var method;
                var parameters;
                if (source == "3" || source == "4")
                {
                    parameters = JSON.stringify({ "policyId": policyId, "policyType": policyType, "officeNo": providerOfficeNo, "orderId": $("#hidOriginalOrderId").val(), "source": source, "choise": 0, needAUTH: NeedCheckOfficeNOAuth, HasSubsidized: HasSubsidized, IsUsePatPrice: IsUsePatPrice, forbidChnagePNR: forbidSet() });
                    method = "/FlightHandlers/ChoosePolicy.ashx/ProduceApplyform";
                } else
                {
                    parameters = JSON.stringify({ "policyId": policyId, "policyType": policyType, "publisher": policyOwner, "officeNo": providerOfficeNo, "source": source, "choise": 0, needAUTH: NeedCheckOfficeNOAuth, HasSubsidized: HasSubsidized, IsUsePatPrice: IsUsePatPrice, forbidChnagePNR: forbidSet() });
                    method = "/FlightHandlers/ChoosePolicy.ashx/ProduceOrder";
                }
                sendPostRequest(method, parameters, function (data)
                {
                    Redirect('/OrderModule/Purchase/OrderPay.aspx?id=' + data + '&type=1&source=' + source);
                }, function (error)
                {
                    if (error.status == '300')
                    {
                        alert(JSON.parse(error.responseText));
                    } else
                    {
                        alert('系统故障，请联系平台');
                    }
                    $("#btnProcessing,#btnProcessing1").hide();
                    $("#btnProduce,#btnProduce1").show();
                    $("#notAgree").removeAttr("disabled");
                    $("#btnProduce,#argee").show();
                });
                //});
                $("#notAgree").click(function ()
                {
                    $("#btnQueryFlight").click();
                });
            } else
            {
                that.hide();
                $("#btnProcessing").show();
                var method;
                var parameters;
                if (source == "3" || source == "4")
                {
                    parameters = JSON.stringify({ "policyId": policyId, "policyType": policyType, "officeNo": providerOfficeNo, "orderId": $("#hidOriginalOrderId").val(), "source": source, "choise": 0, needAUTH: NeedCheckOfficeNOAuth, HasSubsidized: HasSubsidized, IsUsePatPrice: IsUsePatPrice, forbidChnagePNR: forbidSet() });
                    method = "/FlightHandlers/ChoosePolicy.ashx/ProduceApplyform";
                } else
                {
                    parameters = JSON.stringify({ "policyId": policyId, "policyType": policyType, "publisher": policyOwner, "officeNo": providerOfficeNo, "source": source, "choise": 0, needAUTH: NeedCheckOfficeNOAuth, HasSubsidized: HasSubsidized, IsUsePatPrice: IsUsePatPrice, forbidChnagePNR: forbidSet() });
                    method = "/FlightHandlers/ChoosePolicy.ashx/ProduceOrder";
                }
                sendPostRequest(method, parameters, function (data)
                {
                    Redirect('/OrderModule/Purchase/OrderPay.aspx?id=' + data + '&type=1&source=' + source + '&choise=' + 0);
                }, function (error)
                {
                    if (error.status == '300')
                    {
                        alert(JSON.parse(error.responseText));
                    } else
                    {
                        alert('系统故障，请联系平台');
                    }
                    $("#btnProcessing").hide();
                    $("#btnProduce").show();
                });
            }
        }
    });
}

function chooseNormalPolicy(sender, policyId, policyOwner, policyType, officeNo, settleAmount, commission, needCheckOfficeNOAuth, Condition, HasSubsidized, setChangePNREnable)
{
    $(".provision").hide();
    $(sender).parent().parent().next().show();
    choosePolicy(policyId, policyOwner, policyType, officeNo, needCheckOfficeNOAuth);
    var totalAmount = Math.round((settleAmount + AirportFee + BAF) * passengerCount * 100) / 100;
    var totalProfit = Math.round(commission * passengerCount * 100) / 100;
    $("#spTotalAmount").text(fillZero(totalAmount) + priceUnit);
    $("#spTotalProfit").text(fillZero(totalProfit) + priceUnit);
    window.HasSubsidized = HasSubsidized;
    if (selectedPolicy == BargianPolicyType)
    {
        $("#UserSelect1").siblings(".i").remove();
        $("#UserSelect1").before($(sender).next().html());
        $("#PayPrice1").text(fillZero(totalAmount));
        $("#Condition1").html(Condition);
    }
    //SetChangePNR(setChangePNREnable);
    forbidChangePNR = !setChangePNREnable;

}

function chooseSpecialPolicy(sender, policyId, policyOwner, policyType, fare, spType, PolicyDesc, specialPolicy, Condition, needCheckOfficeNoAuth, needApplication, OfficeNO, RenderTicketPrice, IsFreeTicket, IsNOSeat)
{
    $(".provision").hide();
    $("#UserSelect").siblings(".i").remove();
    $("#UserSelect").before($(sender).next().html());
    $(sender).parent().parent().next().show();
    window.IsFreeTicket = IsFreeTicket;
    choosePolicy(policyId, policyOwner, policyType, OfficeNO, needCheckOfficeNoAuth);
    var totalAmount = Math.round((fare + AirportFee + BAF) * passengerCount * 100) / 100;
    $("#PayPrice").text(fillZero(totalAmount));
    $("#spTotalAmount").text(fillZero(totalAmount) + priceUnit);
    $("#productName").text(spType);
    if (IsNOSeat)
    {
        $("#hbTip").show();
        $("#productName").text("候补" + spType);
    }
    $("#productName1").text("什么是" + spType + "?").attr("href", "/About/help.aspx?flag=" + specialPolicy).attr("target", "_blank");
    $("#PolicyDesc").html(PolicyDesc);
    $("#Condition").html(Condition);
    $("#profitInfo").hide();
    renderTicketPrice = RenderTicketPrice;
    forbidChangePNR = false;
    //SetChangePNR(false);
    $("#btnProduce,#btnProduce1").val(needApplication ? "提交申请" : "生成订单");
}
function choosePolicy(policyId, policyOwner, policyType, officeNo, needCheckOfficeNOAuth)
{
    $("#hidPolicyId").val(policyId);
    $("#hidPolicyOwner").val(policyOwner);
    $("#hidPolicyType").val(policyType);
    $("#profitInfo").show();
    selectedPolicy = policyType;
    providerOfficeNo = officeNo;
    NeedCheckOfficeNOAuth = needCheckOfficeNOAuth;
    setTimeout(function ()
    {
        FillYBPrice();
    }, 100);
}

//function SetChangePNR(enable) {
//    if (enable) {
//        $("#chbChangePNRChoise").removeAttr("disabled");
//    } else {
//        $("#chbChangePNRChoise").removeAttr("checked","checked").attr("disabled","disabled");
//    }
//}

function forbidSet() {
    return $("#chbChangePNRChoise").is(":checked");
}