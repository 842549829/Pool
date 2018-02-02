$(function () {
    $(".DepartureWeekFilter,.DepartureDateFilter,.Departure,.Arrival,.Include,.Exclude").tipTip({ limitLength: 3, maxWidth: "300px" });
    $("#btnSave").click(function () {
        return validate($("#hfdSpecialType").val(), $("#hfdIsInternal").val(), $("#hfdIsPeer").val());
    });
    $("input[name='pricetype']").click(function () {
        if ($(this).val() == "0") {
            $(".priceInfo").show();
            $(".sign").html("元");
            $(".discount").hide();
        } if ($(this).val() == "2") {
            $(".priceInfo").hide();
            $(".discount").show();
            $(".sign").html("%");
        }
    });
    SaveDefaultData();
});

//验证修改返佣的信息
function validate(specialType, isInternal, isPeer) {
    var commissionPattern = /^[0-9]{1,2}(\.[0-9])?$/;
    var pricePattern = /^[0-9]{1,10}$/;
    if ($("#radPrice").attr("checked") == "checked") {
        if ($("#txtPrice").val() == "") {
            alert("价格不能为空，请填写");
            return false;
        }
        if ($(".priceInfoT").css("display") != "none" && !pricePattern.test($("#txtPrice").val())) {
            alert("价格格式错误，只能是整数，请填写");
            $("#txtPrice").val("");
            return false;
        }
        if ($(".subordinate").css("display") != "none" && !pricePattern.test($("#txtSubordinateCommission").val())) {
            alert("下级返佣格式错误，只能是整数，请填写");
            $("#txtSubordinateCommission").val("");
            return false;
        }
        if (isInternal == "True") {
            if ( $("#txtInternalCommission").val() == "") {
                alert("内部返佣不能为空，请填写");
                return false;
            }
            if ($(".canHaveSubordinate").css("display") != "none" && !pricePattern.test($("#txtInternalCommission").val())) {
                alert("内部返佣格式错误，只能是整数，请填写");
                $("#txtInternalCommission").val("");
                return false;
            }
        }
        if (isPeer == "True") {
            if ($("#txtProfessionCommission").val() == "") {
                alert("同行返佣不能为空，请填写");
                return false;
            }
            if ($(".allowBrotherPurchase").css("display") != "none" && !pricePattern.test($("#txtProfessionCommission").val())) {
                alert("同行返佣格式错误，只能是整数，请填写");
                $("#txtProfessionCommission").val("");
                return false;
            }
        }
    }
    if ($("#radLapse").attr("checked") == "checked") {
        if ($("#txtLapse").val() == "") {
            alert("直减不能为空，请填写");
            return false;
        }
        if (!commissionPattern.test($("#txtLapse").val())) {
            alert("直减格式错误，只能是数字，不能超过100，请填写");
            $("#txtLapse").val("");
            return false;
        }
        if (specialType == "Bloc" || specialType == "Business") {
            if (!commissionPattern.test($("#txtSubordinateCommission").val())) {
                alert("下级返佣格式错误，只能是数字，不能超过100，请填写");
                $("#txtSubordinateCommission").val("");
                return false;
            }
            if (isInternal == "True") {
                if ($("#txtInternalCommission").val() == "") {
                    alert("内部返佣不能为空，请填写");
                    return false;
                }
                if (!commissionPattern.test($("#txtInternalCommission").val())) {
                    alert("内部返佣格式错误，只能是数字，不能超过100，请填写");
                    $("#txtInternalCommission").val("");
                    return false;
                }
            }
            if (isPeer == "True") {
                if ($("#txtProfessionCommission").val() == "") {
                    alert("同行返佣不能为空，请填写");
                    return false;
                }
                if (!commissionPattern.test($("#txtProfessionCommission").val())) {
                    alert("同行返佣格式错误，只能是数字，不能超过100，请填写");
                    $("#txtProfessionCommission").val("");
                    return false;
                }
            }
        }
    }
    return true;
}

//特殊政策
function ModifyCommission(id, price, priceType, specialType, isInternal, isPeer, internalCommission, subordinateCommission, professionCommission, isBargainBunks) {
    $(".canHaveSubordinate").show();
    $(".allowBrotherPurchase").show();
    $(".subordinate").show();
    $(".commission").show();
    $(".subString").show();
    $("#hidIds").val(id);
    if (isBargainBunks == "False") {
        $(".subString").show();
    } else {
        $(".subString").hide();
    }
    if (priceType == "Price") {
        $(".sign").html("元");
        $("#radPrice").attr("checked", "checked");
        $("#txtPrice").val(price);
    }
    $("#hfdSpecialType").val(specialType);
    $("#hfdIsInternal").val(isInternal);
    $("#hfdIsPeer").val(isPeer);
    $("#txtSubordinateCommission").val(subordinateCommission);
    $("#txtProfessionCommission").val(professionCommission);
    $("#txtInternalCommission").val(internalCommission);
    if (specialType == "Bloc" || specialType == "Business" || specialType == "LowToHigh") {
        if (priceType == "Price") {
            $("#txtPrice").val(price == -1 ? "" : price);
            $("#radPrice").attr("checked", "checked");
            $(".priceInfo").show();
            $(".discount").hide();
            $(".sign").html("元");
        }
        if (priceType == "Subtracting") {
            $("#txtLapse").val(price == -1 ? "" : price);
            $("#radLapse").attr("checked", "checked");
            $(".priceInfo").hide();
            $(".discount").show();
            $(".sign").html("%");
        }
        $(".priceInfoT").show();
        if (priceType == "Commission") {
            $("#txtLapse").val(price == -1 ? "" : price);
            $("#radCommission").attr("checked", "checked");
            $(".priceInfoT").hide();
            $(".sign").html("%");
        }
    } else {
        //        $(".canHaveSubordinate").hide();
        //        $(".allowBrotherPurchase").hide();
        //        $(".subordinate").hide();
        //        $(".commission").hide(); 
        $(".priceInfoT").hide();
    }
    $("#commission").show();
    if (isInternal == "False") {
        $(".canHaveSubordinate").hide();
    } else {
        $(".canHaveSubordinate").show();
    }
    if (isPeer == "False") {
        $(".allowBrotherPurchase").hide();
    } else {
        $(".allowBrotherPurchase").show();
    }
    if (subordinateCommission == "-1") {
        $(".subordinate").hide();
    } else {
        $(".subordinate").show();
    }
    $("#radCommission").hide();
    $("#divOpcial").click();
}