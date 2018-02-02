// 申请退/废票
function ApplyRefund(sender) {
    var flightInfos = GetApplyFlights(sender);
    if (flightInfos[0] == 0) {
        alert('至少选择一个航段');
    } else {
        var passengerInfos = GetApplyPassengers(sender);
        if (passengerInfos[0] == 0) {
            alert('至少选择一个乘机人');
        } else {
            var pnr = GetApplyPNR(sender);
            SetApplyInfo(pnr, passengerInfos[1], flightInfos[1]);
            $("#refundVoyages").html(flightInfos[2]);
            $("#refundPassengerNames").html(passengerInfos[2]);
            $("#applyRefundInfo").click();
            //$("#divRefund").show();
        }
    }
    var container = $("#voyages,#passengers");
    if ($("input[type='checkbox']", container).size() == $("input:checked", container).size() && IsImport) {
        $("#delegateCanclePNR").show();
        $("#chkdelegateCanclePNR").show();
    } else {
        $("#delegateCanclePNR").hide();
        $("#chkdelegateCanclePNR").hide();
        $("#chkdelegateCanclePNR").removeAttr("checked");
    }
}
// 申请改期
function ApplyPostpone(sender) {
    var flightInfos = GetApplyFlights(sender);
    if (flightInfos[0] == 0) {
        alert('至少选择一个航段');
    } else {
        var passengerInfos = GetApplyPassengers(sender);
        if (passengerInfos[0] == 0) {
            alert('至少选择一个乘机人');
        } else {
            SetApplyPostponeFlights(sender);
            var pnr = GetApplyPNR(sender);
            SetApplyInfo(pnr, passengerInfos[1], flightInfos[1]);
            $("#postponeCarrier").html(flightInfos[3]);
            $("#postponePassengerNames").html(passengerInfos[2]);
            $("#applyPostponeInfo").click();
            //$("#divPostpone").show();
        }
    }
}
// 申请升舱
function ApplyUpgrade(sender) {
    var passengerInfos = GetApplyPassengers(sender);
    var flightInfos = GetApplyFlights(sender);
    var passengers = GetApplyPassengerInfos();
    for (var p in passengers) {
        if (passengers[p].PassengerType == "1") {
            alert("不支持儿童票升舱");
            return false;
        }
    }
    if (flightInfos[0] == 0) {
        alert('至少选择一个航段');
    } else {
        if (passengerInfos[0] == 0) {
            alert('至少选择一个乘机人');
        } else {
            var pnr = GetApplyPNR(sender);
            SetApplyInfo(pnr, passengerInfos[1], flightInfos[1], flightInfos[4]);
            SetApplyUpgradeFlights(sender);
            $("#upgradeVoyages").html(flightInfos[2]);
            $("#upgradePassengerNames").html(passengerInfos[2]);
            $("#applyUpgradeInfo").click();
            //$("#divUpgrade").show();
        }
    }
    return false;
}

