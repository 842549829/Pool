$(function () {
    var txtCompanyName = function (e) { return Validate(["EmptyOrObject=公司名称不能为空", "CompanyName=null"], "txtCompanyName", e); };
    var txtCompanyShortName = function (e) { return Validate(["EmptyOrObject=公司简称不能为空", "CompanyName=公司简称格式错误"], "txtCompanyShortName", e); };
    var hidAddress = function () { if ($("#hidAddress").length < 1) { $("#lblLocation").text("请选择所在地"); return false; } else { $("#lblLocation").text(""); return true } };
    var txtPostCode = function (e) { return Validate(["EmptyOrObject=邮政编码不能为空", "PostCode=邮政编码格式错误"], "txtPostCode", e); };
    var txtAddress = function (e) { return Validate(["EmptyOrObject=地址不能为空"], "txtAddress", e) && ValidateLength(["Max=地址最多50个字符"], "txtAddress", 50, e); };
    var txtFaxes = function (e) { if ($("#txtFaxes").val().length > 0) { return Validate(["Phone=传真格式错误"], "txtFaxes", e) && ValidateLength(["Max=传真最多20个字符"], "txtFaxes", 20, e); } else { $("#txtFaxes").message(""); return true; } };
    var txtCompanyPhone = function (e) { return Validate(["EmptyOrObject=公司电话不能为空", "Phone=公司电话格式错误"], "txtCompanyPhone", e) && ValidateLength(["Max=公司电话最多100个字符"], "txtCompanyPhone", 100, e); };
    var txtEmail = function (e) { return Validate(["EmptyOrObject=Emial不能为空", "Email=Email格式错误"], "txtEmail", e) && ValidateLength(["Max=Email最多100个字符"],"txtEmail",100,e); };
    var txtPrincipal = function (e) { return Validate(["EmptyOrObject=负责人不能为空", "Name=负责人格式错误"], "txtPrincipal", e); };
    var txtPrincipalPhone = function (e) { return Validate(["EmptyOrObject=负责人不能为空", "CellPhone=负责人手机格式错误"], "txtPrincipalPhone", e); };
    var txtQQ = function (e) { if ($("#txtQQ").val().length > 0) { return Validate(["QQ=QQ格式错误"], "txtQQ", e) } else { $("#txtQQ").message(""); return true; } };
    var txtMSN = function (e) { if ($("#txtMSN").val().length > 0) { return Validate(["Email=MSN格式错误"], "txtMSN", e) && ValidateLength(["Max=MSN最多100个字符"],"txtMSN",100,e); } else { $("#txtMSN").message(""); return true; } };
    var txtLinkman = function (e) { return Validate(["EmptyOrObject=联系人不能为空", "Name=联系人格式错误"], "txtLinkman", e); };

    var txtLinkManPhone = function (e) { return Validate(["EmptyOrObject=联系人不能为空", "CellPhone=联系人手机格式错误"], "txtLinkManPhone", e); };
    var txtUrgencyLinkMan = function (e) { return Validate(["EmptyOrObject=紧急联系人不能为空", "Name=紧急联系人格式错误"], "txtUrgencyLinkMan", e); };
    var txtUrgencyLinkManPhone = function (e) { return Validate(["EmptyOrObject=紧急联系人不能为空", "CellPhone=紧急联系人手机格式错误"], "txtUrgencyLinkManPhone", e); };
    var txtUserName = function (e) { return Validate(["EmptyOrObject=用户名不能为空", "Name=用户名格式错误"], "txtUserName", e); };
    var txtPetName = function (e) { return Validate(["EmptyOrObject=昵称不能为空", "Name=昵称格式错误"], "txtPetName", e); };
    $("#btnSave").click(function () {
        document.getElementById("hidAddress").value = $(".areaData").val().length < 1 ? $("#hidAddress").val() : $(".areaData").val();
        if ($("#txtUrgencyLinkMan").length > 0 && !(txtCompanyName(true) && txtCompanyShortName(true))) { return false; }
        if ($("#txtUserName").length > 0 && !(txtUserName(true) && txtPetName(true))) { return false; }
        if (!(hidAddress() && txtPostCode(true) && txtAddress(true) && txtFaxes(true))) { return false; }
        if ($("#txtUrgencyLinkMan").length > 0 && !txtCompanyPhone(true)) { return false; }
        if (!(txtEmail(true) && txtLinkman(true) && txtLinkManPhone(true))) { return false; }
        if ($("#txtUrgencyLinkMan").length > 0 && !(txtPrincipal(true) && txtPrincipalPhone(true) && txtUrgencyLinkMan(true) && txtUrgencyLinkManPhone(true))) { return false; }
        if (!(txtQQ(true) && txtMSN(true))) { return false; }
    });
});