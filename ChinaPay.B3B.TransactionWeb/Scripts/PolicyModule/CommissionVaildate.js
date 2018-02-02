function vaildateCommission(type) {
    var reg = /^[0-9]{1,10}(\.[0-9])?$/;
    if ($(".canHaveSubordinate").css("display") != "none" && $("#txtInternalCommission").val() == "") {
        alert("内部返佣不能为空，请填写");
        return false;
    }
    if ($(".canHaveSubordinate").css("display") != "none" && $("#txtInternalCommission").size() != 0) {
        if ($("#txtInternalCommission").val() != "" && !reg.test($("#txtInternalCommission").val())) {
            alert("内部返佣格式错误，只能是数字，请填写");
            $("#txtInternalCommission").val("");
            return false;
        }
        if ($("#txtInternalCommission").val() > 100) {
            alert("内部返佣信息不能大于100");
            return false;
        }
    }
    if ($("#txtSubordinateCommission").val() == "") {
        alert("下级返佣不能为空，请填写");
        return false;
    }
    if ($("#txtSubordinateCommission").size() != 0) {
        if ($("#txtSubordinateCommission").val() != "" && !reg.test($("#txtSubordinateCommission").val())) {
            alert("下级返佣格式错误，只能是数字，请填写");
            $("#txtSubordinateCommission").val("");
            return false;
        }
        if ($("#txtSubordinateCommission").val() > 100) {
            alert("下级返佣信息不能大于100");
            return false;
        }
    }
    if ($(".allowBrotherPurchase").css("display") != "none" && $("#txtProfessionCommission").val() == "") {
        alert("同行返佣不能为空，请填写");
        return false;
    }
    if ($(".allowBrotherPurchase").css("display") != "none" && $("#txtProfessionCommission").size() != 0) {
        if ($("#txtProfessionCommission").val() != "" && !reg.test($("#txtProfessionCommission").val())) {
            alert("同行返佣格式错误，只能是数字，请填写");
            $("#txtProfessionCommission").val("");
            return false;
        }
        if ($("#txtProfessionCommission").val() > 100) {
            alert("同行返佣信息不能大于100");
            return false;
        }
    }
    if (type == null) {
        if ($("#txtPrice").val() == "") {
            alert("价格不能为空，请填写");
            return false;
        }
        if ($("#txtPrice").size() != 0) {
            if ($("#txtPrice").val() != "" && !reg.test($("#txtPrice").val())) {
                alert("价格格式错误，只能是数字，请填写");
                $("#txtPrice").val("");
                return false;
            }
        }
    }
    if (type == "low") {
        if ($("#txtPrice").css("display") == "inline-block") {

            if ($("#txtPrice").val() == "") {
                alert("价格不能为空，请填写");
                return false;
            }
            if ($("#txtPrice").size() != 0) {
                if ($("#txtPrice").val() != "" && !reg.test($("#txtPrice").val())) {
                    alert("价格格式错误，只能是数字，请填写");
                    $("#txtPrice").val("");
                    return false;
                }
            }
        }
        if ($("#txtDisCount").css("display") == "inline-block") {
            if ($("#txtDisCount").val() == "") {
                alert("折扣不能为空，请填写");
                return false;
            }
            if ($("#txtDisCount").size() != 0) {
                if ($("#txtDisCount").val() != "" && !reg.test($("#txtDisCount").val())) {
                    alert("折扣格式错误，只能是数字，请填写");
                    $("#txtDisCount").val("");
                    return false;
                }
            }
        }
    }
    return true;
}