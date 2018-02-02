function publicVaild() {
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
    var bargianType = $.trim($("#titlePolicy").html());
    if (bargianType == "单程") {
        if ($("#txtDepartureAirports_txtAirports").val() != null) {
            if ($("#txtDepartureAirports_txtAirports").val() == "") {
                alert("始发地不能为空，请选择至少一个城市作为始发地!");
                return false;
            }
            if ($("#txtDepartureAirports_rbExclude").is(":checked") && $.trim($("#txtDepartureAirports_lbSource").html()) == "") {
                alert("始发地不能全部设置为不包含，至少有一个始发地！");
                return false;
            }
        }
        if ($("#txtArrivalAirports_txtAirports").val() != null) {
            if ($("#txtArrivalAirports_txtAirports").val() == "") {
                alert("到达地不能为空，请选择至少一个城市作为到达地!");
                return false;
            }
            if ($("#txtArrivalAirports_rbExclude").is(":checked") && $.trim($("#txtArrivalAirports_lbSource").html()) == "") {
                alert("到达地不能全部设置为不包含，至少有一个到达地！");
                return false;
            }
        }
    }
    else if (bargianType == "往返") {
        var obj = $(".diaohuan").eq(0);
        var obj1 = $(".diaohuan").eq(1);
        if (obj.find("#txtShifaAirports_txtAirports").val() != null) {
            if (obj.find("#txtShifaAirports_txtAirports").val() == "") {
                alert("出发地不能为空，请选择至少一个城市作为出发地!");
                return false;
            }
            if (obj1.find("#txtZhongzhuanAirports_txtAirports").val() == "") {
                alert("到达地不能为空，请选择至少一个城市作为到达地!");
                return false;
            }
            if (obj1.find("#txtZhongzhuanAirports_rbExclude").is(":checked") && $.trim(obj1.find("#txtZhongzhuanAirports_lbSource").html()) == "") {
                alert("到达地不能全部设置为不包含，至少有一个到达地！");
                return false;
            }
        } else {
            if (obj1.find("#txtShifaAirports_txtAirports").val() == "") {
                alert("到达地不能为空，请选择至少一个城市作为到达地!");
                return false;
            }
            if (obj.find("#txtZhongzhuanAirports_txtAirports").val() == "") {
                alert("出发地不能为空，请选择至少一个城市作为出发地!");
                return false;
            }
            if (obj.find("#txtZhongzhuanAirports_rbExclude").is(":checked") && $.trim(obj.find("#txtZhongzhuanAirports_lbSource").html()) == "") {
                alert("出发地不能为空，请选择至少一个城市作为出发地!");
                return false;
            }
        }
    }
    else if (bargianType == "中转联程") {
        if ($("#txtDepartureAirports_txtAirports").val() != null) {
            if ($("#txtDepartureAirports_txtAirports").val() == "") {
                alert("出发地不能为空，请选择至少一个城市作为出发地!");
                return false;
            }
            if ($("#txtDepartureAirports_rbExclude").is(":checked") && $.trim($("#txtDepartureAirports_lbSource").html()) == "") {
                alert("出发地不能为空，请选择至少一个城市作为出发地!");
                return false;
            }
        }
        if ($("#txtArrivalAirports_txtAirports").val() != null) {
            if ($("#txtArrivalAirports_txtAirports").val() == "") {
                alert("中转地不能为空，请选择至少一个城市作为到达地!");
                return false;
            }
            if ($("#txtArrivalAirports_rbExclude").is(":checked") && $.trim($("#txtArrivalAirports_lbSource").html()) == "") {
                alert("中转地不能全部设置为不允许，至少有一个到达地！");
                return false;
            }
        }
        if ($("#txtZhongzhuanAirports_txtAirports").val() != null) {
            if ($("#txtZhongzhuanAirports_txtAirports").val() == "") {
                alert("目的地不能为空，请选择至少一个城市作为目的地!");
                return false;
            }
            if ($("#txtZhongzhuanAirports_rbExclude").is(":checked") && $.trim($("#txtZhongzhuanAirports_lbSource").html()) == "") {
                alert("目的地不能为空，请选择至少一个城市作为目的地!");
                return false;
            }
        }
    }
    //    if ($("#txtRemark").val() == "") {
    //        alert("备注不能为空!");
    //        return false;
    //    }
    var reg = /^[0-9]{1,10}?$/;
    var selvalue = $("#selPrice").val();
    if (selvalue == "0" && $("#txtPrice").val() == "") {
        alert("价格不能为空!");
        return false;
    }
    if (selvalue == "0" && !reg.test($("#txtPrice").val())) {
        alert("价格必须为整数!");
        return false;
    }
    if (selvalue == "1" && $("#txtDiscount").val() == "") {
        alert("折扣不能为空!");
        return false;
    }
    var reg1 = /^[0-9]{1,3}?$/;
    if (selvalue == "1" && !reg.test($("#txtDiscount").val())) {
        alert("折扣必须为两位以内的整数!");
        return false;
    } if (selvalue == "1" && parseInt($("#txtDiscount").val()) <= 0) {
        alert("折扣必须为两位以内的整数!");
        return false;
    }
    if ($("#txtRemark").val(), length > 200) {
        alert("备注信息不能超过200个字!");
        $("#txtRemark").val($("#txtRemark").val().substring(0, 200));
        return false;
    }
    if ($("#txtChuxing").size() != 0 && $("#txtChuxing").val() != "" && !reg.test($("#txtChuxing").val())) {
        alert("出行天数必须为大于零的整数!");
        return false;
    }
    return true;
}