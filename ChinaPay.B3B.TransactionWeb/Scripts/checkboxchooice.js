
//得到所有选中的编号
var ids = "";
$(function () {
    $(".chkOne").click(function () {
        var chk_length = $(".info .chkOne:checked").length;
        var chk_all = $(".info .chkAll");
        ids = "";
        var allChecked = $(".info td input[type='checkbox']").not(":checked").size() > 0;
        if (!allChecked) {
            chk_all.attr("checked", "checked");
        } else {
            chk_all.removeAttr("checked");
        }
        ids = $.makeArray($(".info td :checked").map(function () { return $(this).val() })).join(",");
        $("#hidIds").val(ids);
        $("#hidIsAll").val("0");
    });
    $(".chkAll").click(function () {
        //        var chk_obj = $(".info tr input[type='checkbox']");
        //        ids = "";
        //        if ($(".info .chkAll").is(":checked")) {
        //            chk_obj.attr("checked", "checked");
        //        } else {
        //            chk_obj.removeAttr("checked");
        //        }
        //        ids = $.makeArray($(".info td :checked").map(function () { return $(this).val() })).join(",");
        //        $("#hidIds").val(ids);
        if ($(this).is(":checked")) {
            $("#divChooice").click();
            $("#divIsAll").show();
            $("#divIsAll").css("top", "100px");
        } else {
            $(".info tr input[type='checkbox']").removeAttr("checked");
            $("#hidIds").val("");
            $("#hidIsAll").val("");
        }
    });
    $("#btnCancel").click(function () {
        $("#hidIds").val("");
        $(".info tr input[type='checkbox']").removeAttr("checked");
        $("#hidIsAll").val("");
        $("#divIsAll").hide();
        $("#divChooice").hide();
    });
    $("#btnAll").click(function () {
        $("#btnCancel").click();
        $("#hidIsAll").val("1");
        $(".info tr input[type='checkbox']").attr("checked", "checked");
        $("#hidIds").val("1");
        //        var chk_length = $(".info .chkOne:checked").length;
        //        var chk_all = $(".info .chkAll");
        //        ids = "";
        //        var allChecked = $(".info td input[type='checkbox']").not(":checked").size() > 0;
        //        if (!allChecked) {
        //            chk_all.attr("checked", "checked");
        //        } else {
        //            chk_all.removeAttr("checked");
        //        }
        //        ids = $.makeArray($(".info td :checked").map(function () { return $(this).val() })).join(",");
        //        $("#hidIds").val(ids);
    });
    $("#btnCurrt").click(function () {
        $("#btnCancel").click();
        $("#hidIsAll").val("0");
        $(".info tr input[type='checkbox']").attr("checked", "checked");
        //        var chk_length = $(".info .chkOne:checked").length;
        //        var chk_all = $(".info .chkAll");
        //        ids = "";
        //        var allChecked = $(".info td input[type='checkbox']").not(":checked").size() > 0;
        //        if (!allChecked) {
        //            chk_all.attr("checked", "checked");
        //        } else {
        //            chk_all.removeAttr("checked");
        //        }
        ids = $.makeArray($(".info td :checked").map(function () { return $(this).val() })).join(",");
        $("#hidIds").val(ids);
    });
    if ($("#hidIsAll").val() == "1") {
        $(".info tr input[type='checkbox']").attr("checked", "checked");
    }
});