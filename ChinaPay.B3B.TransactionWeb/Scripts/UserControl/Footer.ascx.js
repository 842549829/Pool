goTopEx();
var SuggestType = 1;
function goTopEx()
{
    if (typeof (hideSuggest) != "undefined")
    {
        $("#help_trigger").hide();
    }
    var obj = document.getElementById("return_top");
    function getScrollTop()
    {
        return document.documentElement.scrollTop || document.body.scrollTop;
    }
    function setScrollTop(value)
    {
        document.documentElement.scrollTop = value;
        document.body.scrollTop = value;
    }
    window.onscroll = function ()
    {
        getScrollTop() > 0 ? obj.style.display = "" : obj.style.display = "none";
    }
    obj.onclick = function ()
    {
        var goTop = setInterval(scrollMove, 10);
        function scrollMove()
        {
            setScrollTop(getScrollTop() / 1.1);
            if (getScrollTop() < 1) { clearInterval(goTop); }
        }
    }
}
function Sugges(suggestCategory)
{
    SuggestType = suggestCategory;
}

function ShowMark()
{
    var xp_mark = document.getElementById("xp_mark");
    if (xp_mark != null)
    {
        xp_mark.style.left = 0 + "px";
        xp_mark.style.top = 0 + "px";
        xp_mark.style.position = "absolute";
        xp_mark.style.backgroundColor = "#000";
        xp_mark.style.zIndex = "9998";
        xp_mark.style.filter = "alpha(opacity=50)";
        var Ie_ver = navigator["appVersion"].substr(22, 1);
        if (Ie_ver == 6 || Ie_ver == 5)
        {
            hideSelectBoxes();
        }
        else
        {
            xp_mark.style.opacity = "0.5";
        }
        var XP_pt = XP_getPageSize();
        var XP_height = XP_pt.h + "px";
        xp_mark.style.width = "100%";
        xp_mark.style.height = XP_height;
        xp_mark.style.display = "block";
    }
    else
    {
        $("body").prepend("<div id='xp_mark' style='display:none'></div>");
        ShowMark();
    }
}
function HideMark()
{
    var xp_mark = document.getElementById("xp_mark");
    xp_mark.style.display = "none";
    var Ie_ver = navigator["appVersion"].substr(22, 1);
    if (Ie_ver == 6 || Ie_ver == 5)
    {
        showSelectBoxes();
    }
}
function XP_getPageSize()
{
    var pt = { w: 0, h: 0 };
    if (window.innerHeight && window.scrollMaxY)
    {
        pt.w = document.body.scrollWidth;
        pt.h = window.innerHeight + window.scrollMaxY;
    }
    else if (document.body.scrollHeight > document.body.offsetHeight)
    {
        pt.w = document.body.scrollWidth;
        pt.h = document.body.scrollHeight;
    }
    else
    {
        pt.w = document.body.offsetWidth;
        pt.h = document.body.offsetHeight;
    }
    return pt;
}
function showSelectBoxes()
{
    selects = document.getElementsByTagName("select");
    for (i = 0; i != selects.length; i++)
    {
        selects[i].style.visibility = "visible";
    }
}
function hideSelectBoxes()
{
    selects = document.getElementsByTagName("select");
    for (i = 0; i != selects.length; i++)
    {
        selects[i].style.visibility = "hidden";
    }
}
function showDiv()
{
    var obj = document.getElementById("feedback");
    obj.style.position = "absolute";
    obj.style.zIndex = "9999";
    obj.style.display = "block";

    var d = document.documentElement, b = document.body, w = window;
    var viewPort = { left: 0, top: 0, width: 0, height: 0 };
    viewPort.top = b.scrollTop || d.scrollTop;
    viewPort.left = b.scrollLeft || d.scrollLeft;
    viewPort.height = w.innerHeight || d.clientHeight || b.clientHeight;
    viewPort.width = w.innerWidth || d.clientWidth || b.clientWidth;
    obj.style.top = (viewPort.top + viewPort.height / 2 - obj.offsetHeight / 2) + "px";
    obj.style.left = (viewPort.left + viewPort.width / 2 - obj.offsetWidth / 2) + "px";
}
function closeDiv()
{
    var obj = document.getElementById("feedback");
    obj.style.display = "none";
}
$(document).ready(function ()
{
    $("#help_trigger_a").click(function ()
    {
        $("#help_trigger_box").removeClass("hidden");
        $("#help_trigger").css({ "right": "-210px", "width": "245px" });
        $("#help_trigger").stop().animate({ right: "0" }, "slow");
    })
    $("#help_trigger_box").mouseleave(function () {
        $("#help_trigger").stop().animate({ right: "-210px" }, "slow", function () {
            $("#help_trigger_box").addClass("hidden");
            $("#help_trigger").css({ "width": "35px", "right": "0" });
        });
    });
    $("#help_trigger_box .close1").click(function ()
    {
        $("#help_trigger_box").addClass("hidden");
    });
    $("#feedback .nav li a").click(function ()
    {
        $("#feedback .nav li a").removeClass("selected");
        $(this).addClass("selected");
    });

    $("#SaveSuggest").click(function ()
    {  //保存建议
        var content = $("#suggestContent").val();
        if (content.length > 2000)
        {
            alert("建议内容过长！");
            return false;
        }
        if ($.trim(content) == "")
        {
            alert("还没有输入内容！");
            return false;
        }
        var contractInfomation = $("#contractInfomation").val();
        if (contractInfomation.length > 100)
        {
            alert("联系方式过长！");
            return false;
        }
        if (contractInfomation == 'QQ/邮箱/电话') contractInfomation = "";
        $.post("/AnnounceHandlers/UserSuggest.ashx?action=AddSuggest", { "category": SuggestType, "content": content, "method": contractInfomation }, function (rsp)
        {
            alert("提交成功\n感谢您的支持！");
            $("#suggestContent").val("");
            $("#contractInfomation").val("");
            HideMark(); closeDiv();
        }, 'text');
    });

});