function bindData(data)
{
    var html = new Array();
    $.each(data.Orders, function (index, item) {
        html.push("<tr><td><a href='./OrderDetail.aspx?id=" + item.OrderId + "' class='obvious-a'>" + item.OrderId + "</a></td>");
        html.push("<td><span class='obvious'>" + item.PNR + "</span></td>");
        html.push("<td>" + item.FlightInfo + "</td>");
        html.push("<td>" + item.Passengers + "</td>");
        html.push("<td>" + item.Price + "</td>");
        html.push("<td>" + item.Commission + "</td>");
        html.push("<td>" + item.ProducedTime + (item.RemindIsShow ? ("<br/><a href='javascript:;' class='obvious urgent_btn urgent_order' content='" + item.RemindContent + "'>采购催单</a>") : "") + "</td>");
        html.push("<td><span>" + item.Status + "</span><br/>" + (item.IsEmergentOrder ? "<a id='" + item.OrderId + "' href='javascript:void(0);' class='tips_btn urgent'>紧急</a>" : ""));
        html.push((item.ETDZTime != 0.1 ? "<span class='waitedTime'>" + item.ETDZTime + "</span>" : item.PayTime == '' ? "" : "<span class='waitTime'></span><input type='hidden' value='" + item.PayTime + "' />"));
        html.push("</td><td>");
        html.push("<a href='./OrderDetail.aspx?id=" + item.OrderId + "' class='obvious-a'>详情</a><br />");
        html.push("<a href='../OrderLog.aspx?id=" + item.OrderId + "' class='obvious-a'>日志</a></td></tr>");
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
        })
        urg.mouseleave(function () {
            urg.addClass("hidden");
        });
        html = null;
    } else {
        $("#emptyInfo").show();
        $("#data-list").hide();
    }
    $("#data-list table:not(.ClearAlternate) tr:nth-child(even)").addClass("alternate");
    drawPagination($("#divPagination"), data.Pagination.PageIndex, data.Pagination.PageSize, data.Pagination.RowCount, queryOrders);
    CalcTime(1);
}
function queryOrders(pageIndex,pageSize)
{
    if (!pageIndex) pageIndex = 1;
    pageSize = pageSize || 10;
    var condition = getQueryOrdersCondition();
    var pagination = getQueryOrdersPagination(pageIndex,pageSize);
    var parameters = JSON.stringify({ "pagination": pagination, "orderQueryCondition": condition });
    if (IsFirstLoad)
    { //在第一次查询时使用上一次的查询条件加载数据
        var lastData = getCookie("OrderList2");
        if (lastData != "")
        {
            parameters = lastData;
            LoadPageStates(lastData);
        }
        IsFirstLoad = false;
    } else
    {
        if (parameters != "") setCookieCurrentPath("OrderList2", parameters, 1);
    }

    sendPostRequest("/OrderHandlers/Order.ashx/QueryProvideOrderList", parameters, function (data)
    {
        bindData(data);
    }, function (e)
    {
        if (e.statusText == "timeout")
        {
            alert("服务器忙");
        } else
        {
            alert(e.responseText);
        }
    });
    return false;
}
function getQueryOrdersCondition()
{
    return { "ProducedDateRange": { "Lower": $("#txtStartDate").val(), "Upper": $("#txtEndDate").val() },
        "Passenger": $.trim($("#txtPassenger").val()), "PNR": $.trim($("#txtPNR").val()), "OrderId": $.trim($("#txtOrderId").val()),
        "ProductType": $("#ulProduct>li>a.curr").attr("accesskey"), "OrderStatusText": $("#ulOrderStatus>li>a.curr").text()
    };
}
function getQueryOrdersPagination(pageIndex, pageSize)
{
    return { "PageSize": pageSize, "PageIndex": pageIndex, "GetRowCount": true };
}
window.onload = function () {
    var pageSize = 10;
    $("#txtOrderId").OnlyNumber().LimitLength(13);
    queryOrders();
    document.getElementById("btnSerach").onclick = function () {
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryOrders(1, pageSize);
    };
    $("#ulProduct>li>a").click(function () {
        $("#ulProduct>li>a").each(function () { $(this).removeClass("curr"); });
        $(this).addClass("curr");
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryOrders(1, pageSize);
        return false;
    });
    $("#ulOrderStatus>li>a").click(function () {
        var efficiency = document.getElementById("divEfficiency");
        if (efficiency) {
            var orderText = $.trim($(this).text());
            if (orderText === "全部" || orderText === "已支付待订座" || orderText === "已支付待出票") {
                efficiency.style.display = "block";
            }else{
                efficiency.style.display = "none";
            }
        }
        $("#ulOrderStatus>li>a").each(function () { $(this).removeClass("curr"); });
        $(this).addClass("curr");
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryOrders(1, pageSize);
        return false;
    });
};

function LoadPageStates(parmater)
{
    var condition;
    eval("condition = " + parmater);
    if (condition != "undefined")
    {
        doc("txtStartDate").value = condition.orderQueryCondition.ProducedDateRange.Lower;
        doc("txtEndDate").value = condition.orderQueryCondition.ProducedDateRange.Upper;
        doc("txtPassenger").value = condition.orderQueryCondition.Passenger;
        doc("txtPNR").value = condition.orderQueryCondition.PNR;
        doc("txtOrderId").value = condition.orderQueryCondition.OrderId;
        $("#ulProduct>li>a").each(function ()
        {
            var self = $(this);
            self.removeClass("curr");
            if ($.trim(self.attr("accesskey")) === condition.orderQueryCondition.ProductType)
            {
                self.addClass("curr");
            }
        });
        $("#ulOrderStatus>li>a").each(function ()
        {
            var self = $(this);
            self.removeClass("curr");
            if ($.trim(self.text()) === condition.orderQueryCondition.OrderStatusText)
            {
                self.addClass("curr");
            }
        });
    }
}
function doc(id) { return document.getElementById(id); }

function CalcTime()
{
    if (arguments.length == 0)
    {
        ServerTime = Date.addPart(ServerTime, 'm', 1);
    } else
    {
        sendPostRequest("/OrderHandlers/Order.ashx/GetServerTime", "", function (rsp)
        {
            ServerTime = Date.fromString(rsp);
            CalcTime();
        }, $.noop);
        return;
    }
    $(".waitTime").each(function ()
    {
        var that = $(this);
        var payTime = Date.fromString(that.next().val());
        var waitMininute = Date.diff(ServerTime, payTime, 'm');
        that.text(waitMininute);
        if (waitMininute <= 5)
        {
            that.css("color", "#009900");
        } else if (waitMininute <= 10)
        {
            that.css("color", "#ff8c01");
        } else
        {
            that.css("color", "#ed1c24");
        }
    });
    $(".waitedTime").each(function ()
    {
        var that = $(this);
        var waitedMininute = parseFloat($.trim(that.text()));
        if (waitedMininute <= 5)
        {
            that.css("color", "#009900");
        } else if (waitedMininute <= 10)
        {
            that.css("color", "#ff8c01");
        } else
        {
            that.css("color", "#ed1c24");
        }
    });
}