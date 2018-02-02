$(function () {
    /*帐号*/var txtAccountNo = function (e) { return Validate(["EmptyOrObject=用户名不能为空", "Account=用户名格式错误"], "txtAccountNo", e) && ValidateLength(["Max=用户名格式错误"], "txtAccountNo", 30, e); };
    /*密码*/var txtPassword = function (e) { return Validate(["EmptyOrObject=密码不能为空", "PassWordRegex=请正确设置您的登录密码"], "txtPassword", e) && ValidatePassword("txtAccountNo", "txtPassword", "密码不能与用户名一样", "!=", e); };
    /*确认密码*/var txtConfirmPassword = function (e) { return Validate(["EmptyOrObject=密码不能为空", "PassWordRegex=确认密码格式错误"], "txtConfirmPassword", e) && ValidatePassword("txtPassword", "txtConfirmPassword", "两次密码不一致", "=", e); };
    $("#txtAccountNo").focus(function () { $(this).message("用户名是大于6位的平台帐号"); }).blur(function () {
        var result = txtAccountNo();
        if ($(this).parent().children("span").html() == "√") { $(this).message(""); }
        return result;
    });
    $("#txtPassword").focus(function () { $(this).message("建议使用6位以上的数字+字母组合的密码"); }).blur(function () { return txtPassword(); });
    $("#txtConfirmPassword").focus(function () { $(this).message("请重复输入您的登录密码并牢记"); }).blur(function () { return txtConfirmPassword(); });
    /***个人****/
    /*个人真实姓名*/var txtPresonName = function (e) { return Validate(["EmptyOrObject=真实姓名不能为空", "Name=输入有误，仅允许输入大于2个字符"], "txtPresonName", e); };
    /*个人身份证*/var txtPresonIDCard = function (e) { return IdCard($("#txtPresonIDCard"), e); };
    /*个人手机*/var txtPresonPhone = function (e) { return Validate(["EmptyOrObject=手机不能为空", "CellPhone=输入错误,请输入11位正确的手机号码"], "txtPresonPhone", e); };
    /****企业****/
    /*企业名称*/var txtCompany = function (e) { return Validate(["EmptyOrObject=企业名称不能为空", "CompanyName=输入有误,仅允许输入大于4个字符的汉字"], "txtCompany", e); };
    /*企业简称*/var txtAbbreviation = function (e) { return Validate(["EmptyOrObject=企业简称不能为空", "Abbreviation=请填写正确的公司简称"], "txtAbbreviation", e); };
    /*机构代码*/var txtOrganizationCode = function (e) { return Validate(["EmptyOrObject=机构代码不能为空", "OrganizationCode=输入错误，请检查后重新输入"], "txtOrganizationCode", e); };
    /*企业电话*/var txtCompanyPhone = function (e) { return Validate(["EmptyOrObject=企业电话不能为空", "Phone=请输入正确的电话如028-8883388"], "txtCompanyPhone", e); };
    /*身份证*/var txtCompanyIdCard = function (e) { return IdCard($("#txtCompanyIDCard"), e); };
    /*联系人*/var txtContact = function (e) { return Validate(["EmptyOrObject=联系人不能为空", "Name=输入有误，仅允许输入大于2个字符"], "txtContact", e); };
    /*联系人手机*/var txtContactPhone = function (e) { return Validate(["EmptyOrObject=联系人手机不能为空", "CellPhone=输入错误，请输入11位正确的手机号码"], "txtContactPhone", e); };
    /*负责人*/var txtMangerName = function (e) { return Validate(["EmptyOrObject=负责人不能为空", "Name=输入有误，仅允许输入大于2个字符"], "txtMangerName", e); };
    /*负责人手机*/var txtManagerCellphone = function (e) { return Validate(["EmptyOrObject=负责人手机不能为空", "CellPhone=输入错误，请输入11位正确的手机号码"], "txtManagerCellphone", e); };
    /*紧急联系人*/var txtEmergencyContact = function (e) { return Validate(["EmptyOrObject=紧急联系人不能为空", "Name=输入有误，仅允许输入大于2个字符"], "txtEmergencyContact", e); };
    /*紧急联系人手机*/var txtEmergencyPhone = function (e) { return Validate(["EmptyOrObject=紧急联系人手机不能为空", "CellPhone=输入错误，请输入11位正确的手机号码"], "txtEmergencyPhone", e); };
    $("#txtCompany").focus(function () { $(this).message("请填写您营业执照上的名称，否则会造成结算失败"); }).blur(function () {
        var result = txtCompany();
        if ($(this).parent().children("span").html() == "√") { $(this).message(""); }
        return result;
     });
    $("#txtAbbreviation").focus(function () { $(this).message("建议四个汉字，如博宇科技"); }).blur(function () {
        var result = txtAbbreviation();
        if ($(this).parent().children("span").html() == "√") { $(this).message(""); }
        return result;
    });
    $("#txtOrganizationCode").focus(function () { $(this).message("填写您组织机构代码证上的代码如53038123-x"); }).blur(function () { return txtOrganizationCode(); });
    $("#txtCompanyPhone").focus(function () { $(this).message("认真填写您的企业电话方便客服进行相关事宜的通知"); }).blur(function () { return txtCompanyPhone(); });
    $("#txtCompanyIDCard").focus(function () { $(this).message("身份证号是您的平台购票、兑换礼品等的必要标识，请认真填写"); }).blur(function () { return txtCompanyIdCard(); });
    $("#txtContact").focus(function () { $(this).message("填写联系人有助于在购票或售票时进行相关信息的通知如航班变更等"); }).blur(function () { return txtContact(); });
    $("#txtContactPhone").focus(function () { $(this).message("填写联系人手机号方便在航班变动、出票、退票等情况下进行短信通知"); }).blur(function () { return txtContactPhone(); });
    $("#txtMangerName").focus(function () { $(this).message("负责人用于备用联系"); }).blur(function () { return txtMangerName(); });
    $("#txtManagerCellphone").focus(function () { $(this).message("请填写负责人手机号码"); }).blur(function () { return txtManagerCellphone(); });
    $("#txtEmergencyContact").focus(function () { $(this).message("紧急联系人用于航班变更等通知"); }).blur(function () { return txtEmergencyContact(); });
    $("#txtEmergencyPhone").focus(function () { $(this).message("请填写正确的手机号码"); }).blur(function () { return txtEmergencyPhone(); });

    /*公用*/
    /*所在地*/var ddlCounty = function () { return address($("#ddlCounty")); };
    /*Email*/var txtEmail = function (e) { return Validate(["EmptyOrObject=Email不能为空", "Email=Email格式错误"], "txtEmail", e); };
    /*QQ*/var txtQQ = function (e) { if ($("#txtQQ").val().length > 0) { return Validate(["QQ=QQ号码为5-12位纯数字，请正确输入"], "txtQQ", e); } else { $("#txtQQ").message(""); return true; } };
    /*传真*/var txtFaxes = function (e) { if ($("#txtFaxes").val().length > 0) { return Validate(["Phone=输入错误，格式如028-8883388"], "txtFaxes", e); } else { $("#txtFaxes").message(""); return true; } };
    /*地址*/var txtAddress = function (e) { if ($("#txtAddress").val().length > 0) { return Validate(["Address=null"], "txtAddress", e); } else { $("#txtAddress").message(""); return true; } };
    /*邮编*/var txtPostCode = function (e) { if ($("#txtPostCode").val().length > 0) { return Validate(["PostCode=格式错误，邮编为纯数字"], "txtPostCode", e); } else { $("#txtPostCode").message(""); return true; } };

    /*验证码*/var txtverifyCode = function (e) { return Validate(["EmptyOrObject=验证码不能为空", "VerifyCode=null"], "txtverifyCode", e); };
    $("#txtPresonName").focus(function () { $(this).message("请填写身份证上的姓名，否则会造成结算失败"); }).blur(function () { return txtPresonName(); });
    $("#txtPresonIDCard").focus(function () { $(this).message("身份证号是您的平台购票、兑换礼品等的必要标识，请认真填写"); }).blur(function () { return txtPresonIDCard(); });
    $("#txtPresonPhone").focus(function () { $(this).message("手机号码用于短信消息通知"); }).blur(function () { return txtPresonPhone(); });
    $("#txtAddress").focus(function () { $(this).message("地址用于邮寄礼品或对账单等"); }).blur(function () { return txtAddress(); });
    $("#txtQQ").focus(function () { $(this).message("填写QQ方便在线沟通"); }).blur(function () { return txtQQ(); });
    $("#txtFaxes").focus(function () { $(this).message("用于接收平台所发的资料等"); }).blur(function () { return txtFaxes(); });
    $("#txtPostCode").focus(function () { $(this).message("请填写当地的邮政编码"); }).blur(function () { return txtPostCode(); });
    $("#txtEmail").focus(function () { $(this).message("请填写您常用的email"); }).blur(function () { return txtEmail(); });
    $("#txtverifyCode").blur(function () { return txtverifyCode(); });
    //提交
    $("#btnSubmit").click(function () {
        if (!(txtAccountNo(true) && txtPassword(true) && txtConfirmPassword(true))) return false;
        if ($(".companyContent").is(":hidden")) {
            if (!(txtPresonName(true) && txtPresonIDCard(true) && txtPresonPhone(true))) return false;
        } else {
            if (!(txtCompany(true) && txtAbbreviation(true) && txtAbbreviation(true) && txtOrganizationCode(true) && txtCompanyPhone(true) && txtContact(true) && txtCompanyIdCard(true) && txtContactPhone(true) && txtMangerName(true) && txtManagerCellphone(true) && txtEmergencyContact(true) && txtEmergencyPhone(true))) return false;
        }
        if (!(txtEmail(true) && ddlCounty() && txtAddress(true) && txtQQ(true) && txtFaxes(true) && txtPostCode(true))) return false;
        if ($("#tdverifyCode").length > 0) { if (!txtverifyCode(true)) return false; }
        if ($("#hfdIsOem").val()=="False" && !document.getElementById("chkReadingProtocol").checked) {
            alert("你还没有同意B3B协议");
            return false;
        }
    });
});
function address(self) {
    if ($("option:selected",self).val().length < 1) {
        self.parent().children("span").html("请选择所在地").css("color", "red");
        return false;
    } else {
        self.parent().children("span").html("√").css("color", "green");
        return true;
    }
}
function IdCard(self, e) {
    if (self.val().length < 1) {
        self.parent().children("span").html("身份证不能为空").css("color", "red");
        if (typeof e != "undefined" && e == true) { self.select(); self.focus(); } 
        return false;
    }
    if (!isCardID(self.val())) {
        self.parent().children("span").html("身份证格式错误").css("color", "red");
        if (typeof e != "undefined" && e == true) { self.select(); self.focus(); } return false;
        return false;
    }
    self.parent().children("span").html("√").css("color", "green");
    return true;
}