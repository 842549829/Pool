/// <reference path="/Scripts/jquery-1.7.2-vsdoc.js"/>
var splictor = "||"; var connector = "**";
function ResetSearchOption(selector, needReSearch, dateHolderSelector) {
    selector = selector || ".condition";
    dateHolderSelector = dateHolderSelector || "#hdDefaultDate";
    $(selector).find("input[type='text']").not(".text-s").val("");
    $(selector).find("input[type='hidden'][class='company']").val("");
    $(selector).find(".dropdown").each(function (index, item) {
        $(item).find("ul li:first").trigger("click");
    });
    $(selector).find("select").each(function (index, item) {
        $(item).find("option:first").attr("selected", "selected");
    });
    $(selector).find(".radio_reset").each(function (index, item) {
        $(item).find("input[type=radio]:first").attr("checked", "checked");
    });
    var dataHolder = $(dateHolderSelector);
    if (dataHolder.size() > 0 && dataHolder.val() != "") {
        var elementItem = dataHolder.val().split(splictor);
        for (var itemIndex in elementItem) {
            if (typeof (elementItem[itemIndex]) == "function")
                continue;
            var itemData = elementItem[itemIndex].split(connector);
            $("#" + itemData[0]).val(itemData[1]);
        }
    }
    if (needReSearch) $("#btnQuery").trigger("click");
}

function copyToClipboard(data) {
    window.clipboardData.setData('text', data);
    alert("复制成功！");
}

function SaveDefaultData(containerSelector, dataSelector) {
    containerSelector = containerSelector || "#hdDefaultDate";
    dataSelector = dataSelector || ".text-s";
    var dataHolder = $(containerSelector);
    if (dataHolder.size() > 0 && dataHolder.val() == "") {
        var datas = $.makeArray($(dataSelector).map(function (index, item) {
            var ele = $(item);
            return ele.attr("id") + connector + ele.val();
        }));
        dataHolder.val(datas.join(splictor));
    }
}

var conditions = {};
var pageName = "";
function SaveSearchCondition(pageName)
{
    $(".condition input[type='text']").each(function ()
    {
        var that = $(this);
        conditions[that.attr("id")] = "1|" + that.val();
    });
    $(".condition select").each(function ()
    {
        var that = $(this);
        conditions[that.attr("id")] = "2|" + that.find(":selected").text();
    });
    $(".condition input[type='hidden']").each(function () {
        var that = $(this);
        conditions[that.attr("id")] = "4|" + that.val();
    });
    setConditionCookie(conditions, 2, pageName);
}

function SaveSearchConditionWhenPagging(currentPage)
{
    conditions.yema = "3|" + currentPage;
    SaveSearchCondition(pageName);

}
