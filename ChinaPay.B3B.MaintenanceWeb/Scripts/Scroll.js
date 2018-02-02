$(function () {
    var containerWidth = $("#data-list").width();
    var horzionMaxWidth = Math.floor(containerWidth * 0.9);
    var horzionMinWidth = Math.floor(containerWidth * 0.1);
    $("#data-list").mouseover(function (e) {
        var widthh = e.originalEvent.x - $(this).offset().left || e.originalEvent.layerX - $(this).offset().left || 0;
        var tableWidth = $("#data-list table").width();
        var scrollWidht = parseFloat(tableWidth) - parseFloat(containerWidth);
        if (scrollWidht <= 0) {
            return;
        }
        if (parseFloat(widthh) > parseFloat(horzionMaxWidth)) {
            $(this).dequeue();
            $(this).animate({ scrollLeft: scrollWidht }, 3000);
        }
        if (parseFloat(widthh) < parseFloat(horzionMinWidth)) {
            $(this).dequeue();
            $(this).animate({ scrollLeft: '0' }, 3000);
        }
    }).mouseout(function () {
        $(this).stop();
    }).click(function () {
        $(this).stop();
    });
})