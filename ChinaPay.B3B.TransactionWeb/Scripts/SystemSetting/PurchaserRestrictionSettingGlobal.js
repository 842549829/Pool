$(function () {
    $("#PurchaseOnlySelfNormalPolicy").hide();
    $("#PurchaseOnlySelfBargainPolicy").hide();
    if ($("#rbnPurchaseGlobal").attr("CHECKED") == "checked") {
        $("#divGlobal").show();
    } else {
        $("#divGlobal").hide();
    }

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

    $("#rbnPurchaseOnlySelfNormalPolicy").click(function () {
        $("#PurchaseOnlySelfNormalPolicy").show();
    });
    $("#rbnPurchaseOnlySelfBargainPolicy").click(function () {
        $("#PurchaseOnlySelfBargainPolicy").show();
    });
   
    $("#rbnPurchaseNormalPolicy").click(function () {
        $("#PurchaseOnlySelfNormalPolicy").hide();
    });
    $("#rbnPurchaseBargainPolicy").click(function () {
        $("#PurchaseOnlySelfBargainPolicy").hide();
    });
    $("#rbnPurchaseEach").click(function () {
        $("#divGlobal").hide();
    });
    $("#rbnPurchaseGlobal").click(function () {
        $("#divGlobal").show();
    });
    $("#rbnPurchaseNone").click(function () {
        $("#divGlobal").hide();
    })
    $("input[type='radio'][name='radAir']").click(function () {
        if ($(this).val() == "0") {
            $("#divAirlinelist input[type='checkbox']").attr("checked", "checked");
        } else {
            for (var i = 0; i < $("#divAirlinelist input[type='checkbox']").length; i++) {
                if ($("#divAirlinelist input[type='checkbox']").eq(i).attr("checked")) {
                    $("#divAirlinelist input[type='checkbox']").eq(i).removeAttr("checked");
                } else {
                    $("#divAirlinelist input[type='checkbox']").eq(i).attr("checked", "checked");
                }
            }
        }
    });
})