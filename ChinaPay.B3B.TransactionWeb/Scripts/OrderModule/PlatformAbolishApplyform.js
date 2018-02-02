$(function () {
    var pageSize = 10;
    $("#txtApplyformId").OnlyNumber().LimitLength(13);
    queryApplyform(1);
    $("#refundStatus li a").live("click", function () {
        $("#refundStatus li a").removeClass("curr");
        $("#hfdRefundStatus").val($.trim($(this).html()));
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
    pageSize = pageSize || 10;
    pageIndex = pageIndex || 1;
    var targetUrl = "/OrderHandlers/Applyform.ashx/PlatformProcessApplyform";
    var applyformId = $.trim($("#txtApplyformId").val());
    var pnr = $.trim($("#txtPNR").val());
    var passenger = $.trim($("#txtPassenger").val());
    var startDate = $.trim($("#txtAppliedDateStart").val());
    var endDate = $.trim($("#txtAppliedDateEnd").val());
    var refundStatus = $.trim($("#hfdRefundStatus").val());
//    var purchase = $.trim($("#PurchaseCompany_hidCompanyId").val());
//    var provider = $.trim($("#AgentCompany_hidCompanyId").val());
//    var supplier = $.trim($("#SupplierCompany_hidCompanyId").val());
//    var purchaseTxt = $.trim($("#PurchaseCompany_txtCompanyName").val());
//    var providerTxt = $.trim($("#AgentCompany_txtCompanyName").val());
//    var supplierTxt = $.trim($("#SupplierCompany_txtCompanyName").val());
    var condition = { "ApplyformId": applyformId, "PNR": pnr, "Passenger": passenger, "AppliedDateRange": { "Lower": startDate, "Upper": endDate }, "ApplyformType": 2, "RefundStatusText": refundStatus};
    var pagination = { "PageSize": pageSize, "PageIndex": pageIndex, "GetRowCount": true };
    var parmater = JSON.stringify({ "condition": condition, "pagination": pagination });
    if (IsFirstLoad) { //在第一次查询时使用上一次的查询条件加载数据
        var lastData = getCookie("platformAbolishApplyform");
        if (lastData != "") {
            parmater = lastData;
            LoadPageStates(lastData);
            $("#refundStatus li a").each(function () {
                if ($(this).html() == $("#hfdRefundStatus").val()) {
                    $("#refundStatus li a").removeClass("curr");
                    $(this).addClass("curr");
                }
            });
        }
        IsFirstLoad = false;
    } else {
        if (parmater != "") setCookieCurrentPath("platformAbolishApplyform", parmater, 1);
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
                applyformContents.push("<td><a href='" + ("RefundApplyformDetail.aspx?id=" + item.ApplyformId) + "&returnUrl=AbolishList.aspx' class='obvious-a'>" + item.ApplyformId + "</a></td>");
                applyformContents.push("<td>" + item.ProductType + "</td>");
                applyformContents.push("<td><span class='obvious'>" + item.PNR + "</span></td>");
                applyformContents.push("<td>" + item.Voyage + "</td>");
                applyformContents.push("<td>" + item.FlightInfo + "</td>");
                applyformContents.push("<td>" + item.TakeoffTime + "</td>");
                applyformContents.push("<td>" + item.Passengers + "</td>");
                //                applyformContents.push("<td>" + item.ApplyType + "</td>");
                applyformContents.push("<td  class='b'>" + item.ProcessStatus + "</td>");
                applyformContents.push("<td>" + item.AppliedTime + "</td>");
                applyformContents.push("<td>" + item.LockInfo + "</td>");
                applyformContents.push("<td><a style='display:" + (item.RequireRevisePrice == true ? "none" : "") + "' href='" + (item.ApplyType == "改期" ? "ProcessPostpone.aspx" : "ProcessRefund.aspx") + "?id=" + item.ApplyformId + "&returnUrl=AbolishList.aspx' class='obvious'>" + "处理</a>" +
                                     "<a href='ProcessRevisePrice.aspx?id=" + item.ApplyformId + "&returnUrl=AbolishList.aspx' style='display:" + (item.RequireRevisePrice == true ? "" : "none") + "'>修改价格</a></td>");
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
    $("#hfdRefundStatus").val(condition.RefundStatusText);
//    $("#PurchaseCompany_hidCompanyId").val(condition.Purchase);
//    $("#AgentCompany_hidCompanyId").val(condition.Provider);
//    $("#SupplierCompany_hidCompanyId").val(condition.Supplier);
//    $("#PurchaseCompany_txtCompanyName").val(condition.PurchaseTxt);
//    $("#AgentCompany_txtCompanyName").val(condition.ProviderTxt);
//    $("#SupplierCompany_txtCompanyName").val(condition.SupplierTxt);
}