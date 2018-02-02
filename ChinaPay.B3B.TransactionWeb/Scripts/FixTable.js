
function FixTable(TableID, FixColumnNumber, width, height) {
    var oldtable = $("#" + TableID);
    if (oldtable.size() == 0) return;
    var autoHeight = false;
    if (height && height > 0) {
    } else {
        height = oldtable.height() + 17;
        autoHeight = true;
    }
    if ($("#" + TableID + "_tableLayout").length != 0) {
        $("#" + TableID + "_tableLayout").before($("#" + TableID));
        $("#" + TableID + "_tableLayout").empty();
    }
    else {
        $("#" + TableID).after("<div id='" + TableID + "_tableLayout' style='overflow:hidden;height:" + height + "px; width:" + width + "%;'></div>");
    }

    $(
    /*'<div id="' + TableID + '_tableColumn"></div>' + */
    '<div id="' + TableID + '_tableData"></div>').appendTo("#" + TableID + "_tableLayout");



    var tableColumnClone = oldtable.clone(true);
    tableColumnClone.attr("id", TableID + "_tableColumnClone");
    $("#" + TableID + "_tableColumn").append(tableColumnClone);
    $("#" + TableID + "_tableData").append(oldtable);
    oldtable.addClass("info");

    $("#" + TableID + "_tableLayout table").each(function () {
        $(this).css("margin", "0");
    });



    var ColumnsWidth = 0;
    var ColumnsNumber = 0;
    $("#" + TableID + "_tableColumn tr:last td:gt(" + (FixColumnNumber - 1) + ")").each(function () {
        ColumnsWidth += $(this).outerWidth(true);
        ColumnsNumber++;
    });
    ColumnsWidth += 2;
    if ($.browser.msie) {
        switch ($.browser.version) {
            case "7.0":
                if (ColumnsNumber >= 3) ColumnsWidth--;
                break;
            case "8.0":
                if (ColumnsNumber >= 2) ColumnsWidth--;
                break;
        }
    }
    $("#" + TableID + "_tableColumn").css("width", ColumnsWidth);
    var columnOffset = width - ColumnsWidth;
    var innerOffset = oldtable.width() - ColumnsWidth;

    $("#" + TableID + "_tableData").scroll(function () {
        $("#" + TableID + "_tableColumn").scrollTop($("#" + TableID + "_tableData").scrollTop());
    });

    $("#" + TableID + "_tableColumn").css({ "overflow": "hidden", "height": height - 17, "position": "relative", "z-index": "40", "background-color": "white" }).scrollLeft(innerOffset);
    $("#" + TableID + "_tableData").css({ "overflow": "auto", "width": "100%", "height": height, "position": "relative", "z-index": "35" });

    var tableOffset = $("#" + TableID + "_tableLayout").offset();
    $("#" + TableID + "_tableData").offset(tableOffset);
/*
    if ($("#" + TableID + "_tableData").width() >= $("#" + TableID + "_tableData table").width()) {
        $("#" + TableID + "_tableData").css("width", $("#" + TableID + "_tableData table").width() + 17);
    }
    */
    if ($("#" + TableID + "_tableData").height() > $("#" + TableID + "_tableData table").height()) {
        $("#" + TableID + "_tableColumn").css("height", $("#" + TableID + "_tableData table").height());
        $("#" + TableID + "_tableData").css("height", $("#" + TableID + "_tableData table").height() + 18);
        $("#" + TableID + "_tableColumn").offset({ top: tableOffset.top, left: tableOffset.left + columnOffset });
    } else {
        $("#" + TableID + "_tableColumn").offset({ top: tableOffset.top, left: tableOffset.left + columnOffset - 17 });

    }
}