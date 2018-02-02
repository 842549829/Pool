/*
* 弹出层组件
* 调用方式,在触发的容器上添加属性data, 例如：<a href="" data='{type:"ajax",url:"/test/test.html"}'></a>
* @data 存放弹出层数据和被绑定事件
* @param{type} 弹出层的类型 type 参数 ajax，pop，tips
* @param{url} 弹出层的url
* @param{id} 弹出层的id
*/
$.fn.extend({
    getCPos: function (w, h) {
        var ww = document.body.scrollWidth/*当前网页的宽度*/,
			wh = document.body.scrollHeight/*当前网页的高度*/,
            vw = document.documentElement.clientWidth/*当前窗口的宽度*/,
            vh = document.documentElement.clientHeight/*当前窗口的高度*/,
			l = parseInt(Math.max(document.body.scrollLeft, document.documentElement.scrollLeft))/*滚动条left的位置*/,
			t = parseInt(Math.max(document.body.scrollTop, document.documentElement.scrollTop))/*滚动条top的位置*/,
			ret = { l: l, t: t, st: t, sl: l, w: vw + 22, h: vh };
        //ret = {l:l, t:t, w:Math.max(document.body.scrollWidth, document.documentElement.scrollWidth)+22, h:Math.max($(window).height(), $('body').height(), screen.height)+22};

        if (w < vw) {
            ret.l = parseInt(vw / 2 - w / 2);
        }
        if (h < vh) {
            ret.t = parseInt(vh / 2 - h / 2);
        }
        return ret;
    }
});
function closeLayer() {
    $('div.layer:visible').find('a.close').trigger('click');
}
(function (index) {
    var autoCenter = function () {
        var mask = $('#mask');
        if (mask.css('display') == 'none') return;

        var target = mask.data('target') || $('.layer', mask),
			pos = $.fn.getCPos(target.width(), target.height());
        mask.width(pos.w).height(pos.h);
        if (target.height() < pos.h) {//显示的内容高度没有超过窗口的高度时
            target.css('top', pos.t);
        } else {
            target.css('top', 0);
            mask.width(pos.w - 22);
        }
        if ($('.layer', mask).length == 0) {
            target.css({
                'top': pos.t + pos.st,
                'left': pos.l + pos.sl
            });
        }
    };
    $(function () {
        $('<div id="mask" style=\' position:fixed; \'></div>').data('body', $($.browser.msie ? 'html' : 'body')).on('click', '.close,.closeBtn', function () {
            $(this).trigger('close');
            return false;
        }).on('show', function () {
            var pos = $.fn.getCPos();
            $(this).css('top', 0).css('left', 0).width(pos.w).height(pos.h).show().data('body').addClass('noscroll');
            $(window).on('resize', autoCenter);
        }).on('close', function () {
            $(this).empty().removeData('target').hide().data('body').removeClass('noscroll');
            $(window).off('resize', autoCenter);
        }).appendTo('body');
    });
    var lastTime = +new Date() - 2000;
    $('body').on('click', '[data!=""]', function () {
        try {
            var v = $(this).attr('data');
            if (v === '' || v === undefined || +new Date() - lastTime < 10) throw 'no data';
            lastTime = +new Date();
            var config = eval('(' + $(this).attr('data') + ')');
            if (typeof config.type === 'undefined') throw 'no type';
        } catch (e) { return; }
        if ($(this).attr('switch')) { return }
        if (config.type == 'ajax') {
            if (/\w+\?\w+/.test(config.url)) {
                config.url = config.url + "&rnd=" + new Date().getTime();
            } else {
                config.url = config.url + "?rnd=" + new Date().getTime();
            }
            $('#mask').load(config.url, function (response, status, xhr) {
                var o = $('.layer', this), pos = $.fn.getCPos(o.width(), o.height());
                if (o.height() < pos.h) {//显示的内容高度没有超过窗口的高度时
                    o.css('top', pos.t - 20);
                } else {
                    $('#mask').css('width', pos.w - 22);
                    o.css('top', 0);
                }
                o.show();
            }).on("mouseenter", ".close", (function (event) {
                var target = $(event.target);
                if (target.is("a") && !target.attr("title")) {
                    target.attr("title", "按Esc也可关闭");
                }
            })).trigger('show');
            return false;
        } else if (config.type == 'pop') {
            var o = $('#' + config.id + ''),
				pos = $.fn.getCPos(o.width(), o.height());

            $('#mask').data('target', o).trigger('show');
            o.css({
                position: 'absolute',
                left: pos.l + pos.sl,
                top: pos.t + pos.st,
                'z-index': 9999
            }).hide().fadeIn("slow").on('click', '.close,.closeBtn', function () {
                $('#mask').trigger('close');
                $('#' + config.id + '').css('left', '-2000px');
                o = null;
            });
            return false;
        }
    }).on('keyup', function (e) {
        if (e.which == 27) {
            $('.close').click();
        }
    });
})(999);

