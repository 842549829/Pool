function publicVaild() {
    var reg = /^[0-9]{1,10}?$/;
    if ($("#ddlCustomCode").val() != null) {
        if (($("#dropOffice").val() != null && $("#dropOffice").text() == "") && $("#ddlCustomCode").val() != "") {
            alert("请先设置OFFCIE号或自定义编号，无OFFICE或自定义编号无法修改/复制政策。");
            return false;
        }
    } else {
        if (($("#dropOffice").val() != null && $("#dropOffice").text() == "") && $("#ddlCustomCode").val() != "") {
            alert("请先设置OFFCIE号，无OFFICE无法修改/复制政策。");
            return false;
        }
    }
    var specialType = $.trim($("#titlePolicy").html());

    if (specialType == "单程控位产品") {
        if ($("#txtDepartureAirports_txtAirport").val() == "") {
            alert("始发地不能为空，请选择至少一个城市作为始发地!");
            return false;
        }
        if ($("#txtArrivalAirports_txtAirport").val() == "") {
            alert("到达地不能为空，请选择至少一个城市作为到达地!");
            return false;
        }
    }
    else if (specialType == "散冲团产品") {
        if ($("#txtDepartureAirports_txtAirport").val() == "") {
            alert("始发地不能为空，请选择至少一个城市作为始发地!");
            return false;
        }
        if ($("#txtArrivalAirports_txtAirport").val() == "") {
            alert("到达地不能为空，请选择至少一个城市作为到达地!");
            return false;
        }
    }
    else if (specialType == "免票产品" || specialType == "其他特殊产品") {
        if ($("#txtShifaAirports_txtAirports").val() == "") {
            alert("始发地不能为空，请选择至少一个城市作为始发地!");
            return false;
        }
        if ($("#txtShifaAirports_rbExclude").is(":checked") && $.trim($("#txtShifaAirports_lbSource").html()) == "") {
            alert("始发地不能全部设置为不包含，至少有一个始发地！");
            return false;
        }
        if ($("#txtZhongzhuanAirports_txtAirports").val() == "") {
            alert("到达地不能为空，请选择至少一个城市作为到达地!");
            return false;
        }
        if ($("#txtZhongzhuanAirports_rbExclude").is(":checked") && $.trim($("#txtZhongzhuanAirports_lbSource").html()) == "") {
            alert("始发地不能全部设置为不包含，至少有一个始发地！");
            return false;
        }
    }
    else if (specialType == "集团票产品" || specialType == "散冲团产品") {
        if ($("#txtDepartureAirports_txtAirport").val() == "") {
            alert("始发地不能为空，请选择至少一个城市作为始发地!");
            return false;
        }
        if ($("#txtArrivalAirports_txtAirport").val() == "") {
            alert("到达地不能为空，请选择至少一个城市作为到达地!");
            return false;
        }
    }
    if ($("#txtBunks").size() != 0 && $("#txtBunks").val() == "") {
        alert("舱位不能为空，必须输入一个舱位!");
        return false;
    }
    var pricePattern = /^[0-9]{1,10}$/;
    if ($("#txtPriceNeibu").val() == "") {
        alert("内部返佣格式错误，只能是整数，请填写");
        return false;
    }
    if ($("#txtPriceNeibu").size() != 0 && !pricePattern.test($("#txtPriceNeibu").val())) {
        alert("内部返佣格式错误，只能是整数，请填写");
        return false;
    }
    if ($("#txtPriceXiaji").val() == "") {
        alert("下级返佣格式错误，只能是整数，请填写");
        return false;
    }
    if ($("#txtPriceXiaji").size() != 0 && !pricePattern.test($("#txtPriceXiaji").val())) {
        alert("下级返佣格式错误，只能是整数，请填写");
        return false;
    }
    if ($("#txtPriceTonghang").val() == "") {
        alert("同行返佣格式错误，只能是整数，请填写");
        return false;
    }
    if ($("#txtPriceTonghang").size() != 0 && !pricePattern.test($("#txtPriceTonghang").val())) {
        alert("同行返佣格式错误，只能是整数，请填写");
        return false;
    }
    var reg2 = /^[0-9]{1,10}$/;
    if ($("#txtInternalCommission").val() != null) {
        if ($("#selPrice").val() != null) {
            if ($("#selPrice").val() == "1" && $("#selPrice").css("display") != "none") {
                if ($("#txtInternalCommission").val() != "" && !reg.test($("#txtInternalCommission").val())) {
                    alert("内部佣金格式错误，必须填写一个整数或一位小数,100以内的数!");
                    return false;
                }
                if ($("#txtInternalCommission").val() > 100) {
                    alert("内部佣金不能大于100，必须是100以内的数!");
                    return false;
                }
            } else {
                if ($("#txtInternalCommission").val() != "" && !reg2.test($("#txtInternalCommission").val())) {
                    alert("内部佣金格式错误，必须填写一个整数!");
                    return false;
                }
            }
        } else {
            if ($("#txtInternalCommission").val() != "" && !reg.test($("#txtInternalCommission").val())) {
                alert("内部佣金格式错误，必须填写一个整数或一位小数,100以内的数!");
                return false;
            }
            if ($("#txtInternalCommission").val() > 100) {
                alert("内部佣金不能大于100，必须是100以内的数!");
                return false;
            }
        }
    }
    if ($("#txtSubordinateCommission").val() != null) {
        if ($("#selPrice").val() != null) {
            if ($("#selPrice").val() == "1" && $("#selPrice").css("display") != "none") {
                if ($("#txtSubordinateCommission").val() != "" && !reg.test($("#txtSubordinateCommission").val())) {
                    alert("下级佣金格式错误，必须填写一个整数或一位小数,100以内的数!");
                    return false;
                }
                if ($("#txtSubordinateCommission").val() > 100) {
                    alert("下级佣金不能大于100，必须是100以内的数!");
                    return false;
                }
            } else {
                if ($("#txtSubordinateCommission").val() != "" && !reg2.test($("#txtSubordinateCommission").val())) {
                    alert("下级佣金格式错误，必须填写一个整数!");
                    return false;
                }
            }
        }
        else {
            if ($("#txtSubordinateCommission").val() != "" && !reg.test($("#txtSubordinateCommission").val())) {
                alert("下级佣金格式错误，必须填写一个整数或一位小数,100以内的数!");
                return false;
            }
            if ($("#txtSubordinateCommission").val() > 100) {
                alert("下级佣金不能大于100，必须是100以内的数!");
                return false;
            }
        }
    }
    if ($("#txtProfessionCommission").val() != null) {
        if ($("#selPrice").val() != null) {
            if ($("#selPrice").val() == "1" && $("#selPrice").css("display") != "none") {
                if ($("#txtProfessionCommission").val() != "" && !reg.test($("#txtProfessionCommission").val())) {
                    alert("同行佣金格式错误，必须填写一个整数或一位小数,100以内的数!");
                    return false;
                }
                if ($("#txtProfessionCommission").val() > 100) {
                    alert("同行佣金不能大于100，必须是100以内的数!");
                    return false;
                }
            } else {
                if ($("#txtProfessionCommission").val() != "" && !reg2.test($("#txtProfessionCommission").val())) {
                    alert("同行佣金格式错误，必须填写一个整数!");
                    return false;
                }
            }
        }
        else {
            if ($("#txtProfessionCommission").val() != "" && !reg.test($("#txtProfessionCommission").val())) {
                alert("下级佣金格式错误，必须填写一个整数或一位小数,100以内的数!");
                return false;
            }
            if ($("#txtProfessionCommission").val() > 100) {
                alert("下级佣金不能大于100，必须是100以内的数!");
                return false;
            }
        }
    }
    return true;
}