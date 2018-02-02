function ShowReashoInput() {
    $(".fixed,.layers").show();
}
function CancleInput() {
    $(".fixed,.layers").hide();
}

function getDictionaryItems(typeId, callback) {
    sendPostRequest("/OrderHandlers/Apply.ashx/GetDictionaryItems", JSON.stringify({ sdType: typeId }), callback, $.noop);
}

function CheckReason() {
    var reason = $.trim($("#Reason").val());
    if (reason == "") {
        alert("请输入拒绝原因");
        $("#Reason").select();
        return false;
    } else if (reason.length > 200) {
        alert("拒绝原因不能超过200字");
        $("#Reason").select();
        return false;
    } else if ($("#divDeny .check :checked").size() == 0) {
        alert("请选择拒绝类型");
        return false;
    }
    return true;
}

function CheckRate() {
    var isOk = true;
    $(".faxInput").each(function () {
        var that = $(this);
        if ($.trim(that.val()) == "") {
            isOk = false;
            return false;
        }
        if (!rateReg.test($(this).val())) {
            isOk = false;
            return false;
        }
    });
    $(".FeeInput").each(function () {
        var that = $(this);
        if ($.trim(that.val()) == "") {
            isOk = false;
            return false;
        }
        if (!feeReg.test($(this).val())) {
            isOk = false;
            return false;
        }
    });
    if (!isOk) {
        alert("请输入正确的费率和手续费！");
    }
    var pnr = $.trim($("#txtPNR").val());
    var bpnr = $.trim($("#txtBPNR").val());
    if (RequireSeparatePNR) {
        if (pnr == "" && bpnr == "") {
            alert("没有输入分离编码");
            return false;
        } else if (pnr != "" && !pnrReg.test(pnr)) {
            alert("小编码格式不正确！");
            return false;
        }
        if (bpnr != "" && !pnrReg.test(bpnr)) {
            alert("大编码格式不正确");
            return false;
        }
    }

    return isOk;
}

var rateReg = /^100$|^[1-9]?\d$/;
var feeReg = /^\d+(\.\d+)?$/;
var pnrReg = /^[\w\d]{6}$/;

function ReCalc(sender) {
    var that = $(sender);
    if ($.trim(that.val()) == "") return;
    if (!rateReg.test(that.val())) {
        alert("费率输入错误！");
        return;
    }
    var rate = parseInt(that.val());
    var container = that.parent().parent();
    var ticketPrice = parseFloat(that.next().text());
    $(".FeeInput", container).val(rate * ticketPrice / 100);
    var passengerFee = container.parents("table").next();
    var total = 0;
    $(".faxInput", passengerFee).val(rate).each(function (itemIndex, item) {
        //var price = parseFloat($.trim($(item).parent().find(".ticketPrice").text()));
        $(item).parent().next().find("input").val(rate * ticketPrice / 100);
        var refund = CalcRefund(rate, Math.round(rate * ticketPrice) / 100, $(item).next().val());
        $(item).parent().nextAll().find(".RefundFee").val(refund);
        total += refund;
    });
    $(".RefundTotlal", container).val(total);
}

function FillFee(sender) {
    var that = $(sender);
    var rate = that.parent().prev().find(".faxInput").val();
    if ($.trim(that.val()) == "" || $.trim(rate) == "") return;
    if (!feeReg.test(that.val())) {
        alert("手续费格式错误!");
        return;
    }
    rate = parseInt(rate);
    var fee = parseFloat(that.val());
    var container = that.parent().parent();
    var passengerFee = that.parents("table").next();
    var total = 0;
    $(".FeeInput", passengerFee).val(fee).each(function (itemIndex, item) {
        var refund = CalcRefund(rate, fee, $(item).parent().prev().find("input[type='hidden']").val());
        $(item).parent().next().find(".RefundFee").val(refund);
        total += refund;
    });
    $(".RefundTotlal", container).val(total);
}

function CalcRefund(rate, refundFee, other) {
    var result;
    var parameters;
    eval(other);
    var YinShou = parameters.Price - parameters.Commission + parameters.AirportFee + parameters.BAF;
    if (refundFee > parameters.Price) {
        alert("手续费太多了吧！");
        $(".FeeInput").val("");
        return 0;
    }
    if (rate == 100) {
        result = YinShou - refundFee + parameters.Commission;
    } else {
        result = YinShou - refundFee;
    }
    if (result < parameters.AirportFee + parameters.BAF) {
        result = parameters.AirportFee + parameters.BAF;
    }
    return Round(result, 2);
}

$(function () {
    $("#divDeny input[type='radio']").click(function () {
        $("#selDenyReason").empty();
        getDictionaryItems($(this).val(), function (rsp) {
            if (rsp) {
                for (var i in rsp) {
                    $("<option>" + rsp[i].Remark + "</option>").appendTo("#selDenyReason");
                }
                $("#dl_selDenyReason").remove();
                $("#selDenyReason").removeClass("custed").custSelect({ width: 326 });
            }
        });
    });

    $("#dl_selDenyReason li").live("click", function () {
        $("#Reason").val($(this).find("span").text()).removeClass("null");
    });


    $(".FeeInput,.faxInput", $(".Passengers")).add(".RefundTotlal,.RefundFee").attr("readonly", "readonly").css({ border: "none" });
});
