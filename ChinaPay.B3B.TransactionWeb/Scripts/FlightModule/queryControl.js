var needLoadCookie = true;  //在页面上控制设置以确定是否需要加载Cookie  
var isNewSearch = true;  //标记当前的搜索是不是新记录
$(function ()
{
    $("#radRoundWay").click(function ()
    {
        $("#txtBackDate").attr("disabled", false);
        $('.gobackdate').show();
    });
    $("#radOneWay").click(function ()
    {
        $("#txtBackDate").attr("disabled", true).val('');
        $('.gobackdate').hide();
    });
    $(".hotCityNav span").click(function ()
    {
        $(".hotCityNav span").removeClass();
        $(this).addClass("active");
        $(".cityList").hide();
        var indx = $(".hotCityNav span").index(this);
        $(".cityList").eq(indx).show();
        switch (indx)
        {
            case 1:
                $("#txtCityInfo").html("拼音A-G城市");
                break;
            case 2:
                $("#txtCityInfo").html("拼音H-L城市");
                break;
            case 3:
                $("#txtCityInfo").html("拼音M-T城市");
                break;
            case 4:
                $("#txtCityInfo").html("拼音W-Z城市");
                break;
            default:
                $("#txtCityInfo").html("热门城市");
                break;
        }
    });
    bindFlightQueryCondition($.trim($("#hidFlightQueryCondition").val()));
    $("#btnQueryFlight").click(function ()
    {
        var departureCode = $.trim($("#txtDepartureValue").val());
        if (departureCode == '')
        {
            alert("请输入出发城市");
            $("#txtDeparture").select();
            return false;
        }

        var arrivalCode = $.trim($("#txtArrivalValue").val());
        if (arrivalCode == '')
        {
            alert("请输入到达城市");
            $("#txtArrival").select();

            return false;
        }

        if (departureCode == arrivalCode)
        {
            alert("出发城市与到达城市不能相同");
            $("#txtArrival").select();
            return false;
        }
        var goDate = $("#txtGoDate").val();
        if (goDate == '')
        {
            alert("请选择出发日期");
            $("#txtGoDate").select();
            return false;
        }

        var airline = $("#txtAirlineValue").val();
        var voyageType = "1";
        if ($("#radRoundWay").attr("checked"))
        {
            voyageType = "2";
        }
        var queryPage = 'FlightQueryResult.aspx';
        var queryParameters = 'source=1&airline=' + airline + '&departure=' + departureCode + '&arrival=' + arrivalCode + '&goDate=' + goDate;
        if (voyageType == "2")
        {
            var backDate = $("#txtBackDate").val();
            if (backDate == '')
            {
                alert("请选择回程日期");
                $("#txtBackDate").select();
                return false;
            }
            queryPage = 'FlightQueryGoResult.aspx';
            queryParameters += '&backDate=' + backDate;
        }
        window.top.location = '/FlightReserveModule/' + queryPage + '?' + queryParameters;
        return false;
    });
    getAirlines();
    getAirports();
    if (needLoadCookie)
    {
       var currentSearchIndex = getSearchIndex();
       if (typeof (searchOption) != "undefined" && searchOption.departureCode != "")
        {
            LoadHistory(currentSearchIndex);
            InitSearchOptions(searchOption);
            saveObjectToCookie(searchOption, 8);
        } else
        {
           LoadHistory(currentSearchIndex);
           LoadBeforeData();
        }
    }

});
/** 航空公司 **/
var airlines = null;
var hotCityControl = null;
function showAirlines(valueControl, showControl, sender) {
    $(".tipControl").hide();
    createAirlinesSelectWindow(valueControl, sender, showControl);
    setPos(sender, showControl);
    hotCityControl = showControl;
}
function getAirlines() {
    if (airlines == null) {
        sendPostRequest("/FlightHandlers/Foundation.ashx/QueryAirlines", "", function (data) {
            airlines = data;
        });
    }
}
function createAirlinesSelectWindow(valueControl, textControl, showControl) {
    var airContent = "<li name='' style='color=\"#3399FF\"'>--全部--</li>";
    $.each(airlines, function (index, item) {
        airContent += "<li name='" + item.Code + "'>" + item.Name + "</li>";
    });
    airContent += "<div class=\"clearFloat\"></div>";
    $(".clearfix").eq(0).html(airContent);
    $("ul li", showControl).hover(function () { $(this).css("color", "#FF6600"); }, function () { $(this).css("color", "#666666"); });
    $("ul li", showControl).click(function () {
        fillAirline(valueControl, textControl, $(this));
        showControl.hide();
    });
    $("img", showControl).click(function () {
        showControl.hide();
    });
}
function fillAirline(valueControl, textControl, sender) {
    textControl.val(sender.html());
    valueControl.val(sender.attr('name'));
}
/** 城市/机场 **/
function showCitis(valueControl, showControl, sender, e) {
    if (hotCityControl) hotCityControl.hide();
    if (getEventType(e) == "keyup") {
        switch (e.keyCode) {
            //向上方向键   
            case 38:
                selectPrev();
                return;
                //向下方向键
            case 40:
                selectNext();
                return;
                //向右方向键
            case 39:
                //向左方向键
            case 37:
                //对应Alt键
            case 18:
                //对应CapsLock键
            case 20:
                return;
                //对应回车键
            case 13:
                fillCurrent(valueControl, sender);
                return;
                //对应Esc键
            case 27:
                hideAirportTip();
                return;
        }
    }
    var currentValue = $.trim(sender.val());
    showMatchedAirportTipWindow(valueControl, sender, currentValue);
    showControl.hide();
}
function airportControlBlured(valueControl, showControl, sender) {
    fillCurrent(valueControl, sender);
    hideAirportTip();
    showControl.hide();
}
function showCitiesSelectWindow(valueControl, showControl, txtControl) {
    $(".tipControl").hide();
    resetHotCities(showControl);
    getCities();
    createCitiesSelectWindow(valueControl, txtControl, showControl);
    setPos(txtControl, showControl);
}
function showMatchedAirportTipWindow(valueControl, txtControl) {
    $(".tipControl").hide();
    var inputText = $.trim(txtControl.val());
    var matchedAirports = matchAirportItems(inputText);
    createAirportTipWindow(valueControl, txtControl, matchedAirports, inputText);
    setPos(txtControl, tipContentControl);
}
function getCities() {
    if (cities == null) {
        cities = new Array();
        $.each(airports, function (index, item) {
            if (item.IsMain) {
                cities.push(item);
                if (item.IsHot) {
                    hotCities.push(item);
                }
            }
        });
    }
}
function getAirports() {
    if (airports == null) {
        sendPostRequest("/FlightHandlers/Foundation.ashx/QueryAirports", "", function (data) {
            airports = data;
        });
    }
}
var airports = null;
var cities = null;
var hotCities = new Array();
var A_G = "ABCDEFG", H_L = "HIJKL", M_T = "MNOPQRST", W_Z = "WXYZ", cityInx = [];
var arrA_G = [], arrH_L = [], arrM_T = [], arrW_Z = [];
var hotCityContent = "", ContentA_G = "", ContentH_L = "", ContentM_T = "", ContentW_Z = "";
function createCitiesSelectWindow(valueControl, textControl, showControl) {
    arrA_G = [];
    arrH_L = [];
    arrM_T = [];
    arrW_Z = [];
    hotCityContent = "";
    ContentA_G = "";
    ContentH_L = "";
    ContentM_T = "";
    ContentW_Z = "";

    //热门城市
    $.each(hotCities, function (index, item) {
        hotCityContent += "<li code=" + item.Code + " name=" + item.Name + ">" + item.City + "</li>";
    });
    hotCityContent += "<div class=\"clearFloat\"></div>";
    $(".cityList").eq(0).html(hotCityContent);

    $(".hotCityNav span").show();
    $.each(cities, function (index, item) {
        var curCity = item.Spelling.substring(0, 1).toUpperCase();
        if (A_G.indexOf(curCity) >= 0) {
            arrA_G.push(item);
        } else if (H_L.indexOf(curCity) >= 0) {
            arrH_L.push(item);
        } else if (M_T.indexOf(curCity) >= 0) {
            arrM_T.push(item);
        } else if (W_Z.indexOf(curCity) >= 0) {
            arrW_Z.push(item);
        }
    });
    //其他城市
    $.each(arrA_G, function (index, item) {
        ContentA_G += "<li code=" + item.Code + " name=" + item.Name + ">" + item.City + "</li>";
    });
    $.each(arrH_L, function (index, item) {
        ContentH_L += "<li code=" + item.Code + " name=" + item.Name + ">" + item.City + "</li>";
    });
    $.each(arrM_T, function (index, item) {
        ContentM_T += "<li code=" + item.Code + " name=" + item.Name + ">" + item.City + "</li>";
    });
    $.each(arrW_Z, function (index, item) {
        ContentW_Z += "<li code=" + item.Code + " name=" + item.Name + ">" + item.City + "</li>";
    });

    ContentA_G += "<div class=\"clearFloat\"></div>";
    ContentH_L += "<div class=\"clearFloat\"></div>";
    ContentM_T += "<div class=\"clearFloat\"></div>";
    ContentW_Z += "<div class=\"clearFloat\"></div>";
    $(".cityList").eq(1).html(ContentA_G);
    $(".cityList").eq(2).html(ContentH_L);
    $(".cityList").eq(3).html(ContentM_T);
    $(".cityList").eq(4).html(ContentW_Z);

    $(".cityList li").click(function () {
        fillCity(valueControl, textControl, $(this));
        resetHotCities(showControl);
    });
    $("#hotCity img").click(function () {
        resetHotCities(showControl);
    });
}
var tipContentControl = null;
function createAirportTipWindow(valueControl, textControl, matchedAirports, inputText) {
    initTipWindow();
    var tipTitleMsg = '';
    var tipTitleHtml = '';
    var tipOptionsHtml = '';
    if (inputText == '') {
        tipTitleMsg = '支持汉字/拼音/三字码';
    } else if (matchedAirports.length == 0) {
        tipTitleMsg = '对不起，找不到：' + inputText;
    } else {
        tipTitleMsg = inputText + '，按拼音排序';
    }
    if (matchedAirports.length == 0) {
        tipOptionsHtml = '<table width="180" class="mout" height="2"><tr><td></td></tr></table>';
    } else {
        $.each(matchedAirports, function (index, item) {
            tipOptionsHtml += '<table class="mout" name="option" width="180"><tr>' +
                '<td class="tdleft" height="10" align="left">' + item.Spelling +
                    '</td><td class="tdright" align="right">' + item.City + '[' + item.Code + ']</td><td style="display:none">' + item.Code + '</td></tr></table>';
        });
    }
    tipTitleHtml = '<table class="hint" width="180"><tr align="left"><td class="tdleft" height="10" align="left">' + tipTitleMsg + '</td></tr></table>';
    tipContentControl.html(tipTitleHtml + tipOptionsHtml);
    var optionControls = $("table[name=option]", tipContentControl);
    optionControls.click(function () {
        selectAirport(valueControl, textControl, $(this));
        hideAirportTip();
    });
    optionControls.first().addClass("selected");
    optionControls.first().nextAll().hover(function () {
        this.className = "selected";
    }, function () {
        this.className = "mout";
    });
    reviseAirportTipWindow();
    showAirportTip();
}
function selectNext() {
    var current = $(".selected:last", tipContentControl);
    if (current[0]) {
        var nextOption = current.next();
        if (nextOption[0]) {
            var prevOption = current.prev();
            if (prevOption[0] && prevOption.attr("name") == "option") {
                current[0].className = "mout";
            }
            nextOption[0].className = "selected";
        }
    }
}
function selectPrev() {
    var current = $(".selected:last", tipContentControl);
    if (current[0]) {
        var prevOption = current.prev();
        if (prevOption[0] && prevOption.attr("name") == "option") {
            current[0].className = "mout";
            prevOption[0].className = "selected";
        }
    }
}
function fillCurrent(valueControl, textControl) {
    var current = $(".selected:last", tipContentControl);
    if (current[0]) {
        selectAirport(valueControl, textControl, current);
    }
    hideAirportTip();
    return false;
}
function initTipWindow() {
    if (tipContentControl != null) {
        $("#tipContent").remove();
    }
    var objBody = document.getElementsByTagName("body").item(0);
    var objTipContent = document.createElement("div");

    objTipContent.style.zindex = '100';
    objTipContent.style.position = 'absolute';
    objTipContent.setAttribute('id', 'tipContent');
    objTipContent.setAttribute('align', 'left');
    objTipContent.className = "tipControl";
    objBody.appendChild(objTipContent);
    tipContentControl = $(objTipContent);
}
function fillCity(valueControl, textControl, sender) {
    var code = sender.attr("code");
    var name = sender.attr("name");
    var city = sender.html();
    textControl.val(city + '[' + code + ']');
    valueControl.val(code);
}
function selectAirport(valueControl, textControl, sender) {
    var items = $("td", sender);
    textControl.val(items.eq(1).html());
    valueControl.val(items.eq(2).html());
}
function matchAirportItems(condition) {
    var result = new Array();
    var inputUpperString = condition.toUpperCase();
    if (inputUpperString == '') {
        $.each(airports, function (index, item) {
            result.push(item);
        });
    } else {
        $.each(airports, function (index, item) {
            if (isMatch(item.Code, inputUpperString)
                || isMatch(item.City, inputUpperString)
                    || isMatch(item.Spelling, inputUpperString)
                        || isMatch(item.ShortSpelling, inputUpperString)
                            || isMatch(item.Name, inputUpperString)
                                || isMatch(item.City + '[' + item.Code + ']', inputUpperString)) {
                result.push(item);
            }
        });
    }
    return result;
}
function isMatch(source, pattern) {
    if (source) {
        var length = pattern.length;
        if (source.length >= length) {
            return source.substr(0, length).toUpperCase() == pattern;
        }
    }
    return false;
}

