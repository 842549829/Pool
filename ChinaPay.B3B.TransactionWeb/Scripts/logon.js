var bigScrollSpeed = 500, bigScrollSpeeds = 8000, smallScrollSpeed = 500, smallScrollSpeeds = 5000, bigIndex = 0, smallIndex = 0, bigTimeObj, smallTimeObj;
function loadValidateCode() {
    $("#imgValidateCode").attr("src", '/ValidateCode.aspx?' + Math.random());
}
function logonValidate() {
    if ($("#txtUserName").val() == "") {
        ShowMsg("请输入帐号！");
        $("#txtUserName").focus();
        return false;
    }
    if ($("#txtPassword").val() == "")
    {
        ShowMsg("请输入密码！");
        $("#txtPassword").focus();
        return false;
    }
    if ($("#txtCode").val() == "") {
        ShowMsg("请输入验证码！");
        $("#txtCode").focus();
        return false;
    }
    return true;
}
function loadRecommendCities() {
    sendPostRequest("/FlightHandlers/Recommend.ashx/QueryRecommendCities", "", function (cities) {
        var citiesHTML = "";
        $(cities).each(function (index, item) {
            citiesHTML += '<li><a onmouseover ="loadRecommendInfos(this, \'' + item.Code + '\')">' + item.Name + '</a></li>';
        });
        $("#divRecommendCities").html(citiesHTML);
        $("#divRecommendCities a").first().mouseover();
    }, function (e) {
        $("#divRecommendCities").html("加载数据出错");
    });
}
function loadRecommendInfos(sender, code) {

    $("#divRecommendCities .selectedCity").removeClass("selectedCity");
    $("#divRecommendCities .curr").removeClass("curr");
    $(sender).addClass("selectedCity");
    $(sender).parent().addClass("curr");
    $("#divRecommentContents li").hide();
    var loadedData = $("#divRecommentContents li[name='" + code + "']");
    if (loadedData.size() > 0) {
        loadedData.show();
    } else {
        sendPostRequest("/FlightHandlers/Recommend.ashx/QueryRecommendInfos", JSON.stringify({ "code": code }), function (fares) {
            var contentHTML = new Array();
            $(fares).each(function (index, item) {
                contentHTML.push("<li name=\"", code, "\"><span class=\"flightInfo\">");
                contentHTML.push("<a>", item.Departure, "-", item.Arrival, "</a>");
                contentHTML.push(" <span>", item.Date, "</span><span class=\"price\">￥");
                contentHTML.push(item.Fare, "</span></span></li>");
            });
            $("#divRecommentContents").append(contentHTML.join(''));
            $(sender).next().html(contentHTML);
        }, function () {
            $("#divRecommentContents").html("加载特价信息失败……");
        });
    }
}
function ShowMsg(msg) {
    $("#ErrorMsg").text(msg).addClass("ErrorTip");
}
function ClearMsg() {
    $("#ErrorMsg").html("&nbsp;").removeClass("ErrorTip");
}
$(function () {
    $(".focus").jFocus({ event: "mouseover", auto: 1, time: 5000 });
    $(".smallPicBox").jFocus({ event: "mouseover", auto: 1, time: 7000 });
    $(".smallPicBox .index").css("left", "25%");
    if (typeof msg != "undefined") {
        ShowMsg(msg);
    }
    $("#txtCode,#txtUserName,#loginBorderBox").blur(ClearMsg);
    $("#txtUserName").focus().select();
    loadValidateCode();
    loadRecommendCities();
});