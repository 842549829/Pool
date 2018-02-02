function bindData(data) {
    var html = new Array();
    $.each(data.Orders, function (index, item) {
        html.push("<tr><td><a href='./OrderDetail.aspx?id=" + item.OrderId + "' class='obvious-a'>" + item.OrderId);
        html.push("</a><br/>");
        html.push(item.Source + "</br>");
        html.push(item.IsOEM ? "<span class='purchaseTypes brother btnoem'>OEM</span>&nbsp;" : "");
        html.push(item.IsOEM && !item.AllowPlatformContractPurchaser ? "<a class='urgent btnAllowPlatformContractPurchaser'>禁</a>" : "");
        html.push("</td><td><span class='obvious'>" + item.PNR + "</span></td>");
        html.push("<td>" + item.FlightInfo + "</td>");
        html.push("<td>" + item.Passengers + "</td>");
        html.push("<td>" + item.Price + "</td>");
        html.push("<td>" + item.Commission + "</td>");
        html.push("<td>" + item.ProducedTime + "</td>");
        html.push("<td>" + item.PurchaserName + "</td>");
        html.push("<td>" + item.SupplierName + "</td>");
        html.push("<td>" + item.ProviderName + "</td>");
        html.push("<td><span class='purchaseTypes" + (item.IsBrother ? " brother'>" : item.IsNull ? "'>" : " lower'>") + item.RelationType + "</span>");
        html.push(item.RemindIsShow ? ("<br/><a href='javascript:;' class='obvious urgent_btn urgent_order' content='" + item.RemindContent + "'>采购催单</a>") : "");
        html.push("</td>");
        html.push("<td><span>" + item.Status + "</span><br/>" + (item.IsEmergentOrder ? "<a id='" + item.OrderId + "' href='javascript:void(0);' class='tips_btn urgent'>紧急</a>" : ""));
        html.push((item.ETDZTime != 0.1 ? "<span class='waitedTime'>" + item.ETDZTime + "</span>" : ""));
        html.push("</td>");
        html.push("<td><span>" + item.LockInfo + "</span></td>");
        html.push("<td><a href='./OrderDetail.aspx?id=" + item.OrderId);
        html.push("' class='obvious-a'>详情</a><br /><a href='./Coordination.aspx?id=" + item.OrderId);
        html.push("' class='obvious-a'>协调</a><br /><a href='../OrderLog.aspx?id=" + item.OrderId);
        html.push("' class='obvious-a'>日志</a>" + (item.RenderSupperUnLock ? "<br /><a href='javascript:Unlock(\"" + item.OrderId + "\");' id='a" + item.OrderId + "' class='obvious-a'>解锁</a>" : ""));
        html.push((item.EnableQueryPaymentInfo ? "<br /><a href='javascript:QueryPaymentInfo(\"" + item.OrderId + "\");' id='b" + item.OrderId + "' class='obvious-a'>补单</a>" : ""));
        html.push("</td></tr>");
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

    $("#data-list table:not(.ClearAlternate) tr:nth-child(even)").addClass("alternate");
    drawPagination($("#divPagination"), data.Pagination.PageIndex, data.Pagination.PageSize, data.Pagination.RowCount, queryOrders);
    CalcTime(1);
}
function queryOrders(pageIndex, pageSize) {
    if (!pageIndex) pageIndex = 1;
    pageSize = pageSize || 10;
    var condition = getQueryOrdersCondition();
    var pagination = getQueryOrdersPagination(pageIndex, pageSize);
    var parameters = JSON.stringify({ "pagination": pagination, "orderQueryCondition": condition });
    if (IsFirstLoad) { //在第一次查询时使用上一次的查询条件加载数据
        var lastData = getCookie("OrderList1");
        if (lastData != "") {
            parameters = lastData;
            $("#chkProffession").removeAttr("CHECKED");
            LoadPageStates(lastData);
        }
        IsFirstLoad = false;
    } else {
        if (parameters != "") setCookieCurrentPath("OrderList1", parameters, 1);
    }

    sendPostRequest("/OrderHandlers/Order.ashx/QueryOperateOrderList", parameters, function (data) {
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
    return { "ProducedDateRange": { "Lower": startDate + " " + $("#txtStartTime").val(), "Upper": endDate + " " + $("#txtEndTime").val() },
        "Passenger": $.trim($("#txtPassenger").val()), "PNR": $.trim($("#txtPNR").val()), "OrderId": $.trim($("#txtOrderId").val()),
        "ProductType": $("#ulProduct>li>a.curr").attr("accesskey"), "OrderStatusText": $("#ulOrderStatus>li>a.curr").text(),
        "Purchaser": $("#ucPurchaser_hidCompanyId").val(),
        "Supplier": $("#ucSupplier_hidCompanyId").val(),
        "Provider": $("#ucProvider_hidCompanyId").val(),
        "Carrier": $("#Carrier>option:selected").val(),
        "RelationBrother": ($("#chkProffession").attr("CHECKED") == "checked"),
        "RelationJunion": ($("#chkSubordinate").attr("CHECKED") == "checked"),
        "RelationInterior": ($("#chkInterior").attr("CHECKED") == "checked"),
        "Source": $("#ddlOrderSource").val(),
        "PurchaseTxt": $.trim($("#ucPurchaser_txtCompanyName").val()),
        "ProviderTxt": $.trim($("#ucProvider_txtCompanyName").val()),
        "SupplierTxt": $.trim($("#ucSupplier_txtCompanyName").val())
    };
}
function getQueryOrdersPagination(pageIndex, pageSize) {
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
        $("#ulOrderStatus>li>a").each(function () { $(this).removeClass("curr"); });
        $(this).addClass("curr");
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryOrders(1, pageSize);
        return false;
    });
    
    $(".btnoem").live("mouseenter", function () {
        var self = $(this);
        var layer = $("#divOem");
        layer.removeClass("hidden").css({ "left": self.offset().left + self.width(), "top": self.offset().top - self.height() });
    }).live("mouseleave", function () {
        $("#divOem").addClass("hidden");
    });
    $(".btnAllowPlatformContractPurchaser").live("mouseenter", function () {
        var self = $(this);
        var layer = $("#divAllowPlatformContractPurchaser");
        layer.removeClass("hidden").css({ "left": self.offset().left + self.width(), "top": self.offset().top - self.height() });
    }).live("mouseleave", function () {
        $("#divAllowPlatformContractPurchaser").addClass("hidden");
    });
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
        $("#ulProduct>li>a").each(function () {
            var self = $(this);
            self.removeClass("curr");
            if ($.trim(self.attr("accesskey")) === condition.orderQueryCondition.ProductType) {
                self.addClass("curr");
            }
        });
        $("#ulOrderStatus>li>a").each(function () {
            var self = $(this);
            self.removeClass("curr");
            if ($.trim(self.text()) === condition.orderQueryCondition.OrderStatusText) {
                self.addClass("curr");
            }
        });
        doc("ucPurchaser_txtCompanyName").value = condition.orderQueryCondition.PurchaseTxt;
        doc("ucPurchaser_hidCompanyId").value = condition.orderQueryCondition.Purchaser;
        doc("ucSupplier_txtCompanyName").value = condition.orderQueryCondition.SupplierTxt;
        doc("ucSupplier_hidCompanyId").value = condition.orderQueryCondition.Supplier;
        doc("ucProvider_txtCompanyName").value = condition.orderQueryCondition.ProviderTxt;
        doc("ucProvider_hidCompanyId").value = condition.orderQueryCondition.Provider;
        if (condition.orderQueryCondition.RelationBrother) {
            $("#chkProffession").attr("checked", "cheked");
        }
        if (condition.orderQueryCondition.RelationJunion) {
            $("#chkSubordinate").attr("checked", "cheked");
        }
        if (condition.orderQueryCondition.RelationInterior) {
            $("#chkInterior").attr("checked", "cheked");
        }
        doc("ddlOrderSource").value = condition.orderQueryCondition.Source;
    }
}
function doc(id) { return document.getElementById(id); }
function Unlock(orderId) {
    sendPostRequest("/OrderHandlers/Order.ashx/UnLockData", JSON.stringify({ orderId: orderId }), function () {
        var target = $("#a" + orderId);
        target.parent("td").prev().empty();
        target.remove();
    }, $.noop);
}
function QueryPaymentInfo(orderId) {
    sendPostRequest("/OrderHandlers/Order.ashx/QueryPaymentInfo", JSON.stringify({ orderId: orderId }), function (res) {
        if (res == "OK") {
            var target = $("#b" + orderId);
            target.parent().prev().prev().text("已支付待出票");
            target.remove();
        } else {
            alert("没有查询到付款成功记录!");
        }
    }, function () {
        alert("没有查询到付款成功记录!");
    });
}

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