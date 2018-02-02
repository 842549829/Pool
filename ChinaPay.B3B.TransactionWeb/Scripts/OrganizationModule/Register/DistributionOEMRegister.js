$(document).ready(function () {
    var person = function () {
        $('.personContent').show(); $('.companyContent').hide();
        if ($("#pIsPrivateAccountNo").length > 0) {
            $("#pIsPrivateAccountNo").hide();
            $("#chkIsPersonAccountNo").removeAttr("checked");
        }
        $("#trCompanyIDCard").hide();
    };
    var company = function () {
        $('.personContent').hide(); $('.companyContent').show();
        if ($("#pIsPrivateAccountNo").length > 0) {
            $("#pIsPrivateAccountNo").show();
        }
        $("#trCompanyIDCard").hide();
    };
    //账户类型切换
    $("#person").click(function () {
        person(); $("#hidAccountType").val("person");
    });
    $("#company").click(function () {
        company(); $("#hidAccountType").val("company");
    });
    if ($("#hidAccountType").val() == "person") {
        person(); $("#person").attr("checked", true);
    } else {
        company(); $("#company").attr("checked", true);
    }
    //获取验证码
    $("#btnPhoneCode").click(function () {
        var phone = $("#txtPhone").val(), account = $("#txtAccount").val();
        if (account.length < 1 || (account.length > 30 || !/(^\w+@\w+(\.\w{2,4}){1,2}$)|(^\w{6,30}$)/.test(account))) { alert("请输入正确的用户名"); return false; }
        if (phone.length < 1 || !/^1[3458]\d{9}$/.test(phone)) { alert("请输入正确的手机号码"); } else {
            $("#btnPhoneCode").attr("disabled", true);
            sendPostRequest("/OrganizationHandlers/Address.ashx/SendSMS", JSON.stringify({ "phone": phone, "account": account }),
            function (result) {
                if (result == "") { CountDown(120); } else { alert(result); $("#btnPhoneCode").attr("disabled", false); }
            }, function () { alert("发送验证码失败"); $("#btnPhoneCode").attr("disabled", false); });
        }
    });

    /*账号*/var txtAccount = function (e) { return Validate(["EmptyOrObject=null", "Account=null"], "txtAccount", e) && ValidateLength(["Max=用户名格式错误"], "txtAccount", 30, e); };
    /*自定义国付通账号*/var txtPoolPayUserName = function (e) { return Validate(["EmptyOrObject=null", "Account=null"], "txtPoolPayUserName", e) && ValidateLength(["Max=用户名格式错误"], "txtPoolPayUserName", 30, e); };
    /*密码*/var txtPassword = function (e) { return Validate(["EmptyOrObject=密码不能为空", "PassWordRegex=null"], "txtPassword", e) && ValidatePassword("txtAccount", "txtPassword", "密码不能与账号一样", "!=", e); };
    /*确认密码*/var txtConfirmPassword = function (e) { return Validate(["EmptyOrObject=密码不能为空", "PassWordRegex=确认密码格式错误"], "txtConfirmPassword", e) && ValidatePassword("txtPassword", "txtConfirmPassword", "两次密码不一致", "=", e); };
    $("#txtAccount").focus(function () { $(this).message("账户名是大于6位的平台登录账号"); }).blur(function () {
        var result = txtAccount();
        if ($(this).parent().children("span").html() == "√") { $(this).message(""); }
        return result;
    });
    $("#txtPoolPayUserName").focus(function () { $(this).message("账户名是大于6位的平台登录账号"); }).blur(function () {
        var result = txtPoolPayUserName();
        if ($(this).parent().children("span").html() == "√") { $(this).message(""); }
        return result;
    });
    $("#txtPassword").focus(function () { $(this).message("建议使用6位以上的数字+字母组合的密码"); }).blur(function () { return txtPassword(); });
    $("#txtConfirmPassword").focus(function () { $(this).message("请重复输入您的登录密码并牢记"); }).blur(function () { return txtConfirmPassword(); });

    /**********个人**********/
    /*姓名*/var txtName = function (e) { return Validate(["EmptyOrObject=姓名不能为空", "Name=姓名格式错误"], "txtName", e); };
    /*身份证*/var txtIDCardValues = function (e) {
        var txtIDCard = $("#txtIDCard");
        if (txtIDCard.val().length < 1) {
            txtIDCard.parent().children("span").html("身份证不能为空").css("color", "red");
            if (typeof e != "undefined" && e == true) { txtIDCard.select(); txtIDCard.focus(); } return false;
        }
        if (!isCardID(txtIDCard.val())) {
            txtIDCard.parent().children("span").html("身份证格式错误").css("color", "red");
            if (typeof e != "undefined" && e == true) { txtIDCard.select(); txtIDCard.focus(); } return false;
            return false;
        }
        txtIDCard.parent().children("span").html("√").css("color", "green");
        return true;
    };
    $("#txtName").focus(function () { $(this).message("请填写身份证上的姓名，否则会造成结算失败"); }).blur(function () { return txtName(); });
    $("#txtIDCard").focus(function () { $(this).message("身份证号是您的平台购票、兑换礼品等的必要标识，请认真填写"); }).blur(function () { return txtIDCardValues(); });
    /*****企业*******/
    /*企业名称*/var txtCompany = function (e) { return Validate(["EmptyOrObject=企业名称不能为空", "CompanyName=企业名称格式错误"], "txtCompany", e); };
    /*企业简称*/var txtAbbreviation = function (e) { return Validate(["EmptyOrObject=企业简称不能为空", "Abbreviation=企业简称格式错误"], "txtAbbreviation", e); };
    /*机构代码*/var txtOrganizationCode = function (e) { return Validate(["EmptyOrObject=机构代码不能为空", "OrganizationCode=null"], "txtOrganizationCode", e); };
    /*企业电话*/var txtCompanyPhone = function (e) { return Validate(["EmptyOrObject=企业电话不能为空", "Phone=企业电话格式错误"], "txtCompanyPhone", e); };
    /*联系人*/var txtContact = function (e) { return Validate(["EmptyOrObject=联系人不能为空", "Name=联系人格式错误"], "txtContact", e); };
    /*企业联系人身份证号码*/
    var txtCompanyIDCard = function (e) {
        var txtIDCard = $("#txtCompanyIDCard");
        if (txtIDCard.val().length < 1) {
            txtIDCard.parent().children("span").html("身份证不能为空").css("color", "red");
            if (typeof e != "undefined" && e == true) { txtIDCard.select(); txtIDCard.focus(); } return false;
        }
        if (!isCardID(txtIDCard.val())) {
            txtIDCard.parent().children("span").html("身份证格式错误").css("color", "red");
            if (typeof e != "undefined" && e == true) { txtIDCard.select(); txtIDCard.focus(); } return false;
            return false;
        }
        txtIDCard.parent().children("span").html("√").css("color", "green");
        return true;
    };
    $("#txtCompanyIDCard").focus(function () { $(this).message("身份证号是您的平台购票、兑换礼品等的必要标识，请认真填写"); }).blur(function () { return txtCompanyIDCard(); });
    $("#txtCompany").focus(function () { $(this).message("请填写您营业执照上的名称，否则会造成结算失败"); }).blur(function () {
        var result = txtCompany();
        if ($(this).parent().children("span").html() == "√") { $(this).message(""); }
        return result;
    });
    $("#txtAbbreviation").focus(function () { $(this).message("请填写您企业常用的简称，建议四个汉字，如博宇科技"); }).blur(function () {
        var result = txtAbbreviation();
        if ($(this).parent().children("span").html() == "√") { $(this).message(""); }
        return result;
    });
    $("#txtOrganizationCode").focus(function () { $(this).message("填写您组织机构代码证上的代码如53038123-9"); }).blur(function () { return txtOrganizationCode(); });
    $("#txtCompanyPhone").focus(function () { $(this).message("认真填写您的企业电话方便客服进行相关事宜的通知"); }).blur(function () { return txtCompanyPhone(); });
    $("#txtContact").focus(function () { $(this).message("填写联系人有助于在购票或售票时进行相关信息的通知如航班变更等"); }).blur(function () { return txtContact(); });

    /*手机*/var txtPhone = function (e) { return Validate(["EmptyOrObject=手机号不能为空", "CellPhone=null"], "txtPhone", e); };
    /*手机验证码*/var txtPhoneCode = function (e) { return Validate(["EmptyOrObject=验证码不能为空", "CellVerifyCode=手机验证码格式不正确"], "txtPhoneCode", e); };
    $("#txtPhone").focus(function () { $(this).message("请输入11位的手机号码"); }).blur(function () { return txtPhone(); });
    $("#txtPhoneCode").focus(function () { $(this).message("如2分钟内没有收到验证码，请点击重新发送"); }).blur(function () { return txtPhoneCode(); });

    //提交
    $("#btnSubmit").click(function () {
        if (!(txtAccount(true) && txtPassword(true) && txtConfirmPassword(true))) return false;
        if ($("#txtPoolPayUserName:visible").length > 0) {
            if (!txtPoolPayUserName(true)) { return false; }
        }
        if ($("#hidAccountType").val() == "person") {
            if (!(txtName(true) && txtIDCardValues(true))) return false;
        } else {
            if (!(txtCompany(true) && txtAbbreviation(true) && txtOrganizationCode(true) && txtCompanyPhone(true))) return false;
            if (!txtContact(true)) return false;
            if (doc("chkIsPersonAccountNo").checked) {
                if (!txtCompanyIDCard(true)) return false;
            }
        }
        if (!(txtPhone(true))) return false;
        if ($("#txtPhoneCode").length > 0) { if (!txtPhoneCode(true)) return false; }
    });

    isShow("none");
    doc("aPoolPayUserName").onclick = function () {
        var self = this;
        var isShowPoolPay = self.innerText == "自定义国付通账号" || self.text == "自定义国付通账号";
        var isHidePoolPay = !isShowPoolPay ? "none" : "";
        self.innerText = self.text = isShowPoolPay ? "取消自定义国付通账号" : "自定义国付通账号";
        doc("txtPoolPayUserName").value = "";
        isShow(isHidePoolPay);
    };
    doc("chkIsPersonAccountNo").onclick = function () {
        if (this.checked) {
            doc("trCompanyIDCard").style.display = "";
        } else {
            doc("trCompanyIDCard").style.display = "none";
        }
    };

});
//短信倒计时
function CountDown(num) {
    $("#btnPhoneCode").attr("disabled", true);
    if (num == -1) {
        $("#btnPhoneCode").attr("disabled", false).val("重新获取");
        return;
    } else {
        $("#btnPhoneCode").val(num + "秒后可获取");
        setTimeout("CountDown(" + --num + ")", 1000);
    }
}
function isShow(isHidePoolPay) {
    var trPoolPayUserName = doc("trPoolPayUserName");
    trPoolPayUserName.style.display = isHidePoolPay;
    for (var i = 0; i < trPoolPayUserName.children.length; i++)
        trPoolPayUserName.children[i].style.display = isHidePoolPay;
}
function doc(id) { return document.getElementById(id); }