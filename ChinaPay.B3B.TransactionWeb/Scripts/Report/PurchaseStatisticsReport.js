$(function () {
    $("#btnDownload").click(function () {
        var startDate = $.trim($("#txtStartDate").val());
        var endDate = $.trim($("#txtEndDate").val());
        var departure = $.trim($("#txtDeparture_ddlAirports").val());
        var airlines = $.trim($("#ddlAirlines").val());
        var purchaseCompany = $.trim($("#txtPurchaseCompany_hidCompanyId").val());
        var hasTrade;
        if ($("#chkHasTrade").attr("checked") == "checked") {
             hasTrade = "true";
         } else {
             hasTrade = "false";
        }
        var purchaseStatisticsCondition = startDate + ',' + endDate + ',' + departure + ',' + airlines + ',' + purchaseCompany + ',' + hasTrade;
        window.open('ReportDownload.aspx?purchaseStatisticsCondition=' + purchaseStatisticsCondition, 'newWindow');
    });
    SaveDefaultData(null, '.text-s1');
})