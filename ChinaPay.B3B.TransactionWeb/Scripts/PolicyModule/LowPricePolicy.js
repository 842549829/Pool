$(function () {
    $(".DepartureDateFilter,.Departure,.Arrival,.Transit,.DepartureWeekFilter,.Include,.Exclude").tipTip({ limitLength: 3, maxWidth: "300px" });
    $(".DepartureDateFilter").tipTip({ limitLength: 26, maxWidth: "300px" });
    $("#btnSave").click(function () {
        return valiate($.trim($("#hfdVoyageType").val()), $.trim($("#hfdIsPeer").val()), $.trim($("#hfdIsInternal").val()));
    });
    $("input[name='pricetype']").click(function () {
        if ($(this).val() == "0") {
            $(".priceInfo").show();
            $(".discount").hide();
        } if ($(this).val() == "1") {
            $(".priceInfo").hide();
            $(".discount").show();
        }
    });
    SaveDefaultData();
});
//特价政策
function ModifyCommissionLow(id, InternalCommission, SubordinateCommission, ProfessionCommission, Price, DisCount, Type, canHaveSubordinate, allowBrotherPurchase, voyageType) {
    $("#hidIds").val(id);
    $("#hfdVoyageType").val(voyageType);
    $("#hfdIsInternal").val(canHaveSubordinate);
    $("#hfdIsPeer").val(allowBrotherPurchase);
    $("#txtInternalCommission").val(InternalCommission);
    $("#txtSubordinateCommission").val(SubordinateCommission);
    $("#txtProfessionCommission").val(ProfessionCommission);
    $("#divOpcial").click();
    if (canHaveSubordinate == "False") {
        $(".canHaveSubordinate").hide();
    } else {
        $(".canHaveSubordinate").show();
    }
    if (allowBrotherPurchase == "False") {
        $(".allowBrotherPurchase").hide();
    } else {
        $(".allowBrotherPurchase").show();
    }
    if (voyageType == "RoundTrip") {
        $(".discount1").hide();
    } else {
        $(".discount1").show();
        if (voyageType == "TransitWay") {
            $("#price").hide();
        } else {
            $("#price").show();
        }
    }
    $("#radPrice").removeAttr("checked");
    $("#radDiscount").removeAttr("checked");
    $("#radCommission").removeAttr("checked");
    if (Type == "Price") {
        $("#radPrice").attr("checked", "checked");
        $("#txtPrice").val(Price);
        if (Price == "-1") {
            $("#txtPrice").val("");
        }
        $(".priceInfo").show();
        $(".discount").hide();
        $(".commission1").hide();
    }
    if (Type == "Discount") {
        $("#radDiscount").attr("checked", "checked");
        $("#txtDisCount").val(DisCount);
        $("#txtPrice").val("");
        $(".priceInfo").hide();
        $(".discount").show();
        $(".commission1").hide();
    }
    if (Type == "Commission") { 
        $("#radCommission").attr("checked", "checked");
        $("#txtDisCount").val("");
        $("#txtPrice").val("");
        $("#price").hide();
    }
}

function valiate(voyageType, isPeer, isInternal) {
    var commissionPattern = /^[0-9]{1,2}(\.[0-9])?$/;
    var pricePattern = /^[0-9]{1,10}$/;
    if ($("#txtSubordinateCommission").val() == "") {
        alert("下级返佣不能为空，请填写");
        return false;
    }
    if (!commissionPattern.test($("#txtSubordinateCommission").val())) {
        alert("下级返佣格式错误，只能是数字，不能超过100，请填写");
        $("#txtSubordinateCommission").val("");
        return false;
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
    if (voyageType == "OneWay") {
        if ($("#radPrice").attr("checked") == "checked") {
            if ($("#txtPrice").val() == "") {
                alert("价格不能为空，请填写");
                return false;
            }
            if (!pricePattern.test($("#txtPrice").val())) {
                alert("价格格式错误，只能是数字，请填写");
                $("#txtPrice").val("");
                return false;
            }
        } else if ($("#radDiscount").attr("checked") == "checked") {
            if ($("#txtDisCount").val() == "") {
                alert("折扣不能为空，请填写");
                return false;
            }
            if (!commissionPattern.test($("#txtDisCount").val())) {
                alert("折扣格式错误，只能是3位以内的数字，请填写");
                $("#txtDisCount").val("");
                return false;
            }
        }
    }
    if (voyageType == "RoundTrip") {
        if ($.trim($("#txtPrice").val()) != "") {
            if (!pricePattern.test($("#txtPrice").val())) {
                alert("价格格式错误，只能是数字，请填写");
                $("#txtPrice").val("");
                return false;
            }
        }
    }
    return true;
}