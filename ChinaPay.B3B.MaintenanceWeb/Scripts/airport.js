function addAirport(sourceObj, selectedObj, showObj) {
    var items = getOperateItems(sourceObj);
    moveItems(selectedObj, items, true);
    showCodes(selectedObj, showObj);
}
function removeAirport(sourceObj, selectedObj, showObj) {
    var items = getOperateItems(selectedObj);
    moveItems(sourceObj, items, true);
    showCodes(selectedObj, showObj);
}
function addAllAirports(sourceObj, selectedObj, showObj) {
    var items = getAllItems(sourceObj);
    moveItems(selectedObj, items, true);
    showCodes(selectedObj, showObj);
}
function removeAllAirports(sourceObj, selectedObj, showObj) {
    var items = getAllItems(selectedObj);
    moveItems(sourceObj, items, true);
    showCodes(selectedObj, showObj);
}
function matchAirports(sourceObj, selectedObj, showObj) {
    var codePattern = $.trim(showObj.val()).toUpperCase();
    if (codePattern == '') {
        removeAllAirports(sourceObj, selectedObj, showObj);
    } else if (codePattern == '*') {
        addAllAirports(sourceObj, selectedObj, showObj);
    } else {
        moveItems(sourceObj, getAllItems(selectedObj), false);
        var matchedItems = getMatchedItems(sourceObj, codePattern);
        moveItems(selectedObj, matchedItems);
        showCodes(selectedObj, showObj);
    }
}
function filterAirport(e) {
    // 37、39左右键
    // 191   分隔符
    // 8 46 backspce/delete
    // 65-90 a-z
    // 9 Tab
    // 20 CapsLock
    // 56 *号键
    if (!e) e = window.event;
    if ((65 <= e.keyCode && e.keyCode <= 90)
        || e.keyCode == 8 || e.keyCode == 46
            || e.keyCode == 9 || e.keyCode == 20
                || e.keyCode == 37 || e.keyCode == 39
                    || (e.keyCode == 191 && !e.shiftKey)
                        || (e.keyCode == 56 && e.shiftKey)) {
        return true;
    } else {
        return false;
    }
}
function moveItems(target, items, requireSort) {
    if ($.isArray(items)) {
        $.each(items, function (index, item) {
            item.remove();
            item.appendTo(target);
        });
    } else {
        items.remove();
        items.appendTo(target);
    }
    if (requireSort) {
        //sortList(target);
    }
    clearSelection(target);
}
function getOperateItems(target) {
    return $("option:selected", target);
}
function getAllItems(target) {
    return $("option", target);
}
function getMatchedItems(target, codePatterns) {
    var result = new Array();
    $(codePatterns.split('/')).each(function () {
        var item = $("option[value='" + this + "']", target);
        if (item[0]) {
            result.push(item);
        }
    });
    return result;
}
function addItems(target, items) {
    items.each(function () {
        addItem(target, $(this));
    });
}
function addItem(target, item) {
    var options = $("option", target);
    if (options.size() == 0) {
        item.appendTo(target);
    } else {
        var thisValue = item.attr("value");
        options.each(function () {
            var currentValue = $(this).attr("value");
            if (currentValue > thisValue) {
                $(this).before(item);
                return false;
            } else if (!$(this).next()[0]) {
                $(this).after(item);
                return false;
            }
        });
    }
}
function sortList(target) {
    if (target.size() > 0) {
        //        getAllItems(target).each(function () {
        //            var currentItem = $(this);
        //            var currentValue = currentItem.attr("value");
        //            $(this).nextAll().each(function () {
        //                var next = $(this);
        //                var nextValue = next.attr("value");
        //                if (currentValue > nextValue) {
        //                    currentItem.insertAfter(this);
        //                    return false;
        //                }
        //            });
        //        });


        //        var currentItem = $("option:first", target);
        //        var currentValue = currentItem.attr("value");
        //        $.each(currentItem.nextAll(), function (index, item) {
        //            var nextValue = $(item).attr("value");
        //            if (currentValue > nextValue) {
        //                currentItem.remove();
        //                currentItem.after("<option value=\"" + currentValue + "\">" + currentItem.html() + "</option>");
        //                currentItem = currentItem.next();
        //            }
        //        });


        //        var currentItem;
        //        var currentValue;
        //        for (var i = 0; i < appendLength; i++) {
        //            currentItem = $("option:last", target);
        //            currentValue = currentItem.attr("value");
        //            $.each(currentItem.prevAll(), function (index, item) {
        //                var prev = $(item);
        //                var prevValue = prev.attr("value");
        //                if (currentValue < prevValue) {
        //                    currentItem.insertBefore(prev);
        //                    currentItem = prev;
        //                }
        //            });
        //        }
    }
}
function showCodes(source, target) {
    var codes = '';
    $("option", source).each(function (index) {
        if (index > 0) {
            codes += "/";
        }
        codes += $(this).attr("value");
    });
    target.val(codes);
}
function clearSelection(target) {
    $("option", target).removeAttr("selected");
}