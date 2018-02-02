(function ()
{
    if ($.browser.msie) { $("body").addClass("IE" + $.browser.version); }
    $("#data-list table:not(.ClearAlternate) tr:nth-child(even)").addClass("alternate");
    //$("#data-list table:not(.ClearAlternate) tr:nth-child(even)").addClass("alternate");

    String.prototype.format = function ()
    {
        var searchReg = /\{\d+\}/gi;
        var matchInfo = this.match(searchReg);
        if (matchInfo == null) return this;
        var replaceIndex = 0;
        var replaceArgs = arguments;
        var result = this.replace(searchReg, function () { return replaceArgs[replaceIndex++]; });
        return result;
    };

    $.Min = function (array,selector)
    {
        var min = 999999;
        for (var i = 0; i < array.length; i++)
        {
            if (i == 0) min = selector(array[i]);
            else if (min > selector(array[i]))
            {
                min = selector(array[i]);
            }
        }
        if (min == 999999) return NaN;
        return min;
    };

    $.Max = function (array, selector)
    {
        var max = 0;
        for (var i = 0; i < array.length; i++)
        {
            if (i == 0) max = selector(array[i]);
            else if (max < selector(array[i]))
            {
                max = selector(array[i]);
            }
        }
        if (max == 0) return NaN;
        return max;
    };


    $.fn.LimitLength = function (length)
    {
        var target = this;
        this.keyup(function ()
        {
            var val = target.val();
            if (val.length > length)
            {
                target.val(val.substring(0, length));
            }
        });
        return this;
    };
    $.fn.OnlyNumber = function ()
    {
        var target = this;
        this.keyup(function ()
        {
            if (!/^\d+$/.test(target.val())) target.val(target.val().replace(/[^\d]/g, ''));
        });
        return target;
    };
})($);

function setCookie(name, value, hour, path) {
    path = path || "/FlightReserveModule/";
    var cookie = name + "=" + encodeURIComponent(value)
        + (hour ? "; expires=" + new Date(new Date().getTime() + hour * 60 * 60 * 1000).toGMTString() : "") + ";path=" + path;
    document.cookie = cookie;
}
function setCookieCurrentPath(name, value, hour)
{
    var cookie = name + "=" + encodeURIComponent(value)
        + (hour ? "; expires=" + new Date(new Date().getTime() + hour * 60 * 60 * 1000).toGMTString() : "") + ";";
    document.cookie = cookie;
}


function getCookie(name) {
    var re = new RegExp("(^|;)\\s*(" + name + ")=([^;]*)(;|$)", "i");
    var res = re.exec(document.cookie);
    return res != null ? decodeURIComponent(res[3]) : "";
}

function saveObjectToCookie(obj, days) { for (var k in obj) { setCookie(k, obj[k], days); } }

//将数字类型保留weight为小数
function Round(num, weight) {
    var e = 1;
    for (var i = 0; i < weight; i++) {
        e *= 10;
    }
    return Math.round(num * e) / e;
}
//将数字类型收到小数点前后weight位
function Celling(num, weight) {
    var e = Math.pow(10, weight * -1);
    return Math.ceil(num * e) / e;
}
//将数字类型舍到小数点前后weight位
function Floor(num, weight) {
    var e = Math.pow(10, weight * -1);
    return Math.floor(num * e) / e;
}

//将数字类型格式化成0.00格式的字符串
function fillZero(num)
{
    if (num * 100 % 1 != 0)
    {
        num = Math.round(num * 100) / 100;
    }
    if (num * 10 % 1 != 0)
    {
        return num.toString();
    } else if (num % 1 != 0)
    {
        return num.toString() + '0';
    } else
    {
        return num.toString() + ".00";
    }
}


function copyToClipboard(data) {
    if (window.clipboardData) {
        window.clipboardData.setData('text', data);
    } else if (navigator.userAgent.indexOf("Opera") != -1) {
        window.location = data;
    } else if (window.netscape) {
        try
        {
            netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
        } catch (e)
        {
            alert("\u60A8\u7684firefox\u5B89\u5168\u9650\u5236\u9650\u5236\u60A8\u8FDB\u884C\u526A\u8D34\u677F\u64CD\u4F5C\uFF0C\u8BF7\u6253\u5F00\u2019about:config\u2019\u5C06signed.applets.codebase_principal_support\u2019\u8BBE\u7F6E\u4E3Atrue\u2019\u4E4B\u540E\u91CD\u8BD5\uFF0C\u76F8\u5BF9\u8DEF\u5F84\u4E3Afirefox\u6839\u76EE\u5F55/greprefs/all.js");
            return;
        }
        var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
        if (!clip)
        {
            return;
        }
        var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
        if (!trans)
        {
            return;
        }
        trans.addDataFlavor("text/unicode");
        var str = Components.classes['@mozilla.org/supports-string;1'].createInstance(Components.interfaces.nsISupportsString);
        var copytext = data;
        str.data = copytext;
        trans.setTransferData("text/unicode", str, copytext.length * 2);
        var clipid = Components.interfaces.nsIClipboard;
        if (!clip)
        {
            return;
        }
        clip.setData(trans, null, clipid.kGlobalClipboard);
    }
    alert("复制成功！");
}

function Go(url) {
    location.href = url + (url.indexOf('?') > -1 ? "&" : "?") + "returnUrl=" + location.href+"?Search=Back";
}

function HtmlEncode(text)
{
    return text.replace(/&/g, '&amp').replace(/\"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}

function HtmlDecode(text)
{
    return text.replace(/&amp;/g, '&').replace(/&quot;/g, '\"').replace(/&lt;/g, '<').replace(/&gt;/g, '>');
}

function setConditionCookie(obj, hour,pageName)
{
    var cookie = "";
    var isfirst = true;
    var content = "";
    for (var name in obj)
    {
        if (!isfirst) content += "&";
        content += name + "=" + encodeURIComponent(obj[name]);
        isfirst = false;
    }

    cookie += pageName+"=" + content + (hour ? "; expires=" + new Date(new Date().getTime() + hour * 60 * 60 * 1000).toGMTString() : "") + ";";
    document.cookie = cookie;
}