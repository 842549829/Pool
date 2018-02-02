var airline, departure, arrival, flightDate, goDate, backDate, queryType, prevTripFlightArgs, prevTripPolicyArgs, source, originalOrderId, originalProvider;
var tripType, voyageSerial;
var waitingContainer, contentContainer, flightTitle, flightContent, errorContent;
var flightQueryTime;
var flightValidityMinutes;
var searching = false;
var selectFlightIndex = 0;
var Cities = [
{ name: "北京", val: "PEK" },
{ name: "上海", val: "SHA" },
{ name: "广州", val: "CAN" },
{ name: "深圳", val: "SZX" },
{ name: "成都", val: "CTU" },
{ name: "昆明", val: "KMG" },
{ name: "长沙", val: "CSX" }];
function queryFlights(type, validity, waitingContainerId, contentContainerId, flightTitleId, flightContentId, errorContentId)
{
    var queryString = getRequest();
    flightValidityMinutes = validity;
    queryType = type; // 查询的类型 单程 11， 往返 去程 21 回程 22
    voyageSerial = queryType == "22" ? 2 : 1; // 第几程
    tripType = queryType == "11" ? 1 : 2; // 行程类型 1:单程 2：往返
    airline = queryString.airline; // 航空公司
    departure = queryString.departure; // 出发城市
    arrival = queryString.arrival; // 到达城市
    goDate = queryString.goDate; // 去程日期
    backDate = queryString.backDate; // 回程日期
    prevTripFlightArgs = $("#flightsArgs").val(); // 去程选择的航班信息
    prevTripPolicyArgs = $("#policyArgs").val(); // 去程选择的政策信息
    flightDate = voyageSerial == "2" ? backDate : goDate; // 当前航班日期
    source = queryString.source; // 来源  1:航班查询 4:升舱
    originalOrderId = queryString.orderId; // 用于升舱  升舱原订单号
    originalProvider = queryString.provider;  // 用于升舱 升舱原出票方
    waitingContainer = $("#" + waitingContainerId); // 等待容器
    contentContainer = $("#" + contentContainerId); // 页面信息内容容器
    flightContent = $("#" + flightContentId); // 航班数据容器
    errorContent = $("#" + errorContentId); // 错误容器
    flightTitle = $("#" + flightTitleId); // 航班日期标题容器
    if (searching) return;
    searching = true;
    execFlightQuery();
    var minDate = voyageSerial == 2 ? queryString.goDate : Date.format(new Date(), 'yyyy-MM-dd'); // 航班日期标题处，可选择的最小日期 如果是第一程，则是当前日期，第二程时，则是第一程的航班日期
    createFlightQueryDateTitle(flightDate, minDate);
    loadFlightLowerPrice(queryString.departure, queryString.arrival);
    ChangeTitleDate(flightDate);
}
function GetRandomCitys() {
    var r =  Math.round(Math.random() * 3);
    var result = Cities.slice(r, r + 3);
    for (var i = r; i < r+3; i++) {
        if (departure == Cities[i].val || arrival == Cities[i].val)
        {
            var changeindex = i-r-3;
            if(changeindex<0) changeindex+=7;
            result[i - r] = Cities[changeindex];
        }
    }
    return result;
}

