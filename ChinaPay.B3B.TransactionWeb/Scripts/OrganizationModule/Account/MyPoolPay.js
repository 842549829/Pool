$(document).ready(function () {
    var divPay = $("#divPay"), divCollection = $("#divCollection");
    var isDivCollection = divCollection.length > 0;
    divPay.show(); if (isDivCollection) divCollection.hide();
    //收付款账号切换
    $("#account_list_box>ul>li").click(function () {
        $("#account_list_box>ul>li").removeClass("curr")
        var self = $(this);
        self.addClass("curr");
        var id = self.attr("id");
        if (id === "liShowPay") {
            divPay.show();
            if (isDivCollection) divCollection.hide();
        } else if (id === "liShowCollection") {
            if (isDivCollection) { divPay.hide(); divCollection.show(); }
        } else if (id === "liHideCollection") {
            window.location.href = "./RegisterCollectionAccount.aspx";
        }
    });
    //个人企业账号切换
    $("#rdoIndividual").click(function () { $("#preson").show(); $("#company").hide(); });
    $("#rdoEnterprise").click(function () { $("#company").show(); $("#preson").hide(); })
    //更换验证 
    var txtNewPayAccount = function (e) { return Validate(["EmptyOrObject=收款账号不能为空", "Account=收款账号格式错误"], "txtNewPayAccount", e) && ValidateLength(["Max=收款账号格式错误"], "txtNewPayAccount", 30, e); };
    var txtNewPayPassword = function (e) { return Validate(["EmptyOrObject=支付密码不能为空", "PassWordRegex=支付密码格式错误"], "txtNewPayPassword", e); };
    $("#txtNewPayAccount").focus(function () { $(this).message("请认真核对后输入"); }).blur(function () { return txtNewPayAccount(); });
    $("#txtNewPayPassword").focus(function () { $(this).message("请输入新收款账号的支付密码"); }).blur(function () { return txtNewPayPassword(); });
    //注册账号验证
    /*账号*/var txtRegisterAccountNo = function (e) { return Validate(["EmptyOrObject=null", "Account=null"], "txtRegisterAccountNo", e) && ValidateLength(["Max=用户名格式错误"], "txtAccount", 30, e); };
    /*密码*/var txtLoginPassword = function (e) { return Validate(["EmptyOrObject=密码不能为空", "PassWordRegex=null"], "txtLoginPassword", e) && ValidatePassword("txtRegisterAccountNo", "txtLoginPassword", "密码不能与账号一样", "!=", e); };
    /*确认密码*/var txtConfirmLoginPassword = function (e) { return Validate(["EmptyOrObject=密码不能为空", "PassWordRegex=确认密码格式错误"], "txtConfirmLoginPassword", e) && ValidatePassword("txtLoginPassword", "txtConfirmLoginPassword", "两次密码不一致", "=", e); };
    /*支付密码*/var txtRegisterPayPassword = function (e) { return Validate(["EmptyOrObject=支付密码不能为空", "PassWordRegex=支付密码格式错误"], "txtRegisterPayPassword", e) && ValidatePassword("txtLoginPassword", "txtRegisterPayPassword", "支付密码不能与密码一样", "!=", e); };
    /*确认支付密码*/var txtConfirmRegisterPayPassword = function (e) { return Validate(["EmptyOrObject=支付密码不能为空", "PassWordRegex=确认支付密码格式错误"], "txtConfirmRegisterPayPassword", e) && ValidatePassword("txtRegisterPayPassword", "txtConfirmRegisterPayPassword", "两次支付密码不一致", "=", e); };
    $("#txtRegisterAccountNo").focus(function () { $(this).message("请输入国付通账号"); }).blur(function () { return txtRegisterAccountNo(); });
    $("#txtLoginPassword").focus(function () { $(this).message("请输入登录密码"); }).blur(function () { return txtLoginPassword(); });
    $("#txtConfirmLoginPassword").focus(function () { $(this).message("请重复登录密码并牢记"); }).blur(function () { return txtConfirmLoginPassword(); });
    $("#txtRegisterPayPassword").focus(function () { $(this).message("请输入与登录密码不同的支付密码"); }).blur(function () { return txtRegisterPayPassword(); });
    $("#txtConfirmRegisterPayPassword").focus(function () { $(this).message("请重复支付密码并牢记"); }).blur(function () { return txtConfirmRegisterPayPassword(); });

    //姓名需与身份证及银行卡姓名一致，否则无法结算   填写11位的手机号码  填写您的身份证号
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
    /*个人手机*/var txtPresonCellPhone = function (e) { return Validate(["EmptyOrObject=手机号不能为空", "CellPhone=null"], "txtPresonCellPhone", e); };
    $("#txtName").focus(function () { $(this).message("请填写身份证上的姓名，否则会造成结算失败"); }).blur(function () { return txtName(); });
    $("#txtIDCard").focus(function () { $(this).message("请认真填写您的身份证号码"); }).blur(function () { return txtIDCardValues(); });
    $("#txtPresonCellPhone").focus(function () { $(this).message("请输入11位的手机号码"); }).blur(function () { return txtPresonCellPhone(); });

    /*企业名称*/var txtCompanyName = function (e) { return Validate(["EmptyOrObject=企业名称不能为空", "CompanyName=企业名称格式错误"], "txtCompanyName", e); };
    /*机构代码*/var txtOrganizationCode = function (e) { return Validate(["EmptyOrObject=机构代码不能为空", "OrganizationCode=null"], "txtOrganizationCode", e); };
    /*法人电话*/var txtCompanyPhone = function (e) { return Validate(["EmptyOrObject=法人手机不能为空", "CellPhone=法人手机格式错误"], "txtCompanyPhone", e); };
    /*发人姓名*/var txtLegalPersonName = function (e) { return Validate(["EmptyOrObject=法人姓名不能为空", "Name=法人姓名格式错误"], "txtLegalPersonName", e); };
    /*企业联系人手机*/var txtCompanyCellPhone = function (e) { return Validate(["EmptyOrObject=手机号不能为空", "CellPhone=null"], "txtCompanyCellPhone", e); };
    $("#txtCompanyName").focus(function () { $(this).message("请填写您营业执照上的名称，否则会造成结算失败"); }).blur(function () { return txtCompanyName(); });
    $("#txtOrganizationCode").focus(function () { $(this).message("填写您组织机构代码证上的代码如53038123-9"); }).blur(function () { return txtOrganizationCode(); });
    $("#txtCompanyPhone").focus(function () { $(this).message("认真填写您的法人手机方便客服进行相关事宜的通知"); }).blur(function () { return txtCompanyPhone(); });
    $("#txtLegalPersonName").focus(function () { $(this).message("填写法人姓名"); }).blur(function () { return txtLegalPersonName(); });
    $("#txtCompanyCellPhone").focus(function () { $(this).message("请输入11位的手机号码"); }).blur(function () { return txtCompanyCellPhone(); });
    //打开更换收款账号
    $("#replacementOpen").click(function () {
        $("#lblOriginal").text($("#lblCollectionAccountNo").text());
        $("#replacement").click();
    });
    //更换收款账号
    $("#btnReplacement").click(function () {
        var self = $(this);
        if (!(txtNewPayAccount(true) && txtNewPayPassword(true))) return false;
        self.attr("disabled", "disabled");
        var parameter = JSON.stringify({
            "originalAccountNo": $.trim($("#lblOriginal").text()),
            "newAccountNo": $.trim($("#txtNewPayAccount").val()),
            "payPassword": $.trim($("#txtNewPayPassword").val())
        });
        sendPostRequest("/OrganizationHandlers/Account.ashx/ReplacementAccountNo", parameter, function (relust) {
            $(".confirmClose,#replacementSuccess").click();
            $("#lblNewAccountNo").text($("#txtNewPayAccount").val());
            self.removeAttr("disabled");
        }, function (e) { self.removeAttr("disabled"); alert(e.responseText); });
    });
    //打开注册收款账号
    $("#registerAccountNoOpen").click(function () { $(".confirmClose,#registerAccountNo").click(); });
    //注册收款账号
    $("#btnConfirmPayAccountNo").click(function () {
        var self = $(this);
        if (!(txtRegisterAccountNo(true) && txtLoginPassword(true) && txtConfirmLoginPassword(true) && txtRegisterPayPassword(true) && txtConfirmRegisterPayPassword(true))) return false;
        if (document.getElementById("rdoIndividual").checked) { if (!(txtName(true) && txtIDCardValues(true) && txtPresonCellPhone(true))) return false; }
        if (document.getElementById("rdoEnterprise").checked) { if (!(txtCompanyName(true) && txtOrganizationCode(true) && txtCompanyPhone(true) && txtLegalPersonName(true) && txtCompanyCellPhone(true))) return false; }
        self.attr("disabled", "disabled");
        if (document.getElementById("rdoIndividual").checked) {
            var parameter =JSON.stringify({ "info": {
                "AccountNo": $.trim($("#txtRegisterAccountNo").val()),
                "LoginPassword": $.trim($("#txtLoginPassword").val()),
                "PayPassword": $.trim($("#txtRegisterPayPassword").val()),
                "AdministorName": $.trim($("#txtName").val()),
                "AdministorCertId": $.trim($("#txtIDCard").val()),
                "ContactPhone": $.trim($("#txtPresonCellPhone").val()),
                "IsVip":true
            }});
            sendPostRequest("/OrganizationHandlers/Account.ashx/AddPersonAccount", parameter, function (relust) {
                $(".confirmClose1,#registerSuccess").click();
                $("#lblRegisterNewAccount").text($("#txtRegisterAccountNo").val());
                self.removeAttr("disabled");
            }, function (e) { self.removeAttr("disabled"); alert(e.responseText); });
        } else {
            var parameter = JSON.stringify({ "info": {
                "AccountNo": $.trim($("#txtRegisterAccountNo").val()),
                "LoginPassword": $.trim($("#txtLoginPassword").val()),
                "PayPassword": $.trim($("#txtRegisterPayPassword").val()),
                "CompanyName": $.trim($("#txtCompanyName").val()),
                "OrganizationCode": $.trim($("#txtOrganizationCode").val()),
                "LegalContactPhone": $.trim($("#txtCompanyPhone").val()),
                "LegalPersonName": $.trim($("#txtLegalPersonName").val()),
                "ContactPhone": $.trim($("#txtCompanyCellPhone").val()),
                "IsVip": true
            }});
            sendPostRequest("/OrganizationHandlers/Account.ashx/AddCompanyAccount", parameter, function (relust) {
                $(".confirmClose1,#registerSuccess").click();
                $("#lblRegisterNewAccount").text($("#txtRegisterAccountNo").val());
                self.removeAttr("disabled");
            }, function (e) { self.removeAttr("disabled"); alert(e.responseText); });
        }
    });
    //关闭成功页面
    $("#btnExit,#exit").click(function () { window.location.reload(); });
    $("#btnExit1,#exit1").click(function () { window.location.reload(); });
});
function collectionReturnUrl(type) {
    returnUrl($.trim($("#lblCollectionAccountNo").text()), type);
}
function payReturnUrl(type) {
    returnUrl($.trim($("#lblPayAccountNo").text()), type);
}
function returnUrl(accountNo, type) {
    sendPostRequest("/OrganizationHandlers/ReturnUrl.ashx/GetUrl",
        JSON.stringify({ "accountNo": accountNo, "type": type }), function (retult) {
            if (retult == "") {
                window.location.href = "/Index.aspx";
            } else {
                window.top.location = retult;
            }
        },
        function (retult) {
            window.location.href = "/Index.aspx";
        });
}