// 取消退/废票申请
function CancelApplyRefund() {
    //    $("#divRefund").hide();
    SetApplyInfo("", "");
    $("#txtRefundReason").val("");
    $("#divRefundType input:radio").attr("checked", "");
    closeLayer();
}
// 取消改期申请
function CancelApplyPostpone() {
    //$("#divPostpone").hide();
    SetApplyInfo("", "");
    $("#txtPostponeRemark").val("");
    closeLayer();
}
// 取消升舱申请
function CancelApplyUpgrade() {
    //$("#divUpgrade").hide();
    SetApplyInfo("", "");
    closeLayer();
}
var interval;
// 提交退/废票申请
function CommitApplyRefund(sender) {
    $(sender).add($(sender).next()).attr("disabled", "disabled");
    if (ValidateApplyRefund()) {
        ShowApplyFormBody(false);
        var passenger = $("#hidApplyPassengers").val();
        var voyage = $("#hidApplyFlights").val();
        var pnrPair = $("#hidApplyPNR").val();
        var refundType =  $("#divRefundType input:radio:checked").val();
        $("#LoadingPop").click();
        sendPostRequest("/OrderHandlers/Apply.ashx/CheckRefundCondition", JSON.stringify({ orderId: $("#lblOrderId").html(),
            passenger: passenger, voyages: voyage, refundType: refundType, pnr: pnrPair,
            DelegageCancelPNR: $("#chkdelegateCanclePNR").is(":checked")
        }), function (result)
        {
            ClearProp();
            var stepIndex = 1;
            if (!result.CheckCondition)
            {
                stepIndex = 6;
            } else
            {
                $("#tipItem1,#tipItem2,#tipItem3,#tipItem4").addClass("proceeding");
                $("#tipSuccess,#tipFail").addClass("h");
                $("#tipProceding").removeClass("h");
                $("#CheckProcessPop").click();
            }
            interval = setInterval(function ()
            {
                switch (stepIndex)
                {
                    case 1:
                        clearStatus($("#tipItem1")).addClass(result.IsSameName ? "succeed" : "warning"); break;
                    case 2:
                        clearStatus($("#tipItem2")).addClass(result.TicketUnUse ? "succeed" : "warning"); break;
                    case 3:
                        clearStatus($("#tipItem3")).addClass(result.PNRCancled ? "succeed" : "warning"); break;
                    case 4:
                        clearStatus($("#tipItem4")).addClass(result.IsNotPrinted ? "succeed" : "warning"); break;
                    case 5:
                        $("#tipProceding").addClass("h");
                        if (result.Successed) $("#tipSuccess").removeClass("h");
                        else $("#tipFail").removeClass("h"); break;
                    case 6:
                        clearInterval(interval);
                        if (!result.Successed)
                        {
                            $(sender).add($(sender).next()).removeAttr("disabled");
                            return;
                        }
                        var filePath = "";
                        try {
                            filePath = document.getElementById("iframeChildren").contentWindow.document.getElementById("hfdFilePath").value;
                        } catch(e) {

                        } 
                        var parameters = JSON.stringify({ "orderId": $("#lblOrderId").html(), "pnrCode": pnrPair, "passengers": passenger, "voyages": voyage, "refundType": refundType, "reason": $("#txtRefundReason").val(), DelegageCancelPNR: $("#chkdelegateCanclePNR").is(":checked"), "filePath": filePath });
                        sendPostRequest("/OrderHandlers/Apply.ashx/ApplyRefund", parameters, function (e)
                        {
                            ClearProp();
                            if (e == "OK")
                            {
                                alert("申请成功!");
                                window.location.href = "ApplyformList.aspx";
                            }
                            else if (e == "OK1")
                            {
                                alert("申请成功!");
                                window.location.href = "ApplyformList.aspx";
                            }
                            else
                            {
                                alert(e);
                                $(sender).add($(sender).next()).removeAttr("disabled");
                                return;
                            }
                        }, function (e)
                        {
                            if (e.status == 300)
                            {
                                alert(JSON.parse(e.responseText));
                            }
                            else if (e == "OK1")
                            {
                                alert("申请成功!");
                                window.location.href = "ApplyformList.aspx";
                            }
                            else
                            {
                                ClearProp();
                                //alert("系统故障，请联系平台");
                                alert(JSON.parse(e.responseText));
                                ShowApplyFormBody(true);
                                $(sender).add($(sender).next()).removeAttr("disabled");
                                $("#mask").show();
                            }
                            $(sender).add($(sender).next()).removeAttr("disabled");
                        });
                        break;
                    default:
                        break;
                }
                stepIndex++;
            }, 500);
        }, function (e)
        {
            ClearProp();
            //alert("系统故障，请联系平台");
            alert(JSON.parse(e.responseText));
            ShowApplyFormBody(true);
            $(sender).add($(sender).next()).removeAttr("disabled");
            $("#mask").show();
        });
    } else {
        $(sender).add($(sender).next()).removeAttr("disabled");
    }

    function clearStatus(target) {
        target.removeClass("proceeding").removeClass("warning").removeClass("succeed");
        return target;
    }
}


