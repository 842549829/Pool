$(function () {
    //选项切换
    $("div.divCompanyWorkInfo>div").each(function (index) { if (index != 0) $(this).hide(); });
    $("#sel > li > a").click(function () {
        var self = $(this);
        $("#sel > li > a").removeClass("navType1Selected");
        self.addClass("navType1Selected");
        var id = "." + self.attr("id");
        $("div.divCompanyWorkInfo>div").hide();
        $("div.divCompanyWorkInfo>div" + id).show();
        $(".divCompanyWorkInfo>h2").html(self.text());
    });
    $(".specialProduct").each(function () {
        var self = $(this); var checkbox = self.prev(":checkbox");
        if (checkbox.is(":checked")) { self.show(); } else { self.hide(); }
        checkbox.click(function () {if ($(this).is(":checked")) {self.show();} else {self.hide();}});
    });
});
/*
选项卡切换(记住选中的项)  
options：$("#ul li") 
切换的头部模板
<ul id='ul'><li id='li1' class='curr'></li><li id='li2'></li></ul>
切换的内容模板
<div class='li1'></div>
<div class='li2'></div>
*/
function setOptions(options) {
    function setHiddenAndDisplay(self) {
        $(".curr").removeClass("curr");
        self.addClass("curr");
    };
    function setOptionsHidden() {
        options.each(function () {
            $("." + $(this).attr("id")).hide();
        });
    }
    function optionOnclick(self) {
        setHiddenAndDisplay(self);
        setOptionsHidden();
        $("." + self.attr("id")).show();
        setCookieCurrentPath("OptionsId", self.attr("id"), 1);
    }
    var optionsId = getCookie("OptionsId");
    if (optionsId) {
        setHiddenAndDisplay($("#" + optionsId));
        setOptionsHidden();
        $("." + optionsId).show();
    }
    options.each(function (indext, item) {
        var self = $(this);
        var className = $("." + self.attr("id"));
        if (indext != 0 && !optionsId) {
            className.hide();
        }
        self.off().click(function () {
            optionOnclick($(this));
        });
    });
}