function getEventType(e) {
    var code = '';
    if (!e) e = window.event;
    if (e) {
        if (e.type) code = e.type;
        else if (e.witch) code = e.witch;
    }
    return code;
}
function resetHotCities(sender) {
    sender.hide();
    $(".hotCityNav span", sender).removeClass();
    $(".hotCityNav span", sender).eq(0).addClass("active");
    $(".cityList", sender).hide();
    $(".cityList", sender).eq(0).show();
}
function bindFlightQueryCondition(condition) {
    if (condition != '') {
        var conditionModel = JSON.parse(condition);
        if (conditionModel) {
            if (conditionModel.VayageType == "2") {
                $("#radRoundWay").click();
            } else {
                $("#radOneWay").click();
            }
            if (condition.Airline) {
                $("#txtAirlineValue").val(condition.Airline.Code);
                $("#txtAirline").val(condition.Airline.Name);
            }
            if (condition.Departure) {
                $("#txtDepartureValue").val(condition.Departure.Code);
                $("#txtDeparture").val(condition.Departure.Name);
            }
            if (condition.Arrival) {
                $("#txtArrivalValue").val(condition.Arrival.Code);
                $("#txtArrival").val(condition.Arrival.Name);
            }
            if (condition.GoDate) {
                $("#txtGoDate").val(condition.GoDate);
            }
            if (condition.BackDate) {
                $("#txtBackDate").val(condition.BackDate);
            }
        }
    }
}
//隐藏过长的城市列表
function reviseAirportTipWindow() {
    if (tipContentControl[0].clientHeight > 286) {
        tipContentControl.height(286);
        tipContentControl.css({ overflow: "hidden" });
    }
}
function showAirportTip() {
    tipContentControl.show();
}
function hideAirportTip() {
    if (tipContentControl) {
        tipContentControl.hide();
    }
}
/** 公用部分 **/
function setPos(inputObj, divBox) {
    divBox.show();
    //    divBox.css("position", "absolute");
    //    var x = inputObj.position().left + "px";
    //    var y = inputObj.position().top + inputObj.height() + "px";
    //    var intX = inputObj.position().left + 360 - $(window).width();
    //    if (intX > 0) {
    //        x = inputObj.position().left - 366 + "px";
    //    }
    //    divBox.css("left", x);
    //    divBox.css("top", y);
    var leftpos = 0;
    var toppos = 0;
    var aTag = inputObj[0];
    do {
        aTag = aTag.offsetParent;
        leftpos += aTag.offsetLeft;
        toppos += aTag.offsetTop;
    } while (aTag.tagName != "BODY" && aTag != null && aTag.offsetParent && aTag.offsetParent.style.position != "relative");
    var x, y;
    if (document.layers) {
        x = inputObj[0].offsetLeft + leftpos;
        y = inputObj[0].offsetTop + toppos + inputObj[0].offsetHeight + 2;
    } else {
        x = inputObj[0].offsetLeft + leftpos;
        y = inputObj[0].offsetTop + toppos + inputObj[0].offsetHeight;
    }
    divBox.css("left", x);
    divBox.css("top", y);
}
function LoadBeforeData() {
    if ($("#txtDeparture").val() != "" || $("#txtDepartureValue").val() != ""
        || $("#txtGoDate").val() != "" || getCookie("departure") == "") {
        return;
    }
    $("#txtDeparture").val(getCookie("departure"));
    $("#txtDepartureValue").val(getCookie("departureCode"));
    $("#txtArrival").val(getCookie("arrival"));
    $("#txtArrivalValue").val(getCookie("arrivalCode"));
    //var airline = getCookie("airline");
    //if (typeof(airline)!="undefined"&&airline != "") $("#txtAirline").val(airline);
    //$("#txtAirlineValue").val(getCookie("airlineCode"));
    $("#txtGoDate").val(getCookie("goDate"));
    if (getCookie("IsGoBack")=="true") {
        $("#txtBackDate").val(getCookie("backDate"));
        $("#radRoundWay").trigger("click");
    }
}

