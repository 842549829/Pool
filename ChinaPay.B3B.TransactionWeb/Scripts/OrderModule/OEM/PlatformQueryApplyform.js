function queryApplyform(pageIndex, pageSize) {
    pageSize = pageSize || 10;
    pageIndex = pageIndex || 1;
    var targetUrl = "/OrderHandlers/Applyform.ashx/QueryOEMApplyform";
    var applyformId = $.trim($("#txtApplyformId").val());
    var pnr = $.trim($("#txtPNR").val());
    var passenger = $.trim($("#txtPassenger").val());
    var startDate = $.trim($("#txtAppliedDateStart").val());
    var endDate = $.trim($("#txtAppliedDateEnd").val());
    var applyTypeText = $.trim($("#hfdApplyformType").val());
    var refundTypeText = $.trim($("#hfdRefundType").val());
    var applyType;
    var refundType;
    if (applyTypeText == "退票") {
        applyType = 1;
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
    }
    if (applyTypeText == "废票") {
        applyType = 2;
    }
    if (applyTypeText == "改期") {
        applyType = 4;
    }
    var refundStatus = $.trim($("#hfdRefundStatus").val());
    var posponeStatus = $.trim($("#hfdPosponeStatus").val());
    var purchase = $.trim($("#PurchaseCompany_hidCompanyId").val());
    var purchaseTxt = $.trim($("#PurchaseCompany_txtCompanyName").val());
    var condition = { "ApplyformId": applyformId, "PNR": pnr, "Passenger": passenger, "AppliedDateRange": { "Lower": startDate, "Upper": endDate }, "ApplyformType": applyType, "RefundStatusText": refundStatus, "PostponeStatusText": posponeStatus, "Purchaser": purchase, "RefundType": refundType, "PurchaseTxt": purchaseTxt };
    var pagination = { "PageSize": pageSize, "PageIndex": pageIndex, "GetRowCount": true };
    var parmater = JSON.stringify({ "condition": condition, "pagination": pagination });
    if (IsFirstLoad) { //在第一次查询时使用上一次的查询条件加载数据
        var lastData = getCookie("applyform2");
        if (lastData != "") {
            parmater = lastData;
            LoadPageStates(lastData);
            $("#applyType li a").each(function () {
                if ($(this).html() == $("#hfdApplyformType").val()) {
                    $("#applyType li a").removeClass("curr");
                    $(this).addClass("curr");
                }
            });
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
            $("#posponeStauts li a").each(function () {
                if ($(this).html() == $("#hfdPosponeStatus").val()) {
                    $("#posponeStauts li a").removeClass("curr");
                    $(this).addClass("curr");
                }
            });
            if ($.trim($("#hfdApplyformType").val()) == "退票" || $.trim($("#hfdApplyformType").val()) == "废票") {
                if ($.trim($("#hfdApplyformType").val()) == "退票") {
                    $("#refundType").show();
                } else {
                    $("#refundType").hide();
                }
                $("#posponeStauts").hide();
                $("#refundStatus").show();
            } else {
                $("#refundType").hide();
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
        if (parmater != "") setCookieCurrentPath("applyform2", parmater, 1);
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
                applyformContents.push("<td><a href='" + (item.ApplyTypeDesc == "改期" ? "PostponeApplyformDetail.aspx?id=" + item.ApplyformId : "ApplyformDetail.aspx?id=" + item.ApplyformId) + "' class='obvious-a'>" + item.ApplyformId + "</a></td>");
                applyformContents.push("<td>" + item.ProductType + "</td>");
                applyformContents.push("<td><span class='obvious'>" + item.PNR + "</span></td>");
                applyformContents.push("<td>" + item.Voyage + "</td>");
                applyformContents.push("<td>" + item.FlightInfo + "</td>");
                applyformContents.push("<td>" + item.TakeoffTime + "</td>");
                applyformContents.push("<td>" + item.Passengers + "</td>");
                applyformContents.push("<td>" + item.ApplyTypeDesc + (item.IsRefund ? ("<span><br/>" + item.RefundType + "</span>") : "") + "</td>");
                applyformContents.push("<td  class='b'>" + item.ProcessStatus + "</td>");
                applyformContents.push("<td>" + item.AppliedTime + "</td>");
                applyformContents.push("<td><a href='" + (item.ApplyTypeDesc == "改期" ? "PostponeApplyformDetail.aspx?id=" + item.ApplyformId : "ApplyformDetail.aspx?id=" + item.ApplyformId) + "' class='obvious-a'>" + "详情</a><br />"
                + "<a href='../ApplyformLog.aspx?id=" + item.ApplyformId + "' class='obvious-a'>" + "日志</a>" + "</td>");
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
                    $("#hfdRefundType").val("全部");
                }
            }
        }
    }

    $("#hfdRefundStatus").val(condition.RefundStatusText);
    $("#hfdPosponeStatus").val(condition.PostponeStatusText);
    $("#PurchaseCompany_hidCompanyId").val(condition.Purchase);
    $("#PurchaseCompany_txtCompanyName").val(condition.PurchaseTxt);

}

function Unlock(orderId) {
    sendPostRequest("/OrderHandlers/Order.ashx/UnLockData", JSON.stringify({ orderId: orderId }), function () {
        var target = $("#a" + orderId);
        target.parent("td").prev().empty();
        target.remove();
    }, $.noop);
}


function QueryPaymentInfo(orderId) {
    sendPostRequest("/OrderHandlers/Applyform.ashx/QueryPaymentInfo", JSON.stringify({ applyformId: orderId }), function (res) {
        if (res == "OK") {
            var target = $("#b" + orderId);
            target.parent().prev().prev().prev().text("已支付待改期");
            target.parent().prev().html("");
            target.remove();
        } else {
            alert("没有查询到付款成功记录!");
        }
    }, function () {
        alert("没有查询到付款成功记录!");
    });
}