/*
* SetCwinHeight 让同域的iframe高度自适应
*/

function SetCwinHeight() {
    var iframeid = top.document.getElementById("iframe"); //iframe id
    if (document.getElementById) {
        if (iframeid && !window.opera) {
            if (iframeid.contentDocument && iframeid.contentDocument.body.offsetHeight) {
                iframeid.height = iframeid.contentDocument.body.offsetHeight + 80;
            } else if (iframeid.Document && iframeid.Document.body.scrollHeight) {
                iframeid.height = iframeid.Document.body.scrollHeight + 80;
            }
        }
    }
}
SetCwinHeight();

/*
* drawMenu 侧边栏菜单收起
*/
function drawMenu() {
    var item = $('.menu-client > li');
    var curNum;
    item.click(function () {
        var that = $(this);
        curNum = that.index();
        for (var i = 0, l = item.length; i < l; i++) {
            if (i != curNum) {
                item.eq(i).removeClass('active').find('ul').slideUp();
            }
        }
        that.toggleClass('active').find('ul').slideToggle();
    });
    var objMenu = $('ul.sub-menu');
    objMenu.on('click', 'a', function (e) {
        objMenu.find('a').removeClass('cur');
        $(this).addClass('cur');
        e.stopPropagation();
    });
}

/*
* 表单序列化 2json
*/
function paramString2obj(serializedParams) {
    var obj = {};
    function evalThem(str) {
        var attributeName = str.split("=")[0];
        var attributeValue = str.split("=")[1];
        if (!attributeValue) {
            return;
        }

        var array = attributeName.split(".");
        for (var i = 1; i < array.length; i++) {
            var tmpArray = Array();
            tmpArray.push("obj");
            for (var j = 0; j < i; j++) {
                tmpArray.push(array[j]);
            };
            var evalString = tmpArray.join(".");
            // alert(evalString);
            if (!eval(evalString)) {
                eval(evalString + "={};");
            }
        };

        eval("obj." + attributeName + "='" + attributeValue + "';");

    };

    var properties = serializedParams.split("&");
    for (var i = 0; i < properties.length; i++) {
        evalThem(properties[i]);
    };

    return obj;
}

$.fn.form2json = function () {
    var serializedParams = this.serialize();
    var obj = paramString2obj(serializedParams);
    return JSON.stringify(obj);
}

///获取URL参数
function getRequest() {
    var url = location.search; // 获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") > -1) {
        var pair = url.substr(1).split("&");
        for (var i = 0; i < pair.length; i++) {
            theRequest[pair[i].split("=")[0]] = decodeURI(pair[i].split("=")[1]);
        }
    }
    return theRequest;
}

function sendPostRequest(targetUrl, parameters, successCallback, errorCallback) {
    $.ajax({
        type: "POST",
        url: targetUrl,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: successCallback,
        timeout:30000,
        error: function (e) {
            if (e.responseText == "") return;
            if (e.status == 300) {
                if (JSON.parse(e.responseText) == "RequireLogon") {
                    window.location.href = "/Logon.aspx";
                    return;
                } else if (JSON.parse(e.responseText) == "Unauthorized") {
                    window.location.href = "/StaticHtml/NoAccess.aspx";
                    return;
                }
            } else if (e.status == 401 && e.statusText == "Unauthorized") {
                window.location.href = "/Logon.aspx";
                return;
            }
            if (e.statusText == "timeout")
            {
                alert("服务器忙");
                errorCallback&&errorCallback(e);
            } else if (e)
            {
                errorCallback&&errorCallback(e);
            }
        }
    });
}

