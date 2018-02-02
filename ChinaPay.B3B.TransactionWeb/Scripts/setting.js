function defaultUrl() {
    return decodeURI(window.location.host);
}
// 加入收藏
function addFavorite(title) {
    var url = defaultUrl();
    try {
        window.external.addFavorite(url, title);
    } catch (e) {
        try {
            window.sidebar.addPanel(title, url, "");
        } catch (e) {
            alert("加入收藏失败,请按Ctr+D进行添加书签");
        }
    }
}
//设为首页
function setHomePage(obj) {
    var url = defaultUrl();
    if (document.all) {
        obj.style.behavior = 'url(#default#homepage)';
        obj.setHomePage(url);
    } else if (window.sidebar) {
        if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            } catch (e) {
                alert("此操作被浏览器拒绝！\n请在浏览器地址栏输入“about:config”并回车\n然后将[signed.applets.codebase_principal_support]设置为'true'");
                return false;
            }
        }
        var prefs = Components.classes['@mozilla.org/preferences-service;1'].getService(Components.interfaces.nsIPrefBranch);
        prefs.setCharPref('browser.startup.homepage', homepage);
    } else {
        alert("请手动设置主页，\n我们的网址是：http://www.b3b.so");
    }
}