function execFlightQuery()
{
    waitingContainer.show();
    //contentContainer.hide();
    var method;
    var parameters;
    if (queryType == "21")
    {
        method = "QueryRTFirstTripFlights";
        parameters = JSON.stringify({ "airline": airline, "departure": departure, "arrival": arrival, "flightDate": flightDate });
    } else if (queryType == "22")
    {
        method = "QueryRTSecondTipFlights";
        parameters = JSON.stringify({ "firstTripPolicyInfoArgs": prevTripPolicyArgs, "firstTripFlightArgs": prevTripFlightArgs, "flightDate": flightDate });
    } else
    {
        method = "QueryOWFlights";
        parameters = JSON.stringify({ "airline": airline, "departure": departure, "arrival": arrival, "flightDate": flightDate });
    }
    sendPostRequest("/FlightHandlers/FlightQuery.ashx/" + method, parameters, function (data)
    {
        if (data.length == 0)
        {
            waitingContainer.hide();
            flightContent.hide();
            var otherCity = GetRandomCitys();
            var otherHtml = [];
            var from = { name: $("#txtDeparture").val().replace(/\[\w{3}\]/, ""), val: departure };
            for (var Cindex = 0; Cindex < 3; Cindex++)
            {
                var item = otherCity[Cindex];
                otherHtml.push("<a href='");
                otherHtml.push("/FlightReserveModule/FlightQueryResult.aspx?source=1&departure=", from.val, "&arrival=", item.val, "&goDate=", flightDate);
                otherHtml.push("'>", from.name, "-", item.name, "</a>");
            }
            errorContent.html(" <div class=\"divErrorBox\">      <div class=\"divNoflightPic\">          <p>悲剧了，您查询的航线“没有直达航班”</p>          <p>              看看这几个：              <span id=\"other\">" + otherHtml.join("") + "</span>          </p>      </div>  </div> ").show();
            searching = false;
        } else
        {   
            bindFlightsData(data, false);
            matchPolicies();
            flightQueryTime = new Date();
            waitingContainer.hide();
            //contentContainer.show();
            errorContent.hide();
            flightContent.show();
        }
        //        var flightCountShowControl = $("#flightsCount");
        //        if (flightCountShowControl) flightCountShowControl.html(data.length);
    }, function (error)
    {
        waitingContainer.hide();
        //contentContainer.show();
        flightContent.hide();
        if (typeof (error.responseText)!='undefined' && error.responseText.indexOf("没有直达航班") > -1)
        {
            var otherCity = GetRandomCitys();
            var otherHtml = [];
            var from = { name: $("#txtDeparture").val().replace(/\[\w{3}\]/, ""), val: departure };
            for (var Cindex = 0; Cindex < 3; Cindex++)
            {
                var item = otherCity[Cindex];
                otherHtml.push("<a href='");
                otherHtml.push("/FlightReserveModule/FlightQueryResult.aspx?source=1&departure=", from.val, "&arrival=", item.val, "&goDate=", flightDate);
                otherHtml.push("'>", from.name, "-", item.name, "</a>");
            }
            errorContent.html(" <div class=\"divErrorBox\">      <div class=\"divNoflightPic\">          <p>悲剧了，您查询的航线“没有直达航班”</p>          <p>              看看这几个：              <span id=\"other\">" + otherHtml.join("") + "</span>          </p>      </div>  </div> ").show();



            //errorContent.html('航班查询失败<br />原因：' + error.responseText).show();
        } else
        {
            errorContent.html("<div class=\"divErrorBox\">     <div class=\"divErrorPic\">         <p>查询失败了，网络神马的也太不给力了</p>         <a class=\"refresh\" href=\"javascript:ReSeach()\">重 试</a><span>您也可以<a target='_blank' href=\"/About/OnLineService.aspx\">报告管理员</a></span>     </div><input type='hidden' value='" + error.responseText + "' /> </div>").show();
        }

    searching = false;
    });
    var flightDateShowControl = $("#flightDate");
    if (flightDateShowControl) flightDateShowControl.html(flightDate);
}
function SearchFlight(departure, arrival, date){//转到航班查询页面查询航班
    var now = new Date();
    var searchUrl = "/FlightReserveModule/FlightQueryResult.aspx?source=1&departure={0}&arrival={1}&goDate={2}-{3}";
    var year = now.getFullYear();
    var selectedDateNum = /^(\d{2})-(\d{2})$/.exec(date);
    if (parseInt(selectedDateNum[1], 10) < now.getMonth() + 1
                || parseInt(selectedDateNum) == now.getMonth() + 1 && parseInt(selectedDateNum[2], 10) < now.getDate())
    {
        year++;
    }
    location.href = searchUrl.format(departure, arrival, year, date);
}


