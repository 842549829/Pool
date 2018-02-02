$(function () {
    var limit = $("#hidShouyishezhi").val();
    //修改时的值
    var limitUpdate = "";
    var limitUpdateHtml = "";
    $(".tag-nav li").click(function () {
        $(".tag-nav li").removeClass("cur");
        $(this).addClass("cur");
    });
    $(".xianshi p").live("dblclick", function () {
        if ($.trim(limitUpdateHtml) != "") {
            if (limit != "") {
                limit += "^";
            }
            limit += $.trim(limitUpdate);
            if (limitUpdate.split(',')[0] == "1") {
                $("#bengongsi").append(limitUpdateHtml);
            } else {
                $("#tonghang").append(limitUpdateHtml);
            }
        }
        limitUpdate = $(this).attr("curr");
        limitUpdateHtml = "<p  curr=" + limitUpdate + ">" + $(this).html() + "</p>";
        var templimit = "";
        var currlimit = limit.split('^');
        for (var l = 0; l < currlimit.length; l++) {
            var tt = $.trim(currlimit[l]);
            if (limitUpdate != tt) {
                if (templimit != "") {
                    templimit += "^";
                }
                templimit += tt;
            }
        }
        limit = templimit;

        //移除当前这一项
        $(this).remove();
        $("#rangeItems").html("<tr><td class='rangeSerial'>返点在</td><td><input style='width: 25px' class='text rangeStart' disabled='disabled' value='0' />%(<span>含</span>)</td><td>至</td><td><input style='width: 25px' class='text rangeEnd' value='100' />%(含)</td><td>值</td><td><input style='width: 25px' class='text rangeValue' value='0' />%</td><td><a class='addRange add'>+</a></td><td><a class='delRange reduce' style='display: none;'>-</a></td></tr>");
        var curr = limitUpdate.split(',');
        $(".tag-nav li").removeClass("cur");
        if (curr[0] == "0") {
            $(".tag-nav li").eq(1).addClass("cur");
        } else {
            $(".tag-nav li").eq(0).addClass("cur");
        }
        $("#hangkonggongsi input[type='checkbox']").removeAttr("checked");
        $("input[type='radio'][name='fandiankoudian']").removeAttr("checked");
        var airtemp = curr[1].split('/');
        for (var h = 0; h < airtemp.length; h++) {
            for (var g = 0; g < $("#hangkonggongsi input[type='checkbox']").length; g++) {
                var chktemp = $("#hangkonggongsi input[type='checkbox']")[g];
                if ($.trim(chktemp.value) == $.trim(airtemp[h])) {
                    chktemp.checked = true;
                    break;
                }
            }
        }
        if (curr[2] == "1") {
            $("#radqujian").attr("checked", "checked");
            $("#hidRanges").val(curr[3]);
            parseRanges();
            $(".qujiankoudian").show();
            $(".tongyifandian").hide();
        } else {
            $("#radfandian").attr("checked", "checked");
            $("#tongyifandian").val(curr[3]);
            $(".qujiankoudian").hide();
            $(".tongyifandian").show();
        }
        $("#price").val(curr[4]);
    });
    $("input[type='radio'][name='gongsixuanzhe']").click(function () {
        if ($(this).val() == "0") {
            $("#hangkonggongsi input[type='checkbox']").attr("checked", "checked");
        } else {
            for (var k = 0; k < $("#hangkonggongsi input[type='checkbox']").length; k++) {
                $("#hangkonggongsi input[type='checkbox']")[k].checked = !$("#hangkonggongsi input[type='checkbox']")[k].checked;
            }
        }
    });
    $("input[type='radio'][name='fandiankoudian']").click(function () {
        if ($(this).val() == "1") {
            $(".qujiankoudian").show();
            $(".tongyifandian").hide();
        } else {
            $(".qujiankoudian").hide();
            $(".tongyifandian").show();
        }
    });
    $(".btnAdd").click(function () {

        if (vail()) {
            saveRanges();
            limitUpdate = "";
            limitUpdateHtml = "";
            var isowner = $(".tag-nav .cur").attr("val");
            var chk = $("#hangkonggongsi :checked");
            var airlines = "";
            for (var i = 0; i < chk.length; i++) {
                if (airlines != "") {
                    airlines += "/";
                }
                airlines += chk[i].value;
            }
            var kouType = $.trim($("input[type='radio'][name='fandiankoudian']:checked").val());
            var ranges = kouType == 1 ? $("#hidRanges").val() : $("#tongyifandian").val();
            var pirce = $("#price").val();
            var currValue = isowner + "," + airlines + "," + kouType + "," + ranges + "," + pirce;
            var currlimit = limit.split('^');
            var v = false;
            for (var l = 0; l < currlimit.length; l++) {
                var tt = $.trim(currlimit[l]);
                if (currValue == tt) {
                    v = true;
                    break;
                }
            }
            if (v) {
                alert("当前收益信息设置存在相同的设置信息，请勿重复添加。");
                $("#hidRanges").val("");
                return;
            }
            if (limit != "") {
                limit += "^";
            }
            limit += $.trim(currValue);
            var str1 = "";
            if (kouType == 1) {
                var t = ranges.split(';');
                for (var j = 0; j < t.length; j++) {
                    var temp = t[j].split('|');
                    str1 += "[" + airlines + "]" + " 区域：" + "[" + temp[0] + "," + temp[1] + "];扣点:" + temp[2] + "% <br />";
                }
            } else {
                str1 += "[" + airlines + "]" + " 统一返点：" + ranges + "% <br />";
            }
            var str = " <p curr=" + currValue + ">" + str1 + " 每张票加价：" + pirce + " 元 <a href='#' class='remove' curr=" + currValue + ">删除</a> </p>";
            if (isowner == "1") {
                $("#bengongsi").append(str);
            } else {
                $("#tonghang").append(str);
            }
            $("#hidRanges").val("");
            $("#hidShouyishezhi").val(limit); clearInput();
        }
    });
    $(".remove").live("click", function () {
        var templimit = "";
        var currv = $.trim($(this).attr("curr"));
        var currlimit = limit.split('^');
        for (var l = 0; l < currlimit.length; l++) {
            var tt = $.trim(currlimit[l]);
            if (currv != tt) {
                if (templimit != "") {
                    templimit += "^";
                }
                templimit += tt;
            }
        }
        limit = templimit;
        $("#hidShouyishezhi").val(limit);
        $(this).parent().remove();
        //            alert(templimit);
        //            alert(limit); 
    });

});
function clearInput() {
    $("#hangkonggongsi input[type='checkbox']").removeAttr("checked");
    $("input[type='radio'][name='gongsixuanzhe']").removeAttr("checked");
    $("#rangeItems").html("<tr><td class='rangeSerial'>返点在</td><td><input style='width: 25px' class='text rangeStart' disabled='disabled' value='0' />%(<span>含</span>)</td><td>至</td><td><input style='width: 25px' class='text rangeEnd' value='100' />%(含)</td><td>值</td><td><input style='width: 25px' class='text rangeValue' value='0' />%</td><td><a class='addRange add'>+</a></td><td><a class='delRange reduce' style='display: none;'>-</a></td></tr>");
    $("#tongyifandian,#price").val("");
}
function vail() {
    if ($("#hangkonggongsi :checked").length == 0) {
        alert("请选择限制航空公司！");
        return false;
    }
    var reg = /^[0-9]{1,10}(\.[0-9])?$/;
    if ($("#radfandian").is(":checked") && $("#tongyifandian").val() == "") {
        alert("统一返点不能为空！");
        return false;
    }
    if ($("#radfandian").is(":checked") && !reg.test($("#tongyifandian").val())) {
        alert("统一返点只能为整数或一位小数！");
        return false;
    }
    if ($("#radfandian").is(":checked")) {
        $("#tongyifandian").val(parseFloat($("#tongyifandian").val()));
    }
    if ($("#price").val() == "") {
        alert("每张票加价不能为空！");
        return false;
    }
    reg = /^[0-9]{1,10}?$/;
    if (!reg.test($("#price").val())) {
        alert("每张票加价只能为整数！");
        return false;
    }
    $("#price").val(parseFloat($("#price").val()));
    if ($("#remark").val().length>=200) {
        alert("备注不能超过200个字符！");
        return false;
    }
    return true;
}
