function bindData(data)
{
    var html = new Array();
    $.each(data.Orders, function (index, item)
    {
        html.push("<tr><td><a href='./OrderDetail.aspx?id=" + item.OrderId + "' class='obvious-a'>" + item.OrderId + "</a></td>");
        html.push("<td>");
        html.push(item.PNR);
        html.push("</td><td>");
        html.push(item.FlightInfo);
        html.push("</td><td>");
        html.push(item.Passengers);
        //        if (item.Status == "已出票")
        //        {
        //            html.push(" <a href='javascript:PrintVoyage(\"'", item.OrderId, "'\"", item.Passengers, "\"'");
        //        }
        html.push("</td><td>");
        html.push(item.Price);
        html.push("</td><td>");
        html.push(item.Commission);
        html.push("</td><td>");
        html.push(item.ProducedTime);
        html.push("</td>");
        //html.push("<td>",(item.ETDZTime!=0?"<br />出票等待:"+item.ETDZTime+"(分钟)":item.PayTime==''?""
        //:"<span class='waitTime'></span><input type='hidden' value='"+item.PayTime+"' />"),"</td>");
        html.push("<td><span>");
        html.push(item.Status);
        html.push((item.ETDZTime != 0.1 ? "<br /><span class='waitedTime'>" +
                                             item.ETDZTime + "</span>" : item.PayTime == '' ? "" : "<br /><span class='waitTime'></span><input type='hidden' value='" +
                                              item.PayTime + "' />"));
        html.push("<span></td>");
        html.push("<td><a href='./OrderDetail.aspx?id=" + item.OrderId +
                "' class='obvious-a'>详情</a><br /><a href='" + (item.Status == "等待支付" ? "/OrderModule/Purchase/OrderPay.aspx" : "../OrderLog.aspx") + "?id=" + item.OrderId +
                "'class='obvious-a'>" + (item.Status == "等待支付" ? "支付" : "日志") + "</a><br/>" +
               "<a href='javascript:PrintMessage(\"" + item.OrderId + "\");'" + " style='display:" + (item.Status == "已出票" && !item.PassengerMsgSended ? "" : "none") + "' class='printMessage'>" + "出票短信</a><br style='display:"+(item.Status == "已出票" && !item.PassengerMsgSended ? "" : "none")+"' />" +
                "<a href='javascript:Reminded(\"" + item.OrderId + "\");'" + " style='display:" + (item.IsNeedRemind ? "" : "none") + "' class='reminded'>" +
                "采购催单</a></td></tr>");
    });
    if (data.Orders.length > 0) {
        $("#emptyInfo").hide();
        $("#data-list").show();
        $("#data-list table tbody").html(html.join(''));
    } else {
        $("#emptyInfo").show();
        $("#data-list").hide();
    }
    $("#data-list table:not(.ClearAlternate) tr:nth-child(even)").addClass("alternate");
    drawPagination($("#divPagination"), data.Pagination.PageIndex, data.Pagination.PageSize, data.Pagination.RowCount, queryOrders);
    CalcTime();
}

function CalcTime()
{
    ServerTime = Date.addPart(ServerTime, 'm', 1);
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

function queryOrders(pageIndex,pageSize)
{
    if (!pageIndex) pageIndex = 1;
    pageSize = pageSize || 10;
    var condition = getQueryOrdersCondition();
    var pagination = getQueryOrdersPagination(pageIndex,pageSize);
    var parameters = JSON.stringify({ "pagination": pagination, "orderQueryCondition": condition });
    if (IsFirstLoad)
    { //在第一次查询时使用上一次的查询条件加载数据
        var lastData = getCookie("applyform2");
        if (lastData != "")
        {
            parameters = lastData;
            LoadPageStates(parameters);
        }
        IsFirstLoad = false;
    } else
    {
        if (parameters != "") setCookieCurrentPath("applyform2", parameters, 1);
    }

    sendPostRequest("/OrderHandlers/Order.ashx/QueryPurchaseOrderList", parameters, function (data)
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
    }
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
        $("#ulOrderStatus>li>a").each(function () { $(this).removeClass("curr"); });
        $(this).addClass("curr");
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryOrders(1, pageSize);
        return false;
    });

    $(".tips_btn").live("mouseenter", function () {
        var self = $(this);
        var tips_box = self.next();
        $(".tips_box1").addClass("tips_box").removeClass("tips_box1");
        tips_box.removeClass("hidden").css({ "left": self.offset().left - 110, "top": self.offset().top + 10 });
        var h = tips_box.height();
        var top = tips_box.offset().top;
        var sor = $(window.parent).scrollTop();
        var wh = $(window.parent).height();
        if (h + top - sor > wh / 2) {
            tips_box.css({ top: (top - h - 32) }).addClass("tips_box1").removeClass("tips_box");
        };
    }).live("mouseleave", function () {
        var self = $(this);
        var tips_box = self.next();
        tips_box.mouseenter(function () { $(this).removeClass("hidden"); }).mouseleave(function () { $(this).addClass("hidden"); });
        $(".tips_box1").mouseenter(function () { }).mouseleave(function () { $(this).addClass("hidden"); });
        tips_box.addClass("hidden");
    });
}

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