function clearFlightList() { $(".FlightTable,.subFlightList", flightContent).remove(); }
function ReSeach() {$("#btnQueryFlight").trigger("click"); }

function bindFlightsData(data, showPrice)
{
    var flightsHtml = new Array();
    var availableFlightCount = 0;
    clearFlightList();
    $.each(data, function (index, item)
    {
        if (showPrice && item.LowerPrice < 0) return; //从查询到的航班中排除已经销售完毕的航班
        flightsHtml.push("<table class='FlightTable'><tr> <td class=\"flightCompany\">  <span class=\"flightContent\">");
        flightsHtml.push("<span class='flag flag_",item.AirlineCode,"'>&nbsp;</span>");
        flightsHtml.push("<strong>", item.AirlineName, item.AirlineCode, item.FlightNo, "</strong> ");
        flightsHtml.push("<br> <span class=\"fontgray\"> ", "机型", "<span class='obvious-a'>", item.Aircraft, "</span></span></span>");
        if (item.IsStop) flightsHtml.push(BuildStopInfo(item.AirlineCode, item.FlightNo, item.FlightDate));
        flightsHtml.push(" </td>");
        flightsHtml.push("<td class=\"startTime\"> <b>", item.Departure.Time, "</b> <br> <span class=\"fontgray\"> ", item.Arrival.Time, (item.DaysInterval == 1 ? " <a class=\"nextDay tooltip\" href=\"javascript:void(0)\"> 第2天 <span> 到达时间为第2天<br> " + Date.addPart(Date.fromString(item.FlightDate), "d", 1).format("yyyy-MM-dd") + " " + item.Arrival.Time + "</span> </a>" : ""), " </span> </td>");
        flightsHtml.push(" <td class=\"airport\"> ", item.Departure.City, item.Departure.Name,item.Departure.Terminal, " <br> ", item.Arrival.City, item.Arrival.Name, item.Arrival.Terminal, " </td> ");
        if (showPrice)
        {
            if (item.LowerPrice > -1)
            {
                flightsHtml.push("<td class='flightPrice'> <span class='fontBlodRed'>￥", fillZero(item.LowerPrice), "</span></td>");
                flightsHtml.push("<td class=\"flightOperate\"><a id='hash", index, "'></a> <input type='hidden' value='" + constructFlightsMainArgs(item) + "'/>");
                flightsHtml.push(" <input type='button' value='选择' id=\"flightBtn2\" class=\"flightBtn\"  onclick='showBunks($(this),\"", item.AirlineCode, "\",\"", item.FlightNo, "\",\"" + item.AirportFee + "\",\"" + item.BAF + "\",", index, ")'/>");
                flightsHtml.push("<div class='subBoxTitle' style='display:none'><input type='button' class='flightBtn1' value='隐藏' onclick='hideBunks($(this))'/></div></td>");
            } else
            {
                flightsHtml.push("<td class=\"flightPrice\"> <span class=\"fontBlodRed\">销售完毕</span> </td><td class=\"flightOperate\"></td>");
            }
        } else
        {
            flightsHtml.push("<td class='flightPrice'></td><td class='flightOperate'></td>");
        }
        flightsHtml.push("</tr><table>");
        availableFlightCount++;
    });
    if (showPrice)
    {

        $("#flightsCount").html(availableFlightCount);
        searching = false;
    }
    flightContent.append(flightsHtml.join(''));
}
function BuildStopInfo(airLine, flightNo, flightDate)
{
    return "<a class=\"flightAction tooltip\" href=\"#\" onmouseover='ShowTip(this,\"" + airLine + flightNo + "\",\"" + flightDate + "\")'> 经停 <span> 加载中……</span> </a>";
}
function ShowTip(sender, flight, flightDate)
{
    var that = $(sender);
    if (typeof that.data("Loaded") != "undefined")
    {
        //that.find("span").show();
        return;
    }
    that.data("Loaded", true);
    sendPostRequest("/FlightHandlers/FlightQuery.ashx/GetFlightStopInfo", JSON.stringify({ flightNo: flight, flightDate: flightDate }), function (rsp)
    {
        if (rsp.IsSuccess)
        {
            $("span", that).html(rsp.Obj);
            //that.find("span").show();
        }
    });
}

