/*
保存条件cookie
obj 保存对象
hour 保存时间(已小时计算)
name 保存的名字
*/
function setConditionCookie(obj, hour, name) {
    var cookie = "", content = "";
    var isFirest = false;
    for (var name in obj) {
        if (isFirest) content += "&";
        content += name + "=" + encodeURIComponent(obj[name]);
        isFirest = true;
    }
    cookie += pageName + "=" + content + (hour ? "; expires=" + new Date(new Date().getTime() + hour * 60 * 60 * 1000).toGMTString() : "") + ";";
    document.cookie = cookie;
}
/*
保存查询条件
pageName 保存的名字 
*/
var conditions = new Object();
var pageName = "";
function SaveSearchCondition(pageName) {
    $(".condition input[type='text']").each(function () {
        var self = $(this);
        conditions[self.attr("id")] = "1|" + self.val();
    });
    $(".condition select").each(function () {
        var self = $(this);
        conditions[self.attr("id")] = "2|" + self.find(":selected").text();
    });
    setConditionCookie(conditions, 2, pageName);
}
/*
保存分页控件页码
currentPage 页码
*/
function saveSearchConditionWhenPagging(currentPage) {
    conditions.currentPage = "3|" + currentPage;
    SaveSearchCondition(pageName);
}