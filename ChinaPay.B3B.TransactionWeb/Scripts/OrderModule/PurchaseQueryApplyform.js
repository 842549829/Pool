function queryApplyform(pageIndex, pageSize) {
    pageSize = pageSize || 10;
    pageIndex = pageIndex || 10;
    var targetUrl = "/OrderHandlers/Applyform.ashx/PurchaseQueryApplyform";
    var applyformId = $.trim($("#txtApplyformId").val());
    var pnr = $.trim($("#txtPNR").val());
    var passenger = $.trim($("#txtPassenger").val());
    var startDate = $.trim($("#txtStartDate").val());
    var endDate = $.trim($("#txtEndDate").val());
    var applyTypeText = $.trim($("#hfdApplyformType").val());
    var refundStatus = $.trim($("#hfdRefundStatus").val());
    var applyType;
    if (applyTypeText == "退票") {
        applyType = 1;
    }
    if (applyTypeText == "废票") {
        applyType = 2;
    }
    if (applyTypeText == "改期") {
        applyType = 4;
    }
    if (applyTypeText == "差错退款")
    {
        applyType = 8;
    }
    var posponeStatus = $.trim($("#hfdPosponeStatus").val());
    var condition = { "ApplyformId": applyformId, "PNR": pnr, "AppliedDateRange": { "Lower": startDate, "Upper": endDate }, "ApplyformType": applyType, "Passenger": passenger, "RefundStatusText": refundStatus, "PostponeStatusText": posponeStatus };
    var pagination = { "PageSize": pageSize, "PageIndex": pageIndex, "GetRowCount": true };
    var parmater = JSON.stringify({ "condition": condition, "pagination": pagination });
    if (IsFirstLoad) { //在第一次查询时使用上一次的查询条件加载数据
        var lastData = getCookie("applyform3");
        if (lastData != "") {
            parmater = lastData;
            LoadPageStates(lastData);
            $("#applyType li a").each(function () {
                if ($(this).html() == $("#hfdApplyformType").val()) {
                    $("#applyType li a").removeClass("curr");
                    $(this).addClass("curr");
                }
            });
            $("#refundStatus li a").each(function () {
                if ($(this).html() == $("#hfdRefundStatus").val()) {
                    $("#refundStatus li a").removeClass("curr");
                    $(this).addClass("curr");
                }
            });
            $("#posponeStauts li a").each(function () {
                if ($(this).html() == $("#hfdPosponeStatus").val()) {
                    $("#posponeStauts li a").removeClass("curr");
                    $(this).addClass("curr");
                }
            });
            if ($.trim($("#hfdApplyformType").val()) == "退票" || $.trim($("#hfdApplyformType").val()) == "废票") {
                $("#posponeStauts").hide();
                $("#refundStatus").show();
            } else {
                if ($.trim($("#hfdApplyformType").val()) == "改期") {
                    $("#posponeStauts").show();
                    $("#refundStatus").hide();
                } else {
                    $("#posponeStauts").hide();
                    $("#refundStatus").hide();
                }
            }
        }
        IsFirstLoad = false;
    } else {
        if (parmater != "") setCookieCurrentPath("applyform3", parmater, 1);
    }

    sendPostRequest(targetUrl, parmater, function (result) {
        var applyformContents = new Array();
        if (result.Applyforms.length == 0) {
            $("#emptyInfo").show();
            $("#data-list").hide();
        } else {
            $("#emptyInfo").hide();
            $("#data-list").show();
            $.each(result.Applyforms, function (index, item) {
                applyformContents.push("<tr>");
                applyformContents.push("<td><a href='" + (item.ApplyTypeText == "改期" ? "PostponeApplyformDetail.aspx?id=" + item.ApplyformId : item.ApplyTypeText == "差错退款" ? "BalanceRefundApplyformDetail.aspx?id=" + item.ApplyformId : "RefundApplyformDetail.aspx?id=" + item.ApplyformId) + "' class='obvious-a'>" + item.ApplyformId + "</a></td>");
                applyformContents.push("<td>" + item.ProductType + "</td>");
                applyformContents.push("<td><span class='obvious'>" + item.PNR + "</span></td>");
                applyformContents.push("<td>" + item.Voyage + "</td>");
                applyformContents.push("<td>" + item.FlightInfo + "</td>");
                applyformContents.push("<td>" + item.TakeoffTime + "</td>");
                applyformContents.push("<td>" + item.Passenger + "</td>");
                applyformContents.push("<td>" + item.ApplyTypeText + "</td>");
                applyformContents.push("<td  class='b'>" + item.Status + "</td>");
                applyformContents.push("<td>" + item.Applier + "</td>");
                applyformContents.push("<td>" + item.AppliedTime + "</td>");
                applyformContents.push("<td><a href='" + (item.ApplyTypeText == "改期" ? "PostponeApplyformDetail.aspx?id=" + item.ApplyformId : item.ApplyTypeText == "差错退款" ? "BalanceRefundApplyformDetail.aspx?id=" + item.ApplyformId : "RefundApplyformDetail.aspx?id=" + item.ApplyformId) + "' class='obvious-a'>" + "详情</a><br />" +
                    "<a href='../ApplyformLog.aspx?id=" + item.ApplyformId + " class='obvious-a'" + "日志</a></td>");
                applyformContents.push("</tr>");
            });
            $("#data-list table tbody").html(applyformContents.join(''));
            $("#data-list table:not(.ClearAlternate) tr:nth-child(even)").addClass("alternate");
        }
        var pagination = result.Pagination;
        drawPagination($("#pager"), pagination.PageIndex, pagination.PageSize, pagination.RowCount, queryApplyform);
    }, function (e) {
        if (e.statusText == "timeout") {
            alert("服务器忙");
        } else {
            alert(e.responseText);
        }
    });
}
function LoadPageStates(parmater) {
    var condition;
    eval("condition = " + parmater);

    condition = condition.condition;
    $("#txtApplyformId").val(condition.ApplyformId);
    $("#txtPNR").val(condition.PNR);
    $("#txtPassenger").val(condition.Passenger);
    $("#txtStartDate").val(condition.AppliedDateRange.Lower);
    $("#txtEndDate").val(condition.AppliedDateRange.Upper);
    if (condition.ApplyformType == 1) {
        $("#hfdApplyformType").val("退票");
    } else {
        if (condition.ApplyformType == 2) {
            $("#hfdApplyformType").val("废票");
        } else {
            if (condition.ApplyformType == 4) {
                $("#hfdApplyformType").val("改期");
            } else if (condition.ApplyformType == 8)
            {
                $("#hfdApplyformType").val("差错退款");
            } else
            {
                $("#hfdApplyformType").val("综合查询");
            }
        }
    }
    $("#hfdRefundStatus").val(condition.RefundStatusText);
    $("#hfdPosponeStatus").val(condition.PostponeStatusText);
}