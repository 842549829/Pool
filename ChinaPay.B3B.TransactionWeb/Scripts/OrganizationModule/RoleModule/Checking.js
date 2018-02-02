$(function () {
    $("#btnCheCkAccounNo").click(function () { return validateAccountNo($(this).prev(":text")); });
    $("#btnCheckCompanyName").click(function () { return validateCompanyName($(this).prev(":text")); });
    $("#btnCheCkCompanyShortName").click(function () { return validateCompanyAbbrivateName($(this).prev(":text")); });
    $("#btnCheCkPetName").click(function () { return validatePetName($(this).prev(":text")); });
});
function validateAccountNo(sender) {
    var reg = /^[a-zA-Z][\da-zA-z]{5,20}$/;if (reg.test(sender.val())) { validate(sender, "CheckUpUserNo", { "userNo": sender.val() }, "用户名不能为空", "该用户名可以使用", "该用户名已经存在", "验证用户名异常"); }return false;}
var reg = /^[\da-zA-z\u4e00-\u9fa5]{1,16}$/;
function validateCompanyName(sender) {
    if (reg.test(sender.val())) { validate(sender, "CheckUpCompanyName", { "companyName": $(sender).val() }, "公司名称不能为空", "该公司名称可以使用", "该公司名称已经存在", "验证公司名称异常"); }return false;}
function validateCompanyAbbrivateName(sender) {
    if (reg.test(sender.val())) { validate(sender, "CkeckUpCompanyForShort", { "companyForShort": $(sender).val() }, "公司简称不能为空", "该公司简称可以使用", "该公司简称已经存在", "验证公司简称异常"); } return false;}
function validatePetName(sender) {
    if (reg.test(sender.val())) { validate(sender, "CheckUpPetName", { "petName": $(sender).val() }, "昵称不能为空", "该昵称可以使用", "该昵称已经存在", "验证昵称异常"); }return false;}
function validate(sender, method, parameters, emptyMessage, successMessage, failedMessage, errorMessage) {
    var url = "../../../../OrganizationHandlers/CheckUpComPanyNews.ashx/" + method;
    var parameter = JSON.stringify(parameters);
    sender.next(":button").attr("disabled", true);
    sendPostRequest(url, parameter, function (e) {
        sender.next(":button").attr("disabled", false);
        if (e == false) { showMessage(sender, successMessage); } else { showMessage(sender, failedMessage); }
    }, function (e) {
        sender.next(":button").attr("disabled", false);
        showMessage(sender, errorMessage);
     });
}
function showMessage(sender, message) {$(sender).next().next("span").html(message);}