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
    if ($("#txtDepartureAirports_txtAirports").val() == "") {
        alert("始发地不能为空，请选择至少一个城市作为始发地!");
        return false;
    }
    if ($("#txtDepartureAirports_rbExclude").is(":checked") && $.trim($("#txtDepartureAirports_lbSource").html()) == "") {
        alert("始发地不能全部设置为不允许，至少有一个始发地！");
        return false;
    }
    if ($("#txtArrivalAirports_txtAirports").val() == "") {
        if ($("#txtZhongzhuanAirports_txtAirports").val() == null) {
            alert("到达地不能为空，请选择至少一个城市作为到达地!");
        } else {
            alert("中转地不能为空，请选择至少一个城市作为到达地!");
        }
        return false;
    }
    if ($("#txtArrivalAirports_rbExclude").is(":checked") && $.trim($("#txtArrivalAirports_lbSource").html()) == "") {
        if ($("#txtZhongzhuanAirports_txtAirports").val() == null) {
            alert("到达地不能全部设置为不包含，至少有一个到达地！");
        } else {
            alert("中转地不能全部设置为不包含，至少有一个到达地！");
        }
        return false;
    }
    if ($("#txtZhongzhuanAirports_txtAirports").val() != null) {
        if ($("#txtZhongzhuanAirports_txtAirports").val() == "") {
            alert("中转地不能为空，请选择至少一个城市作为到达地!");
            return false;
        }
        if ($("#txtZhongzhuanAirports_rbExclude").is(":checked") && $.trim($("#txtZhongzhuanAirports_lbSource").html()) == "") {
            alert("中转地不能全部设置为不包含，至少有一个到达地！");
            return false;
        }
    }
    return true;
}