// 提交改期申请
function CommitApplyPostpone(sender) {
    $(sender).add($(sender).next()).attr("disabled", "disabled");
    eval($("#JsParameter").val());
    if (ValidateApplyPostpone()) {
        var flights = GetApplyPostponeFlights();
        var parameters = JSON.stringify({ "orderId": $("#lblOrderId").html(), "pnrCode": $("#hidApplyPNR").val(), "passengers": $("#hidApplyPassengers").val(), "voyages": flights, "remark": $("#txtPostponeRemark").val(), carrair: JsParameter.Carrier });
        sendPostRequest("/OrderHandlers/Apply.ashx/ApplyPostpone", parameters, function (e) {
            if (e == "OK") {
                alert("申请成功");
                window.location.href = "ApplyformList.aspx";
            } else {
                alert(e);
                $(sender).add($(sender).next()).removeAttr("disabled");
                return;
            }
        }, function (e) {
            if (e.status == 300) {
                alert(JSON.parse(e.responseText));
            } else {
                alert("系统故障，请联系平台");
            }
            $(sender).add($(sender).next()).removeAttr("disabled");
        });
    } else {
        $(sender).add($(sender).next()).removeAttr("disabled");
    }
}
// 提交升舱申请
function CommitApplyUpgrade(sender) {
    $(sender).add($(sender).next().next()).attr("disabled", "disabled");
    if (ValidateApplyUpgrade()) {
        var flightInfo = $("#hidApplyFlights").data("Tag");
        var parameters = JSON.stringify({ "orderId": $("#lblOrderId").html(),
            "pnrCode": $("#txtPNR").val(), "passengers": GetApplyPassengerInfos(), //$("#hidApplyPassengers").val(),
            "voyages": flightInfo, originalPNR: $("#hidApplyPNR").val()
        });
        sendPostRequest("/OrderHandlers/Apply.ashx/ApplyUpgrade", parameters, function (e) {
            if (e.IsSuccess) {
                (window.parent || window).location.href = "/FlightReserveModule/ChoosePolicyWithUpgrade.aspx" + e.QueryString;
            } else {
                alert(e.QueryString);
                $(sender).add($(sender).next().next()).removeAttr("disabled");
                return;
            }
        }, function (e) {
            if (e.status == 300) {
                alert(JSON.parse(e.responseText));
            } else {
                alert("系统故障，请联系平台");
            }
            $(sender).add($(sender).next().next()).removeAttr("disabled");
        });
    } else {
        $(sender).add($(sender).next().next()).removeAttr("disabled");
    }
}
function GetApplyPNR(sender) {
    var pnrInfo = $(".divPNRCodeInfo", $(sender).parent().parent());
    var pnrs = / ?((\w{6})\(小\))? ((\w{6})\(大\))?/.exec($(".pnrCode", pnrInfo).html());

    return (typeof pnrs[2] != "undefined" ? pnrs[2] : "") + "|" + (typeof pnrs[3] != "undefined" && pnrs[3] != "" ? pnrs[4] : "");
}
function GetApplyPassengers(sender) {
    var passengerInfos = $("#passengers table", $(sender).parent().parent());
    var passengerIds = '';
    var passengerNames = '';
    var count = 0;
    $("input:checkbox:checked", passengerInfos).each(function () {
        if (count > 0) {
            passengerIds += ",";
            passengerNames += "&nbsp;&nbsp;&nbsp;";
        }
        passengerIds += $(this).val();
        passengerNames += $(this).parent().parent().children().first().html();
        count++;
    });
    return [count, passengerIds, passengerNames];
}
function GetApplyPassengerInfos() {
    var result = new Array();
    $("#passengers input:checked").each(function () {
        result.push({
            Name: $(this).parent().parent().children().eq(0).html(),
            PassengerType: $(this).parent().parent().children().eq(1).children().eq(1).html(),
            CredentialsType: $(this).parent().parent().children().eq(3).children().eq(1).html(),
            Credentials: $(this).parent().parent().children().eq(3).children().eq(0).html(),
            Phone: $(this).parent().parent().children().eq(2).children().eq(2).html(),
            PassengerId: $(this).val()
        });
    });
    return result;
}
function GetApplyFlights(sender) {
    var flightInfos = $("#voyages table", $(sender).parent().parent());
    var flightIds = '';
    var flightNames = '';
    var carrier = '';
    var count = 0;
    var flights = new Array();
    $("input:checkbox:checked", flightInfos).each(function () {
        if (count == 0) {
            carrier = $(this).parent().parent().children().first().html();
        } else {
            flightIds += ",";
            flightNames += "&nbsp;&nbsp;&nbsp;";
        }
        flights.push({
            flightId: $(this).val(),
            Departure: $(this).parent().parent().children().eq(4).children().eq(2).html(),
            Arrival: $(this).parent().parent().children().eq(5).children().eq(2).html(),
            Airline: $(this).parent().parent().children().eq(0).html(),
            FlightNo: $(this).parent().parent().children().eq(1).children().eq(1).html(),
            AirCraft: $(this).parent().parent().children().eq(2).html(),
            Bunk: $(this).parent().parent().children().eq(3).children().eq(0).html().charAt(0)
        });
        flightIds += $(this).val();
        flightNames += $(this).parent().parent().children().eq(4).children().eq(0).html() + " - " + $(this).parent().parent().children().eq(5).children().eq(0).html();
        count++;
    });
    return [count, flightIds, flightNames, carrier, flights];
}
function SetApplyPostponeFlights(sender) {
    var flightInfos = $("#voyages table", $(sender).parent().parent());
    var postponeFlightInfo = $("#divPostpone table");
    var postponeTitle = "<tr><th>航段</th><th>航班号</th><th>航班日期</th><th>舱位</th><th>新航班号</th><th>新航班日期</th></tr>";
    postponeFlightInfo.empty();
    postponeFlightInfo.append($(postponeTitle));
    $("input:checkbox:checked", flightInfos).each(function () {
        var selectedFlight = $(this).parent().parent();
        var flightId = $(this).val();
        var voyage = selectedFlight.children().eq(4).children().eq(0).html() + " - " + selectedFlight.children().eq(5).children().eq(0).html();
        var flightNo = selectedFlight.children().eq(1).text();
        var flightDate = selectedFlight.children().eq(6).text();
        var bunk = selectedFlight.children().eq(3).children().eq(0).html();
        var postponeFlight = "<tr class='flightContent'><td><label style='display:none;' id='flightId'>" + flightId + "</label><label>" + voyage + "</label></td><td>" + flightNo + "</td>"
         + "<td>" + flightDate + "</td><td>" + bunk + "</td><td><input type='text' class='text flightNo' "
         + " value='' /></td>"
         + "<td><input type='text' class='text flightDate text-s' onclick='WdatePicker({startDate:\"" + flightDate + "\",isShowClear:false,readOnly:true,minDate:\"%y-%M-%d\"});' /></td></tr>";
        postponeFlightInfo.append($(postponeFlight));
    });
}
function SetApplyUpgradeFlights(sender) {
    var flightInfos = $("#voyages table", $(sender).parent().parent());
    var postponeFlightInfo = $("#tbUpgradeAirLines");
    postponeFlightInfo.empty();
    $("input:checkbox:checked", flightInfos).each(function (index) {
        var selectedFlight = $(this).parent().parent();
        var flightId = $(this).val();
        var voyage = selectedFlight.children().eq(4).children().eq(0).html() + " - " + selectedFlight.children().eq(5).children().eq(0).html();
        var flightDate = selectedFlight.children().eq(6).html();
        var bunk = selectedFlight.children().eq(3).children().eq(0).html();
        var postponeFlight = "<tr class='flightContent'><td><label style='display:none;' id='flightId'>" + flightId + "</label><label class='Voyage'>" + voyage + "</label></td>"
         + "<td>" + flightDate + "</td><td>" + bunk + "</td>"
         + "<td><input type='text' name='date" + index + "' class='text flightDate text-s' onclick='WdatePicker({isShowClear:false,readOnly:true,minDate:\"%y-%M-%d\"});' /></td></tr>";
        postponeFlightInfo.append($(postponeFlight));
    });
}
function SetApplyInfo(pnr, passengers, flights, flightObj) {
    $("#hidApplyPNR").val(pnr);
    $("#hidApplyPassengers").val(passengers);
    $("#hidApplyFlights").val(flights).data("Tag", flightObj);
}
function ValidateApplyRefund() {
    var refundReason = $.trim($("#txtRefundReason").val());
    if (refundReason == "") {
        alert("请输入退/废票原因");
        $("#txtRefundReason").select();
        return false;
    } else if (refundReason.length > 200) {
        alert("退/废票原因不能超过200字");
        $("#txtRefundReason").select();
        return false;
    }
    if ($("#divRefundType input:radio:checked").length == 0) {
        alert("请选择退票类型");
        return false;
    }
    return true;
}
function ValidateApplyPostpone() {
    var inputs = $("#divPostpone table");
    var flg = true;
    $(".flightNo", inputs).each(function () {
        if ($.trim($(this).val()) == "") {
            alert("请输入新航班号");
            $(this).select();
            flg = false;
            return false;
        } else if (!(/^\w{4,5}$/).test($.trim($(this).val()))) {
            alert("航班号格式错误");
            $(this).select();
            flg = false;
            return false;
        }
    });
    if (flg) {
        $(".flightDate", inputs).each(function () {
            if ($(this).val() == "") {
                alert("请选择新航班日期");
                $(this).focus();
                flg = false;
                return false;
            }
        });
    }
    if (flg && $.trim($("#txtPostponeRemark").val()).length > 200) {
        alert("改期备注不能超过200字");
        $("#txtPostponeRemark").select();
        return false;
    }
    return flg;
}
//验证升舱编码
function ValidateApplyUpgrade() {
    var pnr = $("#txtPNR").val();
    if ($.trim(pnr) == "") {
        alert("请输入编码！");
        $("#txtPNR").select();
        return false;
    }
    if (!/^[\w\d]{6}$/.test(pnr)) {
        alert("请输入正确的编码");
        return false;
    }
    if ($("#hidApplyPNR").val().indexOf($("#txtPNR").val()) > -1) {
        alert("升舱编码与原编码相同");
        return false;
    }
    return true;
}
function CheckDate() {
    var passed = true;
    $("#tbUpgradeAirLines .text-s").each(function (itemIndex, element) {
        var date = $.trim($(element).val());
        if (date == "") {
            alert("请选择改签日期");
            passed = false;
            $(element).select();
            return;
        }
    });
    return passed;
}

