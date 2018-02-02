var ids;
var curruntIndex = 0;
$(function () {
    //判断是否要显示公告 
    AnnounceMethod(null, "/AnnounceHandlers/TerraceAnnounce.ashx/GetColseSession", "showORcolse");

    $("#prvIndex").click(function () {
        if (curruntIndex > 0) {
            curruntIndex = parseInt(curruntIndex) - 1;
        } else {
            curruntIndex = 0;
        }
        ShowOrHide();
        AnnounceMethod(JSON.stringify({ "id": ids[curruntIndex] }), "/AnnounceHandlers/TerraceAnnounce.ashx/QueryAnnounceOnce", "queryContent");
    });
    $("#nextIndex").click(function () {
        if (curruntIndex < (ids.length - 1)) {
            curruntIndex = parseInt(curruntIndex) + 1;
        } else {
            curruntIndex = (ids.length - 1);
        }
        ShowOrHide();
        AnnounceMethod(JSON.stringify({ "id": ids[curruntIndex] }), "/AnnounceHandlers/TerraceAnnounce.ashx/QueryAnnounceOnce", "queryContent");
    });
    $(".userClose").click(function () {
        AnnounceMethod(null, "/AnnounceHandlers/TerraceAnnounce.ashx/SetColseSession", "close");
    });
    $("body").keyup(function (e) {
        if (e.keyCode == 13 || e.keyCode == 27 || e.keyCode == 32) {
            $(".userClose").click();
            e.returnValue = false;
        }
    });
});
function ShowOrHide() {
    if (curruntIndex == (ids.length - 1)) {
        $("#nextIndex").hide();
    } else {
        $("#nextIndex").show();
    } if (curruntIndex == 0) {
        $("#prvIndex").hide();
    } else {
        $("#prvIndex").show();
    }
}

//调用的方法
function AnnounceMethod(parameters, address, type) {
    sendPostRequest(address, parameters, function (e) {
        if (type == "showORcolse") {
            if (e) {
                $("#Announce,#mask").hide();
            } else {
                //加载所有的紧急公告id
                AnnounceMethod(null, "/AnnounceHandlers/TerraceAnnounce.ashx/QueryEmergenceIds", "queryIds");
            }
        }
        if (type == "queryIds") {
            ids = eval(e);
            $("#prvIndex").hide();
            if (ids.length == 1) {
                $("#prvIndex").hide();
                $("#nextIndex").hide();
                $("#pager").hide();
            }
            if (ids.length > 0) {
                AnnounceMethod(JSON.stringify({ "id": ids[0] }), "/AnnounceHandlers/TerraceAnnounce.ashx/QueryAnnounceOnce", "queryContent");
            }
        }
        if (type == "queryContent") {
            $("#lblTitle").html(e.Title);
            $("#content p").html(e.Content);
            $("#lblPublishTime").html(e.PublishTime);
            $("#divOpcial").click();
            $("#Announce").css("display", "");
            $("#userClose").focus();
        }
        if (type == "close") {
            $("#Announce,#mask").hide();
        }
        $("#div_Announce").css("top", "50px");
    }, function (e) {

    });
}