function drawPagination(container, pageIndex, pageSize, dataCount, pageChangedCallback) {
    if (dataCount > 0) {
        var pageCount = parseInt((dataCount + pageSize - 1) / pageSize);
        var contents = new Array();
        contents.push('<div class="dataPager clearfix">');
        if(pageIndex > 1) {
            contents.push('<a id="pager_lbnFirst" value=1>首页</a>');
            contents.push('<a id="pager_lbnPrev" value=' + (pageIndex - 1) + '>&lt; 上一页</a>');
        }
        contents.join('<span id="pPages">');
        for(var i = pageIndex - 4; i <= pageIndex + 4; i++) {
            if(i == pageIndex) {
                contents.push('<a id="pager_lbPage' + i + '" disabled="disabled" class="yemaa" style="margin-left:5px;margin-right:5px;text-decoration:none;">' + i + '</a>');
            } else if(i > 0 && i <= pageCount) {
                contents.push('<a id="pager_lbPage' + i + '" value=' + i + ' style="margin-left:5px;margin-right:5px;">' + i + '</a>');
            }
        }
        contents.join('</span>');
        if(pageIndex < pageCount) {
            contents.push('<a id="pager_lbnNext" value=' + (pageIndex + 1) + '>下一页 &gt;</a>');
            contents.push('<a id="pager_lbnLast" value=' + pageCount + '>末页</a>');
        }
        contents.push("每页显示<select id='dropPageSize' style='width:50px;'><option selected='selected' value='10'>10</option><option value='20'>20</option><option value='30'>30</option><option value='50'>50</option><option value='100'>100</option></select>条");
        contents.push('共 <span id="pager_lblTotalCount">' + dataCount);
        //contents.push('</span> 条 每页 <span id="pager_lblPageSize">' + pageSize);
        contents.push('</span> &nbsp;条 第 <span id="lblCurrentPage">' + pageIndex + '</span> / <span id="pager_lblTotalPage">' + pageCount + '</span> 页</div>');
        container.html(contents.join(''));
        ControlPageHeight();
        var pageSize;
        $("#dropPageSize option[value='" + pageSize + "']").attr("selected","selected");
        if (pageChangedCallback) {
            $("a", container).click(function () {
                var self = $(this);
                if (!self.attr("disabled")) {
                    pageSize = $("#dropPageSize option:selected").val();
                    pageChangedCallback(self.attr("value"),pageSize);
                }
            });
            $("#dropPageSize").die("change").live("change", function () {
                pageSize = $("option:selected", this).val();
                pageChangedCallback(1,pageSize);
            });
        }
        container.show();
    } else {
        ControlPageHeight();
        container.html('');
        container.hide();
    }
}

//省份代码
var aCity = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }
//验证身份证
/*
sid:传入的身份证号码
return:成功?true:fasle
*/
function isCardID(sId) {
    var iSum = 0;
    var info = "";
    if (!/^\d{17}(\d|x)$/i.test(sId)) return false;
    sId = sId.replace(/x$/i, "a");
    if (aCity[parseInt(sId.substr(0, 2))] == null) return false;
    sBirthday = sId.substr(6, 4) + "-" + Number(sId.substr(10, 2)) + "-" + Number(sId.substr(12, 2));
    var d = new Date(sBirthday.replace(/-/g, "/"));
    if (sBirthday != (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate())) return false;
    for (var i = 17; i >= 0; i--) iSum += (Math.pow(2, i) % 11) * parseInt(sId.charAt(17 - i), 11);
    if (iSum % 11 != 1) return false;
    return true; //aCity[parseInt(sId.substr(0,2))]+","+sBirthday+","+(sId.substr(16,1)%2?"男":"女") 
}
//文本框智能提示
function inputTipText() {
    $("input.null").each(function () {
        var that = $(this);
        var tipText = that.attr("tip");
        if (that.val().length == 0 || that.val() == tipText) {
            that.val(tipText);
        } else {
            that.removeClass("null");
        }
        that.focus(function () {
            var currentValue = $.trim(that.val());
            if (currentValue == tipText || currentValue.length == 0) {
                that.removeClass("null");
                that.val("");
            }
        }).blur(function () {
            var currentValue = $.trim(that.val());
            if (currentValue == tipText || currentValue.length == 0) {
                that.addClass("null");
                that.val(tipText);
            }
        });
    });
}
//控制页面高度
function ControlPageHeight() {
    var rightFrame = parent.document.getElementById("rightFrame");
    if (rightFrame) {
        rightFrame.height = document.getElementsByTagName("body")[0].offsetHeight;
    }
}

function LoadSkins()
{
    var skinPath;
    var win = window;
    if (window.parent)
    {
        win = window.parent;
    }
    var skinControl = win.document.getElementById("skinPath");
    if (skinControl)
    {
        skinPath = skinControl.value;
    } else if (win.skinPath)
    {
        skinPath = win.skinPath;
    } else
    {
        skinPath = "/Styles/skins/default/";
    }
    var linkItems = document.getElementsByTagName("link");
    for (var index = 0; index < linkItems.length; index++)
    {
        var linkItem = linkItems[index];
        var hrefValue = linkItem.getAttribute("href");
        linkItem.setAttribute("href", hrefValue.replace("skinPath/", skinPath));
    }
}