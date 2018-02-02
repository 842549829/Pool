$(function () {
    var txtPostCode = function (e) { if ($("#txtPostCode").val().length > 0) { return Validate(["PostCode=null"], "txtPostCode", e); } else { $("#txtPostCode").message(""); return true; } };
    var txtFaxes = function (e) { if ($("#txtFaxes").val().length > 0) { return Validate(["Phone=传真格式错误"], "txtFaxes", e) && ValidateLength(["Max=传真最多20个字符"],"txtFaxes",20,e); } else { $("#txtFaxes").message(""); return true; } };
    var txtQQ = function (e) { if ($("#txtQQ").val().length > 0) { return Validate(["QQ=null"], "txtQQ", e); } else { $("#txtQQ").message(""); return true; } };
    var txtMSN = function (e) { if ($("#txtMSN").val().length > 0) { return Validate(["MSN=null"], "txtMSN", e) && ValidateLength(["Max=MSN最多100个字符"],"txtMSN",100,e); } else { $("txtMSN").message(""); return true; } };
    $("#txtPostCode").blur(function () { return txtPostCode(); });
    $("#txtFaxes").blur(function () { return txtFaxes(); });
    $("#txtEmail").blur(function () { return txtEmail(); });
    $("#txtQQ").blur(function () { return txtQQ(); });
    $("#txtMSN").blur(function () { return txtMSN(); });
    $("#txtBtnSave").click(function () { return txtPostCode(true) && txtFaxes(true) && txtQQ(true) && txtMSN(true); });
});