/*
* 文件框输入时，同步下拉框的选择项
* sender:         文本框
* splitCharacter: 下拉框显示内容分隔符
*/
function SelectItem(sender, splitCharacter) {
    var targetControl = sender.nextSibling.nextSibling;
    var sourceControl = targetControl.nextSibling;
    var inputValue = $.trim(sender.value.toUpperCase());
    targetControl.innerHTML = '';
    if (inputValue.length == 0) {
        $(targetControl).html(sourceControl.innerHTML);
        var defaultValue = targetControl.options[0].value;
        targetControl.value = defaultValue;
    } else {
        var matchedArr = new Array();
        matchedArr.push(sourceControl.options[0].outerHTML);
        var sourceOptions = sourceControl.options;
        for (var i = 0; i < sourceOptions.length; i++) {
            var item = sourceOptions[i];
            if (item.text.toUpperCase().indexOf(inputValue) > -1) {
                matchedArr.push(item.outerHTML);
            }
        }
        $(targetControl).html(matchedArr.join(''));
        if (targetControl.options.length > 1) {
            targetControl.value = targetControl.options[1].value;
        } else {
            targetControl.value = targetControl.options[0].value;
        }
    }
    sourceControl.value = targetControl.value;
}
/*
* 文件框输入时，同步下拉框的选择项
* sender:         下拉框
* splitCharacter: 下拉框显示内容分隔符
* keyIndex:       文本框显示内容在下拉框显示内容中的序号 从0开始
*/
function ShowKey(sender, splitCharacter, keyIndex) {
    var that = $(sender);
    var key = '';
    var currentItem = sender.options[sender.selectedIndex];
    if (currentItem && currentItem.value != '' && currentItem.text != '') {
        var sourceItems = currentItem.text.split(splitCharacter);
        if (sourceItems.length > keyIndex) {
            key = sourceItems[keyIndex];
        }
    }
    that.prev().val(key);
    var sourceControl = that.next();
    sourceControl.val(that.val());
    if (key.length == 0 && that.children().length != sourceControl.children().length) {
        that.html(sourceControl.html());
    }
}
$(function () {
    $("select.ctag").each(function (index, item) {
        var target = $(item);
        target.hide();
        var osel = $("<select onchange=\"ShowKey(this,'-',0);\">" + target.html() + "</select>");
        osel.insertBefore(target);
    });
});