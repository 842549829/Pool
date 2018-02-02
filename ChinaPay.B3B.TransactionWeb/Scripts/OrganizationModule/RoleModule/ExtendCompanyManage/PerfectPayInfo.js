window.history.go(1);
$(function () {
    var target = {
        account: $("#txtAccountNo"),
        lblAccountNo: $("#lblAccountNo"),
        code: $("#txtOrganizationCode"),
        lblCode: $("#lblOrganizationCode"),
        IDCard: $("#txtIDCard"),
        lblIDCard: $("#lblIDCard"),
        btnCheCkAccounNo: $("#btnCheCkAccounNo"),
        LegalPersonName: $("#txtLegalPersonName"),
        lblLegalPersonName: $("#lblLegalPersonName"),
        LegalPersonPhone: $("#txtLegalPersonPhone"),
        lblLegalPersonPhone: $("#lblLegalPersonPhone")
    };
    //验证账号
    var AccountNo = function () {
        if (target.account.val().length < 1) {
            target.lblAccountNo.html("账号不能为空");
            return false;
        }
        var regAccount = /^[^<>]{6,30}$/;
        if (!regAccount.test(target.account.val())) {
            target.lblAccountNo.html("账号格式不正确");
            return false;
        }
        target.lblAccountNo.empty();
        return true;
    };
    //验证代码
    var Code = function (type) {
        if (target.code.val().length < 1) {
            target.lblCode.html("机构代码不能为空");
            if (type != null) { target.code.focus(); target.code.select(); }
            return false;
        }
        var reg = /^\d{8}-[\dXx]{1}$/;
        if (!reg.test(target.code.val())) {
            target.lblCode.html("机构代码格式不正确");
            if (type != null) { target.code.focus(); target.code.select(); }
            return false;
        }
        target.lblCode.empty();
        return true;
    };
    //验证身份证
    var IDCard = function (type) {
        if (target.IDCard.val().length < 1) {
            target.lblIDCard.html("身份证不能为空");
            if (type != null) { target.IDCard.focus(); target.IDCard.select(); }
            return false;
        }
        if (!isCardID(target.IDCard.val())) {
            target.lblIDCard.html("身份证格式不正确");
            if (type != null) { target.IDCard.focus(); target.IDCard.select(); }
            return false;
        }
        target.lblIDCard.empty();
        return true;
    };
    //验证法人姓名
    var LegalPersonName = function (type) {
        if (target.LegalPersonName.val().length < 1) {
            target.lblLegalPersonName.html("法人姓名不能为空");
            if (type != null) { target.LegalPersonName.select(); }
            return false;
        }
        var reg = /^[a-zA-z\u4e00-\u9fa5]{1,12}$/;
        if (!reg.test(target.LegalPersonName.val())) {
            target.lblLegalPersonName.html("法人姓名格式不正确");
            if (type != null) { target.lblLegalPersonName.select(); }
            return false;
        }
        target.lblLegalPersonName.empty();
        return true;
    }
    //验证法人电话号码
    var LegalPersonPhone = function (type) {
        if (target.LegalPersonPhone.val().length < 1) {
            target.lblLegalPersonPhone.html("法人电话不能为空");
            if (type != null) { target.LegalPersonPhone.select(); }
            return false;
        }
        var reg = /^(1[3458]\d{9})$|^(\d{7,8})$|^(\d{3,4}-\d{7,8})$|^(\d{3,4}-\d{7,8}-\d{1,4})$/;
        if (!reg.test(target.LegalPersonPhone.val())) {
            target.lblLegalPersonPhone.html("法人电话格式不正确");
            if (type != null) { target.LegalPersonPhone.select(); }
            return false;
        }
        target.lblLegalPersonPhone.empty();
        return true;
    }
    target.account.focus(function () { target.btnCheCkAccounNo.show(); target.lblAccountNo.empty(); })
    .blur(function () { if (!AccountNo()) { target.btnCheCkAccounNo.hide(); return false; } else { return true; } });
    var strRequest = GetQueryString();
    var bnlcode = false;
    //判断组织机构代码
    if (strRequest != undefined) {
        if (strRequest.value[0] != undefined && strRequest.value[0] == "Agent") {
            bnlcode = true;
        }
    }
    if (bnlcode) {
        target.code.blur(function () { return Code(null); });
        target.LegalPersonName.blur(function () { return LegalPersonName(null); });
        target.LegalPersonPhone.blur(function () { return LegalPersonPhone(null); })
    }
    target.IDCard.blur(function () { return IDCard(null); });
    $("#btnSave").click(function () {
        if (!AccountNo("type")) {
            target.btnCheCkAccounNo.hide();
            return false;
        }
        if (bnlcode) {
            return Code("type");
            return LegalPersonName("type");
            return LegalPersonPhone("type");
        }
        return IDCard("type");
    });
    //验证账户存在否
    target.btnCheCkAccounNo.click(function () {
        $(this).hide();
        if (!AccountNo(null)) {
            return false;
        }
        var paramenters = JSON.stringify({ "accountNo": $("#txtAccountNo").val() });
        sendPostRequest("/../../../../OrganizationHandlers/CheckUpComPanyNews.ashx/CheckPayAcccountNo", paramenters,
            function (e) {
                if (e == false) {
                    target.lblAccountNo.html("该账号可以使用");
                } else {
                    target.lblAccountNo.html("该账号已经存在");
                }
             },
            function (e) {
                $("#lblAccountNo").html(JSON.parse(e.responseText));
             }
        );
    });
});

function GetQueryString() {
    var key = new Array();
    var value = new Array();
    var url = document.location.search.replace("?","");
    var arr = url.split('&');
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split('=');
        if (ar[0] != '') {
            for (var j = 0; j < ar.length; j++) {
                if (j % 2 == 0) {
                    key.push(ar[j]);
                } else {
                    value.push(ar[j]);
                }
            }
        }
    }
    return { "key": key, "value": value };
}