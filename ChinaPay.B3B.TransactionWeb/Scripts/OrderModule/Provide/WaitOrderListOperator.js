var refreshTime = 60;
var num = refreshTime;
var interval;
var refush = true;
$(function () {
    $("#autoRefresh").click(function () {
        if ($("#autoRefresh").attr("checked") == "checked") {
            num = refreshTime;
            $("#countdownId").text(num + "秒");
            $("#countdownSize").show();
            interval = setInterval(function () {
                if (num == 0) {
                    if (refush) {
                        $("#btnSerach").click();
                    }
                    num = refreshTime;
                } else {
                    $("#countdownId").text(num-- + "秒");
                }
            }, 1000);
        } else {
            $("#countdownSize").hide();
            clearInterval(interval);
            num = refreshTime;
        }
    });
    $("#divDeny input[type='radio']").click(function () {
        $("#selDenyReason").empty();
        getDictionaryItems($(this).val(), function (rsp) {
            if (rsp) {
                for (var i in rsp) {
                    $("<option>" + rsp[i].Remark + "</option>").appendTo("#selDenyReason");
                }
            }
        });
    });
    $("#selDenyReason").live("click", function () {
        $("#txtDenyReason").val($(this).find(":selected").html()).removeClass("null");
    });
    $(".ETDZ").live("click", function () {
        var oldOrderId1 = $("#hfdOrderId").val();
        if (oldOrderId1 != "") {
            unLock(oldOrderId1);
        }
        var orderId = $(this).attr("orderId");
        $("#hfdOrderId").val(orderId);
        var tr = $(this).parent().parent();
        $(".PreOrder").hide();
        sendPostRequest("/OrderHandlers/Order.ashx/QueryPaidforETDZOrder", JSON.stringify({ "orderId": orderId }), function (result) {
            if (result.IsLocked) {
                refush = false;
                $("#hfdRequireChangePNR").val(result.RequireChangePNR);
                $("#hfdForbidChangPNR").val(result.ForbidChangPNR);
                var html = new Array();
                html.push("<td colspan='12'><div class='create-ticket-box' style='position:relative;z-index:2000;'><span class='arrow'></span><div class='create-ticket-header'>");
                html.push("<p class='" + (result.TicketType == "" ? "'" : "TicketType'") + "style='" + (result.TicketType == "" ? "display:none;" : "") + "'>出票方式：<label><input type='radio' value='1' name='TicketType'" + (result.TicketType == "BSP" ? "checked='checked'" : "") + " />BSP</label>" +
                    "<label><input type='radio' value='0' name='TicketType'" + (result.TicketType == "B2B" ? " checked='checked'" : "") + " />B2B</label></p>");
                html.push("<p>出票OFFICE号：<select class='text OfficeNo' style='width: 90px;'>");
                for (var item in result.OfficeNo) {
                    html.push("<option " + (result.OfficeNo[item] == result.CurrentOfficeNO ? "selected='selected'" : "") + " value='" + result.OfficeNo[item] + "'>" + result.OfficeNo[item] + "</option>");
                }
                html.push("</select></p>");
                html.push("<p style='display:" + (result.ForbidChangPNR ? "none" : "") + ";'><label><input type='checkbox' class='needChangePNR' " + (result.RequireChangePNR ? "checked='checked'" : "") + " />需换编码出票</label><span style='display:" + (result.RequireChangePNR ? "" : "none") + ";'>    大编：<input type='text' class='text text-s NewBPNR' /> 小编：<input type='text' class='text text-s NewPNR' /><label class='OrignalBPNRCode' style='display:none;' >" + result.BPNRCode + "</label><label class='OrignalPNRCode' style='display:none;' >" + result.PNRCode + "</label></span></p>");
                html.push("</div> <div class='create-ticket-content'><table>");
                for (var passenger in result.Passengers) {
                    html.push("<tr><td style='text-align:right;border-right-color:#fff !important;'><label class='PassengerName'>" + result.Passengers[passenger].Name + "</label> " + (result.Passengers[passenger].PassengerType)
                        + " 票号：</td><td style='text-align:left;border-left-color:#fff !important;'><input type='text' class='text settleCode' style='width: 30px;' value='" + result.Passengers[passenger].SettleCode + "' />&nbsp;&nbsp;<input type='text' class='text ticketNo parentTicketNo'/>");
                    html.push(result.Passengers[passenger].TicketNoCount > 1 ? "<span>-</span><input type='text' class='text ticketNOend' /><span class='addTicketNo'>+</span>" : "");
                    html.push("</td></tr>");
                }
                html.push("</table><div class='btns'><button class='btn class1 btnETDZ' type='button'>出票</button><button class='btn class2 Cancel' type='button' orderId ='" + orderId + "'>取消</button></div></div></div><td>");
                tr.next().html(html.join(''));

                tr.next().find(".settleCode").each(function (index, item) {
                    if (index == 0) {
                        $(item).blur(function () {
                            $(".settleCode", tr.next()).val($(this).val());
                        });
                    } else {

                        $(item).attr("disabled", "disabled");
                    }
                });
                tr.next().show();
            } else {
                alert(result.LockErrorMsg);
                $("#mask").hide();
            }
        }, function (e) {
            if (e.statusText == "timeout") {
                alert("服务器忙");
            } else {
                alert(e.responseText);
            }
        });
        var bHeight = $("body").innerHeight();
        $("#mask").css({ "display": "block", "position": "absolute", "height": bHeight + 500 });
    });
    $(".needChangePNR").live("click", function () {
        if ($(this).attr("checked") == "checked") {
            $(this).parent().next().show();
        } else {
            $(this).parent().next().hide();
        }
    });
    $(".btnETDZ").live("click", function () {
        var requireChangePNR = false;
        requireChangePNR = $("#hfdRequireChangePNR").val() == "true" ? true : false;
        var forbidChangPNR = false;
        forbidChangPNR = $("#hfdForbidChangPNR").val() == "true" ? true : false;
        var container = $(this).parent().parent().parent();
        if (
            !etdz(requireChangePNR, forbidChangPNR, container)) {
            return false;
        }
    });
    $(".Cancel").live("click", function () {
        var oldOrderId2 = $(this).attr("orderId");
        if (oldOrderId2 != "") {
            unLock(oldOrderId2);
            $(this).parent().parent().parent().parent().parent().prev("tr").find(".LockInfo").hide();
            $(".PreOrder").hide();
        }
        $("#mask").css({ "display": "none" });
    });
    $(".denyETDZ").live("click", function () {
        refush = false;
        $(".PreOrder").hide();
        $("#txtDenyReason").val("");
        $("#lnkDeny").click();
        $("#hfdDenyETDZOrderId").val($(this).attr("orderId"));
        setOperationEnable(true);
    });
    $(".removeTicketNo").live("click", function () {
        var parent = $(this).parent().parent();
        if (parent.prev().find(".removeTicketNo").size() == 0 && parent.next().find(".removeTicketNo").size() == 0) {
            parent.prev().find(".ticketNOend").show().prev().show();
        }
        parent.remove();
        ControlPageHeight();
    });
    $(".addTicketNo").live("click", function () {
        var self = $(this);
        var parent = self.parent().parent();
        var ticket = new Array();
        ticket.push("<tr>");
        ticket.push("<td style='text-align:right;border-right-color:#fff !important;'></td><td style='text-align:left;border-left-color:#fff !important;'><input type='text' class='text settleCode' disabled='disabled' value='");
        ticket.push($(".settleCode:first").val());
        ticket.push("' />&nbsp;&nbsp;<input type='text' class='text ticketNo childrenTicketNo'><span class='removeTicketNo'>-</span></td></tr>");
        parent.after(ticket.join(""));
        self.prev().hide().prev().hide();
    });
})
function unLock(orderId) {
    sendPostRequest("/OrderHandlers/Order.ashx/UnLock", JSON.stringify({ orderId: orderId }), function () {
        $("#hfdOrderId").val("");
        refush = true;
    }, function (e) {
        if (e.status == 300) {
            alert(JSON.parse(e.responseText));
        } else {
            alert("系统故障，请联系平台");
        }
        setOperationEnable(true);
    });
}
function etdz(requireChangePNR, forbidChangPNR, Container) {
    var pnrPattern = /^[a-z,A-Z,0-9]{6}$/;
    var ticketPattern = /^[1-9][0-9]{9}$/;
    var pnrCode = $.trim(Container.find(".NewPNR").val());
    var bpnrCode = $.trim(Container.find(".NewBPNR").val());
    var orginalPnrCode = $.trim(Container.find(".OrignalPNRCode").html());
    var orginalBPnrCode = $.trim(Container.find(".OrignalBPNRCode").html());
    if (!forbidChangPNR) {
        if (bpnrCode != '' && pnrCode == '') {
            alert("请输入小编");
            return;
        }
        if (requireChangePNR && pnrCode == "") {
            if (!confirm("小编内容为空\n本订单的匹配政策中需要换编码出票，确定不换编码继续出票吗？")) {
                return;
            }
        }
        if (pnrCode != '' && !pnrPattern.test(pnrCode)) {
            alert("小编格式错误");
            return;
        }
        if (bpnrCode != '' && !pnrPattern.test(bpnrCode)) {
            alert("大编格式错误");
            return;
        }
        if ((pnrCode != "" && orginalPnrCode.indexOf(pnrCode) > -1) || (bpnrCode != "" && orginalBPnrCode.indexOf(bpnrCode) > -1)) {
            alert("编码不能和原编码相同！"); return;
        }
    }
    var newPNRPair = pnrCode + "|" + bpnrCode;
    var ticketNos = new Array();
    var flg = true;
    var settleCode;
    var iIndex = 0;
    Container.find(".ticketNo").each(function (index) {
        var that = $(this);
        var settleCodeCtl = that.siblings(".settleCode");
        var ticketNoEndCtl = that.siblings(".ticketNOend");
        var ticketNo = $.trim(that.val());
        settleCode = $.trim(settleCodeCtl.val());
        var endfixNumber = -1;
        if (ticketNo == '') {
            alert("请输入票号");
            $(this).select();
            flg = false;
            return false;
        } else if (!ticketPattern.test(ticketNo)) {
            alert("票号格式错误");
            $(this).select();
            flg = false;
            return false;
        } else if (settleCode == "") {
            alert("请输结算代码");
            settleCodeCtl.select();
            flg = false;
            return false;
        } else if (!/^\d{3}$/.test(settleCode)) {
            alert("结算代码格式错误");
            settleCodeCtl.select();
            flg = false;
            return false;

        }
        if (ticketNoEndCtl.is(":visible")) {
            var endFix = $.trim(ticketNoEndCtl.val());
            if (endFix == "") {
                alert("请输入后续票号后缀");
                ticketNoEndCtl.select();
                flg = false;
                return false;
            } else if (!/^\d{2}$/.test(endFix)) {
                alert("票号后缀格式错误，请输入两位数字");
                ticketNoEndCtl.select();
                flg = false;
                return false;
            }
            endfixNumber = parseInt(endFix, 10);
        }
        var number = parseInt(ticketNo, 10);
        var numbers = [];
        if (endfixNumber != -1) {
            var lastNumber = number - (number % 100) + endfixNumber;
            if (number % 100 > endfixNumber) {
                lastNumber += 100;
            }
            for (var i = number; i <= lastNumber; i++) {
                numbers.push(i.toString());
            }
        } else {
            numbers.push(ticketNo.toString());
        }
        var passengerName = $(this).parent().parent().children().find(".PassengerName").html();
        if (passengerName == "" || passengerName == null) {
            ticketNos[iIndex - 1].TicketNos.push(numbers.toString());
        } else {
            ticketNos[iIndex] = { "Name": passengerName, "TicketNos": numbers, NewSettleCode: settleCode };
            iIndex++;
        }
    });
    if (flg) {
        setOperationEnable(false);
        var ticketType = Container.find(".TicketType").size() > 0 ? parseInt(Container.find(".TicketType input:checked").val(), 10) : 1;
        var parameters = JSON.stringify({ "orderId": $("#hfdOrderId").val(), "newPNRPair": newPNRPair, "ticketNos": ticketNos, "NewSettleCode": settleCode, officeNo: $(".OfficeNo option:checked", Container).val(), ticketType: ticketType });
        sendPostRequest("/OrderHandlers/Order.ashx/ETDZ", parameters, function (e) {
            alert("操作成功");
            refush = true;
            $("#btnSerach").click();
            $("#hfdOrderId").val("");
        }, function (e) {
            if (e.status == 300) {
                alert(JSON.parse(e.responseText));
            } else {
                alert("系统故障，请联系平台");
            }
            setOperationEnable(true);
        });
        $("#mask").css({ "display": "none" });
    }
}
function setOperationEnable(enable) {
    if (enable) {
        $("button, input:button, input:submit").removeAttr("disabled");
    } else {
        $("button, input:button, input:submit").attr("disabled", "disabled");
        $("#btnSerach").removeAttr("disabled");
    }
}
function commitDenyETDZ() {
    var reason = $.trim($("#txtDenyReason").val());
    if (reason == "") {
        alert("请输入拒绝原因");
        $("#txtDenyReason").select();
        return;
    } else if (reason.length > 200) {
        alert("拒绝原因不能超过200字");
        $("#txtDenyReason").select();
        return;
    } else if ($("#divDeny input:checked").size() == 0) {
        alert("请选择拒绝原因类型");
        return;
    }
    var orderId = $("#hfdDenyETDZOrderId").val();
    setOperationEnable(false);
    var parameters = JSON.stringify({ "orderId": orderId, "reason": reason });
    sendPostRequest("/OrderHandlers/Order.ashx/QuicklyDenyETDZ", parameters, function (result) {
        $("#mask").hide();
        $("#divDeny").hide();
        $("#lnkDeny").hide();
        $("#hfdOrderId").val("");
        if (result.IsLocked) {
            alert("操作成功");
            $("#btnSerach").click();
            refush = true;
        } else {
            alert(result.LockErrorMsg);
        }
    }, function (e) {
        if (e.status == 300) {
            alert(JSON.parse(e.responseText));
        } else {
            alert("系统故障，请联系平台");
        }
        setOperationEnable(true);
    });
}
function getDictionaryItems(typeId, callback) {
    sendPostRequest("/OrderHandlers/Apply.ashx/GetDictionaryItems", JSON.stringify({ sdType: typeId }), callback, $.noop);
}