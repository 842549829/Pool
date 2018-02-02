$(function () {
    var txtName = function (e) { return Validate(["EmptyOrObject=姓名不能为空", "Name=null"], "txtName", e); };
    var txtAccountNo = function (e) { return Validate(["EmptyOrObject=用户名不能为空", "Account=用户名格式错误"], "txtAccountNo", e); };
    var txtPassword = function (e) { return Validate(["EmptyOrObject=密码不能为空"], "txtPassword", e) && ValidatePassword("txtAccountNo", "txtPassword", "密码不能与账号一样", "!="); };
    var txtConfirmPassword = function (e) { return Validate(["EmptyOrObject=确认密码不能为空"], "txtConfirmPassword", e) && ValidatePassword("txtPassword", "txtConfirmPassword", "两次密码不一致", "="); };
    var txtCellPhone = function (e) { return Validate(["EmptyOrObject=手机不能为空", "CellPhone=null"], "txtCellPhone", e); };
    var txtPhone = function (e) { return Validate(["EmptyOrObject=座机不能为空", "Phone=座机格式错误"], "txtPhone", e); };
    var txtEmail = function (e) { return Validate(["EmptyOrObject=Email不能为空", "Email=Email格式错误"], "txtEmail", e); };
    var remark = function (e) { if ($("#remark").text().length > 0) { return Validate(["Remark=备注最多100个字符"], "remark", e); } else { $("#remark").message(""); return true; } };
    if ($("#txtName").length > 0) {
        $("#txtName").blur(function () { return txtName(); });
        $("#txtAccountNo").blur(function () { return txtAccountNo(); });
        $("#txtPassword").blur(function () { return txtPassword(); });
        $("#txtConfirmPassword").blur(function () { return txtConfirmPassword(); });
    }
    $("#txtCellPhone").blur(function () { return txtCellPhone(); });
    $("#txtPhone").blur(function () { return txtPhone(); });
    $("#txtEmail").blur(function () { return txtEmail(); });
    $("#remark").blur(function () { return remark(); });
    var Save = function () {
        if ($("#txtName").length > 0 && !(txtName(true) && txtAccountNo(true) && txtPassword(true) && txtConfirmPassword(true))) { return false; }
        if (!(txtCellPhone(true) && txtPhone(true) && txtEmail(true) && remark(true))) { return false; }
        return true;
    }
    $("#btnSave").click(function () { return Save(); });
    if ($("#btnSaveAndContinue").length > 0) { $("#btnSaveAndContinue").click(function () { return Save(); }); }
});