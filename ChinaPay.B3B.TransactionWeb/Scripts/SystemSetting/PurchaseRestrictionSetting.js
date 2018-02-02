$(function () {
    $("#PurchaseOnlySelfNormalPolicy").hide();
    $("#PurchaseOnlySelfBargainPolicy").hide();

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

    $("#btnSave").click(function () {
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
    });
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



    $("#btnUpdateRelation").click(function () {
        if ($(":checkbox:checked", "#dataList tr td").length == 0) {

            alert("请选择用户");
            return;
        } else {
            var str = '';
            for (var i = 0; i < $(":checkbox:checked", "#dataList tr td").length; i++) {
                str += $(":checkbox:checked", "#dataList tr td").eq(i).attr("companyId") + ',';
            }
            str = str.substring(0, str.length - 1);
            sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/UpdateIncomeGroupRelation", JSON.stringify({ "newIncomeGroupId": $("#hfdCurrentIncomeGroupId").val(), "companyIds": str }), function () {
                alert("添加用户成功");
                $(".close").click();
                window.location.href = 'IncomeGroupList.aspx';
            }, function (e) {
                if (e.statusText == "timeout") {
                    alert("服务器忙");
                } else {
                    alert(e.responseText);
                }
            });
        }
    });
    $("#chkAll").click(function () {
        $("#dataList").find("input[type='checkbox']").attr("checked", "checked");
        $("#chkAll").attr("checked", "checked");
    });
    $("#chkOther").click(function () {
        for (var k = 0; k < $("#dataList").find("input[type='checkbox']").length; k++) {
            if ($("#dataList").find("input[type='checkbox']").eq(k).is(":checked")) {
                $("#dataList").find("input[type='checkbox']").eq(k).removeAttr("checked");
            } else {
                $("#dataList").find("input[type='checkbox']").eq(k).attr("checked", "checked");
            }
        }
        if ($("#dataList input[type='checkbox']:checked").length == $("#dataList").find("input[type='checkbox']").length) {
            $("#chkAll").attr("checked", "checked");
        } else {
            $("#chkAll").removeAttr("checked");
        }
    });
    $("#dataList").find("input[type='checkbox']").live("click", function () {
        if ($("#dataList input[type='checkbox']:checked").length == $("#dataList").find("input[type='checkbox']").length) {
            $("#chkAll").attr("checked", "checked");
        } else {
            $("#chkAll").removeAttr("checked");
        }
    });
})