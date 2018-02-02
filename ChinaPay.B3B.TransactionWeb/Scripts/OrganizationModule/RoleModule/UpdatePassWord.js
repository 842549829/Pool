$(function () {
    var txtOriginalPassword = function (e) { return Validate(["EmptyOrObject=原密码不能为空", "PassWordRegex=原密码格式不正确"], "txtOriginalPassword", e); };
    var txtNewPassword = function (e) { return Validate(["EmptyOrObject=新密码不能为空", "PassWordRegex=新密码格式不正确"], "txtNewPassword", e) && ValidatePassword("txtOriginalPassword", "txtNewPassword", "新密码不能与原密码一样", "!="); };
    var txtConfirmPassword = function (e) { return Validate(["EmptyOrObject=确认密码不能为空", "PassWordRegex=确认密码格式不正确"], "txtConfirmPassword", e) && ValidatePassword("txtNewPassword", "txtConfirmPassword", "两次密码不一致", "="); };
    $("#txtOriginalPassword").blur(function () { return txtOriginalPassword(); });
    $("#txtNewPassword").blur(function () { return txtNewPassword(); });
    $("#txtConfirmPassword").blur(function () { return txtConfirmPassword(); });
    $("#btnConfirmUpdate").click(function () { return (txtOriginalPassword(true) && txtNewPassword(true) && txtConfirmPassword(true)); });
});