$(function () {
    sendPostRequest("/../../OrganizationHandlers/Address.ashx/Addresses", null,function (e) { areaSel(e); }, function (e) {  });
});
function Address() {$("#hidAddress").val($(".areaData").val());}