function GetApplyPostponeFlights() {
    var postponeFlights = "";
    $("#divPostpone .flightContent").each(function (index) {
        if (index > 0) {
            postponeFlights += ",";
        }
        var flightNo = $(".flightNo", this).val();
        postponeFlights += $("#flightId", this).html() + "|" + flightNo + "|" + $(".flightDate", this).val();
    });
    return postponeFlights;
}
function GetApplyUpgradeFlights() {
    var upgradeFlights = "";
    $("#divUpgrade .flightContent").each(function (index) {
        if (index > 0) {
            upgradeFlights += ",";
        }
        upgradeFlights += $("#flightId", this).html() + "|" + $(".flightNo", this).val() + "|" + $(".flightDate", this).val();
    });
    return upgradeFlights;
}
function getDictionaryItems(typeId, callback) {
    sendPostRequest("/OrderHandlers/Apply.ashx/GetDictionaryItems", JSON.stringify({ sdType: typeId }), callback, $.noop);
}
$(function () {
    $("#divRefund input[type='radio']").click(function () {
        $(".h").hide();
        $("#submitApply").removeAttr("disabled");
        if (typeof (param) != "undefined") {
            $("#ScrapTime1,#ScrapTime2").text(param.scrapTime);
            $("#RefundTime1,#RefundTime2,#RefundTime3").text(param.RefundTime);
        }
        if ($(this, $("input:radio:checked")).val() == "-1") {
            if (typeof (param) != "undefined" && !param.scrapEnable && !param.RefundEnabled) {
                $("#BothDisable").show();
                $("#submitApply").attr("disabled", "disabled");
            } else if (typeof (param) != "undefined" && !param.scrapEnable) {
                $("#RefundEnabled").show();
                $("#submitApply").attr("disabled", "disabled");
            }
            else if ($("#hidApplyType").val() == "FALSE") {
                $("#pSpecial").show();
            } else {
                $("#pNotSpecial").show();
            }
        } else if (typeof (param) != "undefined" && !param.RefundEnabled) {
            $("#RefundDisabled").show();
        }
        $("#Reasons").empty();
        var key = $(this).attr("key");
        if (key == "12548" || key == "12549") {
            $("#liAttachment").show();
        } else {
            $("#liAttachment").hide();
        }
        getDictionaryItems($(this).attr("key"), function (rsp) {
            if (rsp) {
                for (var i in rsp) {
                    $("<option>" + rsp[i].Remark + "</option>").appendTo("#Reasons");
                }
                $("#dl_Reasons").remove();
                $("#txtRefundReason").val(rsp[0].Remark);
                $("#Reasons").removeClass("custed").change(function () {
                    $("#txtRefundReason").val($(this).val());
                });
            }
        });
    });
    $("#btnAttachment").click(function () {
        try {
            document.getElementById("iframeChildren").contentWindow.document.getElementById("flAttachment").onchange = function () {
                try {
                    var children = document.getElementById("iframeChildren").contentWindow.document;
                    if (!children.getElementById("flAttachment").value.match(/.jpg|.png|.bmp/i)) {
                        alert("文件格式不正确只支持jpg|png|bmp的图片");
                        return false;
                    }
                    document.getElementById("txtAttachment").value = children.getElementById("flAttachment").value;
                    var card = children.getElementById("btnUploadFile");
                    $(card).trigger("click");
                } catch (e) {

                }
            }
            $(document.getElementById("iframeChildren").contentWindow.document.getElementById("flAttachment")).click();
        } catch (e) {
            alert("上传附件过大，请刷新页面重新上传");
        }
    });
    $("#dl_Reasons li").live("click", function () {
        $("#txtRefundReason").val($(this).find("span").text()).removeClass("null");
    });
    $("#NoandReserve").click(function () {
        $("#btnSearchAireLine,#section2").show();
        $("#btnSavePNR,#section1").hide();
    });
    $("#inputCode").click(function () {
        $("#btnSearchAireLine,#section2").hide();
        $("#btnSavePNR,#section1").show();
    });
});