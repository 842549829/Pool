$(function () {
    $("#dl_ddlProvince dd").click(function () {
        $("#ddlCity option").remove();
        $("#hidCity").val("");
        ResetCities();
    });
    $("#dl_ddlCity dd").live("click", function () {
        $("#hidCity").val($("#ddlCity").val());
    });
    ResetCities();
    SaveDefaultData();
});
function ResetCompanyPWd(companyId) {
    $("#hdCompanyAccount").val(companyId);
    $(".fixed,.layers").show();
}
function CancleReset() {
    $(".fixed,.layers").hide();
}
function ResetCities() {
    var provinceCode = $("#ddlProvince").val();
    $("#ddlCity").removeClass("custed");
    $("#dl_ddlCity").remove();
    $("<option value='' selected='selected'>全部</option>").appendTo("#ddlCity");
    if (provinceCode == '') {
        $("#ddlCity").custSelect({ width: "134px" });
    } else {
        sendPostRequest('/OrganizationHandlers/Address.ashx/GetCities',
                   JSON.stringify({ provinceCode: $("#ddlProvince").val() }), function (data) {
                       var cities = $("#ddlCity");
                       if (data) {
                           $(data).each(function (index, item) {
                               if (item.Code == $("#hidCity").val()) {
                                   $("<option value='" + item.Code + "' selected='selected'>" + item.Name + "</option>").appendTo(cities);
                               } else {
                                   $("<option value='" + item.Code + "'>" + item.Name + "</option>").appendTo(cities);
                               }
                           });
                       }
                       cities.custSelect({ width: "134px" });
                   });
    }
}


function CheckReason() {
    var val = $("#Reason").text();
    if ($.trim(val) == "") {
        alert("重置密码，必须输入原因！");
        $("#Reason").select();
        return false;
    }
    return true;
}

//FixTable("dataListTable", 12, 760);
