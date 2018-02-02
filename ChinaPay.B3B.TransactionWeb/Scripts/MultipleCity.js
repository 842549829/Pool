function addCity(sourceObj, selectedObj, showObj, hidObj) {
    var items = getOperateItemsCity(sourceObj);
    moveItemsCity(selectedObj, items);
    showCityCodes(selectedObj, showObj, hidObj);
}
function removeCity(sourceObj, selectedObj, showObj, hidObj) {
    var items = getOperateItemsCity(selectedObj);
    moveItemsCity(sourceObj, items);
    showCityCodes(selectedObj, showObj, hidObj);
}
function addAllCitys(sourceObj, selectedObj, showObj, hidObj) {
    var items = getAllItemsCity(sourceObj);
    moveItemsCity(selectedObj, items);
    showCityCodes(selectedObj, showObj, hidObj);
}
function removeAllCitys(sourceObj, selectedObj, showObj, hidObj) {
    var items = getAllItemsCity(selectedObj);
    moveItemsCity(sourceObj, items);
    showCityCodes(selectedObj, showObj, hidObj);
}
function matchCitys(sourceObj, selectedObj, showObj, hidObj) {
    var codePattern = $.trim(showObj.val());
    if (codePattern == '') {
        removeAllCitys(sourceObj, selectedObj, showObj, hidObj);
    } else if (codePattern == '*') {
        addAllCitys(sourceObj, selectedObj, showObj, hidObj);
    } else {
        removeAllCitys(sourceObj, selectedObj, showObj, hidObj);
        var matchedItems = getMatchedItemsCity(sourceObj, codePattern);
        moveItemsCity(selectedObj, matchedItems);
        showCityCodes(selectedObj, showObj, hidObj);
    }
}
function moveItemsCity(target, items) {
    if ($.isArray(items)) {
        $.each(items, function (index, item) {
            item.remove();
            item.appendTo(target);
        });
    } else {
        items.remove();
        items.appendTo(target);
    }
    clearSelectionCity(target);
}
function getOperateItemsCity(target) {
    return $("option:selected", target);
}
function getAllItemsCity(target) {
    return $("option", target);
}
function getMatchedItemsCity(target, codePatterns) {
    var result = new Array();
    $(codePatterns.split('/')).each(function () {
        var item = $("option[text='" + this + "']", target);
        if (item[0]) {
            result.push(item);
        }
    });
    return result;
}
function addItemsCity(target, items) {
    items.each(function () {
        addItemCity(target, $(this));
    });
}
function addItemCity(target, item) {
    var options = $("option", target);
    if (options.size() == 0) {
        item.appendTo(target);
    } else {
        var thisValue = item.attr("text");
        options.each(function () {
            var currentValue = $(this).attr("text");
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

function showCityCodes(source, target, hidObj) {
    var codes = '';
    var text = '';
    $("option", source).each(function (index) {
        if (index > 0) {
            codes += "/";
            text += "/";
        }
        codes += $(this).attr("value");
        text += $(this).html();
    });
    hidObj.val(codes);
    target.val(text);
    //$("#codes").html(codes);
}
function clearSelectionCity(target) {
    $("option", target).removeAttr("selected");
}