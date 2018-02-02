/*
* 文件框输入时，同步下拉框的选择项
* sender:         文本框
* splitCharacter: 下拉框显示内容分隔符
*/
function SelectItem(sender, splitCharacter) {
    var sourceControl = sender.nextSibling.nextSibling;
    var inputValue = sender.value.toUpperCase();
    for (var i = 0; i < sourceControl.options.length; i++) {
        var sourceItems = sourceControl.options[i].text.toUpperCase().split(splitCharacter);
        for (var j = 0; j < sourceItems.length; j++) {
            if (sourceItems[j] == inputValue) {
                sourceControl.options[i].selected = true;
                return;
            }
        }
    }
    sourceControl.options[0].selected = true;
}
/*
* 文件框输入时，同步下拉框的选择项
* sender:         下拉框
* splitCharacter: 下拉框显示内容分隔符
* keyIndex:       文本框显示内容在下拉框显示内容中的序号 从0开始
*/
function ShowKey(sender, splitCharacter, keyIndex) {
    var key = '';
    var currentItem = sender.options[sender.selectedIndex];
    if (currentItem && currentItem.value != '' && currentItem.text != '') {
        var sourceItems = currentItem.text.split(splitCharacter);
        if (sourceItems.length > keyIndex) {
            key = sourceItems[keyIndex];
        }
    }
    sender.previousSibling.previousSibling.value = key;
}