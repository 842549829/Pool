$(function () {
    //儿童政策
    $("#chkChildren").attr("checked") == "checked" ? $("#lblchildren").show() : $("#lblchildren").hide();
    $("#btnSave").click(function () {
        if ($("#txtBeginTime").val().length < 1 || $("#txtEndTime").val().length < 1) {
            alert("使用期限不能为空");
            return false;
        }
        var reg = /^\d{1,4}$/;
        if (!reg.test($("#txtLockTicket").val())) {
            alert("锁定政策累积退废票格式错误");
            $("#txtLockTicket").focus().select();
            return false;
        }
        if (!reg.test($("#txtVoluntaryRefundsLimit").val())) {
            alert("自愿退票限时格式错误");
            $("#txtVoluntaryRefundsLimit").focus().select();
            return false;
        }
        if (!reg.test($("#txtAllRefundsLimit").val())) {
            alert("全退限时格式错误");
            $("#txtAllRefundsLimit").focus().select();
            return false;
        }
        var regex = /^\d{1,3}$/;
        if (!regex.test($("#txtPeerTradingRate").val())) {
            $("#txtPeerTradingRate").focus().select();
            alert("同行交易费率格式错误");
            return false;
        }
        if (!regex.test($("#txtLowerRates").val())) {
            alert("下级交易费率格式错误");
            $("#txtLowerRates").focus().select();
            return false;
        }
        if ($("#chkSingleness").is(":checked") && !regex.test($("#txtSingleness").val())) {
            alert("单程控位产品费率格式错误");
            $("#txtSingleness").focus().select();
            return false;
        }
        if ($("#chkDisperse").is(":checked") && !regex.test($("#txtDisperse").val())) {
            alert("散冲团产品费率格式错误");
            $("#txtDisperse").focus().select();
            return false;
        }
        if ($("#chkCostFree").is(":checked") && !regex.test($("#txtCostFree").val())) {
            alert("免票产品费率格式错误");
            $("#txtCostFree").focus().select();
            return false;
        }
        if ($("#chkBloc").is(":checked") && !regex.test($("#txtBloc").val())) {
            alert("集团票产品费率格式错误");
            $("#txtBloc").focus().select();
            return false;
        }
        if ($("#chkBusiness").is(":checked") && !regex.test($("#txtBusiness").val())) {
            alert("商旅卡产品费率格式错误");
            $("#txtBusiness").focus().select();
            return false;
        }
        if ($("#chkOtherSpecial").is(":checked") && !regex.test($("#txtOtherSpecialRate").val())) {
            alert("其他特殊产品费率格式错误");
            $("#txtOtherSpecialRate").focus().select();
            return false;
        }
        return true;
    });
});