function InitSearchOptions(option) {
    $("#txtDeparture").val(option.departure);
    $("#txtDepartureValue").val(option.departureCode);
    $("#txtArrival").val(option.arrival);
    $("#txtArrivalValue").val(option["arrivalCode"]);
    var airline = option["airline"];
    if (typeof (airline) != "undefined" && airline != "") $("#txtAirline").val(airline);
    $("#txtGoDate").val(option["goDate"]);
    var backData = option["backDate"];
    var cache = [option.departure, option.departureCode, option.arrival, option.arrivalCode, option.goDate];
    var cacheIndex = getSearchIndex();
    
    var currentContent = cache.join("_");
    var lastIndex = cacheIndex -1;
    if (lastIndex < 1) lastIndex = 4;
    if (currentContent != getCookie("Cache" + lastIndex))
    {
        if (currentContent.replace(/\d{4}-\d{2}-\d{2}/, "") != getCookie("Cache" + lastIndex).replace(/\d{4}-\d{2}-\d{2}/,""))
        {
            setCookie("SearchIndex", cacheIndex.toString(), 3 * 24);
            setCookie("Cache" + cacheIndex, currentContent, 3 * 24);
        }
        else {
            setCookie("Cache" + lastIndex, currentContent, 3 * 24);
        }
    }
    if (typeof (backData) != "undefined" && backData!="") {
        $("#txtBackDate").val(option["backDate"]);
        $("#radRoundWay").trigger("click");
    }

}

