function etdz() {
    var pnrPattern = /^[a-z,A-Z,0-9]{6}$/;
    var ticketPattern = /^[1-9][0-9]{9}$/;
    var pnrCode = $.trim($("#txtNewPNRCode").val());
    var bpnrCode = $.trim($("#txtNewBPNRCode").val());
    if (bpnrCode != '' && pnrCode == '') {
        alert("请输入小编");
        return;
    }
    if (requireChangePNR && pnrCode == "")
    {
        if (!confirm("小编码内容为空\n本订单的匹配政策中需要换编码出票，确定不换编码继续出票吗？")) {
            return;
        }
    }
    if (pnrCode != '' && !pnrPattern.test(pnrCode)) {
        alert("新小编码格式错误");
        $("#txtNewPNRCode").select();
        return;
    }
    if (bpnrCode != '' && !pnrPattern.test(bpnrCode)) {
        alert("新大编码格式错误");
        $("#txtNewBPNRCode").select();
        return;
    }
    if ((pnrCode != "" && $("#lblPNRCode").text().indexOf(pnrCode) > -1) || (bpnrCode != "" && $("#lblBPNRCode").text().indexOf(bpnrCode) > -1))
    {
        alert("编码不能和原编码相同！");return;
    }
    var newPNRPair = pnrCode + "|" + bpnrCode;
    var ticketNos = new Array();
    var flg = true;
    var settleCode;
    var iIndex = 0;
    $("#passengers .ticketNo").each(function (index) {
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
        var passengerName = $(this).parent().parent().children().first().html();
        if (passengerName == "") {
            ticketNos[iIndex -1].TicketNos.push(numbers.toString());
        } else {
            ticketNos[iIndex] = { "Name": passengerName, "TicketNos": numbers, NewSettleCode: settleCode };
            iIndex++;
        }
    });
    if (flg) {
        setOperationEnable(false);
        var ticketType = $("#TicketTypeContainer").size() > 0 ? parseInt($("#TicketTypeContainer input:checked").val(), 10) : 1;
        var parameters = JSON.stringify({ "orderId": $("#lblOrderId").html(), "newPNRPair": newPNRPair, "ticketNos": ticketNos, "NewSettleCode": settleCode, officeNo: $("#ddlOfficeNo option:checked").val(), ticketType: ticketType });
        sendPostRequest("/OrderHandlers/Order.ashx/ETDZ", parameters, function (e) {
            alert("操作成功");
            window.location.href = $("#hidReturnUrl").val();
        }, function (e) {
            if (e.status == 300) {
                alert(JSON.parse(e.responseText));
            } else {
                alert("系统故障，请联系平台");
            }
            setOperationEnable(true);
        });
    }
}
function denyETDZ() {
    $("#txtDenyReason").val("");
    $("#lnkDeny").click();
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
    var orderId = $("#lblOrderId").html();
    setOperationEnable(false);
    var parameters = JSON.stringify({ "orderId": orderId, "reason": reason });
    sendPostRequest("/OrderHandlers/Order.ashx/DenyETDZ", parameters, function (e) {
        alert("操作成功");
        window.location.href = $("#hidReturnUrl").val();
    }, function (e) {
        if (e.status == 300) {
            alert(JSON.parse(e.responseText));
        } else {
            alert("系统故障，请联系平台");
        }
        setOperationEnable(true);
    });
}
function setOperationEnable(enable) {
    if (enable) {
        $("button, input:button, input:submit").removeAttr("disabled");
    } else {
        $("button, input:button, input:submit").attr("disabled", "disabled");
    }
}
function getDictionaryItems(typeId, callback) {
    sendPostRequest("/OrderHandlers/Apply.ashx/GetDictionaryItems", JSON.stringify({ sdType: typeId }), callback, $.noop);
}
$(function () {
    if (requireChangePNR) {
        $("#needChangePNR").click();
        $(".changeCodeBox").show();
    } else {
        $(".changeCodeBox").hide();
    }
    $("input[name='needChangePNR']").attr("disabled", "disabled");
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
    $(".settleCode").each(function (index, item) {
        if (index == 0) {
            $(item).blur(function () {
                $(".settleCode").val($(this).val());
            });
        } else {
            $(item).attr("disabled", "disabled");
        }
    });
    $(".needChangePNR").live("click", function () {
        if ($(this).attr("checked") == "checked") {
            $(this).parent().next().show();
        } else {
            $(this).parent().next().hide();
        }
    });

    $("#passengers .hd").remove();
    $("#ETDZInfo").prependTo("#passengers");
    $(".addTicketNo").click(function () {
        var self = $(this);
        var parent = self.parents("tr");
        var ticket = new Array();
        ticket.push("<tr><td  colspan='");
        ticket.push(parseInt(parent.children().size()) - 1);
        ticket.push("'></td><td><input type='text' class='text settleCode' disabled='disabled' value='");
        ticket.push($(".settleCode:first").val());
        ticket.push("'/>&nbsp;&nbsp;<input type='text' class='text ticketNo childrenTicketNo'><span class='removeTicketNo'>-</span></td></tr>");
        parent.after(ticket.join(""));
        self.prev().hide().prev().hide();
        $(".removeTicketNo").die("click").live("click", function () {
            var parents = $(this).parents("tr").prev("tr");
            $(this).parents("tr").remove();
            if (parents.next().find(".removeTicketNo").size() == 0) {
                parents.find(".ticketNOend").show().prev().show();
            }
            ControlPageHeight();
        });
    });
});
