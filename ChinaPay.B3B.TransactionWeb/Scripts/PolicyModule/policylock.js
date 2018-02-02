$(function () {
    $("#lock").click(function () {
        if ($("#hidIds").val() == "") {
            alert("没有选中任何行，执行被取消");
            return;
        }
        $("#divPolicy h2").html("锁定政策");
        $("#divPolicy .title").html("请输入锁定原因");
        $("#txtlockReason").show();
        $("#btnSavelock").show();
        $("#btnSaveunlock").hide();
        $("#txtunlockReason").hide();
        $("#divOpcial").click();
        $("#divPolicy").css("top", "100px");
    });
    $("#unlock").click(function () {
        if ($("#hidIds").val() == "") {
            alert("没有选中任何行，执行被取消");
            return;
        }
        $("#divPolicy h2").html("解锁政策");
        $("#divPolicy .title").html("请输入解锁原因");
        $("#txtlockReason").hide();
        $("#btnSavelock").hide();
        $("#btnSaveunlock").show();
        $("#txtunlockReason").show();
        $("#divOpcial").click();
        $("#divPolicy").css("top", "100px");
    });

    $("#btnSavelock").click(function () {
        if ($("#txtlockReason").val() == "") {
            alert("请输入锁定的原因!");
            return false;
        }
        return true;
    });
    $("#btnSaveunlock").click(function () {
        if ($("#txtunlockReason").val() == "") {
            alert("请输入解锁的原因!");
            return false;
        }
        return true;
    });
    $("#btnlock").click(function () {
        if ($("#txtlockReason").val() == "") {
            alert("请输入锁定的原因!");
            return false;
        }
        return true;
    });
    $("#btnunlock").click(function () {
        if ($("#txtunlockReason").val() == "") {
            alert("请输入解锁的原因!");
            return false;
        }
        return true;
    });
});
function unlockpolicy(id) {
    $("#divPolicy h2").html("解锁政策");
    $("#divPolicy .title").html("请输入解锁原因");
    $("#txtlockReason").hide();
    $("#btnSavelock").hide();
    $("#btnSaveunlock").show();
    $("#txtunlockReason").show();
    $("#divOpcial").click();
    $("#hidIds").val(id);
    $("#hidIsAll").val("0");
}
function lockpolicy(id) {
    $("#divPolicy h2").html("锁定政策");
    $("#divPolicy .title").html("请输入锁定原因");
    $("#txtlockReason").show();
    $("#btnSavelock").show();
    $("#btnSaveunlock").hide();
    $("#txtunlockReason").hide();
    $("#divOpcial").click();
    $("#hidIds").val(id);
    $("#hidIsAll").val("0");
}

