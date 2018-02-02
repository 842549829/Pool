$(function () {
    /*账号*/var txtAccountNo = function (e) { return Validate(["EmptyOrObject=null", "Account=null"], "txtAccountNo", e); };
    /*密码*/var txtPassWord = function (e) { return Validate(["EmptyOrObject=密码不能为空", "PassWordRegex=null"], "txtPassWord", e) && ValidatePassword("txtAccountNo", "txtPassWord", "密码不能与账号一样", "!=", e); };
    /*确认密码*/var txtConfirmPassWord = function (e) { return Validate(["EmptyOrObject=密码不能为空", "PassWordRegex=确认密码格式错误"], "txtConfirmPassWord", e) && ValidatePassword("txtPassWord", "txtConfirmPassWord", "两次密码不一致", "=",e); };
    /*公司名称*/var txtCompanyName = function (e) { return Validate(["EmptyOrObject=公司名称不能为空", "CompanyName=null"], "txtCompanyName", e); };
    /*公司简称*/var txtCompanyShortName = function (e) { return Validate(["EmptyOrObject=公司简称不能为空", "CompanyName=公司简称格式错误"], "txtCompanyShortName", e); };
    /*所在地*/var hidAddress = function () { if ($("#hidAddress").val().length < 1) { $("#lblLocation").html("请选择所在地").focus(); return false; } else { $("#lblLocation").empty(); return true; } };
    /*公司地址*/var txtAddress = function (e) { return Validate(["EmptyOrObject=公司地址不能为空"], "txtAddress", e) && ValidateLength(["Max=公司地址最多50位字符"], "txtAddress", 50, e); };
    /*公司电话*/var txtCompanyPhone = function (e) { return Validate(["EmptyOrObject=公司电话不能为空", "Phone=公司电话格式错误"], "txtCompanyPhone", e) && ValidateLength(["Max=公司电话最多100个字符"], "txtCompanyPhone", 100, e); };
    /*负责人*/var txtPrincipal = function (e) { return Validate(["EmptyOrObject=负责人不能为空", "Name=负责人格式错误"], "txtPrincipal", e); };
    /*负责人手机*/var txtPrincipalPhone = function (e) { return Validate(["EmptyOrObject=负责人手机不能为空", "CellPhone=负责人手机格式错误"], "txtPrincipalPhone", e); };
    /*联系人*/var txtLinkman = function (e) { return Validate(["EmptyOrObject=联系人不能为空", "Name=联系人格式错误"], "txtLinkman", e); };
    /*联系人手机*/var txtLinkManPhone = function (e) { return Validate(["EmptyOrObject=联系人手机不能为空", "CellPhone=联系人手机格式错误"], "txtLinkManPhone", e); };
    /*紧急联系人*/var txtUrgencyLinkMan = function (e) { return Validate(["EmptyOrObject=紧急联系人不能为空", "Name=紧急联系人格式错误"], "txtUrgencyLinkMan", e); };
    /*紧急联系人手机*/var txtUrgencyLinkManPhone = function (e) { return Validate(["EmptyOrObject=紧急联系人手机不能为空", "CellPhone=紧急联系人手机格式错误"], "txtUrgencyLinkManPhone", e); };
    /*Email*/var txtEmail = function (e) { return Validate(["EmptyOrObject=Emali不能为空", "Email=Email格式错误"], "txtEmail", e) && ValidateLength(["Max=Email最多100位"],"txtEmail",100,e); };
    /*邮政编码*/var txtPostCode = function (e) { return Validate(["EmptyOrObject=邮政编码不能为空", "PostCode=邮政编码格式错误"], "txtPostCode", e); };
    /*传真*/var txtFaxes = function (e) { if ($("#txtFaxes").val().length > 0) { return Validate(["Phone=传真格式错误"], "txtFaxes", e) && ValidateLength(["Max=传真最多20个字符"], "txtFaxes",20,e); } else { $("#txtFaxes").message(""); return true; } };
    /*MSN*/var txtMSN = function (e) { if ($("#txtMSN").val().length > 0) { return Validate(["Email=MSN格式错误"], "txtMSN", e) && ValidateLength(["Max=MSN最多100位"], "txtMSN", 100, e); } else { $("#txtMSN").message(""); return true; } };
    /*QQ*/var txtQQ = function (e) { if ($("#txtQQ").val().length > 0) { return Validate(["QQ=QQ格式错误"], "txtQQ", e); } else { $("#txtQQ").message(""); return true; } };
    /*航协经营批准号*/var Licence = function (e) { return Validate(["EmptyOrObject=航协经营批准号不能为空", "Licence=航协经营批准号格式错误"], "txtIATABusinessApprovalNumber", e); };
    /*IATA号*/var txtIATANumber = function (e) { return Validate(["EmptyOrObject=IATA号不能为空", "Licence=IATA号格式错误"], "txtIATANumber", e); };
    /*OFFICE号*/var txtOFFICENumber = function (e) { return Validate(["EmptyOrObject=OFFICE号不能为空", "Offices=OFFICE号格式错误"], "txtOFFICENumber", e); };
    /*中航协担保金*/var Deposit = function (e) { return Validate(["EmptyOrObject=中航协担保金不能为空", "Licence=中航协担保金格式错误"], "txtCaticAssociationSuch", e); };
    /*验证码*/var txtCode = function (e) { return Validate(["EmptyOrObject=验证码不为空", "VerifyCode=验证码格式错误"], "txtCode", e); };

    $("#txtAccountNo").blur(function () { return txtAccountNo(); }).focus(function () { $(this).message("用户名只能用字母和数字，以字母开头，长度不少于6位"); });
    $("#txtPassWord").blur(function () { return txtPassWord(); }).focus(function () { $(this).message("密码长度6-20位，由英文字母a-z，数字0-9，特殊字符组成"); });
    $("#txtConfirmPassWord").blur(function () { return txtConfirmPassWord(); }).focus(function () { $(this).message("请您再输入一遍您上面输入的密码"); });
    $("#txtCompanyName").blur(function () { return txtCompanyName(); }).focus(function () { $(this).message("必须为汉字、英文字母、数字，不能包含非法字符"); });
    $("#txtCompanyShortName").blur(function () { return txtCompanyShortName(); }).focus(function () { $(this).message("必须为汉字、英文字母、数字，不能包含非法字符"); });
    $("#txtAddress").blur(function () { return txtAddress(); }).focus(function () { $(this).message("填写用于联系的通讯地址"); });
    $("#txtCompanyPhone").blur(function () { return txtCompanyPhone(); }).focus(function () { $(this).message("电话号码格式如：028-84254982"); });
    $("#txtPrincipal").blur(function (e) { return txtPrincipal(); }).focus(function () { $(this).message("填写负责人真实姓名，必须为汉字或者英文字母"); });
    $("#txtPrincipalPhone").blur(function () { return txtPrincipalPhone(); }).focus(function () { $(this).message("请正确填写负责人的手机号码，以便我们及时取得联系"); });
    $("#txtLinkman").blur(function () { return txtLinkman(); }).focus(function () { $(this).message("填写联系人真实姓名，必须为汉字或者英文字母"); });
    $("#txtLinkManPhone").blur(function () { return txtLinkManPhone(); }).focus(function () { $(this).message("请正确填写联系人的手机号码，以便我们及时取得联系"); });
    $("#txtUrgencyLinkMan").blur(function () { return txtUrgencyLinkMan(); }).focus(function () { $(this).message("填写紧急联系人真实姓名，必须为汉字或者英文字母"); });
    $("#txtUrgencyLinkManPhone").blur(function () { return txtUrgencyLinkManPhone(); }).focus(function () { $(this).message("请正确填写紧急联系人的手机号码，以便我们及时取得联系"); });
    $("#txtEmail").blur(function () { return txtEmail(); }).focus(function () { $(this).message("填写您常用的电子邮箱地址"); });
    $("#txtPostCode").blur(function () { return txtPostCode(); }).focus(function () { $(this).message("填写邮政编码"); });
    $("#txtFaxes").blur(function () { return txtFaxes(); });
    $("#txtMSN").blur(function () { return txtMSN(); });
    $("#txtQQ").blur(function () { return txtQQ(); });
    if ($("#txtIATABusinessApprovalNumber").length > 0) {/*如果为注册则验证*/
        $("#txtIATABusinessApprovalNumber").blur(function () { return Licence(); }).focus(function () { $(this).message(""); });
        $("#txtIATANumber").blur(function () { return txtIATANumber(); }).focus(function () { $(this).message(""); });
        $("#txtOFFICENumber").blur(function () { return txtOFFICENumber(); }).focus(function () { $(this).message(""); });
        $("#txtCaticAssociationSuch").blur(function () { return Deposit(); }).focus(function () { $(this).message(""); });
    }
    if ($("#rdolHowToKnow").length > 0) { $("#txtCode").blur(function () { return txtCode(); }); }
    $("#btnSubmit").click(function () {
        Address(); /*获取地址信息*/
        if (!(txtAccountNo(true) && txtPassWord(true) && txtConfirmPassWord(true) && txtCompanyName(true) && txtCompanyShortName(true) && hidAddress() && txtAddress(true) && txtCompanyPhone(true) && txtPrincipal(true) && txtPrincipalPhone(true) && txtLinkman(true) && txtLinkManPhone(true) && txtUrgencyLinkMan(true) && txtUrgencyLinkManPhone(true) && txtEmail(true) && txtPostCode(true) && txtFaxes(true) && txtMSN(true) && txtQQ(true))) { return false; }
        if ($("#txtIATABusinessApprovalNumber").length > 0 && !(Licence(true) && txtIATANumber(true) && txtOFFICENumber(true) && Deposit(true))) { /*如果为注册则验证*/return false; }
        if ($(":radio[name='rdolHowToKnow']").last().is(":checked") && !txtMarket()) { return false; }
        if ($("#rdolHowToKnow").length > 0 && !(txtCode())) { return false; }
        if ($("#chkAgreedToRead").length > 0) { return AgreedToRead(); } //验证是否阅读B3B协议
        return true;
    });
});