function matchPolicies()
{
    var method;
    var parameters;
    if (queryType == "22")
    {
        method = "MatchRTSecondTripPolicy";
        parameters = JSON.stringify({ "firstTripFlightArgs": prevTripFlightArgs });
    } else if (queryType == "21")
    {
        method = "MatchRTFirstTripPolicy";
        parameters = JSON.stringify({ "backDate": backDate });
    } else
    {
        method = "MatchOWPolicy";
        parameters = '';
    }
    sendPostRequest("/FlightHandlers/FlightQuery.ashx/" + method, parameters, function (data)
    {
        bindFlightsData(data, true);
    }, function (error)
    {
        alert("匹配最低价出错\n原因:" + JSON.parse(error.responseText));
    });
}
function showBunks(sender, airlineCode, flightNo, airPortFee, BAF,index)
{
    $(".flightBtn1:visible").trigger("click");  //折叠其他的航班仓位
    if (index!=0&&selectFlightIndex < index) {
        var hash = sender.siblings("a").attr("id");
        location.href = location.href + "#" + hash;
    }
    selectFlightIndex = index;
    sender.hide();
    var nextRow = sender.parent().parent().parent().parent().next();
    if (nextRow.hasClass("flightContents"))
    {
        nextRow.show().find("div.table-sel").show().find(":hidden").show();
        sender.next().show();
    } else
    {
        var method;
        var parameters;
        if (queryType == "22")
        {
            method = "QueryRTSecondTripBunks";
            parameters = JSON.stringify({ "firstTripFlightArgs": prevTripFlightArgs, "airline": airlineCode, "flightNo": flightNo });
        } else if (queryType == "21")
        {
            method = "QueryRTFirstTripBunks";
            parameters = JSON.stringify({ "airline": airlineCode, "flightNo": flightNo, "backDate": backDate });
        } else
        {
            method = "QueryOWBunks";
            parameters = JSON.stringify({ "airline": airlineCode, "flightNo": flightNo });
        }
        sendPostRequest("/FlightHandlers/FlightQuery.ashx/" + method, parameters, function (data)
        {
            bindBunks(sender, data, airlineCode, airPortFee, BAF);
            sender.next().show();
            sender.parent().parent().parent().parent().next().addClass("flightContents");
        }, function (error)
        {
            alert(error.responseText);
            sender.show();
            sender.next().hide();
        });
    }
}
function bindBunks(sender, data, airlineCode, airPortFee, BAF)
{
    var bunkHtml = new Array();
    var currentFlightInfo = sender.parent().parent().parent().parent();
    bunkHtml.push("<div id=\"subFlightList1\" class=\"subFlightList\" style=\"display: block;\"> <div class=\"sunFlightinfo\">  <span class=\"bprice\"> 本航班民航基金：", airPortFee, "元 </span> <span class=\"oprice\"> 燃油附加费：", BAF, "元 </span> </div> <div class=\"subMain\"> <table class=\"subtitle\"> <tbody><tr> <th class=\"cabin\">舱位</th> <th class=\"ticketPrice\">票面价格</th> <th class=\"tickets\">剩余票数</th> <th class=\"rule\">订票规则</th> <th class=\"mainPrice\">结算价格(不含税费)</th> <th class=\"return\">返点</th> <th class=\"operate\"></th> </tr> </tbody></table>");
    $.each(data, function (index, item)
    {
        var policyType = item.Policy.Type;
        var bunkType = item.BunkType;
        bunkHtml.push("<table class=\"subTable\"><tbody><tr>");
        if (policyType == 8)
        {
            bunkHtml.push("<td class=\"cabin\" style=\"color:#255fb1\"><strong>", item.Description, "</strong>");
        } else
        {
            bunkHtml.push("<td class=\"cabin\">", item.Description);
        }
        if (item.Code && item.Code.length > 0)
        {
            bunkHtml.push("(", item.Code, ")");
        }
        bunkHtml.push("</td>");
        bunkHtml.push("<td class=\"ticketPrice\">", item.ShowPrice ? "￥" + item.Fare.toString() : "出票后可见");
        var isGeneralType = (policyType == 1 || policyType == 2) && (bunkType == 0 || bunkType == 1); //普通政策或者默认政策 并且是 明折明扣舱
        //if (isGeneralType)
        if (item.RenderDiscount != "")
        {
            bunkHtml.push(" (" + parseInt(item.RenderDiscount * 100) + "折)");
        }
        bunkHtml.push("</td> ");
        bunkHtml.push("<td class=\"tickets\">", (item.SeatCount > 9 ? "≥9" : item.SeatCount), "张</td>");
        bunkHtml.push("<td class=\"rule\"> <a class=\"flightEI\" href=\"javascript:void(0)\"> 退改签 <span style='display:none'>", item.EI, "</span> </a> </td>");
        bunkHtml.push("<td class=\"mainPrice\"> <span class=\"fontBlodRed\">￥", fillZero(item.Amount), "</span>");
        bunkHtml.push("</td>");
        bunkHtml.push(" <td class=\"return\"> <span ", item.Policy.Type != 16 ? "class=\"fan\"" : "class=\"wufan\"", ">", (item.Policy.Type != 16 ? (parseInt(item.Rebate * 1000) / 10).toString() : "无 返"), item.Policy.Type != 16 ? "%" : "", "</span> </td> <td class=\"operate\">");
        if (item.Amount > 0)
        {
            var flightInfo = sender.prev().val();
            if (queryType == "22")
            {
                // 往返回程
                bunkHtml.push("<input type=\"button\" class=\"subFlightBtn\" value=\"预&nbsp;订\" onclick=\"bookingRTBack('");
                bunkHtml.push(constructPolicyArgs(item) + "','" + constructFlightsReserveArgs(flightInfo, item) + "')\"/>");
            } else if (queryType == "21")
            {
                // 往返去程
                bunkHtml.push("<input type=\"button\" class=\"subFlightGoBtn\" value=\"查询回程\" onclick=\"bookingRTGo('");
                bunkHtml.push(constructPolicyArgs(item) + "','" + constructFlightsReserveArgs(flightInfo, item) + "','" + airlineCode + "')\"/>");
            } else
            {
                // 单程
                bunkHtml.push("<input type=\"button\" class=\"subFlightBtn\" value=\"预&nbsp;订\" onclick=\"bookingOW('");
                bunkHtml.push(constructPolicyArgs(item) + '\',\'' + constructFlightsReserveArgs(flightInfo, item) + "','" + item.ShowPrice + "')\"/>");
            }
        }
        bunkHtml.push(" </td></tr></tbody></table> ");
    });
    currentFlightInfo.after($(bunkHtml.join('')));
}
function constructFlightsMainArgs(flightInfo)
{
    return voyageSerial + '|' + flightInfo.AirlineCode + '|' + flightInfo.AirlineName + '|' + departure + '|' + flightInfo.Departure.Name
        + '|' + flightInfo.Departure.City + '|' + flightInfo.Departure.Terminal + '|' + flightDate + ' ' + flightInfo.Departure.Time + '|' + arrival
        + '|' + flightInfo.Arrival.Name + '|' + flightInfo.Arrival.City + '|' + flightInfo.Arrival.Terminal + '|' + flightDate + ' ' + flightInfo.Arrival.Time + '|' + flightInfo.DaysInterval
        + '|' + flightInfo.FlightNo + '|' + flightInfo.Aircraft + '|' + flightInfo.YBPrice + '|' + flightInfo.AirportFee + '|' + flightInfo.BAF + '|' + flightInfo.AdultBAF + '|' + flightInfo.ChildBAF;
}
function constructFlightsReserveArgs(flightInfo, bunkInfo)
{
    return flightInfo + '|' + bunkInfo.Code + '|' + bunkInfo.BunkType + '|' + bunkInfo.SeatCount + '|' + bunkInfo.Description + '|' + bunkInfo.Discount +
        '|' + bunkInfo.EI.replace(/</g,'lt;').replace(/>/g,'rt;').replace(/\?|&/g,'') + '|' + bunkInfo.Fare + '|' + bunkInfo.Rebate + '|' + bunkInfo.Amount + '|' + bunkInfo.SuportChild+'|'+bunkInfo.RenderDiscount;
}
function constructPolicyArgs(bunkInfo)
{
    return bunkInfo.Policy.Id + '|' + bunkInfo.Policy.Owner + '|' + bunkInfo.Policy.Type + '|' + bunkInfo.Policy.CustomerResource + '|' + bunkInfo.Amount;
}
function checkIsExpired()
{
    return Date.diff(new Date(), flightQueryTime, 'm') > flightValidityMinutes;
}
function bookingOW(policyArgs, flightsArgs,isShowPrice)
{
    if (checkIsExpired())
    {
        showFlightExpiredMessage();
    } else
    {
        var form = $("form[id='flightForm']");
        form.attr("action", "/FlightReserveModule/FillPassenger.aspx?source=" + source + getAdditionalParameterInfo()+"&ShowPrice="+isShowPrice);
        $("#flightsArgs").val(flightsArgs);
        $("#policyArgs").val(policyArgs);
        form.trigger("submit");
    }
}
function bookingRTGo(policyArgs, flightsArgs, airlineCode)
{
    if (checkIsExpired())
    {
        showFlightExpiredMessage();
    } else
    {
        var form = $("form[id='flightForm']");
        form.attr("action", "/FlightReserveModule/FlightQueryBackResult.aspx?source=" + source + "&airline=" + airlineCode + '&departure=' + arrival
            + '&arrival=' + departure + '&goDate=' + flightDate + '&backDate=' + backDate + getAdditionalParameterInfo());
        $("#flightsArgs").val(flightsArgs);
        $("#policyArgs").val(policyArgs);
        form.trigger("submit");
    }
}
function bookingRTBack(policyArgs, backFlightsArgs)
{
    if (checkIsExpired())
    {
        showFlightExpiredMessage();
    } else
    {
        var form = $("form[id='flightForm']");
        form.attr("action", "/FlightReserveModule/FillPassenger.aspx?source=" + source + getAdditionalParameterInfo());
        $("#flightsArgs").val(prevTripFlightArgs + '$' + backFlightsArgs);
        $("#policyArgs").val(policyArgs);
        form.trigger("submit");
    }
}
function getAdditionalParameterInfo()
{
    if (source == "4")
    {
        return "&orderId=" + originalOrderId + "&provider=" + originalProvider;
    } else
    {
        return '';
    }
}
function showFlightExpiredMessage()
{
    alert("航班数据已过期，请重新查询");
}
function hideBunks(sender)
{
    sender.parent().hide();
    var bunksInfo = sender.parent().parent().parent().parent().parent().next();
    if (bunksInfo.hasClass('flightContents'))
    {
        bunksInfo.hide();
        sender.parent().prev().show();
    }
}
function loadFlightLowerPrice(departure, arrival)
{
    var startDate = $("li:first", flightTitle).attr('name');
    var endDate = $("li:last", flightTitle).attr('name');
    var parameters = JSON.stringify({ "departure": departure, "arrival": arrival, "startDate": startDate, "endDate": endDate });
    sendPostRequest("/FlightHandlers/Recommend.ashx/QueryLowerFares", parameters, function (data)
    {
        $.each(data, function (index, item)
        {
            $("li[name=" + item.Date + "]", flightTitle).find(".price").html('￥' + fillZero(item.Fare));
        });
    });
}
function createFlightQueryDateTitle(flightDate, minDate)
{
    var today = new Date();
    var dateDiff;
    if (minDate)
    {
        dateDiff = Date.diff(Date.fromString(flightDate), Date.fromString(minDate), 'd');
    } else
    {
        dateDiff = Date.diff(Date.fromString(flightDate), today, 'd');
        minDate = Date.format(today, 'yyyy-MM-dd');
    }
    var startDate = Date.addPart(Date.fromString(flightDate), 'd', -(dateDiff >= 3 ? 3 : dateDiff));
    drawDateTitle(startDate, minDate);
}
function drawDateTitle(startDate, minDate)
{
    var currentDate;
    var currentDateString;
    var titleHtml = new Array();

    titleHtml.push("<div id=\"flightChooseBottom\"><input type='button' onclick=\"prevWeek('" + Date.format(startDate, 'yyyy-MM-dd') + "','" + minDate + "')\" id=\"leftBtn\"></button><ul>");
    for (var i = 0; i < 7; i++)
    {
        currentDate = Date.addPart(startDate, 'd', i);
        currentDateString = Date.format(currentDate, 'yyyy-MM-dd');
        titleHtml.push("<li name='", currentDateString, "'><a href='");
        titleHtml.push(currentDateString != flightDate ? ("javascript:changeFlightDate(\"" + currentDateString + "\",\"" + minDate + "\")") : "javascript:void(0)");
        titleHtml.push("' " + (currentDateString == flightDate ? "class=\"liselected\"" : "") + "><span class=\"fontBlue\">");
        titleHtml.push(Date.format(currentDate, 'MM-dd') + " " + Date.chineseWeeks[currentDate.getDay()] + "</span><br> <span class=\"fontRed price\"></span></a></li>");
    }
    titleHtml.push("</ul><input type=\"button\" onclick=\"nextWeek('" + currentDateString + "','" + minDate + "')\" id=\"rightBtn\"></button></div>");
    flightTitle.html(titleHtml.join(''));
}
function changeFlightDate(currentDate, minDate)
{
    errorContent.hide();
    if (searching) return;
    searching = true;
    clearFlightList();
    ChangeTitleDate(currentDate);
    flightDate = currentDate;
    ReInitSearchOption(currentDate);
    execFlightQuery();
    createFlightQueryDateTitle(currentDate, minDate);
    loadFlightLowerPrice(departure, arrival);
}
function ChangeTitleDate(currentDate)
{
    currentDate = Date.fromString(currentDate);
    $("#pickDate").text(Date.format(currentDate, ' MM月dd日 ') + Date.chineseWeeks[currentDate.getDay()]);
}

function ReInitSearchOption(date)
{
    if (queryType != 22)
    {
        $("#txtGoDate").val(date);
        setCookie("goDate", date, 8);
    } else
    {
        $("#txtBackDate").val(date);
        setCookie("backDate", date, 8);
    }
}

function prevWeek(currentDate, minDate)
{
    var startDate = Date.addPart(Date.fromString(currentDate), 'd', -7);
    var minDateModel = Date.fromString(minDate);
    if (startDate < minDateModel)
    {
        startDate = minDateModel;
    }
    drawDateTitle(startDate, minDate);
    loadFlightLowerPrice(departure, arrival);
}
function nextWeek(currentDate, minDate)
{
    var startDate = Date.addPart(Date.fromString(currentDate), 'd', 1);
    drawDateTitle(startDate, minDate);
    loadFlightLowerPrice(departure, arrival);
}

