$(function () {
    $(".postion_tr").mouseover(function () {
        if ($(this).find(".DepartureTip") != null) {
            $(this).find(".DepartureTip").show();
        }
    });
    $(".postion_tr").mouseleave(function () {
        if ($(this).find(".DepartureTip") != null) {
            $(this).find(".DepartureTip").hide();
        }
    });
})