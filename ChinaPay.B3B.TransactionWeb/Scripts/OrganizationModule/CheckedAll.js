function choice(id) {
    $("input[type='radio'][name='choice']").live("click",function () {
        var tip = $("input:checkbox", "#" + id);
        for (var i = 0; i < tip.length; i++) {
            if ($(this).val() == "0")
                tip.eq(i).attr("checked", true);
            else
                tip.eq(i).attr("checked", !tip.eq(i).attr("checked"));
        }
    });
}