function LoadHistory(searchIndex) {
    var pointer = searchIndex;
    for (var i = 0; i < 4; i++)
    {
        if (pointer <1) pointer = 4;
        var cache = getCookie("Cache" + pointer);
        pointer--;
        if (cache == "") continue;
        var cacheItems = cache.split('_');
        AppendHistory(cacheItems[0], cacheItems[1], cacheItems[2], cacheItems[3], cacheItems[4]);
    }

    function AppendHistory(departure, departureCode, arrival, arrivalCode, goDate) {
        departure = departure.replace(/\[\w{3}\]/, "");
        arrival = arrival.replace(/\[\w{3}\]/, "");
        $("#recentlyHistory").append("<li><a href='/FlightReserveModule/FlightQueryResult.aspx?source=1&airline=&departure=" + departureCode + "&arrival=" + arrivalCode + "&goDate=" + goDate + "'>" + departure + "-" + arrival + "</a></li>");
    }
}



function getSearchIndex() {
    var cacheIndexStr = getCookie("SearchIndex");
    if (cacheIndexStr == "" || cacheIndexStr == "4")
    {
        return 1;
    } else
    {
       return parseInt(cacheIndexStr, 10)+1;
    }
}

//function getreturnfocus() {
//    $("#txtBackDate").select();
//}

//function getdeparturefocus() {
//    $("#txtGoDate").select();
//}

//function getairlinefocus() {
//    $("#txtAirline").select();
//}

function change() {
    this.City = $("#txtArrival").val();
    this.CityCode = $("#txtArrivalValue").val();
    $("#txtArrival").val($("#txtDeparture").val());
    $("#txtArrivalValue").val($("#txtDepartureValue").val());
    $("#txtDeparture").val(this.City);
    $("#txtDepartureValue").val(this.CityCode);
}

