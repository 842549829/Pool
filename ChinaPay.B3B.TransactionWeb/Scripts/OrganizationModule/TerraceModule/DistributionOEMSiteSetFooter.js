$(function () {
    var count = 0;
    var flag = 5;
    var remove;
    $(".add").live("click", function () {
        $line = $("<div><input type='text' placeholder='链接名称' value='链接名称' class='text text-s'  />\n<input type='text' placeholder='链接地址' value='链接地址'  class='text' />\n<a class='add'>+</a></div>");
        count = $(this).parent().parent().find("div").length;
        if (count < flag) {
            $(this).parent().parent().append($line).end().end().html("-").attr("class", "reduce");
        }
        //        alert();
        count = $(this).parent().parent().find("div").length;
        if (count == flag) {
            $(this).parent().parent().find(".add").css("visibility", "hidden");
        };
    });
    $(".reduce").live("click", function () {
        if ($(this).next().hasClass("muted")) {
            remove = $(this).next().remove();
            $(this).parent().next().append(remove).end().remove();
        } else {
            $(this).parent().remove();
        };
        count = $(this).parent().parent().find("div").length;
        if (count != flag) {
            //            alert($(this).parent().parent().parent().parent().find(".add").css("visibility"));
            $(".add").css("visibility", "visible");
        }
    });
    $(".link_text input[type='text']").live("focus", function () {
        if ($(this).val() == $(this).attr("placeholder")) {
            $(this).val("");
        }
    }).live("blur", function () {
        if ($(this).val() == "") {
            $(this).val($(this).attr("placeholder"));
        }
    });
});