function bindData(data) {
    var html = new Array();
    $.each(data.UserList, function (index, item) {
        html.push("<tr><td>");
        html.push("<input type='checkbox' companyId='" + item.CompanyId + "' incomegroupId='" + item.IncomeGroupId + "' /></td>");
        html.push("<td>" + item.RegisterTime + "</td>");
        html.push("<td>");
        html.push(item.Login);
        html.push("</td><td>");
        html.push(item.AbbreviateName);
        html.push("</td><td>");
        html.push(item.IncomeGroupName);
        html.push("</td></tr>");
    });
    if (data.UserList.length > 0) {
        $("#emptyInfo").hide();
        $("#data-list").show();
        $("#data-list table tbody").html(html.join(''));
        $("#divChoise").show();
        $("#divBtns").show();
    } else {
        $("#emptyInfo").show();
        $("#data-list").hide();
        $("#divChoise").hide();
        $("#divBtns").hide();
    }
    $("#data-list table:not(.ClearAlternate) tr:nth-child(even)").addClass("alternate");
    drawPagination($("#divPagination"), data.Pagination.PageIndex, data.Pagination.PageSize, data.Pagination.RowCount, queryOrders);
}

function queryOrders(pageIndex, pageSize) {
    if (!pageIndex) pageIndex = 1;
    pageSize = pageSize || 10;
    var condition = getQueryOrdersCondition();
    var pagination = getQueryOrdersPagination(pageIndex, pageSize);
    var parameters = JSON.stringify({ "pagination": pagination, "condition": condition });
    sendPostRequest("/OrganizationHandlers/DistributionOEM.ashx/QueryDistrutionUserList", parameters, function (data) {
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
    return { "RegisterBeginTime": $("#txtBeginTime").val(), "RegisterEndTime": $("#txtEndTime").val(),
        "UserNo": $.trim($("#txtUserNo").val()),  "IncomeGroup": $("#ddlIncomeGroup").val(),
        "AbbreviateName": $("#txtAbbreviateName").val(),
        "IncomeGroup": $("#hfdCurrentIncomeGroupId").val()
    };
}
function getQueryOrdersPagination(pageIndex, pageSize) {
    return { "PageSize": pageSize, "PageIndex": pageIndex, "GetRowCount": true };
}