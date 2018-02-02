var errMsg = {
    EmptyOrObject: { msg: "用户名不能为空", fun: function (obj) { return obj.length > 0; } },
    Account: { msg: "用户名格式错误", fun: function (obj) { var reg = /(^\w+@\w+(\.\w{2,4}){1,2}$)|(^\w{6,30}$)/; return reg.test(obj); } },
    PassWordRegex: { msg: "密码格式不正确", fun: function (obj) { var reg = /^[^<>]{6,20}$/; return reg.test(obj); } },
    PassWord: { msg: "密码不能与账号一样", fun: function (obj) { return obj != $("#txtAccountNo").val(); } },
    Confirm: { msg: "两次密码不一致", fun: function (obj) { return obj == $("#txtPassWord").val(); } },
    Phone: { msg: "电话格式错误", fun: function (obj) { var reg = /((\(^0\d{2,3}\))|(^0\d{2,3}(-)?))\d{7,8}/; return reg.test(obj); } },
    Linkman: { msg: "联系人格式错误", fun: function (obj) { var reg = /(^[\u4e00-\u9fa5]{2,10}$)|(^[\u4e00-\u9fa5]{1,10}[a-z,A-Z]{1,10}$)|(^[a-z,A-Z]{1,9}\/[a-z,A-Z]{1,10}$)/; return reg.test(obj); } },
    Email: { msg: "Emali格式错误", fun: function (obj) { var reg = /^\w+@\w+(\.\w{2,4}){1,2}$/; return reg.test(obj); } },
    QQ: { msg: "QQ格式错误", fun: function (obj) { var reg = /^\d{5,12}$/; return reg.test(obj); } },
    PostCode: { msg: "邮编格式错误", fun: function (obj) { var reg = /^\d{6}$/; return reg.test(obj); } },
    CellPhone: { msg: "手机格式错误", fun: function (obj) { var reg = /^1[3458]\d{9}$/; return reg.test(obj); } },
    MSN: { msg: "MSN格式错误", fun: function (obj) { var reg = /^\w+@\w+(\.\w{2,4}){1,}$/; return reg.test(obj); } },
    CompanyName: { msg: "企业名称格式错误", fun: function (obj) { var reg = /^[\u4e00-\u9fa5]{4,25}$/; return reg.test(obj); } },
    VerifyCode: { msg: "验证码格式错误", fun: function (obj) { var reg = /^[\da-zA-z]{5}$/; return reg.test(obj); } },
    Name: { msg: "姓名格式错误", fun: function (obj) { var reg = /(^[\u4e00-\u9fa5]{2,10}$)|(^[\u4e00-\u9fa5]{1,10}[a-z,A-Z]{1,10}$)|(^[a-z,A-Z]{1,9}\/[a-z,A-Z]{1,10}$)/; return reg.test(obj); } },
    Licence: { msg: "航协经营批准号格式错误", fun: function (obj) { var reg = /^\d+$/; return reg.test(obj); } },
    Remark: { msg: "描述太长了", fun: function (obj) { var reg = /^[^<>]{0,100}$/; return reg.test(obj); } },
    Office: { msg: "Offcie格式错误", fun: function (obj) { var reg = /^[a-zA-Z]{3}\d{3}$/; return reg.test(obj); } },
    Offices: { msg: "Office格式错误", fun: function (obj) { var reg = /^[a-zA-z]{3}\d{3}(\/[a-zA-z]{3}\d{3})*$/; return reg.test(obj); } },
    Min: { msg: "", fun: function (obj, leg) { return obj > leg } },
    Max: { msg: "", fun: function (obj, leg) { return obj < leg } },
    OrganizationCode: { msg: "机构代码格式错误", fun: function (obj) { var reg = /^\d{8}-[\dxX]{1}$/; return reg.test(obj); } },
    Abbreviation: { msg: "企业简称格式错误", fun: function (obj) { var reg = /^[\u4e00-\u9fa5]{1,10}$/; return reg.test(obj); } },
    Address: { msg: "地址格式错误", fun: function (obj) { var reg = /^[^<>]{1,100}$/; return reg.test(obj); } },
    CellVerifyCode:{msg:"验证码错误",fun:function(obj){ var reg=/^[a-zA-z0-9]{6}$/; return reg.test(obj);}}
};
//验证
function Validate(parmeters, tipId, type) {
    var tip = $("#" + tipId),msg = tip.parent().children("span"),blnSuc = false;
    $.each(parmeters, function (index, value) {
        var par = parmeters[index].split("=");
        if (!errMsg[par[0]].fun($.trim(tip.val()))) {
            var html = par[1] != "null" ? par[1] : errMsg[par[0]].msg;
            if (typeof type != "undefined" && type == true) { tip.select(); tip.focus(); }
            msg.html(html).css("color","red"); blnSuc = false; return false;
        } else {
            msg.html("√").css("color","green"); blnSuc = true; return true;
        }
    });
    return blnSuc;
}
function ValidateLength(parmeters, tipId,length,type) {
    var tip = $("#" + tipId), msg = tip.parent().children("span"), blnSuc = false;
    $.each(parmeters, function (index, value) {
        var par = parmeters[index].split("=");
        if (!errMsg[par[0]].fun($.trim(tip.val()).length,length)) {
            var html = par[1] != "null" ? par[1] : errMsg[par[0]].msg;
            if (typeof type != "undefined" && type == true) { tip.select(); tip.focus(); }
            msg.html(html).css("color", "red"); blnSuc = false; return false;
        } else {
            msg.html("√").css("color", "green"); blnSuc = true; return true;
        }
    });
    return blnSuc;
}
function ValidatePassword(tip, tipId, messsgae, type,e) {
    var curvalue = $("#" + tip).val(), ls_data = $("#" + tipId).val(), isValid = false;
    switch (type) {
        case "=":
            isValid = (curvalue == ls_data);
            break;
        case "!=":
            isValid = (curvalue != ls_data);
            break;
        default:
           isValid = false;
            break;
    }
    if (!isValid) {
        $("#" + tipId).parent().children("span").html(messsgae).css("color","red");
        if ((typeof type != "undefinded") && (typeof e != "undefined")) { $("#" + tipId).select(); $("#" + tipId).focus(); }
    }
    return isValid;
}
$.fn.extend({
    message:function(msg){
        $(this).parent().children("span").html(msg).css("color", "#828282");
    }
});