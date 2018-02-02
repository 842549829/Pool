$(function () {
    var targetUrl = "/OrganizationHandlers/CompanyInfoMaintain.ashx/QueryDrawditions";
    sendPostRequest(targetUrl, null, function (result) {
        $("#divZidingyiTb table").remove();
        var strT = "<table id='1'><tr><th>内容</th><th>操作</th></tr>";
        var strT1 = "<table id='2'><tr><th>内容</th><th>操作</th></tr>";
        var val = 0, val1 = 0;

        $.each(eval(result), function (i, item) {
            if (item.Type == 0) {
                strT += "<tr><td>" + item.Context + "</td><td><input type='button' value='选择' class='class2 btn btnChooice' conType='1' con='" + item.Context + "'></td></tr>";
                val = 1;
            } else {
                strT1 += "<tr><td>" + item.Context + "</td><td><input type='button' value='选择' class='class2 btn btnChooice' conType='2' con='" + item.Context + "'></td></tr>";
                val1 = 1;
            }
        });
        strT += "</table>";
        strT1 += "</table>";
        if (val == 0) {
            $("#divZidingyiTb").append("<table id='1'><tr><th>您还没有设置过自定义出票条件，请在 公司管理----公司工作信息设置----出票/备注 设置</th></tr>");
        } else {
            $("#divZidingyiTb").append(strT);
        }
        if (val1 == 0) {
            $("#divZidingyiTb").append("<table id='2'><tr><th>您还没有设置过自定义政策备注，请在 公司管理----公司工作信息设置----出票/备注 设置</th></tr>");
        } else {
            $("#divZidingyiTb").append(strT1);
        }

        //        alert(strT);
    }, function () {
    });
    $(".btnChooice").live("click", function () {
        if ($(this).attr("conType") == "1") {
            $("#txtDrawerCondition").val($(this).attr("con"));
        } else {
            $("#txtRemark").val($(this).attr("con"));
        }
        $(".close").click();
    });
});
function zidingyi(o) {
    if (o == 1) {
        $("#divZidingyiTb table[id='1']").show();
        $("#divZidingyiTb table[id='2']").hide();
    } else {
        $("#divZidingyiTb table[id='1']").hide();
        $("#divZidingyiTb table[id='2']").show();
    }
    if ($("#divZidingyiTb table").size() != 0) {
        $("#divOpcial").click();
    }
}
//function selDrawDondition(obj) {
//    if ($(obj).val() != "") {
//        $("#txtDrawerCondition").val($(obj).val());
//    }
//}
//function selPRemark(obj) {
//    if ($(obj).val() != "") {
//        $("#txtRemark").val($(obj).val());
//    }
//}