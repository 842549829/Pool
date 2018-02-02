$(function () {
    $("#ulProduct>li>a").click(function () {
        $("#ulProduct>li>a").each(function () { $(this).removeClass("curr"); });
        $(this).addClass("curr");
        $("#hfdProduct").val($(this).attr("accesskey"));
        $("#btnSerach").click();
    });
    $("#ulOrderStatus>li>a").click(function () {
        $("#ulOrderStatus>li>a").each(function () { $(this).removeClass("curr"); });
        $(this).addClass("curr");
        $("#hfdOrderStatus").val($(this).attr("accesskey"));
        $("#btnSerach").click();
    });
    $("#ulProduct>li>a").each(function () { $(this).removeClass("curr"); if ($(this).attr("accesskey") == $("#hfdProduct").val()) { $(this).addClass("curr"); } });
    $("#ulOrderStatus>li>a").each(function () { $(this).removeClass("curr"); if ($(this).attr("accesskey") == $("#hfdOrderStatus").val()) { $(this).addClass("curr"); } });
});