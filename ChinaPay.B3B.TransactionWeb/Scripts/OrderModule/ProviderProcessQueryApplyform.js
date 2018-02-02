$(function () {
    var pageSize = 10;
    $("#txtApplyformId").OnlyNumber().LimitLength(13);
    $("#refundType li").eq(0).find("a").addClass("curr");
    queryApplyform(1);
    $("#refundStatus li a").each(function () {
        if ($(this).html() == $("#hfdRefundStatus").val()) {
            $("#refundStatus li a").removeClass("curr");
            $(this).addClass("curr");
        }
    });
    $("#refundType li a").live("click", function () {
        $("#refundType li a").removeClass("curr");
        $("#hfdRefundType").val($.trim($(this).html()));
        $(this).addClass("curr");
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryApplyform(1, pageSize);
    });
    $("#refundType li a").each(function () {
        if ($(this).html() == $("#hfdRefundType").val()) {
            $("#refundType li a").removeClass("curr");
            $(this).addClass("curr");
        }
    });
    $("#refundStatus li a").live("click", function () {
        $("#refundStatus li a").removeClass("curr");
        $("#hfdRefundStatus").val($.trim($(this).text()));
        $(this).addClass("curr");
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryApplyform(1, pageSize);
    });
    $("#btnQuery").click(function () {
        if ($("#dropPageSize").size() > 0) {
            pageSize = $("#dropPageSize option:selected").val();
        }
        queryApplyform(1, pageSize);
    });
})

function queryApplyform(pageIndex, pageSize) {
    pageIndex = pageIndex || 1;
    pageSize = pageSize || 10;
    var targetUrl = "/OrderHandlers/Applyform.ashx/ProvideProcessApplyformNew";
    var applyformId = $.trim($("#txtApplyformId").val());
    var pnr = $.trim($("#txtPNR").val());
    var passenger = $.trim($("#txtPassenger").val());
    var startDate = $.trim($("#txtAppliedDateStart").val());
    var endDate = $.trim($("#txtAppliedDateEnd").val());
    var refundTypeText = $.trim($("#hfdRefundType").val());
    var refundType;
    if (refundTypeText == "升舱全退") {
        refundType = 0;
    }
    if (refundTypeText == "自愿按客规退票") {
        refundType = 1;
    }
    if (refundTypeText == "非自愿退票") {
        refundType = 2;
    }
    if (refundTypeText == "特殊原因退票") {
        refundType = 3;
    }

    var refundStatus = $.trim($("#hfdRefundStatus").val());
    var condition = { "ApplyformId": applyformId, "PNR": pnr, "Passenger": passenger, "AppliedDateRange": { "Lower": startDate, "Upper": endDate }, "ApplyformType": 1, "RefundStatusText": refundStatus, "RefundType": refundType };
    var pagination = { "PageSize": pageSize, "PageIndex": pageIndex, "GetRowCount": true };
    var parmater = JSON.stringify({ "condition": condition, "pagination": pagination });
    if (IsFirstLoad) { //在第一次查询时使用上一次的查询条件加载数据
        var lastData = getCookie("provideProcessQueryApplyform");
        if (lastData != "") {
            parmater = lastData;
            LoadPageStates(lastData);
            $("#refundType li a").each(function () {
                if ($(this).html() == $("#hfdRefundType").val()) {
                    $("#refundType li a").removeClass("curr");
                    $(this).addClass("curr");
                }
            });
            $("#refundStatus li a").each(function () {
                if ($(this).html() == $("#hfdRefundStatus").val()) {
                    $("#refundStatus li a").removeClass("curr");
                    $(this).addClass("curr");
                }
            });
        }
        IsFirstLoad = false;
    } else {
        if (parmater != "") setCookieCurrentPath("provideProcessQueryApplyform", parmater, 1);
    }

    sendPostRequest(targetUrl, parmater, function (result) {
        var applyformContents = new Array();
        if (result.Applyforms.length == 0) {
            $("#emptyInfo").show();
            $("#data-list").hide();
        } else {
            $("#emptyInfo").hide();
            $("#data-list").show();
            var isToday = false;
            var classStyle = "style = 'color:red;'";
            $.each(result.Applyforms, function (index, item) {
                isToday = false;
                var temp = item.TakeoffTimeIsToday.split('|');
                for (var i = 0; i < temp.length; i++) {
                    if (temp[i] == "1") {
                        isToday = true;
                        break;
                    }
                }
                applyformContents.push("<tr>");
                applyformContents.push("<td><a href='" + "RefundApplyformDetail.aspx?id=" + item.ApplyformId + "&returnUrl=ChangeProcessList.aspx' class='obvious-a'>" + item.ApplyformId + "</a></td>");
                applyformContents.push("<td " + (isToday ? classStyle : "") + ">" + item.ProductType + "</td>");
                applyformContents.push("<td><span class='obvious'>" + item.PNR + "</td>");
                applyformContents.push("<td " + (isToday ? classStyle : "") + ">" + item.Voyage + "</td>");
                applyformContents.push("<td " + (isToday ? classStyle : "") + ">" + item.FlightInfo + "</td>");
                applyformContents.push("<td " + (isToday ? classStyle : "") + ">" + item.TakeoffTime + "</td>");
                applyformContents.push("<td " + (isToday ? classStyle : "") + ">" + item.Passengers + "</td>");
//                applyformContents.push("<td>" + item.ApplyType + "</td>");
                applyformContents.push("<td " + (isToday ? classStyle : "") + ">" + item.RefundType + "</td>");
                applyformContents.push("<td " + (isToday ? classStyle : "") + ">" + item.AppliedTime + "</td>");
                applyformContents.push("<td " + (isToday ? classStyle : "") + ">" + item.LockInfo + "</td>");
                applyformContents.push("<td><a href='" + (item.ApplyType == "废票" ? "Abolish.aspx?id=" + item.ApplyformId : "ProcessRefund.aspx?id=" + item.ApplyformId) + "' class='obvious'>" + "处理</a></td>");
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
    $("#txtAppliedDateStart").val(condition.AppliedDateRange.Lower);
    $("#txtAppliedDateEnd").val(condition.AppliedDateRange.Upper);
    if (condition.RefundType == 0) {
        $("#hfdRefundType").val("升舱全退");
    } else {
        if (condition.RefundType == 1) {
            $("#hfdRefundType").val("自愿按客规退票");
        } else {
            if (condition.RefundType == 2) {
                $("#hfdRefundType").val("非自愿退票");
            } else {
                if (condition.RefundType == 3) {
                    $("#hfdRefundType").val("特殊原因退票");
                } else {
                    $("#hfdRefundType").val("综合查询");
                }
            }
        }
    }
    $("#hfdRefundStatus").val(condition.RefundStatusText);
}