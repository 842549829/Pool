var editInfo;
//获取航空公司
function getAirlines() {
    var arrayAirlines = $.makeArray($.map($("#divAirlinelist :checked"), function (item) {
        return $(item).attr("value");
    }));
    return arrayAirlines.join('/');
}



$(function () {
    var strLimitation = $("#hfdLimitation").val();
    if ($.trim(strLimitation).length > 0) {
        eval(strLimitation);
        if (limitation.length > 0) {
            $.each(limitation, function (item) {
                limitation[item].Data = JSON.stringify(limitation[item]);
                $.each(limitation[item].Rebate, function (it) {
                    if (limitation[item].Rebate[it].Rebate == null) {
                        limitation[item].Rebate[it].Rebate = NaN;
                    }
                })
            })
            $("#limitationTmpl").tmpl(limitation, { CountAcc: CountAcc }).appendTo("#divPurchaseLimitation");
        }
    }
    $("#btnSave").click(function () {
        if (($("#rbnPurchaseGlobal").attr("checked") == "checked") && ($("#divPurchaseLimitation").find("p").length == 0)) {
            alert("请添加到下面的组");
            return false;
        }
    });

    $("#divPurchaseLimitation p").live("dblclick", function () {
        EmptyInfo();
        if (editInfo != "") {
            $(editInfo).appendTo("#divPurchaseLimitation");
        }
        editInfo = "<p>" + $(this).html() + "</p>";
        var strLimitation = "var limitation=" + $(this).find(":input").val();
        eval(strLimitation);
        var airline = limitation.Airlines.split('/');
        $.map($("#divAirlinelist :checkbox"), function (item) {
            if ($.inArray($(item).val(), airline) >= 0) {
                $(item).attr("checked", "checked")
            }
        })
        var departures = limitation.Departures;
        $("#txtDepartureAirports_rbInclude").attr("checked", "checked");
        $("#txtDepartureAirports_txtAirports").val(departures);
        $("#txtDepartureAirports_txtAirports").blur();
        var rebate = limitation.Rebate;
        $.map($("#rebateInput .simple-box"), function (item) {
            for (var i = 0; i < limitation.Rebate.length; i++) {
                if (parseInt($(".policyType", item).val()) == limitation.Rebate[i].Type) {
                    if (limitation.Rebate[i].AllowOnlySelf) {
                        $(":radio", item).last().attr("checked", "checked");
                        $(".text", item).val(Round(parseFloat(limitation.Rebate[i].Rebate * 100), 3));
                    } else {
                        $(":radio", item).first().attr("checked", "checked");
                    }
                }
            }
        });
        if ($("#rbnPurchaseOnlySelfNormalPolicy").attr("CHECKED") == "checked") {
            $("#PurchaseOnlySelfNormalPolicy").show();
        }
        if ($("#rbnPurchaseNormalPolicy").attr("CHECKED") == "checked") {
            $("#PurchaseOnlySelfNormalPolicy").hide();
        }

        if ($("#rbnPurchaseOnlySelfBargainPolicy").attr("CHECKED") == "checked") {
            $("#PurchaseOnlySelfBargainPolicy").show();
        }
        if ($("#rbnPurchaseBargainPolicy").attr("CHECKED") == "checked") {
            $("#PurchaseOnlySelfBargainPolicy").hide();
        }
        $(this).remove();
    });

    $(".remove").live("click", function () {
        $(this).parent().remove();
    });
    $("#addSubGroup").click(function () {
        editInfo = "";
        var limitObject = getLimitation();
        var airline = limitObject.Airlines;
        if (airline == "") {
            alert("请选择限制航空公司");
            return false;
        }
        var departure = limitObject.Departures;
        if (departure == "" || departure == undefined) {
            alert("请选择限制出港地");
            return false;
        }
        var reg = /^\d{1,2}(\.\d{1})?$/;
        if ($("#rbnPurchaseOnlySelfNormalPolicy").attr("CHECKED") == "checked") {
            if (!reg.test($("#txtDefaultRebateAdultNormalPolicy").val())) {
                alert("下级默认返点（普通政策）格式错误,只支持保留一位小数");
                $("#txtDefaultRebateAdultNormalPolicy").select();
                return false;
            }
        }
        if ($("#rbnPurchaseOnlySelfBargainPolicy").attr("CHECKED") == "checked") {
            if (!reg.test($("#txtDefaultRebateAdultBargainPolicy").val())) {
                alert("下级默认返点（特价政策）格式错误,只支持保留一位小数");
                $("#txtDefaultRebateAdultBargainPolicy").select();
                return false;
            }
        }
        var limit = JSON.stringify(limitObject);
        limitObject.Data = limit;
        $("#limitationTmpl").tmpl([limitObject], { CountAcc: CountAcc }).appendTo("#divPurchaseLimitation");
        EmptyInfo();
    })
});


//获取出港城市
function getDepartures() {
    var departures = "";
    if ($("#txtDepartureAirports_rbInclude").attr("checked") == "checked") {
        departures = $("#txtDepartureAirports_txtAirports").val();
    }
    else {
        departures = $("#txtDepartureAirports_lbSource option").val();
    }
    return departures;
}
//获取返点集合
function getRates() {
    return $.makeArray($.map($("#rebateInput .simple-box"), function (item) {
        var policyRebate = new Object();
        policyRebate.Rebate = Round(parseFloat($(".text", item).val()) / 100, 3);
        policyRebate.Type = parseInt($(".policyType", item).val());
        policyRebate.AllowOnlySelf = ($(":radio", item).last().attr("checked") == "checked");
        return policyRebate;
    }));
}
function getLimitation() {
    var limit = new Object();
    limit.Airlines = getAirlines();
    limit.Departures = getDepartures();
    limit.Rebate = getRates();
    return limit;
}

function TranslateRelationName(relationType) {
    switch (relationType) {
        case 0: return "普通政策：";
        case 1: return "特价政策：";
        case 2: return "团队政策：";
        case 3: return "缺口政策：";
        case 4: return "特殊政策：";
        default: return "政策";
    }
}

function RenderRate(rate) {
    if (isNaN(rate)) {
        return "";
    } else {
        return Round(parseFloat(rate * 100), 3);
    }
}

function TranslatePolicyTypeName(policyType) {
    if (policyType == 4) {
        return "只显示我的默认政策";
    } else {
        return "只显示我的默认返点政策";
    }
}
var limitationCount = 0;
function CountAcc() {
    return limitationCount++;
}

function EmptyInfo() {
    $("#divAirlinelist :checkbox").removeAttr("checked");
    $("#txtDepartureAirports_rbInclude").attr("checked", "checked");
    $("#txtDepartureAirports_txtAirports").val("");
    $("#txtDepartureAirports_txtAirports").blur();
    $.map($("#rebateInput .simple-box"), function (item) {
        $(".text", item).val("");
        $(":radio", item).first().attr("checked", "checked");
        $(":radio", item).last().removeAttr("checked");
    });
    if ($("#rbnPurchaseOnlySelfNormalPolicy").attr("CHECKED") == "checked") {
        $("#PurchaseOnlySelfNormalPolicy").show();
    }
    if ($("#rbnPurchaseNormalPolicy").attr("CHECKED") == "checked") {
        $("#PurchaseOnlySelfNormalPolicy").hide();
    }

    if ($("#rbnPurchaseOnlySelfBargainPolicy").attr("CHECKED") == "checked") {
        $("#PurchaseOnlySelfBargainPolicy").show();
    }
    if ($("#rbnPurchaseBargainPolicy").attr("CHECKED") == "checked") {
        $("#PurchaseOnlySelfBargainPolicy").hide();
    }
}