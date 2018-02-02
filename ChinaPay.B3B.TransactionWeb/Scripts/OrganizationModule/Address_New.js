$(function () {
    var IsAddress = false;
    var addressCode = $("#hfldAddressCode"), ddlProvince = $("#ddlProvince"), ddlCity = $("#ddlCity"), ddlCounty = $("#ddlCounty");
    var html = "<option value=''>-请选择-</option>", url = "/OrganizationHandlers/Address.ashx/", provincesCode = "", citysCode = "";
    IsAddress = addressCode.length > 0 && addressCode.val().length > 0;
    if (IsAddress) { var address = JSON.parse(addressCode.val()); }
    //禁止select获取焦点的时候出发键盘上下事件
    $("#ddlProvince,#ddlCity,#ddlCounty").keydown(function (event) {
        var currKey = event.keyCode || event.which || event.charCode;
        if (currKey == 38 || currKey == 40) { $(this).blur(); return false; }
    });
    //获取数据
    var district = function (par) {
        var parmeter = par || $("#ddlCounty option:selected").val();
        addressCode.val(JSON.stringify({ "ProvinceCode": provincesCode, "CityCode": citysCode, "CountyCode": parmeter }));
    };
    //县
    var countys = function (par) {
        var countyHtml = html, parmeter = par || $("#ddlCity option:selected").val();
        citysCode = parmeter;
        sendPostRequest(url + "GetCounty",
        JSON.stringify({ "cityCode": parmeter }),
        function (data) {
            for (county in data) {
                countyHtml += "<option value=" + data[county].Code + ">" + data[county].Name + "</option>";
            }
            $("#ddlCounty option:gt(0)").remove();
            ddlCounty.html($(countyHtml));
            if (IsAddress) { $("#ddlCounty option[value='" + address.CountyCode + "']").attr("selected", true).change(district(address.CountyCode)); IsAddress = false; }
            ddlCounty.change(function () { district(); });
        });
    };
    //市
    var citys = function (par) {
        var cityHtml = html, parmeter = par || $("#ddlProvince option:selected").val();
        provincesCode = parmeter;
        sendPostRequest(url + "GetCities",
        JSON.stringify({ "provinceCode": parmeter }),
        function (data) {
            for (city in data) {
                cityHtml += "<option value=" + data[city].Code + ">" + data[city].Name + "</option>";
            }
            $("#ddlCity option:gt(0),#ddlCounty option:gt(0)").remove();
            ddlCity.html(cityHtml);
            if (IsAddress) { $("#ddlCity option:[value='" + address.CityCode + "']").attr("selected", true).change(countys(address.CityCode)); }
            ddlCity.change(function () { countys(); });
        });
    };
    //省
    sendPostRequest(url + "GteProvince", JSON.stringify({ "code": "" }), function (data) {
        var provinceHtml = html;
        for (province in data) {
            provinceHtml += "<option value=" + data[province].Code + ">" + data[province].Name + "</option>";
        }
        ddlProvince.html($(provinceHtml));
        if (IsAddress) { $("#ddlProvince option:[value='" + address.ProvinceCode + "']").attr("selected", true).change(citys(address.ProvinceCode)); }
        ddlProvince.change(function () { citys(); });
    });
});