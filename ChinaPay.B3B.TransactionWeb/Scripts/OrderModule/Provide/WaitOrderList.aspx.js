function bindData(data) {
    var html = new Array();
    $.each(data.Orders, function (index, item) {
        html.push("<tr class='" + (item.TodaysFlight ? "redBackground" : "") + "'>");
        html.push("<td><a href='./OrderDetail.aspx?id=" + item.OrderId + "' class='obvious-a'>" + item.OrderId);
        html.push("</a><br/>");
        html.push(item.ProductType + "<span class='" + (item.IsChildTicket ? "greenBolder'>" : "Bolder'>"));
        html.push("(" + item.PassengerType + ")</span>");
        html.push("</td><td><span class='obvious'>" + item.PNR + "</span></td>");
        html.push("<td>" + item.AirportPair + "</td>");
        html.push("<td>" + item.Passenger + "</td>");
        html.push("<td>" + item.Fare + "<br/>" + item.AirportFee + "/" + item.BAF + "</td>");
        html.push("<td>" + item.SettleAmount + "<br/>" + item.Rebate + "/" + item.Commission + "</td>");
        html.push("<td>" + item.ProducedTime + "</td>");
//        html.push("<td>" + item.PayTime + "</td>");
        html.push("<td><span class='purchaseTypes" + (item.PurchaseIsBother ? " brother'>" : " lower'>") + item.Relation + "</span>");
        html.push(item.RemindIsShow ? ("<br/><a href='javascript:void(0);' class='obvious urgent_btn urgent_order' content='" + item.RemindContent + "'>采购催单</a>") : "");
        html.push("</td>");
        html.push("<td>" + item.Status + "</td>");
        html.push("<td>" + item.OfficeNum + item.TicketType + "<br/>" + (item.IsEmergentOrder ? "<a id='" + item.OrderId + "' href='javascript:void(0);' class='tips_btn urgent'>紧急</a>" : ""));
        html.push((item.ETDZTime != 0.1 ? "<span class='waitedTime'>" + item.ETDZTime + "</span>" : ""));
        html.push("</td>");
        html.push("<td><span class='LockInfo'>" + item.LockInfo + "</span>");
        html.push(item.IsPaidForETDZ ? ("<a href='javascript:void(0);' orderId ='" + item.OrderId + "' class='obvious-a ETDZ'>出票</a><br/><a href='ETDZ.aspx?id=" + item.OrderId + "' class='obvious-a'>详情</a><br/><a href='javascript:void(0);' orderId ='" + item.OrderId + "' class='obvious-a denyETDZ'>拒绝出票</a>") : ("<a href=\"javascript:Go('Supply.aspx','" + item.OrderId + "');\" class='obvious-a b font-c'>处理</a>"));
        html.push("</td></tr>");
        html.push(item.IsPaidForETDZ?"<tr class='empty-tr PreOrder' style='display:none;'></tr><tr class='empty-tr' style='display:none;'><td colspan='12'></td></tr>":"");
    });
    if (data.Orders.length > 0) {
        $("#emptyInfo").hide();
        $("#data-list").show();
        $("#data-list table tbody").html(html.join(""));
        var urg = $(".urgent_orderDiv");
        $(".urgent_order").mouseenter(function () {
            $(".urgent_content p").html($(this).attr("content"));
            urg.removeClass("hidden");
            urg.css("left", $(this).offset().left - 100);
            urg.css("top", $(this).offset().top + 12);
        }).mouseleave(function () {
            urg.addClass("hidden");
        });
        html = null
    } else {
        $("#emptyInfo").show();
        $("#data-list").hide();
    }
    $(".waitedTime").each(function () {
        var that = $(this);
        var waitedMininute = parseFloat($.trim(that.text()));
        if (waitedMininute <= 5) {
            that.css("color", "#009900");
        } else if (waitedMininute <= 10) {
            that.css("color", "#ff8c01");
        } else {
            that.css("color", "#ed1c24");
        }
    });
    $("#data-list table:not(.ClearAlternate) tr:nth-child(even)").addClass("alternate");
    drawPagination($("#divPagination"), data.Pagination.PageIndex, data.Pagination.PageSize, data.Pagination.RowCount, queryOrders);
    //CalcTime(1);
}
function queryOrders(pageIndex, pageSize) {
    if (!pageIndex) pageIndex = 1;
    pageSize = pageSize || 10;
    var condition = getQueryOrdersCondition();
    var pagination = getQueryOrdersPagination(pageIndex, pageSize);
    var parameters = JSON.stringify({ "pagination": pagination, "orderQueryCondition": condition });
    if (IsFirstLoad) { //在第一次查询时使用上一次的查询条件加载数据
        var lastData = getCookie("WaitOrderListNew");
        if (lastData != "") {
            parameters = lastData;
            LoadPageStates(lastData);
        }
        IsFirstLoad = false;
    } else {
        if (parameters != "") setCookieCurrentPath("WaitOrderListNew", parameters, 1);
    }

    sendPostRequest("/OrderHandlers/Order.ashx/QueryProvideWaitOrderList", parameters, function (data) {
        bindData(data);
    }, function (e) {
        if (e.statusText == "timeout") {
            alert("服务器忙");
        } else {
            alert(e.responseText);
        }
    });
    return false;
}
function getQueryOrdersCondition() {
    var dateReg = /(\d{4}-\d{1,2}-\d{1,2}).*/;
    var startDate = $("#txtStartDate").val();
    dateReg.test(startDate);
    startDate = RegExp.$1;
    var endDate = $("#txtEndDate").val();
    dateReg.test(endDate);
    endDate = RegExp.$1;
    return { "ProducedDateRange": { "Lower": $("#txtStartDate").val(), "Upper": $("#txtEndDate").val() },
        "Passenger": $.trim($("#txtPassenger").val()), "PNR": $.trim($("#txtPNR").val()), "OrderId": $.trim($("#txtOrderId").val()),
        "Carrier": $("#ddlCarrier>option:selected").val(),
        "OfficeNo": $("#ddlOfficeNumber").val(),
        "ProviderProductType":$("#ddlProduct").val(),
        "Status": $("#ddlStatus").val()
    };
}
function getQueryOrdersPagination(pageIndex, pageSize) {
    return { "PageSize": pageSize, "PageIndex": pageIndex, "GetRowCount": true };
}
window.onload = function () {

    var pageSize = 10;
    $("#txtOrderId").OnlyNumber().LimitLength(13);
    queryOrders(1, 10);
    document.getElementById("btnSerach").onclick = function () {
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryOrders(1, pageSize);
    };
};
function LoadPageStates(parmater) {
    var condition;
    eval("condition = " + parmater);
    if (condition != "undefined") {
        doc("txtStartDate").value = condition.orderQueryCondition.ProducedDateRange.Lower;
        doc("txtEndDate").value = condition.orderQueryCondition.ProducedDateRange.Upper;
        doc("txtPassenger").value = condition.orderQueryCondition.Passenger;
        doc("txtPNR").value = condition.orderQueryCondition.PNR;
        doc("txtOrderId").value = condition.orderQueryCondition.OrderId;
        doc("ddlCarrier").value = condition.orderQueryCondition.Carrier;
        doc("ddlOfficeNumber").value = condition.orderQueryCondition.OfficeNo;
        doc("ddlProduct").value = condition.orderQueryCondition.ProviderProductType;
        doc("ddlStatus").value = condition.orderQueryCondition.Status;
    }
}
function doc(id) { return document.getElementById(id); }
function CalcTime() {
    if (arguments.length == 0) {
        ServerTime = Date.addPart(ServerTime, 'm', 1);
    }
    else {
        sendPostRequest("/OrderHandlers/Order.ashx/GetServerTime", "", function (rsp) {
            ServerTime = Date.fromString(rsp);
            CalcTime();
        }, $.noop);
        return;
    }
    $(".waitTime").each(function () {
        var that = $(this);
        var payTime = Date.fromString(that.next().val());
        var waitMininute = Date.diff(ServerTime, payTime, 'm');
        that.text(waitMininute);
        if (waitMininute <= 5) {
            that.css("color", "#009900");
        } else if (waitMininute <= 10) {
            that.css("color", "#ff8c01");
        } else {
            that.css("color", "#ed1c24");
        }
    });
    $(".waitedTime").each(function () {
        var that = $(this);
        var waitedMininute = parseFloat($.trim(that.text()));
        if (waitedMininute <= 5) {
            that.css("color", "#009900");
        } else if (waitedMininute <= 10) {
            that.css("color", "#ff8c01");
        } else {
            that.css("color", "#ed1c24");
        }
    });

}
function Go(url, id) {
    location.href = url + "?id=" + id + "&returnUrl=" + location.href;
}
