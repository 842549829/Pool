
$(function () {
    $(".del_click").click(function () {
        //alert($(".grv_policy tr td").eq(parseInt($(this).parent().index()) - 2).html()); 
        if (($(".info tbody tr").eq($(this).parent().parent().index()).find("td").eq(parseInt($(this).parent().index()) - 2).html()) == "已审") {
            alert("本条政策已经审核通过，将不能删除。\n         执行被取消。");
            return false;
        }
        return confirm("是否删除?");
    });
    $("#btnDel").click(function () {
        if ($("#hidIds").val() == "") {
            alert("未选中任何行，执行被取消");
            return false;
        }
        var count_del = 0;
        var count_notdel = 0;
        var str = "";
        var msg_str = "";
        $.each($("#hidIds").val().split(','), function (i, item) {
            if ($(".info tbody tr td input[type='checkbox']:checked").eq(i).parent().parent().find("td").eq(parseInt($(".info tbody tr").eq(1).find("td").length) - 3).html() == "已审") {
                count_notdel = parseInt(count_notdel) + 1;
            } else {
                if (str != "") {
                    str += ",";
                }
                str += item;
                count_del = parseInt(count_del) + 1;
            }
        });
        if ($("#hidIsAll").val() == "0") {
            if (str == "") {
                alert("选中 [ " + count_notdel + " ] 条已审核政策不能删除.\n         执行被取消.");
                return false;
            }
        }
        $("#hidIds").val(str);
        if ($("#hidIsAll").val() == "1") {
            $("#hidIds").val("1");
            if (confirm("是否删除 [ " + $("#pager_lblTotalCount").html() + " ] 条的政策？\n如果包含了已审核的政策将不能删除成功！")) {
                return true;
            } else {
                return false;
            }
        } else {
            if (count_notdel != 0) {
                msg_str = "本次操作中有 [ " + count_notdel + " ] 条政策已审核，不能被删除\n       是否删除 [ " + count_del + " ] 条未审核的政策？";
            } else {
                msg_str = "是否删除 [ " + count_del + " ] 条未审核的政策？";
            }
            if (confirm(msg_str)) {
                return true;
            } else {
                var ids = "";
                for (var k = 0; k < $(".info tbody tr td input[type='checkbox']:checked").length; k++) {
                    if ($(".info tbody tr td input[type='checkbox']:checked").eq(k).val() != "") {
                        if (ids != "") {
                            ids += ",";
                        }
                        ids += $(".info tbody tr td input[type='checkbox']:checked").eq(k).val();
                    }
                }
                $("#hidIds").val(ids);
                return false;
            }
        }
    });
    $("#btnAudited").click(function () {
        if ($("#hidIds").val() == "") {
            alert("未选中任何行，执行被取消");
            return false;
        }
    });
    $("#btnUnAudited").click(function () {
        if ($("#hidIds").val() == "") {
            alert("未选中任何行，执行被取消");
            return false;
        }
    });
});