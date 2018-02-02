var result = {};
var orderId;
var ticketPrice, airPortFee, bAF, fillDate, insureFee;
var ticketNos;
$("#btnSearch").click(function () {
    initialSatus();
    var txtTicketNOInput = $.trim($("#txtTicketNOInput").val());
    if (txtTicketNOInput.length <= 0) {
        alert("导入内容不能为空");
        return false;
    }
    var selectType = $("#dropSelect option:selected").val();
    if (selectType == "1") {
        sendPostRequest("/OrderHandlers/Order.ashx/SearchItinerary", JSON.stringify({ ticketNo: txtTicketNOInput }), function (rsp) {
            BindPassengerInfo(rsp);
            BindFlightInfo(rsp.Flights);
            BindFeeInfo(rsp);
            CalcFee();
            $("#ulPassengers").empty();
        }, function (e) {
            alert(e.responseText);
        });
    } else {
        var reg = new RegExp("^[0-9]{13}$");
        if (!reg.test(txtTicketNOInput)) {
            alert("订单号格式错误");
            return;
        }
        orderId = txtTicketNOInput;
        sendPostRequest("/OrderHandlers/Order.ashx/SearchPassengerName", JSON.stringify({ orderId: txtTicketNOInput }), function (data) {
            bindPassenger(data);
            clearText();
        }, function (e) {
            alert(e.responseText);
        });
    }
});
//绑定乘机人
function bindPassenger(data) {
    var html = new Array();
    for (var i = 0; i < data.length; i++) {
        html.push("<li class='" + (i == 0 ? "curr" : "") + "'>");
        html.push(data[i]);
        html.push("</li>");
    }
    if (data.length > 1) {
        html.push("<span>合并打印告知单</span>");
    }
    document.getElementById("ulPassengers").innerHTML = html.join('');
    if (data.length > 0) {
        var passengersName = data[0];
        searchItineraryByOrder(passengersName);
    }
}
function BindPassengerInfo(data) {
    $("#PassengerName").val(data.Name);
    $("#Credentials").val(data.Credentials);
    $("#TicketNos").val(data.TicketNos);
    setTicketNos(data.TicketNos);
}
//获取第一个票号
var firstNos = false;
function setTicketNos(str) {
    if (firstNos === false) {
        ticketNos = str;
        firstNos = true;
    }
}

function BindFlightInfo(data) {
    for (var i = data.length; i < 5; i++) {
        data.push({});
    }
    var firstLine = true;
    $(".data").remove();
    $("#FlightsInfoTmpl").tmpl(data, { getText: function () {
        if (firstLine)  {
            firstLine = false;
            return "自";
        } else {
            return "至";
        }
    }
    }).insertAfter("#thead");
}

function BindFeeInfo(data) {
    $("#txtTicketPrice").val(data.TicketPrice);
    $("#txtAirPortFee").val(data.AirPortFee);
    $("#txtBAF").val(data.BAF);
    $("#InputDate").val(data.FillDate);
    $("#inputer").val(data.inputer);
    ticketPrice = data.TicketPrice;
    airPortFee = data.AirPortFee;
    bAF = data.BAF;
    fillDate = data.FillDate;
    insureFee = data.insureFee;
}

function CalcFee() {
    var total = 0;
    if ($.trim($("#txtTicketPrice").val()) != "") {
        total += parseFloat($("#txtTicketPrice").val());
    }
    if ($.trim($("#txtAirPortFee").val()) != "") {
        total += parseFloat($("#txtAirPortFee").val());
    }
    if ($.trim($("#txtBAF").val()) != "") {
        total += parseFloat($("#txtBAF").val());
    }
    if ($.trim($("#insureFee").val()) != "") {
        total += parseFloat($("#insureFee").val());
    }
    if ($.trim($("#txtOtherFee").val()) != "") {
        total += parseFloat($("#txtOtherFee").val());
    }
    $("#txtTotal").val(Round(total, 2));
}

function PrintItinerary() {
    $("#btns,#Header,#TicketInputer,#ulPassengers,#Tips").hide();
    $("body").addClass("print");
    window.print();
    $("#ulPassengers li").each(function () {
        if ($.trim($(this).text()) == $.trim($("#PassengerName").val()) && !$("#ulPassengers span").hasClass("curr")) {
            $(this).remove();
            firstNos = false;
            if ($("#ulPassengers li").size() == 1) { $("#ulPassengers span").remove(); }
            return false;
        }
    });
    if ($("#ulPassengers li:first").length <= 0) {
        clearText();
    } else if (!$("#ulPassengers span").hasClass("curr")) {
        searchItineraryByOrder($("#ulPassengers li:first").addClass("curr").text());
    }
    $("#btns,#TicketInputer,#Header,#ulPassengers,#Tips").show();
    $("body").removeClass("print");
}

$(function () {
    var query = location.search;
    query = decodeURI(query);
    if (/OrderId=(\d{13})&Passenger=(.*)/.test(query)) {
        orderId = RegExp.$1;
        var passengerName = RegExp.$2;
        var data = passengerName.split(",");
        if (passengerName != "undefined" && data[0] != "undefined") {
            bindPassenger(data);
        }
    }
    $("#ulPassengers li").live("click", function () {
        initialSatus();
        if (orderId) {
            $(".curr").removeClass("curr");
            $(this).addClass("curr");
            searchItineraryByOrder($(this).text());
        }
    });
    $("#ulPassengers span").live("click", function () {
        $(".curr").removeClass("curr");
        $(this).addClass("curr");
        $("#divPassengers").hide();
        var passengers = new Array();
        $("#ulPassengers li").each(function () { passengers.push($.trim($(this).text())); });
        var value = passengers.join("、");
        var length = value.length;
        var rows = parseInt(length / 70) + 1;
        $("#textPassengerName").val(value).show().attr("rows", rows);
        if (ticketPrice) {
            $("#txtTicketPrice").val(ticketPrice * passengers.length);
        }
        if (airPortFee) {
            $("#txtAirPortFee").val(airPortFee * passengers.length);
        }
        if (bAF) {
            $("#txtBAF").val(bAF * passengers.length);
        }
        if (insureFee) {
            $("#insureFee").val(insureFee * passengers.length);
        }
        CalcFee();
        if (ticketNos) {
            var strNos = ticketNos.substring(0, 14);
            var intNos = parseFloat(ticketNos.substring(12));
            $("#TicketNos").val(strNos + "-" + (intNos + passengers.length - 1));
        }
    });
});
//根据订单号和姓名查询行程单
function searchItineraryByOrder(passengersName) {
    sendPostRequest("/OrderHandlers/Order.ashx/SearchItineraryByOrder", JSON.stringify({ orderId: orderId, passengerName: passengersName }), function (rsp) {
        BindPassengerInfo(rsp);
        BindFlightInfo(rsp.Flights);
        BindFeeInfo(rsp);
        CalcFee();
    }, function (e) {
        alert(e.responseText);
    });
}
//清除文本
function clearText() {
    $("#ticket-table :text").each(function () { $(this).val(""); });
}
function initialSatus(){
    $("#textPassengerName").hide();
    $("#divPassengers").show();
}